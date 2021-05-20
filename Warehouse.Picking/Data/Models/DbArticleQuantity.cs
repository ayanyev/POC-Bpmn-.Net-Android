using Microsoft.EntityFrameworkCore;

namespace Warehouse.Picking.Api.Data.Models
{
    [Owned]
    public class DbArticleQuantity
    {
        public int Expected { get; set; }

        public int Processed { get; set; }
    }
}