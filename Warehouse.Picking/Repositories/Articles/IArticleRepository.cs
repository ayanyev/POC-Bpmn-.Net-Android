using System.Collections.Generic;
using System.Threading.Tasks;
using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Repositories.Articles
{
    public interface IArticleRepository
    {
        Task<List<Article>> FetchByNoteId(string noteId);
        
        List<Article> FindByNoteId(string noteId);
        
        Task<List<ArticleBundle>> FetchKnownBundlesByGtin(string gtin);
        
        Task<Article> UpdateArticle(string noteId, int articleId, int quantity);
        
    }
}