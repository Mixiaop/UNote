using System;
using System.Collections.Generic;
using AjaxPro;
using U;
using U.Web.Models;
using UNote.Services.Authrization;
using UNote.Services.Users;
using UNote.Services.Users.Dto;

namespace UNote.Web.AjaxServices
{
    [AjaxNamespace("UserService")]
    public partial class UserService : Infrastructure.AjaxPageBase
    {
        IUserService _userService = UPrimeEngine.Instance.Resolve<IUserService>();
        IOnlineUserService _onlineUserService = UPrimeEngine.Instance.Resolve<IOnlineUserService>();
        IAuthenticationService _authenticationService = UPrimeEngine.Instance.Resolve<IAuthenticationService>();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [AjaxMethod]
        public AjaxResponse<IList<UserDto>> Query(string keywords)
        {
            AjaxResponse<IList<UserDto>> res = new AjaxResponse<IList<UserDto>>();

            try
            {
                var users = _userService.Query(keywords);

                res.Result = users;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }

        [AjaxMethod]
        public AjaxResponse Heartbeat() {
            AjaxResponse res = new AjaxResponse();
            _onlineUserService.Heartbeat();
            return res;
        }

        [AjaxMethod]
        public AjaxResponse CurrentUsedTeam(string teamKey) {
            AjaxResponse res = new AjaxResponse();
            var user =_authenticationService.GetAuthenticatedUser();
            if (teamKey.IsNotNullOrEmpty() && user != null)
            {
                _userService.UpdateCurrentUsedTeam(user.Id, teamKey);
            }
            return res;
        }
    }
}