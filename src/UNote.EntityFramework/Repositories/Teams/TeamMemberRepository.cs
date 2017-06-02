using UNote.Domain.Teams;
namespace UNote.EntityFramework.Repositories.Teams
{
    public class TeamMemberRepository : UNoteRepositoryBase<TeamMember>, ITeamMemberRepository
    {
        public TeamMemberRepository(UNodeDbContext dbContext) : base(dbContext) { }
    }
}


