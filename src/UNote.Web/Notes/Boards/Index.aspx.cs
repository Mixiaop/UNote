using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U;
using U.Utilities.Web;
using UNote.Domain.Notes;
using UNote.Services.Notes;

namespace UNote.Web.Notes.Boards
{
    /// <summary>
    /// 分类（黑板模式）首页
    /// </summary>
    public partial class Index : NodePage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();

        protected BoardsIndexModel Model = new BoardsIndexModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentNav = Infrastructure.Navigation.Contents;
            CurrentNavNodeId = Model.GetNodeId;
            Model.Node = _nodeService.GetById(Model.GetNodeId);
            if (Model.Node == null)
                Invoke404();

            if (Model.Node.NodeType != NodeType.Board) {
                Response.Redirect(RouteContext.GetRouteUrl("Notes.Contents", Model.GetNodeId));
            }

            #region InitDatas
            if (Model.Node.TeamId > 0)
                Model.MyNodes = _nodeService.GetAllByTeam(Model.Node.TeamId);
            else
                Model.MyNodes = _nodeService.GetAll(GetLoginedUser().Id);

            if (Model.Node.ParentsPath.IsNotNullOrEmpty())
            {
                string[] parentIds = Model.Node.ParentsPath.Split(',');
                foreach (var id in parentIds)
                {
                    var node = Model.MyNodes.Where(x => x.Id == id.ToInt()).FirstOrDefault();
                    if (node != null)
                        Model.Parents.Add(node);
                }
            }
            #endregion
        }
    }

    public class BoardsIndexModel {
        public BoardsIndexModel()
        {
            Parents = new List<Node>();
        }

        public int GetNodeId { get { return WebHelper.GetInt("nodeId", 0); } }

        public Node Node { get; set; }

        public IList<Node> MyNodes { get; set; }

        public IList<Node> Parents { get; set; }
    }
}