using System;
using System.Text.Json.Serialization;
using POCPicking.Processes.Rest.Models;

namespace POCPicking.Models
{
    [Serializable]
    public class Picker : IInstanceToken
    {
        public string Id { get; set; } 
        public string Name { get; set; }
        [JsonIgnore]
        public string InstanceId { get; set; }

        public Picker(string id, string name)
        {
            Id = id;
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
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Picker) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
        
    }
}