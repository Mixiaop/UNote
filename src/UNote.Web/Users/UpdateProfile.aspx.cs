using System;
using U;
using U.UI;
using UNote.Services.Users;
using UNote.Services.Users.Dto;

namespace UNote.Web.Users
{
    public partial class UpdateProfile : Infrastructure.UserPage
    {
        IUserService _userService = UPrimeEngine.Instance.Resolve<IUserService>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var user = GetLoginedUser();
                if (user.NickName.IsNotNullOrEmpty())
                    tbNickName.Text = user.NickName;
                if (user.AvatarUrl.IsNotNullOrEmpty())
                    hfPreviewUrl.Value = user.AvatarUrl;
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            var input = new UpdateProfileInput();
            input.UserId = GetLoginedUser().Id;
            input.NickName = tbNickName.Text.Trim();
            input.AvatarId = hfPreviewId.Value.ToInt();
            input.PicX = hfPicX.Value.ToInt();
            input.PicY = hfPicY.Value.ToInt();
            input.PicW = hfPicW.Value.ToInt();
            input.PicH = hfPicH.Value.ToInt();

            if (input.NickName.IsNullOrEmpty())
            {
                ltlMessage.Text = AlertError("请至少输个昵称吧");
                return;
            }

            try
            {
                var res = _userService.UpdateProfile(input);
                if (!res.Success)
                {
                    ltlMessage.Text = AlertError(res.Errors[0]);
                    return;
                }
                else
                {
                    ltlMessage.Text = AlertSuccess("修改成功");
                    this.RedirectByTime("UpdateProfile.aspx", 500);
                }
            }
            catch (UserFriendlyException ex)
            {
                ltlMessage.Text = AlertError(ex.Message);
                return;
            }
        }


    }
}