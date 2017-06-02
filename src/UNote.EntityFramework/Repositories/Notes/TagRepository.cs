using UNote.Domain.Notes;

namespace UNote.EntityFramework.Repositories.Notes
{
    public class TagRepository : UNoteRepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(UNodeDbContext dbContext) : base(dbContext) { }
    }
}
