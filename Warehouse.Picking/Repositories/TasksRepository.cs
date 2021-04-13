using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using warehouse.picking.api.Domain;
using warehouse.picking.api.Hubs;

namespace Warehouse.Picking.Api.Repositories
{
    public class TasksRepository : ITaskRepository
    {
        private readonly Queue<PickerTask> _tasksNotAssigned = new();

        private readonly HashSet<PickerTask> _tasksAssigned = new();

        private readonly IHubContext<PickingDashboardHub> _dashboardHubContext;

        public TasksRepository(IHubContext<PickingDashboardHub> hubContext)
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
            SendAvailableTasksToDashboard();
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
                SendAvailableTasksToDashboard();
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

        private void SendAvailableTasksToDashboard()
        {
            var tasks = _tasksNotAssigned.Concat(_tasksAssigned).ToList();
            _dashboardHubContext.Clients.Group("Dashboard")
                .SendAsync("DeliveryArticles", tasks);
        }
        
    }
}