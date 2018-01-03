using UNote.Services.Users.Dto;

namespace UNote.Services.Notes.Dto
{
    /// <summary>
    /// 任务板日志跟踪
    /// </summary>
    public class BoardTaskLogDto : U.Application.Services.Dto.CreationAuditedEntityDto
    {
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
        public UserBriefDto User { get; set; }

        /// <summary>
        /// 格式化时间
        /// </summary>
        public string FormatCreationTime { get; set; }
    }
}
