using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Subjects;
using POCPicking.Models;

namespace POCPicking.Repositories
{
    public class PickerRepository : IPickerRepository
    {
        private readonly Dictionary<Picker, PickerTask> _pickersInfo = new();

        private readonly BehaviorSubject<Dictionary<Picker, PickerTask>> _observablePickersInfo
            = new(new Dictionary<Picker, PickerTask>());

        public PickerRepository()
        {
            _pickersInfo.Add(new Picker("", "Markus"), null);
            _pickersInfo.Add(new Picker("", "Jens"), null);
            _pickersInfo.Add(new Picker("", "Andreas"), null);
            _observablePickersInfo.OnNext(_pickersInfo);
        }

        public IObservable<Dictionary<Picker, PickerTask>> Observe()
        {
            return _observablePickersInfo;
        }

        public bool StartShift(Picker picker)
        {
            try
            {
                _pickersInfo.Add(picker, null);
                _observablePickersInfo.OnNext(_pickersInfo);
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
            if (_pickersInfo.Remove(picker))
            {
                _observablePickersInfo.OnNext(_pickersInfo);
                return true;
            }

            return false;
        }

        public List<Picker> FindAll()
        {
            return _pickersInfo.Keys.ToList();
        }

        public Picker FindByName([NotNull] string name)
        {
            return _pickersInfo.First(p => p.Key.Name.Equals(name)).Key;
        }
    }
}