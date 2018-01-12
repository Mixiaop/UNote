using UNote.Domain.Users;
using UNote.Services.Notes.Dto;

namespace UNote.Services.Events
{
    public class TaskContentUpdatedEvent
    {
        public TaskContentUpdatedEvent(BoardTaskDto task, string operatorName)
        {
            Task = task;
            OperatorName = operatorName;
        }

        public BoardTaskDto Task { get; set; }

        public string OperatorName { get; set; }
    }
}
