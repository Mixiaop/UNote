using System;
using Newtonsoft.Json;
using U;
using U.Utilities.Web;
using UNote.Configuration;
using UNote.Services.Users;
using UNote.Services.Authrization;

namespace UNote.Web
{
    public partial class Login : Infrastructure.PageBase
    {
        private UNoteSettings _settings = UPrimeEngine.Instance.Resolve<UNoteSettings>();
        private IUserRegistrationService _userRegistrationService = UPrimeEngine.Instance.Resolve<IUserRegistrationService>();
        private IAuthenticationService _authenticationService = UPrimeEngine.Instance.Resolve<IAuthenticationService>();

        protected string ErrorMessage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            btnLogin.ServerClick += btnLogin_Click;
            if (IsLogined())
                Response.Redirect("/Users/Default.aspx");

            if (_settings.ExternalLoginOpened && _settings.ExternalLoginUrl.IsNotNullOrEmpty()) {
                Response.Redirect(string.Format("{0}?unote=1", _settings.ExternalLoginUrl));
                Response.End();
            }

            if (!IsPostBack)
            {
                //获取记住的用户
                var rememberUser = GetCookieUser();
                if (rememberUser != null)
                {
                    cbRememberMe.Checked = true;
                    tbUsername.Text = rememberUser.Username;
                    //tbPassword.Text = rememberUser.Password;
                }
            }

        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            ErrorMessage = "";
            var username = tbUsername.Text.Trim();
            var password = tbPassword.Text.Trim();
            if (username.IsNullOrEmpty())
            {
                ErrorMessage = "请输入用户名";
                return;
            }

            if (password.IsNullOrEmpty())
            {
                ErrorMessage = "请输入密码";
                return;
            }

            try
            {
                if (cbRememberMe.Checked)
                {
                    //记住我
                    SetCookieUser(username, password);
                }
                else
                {
                    //清除
                    ClearCookieUser();
                }

                var result = _userRegistrationService.Verify(username, password);
                if (result.Success)
                {
                    _userRegistrationService.LoginedUpdateStatus(result.User);
                    _authenticationService.SignIn(result.User, true);
                    if (WebHelper.GetString("gourl").IsNotNullOrEmpty())
                    {
                        Response.Redirect(WebHelper.GetString("gourl"));
                    }
                    else
                    {
                        Response.Redirect("/");
                    }
                }
                else
                {
                    ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "用户名或密码有误【" + ex.Message + "】";
            }

        }

        #region CookieUser
        string COOKIE_USER_NAME = "UNOTE_RememberUser";
        public class RememberUser
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        void SetCookieUser(string username, string password)
        {
            if (username.IsNotNullOrEmpty() && password.IsNotNullOrEmpty())
            {
                var settings = UPrimeEngine.Instance.Resolve<UNoteSettings>();
                var expiresTime = DateTime.Now.AddMinutes(settings.RememberUserExpiresTime);
                var user = new RememberUser();
                user.Username = username;
                user.Password = password;
                var value = JsonConvert.SerializeObject(user).EncodeUTF8Base64();
                CookieHelper.SetCookie(COOKIE_USER_NAME, value, expiresTime);
            }
        }

        RememberUser GetCookieUser()
        {
            var value = CookieHelper.GetCookieValue(COOKIE_USER_NAME);
            if (value.IsNotNullOrEmpty())
                return JsonConvert.DeserializeObject<RememberUser>(value.DecodeUTF8Base64());
            else
                return null;
        }

        void ClearCookieUser()
        {
            CookieHelper.ClearCookie(COOKIE_USER_NAME);
        }
        #endregion
    }
}