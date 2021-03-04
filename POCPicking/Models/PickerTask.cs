using System;

namespace POCPicking
{
    public class PickerTask
    {
        public Guid guid  { get; }
        
        public PickerTask()
        {
            guid = Guid.NewGuid();
        }

    }
}