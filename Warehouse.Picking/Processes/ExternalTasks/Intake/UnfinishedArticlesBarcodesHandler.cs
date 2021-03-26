using System.Collections.Generic;
using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads;
using Warehouse.Picking.Api.Services;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake
{
    [ExternalTaskHandler(topic: "Intake.Note.Articles.Unfinished.Barcodes")]
    public class UnfinishedArticlesBarcodesHandler : IExternalTaskHandler<NoteIdPayload, HashSet<string>>
    {
        private readonly IntakeService _service;

        public UnfinishedArticlesBarcodesHandler(IntakeService service)
        {
            _service = service;
        }

        public Task<HashSet<string>> HandleAsync(NoteIdPayload input, ExternalTask task)
        {
            return Task.FromResult(_service.GetBarcodesForUnfinishedArticles(input.NoteId));
        }
    }

}