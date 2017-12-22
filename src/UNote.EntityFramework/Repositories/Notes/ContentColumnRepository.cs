using UNote.Domain.Notes;

namespace UNote.EntityFramework.Repositories.Notes
{
    public class ContentColumnRepository : UNoteRepositoryBase<ContentColumn>, IContentColumnRepository
    {
        public ContentColumnRepository(UNodeDbContext dbContext) : base(dbContext) { }
    }
}
