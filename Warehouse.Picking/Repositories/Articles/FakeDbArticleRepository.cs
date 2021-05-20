using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Warehouse.Picking.Api.Data;
using Warehouse.Picking.Api.Data.Mappers;
using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Repositories.Articles
{
    public class FakeDbArticleRepository : IArticleRepository
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        public FakeDbArticleRepository(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public Task<List<Article>> FetchByNoteId(string noteId)
        {
            var articles = FindByNoteId(noteId);
            if (articles.Count != 0) return Task.FromResult(articles);
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Articles.AddRange(FakeData.Note1Articles.Select(a => a.ToDbModel()));
                context.SaveChanges();
            }
            return Task.FromResult(FindByNoteId(noteId));
        }

        public List<Article> FindByNoteId(string noteId)
        {
            return _dbContextFactory.CreateDbContext().Articles
                .Where(a => a.NoteId.Equals(noteId))
                .Select(a => a.ToDomainModel())
                .ToList();
        }

        public Task<List<ArticleBundle>> FetchKnownBundlesByGtin(string gtin)
        {
            return _dbContextFactory.CreateDbContext().Articles
                .Where(a => a.Gtin.Equals(gtin))
                .Select(a => a.Bundle.ToDomainModel())
                .ToListAsync();
        }

        public Task<Article> UpdateArticle(string noteId, int articleId, int quantity)
        {
            Article article;
            using (var context = _dbContextFactory.CreateDbContext())
            {
                article = context.Articles.AsNoTracking()
                    .Single(a => a.NoteId.Equals(noteId) && a.Id.Equals(articleId))
                    .ToDomainModel();

                if (article == null)
                {
                    throw new NoNullAllowedException(
                        $"Article not found for given note and article ids ({noteId}, {articleId})");
                }

                if (article.Quantity.Expected >= article.Quantity.Processed + quantity)
                {
                    article.UpdateProcessedQuantity(quantity);
                }

                if (article.Quantity.Expected < article.Quantity.Processed + quantity)
                {
                    article.IsSuspended = true;
                }

                context.Articles.Update(article.ToDbModel());
                context.SaveChanges();
            }
            return Task.FromResult(article);
        }
    }
}