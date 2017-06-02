using UNote.Domain.Notes;

namespace UNote.EntityFramework.Repositories.Notes
{
    public class ContentRepository : UNoteRepositoryBase<Content>, IContentRepository
    {
        public ContentRepository(UNodeDbContext dbContext) : base(dbContext) { }
    }
}
