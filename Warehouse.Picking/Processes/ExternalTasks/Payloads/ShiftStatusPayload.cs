using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Payloads
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ShiftStatusPayload
    {
        public Picker Picker { get; set; }
        
        public bool Status { get; set; }

        public ShiftStatusPayload()
        {
        }

        public ShiftStatusPayload(Picker picker, bool status)
        {
            Picker = picker;
            Status = status;
        }
    }
}