using System;
using System.Linq;
using AtlasEngine.UserTasks;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public class ClientTaskFactory
    {
        private readonly IUserTaskPayloadFactory _payloadFactory;

        public ClientTaskFactory(IUserTaskPayloadFactory payloadFactory)
        {
            _payloadFactory = payloadFactory;
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
                .ToDictionary(f => f.Id, f => f.DefaultValue);

            var payload = type switch
            {
                "Selection" => _payloadFactory.CreateSelectionOptionsPayload(userTask),
                _ => null
            };

            resultTemplate.Add("taskId", userTask.Id);

            return new ClientTask(userTask.Id, type, resultKey, label, error, payload, resultTemplate);
        }
    }
}