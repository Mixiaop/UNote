using System;
using UNote.Domain.Users;
using U.Utilities.SqlServer;

namespace UNote.EntityFramework.Repositories.Users
{
    public class UserVisitLogRepository : UNoteRepositoryBase<UserVisitLog>, IUserVisitLogRepository
    {
        public UserVisitLogRepository(UNodeDbContext dbContext) : base(dbContext) { }

    }
}
