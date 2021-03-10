using System;

namespace POCPicking.Models
{
    public class PickerTask
    {
        public Guid Guid { get; set; }

        public string Status { get; set; }

        public PickerTask()
        {
            Guid = Guid.NewGuid();
            Status = "created";
        }

        private bool Equals(PickerTask other)
        {
            return Guid.Equals(other.Guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PickerTask) obj);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}