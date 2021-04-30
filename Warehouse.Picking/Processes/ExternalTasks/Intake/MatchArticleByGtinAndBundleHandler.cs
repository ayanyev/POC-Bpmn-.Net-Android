using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads;
using Warehouse.Picking.Api.Services;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake
{
    [ExternalTaskHandler(topic: "Intake.Article.Match")]
    public class MatchArticleByGtinAndBundleHandler : IExternalTaskHandler<MatchArticlePayload, Article>
    {
        private readonly IntakeService _service;

        public MatchArticleByGtinAndBundleHandler(IntakeService service)
        {
            _service = service;
        }

        public Task<Article> HandleAsync(MatchArticlePayload input, ExternalTask task)
        {
            var article = _service.MatchUnfinishedArticleByGtinAndBundle(input.NoteId, input.Gtin, input.BundleId);
            return article != null
                ? Task.FromResult(article)
                : Task.FromException<Article>(
                    new BundleNotPresentInDelivery(input.NoteId, input.BundleId, null)
                );
        }
    }
}