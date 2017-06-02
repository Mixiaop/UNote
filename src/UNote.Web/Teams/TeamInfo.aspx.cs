using System;
using U;
using U.UI;
using U.Utilities.Web;
using UNote.Domain.Teams;
using UNote.Services.Teams;


namespace UNote.Web.Teams
{
    public partial class TeamInfo : Infrastructure.UserPage
    {
        ITeamService _teamService = UPrimeEngine.Instance.Resolve<ITeamService>();

        protected TeamInfoModel Model = new TeamInfoModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Click += btnSave_Click;

            CurrentNav = Infrastructure.Navigation.Team;
            CurrentNavTeamKey = Model.TeamKey;
            tabMenu.SelectedTab = TeamTab.Info;
            Model.Team = _teamService.GetByKey(Model.TeamKey);
            if (Model.Team == null)
                Invoke404();

            if (!IsPostBack) {
                tbName.Text = Model.Team.Name;
                tbIntroduction.Text = Model.Team.Introduction;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            #region 保存信息
            string name = tbName.Text.Trim();
            string introduction = tbIntroduction.Text.Trim();

            try
            {
                if (name.IsNullOrEmpty())
                {
                    ltlMessage.Text = AlertError("请输入团队名称");
                    return;
                }
                
                Model.Team.Name = name;
                Model.Team.Introduction = introduction;
                
                _teamService.Update(Model.Team);
                ltlMessage.Text = AlertSuccess("保存成功", "", 2000);
            }
            catch (UserFriendlyException ex)
            {
                ltlMessage.Text = AlertError(ex.Message);
            }
            #endregion
        }
    }

    public class TeamInfoModel
    {
        public Team Team { get; set; }

        public string TeamKey
        {
            get { return WebHelper.GetString("key"); }
        }
    }
}