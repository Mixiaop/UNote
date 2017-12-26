namespace UNote.Services.Notes.Dto
{
    public class TagDto : U.Application.Services.Dto.FullAuditedEntityDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 别名（“别名”是在URL中使用的别称，它可以令URL更美观。通常使用小写，只能包含字母，数字和连字符（-））
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 使用次数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 所属目录Id
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// Css 颜色
        /// </summary>
        public string StyleColor { get; set; }
    }
}
