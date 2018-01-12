using System.Collections.Generic;
using U;
using U.Net.Mail;
using U.Utilities.Web;
using U.BackgroundJobs;
using UNote.Domain.Notes;
using UNote.Domain.Users;
using UNote.Services.Notes;
using UNote.Services.Notes.Dto;
using UNote.Services.Users;
using UNote.Services.External;

namespace UNote.Services.Notification
{
    public class NotificationService : ServiceBase, INotificationService
    {
        private readonly IEmailSender _emailSender;
        private readonly INodeService _nodeService;
        private readonly IUserService _userService;
        private readonly ICorpWeixinService _corpWeixinService;

        public NotificationService(IEmailSender emailSender, INodeService nodeService, IUserService userService, ICorpWeixinService corpWeixinService)
        {
            _emailSender = emailSender;
            _nodeService = nodeService;
            _userService = userService;
            _corpWeixinService = corpWeixinService;
        }

        #region Contents general
        /// <summary>
        /// 添加笔记后发送（如果有关注的人）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="users"></param>
        public void AddContentAfterSending(Content content, IList<User> users)
        {
            if (users != null)
            {
                foreach (var user in users)
                {
                    if (user.Username.IsValidEmail())
                    {
                        string host = WebHelper.GetHost();

                        if (content.Node == null)
                            content.Node = _nodeService.GetById(content.NodeId);
                        if (content.User == null)
                            content.User = _userService.GetById(content.UserId);

                        string title = string.Format("【{0}】【{1}】 => 笔记创建", content.Node.NodeName, content.Title);
                        string body = string.Format("【{0}】笔记创建 => {1} => <a href=\"{3}/Notes/ContentInfo.aspx?contentid={4}\" target=\"_blank\">{2}</a>",
                            content.User.FormatNickName,
                            content.Node.NodeName,
                            content.Title,
                            host.TrimEnd('/'),
                            content.Id);

                        var args = new SendMailJobArgs();
                        if (user.Email.IsNotNullOrEmpty())
                        {
                            args.Email = user.Email;
                        }
                        else
                        {
                            args.Email = user.Username;
                        }
                        args.Title = title;
                        args.Body = body;
                        UPrimeEngine.Instance.Resolve<IBackgroundJobManager>().EnqueueAsync<SendMailJob, SendMailJobArgs>(args);
                        //_emailSender.Send(user.Username, title, body);
                    }
                }
            }
        }

        /// <summary>
        /// 更新笔记后发送（如果有关注的人）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="users"></param>
        public void UpdateContentAfterSending(Content content, IList<User> users)
        {
            if (users != null)
            {
                foreach (var user in users)
                {
                    if (user.Username.IsValidEmail())
                    {
                        string host = WebHelper.GetHost();
                        if (content.LastModifierUserId != content.UserId)
                        {
                            content.User = _userService.GetById(content.LastModifierUserId.ToInt());
                        }

                        string title = string.Format("【{0}】【{1}】 => 笔记更新", content.Node.NodeName, content.Title);
                        string body = string.Format("【{0}】笔记更新 => {1} => <a href=\"{3}/Notes/ContentInfo.aspx?contentid={4}\" target=\"_blank\">{2}</a>",
                            content.User.FormatNickName,
                            content.Node.NodeName,
                            content.Title,
                            host.TrimEnd('/'),
                            content.Id);

                        var args = new SendMailJobArgs();
                        if (user.Email.IsNotNullOrEmpty())
                        {
                            args.Email = user.Email;
                        }
                        else
                        {
                            args.Email = user.Username;
                        }
                        args.Title = title;
                        args.Body = body;
                        UPrimeEngine.Instance.Resolve<IBackgroundJobManager>().EnqueueAsync<SendMailJob, SendMailJobArgs>(args);
                        //_emailSender.Send(user.Username, title, body);
                    }
                }
            }
        }
        #endregion

        #region Tasks
        /// <summary>
        /// 当（用户）被加入到任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="operatorName"></param>
        /// <param name="user"></param>
        public void TaskAddedFollower(BoardTaskDto task, string operatorName, User user)
        {
            if (Settings.CorpWeixinOpend && user.CorpWeixinUserId.IsNotNullOrEmpty())
            {
                var userList = new List<string>();
                userList.Add(user.CorpWeixinUserId);
                string content = string.Format("你已被【{0}】加入并参与到任务（【{2}】{1}）。", operatorName, task.Title, task.Node.NodeName);
                _corpWeixinService.SendMessage(userList, content);
            }
        }

        public void TaskRemovedFollower(BoardTaskDto task, string operatorName, User user)
        {
            if (Settings.CorpWeixinOpend && user.CorpWeixinUserId.IsNotNullOrEmpty())
            {
                var userList = new List<string>();
                userList.Add(user.CorpWeixinUserId);
                string content = string.Format("你已被【{0}】从任务（【{2}】{1}）参与者中移除了。", operatorName, task.Title, task.Node.NodeName);
                _corpWeixinService.SendMessage(userList, content);
            }
        }

        public void TaskContentUpdated(BoardTaskDto task, string operatorName)
        {
            if (Settings.CorpWeixinOpend)
            {
                var userList = new List<string>();
                task.Followers.ForEach((f) => {
                    if (f.CorpWeixinUserId.IsNotNullOrEmpty()) {
                        userList.Add(f.CorpWeixinUserId);
                    }
                });

                string content = string.Format("【{0}】更新了任务（【{1}】{2}）的内容。", operatorName, task.Node.NodeName, task.Title);
                _corpWeixinService.SendMessage(userList, content);
            }
        }

        public void TaskFinished(BoardTaskDto task, string operatorName) {
            if (Settings.CorpWeixinOpend)
            {
                var userList = new List<string>();
                task.Followers.ForEach((f) => {
                    if (f.CorpWeixinUserId.IsNotNullOrEmpty())
                    {
                        userList.Add(f.CorpWeixinUserId);
                    }
                });

                string content = string.Format("【{0}】完成了任务（【{1}】{2}）。", operatorName, task.Node.NodeName, task.Title);
                _corpWeixinService.SendMessage(userList, content);
            }
        }

        public void TaskCanceled(BoardTaskDto task, string operatorName) {
            if (Settings.CorpWeixinOpend)
            {
                var userList = new List<string>();
                task.Followers.ForEach((f) => {
                    if (f.CorpWeixinUserId.IsNotNullOrEmpty())
                    {
                        userList.Add(f.CorpWeixinUserId);
                    }
                });

                string content = string.Format("【{0}】取消完成了任务（【{1}】{2}）。", operatorName, task.Node.NodeName, task.Title);
                _corpWeixinService.SendMessage(userList, content);
            }
        }
        #endregion
    }
}
