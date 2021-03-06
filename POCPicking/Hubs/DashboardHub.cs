using System;
using System.Collections.Generic;
using System.Linq;
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
    }

    public class DashboardHub : Hub<IDashboardClient>
    {
        private readonly IPickerRepository _pickerRepository;

        private IDisposable _disposable;
        
        public override Task OnConnectedAsync()
        {
            _disposable = _pickerRepository.Observe()
                .Select(data => data.Keys.ToList())
                .Subscribe(
                    data => Clients.Caller.AvailablePickers(data)
                );
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _disposable?.Dispose();
            return base.OnDisconnectedAsync(exception);
        }

        protected override void Dispose(bool disposing)
        {
            // _disposable?.Dispose();
        }

        public DashboardHub(IPickerRepository pickerRepository)
        {
            _pickerRepository = pickerRepository;
        }

        public async void GetAvailablePickers()
        {
            // var res = _pickerRepository.FindAll();
            // await Clients.Client(Context.ConnectionId).AvailablePickers(res);
        }
    }
}