using System;
using System.Collections.Generic;
using System.Linq;
using AtlasEngine.UserTasks;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake;
using Warehouse.Picking.Api.Services;
using Warehouse.Picking.Api.Utilities;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public class IntakeClientTaskPayloadHandler : IClientTaskPayloadHandler
    {
        private readonly IntakeService _intakeService;

        public IntakeClientTaskPayloadHandler(IntakeService intakeService)
        {
            _intakeService = intakeService;
        }

        public string CreateInfoPayload(UserTask task)
        {
            return task.GetQualifier() switch
            {
                "WrongArticleQuantity" => task.GetFormFieldValue<string>("Text"),
                _ => throw new ArgumentException("User task is not of Info type")
            };
        }

        public ScanPayload CreateScanPayload(UserTask task)
        {
            return task.GetQualifier() switch
            {
                "Article" => task.GetPayload<ScanPayload>(),
                "Location" => new ScanPayload(
                    new List<string>
                    {
                        task.GetPayload<BookLocationResult>().Barcode
                    }
                ),
                _ => new ScanPayload(new List<string>())
            };
        }

        public SelectionOptions CreateSelectionOptionsPayload(UserTask task)
        {
            switch (task.GetQualifier())
            {
                case "Bundle":
                {
                    var payload = task.GetPayload<NoteGtinPayload>()
                                  ?? throw new NullReferenceException("Payload is not of NoteGtinPayload type");
                    var options = _intakeService.GetAllBundlesByGtin(payload.Barcode).Result
                        .Select(bundle => new SelectionOption(bundle.Id, bundle.Name));
                    return new SelectionOptions(options);
                }
                default: throw new ArgumentException("User task is not of Info type");
            }
        }
    }

    public class NoteGtinPayload
    {
        public string NoteId { get; set; }
        public string Barcode { get; set; }
    }
}