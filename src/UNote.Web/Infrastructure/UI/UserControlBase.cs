using U.FakeMvc.Routes;

namespace UNote.Web.Infrastructure
{
    public class UserControlBase : System.Web.UI.UserControl
    {
        public RouteContext RouteContext = RouteContext.Instance;
    }
}