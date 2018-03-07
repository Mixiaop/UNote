using System;
using U.Utilities.Web;
using UNote.Domain.Users;

namespace UNote.Web.Infrastructure
{
    public class UserPage : PageBase
    {
        private User _cachedUser = null;

        protected override void OnInit(EventArgs e)
        {
            if (!IsLogined()) {
                Response.Redirect(RouteContext.GetRouteUrl("Users.Login", WebHelper.GetThisPageUrl(true)));
            }
        }


        /// <summary>
        /// 获取已登录用户信息
        /// </summary>
        /// <returns></returns>
        public User GetLoginedUser() {
            if (_cachedUser == null)
                _cachedUser = authenticationService.GetAuthenticatedUser();

            return _cachedUser;
        }

        public Navigation CurrentNav = Navigation.None;

        public bool HiddenHeader { get; set; } = false;

        public int CurrentNavNodeId = 0;

        public string CurrentNavTeamKey = "";

        public bool ClearUserPage = false;
    }

    public enum Navigation { 
        AddContent,
        ManageNode,
        Contents,
        UserHomePage,
        CreateNewTeam,
        Team,
        None
        
    }
}