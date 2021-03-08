using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using POCPicking.Models;
using POCPicking.Processes.Rest;
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
        private const string ProcessName = "PickerShiftProcess";
        
        private readonly IPickerRepository _pickerRepository;

        private readonly ProcessesHttpClient _httpClient;

        public PickersHub(IPickerRepository pickerRepository, ProcessesHttpClient httpClient)
        {
            _pickerRepository = pickerRepository;
            _httpClient = httpClient;
            _httpClient.GetAllProcessModels(ProcessName);
        }

        public async Task StartShift(string name)
        {
            var picker = new Picker(Context.ConnectionId, name);
            var instanceToken = await _httpClient.CreateProcessInstanceByModelId(ProcessName, picker);
            if (_pickerRepository.StartShift(instanceToken as Picker))
            {
                await Clients.Caller.ShiftStartConfirmed();
            }
            else
            {
                await _httpClient.TerminateProcessInstanceById(instanceToken.InstanceId);
            }
        }

        public async Task StopShift(string name)
        {
            var picker = _pickerRepository.FindByName(name);
            if (picker == null) return;

            if (await _httpClient.TerminateProcessInstanceById(picker.InstanceId) &&
                _pickerRepository.StopShift(new Picker(Context.ConnectionId, name)))
            {
                await Clients.Caller.ShiftStopConfirmed();
            }
        }
    }
}