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

        Task DoInput(string taskId, string key);
        
        Task DoInputQuantity(string taskId, string key);
        
        Task DoInputSelection(string taskId, string key, SelectionOptions options);
        
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
                            case var t when t.Id.Equals("Intake.UT.Input.NoteId"):
                                Clients.Group(correlationId).DoInput(t.Id, "noteId");
                                handledTask = t;
                                break;
                            case var t when t.Id.Contains("Intake.UT.Input.Scan"):
                                Clients.Group(correlationId).DoInput(t.Id, "barcode");
                                handledTask = t;
                                break;
                            case var t when t.Id.Contains("Intake.UT.Input.Selection"):
                                var options = _userTaskPayloadFactory.CreateSelectionOptionsPayload(t);
                                Clients.Group(correlationId).DoInputSelection(t.Id, "bundleId", options);
                                handledTask = t;
                                break;
                            case var t when t.Id.Contains("Intake.UT.Input.Quantity"): 
                                Clients.Group(correlationId).DoInputQuantity(t.Id, "quantity");
                                handledTask = t;
                                break;
                        }
                    }
                    return handledTask;
                });
            
            await Clients.Caller.ProcessStartConfirmed();
        }

        // public async Task ProvideNoteId(string noteId)
        // {
        //     var correlationId = Context.GetUserId();
        //     const string taskId = "Intake.UT.Input.NoteId";
        //     var result = new Dictionary<string, object> {{"noteId", noteId}};
        //     await _processClient.FinishUserTask(taskId, correlationId, result);
        // }
        //
        // public async Task SendScanResult(string barcode)
        // {
        //     var correlationId = Context.GetUserId();
        //     const string taskId = "Intake.UT.Input.Scan";
        //     var result = new Dictionary<string, object> {{"barcode", barcode}};
        //     await _processClient.FinishUserTask(taskId, correlationId, result);
        // }

        public async Task SendInput(Dictionary<string, object> input)
        {
            string taskId = ((JsonElement) input["taskId"]).GetString() 
                            ?? throw new ArgumentNullException(nameof(taskId));

            // if (taskId.Contains("Intake.UT.Input.Quantity"))
            // {
            //     input.Add("forced_valid", taskId.Contains("Adjust"));
            // }

            var correlationId = Context.GetUserId();
            await _processClient.FinishUserTask(taskId, correlationId, input);
        }
    }
}