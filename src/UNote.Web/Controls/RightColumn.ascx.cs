using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U;
using UNote.Domain.Notes;
using UNote.Domain.Users;
using UNote.Services.Notes;
using UNote.Services.Users;
using UNote.Services.Authrization;

namespace UNote.Web.Controls
{
    public partial class RightColumn : Infrastructure.UserControlBase
    {
        private IAuthenticationService _authenticationService = UPrimeEngine.Instance.Resolve<IAuthenticationService>();
        private IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();

        protected RightColumnModel Model = new RightColumnModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            Model.LoginedUser = _authenticationService.GetAuthenticatedUser();
            Model.List = _contentService.QueryRecentNotes(Model.LoginedUser, 12);
        }
    }

    public class RightColumnModel {
        public IList<UNote.Domain.Notes.Content> List { get; set; }

        public User LoginedUser { get; set; }
    }
}