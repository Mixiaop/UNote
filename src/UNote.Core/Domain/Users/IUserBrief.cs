
namespace UNote.Domain.Users
{
    public interface IUserBrief
    {
        int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        string NickName { get; set; }

        /// <summary>
        /// 发布的笔记总数
        /// </summary>
        int ContentTotal { get; set; }

        /// <summary>
        /// 头像Id
        /// </summary>
        int AvatarId { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        string AvatarUrl { get; set; }

    }
}
