using System;

namespace POCPicking
{
    public class PickerTask
    {
        public Guid Guid  { get; }
        
        public PickerTask()
        {
            Guid = Guid.NewGuid();
        }

    }
}