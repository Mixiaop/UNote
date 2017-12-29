using System;
namespace UNote.Services.Notes.Dto
{
    public class BoardTaskBriefDto : U.Application.Services.Dto.FullAuditedEntityDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 所属分类 
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// 所属列Id（Board模式）
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// 所属列排序（Board模式）
        /// </summary>
        public int ColumnOrder { get; set; }

        /// <summary>
        /// 任务是否完成
        /// </summary>
        public bool ColumnTaskFinished { get; set; }

        /// <summary>
        /// 任务完成的时间
        /// </summary>
        public DateTime? ColumnTaskFinishedTime { get; set; }

        /// <summary>
        /// 任务完成的用户Id
        /// </summary>
        public int ColumnTaskFinishedUserId { get; set; }

        public bool ExistsBody { get; set; }
    }
}
