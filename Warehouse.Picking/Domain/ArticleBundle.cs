using System;

namespace warehouse.picking.api.Domain
{
    [Serializable]
    public class ArticleBundle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ArticleBundle(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public ArticleBundle()
        {
        }
    }
}