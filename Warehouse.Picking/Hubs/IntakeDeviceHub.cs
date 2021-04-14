using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Processes;
using Warehouse.Picking.Api.Utilities;

namespace warehouse.picking.api.Hubs
{
    public interface IIntakeClient
    {
        Task ProcessStartConfirmed();

        Task ArticlesListReceived(List<Article> articles);
    }

    public class IntakeDeviceHub : Hub<IIntakeClient>
    {
        private const string ProcessModelId = "IntakeProcess";

        private const string ProcessStartEvent = "";

        private readonly IProcessClient _processClient;

        public IntakeDeviceHub(IProcessClient processClient)
        {
            _processClient = processClient;
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

        public async Task StartIntakeProcess(string workerId)
        {
            var correlationId = Context.GetUserId();
            await _processClient.CreateProcessInstanceByModelId<>(ProcessModelId, ProcessStartEvent, null, correlationId);
            await Clients.Caller.ProcessStartConfirmed();
        }

        public async Task ProvideNoteId(string noteId)
        {
            var correlationId = Context.GetUserId();
            const string taskId = "Intake.UT.Input.NoteId";
            var result = new Dictionary<string, object> {{"noteId", noteId}};
            await _processClient.FinishUserTask(taskId, correlationId, result);
        }
    }
}