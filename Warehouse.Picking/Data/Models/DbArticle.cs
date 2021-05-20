using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Picking.Api.Data.Models
{
    [Table("articles")]
    public class DbArticle
    {
        public int Id { get; set; }
        public string NoteId { get; set; }

        public string Name { get; set; }

        public string Gtin { get; set; }

        public bool IsSuspended { get; set; }

        public DbArticleQuantity Quantity { get; set; }

        public DbArticleBundle Bundle { get; set; }
    }
}