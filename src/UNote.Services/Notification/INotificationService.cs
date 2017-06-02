using System.Collections.Generic;
using UNote.Domain.Users;
using UNote.Domain.Notes;

namespace UNote.Services.Notification
{
    /// <summary>
    /// 通知服务 
    /// </summary>
    public interface INotificationService : U.Application.Services.IApplicationService
    {
        /// <summary>
        /// 添加笔记后发送（如果有关注的人）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="users"></param>
        void AddContentAfterSending(Content content, IList<User> users);

        /// <summary>
        /// 更新笔记后发送（如果有关注的人）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="users"></param>
        void UpdateContentAfterSending(Content content, IList<User> users);

    }
}
