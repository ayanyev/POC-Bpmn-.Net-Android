namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads
{
    public class StockyardLocationPayload
    {
        public string NoteId { get; set; }
        
        public int ArticleId { get; set; }
        
        public int Quantity { get; set; }

        public StockyardLocationPayload()
        {
        }
    }
}