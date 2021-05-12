using System;
using System.Linq;
using System.Threading.Tasks;
using AtlasEngine.UserTasks;
using Warehouse.Picking.Api.Utilities;
using static Warehouse.Picking.Api.Processes.UserTasks.ClientTaskType;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public class ClientTaskFactory
    {
        private readonly IProcessHandlersFactory _processHandlersFactory;
        private readonly IProcessClient _processClient;

        public ClientTaskFactory(IProcessHandlersFactory processHandlersFactory, IProcessClient processClient)
        {
            _processHandlersFactory = processHandlersFactory;
            _processClient = processClient;
        }

        public ClientTask Create(UserTask userTask)
        {
            // UT.<Type>.<ResultKey>.<Qualifier>

            var type = userTask.GetClientTaskType();

            var resultKey = userTask.GetResultKey();

            var label = userTask.Configuration.FormFields.ToList().Find(f => f.Id.Equals(resultKey))?.Label
                        ?? throw new ArgumentException($"No form field for result key ({resultKey}) found");

            var error = userTask.Tokens[0].Payload.GetPayload<TaskError>();

            var resultTemplate = userTask.Configuration.FormFields
                .ToDictionary(f => f.Id, f => f.ParseValue());

            var payload = CreatePayload(userTask, type).Result;

            resultTemplate.Add("taskId", userTask.Id);

            return new ClientTask(userTask.Id, type, resultKey, label, error, payload, resultTemplate);
        }

        private async Task<object> CreatePayload(UserTask task, ClientTaskType type)
        {
            var payloadCreator = _processHandlersFactory.GetPayloadHandler(task.ProcessModelId);
            
            if (task.HasErrorPayload())
            {
                task = await _processClient.GetPrevFinishedTaskOfSameKind(task)
                       ?? throw new Exception(
                           $"Cannot create payload because no previous finished task found ({task.Id})");
            }

            return type switch
            {
                Selection => payloadCreator?.CreateSelectionOptionsPayload(task),
                Scan => payloadCreator?.CreateScanPayload(task),
                Info => payloadCreator?.CreateInfoPayload(task),
                _ => null
            };
        }
    }
}