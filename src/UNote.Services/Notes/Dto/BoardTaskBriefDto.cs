using System;
using System.Collections.Generic;

namespace UNote.Services.Notes.Dto
{
    public class BoardTaskBriefDto : U.Application.Services.Dto.FullAuditedEntityDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 团队Id
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// 所属分类 
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// 标签
        /// 多个标签用英文逗号（,）分开
        /// </summary>
        public string Tag { get; set; }

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

        #region Custom
        /// <summary>
        /// 所有参与者
        /// </summary>
        public List<BoardTaskFollowerDto> Followers { get; set; }

        public bool ExistsBody { get; set; }

        List<string> _formatTags;
        public List<string> FormatTags
        {
            get
            {
                if (_formatTags == null)
                    _formatTags = new List<string>();

                if (Tag.IsNotNullOrEmpty())
                {
                    var tags = Tag.Split(',');
                    foreach (var t in tags)
                    {
                        if (t.IsNullOrEmpty())
                        {
                            _formatTags.Add(t);
                        }
                    }
                }

                return _formatTags;
            }
        }
        #endregion
    }
}
