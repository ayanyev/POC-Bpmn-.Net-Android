using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using warehouse.picking.api.Domain;
using warehouse.picking.api.Hubs;
using Warehouse.Picking.Api.Repositories;

namespace Warehouse.Picking.Api.Services
{
    public class IntakeService
    {
        private readonly ConnectionMapping _connectionMapping;

        private readonly IArticleRepository _articleRepository;

        private readonly ILocationRepository _locationRepository;

        private readonly IHubContext<DeviceHub> _intakeDeviceHubContext;

        private readonly IHubContext<IntakeDashboardHub> _intakeDashboardHubContext;


        public IntakeService(IArticleRepository articleRepository, ILocationRepository locationRepository,
            IHubContext<DeviceHub> intakeDeviceHubContext,
            IHubContext<IntakeDashboardHub> intakeDashboardHubContext, ConnectionMapping connectionMapping)
        {
            _articleRepository = articleRepository;
            _locationRepository = locationRepository;
            _intakeDeviceHubContext = intakeDeviceHubContext;
            _intakeDashboardHubContext = intakeDashboardHubContext;
            _connectionMapping = connectionMapping;
        }

        public async Task<bool> FetchArticlesForDeliveryNote(string correlationId, string noteId)
        {
            var articles = await _articleRepository.FetchByNoteId(noteId);
            await _intakeDeviceHubContext.Clients
                .Client(_connectionMapping.GetConnection(correlationId))
                .SendAsync("ArticlesListReceived", new Articles(articles));
            await _intakeDashboardHubContext.Clients
                .Group("Dashboard")
                .SendAsync("DeliveryArticles", articles.Select(a => new SimplifiedArticle(a)));
            return true;
        }

        public HashSet<string> GetBarcodesForUnfinishedArticles(string noteId)
        {
            return _articleRepository.FindByNoteId(noteId)
                .Where(a => a.IsUnfinished())
                .Select(a => a.Gtin)
                .ToHashSet();
        }

        public async Task<Article> UpdateArticleQuantity(string correlationId, string noteId, int articleId,
            int quantity)
        {
            var articles = _articleRepository.FindByNoteId(noteId);
            var updatedArticle = articles.Find(a => a.Id.Equals(articleId));

            updatedArticle?.UpdateProcessedQuantity(quantity);

            await _intakeDeviceHubContext.Clients
                .Client(_connectionMapping.GetConnection(correlationId))
                .SendAsync("ArticlesListReceived", new Articles(articles));

            await _intakeDashboardHubContext.Clients
                .Group("Dashboard")
                .SendAsync("DeliveryArticles", articles.Select(a => new SimplifiedArticle(a)));

            return updatedArticle;
        }

        private List<Article> GetUnfinishedArticlesByGtin(string noteId, string gtin)
        {
            return _articleRepository.FindByNoteId(noteId)
                .Where(a => a.Gtin == gtin && a.IsUnfinished())
                .ToList();
        }

        public Article MatchUnfinishedArticleByGtinAndBundle(string noteId, string gtin, int bundleId)
        {
            return GetUnfinishedArticlesByGtin(noteId, gtin)
                .Find(a => a.Gtin == gtin && a.Bundle.Id == bundleId);
        }

        public async Task<string> BookStockYardLocation(string noteId, int articleId, int quantity)
        {
            return await _locationRepository.BookLocation(noteId, articleId, quantity);
        }

        public async Task<List<ArticleBundle>> GetAllBundlesByGtin(string gtin)
        {
            return await _articleRepository.FetchKnownBundlesByGtin(gtin);
        }
    }
}