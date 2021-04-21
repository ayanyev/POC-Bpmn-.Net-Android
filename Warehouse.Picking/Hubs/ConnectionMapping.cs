using System.Collections.Generic;

namespace warehouse.picking.api.Hubs
{
    public class ConnectionMapping
    {
        private readonly object _mappingLock = new ();
        
        private readonly Dictionary<string, string> _connections = new();

        public void Add(string key, string connectionId)
        {
            lock (_mappingLock)
            {
                _connections[key] = connectionId;
            }
        }

        public string GetConnection(string key)
        {
            lock (_mappingLock)
            {
                return _connections[key];
            }
        }

        public void Remove(string key)
        {
            lock (_mappingLock)
            {
                _connections.Remove(key);
            }
        }
    }
}