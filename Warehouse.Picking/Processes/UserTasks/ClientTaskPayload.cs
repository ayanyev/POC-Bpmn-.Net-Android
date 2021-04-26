using System.Collections.Generic;
using System.Linq;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public interface IClientTaskPayload
    {
        
    }
    
    public class ScanPayload : IClientTaskPayload
    {
        public List<string> Barcodes { get; set; }

        public ScanPayload()
        {
        }

        public ScanPayload(List<string> validBarcodes)
        {
            Barcodes = validBarcodes;
        }
    }
    
    public class SelectionOptions : IClientTaskPayload
    {
        public List<SelectionOption> Items { get; }

        public SelectionOptions(IEnumerable<SelectionOption> items)
        {
            Items = items.ToList();
        }

        public override string ToString()
        {
            return Items.ToString();
        }
    }

    public class SelectionOption
    {
        public int Id { get; }

        public string Text { get; }

        public SelectionOption(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}