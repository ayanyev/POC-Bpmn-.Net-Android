using System;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake
{
    public class Exceptions
    {
        public class UnhandledException : Exception
        {
            public UnhandledException(Exception e) : base(e.Message, e)
            {
            }
        }

        public class UnknownNoteIdException : Exception
        {
            public UnknownNoteIdException(string noteId)
            {
                NoteId = noteId;
            }

            private string NoteId { get; }

            public override string Message => $"Given note id ({NoteId}) is unknown";
        }
    }
}