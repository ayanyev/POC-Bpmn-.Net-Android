using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AtlasEngine.UserTasks;
using Microsoft.AspNetCore.SignalR;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Processes;
using Warehouse.Picking.Api.Processes.UserTasks;
using Warehouse.Picking.Api.Utilities;

namespace warehouse.picking.api.Hubs
{
    public interface IIntakeClient
    {
        Task ProcessStartConfirmed();

        Task ArticlesListReceived(List<Article> articles);

        Task DoInputScan();
        
        Task DoInputQuantity();

        Task DoInputSelection(SelectionOptions options);
        
    }

    public class IntakeDeviceHub : Hub<IIntakeClient>
    {
        private const string ProcessModelId = "intake";

        private const string ProcessStartEvent = "";

        private readonly IProcessClient _processClient;

        private readonly IUserTaskPayloadFactory _userTaskPayloadFactory;

        public IntakeDeviceHub(IProcessClient processClient, IUserTaskPayloadFactory userTaskPayloadFactory)
        {
            _processClient = processClient;
            _userTaskPayloadFactory = userTaskPayloadFactory;
        }

        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.GetUserId());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.GetUserId());
            return base.OnDisconnectedAsync(exception);
        }

        public async Task StartIntakeProcess()
        {
            var correlationId = Context.GetUserId();
            
            await _processClient.CreateProcessInstanceByModelId<string>(ProcessModelId, ProcessStartEvent, null,
                correlationId);
            
            _processClient.SubscribeForPendingUserTasks(correlationId,
                tasks =>
                {
                    UserTask handledTask = null;
                    foreach (var task in tasks)
                    {
                        switch (task)
                        {
                            case var t when t.Id.Contains("Intake.UT.Input.Scan"):
                                Clients.Group(correlationId).DoInputScan();
                                handledTask = t;
                                break;
                            case var t when t.Id.Contains("Intake.UT.Input.Any"):
                                var options = _userTaskPayloadFactory.CreateSelectionOptionsPayload(t);
                                Clients.Group(correlationId).DoInputSelection(options);
                                handledTask = t;
                                break;
                            case var t when t.Id.Contains("Intake.UT.Input.Quantity"): 
                                Clients.Group(correlationId).DoInputQuantity();
                                handledTask = t;
                                break;
                        }
                    }
                    return handledTask;
                });
            
            await Clients.Caller.ProcessStartConfirmed();
        }

        public async Task ProvideNoteId(string noteId)
        {
            var correlationId = Context.GetUserId();
            const string taskId = "Intake.UT.Input.NoteId";
            var result = new Dictionary<string, object> {{"noteId", noteId}};
            await _processClient.FinishUserTask(taskId, correlationId, result);
        }

        public async Task SendScanResult(string barcode)
        {
            var correlationId = Context.GetUserId();
            const string taskId = "Intake.UT.Input.Scan";
            var result = new Dictionary<string, object> {{"barcode", barcode}};
            await _processClient.FinishUserTask(taskId, correlationId, result);
        }

        public async Task SendInput(Dictionary<string, object> input)
        {
            var taskId = "Intake.UT.Input.Any";
            if (input.Keys.Contains("quantity"))
            {
                var isForced = bool.Parse(((JsonElement) input["forced_valid"]).GetString() ?? "false");
                taskId = isForced
                    ? "Intake.UT.Input.Quantity.Adjust"
                    : "Intake.UT.Input.Quantity";
                input["forced_valid"] = isForced;
            }

            var correlationId = Context.GetUserId();
            await _processClient.FinishUserTask(taskId, correlationId, input);
        }
    }
}