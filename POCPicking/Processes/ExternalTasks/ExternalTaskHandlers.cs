using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using POCPicking.Models;
using POCPicking.Processes.ExternalTasks.Payloads;
using POCPicking.Repositories;

namespace POCPicking.Processes.ExternalTasks
{
    [ExternalTaskHandler(topic: "Picker.Shift.Status")]
    public class ShiftStatusHandler : IExternalTaskHandler<ShiftStatusPayload, Picker>
    {
        private readonly IPickerRepository _pickerRepository;

        public ShiftStatusHandler(IPickerRepository pickerRepository)
        {
            _pickerRepository = pickerRepository;
        }

        public Task<Picker> HandleAsync(ShiftStatusPayload input, ExternalTask task)
        {
            var picker = input.Picker;
            if (input.Status)
            {
                _pickerRepository.StartShift(picker);
            }
            else
            {
                _pickerRepository.StopShift(picker, true);
            }

            return Task.FromResult(picker);
        }
    }

    [ExternalTaskHandler(topic: "Picker.Task.Assign")]
    public class AssignTaskHandler : IExternalTaskHandler<PickerTaskPayload, Picker>
    {
        private readonly IPickerRepository _pickerRepository;
        private readonly ITaskRepository _taskRepository;

        public AssignTaskHandler(IPickerRepository pickerRepository, ITaskRepository taskRepository,
            IProcessClient processClient)
        {
            _pickerRepository = pickerRepository;
            _taskRepository = taskRepository;
        }

        public Task<Picker> HandleAsync(PickerTaskPayload input, ExternalTask task)
        {
            var p = _pickerRepository.AssignTask(input.Picker, input.Task);
            _taskRepository.Update(p.Task);
            return Task.FromResult(p);
        }
    }

    [ExternalTaskHandler(topic: "Task.Dequeue")]
    public class PeekTaskHandler : IExternalTaskHandler<Picker, PickerTask>
    {
        private readonly ITaskRepository _repository;

        public PeekTaskHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public Task<PickerTask> HandleAsync(Picker input, ExternalTask task)
        {
            return Task.FromResult(_repository.Dequeue());
        }
    }
}