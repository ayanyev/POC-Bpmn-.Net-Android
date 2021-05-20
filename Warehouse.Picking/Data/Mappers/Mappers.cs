using Warehouse.Picking.Api.Data.Models;
using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Data.Mappers
{
    public static class ToDbModelMappers
    {
        internal static DbArticleQuantity ToDbModel(this ArticleQuantity domain)
            => new() {Expected = domain.Expected, Processed = domain.Processed};

        internal static DbArticleBundle ToDbModel(this ArticleBundle domain)
            => new() {Id = domain.Id, Name = domain.Name};

        internal static DbArticle ToDbModel(this Article domain)
            => new()
            {
                Id = domain.Id, NoteId = domain.NoteId, Name = domain.Name, Gtin = domain.Gtin,
                IsSuspended = domain.IsSuspended, Quantity = domain.Quantity.ToDbModel(), Bundle =
                    domain.Bundle.ToDbModel()
            };
    }

    public static class ToDomainModelMappers
    {
        internal static ArticleQuantity ToDomainModel(this DbArticleQuantity domain)
            => new() {Expected = domain.Expected, Processed = domain.Processed};

        internal static ArticleBundle ToDomainModel(this DbArticleBundle domain)
            => new() {Id = domain.Id, Name = domain.Name};

        internal static Article ToDomainModel(this DbArticle domain)
            => new()
            {
                Id = domain.Id, NoteId = domain.NoteId, Name = domain.Name, Gtin = domain.Gtin,
                IsSuspended = domain.IsSuspended, Quantity = domain.Quantity.ToDomainModel(), Bundle =
                    domain.Bundle.ToDomainModel()
            };
    }
}