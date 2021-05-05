using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads;
using Warehouse.Picking.Api.Services;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake
{
    [ExternalTaskHandler(topic: "ST.Intake.Articles.Match.Barcode.Bundle")]
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

            string faultReason = null;

            if (article == null)
            {
                faultReason = "absent in delivery";
            }
            else if (article.IsSuspended)
            {
                faultReason = "suspended";
            }
            else if (!article.IsUnfinished)
            {
                faultReason = "completed";
            }

            if (faultReason != null)
            {
                return Task.FromException<Article>(
                    new SelectedBundleNotAvailable(input.NoteId, input.BundleId, faultReason)
                );
            }

            return Task.FromResult(article);
        }
    }
}