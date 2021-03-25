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
        
        private readonly Dictionary<string, List<Article>> _noteArticles = new();

        public IntakeService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
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
                .Where(a => a.Quantity.Expected > a.Quantity.Processed)
                .Select(a => a.Gtin)
                .ToHashSet();
        }

        public Article UpdateArticleQuantity(string noteId, int articleId, int quantity)
        {
            var article = _noteArticles[noteId].Find(a => a.Id == articleId);
            article?.UpdateProcessedQuantity(quantity);
            return article;
        }
        
    }
}