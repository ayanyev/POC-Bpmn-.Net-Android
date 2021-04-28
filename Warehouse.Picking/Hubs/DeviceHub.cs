using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AtlasEngine.UserTasks;
using Microsoft.AspNetCore.SignalR;
using AtlasEngine.Logging;
using Warehouse.Picking.Api.Processes;
using Warehouse.Picking.Api.Processes.UserTasks;
using Warehouse.Picking.Api.Utilities;

namespace warehouse.picking.api.Hubs
{
    public interface IDeviceClient
    {
        Task ProcessStartConfirmed(string processModelName);

        Task ProcessStopConfirmed();

        Task DoInput(ClientTask task);

        Task DoInputScan(ClientTask task);

        Task DoInputSelection(ClientTask task);

        Task ShowInfo(ClientTask task);
    }

    public class DeviceHub : Hub<IDeviceClient>
    {
        private readonly ILogger _logger = ConsoleLogger.Default;

        private readonly IProcessClient _processClient;

        private readonly ConnectionMapping _connectionMapping;

        private readonly IProcessInfoProvider _processInfoProvider;

        private readonly IServiceProvider _serviceProvider;

        private readonly ClientTaskFactory _clientTaskFactory;

        public DeviceHub(IProcessClient processClient, ConnectionMapping connectionMapping,
            IProcessInfoProvider processInfoProvider, ClientTaskFactory clientTaskFactory)
        {
            _processClient = processClient;
            _connectionMapping = connectionMapping;
            _processInfoProvider = processInfoProvider;
            _clientTaskFactory = clientTaskFactory;
        }

        public override Task OnConnectedAsync()
        {
            _connectionMapping.AddConnection(Context.GetUserId(), Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connectionMapping.RemoveConnection(Context.GetUserId());
            return base.OnDisconnectedAsync(exception);
        }

        public async Task StartProcess()
        {
            var correlationId = Context.GetUserId();

            var processId = _connectionMapping.GetProcess(correlationId);

            var processInfo = _processInfoProvider.Get(correlationId);

            if (processId == null || !await _processClient.IsProcessInstanceRunning(processId))
            {
                var result = await _processClient.CreateProcessInstanceByModelId<string>(
                    correlationId, processInfo, null
                );
                _connectionMapping.AddProcess(result.CorrelationId, result.ProcessInstanceId);
            }

            var connectionId = _connectionMapping.GetConnection(correlationId);

            await Clients.Caller.ProcessStartConfirmed(processInfo.ModelId);

            _processClient.SubscribeForPendingUserTasks(correlationId, tasks =>
            {
                UserTask handledTaskId = null;
                foreach (var task in tasks)
                {
                    try
                    {
                        _logger.Log(LogLevel.Debug, $"Exec: Handled task: {task.Id}");
                        switch (_clientTaskFactory.Create(task))
                        {
                            case {Type: ClientTaskType.Selection} t:
                                Clients.Client(connectionId).DoInputSelection(t);
                                handledTaskId = task;
                                break;
                            case {Type: ClientTaskType.Scan} t:
                                Clients.Client(connectionId).DoInputScan(t);
                                handledTaskId = task;
                                break;
                            case {Type: ClientTaskType.Info} t:
                                Clients.Client(connectionId).ShowInfo(t);
                                handledTaskId = task;
                                break;
                            case { } t:
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


            _processClient.SubscribeForProcessInstanceStateChange(_connectionMapping.GetProcess(correlationId),
                instance =>
                {
                    Clients.Client(_connectionMapping.GetConnection(correlationId)).ProcessStopConfirmed();
                    return instance.State;
                });
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