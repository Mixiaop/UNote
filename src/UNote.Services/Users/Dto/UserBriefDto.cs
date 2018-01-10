namespace UNote.Services.Users.Dto
{
    public class UserBriefDto : U.Application.Services.Dto.EntityDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }


        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 拼音名称
        /// </summary>
        public string PinYin { get; set; }

        /// <summary>
        /// 头像Id
        /// </summary>
        public int AvatarId { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string AvatarUrl { get; set; }
    }
}
