using UNote.Services.Notes.Dto;

namespace UNote.Services.Events
{
    public class TaskExpirationDateUpdatedEvent
    {
        public TaskExpirationDateUpdatedEvent(BoardTaskDto task, string operatorName)
        {
            Task = task;
            OperatorName = operatorName;
        }

        public BoardTaskDto Task { get; set; }

        public string OperatorName { get; set; }
    }
}
