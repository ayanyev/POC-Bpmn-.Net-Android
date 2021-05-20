using Microsoft.EntityFrameworkCore;

namespace Warehouse.Picking.Api.Data.Models
{
    [Owned]
    public class DbArticleBundle
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}