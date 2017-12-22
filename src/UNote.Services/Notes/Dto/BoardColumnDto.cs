using UNote.Services.Teams.Dto;

namespace UNote.Services.Notes.Dto
{
    /// <summary>
    /// 列表（任务板模式）
    /// </summary>
    public class BoardColumnDto : U.Application.Services.Dto.FullAuditedEntityDto
    {
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
        public int ContentCount { get; set; }

        /// <summary>
        /// 所属栏目
        /// </summary>
        public NodeDto Node { get; set; }

        /// <summary>
        /// 所属团队
        /// </summary>
        public TeamDto Team { get; set; }
    }
}
