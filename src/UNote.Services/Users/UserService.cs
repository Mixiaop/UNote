using System.Collections.Generic;
using System.Linq;
using U.AutoMapper;
using U.UI;
using U.Application.Services.Dto;
using UNote.Domain.Users;
using UNote.Services.Users.Dto;
using UNote.Services.Teams;

namespace UNote.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeamService _teamService;
        public UserService(IUserRepository userRepository, ITeamService teamService)
        {
            _userRepository = userRepository;
            _teamService = teamService;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public PagedResultDto<User> Query(int pageIndex = 1, int pageSize = 20, string keywords = "")
        {
            var query = _userRepository.GetAll();
            if (keywords.IsNotNullOrEmpty())
            {
                query = query.Where(x => x.Username.Contains(keywords) || x.NickName.Contains(keywords));
            }



            var list = query.OrderByDescending(x => x.CreationTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var count = query.Count();
            return new PagedResultDto<User>(count, (List<User>)list);
        }

        /// <summary>
        /// 查询几条用户
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="takCount"></param>
        /// <returns></returns>
        public IList<UserDto> Query(string keywords, int takCount = 5)
        {
            var query = _userRepository.GetAll();
            if (keywords.IsNotNullOrEmpty())
            {
                query = query.Where(x => x.Username.Contains(keywords) || x.NickName.Contains(keywords));
            }

            var list = query.OrderByDescending(x => x.CreationTime).Take(takCount).ToList();
            return list.MapTo<IList<UserDto>>();
        }

        /// <summary>
        /// 通过用户Id获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetById(int userId)
        {
            var user = _userRepository.Get(userId);
            if (user == null)
                throw new UserFriendlyException("user is null");

            return user;
        }

        /// <summary>
        /// 通过用户名获取用户信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public User GetByUsername(string username)
        {
            var user = _userRepository.GetAll().Where(x => x.Username == username).FirstOrDefault();
            if (user == null)
                throw new UserFriendlyException("user is null");

            return user;
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool ExistsUser(string username)
        {
            var count = _userRepository.Count(x => x.Username == username);
            return count > 0;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        public void Update(User user)
        {
            if (user != null)
                _userRepository.Update(user);
        }

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="picX"></param>
        /// <param name="picY"></param>
        /// <param name="picW"></param>
        /// <param name="picH"></param>
        public void UpdateAvatar(int pictureId, int picX, int picY, int picW, int picH)
        {
            //_userRepository.PictureCut(pictureId, picX, picY, picW, picH);
        }

        /// <summary>
        /// 图片裁切
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public void PictureCut(int pictureId, int x, int y, int w, int h)
        {
            SortedDictionary<string, string> parms = new SortedDictionary<string, string>();
            parms.Add("pictureId", pictureId.ToString());
            parms.Add("x", x.ToString());
            parms.Add("y", y.ToString());
            parms.Add("w", w.ToString());
            parms.Add("h", h.ToString());

            //var message = _webRequest.CreateRequestAsGet(
            //    GetUrl("pictures/cut"),
            //    parms, _signKey);

            //return ToObject<PictureDto>(message);
        }


        public void UpdateCurrentUsedTeam(int userId, string teamKey)
        {
            var user = GetById(userId);
            if (teamKey.IsNotNullOrEmpty())
            {
                if (teamKey == "0" || teamKey == "-1")
                {
                    user.CurrentUsedTeamId = 0;
                }
                else
                {
                    var team = _teamService.GetByKey(teamKey);
                    if (team != null)
                    {
                        user.CurrentUsedTeamId = team.Id;
                    }
                    else {
                        user.CurrentUsedTeamId = 0;
                    }
                }
                _userRepository.Update(user);
            }

        }



        /// <summary>
        /// 删除一个用户
        /// </summary>
        /// <param name="user"></param>
        public void Delete(User user)
        {
            if (user == null)
                throw new UserFriendlyException("user is null");

            _userRepository.Delete(user);
        }
    }
}
