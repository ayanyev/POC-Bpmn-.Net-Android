using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Repositories;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Picking
{
    [ExternalTaskHandler(topic: "Task.Dequeue")]
    public class DequeueTaskHandler : IExternalTaskHandler<Picker, PickerTask>
    {
        private readonly ITaskRepository _repository;

        public DequeueTaskHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public Task<PickerTask> HandleAsync(Picker input, ExternalTask task)
        {
            return Task.FromResult(_repository.Dequeue());
        }
    }
}