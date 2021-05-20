using Microsoft.EntityFrameworkCore;
using Warehouse.Picking.Api.Data.Models;

namespace Warehouse.Picking.Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<DbArticle> Articles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlite("Data source=./Data/application.db")
                .EnableSensitiveDataLogging();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbArticle>(
                entity =>
                {
                    entity.HasKey(c => new {c.Id, c.NoteId});
                    entity.OwnsOne(a => a.Quantity);
                    entity.OwnsOne(a => a.Bundle);
                }
            );
        }
        
        
    }
}