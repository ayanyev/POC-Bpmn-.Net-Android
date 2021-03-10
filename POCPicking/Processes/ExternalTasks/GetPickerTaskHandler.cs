using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using POCPicking.Models;
using POCPicking.Repositories;

namespace POCPicking.Processes.ExternalTasks
{
    // [ExternalTaskHandler(topic: "Picker.Task.GetOrCreate")]
    public class GetPickerTaskHandler : IExternalTaskHandler<Picker, PickerTask>
    {
        private readonly ITaskRepository _repository;

        public GetPickerTaskHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public Task<PickerTask> HandleAsync(Picker input, ExternalTask task)
        {
            return Task.FromResult(_repository.FindByPicker(input));
        }
    }
    
    [ExternalTaskHandler(topic: "Task.Peek")]
    public class PeekTaskHandler : IExternalTaskHandler<Picker, PickerTask>
    {
        private readonly ITaskRepository _repository;

        public PeekTaskHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public Task<PickerTask> HandleAsync(Picker input, ExternalTask task)
        {
            return Task.FromResult(_repository.Peek());
        }
    }
    
}