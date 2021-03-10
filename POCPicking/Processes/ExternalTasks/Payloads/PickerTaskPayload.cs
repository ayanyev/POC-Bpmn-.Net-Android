using POCPicking.Models;

namespace POCPicking.Processes.ExternalTasks.Payloads
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PickerTaskPayload
    {
        public Picker Picker { get; set; }
        
        public PickerTask Task { get; set; }

        public PickerTaskPayload()
        {
        }

        public PickerTaskPayload(Picker picker, PickerTask task)
        {
            Picker = picker;
            Task = task;
        }
    }
}