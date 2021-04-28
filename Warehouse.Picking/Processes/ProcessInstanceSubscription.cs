using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AtlasEngine.ProcessInstances;
using AtlasEngine.ProcessInstances.Requests;
using AtlasEngine.UserTasks;

namespace Warehouse.Picking.Api.Processes
{
    public static class EngineExtensions
    {
        public static ISubscription SubscribeForProcessesInstances(
            this IProcessInstancesClient client,
            OnProcessInstanceCallback callback,
            InstanceSubscriptionSettings subscriptionSettings
        )
        {
            var subscriptionSettings1 = subscriptionSettings ?? new InstanceSubscriptionSettings();

            if (subscriptionSettings1.FetchInterval <= TimeSpan.Zero)
                throw new ArgumentException(
                    $"Fetch interval '{subscriptionSettings1.FetchInterval}' should be positive.");

            var subscription = new Subscription(client, callback, subscriptionSettings1);
            subscription.StartAsync();
            return subscription;
        }
    }

    public sealed class InstanceSubscriptionSettings
    {
        public Action<QueryProcessInstancesOptions> ConfigureQuery { get; set; } =
            p => p.FilterByState(ProcessState.Finished);

        public bool SubscribeOnce { get; set; }

        public TimeSpan FetchInterval { get; set; } = TimeSpan.FromSeconds(2.0);
    }

    internal sealed class Subscription : ISubscription, IDisposable
    {
        private readonly IProcessInstancesClient _instanceClient;
        private readonly OnProcessInstanceCallback _callback;
        private readonly InstanceSubscriptionSettings _subscriptionSettings;
        private readonly CancellationTokenSource _stopSignal;

        public Subscription(
            IProcessInstancesClient userTaskClient,
            OnProcessInstanceCallback callback,
            InstanceSubscriptionSettings subscriptionSettings)
        {
            _instanceClient = userTaskClient ?? throw new ArgumentNullException(nameof(userTaskClient));
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
            _subscriptionSettings = subscriptionSettings ?? throw new ArgumentNullException(nameof(subscriptionSettings));
            _stopSignal = new CancellationTokenSource();
        }

        public bool IsActive { get; private set; }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            IsActive = true;
            try
            {
                while (!_stopSignal.IsCancellationRequested)
                {
                    _callback(await this._instanceClient.QueryAsync(
                        o => _subscriptionSettings.ConfigureQuery(o),
                        cancellationToken
                    ));
                    if (_subscriptionSettings.SubscribeOnce)
                        Dispose();
                    else
                        await Task.Delay(_subscriptionSettings.FetchInterval, cancellationToken);
                }
            }
            finally
            {
                IsActive = false;
            }
        }

        public void Dispose() => _stopSignal.Cancel();
    }

    public delegate void OnProcessInstanceCallback(IEnumerable<ProcessInstance> userTasks);
}