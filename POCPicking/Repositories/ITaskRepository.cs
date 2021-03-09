using POCPicking.Models;

namespace POCPicking.Repositories
{
    public interface ITaskRepository
    {
        public PickerTask GetTaskForPicker(Picker picker);
        
    }
}