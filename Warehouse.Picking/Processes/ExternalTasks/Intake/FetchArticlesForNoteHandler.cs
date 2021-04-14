using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads;
using Warehouse.Picking.Api.Services;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake
{
    [ExternalTaskHandler(topic: "Intake.Note.FetchArticles")]
    public class FetchArticlesForNoteHandler : IExternalTaskHandler<NoteIdPayload, SuccessResult>
    {
        private readonly IntakeService _service;

        public FetchArticlesForNoteHandler(IntakeService service)
        {
            _service = service;
        }

        public Task<SuccessResult> HandleAsync(NoteIdPayload input, ExternalTask task)
        {
            var r = _service.FetchArticlesForDeliveryNote(task.CorrelationId, input.NoteId);
            return Task.FromResult(new SuccessResult());
        }
    }

    public class SuccessResult
    {
        public SuccessResult()
        {
        }
    }
    
}