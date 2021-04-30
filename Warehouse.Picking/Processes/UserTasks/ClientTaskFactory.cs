using System;
using System.Linq;
using System.Threading.Tasks;
using AtlasEngine.UserTasks;
using Warehouse.Picking.Api.Utilities;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public class ClientTaskFactory
    {
        private readonly IUserTaskPayloadFactory _payloadFactory;
        private readonly IProcessClient _processClient;

        public ClientTaskFactory(IUserTaskPayloadFactory payloadFactory, IProcessClient processClient)
        {
            _payloadFactory = payloadFactory;
            _processClient = processClient;
        }

        public ClientTask Create(UserTask userTask)
        {
            // UT.Input.<Type>.<ResultKey>

            var a = userTask.Id.Split(".");

            if (a.Length < 4) throw new ArgumentException($"Wrong user task id: {userTask.Id}");

            var type = a[2];

            var resultKey = a[3];

            var label = userTask.Configuration.FormFields.ToList().Find(f => f.Id.Equals(resultKey))?.Label
                        ?? throw new ArgumentException($"No form field for result key ({resultKey}) found");

            var error = userTask.Tokens[0].Payload.GetPayload<TaskError>();

            var resultTemplate = userTask.Configuration.FormFields
                .ToDictionary(f => f.Id, f => Parse(f.DefaultValue, f.Type));

            var payload = CreatePayload(userTask, type).Result;

            resultTemplate.Add("taskId", userTask.Id);

            return new ClientTask(userTask.Id, type, resultKey, label, error, payload, resultTemplate);
        }

        private async Task<IClientTaskPayload> CreatePayload(UserTask task, string type)
        {
            if (task.HasErrorPayload())
            {
                task = await _processClient.GetPrevFinishedTaskOfSameKind(task)
                       ?? throw new Exception(
                           $"Cannot create payload because no previous finished task found ({task.Id})");
            }

            return type switch
            {
                "Selection" => _payloadFactory.CreateSelectionOptionsPayload(task),
                "Scan" => _payloadFactory.CreateScanPayload(task),
                _ => null
            };
        }

        private static object Parse(string value, FormFieldType type)
        {
            return type switch
            {
                FormFieldType.Boolean => bool.TryParse(value, out var result) && result,
                FormFieldType.Number => int.TryParse(value, out var result) ? result : 0,
                FormFieldType.Long => long.TryParse(value, out var result) ? result : 0,
                _ => value
            };
        }
    }
}