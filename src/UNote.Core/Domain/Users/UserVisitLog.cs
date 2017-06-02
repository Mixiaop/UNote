using System;
using UNote.Domain.Notes;

namespace UNote.Domain.Users
{
    /// <summary>
    /// 用户访问的日志（笔记内容、栏目）
    /// 第二次访问时只会更新访问时间
    /// </summary>
    public class UserVisitLog : U.Domain.Entities.Entity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 栏目Id
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// 内容Id
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// 类型【1=栏目，2=内容】
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastVisitTime { get; set; }


        public virtual User User { get; set; }

        public virtual Node Node { get; set; }

        public virtual Content Content { get; set; }

        public string FormatLastVisitTime
        {
            get { return CommonHelper.FormatTimeNote(LastVisitTime, LastVisitTime.ToString("yy-MM-dd HH:mm")); }
        }
    }
}
