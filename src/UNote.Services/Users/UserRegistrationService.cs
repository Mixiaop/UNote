using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U.UI;
using U.Utilities.Security;
using UNote.Domain.Users;
using UNote.Services.Users.Dto;

namespace UNote.Services.Users
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        public UserRegistrationService(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        public bool ExistsUser(string username) {
            return _userService.ExistsUser(username);
        }

       
        public int Registration(string username, string oriPassword, string nickName, UserType userType = UserType.General)
        {
            var user = new User();
            user.Username = username;
            user.NickName = nickName;
            user.UserType = userType;
            user.Password = oriPassword;
            //user.Email = email;

            if (user.Username.IsNullOrEmpty()) {
                throw new UserFriendlyException("用户名不能为空");
            }

            if (user.Password.IsNullOrEmpty()) {
                throw new UserFriendlyException("密码不能为空");
            }

            if (user.NickName.IsNullOrEmpty())
            {
                throw new UserFriendlyException("昵称不能为空");
            }
            user.Password = EncryptPassword(user.Password);

            user.Id = _userRepository.InsertAndGetId(user);
            return user.Id;
        }

        /// <summary>
        /// 验证用户名密码是否正确
        /// </summary>
        /// <param name="username">需要验证的用户名</param>
        /// <param name="password">需要验证的密码</param>
        /// <returns></returns>
        public VerifyUserOutput Verify(string username, string password)
        {
            VerifyUserOutput result = new VerifyUserOutput();
            result.User = _userService.GetByUsername(username);
            if (result.User == null)
            {
                result.Success = false;
                result.ErrorMessage = "用户名或密码有误";
            }
            else
            {
                if (result.User.Password == EncryptPassword(password))
                {
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "用户名或密码有误";
                }
            }

            return result;
        }

        /// <summary>
        /// 登录后更新用户状态
        /// </summary>
        /// <param name="user"></param>
        public void LoginedUpdateStatus(User user) {
            if (user == null)
                throw new UserFriendlyException("user is null");
            user.LastLoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            user.LastIpAddress = U.Utilities.Web.WebHelper.GetIP();
            _userRepository.Update(user);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(User user, string oldPassword, string newPassword, string confirmPassword) {
            oldPassword = oldPassword.Trim();
            newPassword = newPassword.Trim();
            confirmPassword = confirmPassword.Trim();
            if (user == null)
                new UserFriendlyException("用户[User]不能为空");

            var verify = Verify(user.Username, oldPassword);
            if (!verify.Success) {
                new UserFriendlyException("原密码不正确");
            }

            if (newPassword != confirmPassword) {
                new UserFriendlyException("两次输入的密码不一致");
            }

            user.Password = EncryptPassword(newPassword);
            _userService.Update(user);

            return true;
        }

        #region Utilities

        public string EncryptPassword(string metaStr) {
            return EncriptionHelper.MD5(metaStr);
        }
        #endregion
    }
}
