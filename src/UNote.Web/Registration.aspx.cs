using System;
using Newtonsoft.Json;
using U;
using U.Utilities.Web;
using UNote.Configuration;
using UNote.Services.Users;
using UNote.Services.Authrization;

namespace UNote.Web
{
    public partial class Registration : System.Web.UI.Page
    {
        private IUserService _userService = UPrimeEngine.Instance.Resolve<IUserService>();
        private IUserRegistrationService _userRegistrationService = UPrimeEngine.Instance.Resolve<IUserRegistrationService>();
        private IAuthenticationService _authenticationService = UPrimeEngine.Instance.Resolve<IAuthenticationService>();

        protected string ErrorMessage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            btnRegister.ServerClick += btnRegister_ServerClick;
        }

        void btnRegister_ServerClick(object sender, EventArgs e)
        {
            #region 注册
            string username = tbUsername.Text.Trim();
            string password = tbPassword.Text.Trim();
            string password2 = tbPassword2.Text.Trim();
            string nickName = tbNickName.Text.Trim();

            if (username.IsNullOrEmpty()) {
                ErrorMessage = "请输入邮箱（用户名）";
                return;
            }
            ErrorMessage = "";

            if (!username.IsValidEmail())
            {
                ErrorMessage = "邮箱格式有误";
                return;
            }
            ErrorMessage = "";

            if (_userRegistrationService.ExistsUser(username)) {
                ErrorMessage = "邮箱已存在";
                return;
            }
            ErrorMessage = "";

            if (password.IsNullOrEmpty()) {
                ErrorMessage = "请输入密码";
                return;
            }

            if (password.Length < 6) {
                ErrorMessage = "密码必须大于6位";
                return;
            }

            if (password != password2) {
                ErrorMessage = "两次输入的密码不一致";
                return;
            }

            if (nickName.IsNullOrEmpty()) {
                ErrorMessage = "请输入昵称";
                return;
            }

            try
            {
                var userId = _userRegistrationService.Registration(username, password, nickName, UserType.Email);
                if (userId > 0)
                {
                    var user = _userService.GetById(userId);
                    if (user != null)
                    {
                        _authenticationService.SignIn(user, true);
                        Response.Redirect("/");
                    }
                }
                else {
                    ErrorMessage = "注册失败";
                    return;
                }
            }
            catch (Exception ex) {
                ErrorMessage = ex.Message;
            }

            #endregion
        }
    }
}