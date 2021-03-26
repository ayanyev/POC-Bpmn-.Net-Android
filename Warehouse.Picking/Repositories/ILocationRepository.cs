using System.Threading.Tasks;

namespace Warehouse.Picking.Api.Repositories
{
    public interface ILocationRepository
    {
        Task<string> BookLocation(string noteId, int articleId, int quantity);
        
    }
}