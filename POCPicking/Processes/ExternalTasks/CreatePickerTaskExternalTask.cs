using System.Threading.Tasks;
using AtlasEngine.ExternalTasks;
using POCPicking.Models;
using POCPicking.Repositories;

namespace POCPicking.Processes.ExternalTasks
{
    public class CreatePickerTaskExternalTask : IExternalTaskHandler<Picker, PickerTask>
    {
        private readonly ITaskRepository _repository;

        public CreatePickerTaskExternalTask(ITaskRepository repository)
        {
            _repository = repository;
        }

        public Task<PickerTask> HandleAsync(Picker input, ExternalTask task)
        {
            return Task.FromResult(_repository.GetTaskForPicker(input));
        }
    }
}