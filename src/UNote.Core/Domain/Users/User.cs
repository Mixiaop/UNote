using System;
using U.Domain.Entities.Auditing;

namespace UNote.Domain.Users
{
    /// <summary>
    /// 代表一个“用户”
    /// </summary>
    public class User : FullAuditedEntity
    {
        public User() {
            Username = "";
            Password = "";
            NickName = "";
            UserType = UserType.General;
            ContentTotal = 0;
            AvatarId = 0;
            AvatarUrl = "";
            Email = "";
            EmailValidation = false;
            LastIpAddress = "";
            LastLoginTime = "";
            CurrentUsedTeamId = 0;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户类型Id
        /// </summary>
        public int UserTypeId { get; set; }

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

        /// <summary>
        /// Email （找回密码时用到）
        /// </summary>
        public string Email { get; set; }

        /// <summary>mi
        /// 邮箱是否验证
        /// </summary>
        public bool EmailValidation { get; set; }

        /// <summary>
        /// 最后一次访问IP
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public string LastLoginTime { get; set; }

        /// <summary>
        /// 心跳时间（默认每隔30秒）
        /// </summary>
        public DateTime? HeartbeatTime { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 当前使用的小组（0或-1 = 我的笔记）
        /// </summary>
        public int CurrentUsedTeamId { get; set; }

        #region Custom Properties
        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType {
            get {
                return (UserType)UserTypeId;
            }
            set { UserTypeId = (int)value; }
        }

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
