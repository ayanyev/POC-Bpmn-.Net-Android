using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using POCPicking.Models;
using POCPicking.Processes.ExternalTasks.Payloads;
using POCPicking.Repositories;

namespace POCPicking.Processes.ExternalTasks
{
    [ExternalTaskHandler(topic: "Picker.Task.Assign")]
    public class AssignTaskHandler : IExternalTaskHandler<PickerTaskPayload, Picker>
    {
        private readonly IPickerRepository _pickerRepository;
        private readonly ITaskRepository _taskRepository;

        public AssignTaskHandler(IPickerRepository pickerRepository, ITaskRepository taskRepository)
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