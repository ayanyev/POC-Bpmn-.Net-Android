using System;
using System.Text.Json.Serialization;
using Warehouse.Picking.Api.Processes.Rest.Models;

namespace warehouse.picking.api.Domain
{
    [Serializable]
    public class Picker : IInstanceToken
    {
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        [JsonIgnore]
        public string InstanceId { get; set; }
        public PickerTask Task { get; set; }

        public Picker()
        {
        }

        public Picker(string connectionId, string name)
        {
            ConnectionId = connectionId;
            Name = name;
        }

        protected bool Equals(Picker other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Picker) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
        
    }
}