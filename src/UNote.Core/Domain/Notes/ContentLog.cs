﻿using U.Domain.Entities.Auditing;
using UNote.Domain.Users;

namespace UNote.Domain.Notes
{
    /// <summary>
    /// 内容动态，记录一篇内容的一生
    /// (分类开启任务板模式时使用)
    /// </summary>
    public class ContentLog : CreationAuditedEntity
    {
        public ContentLog() {
            ContentId = 0;
            UserId = 0;
            Desc = "";
        }

        /// <summary>
        /// 内容Id
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 创建者信息
        /// </summary>
        public virtual User User { get; set; }
    }
}
