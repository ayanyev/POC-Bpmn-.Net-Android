using System;
using System.Collections.Generic;
using System.Linq;
using AtlasEngine.UserTasks;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake;
using Warehouse.Picking.Api.Services;
using Warehouse.Picking.Api.Utilities;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public class IntakeUserTaskPayloadFactory : IUserTaskPayloadFactory
    {
        private readonly IntakeService _intakeService;

        public IntakeUserTaskPayloadFactory(IntakeService intakeService)
        {
            _intakeService = intakeService;
        }

        public ScanPayload CreateScanPayload(UserTask task)
        {
            return task.Id switch
            {
                "UT.Input.Scan.Barcode.Article" => task.GetPayload<ScanPayload>(),
                "UT.Input.Scan.Barcode.Location" => new ScanPayload(
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
            var payload = task.GetPayload<NoteGtinPayload>()
                          ?? throw new NullReferenceException("Payload is not of NoteGtinPayload type");

            var options = _intakeService.GetAllBundlesByGtin(payload.Barcode).Result
                .Select(bundle => new SelectionOption(bundle.Id, bundle.Name));

            return new SelectionOptions(options);
        }
    }

    public class NoteGtinPayload
    {
        public string NoteId { get; set; }
        public string Barcode { get; set; }
    }
}