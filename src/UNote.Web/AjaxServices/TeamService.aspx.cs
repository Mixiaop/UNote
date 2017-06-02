using System;
using System.Collections.Generic;
using AjaxPro;
using U;
using U.Web.Models;
using U.AutoMapper;
using UNote.Domain.Teams;
using UNote.Services.Teams;
using UNote.Services.Users;
using UNote.Services.Users.Dto;

namespace UNote.Web.AjaxServices
{
    [AjaxNamespace("TeamService")]
    public partial class TeamService : Infrastructure.AjaxPageBase
    {
        IUserService _userService = UPrimeEngine.Instance.Resolve<IUserService>();
        ITeamService _teamService = UPrimeEngine.Instance.Resolve<ITeamService>();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamKey"></param>
        /// <param name="userIds">Id之间用空格“ ”分割</param>
        /// <returns></returns>
        [AjaxMethod]
        public AjaxResponse AddMembers(string teamKey, string userIds)
        {
            AjaxResponse res = new AjaxResponse();

            try
            {
                userIds = userIds.Trim();
                if (userIds.IsNotNullOrEmpty())
                {
                    string[] ids = userIds.Split(' ');

                    var team = _teamService.GetByKey(teamKey);
                    if (team == null)
                        throw new Exception("team is null");

                    foreach (var userId in ids)
                    {
                        _teamService.AddMember(team, userId.ToInt());
                    }
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }

        [AjaxMethod]
        public AjaxResponse<IList<UserDto>> GetAllMembers(int teamId) {
            AjaxResponse<IList<UserDto>> res = new AjaxResponse<IList<UserDto>>();
            var members = _teamService.GetAllMembers(teamId);
            res.Result = new List<UserDto>();
            if (members != null) {
                foreach (var member in members) {
                    var user = member.User.MapTo<UserDto>();
                    res.Result.Add(user);
                }
            }

            return res;
        }
    }
}