using System;
using U;
using UNote.Services.Users;

namespace UNote.Console.Users
{
    public partial class User_Add : Infrastructure.UNotePage
    {
        IUserRegistrationService _userRegistrationService = UPrimeEngine.Instance.Resolve<IUserRegistrationService>();
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Click += btnSave_Click;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _userRegistrationService.Registration(tbUsername.Text.Trim(),
                    tbPassword.Text.Trim(), 
                    tbNickName.Text.Trim());

                ltlMessage.Text = AlertSuccess("添加新用户成功");
                this.RedirectByTime("User_List.aspx", 1000);
            }
            catch (Exception ex)
            {
                ltlMessage.Text = AlertError(ex.Message);
            }
        }
    }
}