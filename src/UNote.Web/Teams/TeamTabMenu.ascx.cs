using System;
using U.Utilities.Web;

namespace UNote.Web.Teams
{
    public partial class TeamTabMenu : Infrastructure.UserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private TeamTab _selectedTab = TeamTab.Info;
        public TeamTab SelectedTab
        {
            get {
                return _selectedTab;
            }
            set { _selectedTab = value; }
        }

        public string TeamKey
        {
            get { return WebHelper.GetString("key"); }
        }
    }

    public enum TeamTab { 
        Info,
        Members,
        Nodes
    }
}