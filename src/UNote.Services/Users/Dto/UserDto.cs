﻿
namespace UNote.Services.Users.Dto
{
    public class UserDto : U.Application.Services.Dto.FullAuditedEntityDto
    {
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

        #region Custom Properties
        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// 格式化用户名
        /// </summary>
        public string FormatNickName { get; set; }
        #endregion
    }
}