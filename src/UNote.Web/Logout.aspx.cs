using System;
using U;
using U.Utilities.Web;
using UNote.Services.Authrization;

namespace UNote.Web
{
    public partial class Logout : Infrastructure.PageBase
    {
        private IAuthenticationService _authenticationService = UPrimeEngine.Instance.Resolve<IAuthenticationService>();
        protected void Page_Load(object sender, EventArgs e)
        {
            string goUrl = WebHelper.GetString("gourl");

            _authenticationService.SignOut();
            if (!string.IsNullOrEmpty(goUrl))
                Response.Redirect(goUrl);
            else
                Response.Redirect("/");
        }
    }
}