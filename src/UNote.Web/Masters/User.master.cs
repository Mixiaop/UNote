using System;
using System.Collections.Generic;
using System.Linq;
using U;
using U.Utilities.Web;
using U.FakeMvc.Routes;
using UNote.Configuration;
using UNote.Domain.Notes;
using UNote.Domain.Teams;
using UNote.Services.Notes;
using UNote.Services.Teams;
using UNote.Web.Infrastructure;

namespace UNote.Web.Masters
{
    public partial class User : System.Web.UI.MasterPage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        ITeamService _teamService = UPrimeEngine.Instance.Resolve<ITeamService>();
        IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();
        UNoteSettings _settings = UPrimeEngine.Instance.Resolve<UNoteSettings>();

        protected RouteContext RouteContext = RouteContext.Instance;
        protected UserModel Model = new UserModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            Model.CurrentPage = (UserPage)this.Page;
            Model.LoginedUser = Model.CurrentPage.GetLoginedUser();
            Model.MyNodes = _nodeService.GetAll(Model.LoginedUser.Id);
            Model.MyAdminTeams = _teamService.GetAllTeams(Model.LoginedUser.Id, false);
            Model.Teams = _teamService.GetAllTeams(Model.LoginedUser.Id, false);
            Model.UserHeartbeatTime = _settings.UserHeartbeatTime * 1000;

            if (Model.Teams != null) {
                List<int> teamIds = new List<int>();
                foreach (var t in Model.Teams)
                    teamIds.Add(t.Id);

                Model.TeamNodes = _nodeService.GetAllByTeams(teamIds);
            }

            if (Model.LoginedUser.CurrentUsedTeamId > 0)
                Model.CurrentUsedTeam = Model.Teams.Where(x => x.Id == Model.LoginedUser.CurrentUsedTeamId).FirstOrDefault();

            #region RouteWriteUrl
            if (Model.GetNodeId == 0)
            {
                if (Model.MyNodes.Count > 0)
                {
                    Model.RouteWriteUrl = RouteContext.GetRouteUrl("Notes.AddContent", Model.MyNodes[0].Id);
                }
            }
            else
            {
                Model.RouteWriteUrl = RouteContext.GetRouteUrl("Notes.AddContent", Model.GetNodeId);
            }
            #endregion

            #region RouteSearchUrl
            if (Model.CurrentPage.CurrentNavNodeId > 0)
            {
                Model.RouteSearchUrl = RouteContext.GetRouteUrl("Notes.Contents", Model.GetNodeId); 
            }
            else {
                Model.RouteSearchUrl = RouteContext.GetRouteUrl("Users.Home");
            }
            #endregion
        }
    }

    public class UserModel
    {
        public UserPage CurrentPage { get; set; }

        public IList<Node> MyNodes { get; set; }

        public IList<Team> MyAdminTeams { get; set; }

        public Team CurrentUsedTeam { get; set; }

        public IList<Team> Teams { get; set; }

        public IList<Node> TeamNodes { get; set; }

        public Domain.Users.User LoginedUser { get; set; }

        public int UserHeartbeatTime { get; set; }

        /// <summary>
        /// 写笔记路由
        /// </summary>
        public string RouteWriteUrl { get; set; }

        public string RouteSearchUrl { get; set; }

        public string GetKeywords {
            get { return WebHelper.GetString("wd"); }
        }

        public int GetNodeId {
            get { return WebHelper.GetInt("nodeid", 0); }
        }
    }

}