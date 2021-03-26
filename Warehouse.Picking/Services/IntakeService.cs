using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Repositories;

namespace Warehouse.Picking.Api.Services
{
    public class IntakeService
    {
        private readonly IArticleRepository _articleRepository;

        private readonly ILocationRepository _locationRepository;

        private readonly Dictionary<string, List<Article>> _noteArticles = new();

        public IntakeService(IArticleRepository articleRepository, ILocationRepository locationRepository)
        {
            _articleRepository = articleRepository;
            _locationRepository = locationRepository;
        }

        public async Task<bool> FetchArticlesForDeliveryNote(string noteId)
        {
            var articles = await _articleRepository.Fetch(noteId);
            _noteArticles.Add(noteId, articles);
            return true;
        }

        public HashSet<string> GetBarcodesForUnfinishedArticles(string noteId)
        {
            return _noteArticles[noteId]
                .Where(a => a.IsUnfinished())
                .Select(a => a.Gtin)
                .ToHashSet();
        }

        public Article UpdateArticleQuantity(string noteId, int articleId, int quantity)
        {
            var article = _noteArticles[noteId].Find(a => a.Id.Equals(articleId));
            article?.UpdateProcessedQuantity(quantity);
            return article;
        }

        private List<Article> GetUnfinishedArticlesByGtin(string noteId, string gtin)
        {
            return _noteArticles[noteId]
                .Where(a => a.Gtin == gtin && a.IsUnfinished())
                .ToList();
        }

        public List<ArticleBundle> GetBundlesForUnfinishedArticlesByGtin(string noteId, string gtin)
        {
            return GetUnfinishedArticlesByGtin(noteId, gtin)
                .Select(a => a.Bundle)
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
    }
}