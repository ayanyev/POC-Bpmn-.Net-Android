using System;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads
{
    [Serializable]
    public class NoteIdPayload
    {
        public string NoteId { get; set; }
        
    }
}