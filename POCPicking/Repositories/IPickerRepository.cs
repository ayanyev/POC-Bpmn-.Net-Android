using System;
using System.Collections.Generic;
using POCPicking.Models;

namespace POCPicking.Repositories
{
    public interface IPickerRepository
    {
        IObservable<Dictionary<Picker, PickerTask>> Observe();

        Boolean StartShift(Picker picker);
        
        Boolean StopShift(Picker picker);

        List<Picker> FindAll();
        
    }
}