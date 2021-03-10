using System;
using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using AtlasEngine.Logging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using POCPicking.Models;
using POCPicking.Processes;
using POCPicking.Processes.ExternalTasks;
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
        
        private const string ProcessModelId = "PickerShiftProcess";

        private const string ProcessStartEvent = ""; //"StartEvent_1mox3jl";
        
        private readonly IPickerRepository _pickerRepository;

        private readonly IProcessClient _processClient;

        public PickersHub(IPickerRepository pickerRepository, IProcessClient processClient)
        {
            _pickerRepository = pickerRepository;
            _processClient = processClient;
        }

        public async Task StartShift(string name)
        {
            var picker = new Picker(Context.ConnectionId, name);
            var startResponse = await _processClient.CreateProcessInstanceByModelId(ProcessModelId, ProcessStartEvent, picker);
            picker.InstanceId = startResponse.ProcessInstanceId;
            if (_pickerRepository.StartShift(picker))
            {
                await Clients.Caller.ShiftStartConfirmed();
            }
            else
            {
                await _processClient.TerminateProcessInstanceById(startResponse.ProcessInstanceId);
            }
        }

        public async Task StopShift(string name)
        {
            var picker = _pickerRepository.FindByName(name);
            if (picker == null) return;

            if (await _processClient.TerminateProcessInstanceById(picker.InstanceId) &&
                _pickerRepository.StopShift(new Picker(Context.ConnectionId, name)))
            {
                await Clients.Caller.ShiftStopConfirmed();
            }
        }
    }
}