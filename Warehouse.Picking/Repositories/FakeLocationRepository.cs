using System.Threading.Tasks;

namespace Warehouse.Picking.Api.Repositories
{
    public class FakeLocationRepository : ILocationRepository
    {
        public Task<string> BookLocation(string noteId, int articleId, int quantity)
        {
            return Task.FromResult("123.456.7.8");
        }
    }
}