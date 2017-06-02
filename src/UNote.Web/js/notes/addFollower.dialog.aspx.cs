using System;
using System.Collections.Generic;
using AjaxPro;
using U;
using U.Web.Models;
using U.AutoMapper;
using U.Utilities.Web;
using UNote.Domain.Teams;
using UNote.Services.Teams;
using UNote.Services.Users;
using UNote.Services.Users.Dto;

namespace UNote.Web.js.notes
{
    public partial class addFollower_dialog : System.Web.UI.Page
    {
        IUserService _userService = UPrimeEngine.Instance.Resolve<IUserService>();
        ITeamService _teamService = UPrimeEngine.Instance.Resolve<ITeamService>();

        protected IList<TeamMember> Members;
        protected void Page_Load(object sender, EventArgs e)
        {
            var teamId = WebHelper.GetInt("teamId", 0);
            if (teamId > 0) {
                Members = _teamService.GetAllMembers(teamId);
            }
        }
    }
}