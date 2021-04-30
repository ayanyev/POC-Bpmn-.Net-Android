using System;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake
{
    public abstract class ProcessException : Exception
    {
        public string DisplayableMessage { get; }

        protected ProcessException(string message, string displayableMessage, Exception? innerException)
            : base(message, innerException)
        {
            DisplayableMessage = displayableMessage;
        }
    }

    public class UnhandledException : ProcessException
    {
        public UnhandledException(Exception e)
            : base(e.Message, "Something wrong happened", e)
        {
        }
    }

    public class UnknownNoteIdException : ProcessException
    {
        public UnknownNoteIdException(string noteId, Exception? originalException)
            : base(
                $"Given note id ({noteId}) is unknown",
                "Delivery for given note id not found",
                originalException
            )
        {
        }
    }

    public class BundleNotPresentInDelivery : ProcessException
    {
        public BundleNotPresentInDelivery(string noteId, int bundleId, Exception? originalException)
            : base(
                $"Bundle ({bundleId}) is not in delivery ({noteId})",
                "Bundle is not in delivery. Select proper",
                originalException
            )
        {
        }
    }
}