using System;
using System.Linq;
using AtlasEngine.UserTasks;
using Microsoft.Extensions.Logging;
using Warehouse.Picking.Api.Services;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public class IntakeUserTaskPayloadFactory : IUserTaskPayloadFactory
    {
        private readonly IntakeService _intakeService;

        public IntakeUserTaskPayloadFactory(IntakeService intakeService, ILogger<IntakeUserTaskPayloadFactory> logger)
        {
            _intakeService = intakeService;
        }

        public SelectionOptions CreateSelectionOptionsPayload(UserTask task)
        {
            var payload = task.Tokens.Find(t => t.Type == TokenType.OnEnter)?.Payload?.GetPayload<NoteGtinPayload>()
                          ?? throw new NullReferenceException("Payload is not of NoteGtinPayload type");
            
            var options = _intakeService
                .GetBundlesForUnfinishedArticlesByGtin(payload.NoteId, payload.Barcode)
                .Select(bundle => new SelectionOption(bundle.Id, bundle.Name));

            return new SelectionOptions(options);
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class NoteGtinPayload
    {
        public string Barcode { get; }
        public string NoteId { get; }

        public NoteGtinPayload(string barcode, string noteId)
        {
            Barcode = barcode;
            NoteId = "note1";
        }
    }
}