using System;
using U;
using U.Utilities.Web;
using UNote.Domain.Users;
using UNote.Services.Authrization;
using UNote.Services.Notes;

namespace UNote.Web.Users
{
    public partial class UserInfo : Infrastructure.UserControlBase
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();
        IAuthenticationService _authenticationService = UPrimeEngine.Instance.Resolve<IAuthenticationService>();

        protected User LoginedUser;
        protected int ContentTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoginedUser = _authenticationService.GetAuthenticatedUser();
            
            ContentTotal = _contentService.Count(LoginedUser.Id);

            if (NodeId == 0) {
                var nodes = _nodeService.GetAll(LoginedUser.Id);
                if (nodes.Count > 0)
                    NodeId = nodes[0].Id;
            }
        }

        private int _nodeId = WebHelper.GetInt("nodeid", 0);
        public int NodeId
        {
            get { return _nodeId; }
            set { _nodeId = value; }
        }
    }


}