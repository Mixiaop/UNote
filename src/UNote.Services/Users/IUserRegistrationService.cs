using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNote.Domain.Users;
using UNote.Services.Users.Dto;

namespace UNote.Services.Users
{
    /// <summary>
    /// “用户注册”应用服务
    /// </summary>
    public interface IUserRegistrationService : U.Application.Services.IApplicationService
    {
        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool ExistsUser(string username);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="oriPassword">原始密码（未加密的）</param>
        /// <param name="nickName">昵称</param>
        /// <param name="userType">用户类型</param>
        /// <param name="email"></param>
        /// <returns></returns>
        int Registration(string username, string oriPassword, string nickName, UserType userType = UserType.General);

        /// <summary>
        /// 验证用户名密码是否正确
        /// </summary>
        /// <param name="username">需要验证的用户名</param>
        /// <param name="password">需要验证的密码</param>
        /// <returns></returns>
        VerifyUserOutput Verify(string username, string password);

        /// <summary>
        /// 登录后更新用户状态
        /// </summary>
        /// <param name="user"></param>
        void LoginedUpdateStatus(User user);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        bool ChangePassword(User user, string oldPassword, string newPassword, string confirmPassword);
    }
}
