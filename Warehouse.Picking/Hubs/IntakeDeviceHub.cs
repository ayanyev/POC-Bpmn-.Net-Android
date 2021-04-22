using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AtlasEngine.UserTasks;
using Microsoft.AspNetCore.SignalR;
using AtlasEngine.Logging;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Processes;
using Warehouse.Picking.Api.Processes.UserTasks;
using Warehouse.Picking.Api.Utilities;

namespace warehouse.picking.api.Hubs
{
    public interface IIntakeClient
    {
        Task ProcessStartConfirmed();

        Task ProcessStopConfirmed();

        Task DoInput(ClientTask task);
        
        Task DoInputSelection(ClientTask task);
        
    }

    public class IntakeDeviceHub : Hub<IIntakeClient>
    {
        private const string ProcessModelId = "intake";

        private const string ProcessStartEvent = "";
        
        private readonly ILogger _logger = ConsoleLogger.Default;

        private readonly IProcessClient _processClient;

        private readonly ConnectionMapping _connectionMapping;

        private readonly ClientTaskFactory _clientTaskFactory;

        public IntakeDeviceHub(IProcessClient processClient, ConnectionMapping connectionMapping, ClientTaskFactory clientTaskFactory)
        {
            _processClient = processClient;
            _connectionMapping = connectionMapping;
            _clientTaskFactory = clientTaskFactory;
        }

        public override Task OnConnectedAsync()
        {
            _connectionMapping.Add(Context.GetUserId(), Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connectionMapping.Remove(Context.GetUserId());
            return base.OnDisconnectedAsync(exception);
        }

        public async Task StartProcess()
        {
            var correlationId = Context.GetUserId();
            
            await _processClient.CreateProcessInstanceByModelId<string>(
                ProcessModelId, ProcessStartEvent, null, correlationId
            );
            
            var connectionId = _connectionMapping.GetConnection(Context.GetUserId());
            
            _processClient.SubscribeForPendingUserTasks(correlationId,
                tasks =>
                {
                    UserTask handledTaskId = null;
                    foreach (var task in tasks)
                    {
                        try
                        {
                            _logger.Log(LogLevel.Debug, $"Exec: Handled task: {task.Id}");
                            switch (_clientTaskFactory.Create(task))
                            {
                                case {Type: "Selection"} t:
                                    Clients.Client(connectionId).DoInputSelection(t);
                                    handledTaskId = task;
                                    break;
                                case {} t:
                                    Clients.Client(connectionId).DoInput(t);
                                    handledTaskId = task;
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                    return handledTaskId;
                });
            
            await Clients.Caller.ProcessStartConfirmed();
        }
        
        public async Task StopProcess()
        {
            await _processClient.TerminateProcessCorrelationId(Context.GetUserId());
            await Clients.Caller.ProcessStopConfirmed();
        }

        public async Task SendInput(Dictionary<string, object> input)
        {
            string taskId = ((JsonElement) input["taskId"]).GetString() 
                            ?? throw new ArgumentNullException(nameof(taskId));

            var correlationId = Context.GetUserId();
            await _processClient.FinishUserTask(taskId, correlationId, input);
        }
    }
}