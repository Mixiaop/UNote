using U;
using U.Application.Services.Events;
using UNote.Services.Notes;
namespace UNote.Services.Events.Consumers
{
    public class NoteEventConsumer : IConsumer<TagUpdatedEvent>
    {
        private readonly IBoardService _boradService;
        public NoteEventConsumer() {
            _boradService = UPrimeEngine.Instance.Resolve<IBoardService>();
        }

        public void HandleEvent(TagUpdatedEvent eventMessage)
        {
            if (eventMessage != null && eventMessage.Node != null && eventMessage.OriginalTagName.IsNotNullOrEmpty() && eventMessage.NewTagName.IsNotNullOrEmpty()) {
                _boradService.ReplaceAllTaskTags(eventMessage.Node.Id, eventMessage.OriginalTagName, eventMessage.NewTagName);
            }
        }
    }
}
