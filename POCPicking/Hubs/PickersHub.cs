using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using POCPicking.Models;
using POCPicking.Repositories;

namespace POCPicking.Hubs
{

    public interface IPickerClient
    {

        Task ShiftStartConfirmed();
        
        Task ShiftStopConfirmed();

    }
    
    public class PickersHub : Hub<IPickerClient>
    {

        private readonly IPickerRepository _pickerRepository;

        public PickersHub(IPickerRepository pickerRepository)
        {
            _pickerRepository = pickerRepository;
        }

        public override async Task OnConnectedAsync()
        {
            // await Clients.Caller.AvailablePickers(_pickerRepository.FindAll());
            await base.OnConnectedAsync();
        }

        public async Task<Boolean> StartShift(string name)
        {
            var result = _pickerRepository.StartShift(new Picker(name, Context.ConnectionId));
            return result;
        }
        
        public Boolean StopShift(string name)
        {
            return _pickerRepository.StopShift(new Picker(name, Context.ConnectionId));
        }

    }
}
