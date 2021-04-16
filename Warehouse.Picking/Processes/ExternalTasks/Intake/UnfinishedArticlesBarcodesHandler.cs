using System.Collections.Generic;
using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads;
using Warehouse.Picking.Api.Services;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake
{
    [ExternalTaskHandler(topic: "Intake.Note.Articles.Unfinished.Barcodes")]
    public class UnfinishedArticlesBarcodesHandler : IExternalTaskHandler<NoteIdPayload, UnfinishedBarcodes>
    {
        private readonly IntakeService _service;

        public UnfinishedArticlesBarcodesHandler(IntakeService service)
        {
            _service = service;
        }

        public Task<UnfinishedBarcodes> HandleAsync(NoteIdPayload input, ExternalTask task)
        {
            var barcodes = _service.GetBarcodesForUnfinishedArticles(input.NoteId);
            return Task.FromResult(new UnfinishedBarcodes(barcodes));
        }
    }
    
    public class UnfinishedBarcodes
    {
        public HashSet<string> Barcodes { get; }
        public UnfinishedBarcodes(HashSet<string> barcodes)
        {
            Barcodes = barcodes;
        }
    }

}