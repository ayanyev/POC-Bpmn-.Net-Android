using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Subjects;
using POCPicking.Models;

namespace POCPicking.Repositories
{
    public class PickersRepository : IPickerRepository
    {
        private readonly HashSet<Picker> _pickers = new();

        private readonly BehaviorSubject<HashSet<Picker>> _observableData
            = new(new HashSet<Picker>());

        public PickersRepository()
        {
            // _pickers.Add(new Picker("", "Markus"));
            // _pickers.Add(new Picker("", "Jens"));
            // _pickers.Add(new Picker("", "Andreas"));
            // _observableData.OnNext(_pickers);
        }

        public IObservable<HashSet<Picker>> Observe()
        {
            return _observableData;
        }

        public Picker AssignTask(Picker picker, PickerTask task)
        {
            _pickers.Remove(picker);
            picker.Task = task;
            picker.Task.Status = "assigned";
            _pickers.Add(picker);
            _observableData.OnNext(_pickers);
            return picker;
        }

        public bool StartShift(Picker picker)
        {
            try
            {
                _pickers.Add(picker);
                _observableData.OnNext(_pickers);
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
            if (_pickers.Remove(picker))
            {
                _observableData.OnNext(_pickers);
                return true;
            }

            return false;
        }

        public List<Picker> FindAll()
        {
            return _pickers.ToList();
        }

        public Picker FindByName([NotNull] string name)
        {
            return _pickers.First(p => p.Name.Equals(name));
        }
    }
}