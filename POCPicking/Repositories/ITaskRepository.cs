using System;
using System.Collections.Generic;
using POCPicking.Models;

namespace POCPicking.Repositories
{
    public interface ITaskRepository
    {
        
        IObservable<List<PickerTask>> Observe();

        public PickerTask Create();
        
        public void Update(PickerTask task);
        
        public PickerTask Dequeue();
        
        List<PickerTask> FindAll();
        
    }
}