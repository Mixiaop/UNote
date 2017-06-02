using U.Domain.Entities.Auditing;
using UNote.Domain.Users;

namespace UNote.Domain.Teams
{
    /// <summary>
    /// 代表一个团队
    /// </summary>
    public class Team : FullAuditedEntity
    {
        public Team() {
            Name = "";
            Introduction = "";
            MemberCount = 0;
            NodeCount = 0;
            CommanderId = 0;
            LogoId = 0;
            LogoUrl = "";
            Public = false;
        }

        /// <summary>
        /// 团队公开标识（GUID生成）
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 团队名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 成员数量
        /// </summary>
        public int MemberCount { get; set; }

        /// <summary>
        /// 目录数量 
        /// </summary>
        public int NodeCount { get; set; }

        /// <summary>
        /// 团队负责人
        /// </summary>
        public int CommanderId { get; set; }

        /// <summary>
        /// *目前未使用到 是否公开
        /// </summary>
        public bool Public { get; set; }

        /// <summary>
        /// Logo id
        /// </summary>
        public int LogoId { get; set; }

        /// <summary>
        /// Logo url
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 团队负责人信息
        /// </summary>
        public virtual User Commander { get; set; } 
    }
}
