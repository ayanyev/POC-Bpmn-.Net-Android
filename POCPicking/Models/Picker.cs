using System;
using System.Collections.Generic;

namespace POCPicking.Models
{
    [Serializable]
    public class Picker
    {
        public string Id { get; set; } 
        public string Name { get; set; }

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