using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using POCPicking.Models;
using POCPicking.Repositories;

namespace POCPicking.Processes.ExternalTasks
{
    [ExternalTaskHandler(topic: "Picker.Task.GetOrCreate")]
    public class CreatePickerTaskHandler : IExternalTaskHandler<Picker, PickerTask>
    {
        private readonly ITaskRepository _repository;

        public CreatePickerTaskHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public Task<PickerTask> HandleAsync(Picker input, ExternalTask task)
        {
            return Task.FromResult(_repository.GetTaskForPicker(input));
        }
    }
}