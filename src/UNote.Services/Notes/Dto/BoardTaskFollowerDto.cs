namespace UNote.Services.Notes.Dto
{
    /// <summary>
    /// 任务板任务参与者Dto
    /// </summary>
    public class BoardTaskFollowerDto : U.Application.Services.Dto.IDto
    {
        public int UserId { get; set; }

        public string NickName { get; set; }

        public string PinYin { get; set; }

        public string CorpWeixinUserId { get; set; }
    }
}
