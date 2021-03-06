using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads;
using Warehouse.Picking.Api.Services;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake
{
    [ExternalTaskHandler(topic: "ST.Intake.Articles.NoteId")]
    public class FetchArticlesForNoteHandler : IExternalTaskHandler<NoteIdPayload, SuccessResult>
    {
        private readonly IntakeService _service;

        public FetchArticlesForNoteHandler(IntakeService service)
        {
            _service = service;
        }

        public async Task<SuccessResult> HandleAsync(NoteIdPayload input, ExternalTask task)
        {
            try
            {
                await _service.FetchArticlesForDeliveryNote(task.CorrelationId, input.NoteId);
                return new SuccessResult();
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e);
                throw new UnknownNoteIdException(input.NoteId, e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UnhandledException(e);
            }
        }
    }

    public class SuccessResult
    {
        public SuccessResult()
        {
        }
    }
}