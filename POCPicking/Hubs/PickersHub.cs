using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using POCPicking.Repositories;

namespace POCPicking.Hubs
{

    public interface IPickingClient
    {

        Task AvailablePickers(List<Picker> pickers);

        Task ShiftStartConfirmed();
        
        Task ShiftStopConfirmed();

    }
    
    public class PickersHub : Hub<IPickingClient>
    {

        private readonly IPickerRepository _pickerRepository;

        public PickersHub(IPickerRepository pickerRepository)
        {
            _pickerRepository = pickerRepository;
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.ConnectionId.Equals("dashboard"))
            {
                await Clients.Caller.AvailablePickers(_pickerRepository.FindAll());
            }
            await base.OnConnectedAsync();
        }

        public async Task<Boolean> StartShift(string name)
        {
            var result = _pickerRepository.StartShift(new Picker(name, Context.ConnectionId));
            if (result) await Clients.Group("dashboard").AvailablePickers(_pickerRepository.FindAll());
            return result;
        }
        
        public Boolean StopShift(string name)
        {
            return _pickerRepository.StopShift(new Picker(name, Context.ConnectionId));
        }
        
        public async void GetAvailablePickers()
        {
            await Clients.Caller.AvailablePickers(_pickerRepository.FindAll());
        }
        
    }
}
