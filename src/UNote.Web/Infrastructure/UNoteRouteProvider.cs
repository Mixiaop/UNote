using U.FakeMvc.Routes;

namespace UNote.Web.Infrastructure
{
    public class UNoteRouteProvider : IRouteProvider
    {

        public void RegisterRewriterRole(RouteContext context)
        {
            context.AddRewriterRule(new RewriterRule(@"/users/login", "/Login.aspx"));
            context.AddRewriterRule(new RewriterRule("/users/logout", "/Logout.aspx"));
            context.AddRewriterRule(new RewriterRule("/users/reg", "/Registration.aspx"));
            context.AddRewriterRule(new RewriterRule("/users/home", "/Users/Default.aspx"));
            context.AddRewriterRule(new RewriterRule("/users/changePassword", "/Users/ChangePassword"));

            context.AddRewriterRule(new RewriterRule("/notes/catalog", "/Notes/ManageNode.aspx"));
            context.AddRewriterRule(new RewriterRule(@"/notes/add", "/Notes/AddContent.aspx"));
            context.AddRewriterRule(new RewriterRule(@"/notes/addItem", "/Notes/AddContent.aspx"));
            ///notes/([0-9]*)/contents?wd=([\\s\\S]*)", "/Notes/Contents.aspx?nodeId=$1&wd=$2
            context.AddRewriterRule(new RewriterRule("/notes/edit", "/Notes/EditContent.aspx"));
            context.AddRewriterRule(new RewriterRule("/notes/([0-9]*)/contents", "/Notes/Contents.aspx?nodeId=$1"));
            context.AddRewriterRule(new RewriterRule("/notes/([0-9]*)/boards", "/Notes/Boards/Index.aspx?nodeId=$1"));
            context.AddRewriterRule(new RewriterRule("/notes/([0-9]*)/boards/archivedTasks", "/Notes/Boards/ArchivedTasks.aspx?nodeId=$1"));
            context.AddRewriterRule(new RewriterRule("/notes/content/([0-9]*)", "/Notes/ContentInfo.aspx?contentId=$1"));

            context.AddRewriterRule(new RewriterRule("/teams/create", "/Teams/CreateNewTeam.aspx"));
            context.AddRewriterRule(new RewriterRule(@"/teams/([\s\S]*)/info", "/Teams/TeamInfo.aspx?key=$1"));
            context.AddRewriterRule(new RewriterRule(@"/teams/([\s\S]*)/members", "/Teams/Members.aspx?key=$1"));
            context.AddRewriterRule(new RewriterRule(@"/teams/([\s\S]*)/catalog", "/Teams/Nodes.aspx?key=$1"));
        }

        public void RegisterRoutes(RouteContext context)
        {
            context.AddRoute(new Route("Users.Login", Format("/users/login?goUrl={0}")));
            context.AddRoute(new Route("Users.Logout", Format("/users/logout?goUrl={0}")));
            context.AddRoute(new Route("Users.Register", Format("/users/reg")));
            context.AddRoute(new Route("Users.Home", Format("/users/home")));
            context.AddRoute(new Route("Users.ChangePassword", Format("/users/changepassword")));

            context.AddRoute(new Route("Notes.Nodes", Format("/notes/catalog")));
            context.AddRoute(new Route("Notes.AddContent", Format("/notes/add?nodeId={0}&nodeType={1}")));
            context.AddRoute(new Route("Notes.AddContentItem", Format("/notes/addItem?contentId={0}&nodeId={1}&nodeType={2}")));
            context.AddRoute(new Route("Notes.EditContent", Format("/notes/edit?contentId={0}&nodeType={1}&goUrl={2}")));
            context.AddRoute(new Route("Notes.Contents", Format("/notes/{0}/contents?wd={1}&tag={2}&page={3}")));
            context.AddRoute(new Route("Notes.Boards", Format("/notes/{0}/boards")));
            context.AddRoute(new Route("Notes.Boards.ArchivedTasks", Format("/notes/{0}/boards/archivedTasks")));
            context.AddRoute(new Route("Notes.ContentInfo", Format("/notes/content/{0}")));

            context.AddRoute(new Route("Teams.Create", Format("/teams/create")));
            context.AddRoute(new Route("Teams.Info", Format("/teams/{0}/info")));
            context.AddRoute(new Route("Teams.Members", Format("/teams/{0}/members")));
            context.AddRoute(new Route("Teams.Nodes", Format("/teams/{0}/catalog")));
        }

        private string Format(string url)
        {
            var host = "/";
            url = url.TrimStart('/');
            return host + url;
        }
    }
}