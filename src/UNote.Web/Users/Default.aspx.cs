using System;
using System.Collections.Generic;
using U;
using U.Utilities.Web;
using U.Application.Services.Dto;
using UNote.Domain.Users;
using UNote.Domain.Notes;
using UNote.Domain.Teams;
using UNote.Services.Notes;
using UNote.Services.Teams;
using UNote.Services.Users;
using UNote.Web.Infrastructure;

namespace UNote.Web.Users
{
    public partial class Default : Infrastructure.UserPage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();
        IContentFollowerService _contentFollowerService = UPrimeEngine.Instance.Resolve<IContentFollowerService>();
        ITeamService _teamService = UPrimeEngine.Instance.Resolve<ITeamService>();
        IUserVisitLogService _userVisitLogService = UPrimeEngine.Instance.Resolve<IUserVisitLogService>();
        IUserService _userService = UPrimeEngine.Instance.Resolve<IUserService>();

        protected UserDefaultModel Model = new UserDefaultModel();


        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentNav = Infrastructure.Navigation.UserHomePage;
            Model.LoginedUser = GetLoginedUser();
            if (!IsPostBack)
            {
                
                BindPageDatas();
            }
        }

        private void BindPageDatas()
        {
            Model.NotesCount = _contentService.Count(Model.LoginedUser.Id);
            Model.FollowersCount = _contentFollowerService.Count(Model.LoginedUser.Id);

            Model.Followers = _contentFollowerService.GetLastFollowers(Model.LoginedUser.Id, 10);
            Model.VisitLogs = _userVisitLogService.QueryLastVisitLogs(Model.LoginedUser, 10);
        }

    }

    public class UserDefaultModel
    {
        public UserDefaultModel()
        {
        }

        public User LoginedUser { get; set; }


        /// <summary>
        /// 笔记数
        /// </summary>
        public int NotesCount { get; set; }

        /// <summary>
        /// 关注的笔记数
        /// </summary>
        public int FollowersCount { get; set; }

        public IList<ContentFollower> Followers { get; set; }

        public IList<UserVisitLog> VisitLogs { get; set; }

        public string GetUsedTeam { get { return WebHelper.GetString("usedTeam"); } }
    }
}