using System;
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

        public async Task StartShift(string name)
        {
            var result = _pickerRepository.StartShift(new Picker(Context.ConnectionId, name));
            if (result) await Clients.Caller.ShiftStartConfirmed();
            
        }
        
        public async Task StopShift(string name)
        {
            var result = _pickerRepository.StopShift(new Picker(Context.ConnectionId, name));
            if (result) await Clients.Caller.ShiftStopConfirmed();
        }

    }
}
