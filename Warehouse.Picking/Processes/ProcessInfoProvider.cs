namespace Warehouse.Picking.Api.Processes
{
    public class ProcessInfoProvider : IProcessInfoProvider
    {
        public ProcessInfo Get(string name)
        {
            return name switch
            {
                "Max" => new ProcessInfo("WH.Intake", ""),
                "Jorg" => new ProcessInfo("WH.Test.1", ""),
                "Michael" => new ProcessInfo("intake", ""),
                _ => null
            };
        }
    }
}