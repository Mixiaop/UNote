using System.Collections.Generic;
using UNote.Domain.Notes;
using UNote.Domain.Users;

namespace UNote.Services.Notes
{
    /// <summary>
    /// 内容关注者服务
    /// 可以为“笔记内容”添加多个关注者，如果“笔记内容”有变动则会通知（邮箱）用户
    /// </summary>
    public interface IContentFollowerService : U.Application.Services.IApplicationService
    {
        void AddFollower(Content content, User user, string remark = "");

        IList<ContentFollower> GetAllFollowers(int contentId);

        IList<ContentFollower> GetAllFollowers(List<int> contentIds);

        IList<ContentFollower> GetLastFollowers(int userId, int count = 0);

        int Count(int userId);

        void RemoveFollower(int followerId);

        void RemoveFollower(Content content, User user);

        void DeleteFollowers(int contentId);

        bool ExistsFollower(Content content, User user);
    }
}
