using U.Domain.Entities.Auditing;

namespace UNote.Domain.Users
{
    /// <summary>
    /// 代表一个“用户”
    /// </summary>
    public class UserBrief : FullAuditedEntity
    {
        public UserBrief()
        {
            Username = "";
            NickName = "";
            ContentTotal = 0;
            AvatarId = 0;
            AvatarUrl = "";
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 发布的笔记总数
        /// </summary>
        public int ContentTotal { get; set; }

        /// <summary>
        /// 头像Id
        /// </summary>
        public int AvatarId { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string AvatarUrl { get; set; }


        #region Custom Properties
        /// <summary>
        /// 格式化用户名
        /// </summary>
        public string FormatNickName {
            get {
                string name = NickName.IsNotNullOrEmpty() ? NickName : Username;

                return name;
            }
        }
        #endregion
    }
}
