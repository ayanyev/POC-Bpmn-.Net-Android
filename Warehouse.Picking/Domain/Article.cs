using System.Collections.Generic;

namespace warehouse.picking.api.Domain
{
    public class Article
    {
        public string Id { get; set; }
        
        public string NoteId { get; set; }

        public string Ean { get; set; }

        public ArticleQuantity Quantity { get; set; }

        public HashSet<ArticleBundle> Bundles { get; set; }

        public void UpdateProcessedQuantity(int q)
        {
            Quantity.Processed = q;
        }
        
    }
}