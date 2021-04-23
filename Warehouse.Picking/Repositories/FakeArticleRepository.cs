using System.Collections.Generic;
using System.Threading.Tasks;
using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Repositories
{
    public class FakeArticleRepository : IArticleRepository
    {
        private static readonly List<Article> Note1Articles = new()
        {
            new(
                111, "noteId", "Pepsi Cola Maxx", "567.456.7.9",
                new ArticleQuantity(20),
                new ArticleBundle(1, "6x1.5L")),
            new(
                222, "noteId", "Pepsi Cola Maxx", "567.456.7.9",
                new ArticleQuantity(30),
                new ArticleBundle(2, "12x1.5L")),
            new(
                333, "noteId", "Gerolsteiner Naturell", "234.147.2.8",
                new ArticleQuantity(25),
                new ArticleBundle(3, "12x1.0L Glass")),
            new(
                444, "noteId", "Bauer Apfelsaft", "345.128.5.0",
                new ArticleQuantity(40),
                new ArticleBundle(4, "6x1.5L")),
            new(
                555, "noteId", "Gerolsteiner Mild", "195.247.2.7",
                new ArticleQuantity(25),
                new ArticleBundle(3, "12x1.0L Glass")),
            new(
                666, "noteId", "Bauer Orangensaft", "567.135.7.9",
                new ArticleQuantity(40),
                new ArticleBundle(5, "24x0.2L"))
        };

        private readonly Dictionary<string, List<Article>> _noteArticles = new() {{"note1", Note1Articles}};

        public Task<List<Article>> FetchByNoteId(string noteId)
        {
            return Task.FromResult(FindByNoteId(noteId));
        }

        public List<Article> FindByNoteId(string noteId)
        {
            return _noteArticles[noteId];
        }
    }
}