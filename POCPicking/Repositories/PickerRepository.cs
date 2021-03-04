using System;
using System.Collections.Generic;
using System.Linq;

namespace POCPicking.Repositories
{
    public class PickerRepository : IPickerRepository
    {
        private readonly Dictionary<Picker, PickerTask> _pickersInfo = new();

        public PickerRepository()
        {
            _pickersInfo.Add(new Picker("", "Markus"), null);
            _pickersInfo.Add(new Picker("", "Jens"), null);
            _pickersInfo.Add(new Picker("", "Andreas"), null);
        }

        public bool StartShift(Picker picker)
        {
            try
            {
                _pickersInfo.Add(picker, null);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool StopShift(Picker picker)
        {
            return _pickersInfo.Remove(picker);
        }

        public List<Picker> FindAll()
        {
            return _pickersInfo.Keys.ToList();
        }
    }
}