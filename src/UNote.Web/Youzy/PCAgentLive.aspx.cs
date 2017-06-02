using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UNote.Web.Youzy
{
    public partial class PCAgentLive : Infrastructure.UserPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearUserPage = true;
        }
    }
}