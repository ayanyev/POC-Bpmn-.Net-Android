using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads;
using Warehouse.Picking.Api.Services;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake
{
    [ExternalTaskHandler(topic: "Intake.Article.Update")]
    public class UpdateArticleHandler : IExternalTaskHandler<StockyardLocationPayload, Article>
    {
        private readonly IntakeService _service;

        public UpdateArticleHandler(IntakeService service)
        {
            _service = service;
        }

        public Task<Article> HandleAsync(StockyardLocationPayload input, ExternalTask task)
        {
            return _service.UpdateArticleQuantity(task.CorrelationId, input.NoteId, input.ArticleId, input.Quantity);;
        }
    }

}