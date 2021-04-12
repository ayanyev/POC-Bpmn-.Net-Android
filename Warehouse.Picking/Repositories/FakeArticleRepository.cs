using System.Collections.Generic;
using System.Threading.Tasks;
using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Repositories
{
    public class FakeArticleRepository : IArticleRepository
    {
        public Task<List<Article>> GetByNoteId(string noteId)
        {
            var a1 = new Article(
                111, "note1", "Pepsi Cola Maxx", "567.456.7.9",
                new ArticleQuantity(20),
                new ArticleBundle(1, "6x1.5L"));

            var a2 = new Article(
                222, "note1", "Pepsi Cola Maxx", "567.456.7.9",
                new ArticleQuantity(30),
                new ArticleBundle(2, "12x1.5L"));

            var a3 = new Article(
                333, "note1", "Gerolsteiner Naturell", "234.147.2.8",
                new ArticleQuantity(25),
                new ArticleBundle(3, "12x1.0L Glass"));

            var a4 = new Article(
                444, "note1", "Bauer Apfelsaft", "345.128.5.0",
                new ArticleQuantity(40),
                new ArticleBundle(4, "6x1.5L"));

            var a5 = new Article(
                555, "note1", "Gerolsteiner Mild", "195.247.2.7",
                new ArticleQuantity(25),
                new ArticleBundle(3, "12x1.0L Glass"));

            var a6 = new Article(
                666, "note1", "Bauer Orangensaft", "567.135.7.9",
                new ArticleQuantity(40),
                new ArticleBundle(5, "24x0.2L"));

            return Task.FromResult(
                new List<Article> {a1, a2, a3, a4, a5, a6}
            );
        }
    }
}