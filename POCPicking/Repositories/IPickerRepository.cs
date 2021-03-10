using System;
using System.Collections.Generic;
using POCPicking.Models;

namespace POCPicking.Repositories
{
    public interface IPickerRepository
    {
        bool StartShift(Picker picker);

        bool ResumeShift(Picker picker, string connectionId);
        
        bool StopShift(Picker picker);

        Picker AssignTask(Picker picker, PickerTask task);

        Picker FindByName(string name);

        List<Picker> FindAll();
        
    }
}