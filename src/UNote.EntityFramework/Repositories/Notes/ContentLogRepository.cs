using UNote.Domain.Notes;

namespace UNote.EntityFramework.Repositories.Notes
{
    public class ContentLogRepository : UNoteRepositoryBase<ContentLog>, IContentLogRepository
    {
        public ContentLogRepository(UNodeDbContext dbContext) : base(dbContext) { }
    }
}
