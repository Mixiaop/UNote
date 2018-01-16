using System;
using U;
using U.UI;
using UNote.Domain.Users;
using UNote.Services.Users;

namespace UNote.Web.Users
{
    public partial class UpdateProfile : Infrastructure.UserPage
    {

        IUserService _userService = UPrimeEngine.Instance.Resolve<IUserService>();

        protected User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                currentUser = GetLoginedUser();
                tbNickName.Text = currentUser.NickName;
                hfPreviewUrl.Value = currentUser.AvatarUrl;
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            currentUser = GetLoginedUser();

            var _hfPicId = hfPreviewId.Value.ToInt();
            var _hfPicX = hfPicX.Value.ToInt();
            var _hfPicY = hfPicY.Value.ToInt();
            var _hfPicW = hfPicW.Value.ToInt();
            var _hfPicH = hfPicH.Value.ToInt();

            var nickName = tbNickName.Text.Trim();

            if (nickName.IsNullOrEmpty())
            {
                ltlMessage.Text = AlertError("请至少输个昵称吧");
                return;
            }

            currentUser.NickName = nickName;

            try
            {
                _userService.Update(currentUser);
                if (_hfPicId >0)
                {
                    _userService.UpdateAvatar(_hfPicId, _hfPicX, _hfPicY, _hfPicW, _hfPicH);
                }
                ltlMessage.Text = AlertSuccess("修改成功");
                this.RedirectByTime("UpdateProfile.aspx", 500);
            }
            catch (UserFriendlyException ex)
            {
                ltlMessage.Text = AlertError(ex.Message);
                return;
            }
        }

       
    }
}