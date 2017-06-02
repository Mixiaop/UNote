
namespace UNote.Domain.Notification
{
    /// <summary>
    /// 记录一条需要发送的邮箱，用于队列发送
    /// </summary>
    public class UNoteMail : U.Domain.Entities.Auditing.FullAuditedEntity
    {
        public UNoteMail() {
            Email = "";
            Title = "";
            Body = "";
            IsSend = false;
        }


        public string Email { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public bool IsSend { get; set; }
    }
}
