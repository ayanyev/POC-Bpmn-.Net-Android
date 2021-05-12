using System;

namespace Warehouse.Picking.Api.Processes
{
    public abstract class ProcessReattachHandler
    {
        protected abstract Action DoOnReattach { get; set; }

        public void OnReattach()
        {
            DoOnReattach?.Invoke();
        }
    }
}