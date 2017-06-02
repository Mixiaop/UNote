using U.Domain.Entities;
using U.EntityFramework.Repositories;

namespace UNote.EntityFramework
{
    public abstract class UNoteRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<UNodeDbContext, TEntity, TPrimaryKey>
       where TEntity : class, IEntity<TPrimaryKey>
    {
        protected UNoteRepositoryBase(UNodeDbContext dbContext)
            : base(dbContext, false)
        {

        }
    }

    public abstract class UNoteRepositoryBase<TEntity> : EfRepositoryBase<UNodeDbContext, TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected UNoteRepositoryBase(UNodeDbContext dbContext)
            : base(dbContext, false)
        {

        }
    }
}
