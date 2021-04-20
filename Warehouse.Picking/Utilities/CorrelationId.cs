using System;

namespace Warehouse.Picking.Api.Utilities
{
    public class CorrelationId
    {
        public string WorkerId { get; }

        public string ConnectionId { get; }

        public CorrelationId(string workerId, string connectionId)
        {
            WorkerId = workerId;
            ConnectionId = connectionId;
        }

        public override string ToString()
        {
            return $"{WorkerId}&{ConnectionId}";
        }

        public static CorrelationId From(string correlationId)
        {
            var a = correlationId.Split("&");
            if (a.Length == 2)
            {
                return new CorrelationId(a[0], a[1]);
            }
            throw new ArgumentException($"Invalid correlation id: {correlationId}");
        }
    }
}