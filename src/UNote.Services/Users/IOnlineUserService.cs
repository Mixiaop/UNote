using System.Collections.Generic;
using UNote.Domain.Users;
using UNote.Services.Users.Dto;

namespace UNote.Services.Users
{
    public interface IOnlineUserService : U.Application.Services.IApplicationService
    {
        void Heartbeat();

        List<OnlineUserDto> GetOnlineUsers();
    }
}
