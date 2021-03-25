namespace warehouse.picking.api.Domain
{
    public class ArticleBundle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ArticleBundle(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}