using System;
using System.Collections.Generic;
using U;
using U.Utilities.Web;
using UNote.Domain.Notes;
using UNote.Domain.Teams;
using UNote.Services.Teams;
using UNote.Services.Notes;

namespace UNote.Web.Teams
{
    public partial class Nodes : Infrastructure.UserPage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        ITeamService _teamService = UPrimeEngine.Instance.Resolve<ITeamService>();

        protected NodesModel Model = new NodesModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentNav = Infrastructure.Navigation.Team;
            CurrentNavTeamKey = Model.TeamKey;
            tabMenu.SelectedTab = TeamTab.Nodes;
            Model.Team = _teamService.GetByKey(Model.TeamKey);
            if (Model.Team == null)
                Invoke404();

            Model.NodeList = _nodeService.GetAllByTeam(Model.Team.Id);
        }
    }

    public class NodesModel
    {
        public string TeamKey
        {
            get { return WebHelper.GetString("key"); }
        }

        public Team Team { get; set; }


        public IList<Node> NodeList { get; set; }
    }
}