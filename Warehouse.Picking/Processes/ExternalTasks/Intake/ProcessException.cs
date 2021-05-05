using System;
using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake
{
    public abstract class ProcessException : Exception
    {
        public string DisplayableMessage { get; }

        protected ProcessException(string message, string displayableMessage, Exception innerException)
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
        public UnknownNoteIdException(string noteId, Exception originalException)
            : base(
                $"Given note id ({noteId}) is unknown",
                "Delivery for given note id not found",
                originalException
            )
        {
        }
    }

    public class SelectedBundleNotAvailable : ProcessException
    {
        public SelectedBundleNotAvailable(string noteId, int bundleId, string reason) : base(
            $"Article with selected bundle is {reason} (noteId={noteId}, bundleId={bundleId})",
            $"Article with selected bundle is {reason}. Select other",
            null
        )
        { }
    }
}