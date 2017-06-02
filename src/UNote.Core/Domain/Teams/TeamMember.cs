using U.Domain.Entities.Auditing;
using UNote.Domain.Users;

namespace UNote.Domain.Teams
{
    /// <summary>
    /// 代表“团队成员”
    /// </summary>
    public class TeamMember : FullAuditedEntity
    {
        public TeamMember() {
            TeamId = 0;
            UserId = 0;
            IsAdmin = false;
        }

        public int TeamId { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 是否管理员（对应管理权限）
        /// </summary>
        public bool IsAdmin { get; set; }

        public virtual Team Team { get; set; }

        public virtual User User { get; set; }
    }
}
