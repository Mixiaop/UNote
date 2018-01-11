using Owin;

namespace UNote.Web.Infrastructure.SignalR
{
    public static class Startup
    {
        public static void ConfigurationSignalR(IAppBuilder app) {
            app.MapSignalR();
        }
    }
}