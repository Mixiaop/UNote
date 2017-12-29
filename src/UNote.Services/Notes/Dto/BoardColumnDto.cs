using UNote.Services.Teams.Dto;

namespace UNote.Services.Notes.Dto
{
    /// <summary>
    /// 列表（任务板模式）
    /// </summary>
    public class BoardColumnDto : BoardColumnBriefDto
    {
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
