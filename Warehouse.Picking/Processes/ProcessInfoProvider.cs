namespace Warehouse.Picking.Api.Processes
{
    public class ProcessInfoProvider : IProcessInfoProvider
    {
        public ProcessInfo Get(string name)
        {
            return name switch
            {
                "Max" => new ProcessInfo("intake", ""),
                "Jorg" => new ProcessInfo("intake", ""),
                "Michael" => new ProcessInfo("intake", ""),
                _ => null
            };
        }
    }
}