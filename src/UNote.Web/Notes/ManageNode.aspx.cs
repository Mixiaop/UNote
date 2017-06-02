using System;
using System.Collections.Generic;
using U;
using UNote.Domain.Notes;
using UNote.Services.Notes;

namespace UNote.Web.Notes
{
    public partial class ManageNode : Infrastructure.UserPage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();

        protected IList<Node> MyNodeList;
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentNav = Infrastructure.Navigation.ManageNode;
            MyNodeList = _nodeService.GetAll(GetLoginedUser().Id);

        }
    }
}