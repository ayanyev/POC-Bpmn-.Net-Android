using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using POCPicking.Models;
using POCPicking.Processes;
using POCPicking.Repositories;

namespace POCPicking.Hubs
{
    public interface IPickerClient
    {
        Task ShiftStartConfirmed();

        Task ShiftStopConfirmed();
        
        Task TaskAssigned(string guid);
        
        Task TaskRemoved(string guid);

    }

    public class PickersHub : Hub<IPickerClient>
    {
        private const string ProcessModelId = "PickerShiftProcess";

        private const string ProcessStartEvent = ""; //"StartEvent_1mox3jl";

        private readonly IPickerRepository _pickerRepository;
        
        private readonly ITaskRepository _taskRepository;

        private readonly IProcessClient _processClient;

        public PickersHub(IPickerRepository pickerRepository, IProcessClient processClient, ITaskRepository taskRepository)
        {
            _pickerRepository = pickerRepository;
            _processClient = processClient;
            _taskRepository = taskRepository;
        }

        public async Task CheckActiveShift(string name)
        {
            var picker = _pickerRepository.FindByName(name);
            if (picker != null && _pickerRepository.ResumeShift(picker, Context.ConnectionId))
            {
                await Clients.Caller.ShiftStartConfirmed();
                if (picker.Task != null)
                {
                    await Clients.Caller.TaskAssigned(picker.Task.Guid.ToString());
                }
            }
            else
            {
                await Clients.Caller.ShiftStopConfirmed();
            }
        }

        public async Task StartShift(string name)
        {
            var picker = _pickerRepository.FindByName(name) ?? new Picker(Context.ConnectionId, name);
            if (picker.InstanceId == null || !await _processClient.IsProcessInstanceRunning(picker.InstanceId))
            {
                var startResponse =
                    await _processClient.CreateProcessInstanceByModelId(ProcessModelId, ProcessStartEvent, picker);
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
            else
            {
                if (_pickerRepository.ResumeShift(picker, Context.ConnectionId))
                {
                    await Clients.Caller.ShiftStartConfirmed();
                }
                else
                {
                    // terminate process
                }
            }
        }

        public async Task StopShift(string name)
        {
            var picker = _pickerRepository.FindByName(name);
            if (picker == null) return;

            if (await _processClient.TerminateProcessInstanceById(picker.InstanceId) &&
                _pickerRepository.StopShift(picker))
            {
                await Clients.Caller.ShiftStopConfirmed();
            }
        }

        public async Task FinishTask(string name)
        {
            var picker = _pickerRepository.FindByName(name);
            if (picker?.Task == null) return;
            var task = _pickerRepository.RemoveTask(picker);
            _taskRepository.Update(task);
            await Clients.Caller.TaskRemoved(task.Guid.ToString());

            var tasks = await _processClient.GetAllUserTasks(picker.InstanceId);
            
            var userTask = tasks.First( t => t.Id.Equals("Task_wait_for_execution"));
            
            await _processClient.FinishUserTask(userTask, new Dictionary<string, object>());
        }
    }
}