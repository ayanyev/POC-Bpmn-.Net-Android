namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads
{
    public class MatchArticlePayload
    {
        public string NoteId { get; set; }
        
        public string Gtin { get; set; }
        
        public int BundleId { get; set; }

        public MatchArticlePayload()
        {
        }
    }
}