using System;
using UNote.Domain.Users;
using U.Utilities.SqlServer;

namespace UNote.EntityFramework.Repositories.Users
{
    public class UserRepository : UNoteRepositoryBase<User>, IUserRepository
    {
        public UserRepository(UNodeDbContext dbContext) : base(dbContext) { }

        public void FlushHeartbeat(int userId)
        {
            SqlHelper.ExecuteSql(string.Format("UPDATE [{0}] SET [HeartbeatTime]='{1}' WHERE [Id]={2}",
                                               DbConsts.DbTableName.Users_Users,
                                               DateTime.Now.ToString(),
                                               userId));

            Context.Current.SaveChanges();
        }
    }
}
