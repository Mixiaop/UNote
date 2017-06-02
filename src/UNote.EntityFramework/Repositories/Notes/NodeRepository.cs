using UNote.Domain.Notes;

namespace UNote.EntityFramework.Repositories.Notes
{
    public class NodeRepository : UNoteRepositoryBase<Node>, INodeRepository
    {
        public NodeRepository(UNodeDbContext dbContext) : base(dbContext) { }
    }
}
