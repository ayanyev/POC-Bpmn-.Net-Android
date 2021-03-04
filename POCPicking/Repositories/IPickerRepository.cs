using System;
using System.Collections.Generic;

namespace POCPicking.Repositories
{
    public interface IPickerRepository
    {
        Boolean StartShift(Picker picker);
        
        Boolean StopShift(Picker picker);

        List<Picker> FindAll();
    }
}