using System.Collections.Generic;
using System.Threading.Tasks;
using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Repositories
{
    public interface IArticleRepository
    {
        Task<List<Article>> FetchByNoteId(string noteId);
        
        List<Article> FindByNoteId(string noteId);
        
    }
}