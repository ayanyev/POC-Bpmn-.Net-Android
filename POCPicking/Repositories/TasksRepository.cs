using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using POCPicking.Hubs;
using POCPicking.Models;

namespace POCPicking.Repositories
{
    public class TasksRepository : ITaskRepository
    {
        private readonly Queue<PickerTask> _tasksNotAssigned = new();

        private readonly HashSet<PickerTask> _tasksAssigned = new();

        private readonly IHubContext<DashboardHub> _dashboardHubContext;

        public TasksRepository(IHubContext<DashboardHub> hubContext)
        {
            _dashboardHubContext = hubContext;
        }

        public void Update(PickerTask task)
        {
            if (!_tasksAssigned.Add(task))
            {
                _tasksAssigned.Remove(task);
                _tasksAssigned.Add(task);
            }

            Emit();
        }

        public PickerTask Dequeue()
        {
            try
            {
                return _tasksNotAssigned.Dequeue();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public PickerTask Create()
        {
            try
            {
                var t = new PickerTask();
                _tasksNotAssigned.Enqueue(t);
                Emit();
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
            return _tasksNotAssigned.Concat(_tasksAssigned).ToList();
        }

        private void SendAvailablePickersToDashboard(IEnumerable<PickerTask> tasks)
        {
            _dashboardHubContext.Clients.Group("Dashboard")
                .SendAsync("AvailableTasks", tasks.ToList());
        }
        
        private void Emit()
        {
            var list = _tasksNotAssigned.Concat(_tasksAssigned).ToList();
            SendAvailablePickersToDashboard(list);
        }
    }
}