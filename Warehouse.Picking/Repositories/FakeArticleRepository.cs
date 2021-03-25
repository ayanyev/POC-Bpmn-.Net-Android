using System.Collections.Generic;
using System.Threading.Tasks;
using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Repositories
{
    public class FakeArticleRepository : IArticleRepository
    {
        public Task<List<Article>> Fetch(string noteId)
        {
            var a1 = new Article(
                111, "note1", "Pepsi Cola Maxx", "1234567",
                new ArticleQuantity(20),
                new ArticleBundle(1, "6x1.5L"));

            var a2 = new Article(
                111, "note1", "Pepsi Cola Maxx", "1234567",
                new ArticleQuantity(30),
                new ArticleBundle(2, "12x1.0L"));

            var a3 = new Article(
                111, "note1", "Gerolsteiner Naturell", "56786787",
                new ArticleQuantity(25),
                new ArticleBundle(3, "12x1.0L Glass"));

            var a4 = new Article(
                111, "note1", "Bauer Apfelsaft", "345t4656",
                new ArticleQuantity(40),
                new ArticleBundle(4, "6x1.5L"));

            var a5 = new Article(
                111, "note1", "Gerolsteiner Mild", "67876576",
                new ArticleQuantity(25),
                new ArticleBundle(3, "12x1.0L Glass"));

            var a6 = new Article(
                111, "note1", "Bauer Orangensaft", "34674567876",
                new ArticleQuantity(40),
                new ArticleBundle(4, "24x0.2L"));

            return Task.FromResult(
                new List<Article> {a1, a2, a3, a4, a5, a6}
            );
        }
    }
}