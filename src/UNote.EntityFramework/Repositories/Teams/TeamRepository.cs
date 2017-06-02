using UNote.Domain.Teams;
namespace UNote.EntityFramework.Repositories.Teams
{
    public class TeamRepository : UNoteRepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(UNodeDbContext dbContext) : base(dbContext) { }
    }
}


