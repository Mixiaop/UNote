
namespace UNote.Services.Notification
{
    public class SendMailJobArgs
    {
        public string Email { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
