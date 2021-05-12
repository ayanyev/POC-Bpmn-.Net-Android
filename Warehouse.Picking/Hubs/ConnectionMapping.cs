using System.Collections.Generic;

namespace warehouse.picking.api.Hubs
{
    public class ConnectionMapping
    {
        private readonly object _mappingLock = new();

        private readonly Dictionary<string, string> _connections = new();

        private readonly Dictionary<string, string> _processes = new();

        public void MapConnection(string key, string connectionId)
        {
            lock (_mappingLock)
            {
                _connections[key] = connectionId;
            }
        }

        public void MapProcess(string key, string processId)
        {
            lock (_mappingLock)
            {
                _processes[key] = processId;
            }
        }

        public string GetConnection(string key)
        {
            lock (_mappingLock)
            {
                return _connections.TryGetValue(key, out var result) ? result : "";
            }
        }

        public string GetProcess(string key)
        {
            lock (_mappingLock)
            {
                return _processes.TryGetValue(key, out var result) ? result : null;
            }
        }

        public void UnmapConnection(string key)
        {
            lock (_mappingLock)
            {
                _connections.Remove(key);
            }
        }

        public void UnmapProcess(string key)
        {
            lock (_mappingLock)
            {
                _processes.Remove(key);
            }
        }
    }
}