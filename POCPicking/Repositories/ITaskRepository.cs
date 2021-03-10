using System;
using System.Collections.Generic;
using POCPicking.Models;

namespace POCPicking.Repositories
{
    public interface ITaskRepository
    {
        
        IObservable<Dictionary<PickerTask, Picker>> Observe();

        public PickerTask Create();
        
        public PickerTask FindByPicker(Picker picker);

        public PickerTask Peek();
        
        List<PickerTask> FindAll();
        
    }
}