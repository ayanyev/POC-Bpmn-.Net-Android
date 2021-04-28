namespace Warehouse.Picking.Api.Processes
{
    public interface IProcessInfoProvider
    {
        public ProcessInfo Get(string name);
    }

    public class ProcessInfo
    {
        public string ModelId { get; }
        
        public string StartEvent { get; }

        public ProcessInfo(string modelId, string startEvent)
        {
            ModelId = modelId;
            StartEvent = startEvent;
        }
    }
}