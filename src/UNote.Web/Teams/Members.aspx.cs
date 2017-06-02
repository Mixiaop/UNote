using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U;
using U.Utilities.Web;
using U.Application.Services.Dto;
using UNote.Configuration;
using UNote.Domain.Teams;
using UNote.Services.Teams;
using UNote.Web.Infrastructure;

namespace UNote.Web.Teams
{
    public partial class Members : Infrastructure.UserPage
    {
        ITeamService _teamService = UPrimeEngine.Instance.Resolve<ITeamService>();

        protected MembersModel Model = new MembersModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentNav = Infrastructure.Navigation.Team;
            CurrentNavTeamKey = Model.TeamKey;
            tabMenu.SelectedTab = TeamTab.Members;
            Model.Settings = UPrimeEngine.Instance.Resolve<UNoteSettings>();

            Model.Team = _teamService.GetByKey(Model.TeamKey);
            if (!IsPostBack)
            {
                PagingInfo pageInfo = new PagingInfo();
                pageInfo.PageIndex = WebHelper.GetInt("page", 1);
                pageInfo.PageSize = 100;
                pageInfo.Url = WebHelper.GetUrl();

                var datas = _teamService.QueryMembers(Model.Team.Id, pageInfo.PageIndex, pageInfo.PageSize);

                Model.Members = datas;

                pageInfo.TotalCount = datas.TotalCount;
                Model.PagiHtml = new Paginations(pageInfo).GetPaging();
            }
        }
    }

    public class MembersModel
    {
        public string TeamKey
        {
            get { return WebHelper.GetString("key"); }
        }

        public Team Team { get; set; }

        public PagedResultDto<TeamMember> Members { get; set; }

        public string PagiHtml { get; set; }

        public UNoteSettings Settings { get; set; }

    }
}