using System.Collections.Generic;
using System.Threading.Tasks;
using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Repositories
{
    public class FakeArticleRepository : IArticleRepository
    {
        private readonly Dictionary<string, List<Article>> _noteArticles = new();

        public Task<List<Article>> FetchByNoteId(string noteId)
        {
            var result = new List<Article>
            {
                new(
                    111, "note1", "Pepsi Cola Maxx", "567.456.7.9",
                    new ArticleQuantity(20),
                    new ArticleBundle(1, "6x1.5L")),
                new(
                    222, "note1", "Pepsi Cola Maxx", "567.456.7.9",
                    new ArticleQuantity(30),
                    new ArticleBundle(2, "12x1.5L")),
                new(
                    333, "note1", "Gerolsteiner Naturell", "234.147.2.8",
                    new ArticleQuantity(25),
                    new ArticleBundle(3, "12x1.0L Glass")),
                new(
                    444, "note1", "Bauer Apfelsaft", "345.128.5.0",
                    new ArticleQuantity(40),
                    new ArticleBundle(4, "6x1.5L")),
                new(
                    555, "note1", "Gerolsteiner Mild", "195.247.2.7",
                    new ArticleQuantity(25),
                    new ArticleBundle(3, "12x1.0L Glass")),
                new(
                    666, "note1", "Bauer Orangensaft", "567.135.7.9",
                    new ArticleQuantity(40),
                    new ArticleBundle(5, "24x0.2L"))
            };
            _noteArticles.Add(noteId, result);
            return Task.FromResult(result);
        }

        public List<Article> FindByNoteId(string noteId)
        {
            return _noteArticles[noteId];
        }
    }
}   