using UNote.Domain.Users;
using UNote.Services.Notes.Dto;

namespace UNote.Services.Events
{
    public class TaskFollowerAddedEvent
    {
        public BoardTaskDto Task { get; set; }

        public string OperatorName { get; set; }

        public User User { get; set; }
    }
}
