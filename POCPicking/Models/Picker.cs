using System;

namespace POCPicking
{
    public class Picker
    {
        
        private string Id { get; set; } 
        private string Name { get; }

        public Picker(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}