using System;
using U;
using U.Utilities.Web;
using UNote.Configuration;
using UNote.Services.Users;
using UNote.Services.Authrization;

namespace UNote.Web
{
    /// <summary>
    /// 外部用户登录
    /// </summary>
    public partial class ExternalLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var settings = UPrimeEngine.Instance.Resolve<UNoteSettings>();
            var userService = UPrimeEngine.Instance.Resolve<IUserService>();
            var authenticationService = UPrimeEngine.Instance.Resolve<IAuthenticationService>();

            //已登录
            if (authenticationService.GetAuthenticatedUser() != null)
            {
                Response.Redirect("/");
                Response.End();
            }

            var appKey = WebHelper.GetString("appKey");
            var username = WebHelper.GetString("username");
            if (appKey.IsNotNullOrEmpty() && username.IsNotNullOrEmpty())
            {
                if (appKey != settings.ExternalLoginAppKey) {
                    Response.Write("appKey error");
                    Response.End();
                }
                var user = userService.GetByUsername(username);
                if (user != null) {
                    authenticationService.SignIn(user, true);
                    Response.Redirect("/");
                }
            }
            else {
                Response.Write("Failed");
            }
        }
    }
}