using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U.Application.Services.Dto;
using UNote.Domain.Users;
using UNote.Services.Users.Dto;

namespace UNote.Services.Users
{
    /// <summary>
    /// “用户信息” 应用服务
    /// </summary>
    public interface IUserService : U.Application.Services.IApplicationService
    {
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        PagedResultDto<User> Query(int pageIndex = 1, int pageSize = 20, string keywords = "");

        /// <summary>
        /// 查询几条用户
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="takCount"></param>
        /// <returns></returns>
        IList<UserDto> Query(string keywords, int takCount = 5);

        /// <summary>
        /// 通过用户Id获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        User GetById(int userId);

        /// <summary>
        /// 通过用户名获取用户信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        User GetByUsername(string username);

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool ExistsUser(string username);

        void Update(User user);

        void UpdateCurrentUsedTeam(int userId, string teamKey);

        /// <summary>
        /// 删除一个用户
        /// </summary>
        /// <param name="user"></param>
        void Delete(User user);
    }
}
