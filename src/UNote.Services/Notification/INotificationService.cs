using System.Collections.Generic;
using UNote.Domain.Users;
using UNote.Domain.Notes;
using UNote.Services.Notes.Dto;

namespace UNote.Services.Notification
{
    /// <summary>
    /// 通知服务 
    /// </summary>
    public interface INotificationService : IService
    {
        #region Contents general
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
        #endregion

        #region Tasks
        /// <summary>
        /// 当（用户）被加入到任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="operatorName"></param>
        /// <param name="user"></param>
        void TaskAddedFollower(BoardTaskDto task, string operatorName, User user);

        void TaskRemovedFollower(BoardTaskDto task, string operatorName, User user);

        void TaskContentUpdated(BoardTaskDto task, string operatorName);

        void TaskFinished(BoardTaskDto task, string operatorName);

        void TaskCanceled(BoardTaskDto task, string operatorName);

        void TaskExpirationDateUpdated(BoardTaskDto task, string operatorName);

        //void TaskColumnMoveChanged(BoardColumnBriefDto , string operatorName);
        #endregion
    }
}
