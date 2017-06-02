using U.Application.Services;
using UNote.Domain.Users;

namespace UNote.Services.Authrization
{
    public interface IAuthenticationService : IApplicationService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="createPersistentCookie">是否创建固定的cookie</param>
        void SignIn(User user, bool createPersistentCookie);


        /// <summary>
        /// 登出
        /// </summary>
        void SignOut();


        /// <summary>
        /// 获取已经通过身份证证的用户
        /// </summary>
        /// <returns></returns>
        User GetAuthenticatedUser();
    }
}
