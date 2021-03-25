using System.Collections.Generic;

namespace warehouse.picking.api.Domain
{
    public class DeliveryNote
    {
        public string Id { get; set; }

        public List<Article> Articles { get; set; }

    }
}