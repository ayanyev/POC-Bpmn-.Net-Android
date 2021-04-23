using System;
using System.Linq;
using System.Text.Json;
using AtlasEngine.UserTasks;
using Warehouse.Picking.Api.Services;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public class IntakeUserTaskPayloadFactory : IUserTaskPayloadFactory
    {
        private readonly IntakeService _intakeService;

        public IntakeUserTaskPayloadFactory(IntakeService intakeService)
        {
            _intakeService = intakeService;
        }

        public SelectionOptions CreateSelectionOptionsPayload(UserTask task)
        {
            var rawPayload = task.Tokens.Find(t => t.Type == TokenType.OnEnter)?.Payload?.RawPayload
                             ?? throw new NullReferenceException("Payload of OnEnter type not found");
            
            var payload = JsonSerializer.Deserialize<NoteGtinPayload>(rawPayload)
                          ?? throw new NullReferenceException("Payload is not of NoteGtinPayload type");

            var options = _intakeService
                .GetBundlesForUnfinishedArticlesByGtin(payload.NoteId, payload.Barcode)
                .Select(bundle => new SelectionOption(bundle.Id, bundle.Name));

            return new SelectionOptions(options);
        }
    }

    [Serializable]
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class NoteGtinPayload
    {
        public string NoteId { get; }
        public string Barcode { get; }

        public NoteGtinPayload(string barcode, string noteId)
        {
            Barcode = barcode;
            NoteId = noteId;
        }
    }
}