using U;
using U.Logging;
using U.Application.Services.Events;
using UNote.Services.Authrization;
using UNote.Configuration;

namespace UNote.Services
{
    /// <summary>
    /// 所有应用服务的基类服务
    /// </summary>
    public abstract class ServiceBase : U.Application.Services.ApplicationService
    {
        public IEventPublisher EventPublisher;
        public IAuthenticationService AuthService;
        public UNoteSettings Settings;
        public ServiceBase() {
            EventPublisher = UPrimeEngine.Instance.Resolve<IEventPublisher>();
            AuthService = UPrimeEngine.Instance.Resolve<IAuthenticationService>();
            Settings = UPrimeEngine.Instance.Resolve<UNoteSettings>();
        }

        public int GetLoginedUserId() {
            var user = AuthService.GetAuthenticatedUser();
            return user != null ? user.Id : 0;
        }

        public string GetLoginedUserNickName() {
            var user = AuthService.GetAuthenticatedUser();
            return user != null ? user.NickName : "";
        }
    }
}
