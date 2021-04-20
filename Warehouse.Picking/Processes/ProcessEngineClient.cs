using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using AtlasEngine.Logging;
using AtlasEngine.ProcessDefinitions;
using AtlasEngine.ProcessDefinitions.Requests;
using AtlasEngine.ProcessInstances;
using AtlasEngine.UserTasks;
using AtlasEngine.UserTasks.Requests;

namespace Warehouse.Picking.Api.Processes
{
    public class ProcessEngineClient : IProcessClient
    {
        private const string BaseUrl = "http://localhost:56000";

        private readonly ILogger _logger = ConsoleLogger.Default;

        private readonly IProcessDefinitionsClient _defClient =
            ClientFactory.CreateProcessDefinitionsClient(new Uri(BaseUrl));

        private readonly IProcessInstancesClient _instanceClient =
            ClientFactory.CreateProcessInstancesClient(new Uri(BaseUrl));

        private readonly IUserTaskClient _userTaskClient =
            ClientFactory.CreateUserTaskClient(new Uri(BaseUrl));

        public ProcessEngineClient()
        {
        }

        public static IExternalTaskClient CreateExternalTaskClient()
        {
            return ClientFactory.CreateExternalTaskClient(new Uri(BaseUrl),
                logger: ConsoleLogger.Default);
        }

        public void SubscribeForPendingUserTasks(string correlationId, Func<IEnumerable<UserTask>, UserTask> action)
        {
            var queryOptions = new QueryUserTasksOptions();
            queryOptions.FilterByState(UserTaskState.Suspended);
            queryOptions.FilterByCorrelationId(correlationId);

            var subscriptionSettings = new SubscriptionSettings {SubscribeOnce = false};
            subscriptionSettings.ConfigureQuery(queryOptions);
            
            var handledTaskId = "";

            Action<UserTask> updateRecentTaskId = task =>
            {
                handledTaskId = task?.Id ?? handledTaskId;
            };

            Func <string> getRecentTaskId = () => handledTaskId;
            
            void Callback(IEnumerable<UserTask> tasks)
            {
                _logger.Log(LogLevel.Debug, $"Start: Handled task: {getRecentTaskId()}");
                var filteredTasks = tasks.ToList().FindAll(t => !t.Id.Equals(getRecentTaskId()));
                updateRecentTaskId(action(filteredTasks));
                _logger.Log(LogLevel.Debug, $"End: Handled task: {getRecentTaskId()}");
            }

            _userTaskClient.SubscribeForPendingUserTask(Callback, subscriptionSettings);
        }

        public async Task FinishUserTask(string taskId, string correlationId, Dictionary<string, object> result)
        {
            try
            {
                var tasks = await _userTaskClient.QueryAsync(
                    options =>
                    {
                        options.FilterByState(UserTaskState.Suspended);
                        options.FilterByCorrelationId(correlationId);
                    });
                var task = tasks.First(t => t.Id.Contains(taskId));
                await FinishUserTask(task, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task FinishUserTask(UserTask task, Dictionary<string, object> result)
        {
            await _userTaskClient.FinishUserTaskAsync(task, new UserTaskResult(result));
        }

        public async Task<List<UserTask>> GetAllUserTasks(string processId)
        {
            var tasks = await _userTaskClient.QueryAsync(
                q => q.FilterByProcessInstanceId(processId)
            );
            return tasks.ToList();
        }

        public async Task<bool> IsProcessInstanceRunning(string processId)
        {
            var res = (await _instanceClient.QueryAsync(
                options => options.FilterByProcessInstanceId(processId)
            )).ToList();
            return res.Count > 0 && res.First().State == ProcessState.Running;
        }

        public async Task<StartProcessInstanceResponse> CreateProcessInstanceByModelId<T>(string modelId,
            string startEvent, T token)
        {
            return await CreateProcessInstanceByModelId(modelId, startEvent, token, "");
        }

        public async Task<StartProcessInstanceResponse> CreateProcessInstanceByModelId<T>(string modelId,
            string startEvent, T token, string correlationId)
        {
            try
            {
                // terminate process with same correlationId before start new
                // temporary solution
                (await _instanceClient.QueryAsync(
                    options => options.FilterByCorrelationId(correlationId)
                )).ToList().ForEach(
                    p => _instanceClient.TerminateProcessInstanceAsync(p.Id)
                );
                return await _defClient.StartProcessInstanceAsync(modelId, startEvent, token, correlationId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> TerminateProcessInstanceById(string processId)
        {
            try
            {
                await _instanceClient.TerminateProcessInstanceAsync(processId);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}