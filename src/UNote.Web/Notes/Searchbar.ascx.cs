using System;
using System.ComponentModel;
using U;
using U.Utilities.Web;
using UNote.Domain.Users;
using UNote.Services.Authrization;
using UNote.Services.Notes;

namespace UNote.Web.Notes
{
    public partial class Searchbar : Infrastructure.UserControlBase
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();
        IAuthenticationService _authenticationService = UPrimeEngine.Instance.Resolve<IAuthenticationService>();

        protected User LoginedUser;
        protected int ContentTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            WriteUrl = Routes.Nodes_AddContent + "?nodeid=";

            LoginedUser = _authenticationService.GetAuthenticatedUser();

            if (NodeId == 0)
            {
                var nodes = _nodeService.GetAll(LoginedUser.Id);
                if (nodes.Count > 0)
                {
                    WriteUrl += nodes[0].Id;
                }
            }
            else {
                WriteUrl += NodeId.ToString();
            }
        }

        public string WriteUrl;

        public string Keywords
        {
            get { return WebHelper.GetString("wd"); }
        }

        private string _searchUrl = "";
        [Browsable(true)]
        public string SearchUrl
        {
            get
            {
                if (_searchUrl.IsNullOrEmpty())
                {
                    _searchUrl = WebHelper.GetThisPageUrl(false);
                }

                if (NodeId > 0)
                {
                    _searchUrl += "?nodeid=" + NodeId + "&wd=";
                }
                else
                {
                    _searchUrl += "?wd=";
                }

                return _searchUrl;
            }
            set { _searchUrl = value; }
        }

        private int _nodeId = 0;
        [Browsable(true)]
        public int NodeId
        {
            get
            {
                if (_nodeId == 0)
                    _nodeId = WebHelper.GetInt("nodeid", 0);

                return _nodeId;
            }
            set { _nodeId = value; }
        }
    }

}