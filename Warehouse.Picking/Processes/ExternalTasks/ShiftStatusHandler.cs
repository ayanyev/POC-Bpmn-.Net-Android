using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Processes.ExternalTasks.Payloads;
using Warehouse.Picking.Api.Repositories;

namespace Warehouse.Picking.Api.Processes.ExternalTasks
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
}