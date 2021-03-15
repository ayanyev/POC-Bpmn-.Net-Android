using System.Collections.Generic;
using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Repositories
{
    public interface ITaskRepository
    {
        
        public PickerTask Create();
        
        public void Update(PickerTask task);
        
        public PickerTask Dequeue();
        
        List<PickerTask> FindAll();
        
    }
}