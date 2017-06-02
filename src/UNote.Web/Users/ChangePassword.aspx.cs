using System;
using U;
using U.UI;
using UNote.Services.Users;

namespace UNote.Web.Users
{
    public partial class ChangePassword : Infrastructure.UserPage
    {
        IUserRegistrationService _userRegistrationService = UPrimeEngine.Instance.Resolve<IUserRegistrationService>();
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Click += btnSave_Click;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            var oldPassword = tbOldPassword.Text.Trim();
            var password = tbPassword.Text.Trim();
            var password2 = tbPassword2.Text.Trim();

            if (oldPassword.IsNullOrEmpty()) {
                ltlMessage.Text = AlertError("请输入原密码");
                return;
            }

            if (password.IsNullOrEmpty())
            {
                ltlMessage.Text = AlertError("请输入新密码");
                return;
            }

            if (password!=password2)
            {
                ltlMessage.Text = AlertError("两次输入的密码不一致");
                return;
            }

            try
            {
                _userRegistrationService.ChangePassword(GetLoginedUser(), oldPassword, password, password2);
                ltlMessage.Text = AlertSuccess("修改成功");
                this.RedirectByTime("/Logout.aspx?goUrl=/Login.aspx", 500);
            }
            catch (UserFriendlyException ex) {
                ltlMessage.Text = AlertError(ex.Message);
                return;
            }
        }
    }
}