using System;
using System.Collections.Generic;
using System.Linq;
using UNote.Configuration;
using UNote.Domain.Users;
using UNote.Services.Authrization;
using UNote.Services.Users.Dto;

namespace UNote.Services.Users
{
    public class OnlineUserService : IOnlineUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly UNoteSettings _settings;
        public OnlineUserService(IUserRepository userRepository, IAuthenticationService authenticationService, UNoteSettings settings)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
            _settings = settings;
        }

        public void Heartbeat() {
            if (_authenticationService.GetAuthenticatedUser() !=null) {
                var user = _authenticationService.GetAuthenticatedUser();
                if (user != null)
                    _userRepository.FlushHeartbeat(user.Id);
            }
        }

        public List<OnlineUserDto> GetOnlineUsers()
        {
            List<OnlineUserDto> users = new List<OnlineUserDto>();
            DateTime timeBefore = DateTime.Now.AddSeconds((-_settings.OnlineUserTime));
            var list = _userRepository.GetAll()
                .Where(x => x.HeartbeatTime >= timeBefore)
                .OrderByDescending(x => x.HeartbeatTime).ToList();

            foreach (var user in list)
            {
                users.Add(new OnlineUserDto() { UserId = user.Id, NickName = user.FormatNickName });
            }
            return users;
        }
    }
}
