using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Processes.ExternalTasks.Payloads;
using Warehouse.Picking.Api.Repositories;

namespace Warehouse.Picking.Api.Processes.ExternalTasks
{
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
}