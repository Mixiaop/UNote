using U;
using U.Logging;
using U.Application.Services.Events;

namespace UNote.Services
{
    /// <summary>
    /// 所有应用服务的基类服务
    /// </summary>
    public abstract class ServiceBase : U.Application.Services.ApplicationService
    {
        public ILogger Logger;
        public IEventPublisher EventPublisher;
        public ServiceBase() {
            Logger = UPrimeEngine.Instance.Resolve<ILogger>();
            EventPublisher = UPrimeEngine.Instance.Resolve<IEventPublisher>();
        }
    }
}
