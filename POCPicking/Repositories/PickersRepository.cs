using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using POCPicking.Hubs;
using POCPicking.Models;

namespace POCPicking.Repositories
{
    public class PickersRepository : IPickerRepository
    {
        private readonly HashSet<Picker> _pickers = new();

        private readonly IHubContext<PickersHub> _pickersHubContext;
        
        private readonly IHubContext<DashboardHub> _dashboardHubContext;

        public PickersRepository(IHubContext<PickersHub> pickersHubContext, IHubContext<DashboardHub> dashboardHubContext)
        {
            _pickersHubContext = pickersHubContext;
            _dashboardHubContext = dashboardHubContext;
        }

        public Picker AssignTask(Picker picker, PickerTask task)
        {
            var updated = FindByName(picker.Name);
            _pickers.Remove(updated);
            task.Status = "assigned";
            updated.Task = task;
            _pickers.Add(updated);
            SendAvailablePickersToDashboard(_pickers);
            SendTaskToPickerClient(updated);
            return updated;
        }

        public bool StartShift(Picker picker)
        {
            if (!_pickers.Add(picker)) return false;
            SendAvailablePickersToDashboard(_pickers);
            return true;
        }

        public bool ResumeShift(Picker picker, string connectionId)
        {
            _pickers.Remove(picker);
            picker.ConnectionId = connectionId;
            return StartShift(picker);
        }

        public bool StopShift(Picker picker)
        {
            if (!_pickers.Remove(picker)) return false;
            SendAvailablePickersToDashboard(_pickers);
            return true;
        }

        public List<Picker> FindAll()
        {
            return _pickers.ToList();
        }

        public Picker FindByName([NotNull] string name)
        {
            return _pickers.FirstOrDefault(p => p.Name.Equals(name));
        }


        private void SendAvailablePickersToDashboard(IEnumerable<Picker> pickers)
        {
            _dashboardHubContext.Clients.Group("Dashboard")
                .SendAsync("AvailablePickers", pickers.ToList());
        }

        private void SendTaskToPickerClient(Picker updated)
        {
            _pickersHubContext.Clients.Client(updated.ConnectionId)
                .SendAsync("TaskAssigned", updated.Task.Guid.ToString());
        }
    }
}