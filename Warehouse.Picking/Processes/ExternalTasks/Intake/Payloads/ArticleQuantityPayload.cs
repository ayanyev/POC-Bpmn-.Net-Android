namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ArticleQuantityPayload
    {
        public string NoteId { get; set; }
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
    }
}