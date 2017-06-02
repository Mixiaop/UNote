using U.Domain.Repositories;

namespace UNote.Domain.Users
{
    public interface IUserRepository : IRepository<User, int>
    {
        void FlushHeartbeat(int userId);
    }
}
