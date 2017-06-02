using U.Domain.Entities.Auditing;
using UNote.Domain.Users;

namespace UNote.Domain.Notes
{
    /// <summary>
    /// 内容关注者
    /// </summary>
    public class ContentFollower : FullAuditedEntity
    {
        public ContentFollower() {
            ContentId = 0;
            UserId = 0;
            Remark = string.Empty;
        }

        /// <summary>
        /// 内容Id
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public virtual Content Content { get; set; }

        public virtual User User { get; set; }
    }
}
