using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using POCPicking.Models;
using POCPicking.Repositories;

namespace POCPicking.Hubs
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
            _disposables.Add(_pickerRepository.Observe()
                .Select(data => data.ToList())
                .Subscribe(
                    data => Clients.Caller.AvailablePickers(data)
                ));
            _disposables.Add(_taskRepository.Observe()
                .Subscribe(
                    data => Clients.Caller.AvailableTasks(data)
                ));
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _disposables?.Dispose();
            return base.OnDisconnectedAsync(exception);
        }

        protected override void Dispose(bool disposing)
        {
            // _disposables?.Dispose();
        }

        public async void GetAvailablePickers()
        {
            var res = _pickerRepository.FindAll();
            await Clients.Client(Context.ConnectionId).AvailablePickers(res);
        }

        public async void CreateNewTask()
        {
            _taskRepository.Create();
        }
    }
}