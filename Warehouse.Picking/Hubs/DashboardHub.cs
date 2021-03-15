using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Repositories;

namespace warehouse.picking.api.Hubs
{
    public interface IDashboardClient
    {
        Task AvailablePickers(List<Picker> pickers);

        Task AvailableTasks(List<PickerTask> tasks);
    }

    public class DashboardHub : Hub<IDashboardClient>
    {
        private readonly IPickerRepository _pickerRepository;

        private readonly ITaskRepository _taskRepository;

        private readonly CompositeDisposable _disposables = new();

        public DashboardHub(IPickerRepository pickerRepository, ITaskRepository taskRepository)
        {
            _pickerRepository = pickerRepository;
            _taskRepository = taskRepository;
        }

        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, "Dashboard");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "Dashboard");
            return base.OnDisconnectedAsync(exception);
        }

        public async void GetAvailablePickers()
        {
            var res = _pickerRepository.FindAll();
            await Clients.Client(Context.ConnectionId).AvailablePickers(res);
        }
        
        public async void GetAvailableTasks()
        {
            var res = _taskRepository.FindAll();
            await Clients.Client(Context.ConnectionId).AvailableTasks(res);
        }

        public async void CreateNewTask()
        {
            _taskRepository.Create();
        }
    }
}