using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using POCPicking.Models;

namespace POCPicking.Repositories
{
    public class TasksRepository : ITaskRepository
    {
        private readonly Queue<KeyValuePair<PickerTask, Picker>> _tasks = new();
        
        private readonly BehaviorSubject<Dictionary<PickerTask, Picker>> _observableTasks
            = new(new Dictionary<PickerTask, Picker>());

        public TasksRepository()
        {
            // _tasks.Add(new PickerTask(), null);
            // _tasks.Add(new PickerTask(), null);
            // _tasks.Add(new PickerTask(), null);
            // _observableTasks.OnNext(_tasks);
        }

        public IObservable<Dictionary<PickerTask, Picker>> Observe()
        {
            return _observableTasks;
        }

        public PickerTask Peek()
        {
            try
            {
                return _tasks.Peek().Key;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public PickerTask Create()
        {
            var t = new PickerTask();
            try
            {
                _tasks.Enqueue(new KeyValuePair<PickerTask, Picker>(t, null));
                _observableTasks.OnNext(_tasks.ToDictionary(
                    x => x.Key,
                    y => y.Value
                    ));
                return t;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public List<PickerTask> FindAll()
        {
            return _tasks.Select( d => d.Key).ToList();
        }

        public PickerTask FindByPicker(Picker picker)
        {
            try
            {
                return _tasks
                    .First(p => p.Value?.Equals(picker) ?? false)
                    .Key;
            }
            catch (InvalidOperationException e)
            {
                return null;
            }
        }
    }
}