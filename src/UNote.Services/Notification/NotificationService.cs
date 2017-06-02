using System.Collections.Generic;
using U;
using U.Net.Mail;
using U.Utilities.Web;
using U.BackgroundJobs;
using UNote.Domain.Notes;
using UNote.Domain.Users;
using UNote.Services.Notes;
using UNote.Services.Users;

namespace UNote.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailSender _emailSender;
        private readonly INodeService _nodeService;
        private readonly IUserService _userService;
        public NotificationService(IEmailSender emailSender, INodeService nodeService, IUserService userService)
        {
            _emailSender = emailSender;
            _nodeService = nodeService;
            _userService = userService;
        }

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
    }
}
