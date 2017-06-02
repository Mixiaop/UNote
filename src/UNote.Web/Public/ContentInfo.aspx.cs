using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U;
using U.Utilities.Web;
using UNote.Domain.Notes;
using UNote.Domain.Users;
using UNote.Services.Notes;
using UNote.Web.Notes;

namespace UNote.Web.Public
{
    public partial class ContentInfo : Infrastructure.UserPage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();

        protected ContentInfoModel Model = new ContentInfoModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            Model.Content = _contentService.GetById(Model.ContentId);
            CurrentNav = Infrastructure.Navigation.UserHomePage;

            Model.LoginedUser = GetLoginedUser();
        }
    }
}