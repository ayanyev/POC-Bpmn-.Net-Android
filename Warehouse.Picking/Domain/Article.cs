using System;
using System.Collections.Generic;

namespace warehouse.picking.api.Domain
{
    [Serializable]
    public class Article
    {
        public int Id { get; set; }
        
        public string NoteId { get; set; }
        
        public string Name { get; set; }

        public string Gtin { get; set; }
        
        public bool IsSuspended { get; set; }

        public ArticleQuantity Quantity { get; set; }

        public ArticleBundle Bundle { get; set; }

        public void UpdateProcessedQuantity(int q)
        {
            Quantity.Processed += q;
        }

        public bool IsUnfinished => Quantity.Processed < Quantity.Expected;

        public Article(int id, string noteId, string name, string gtin, ArticleQuantity quantity, ArticleBundle bundle, bool isSuspended)
        {
            Id = id;
            NoteId = noteId;
            Name = name;
            Gtin = gtin;
            Quantity = quantity;
            Bundle = bundle;
            IsSuspended = isSuspended;
        }
    }

    [Serializable]
    public class Articles
    {
        public Articles(List<Article> articles)
        {
            Items = articles;
        }

        public List<Article> Items { get; }
        
    }
}