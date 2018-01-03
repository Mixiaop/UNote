using System;
using System.Linq;
using System.Collections.Generic;
using U.UI;
using U.Application.Services.Dto;
using UNote.Domain.Teams;
using UNote.Domain.Users;

namespace UNote.Services.Teams
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamMemberRepository _teamMemberRepository;
        public TeamService(ITeamRepository teamRepository, ITeamMemberRepository teamMemberRepository)
        {
            _teamRepository = teamRepository;
            _teamMemberRepository = teamMemberRepository;
        }

        public IList<Team> GetAllTeams(int userId, bool isAdmin = true)
        {
            var query = _teamMemberRepository.GetAll();
            query = query.Where(x => x.UserId == userId);
            if (isAdmin)
                query = query.Where(x => x.IsAdmin == true);
            var list = query.OrderByDescending(x => x.CreationTime).ToList();
            List<Team> result = new List<Team>();
            foreach (var info in list)
                result.Add(info.Team);

            return result;
        }

        public Team GetById(int teamId)
        {
            return _teamRepository.Get(teamId);
        }

        public Team GetByKey(string key)
        {
            var team = _teamRepository.GetAll().Where(x => x.Key == key).FirstOrDefault();
            return team;
        }

        public string Create(Team team)
        {
            if (team == null)
                throw new UserFriendlyException("team is null");

            if (team.Name.IsNullOrEmpty())
            {
                throw new UserFriendlyException("团队名称不能为空");
            }
            team.Key = Guid.NewGuid().ToString().Replace("-", "");
            if (team.CommanderId > 0)
                team.MemberCount = 1;
            var teamId = _teamRepository.InsertAndGetId(team);
            //添加团队负责人成员 
            if (team.CommanderId > 0)
            {
                TeamMember member = new TeamMember();
                member.UserId = team.CommanderId;
                member.TeamId = teamId;
                member.IsAdmin = true;
                _teamMemberRepository.Insert(member);
            }
            return team.Key;
        }

        public void Update(Team team)
        {
            _teamRepository.Update(team);
        }

        public void Delete(Team team)
        {
            _teamRepository.Delete(team);
        }

        #region Members
        public PagedResultDto<TeamMember> QueryMembers(int teamId, int pageIndex = 1, int pageSize = 10)
        {
            var query = _teamMemberRepository.GetAll();
            query = query
                .Where(x => x.TeamId == teamId);

            int count = query.Count();

            query = query.OrderByDescending(x => x.IsAdmin)
                         .Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var list = query.ToList();

            return new PagedResultDto<TeamMember>(count, list);
        }

        public IList<TeamMember> GetAllMembers(int teamId)
        {
            var query = _teamMemberRepository.GetAll();
            query = query
                    .Where(x => x.TeamId == teamId && x.User != null)
                    .OrderByDescending(x => x.IsAdmin);

            var list = query.ToList();

            return list;
        }

        public bool ExistsMember(int teamId, int userId)
        {
            var query = _teamMemberRepository.GetAll()
                        .Where(x => x.TeamId == teamId && x.UserId == userId);
            return query.Count() > 0;
        }

        public void AddMember(Team team, int userId)
        {
            if (team == null)
                throw new UserFriendlyException("team is null");

            if (!ExistsMember(team.Id, userId))
            {
                TeamMember member = new TeamMember();
                member.TeamId = team.Id;
                member.UserId = userId;
                _teamMemberRepository.Insert(member);

                team.MemberCount++;
                Update(team);
            }
        }
        #endregion
    }
}
