using System;

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
    }
}