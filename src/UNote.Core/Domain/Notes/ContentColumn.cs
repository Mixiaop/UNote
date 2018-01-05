using System;
using U.Domain.Entities.Auditing;
using UNote.Domain.Teams;

namespace UNote.Domain.Notes
{
    /// <summary>
    /// 内容列表，当使用Node使用Board模式时使用到
    /// </summary>
    public class ContentColumn : FullAuditedEntity
    {
        public ContentColumn() {
            TeamId = 0;
            NodeId = 0;
            Title = string.Empty;
            Class = string.Empty;
            Order = 0;
            //ContentCount = 0;
        }

        /// <summary>
        /// 团队Id
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// 所属栏目Id
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 自定义class（一般用于颜色）
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }


        /// <summary>
        /// 内容项总数
        /// </summary>
        //public int ContentCount { get; set; }

        #region Navigation
        /// <summary>
        /// 所属栏目
        /// </summary>
        public virtual Node Node { get; set; }

        /// <summary>
        /// 所属团队
        /// </summary>
        public virtual Team Team { get; set; }
        #endregion
    }
}
