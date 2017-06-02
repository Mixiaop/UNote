using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U;
using U.UI;
using UNote.Domain.Teams;
using UNote.Services.Teams;

namespace UNote.Web.Teams
{
    public partial class CreateNewTeam : Infrastructure.UserPage
    {
        ITeamService _teamService = UPrimeEngine.Instance.Resolve<ITeamService>();

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentNav = Infrastructure.Navigation.CreateNewTeam;
            btnSave.Click += btnSave_Click;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            #region 新建团队
            string name = tbName.Text.Trim();
            string introduction = tbIntroduction.Text.Trim();

            try
            {
                if (name.IsNullOrEmpty()) {
                    ltlMessage.Text = AlertError("请输入团队名称");
                    return;
                }
                Team team = new Team();
                team.Name = name;
                team.Introduction = introduction;
                team.CommanderId = GetLoginedUser().Id;
                string teamKey = _teamService.Create(team);
                ltlMessage.Text = AlertSuccess(team.Name + " 团队新建成功");
                this.RedirectByTime(RouteContext.GetRouteUrl("Teams.Members", teamKey), 500);
            }
            catch (UserFriendlyException ex)
            {
                ltlMessage.Text = AlertError(ex.Message);
            }
            #endregion
        }
    }
}