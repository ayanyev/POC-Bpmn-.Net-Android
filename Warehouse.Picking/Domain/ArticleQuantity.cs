namespace warehouse.picking.api.Domain
{
    public class ArticleQuantity
    {
        public int Expected { get; set; }

        public int Processed { get; set; }

        public ArticleQuantity(int expected)
        {
            Expected = expected;
            Processed = 0;
        }
    }
}