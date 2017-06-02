using UNote.Domain.Notes;

namespace UNote.EntityFramework.Repositories.Notes
{
    public class ContentFollowerRepository : UNoteRepositoryBase<ContentFollower>, IContentFollowerRepository
    {
        public ContentFollowerRepository(UNodeDbContext dbContext) : base(dbContext) { }
    }
}
