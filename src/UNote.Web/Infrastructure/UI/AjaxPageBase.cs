using U;
using U.Web.Models;
using UNote.Domain.Users;
using UNote.Services.Authrization;

namespace UNote.Web.Infrastructure
{
    public class AjaxPageBase : PageBase
    {
        private User _cachedUser = null;
        
        public AjaxPageBase()
        { 
        
        }

        /// <summary>
        /// 获取已登录用户信息
        /// </summary>
        /// <returns></returns>
        public User GetLoginedUser()
        {
            if (_cachedUser == null)
                _cachedUser = authenticationService.GetAuthenticatedUser();

            return _cachedUser;
        }

        public AjaxResponse<T> GetUserTimeoutErrorInfo<T>() {
            AjaxResponse<T> error = new AjaxResponse<T>();
            error.Success = false;
            error.Error = new ErrorInfo("用户信息已超时，请重新登录");
            return error;
        }
    }
}
