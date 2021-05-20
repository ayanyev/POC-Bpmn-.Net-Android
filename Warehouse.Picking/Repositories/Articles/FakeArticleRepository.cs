using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Repositories.Articles
{
    public class FakeArticleRepository : IArticleRepository
    {
        private readonly Dictionary<string, List<Article>> _noteArticles = new() {{"note1", FakeData.Note1Articles}};

        public Task<List<Article>> FetchByNoteId(string noteId)
        {
            return Task.FromResult(FindByNoteId(noteId));
        }

        public List<Article> FindByNoteId(string noteId)
        {
            return _noteArticles[noteId];
        }

        public Task<List<ArticleBundle>> FetchKnownBundlesByGtin(string gtin)
        {
            var result = FakeData.Note1Articles.FindAll(a => a.Gtin.Equals(gtin))
                .Select(a => a.Bundle).ToList();
            result.Add(new ArticleBundle(999, "48x0.2L"));
            return Task.FromResult(result);
        }

        public Task<Article> UpdateArticle(string noteId, int articleId, int quantity)
        {
            // update article info on server side

            var articles = FindByNoteId(noteId);
            var article = articles.Find(a => a.Id.Equals(articleId));

            if (article == null)
            {
                throw new NoNullAllowedException(
                    $"Article not found for given note and article ids ({noteId}, {articleId})");
            }

            if (article.Quantity.Expected >= article.Quantity.Processed + quantity)
            {
                article.UpdateProcessedQuantity(quantity);
            }

            if (article.Quantity.Expected < article.Quantity.Processed + quantity)
            {
                article.IsSuspended = true;
            }

            return Task.FromResult(article);
        }
    }
}