using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U;
using U.Application.Services.Dto;
using UNote.Domain.Notes;
using UNote.Domain.Users;
using UNote.Services.Notes;
using UNote.Services.Users;

namespace UNote.Web
{
    public partial class Default : Infrastructure.PageBase
    {

        private INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        private ITagService _tagService = UPrimeEngine.Instance.Resolve<ITagService>();
        private IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();

        public DefaultModel Model = new DefaultModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(RouteContext.GetRouteUrl("Users.Home"));
            
            
        }
    }

    public class DefaultModel {
        
    }
}