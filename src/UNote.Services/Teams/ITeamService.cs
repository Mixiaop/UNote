using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U.Application.Services.Dto;
using UNote.Domain.Teams;
using UNote.Domain.Users;

namespace UNote.Services.Teams
{
    /// <summary>
    /// 团队管理应用服务
    /// </summary>
    public interface  ITeamService : U.Application.Services.IApplicationService
    {
        IList<Team> GetAllTeams(int userId, bool isAdmin = true);

        Team GetById(int teamId);

        Team GetByKey(string key);

        string Create(Team team);

        void Update(Team team);
       
        void Delete(Team team);


        #region Members
        PagedResultDto<TeamMember> QueryMembers(int teamId,int pageIndex = 1, int pageSize = 10);

        IList<TeamMember> GetAllMembers(int teamId);

        bool ExistsMember(int teamId, int userId);

        void AddMember(Team team, int userId);
        #endregion
    }
}
