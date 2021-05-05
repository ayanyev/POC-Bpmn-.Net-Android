namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MatchArticlePayload
    {
        public string NoteId { get; set; }

        public string Gtin { get; set; }

        public int BundleId { get; set; }
    }
}