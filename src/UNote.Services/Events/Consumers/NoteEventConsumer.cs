using U;
using U.Application.Services.Events;
using UNote.Services.Notes;
using UNote.Services.Notification;

namespace UNote.Services.Events.Consumers
{
    public class NoteEventConsumer : IConsumer<TagUpdatedEvent>,
                                     IConsumer<TaskFollowerAddedEvent>,
                                     IConsumer<TaskFollowerRemovedEvent>,
                                     IConsumer<TaskContentUpdatedEvent>,
                                     IConsumer<TaskFinishedEvent>,
                                     IConsumer<TaskCanceledEvent>
    {
        private readonly IBoardService _boradService;
        private readonly INotificationService _notificationService;
        public NoteEventConsumer() {
            _boradService = UPrimeEngine.Instance.Resolve<IBoardService>();
            _notificationService = UPrimeEngine.Instance.Resolve<INotificationService>();
        }

        public void HandleEvent(TagUpdatedEvent eventMessage)
        {
            if (eventMessage != null && eventMessage.Node != null && eventMessage.OriginalTagName.IsNotNullOrEmpty() && eventMessage.NewTagName.IsNotNullOrEmpty()) {
                _boradService.ReplaceAllTaskTags(eventMessage.Node.Id, eventMessage.OriginalTagName, eventMessage.NewTagName);
            }
        }

        public void HandleEvent(TaskFollowerAddedEvent eventMessage)
        {
            if (eventMessage != null)
            {
                _notificationService.TaskAddedFollower(eventMessage.Task, eventMessage.OperatorName, eventMessage.User);
            }
        }

        public void HandleEvent(TaskFollowerRemovedEvent eventMessage)
        {
            if (eventMessage != null)
            {
                _notificationService.TaskRemovedFollower(eventMessage.Task, eventMessage.OperatorName, eventMessage.User);
            }
        }

        public void HandleEvent(TaskContentUpdatedEvent eventMessage)
        {
            if (eventMessage != null)
            {
                _notificationService.TaskContentUpdated(eventMessage.Task, eventMessage.OperatorName);
            }
        }

        public void HandleEvent(TaskFinishedEvent eventMessage)
        {
            if (eventMessage != null)
            {
                _notificationService.TaskFinished(eventMessage.Task, eventMessage.OperatorName);
            }
        }

        public void HandleEvent(TaskCanceledEvent eventMessage)
        {
            if (eventMessage != null)
            {
                _notificationService.TaskCanceled(eventMessage.Task, eventMessage.OperatorName);
            }
        }
    }
}
