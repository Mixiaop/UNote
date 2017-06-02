using System;
using System.Web;
using System.Web.Security;
using UNote.Services.Users;
using UNote.Domain.Users;

namespace UNote.Services.Authrization
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        #region Fields & Ctor
        private readonly IUserService _userService;
        private readonly TimeSpan _expirationTimeSpan;
        private User _cachedUser;
        public FormsAuthenticationService(IUserService userService)
        {
            _userService = userService;
            _expirationTimeSpan = FormsAuthentication.Timeout;
        }
        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="createPersistentCookie">是否创建固定的cookie</param>
        public void SignIn(User user, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();
            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                user.Username,
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                user.Id.ToString(),
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
            _cachedUser = user;
        }

        /// <summary>
        /// 登出
        /// </summary>
        public void SignOut()
        {
            _cachedUser = null;
            FormsAuthentication.SignOut();
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取已经通过身份证证的用户
        /// </summary>
        /// <returns></returns>
        public User GetAuthenticatedUser()
        {
            if (_cachedUser != null)
                return _cachedUser;

            if (HttpContext.Current == null ||
                HttpContext.Current.Request == null ||
                !HttpContext.Current.Request.IsAuthenticated ||
                !(HttpContext.Current.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)HttpContext.Current.User.Identity;
            var user = GetAuthenticatedAdminFromTicket(formsIdentity.Ticket);
            if (user != null)
                _cachedUser = user;

            return _cachedUser;
        }

        private User GetAuthenticatedAdminFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            var userId = ticket.UserData;

            if (string.IsNullOrWhiteSpace(userId))
                return null;

            if (userId.ToInt() > 0)
            {
                var user = _userService.GetById(userId.ToInt());
                return user;
            }
            else
                return null;
        }
    }
}
