using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U;
using U.Utilities.Web;
using UNote.Domain.Notes;
using UNote.Domain.Users;
using UNote.Services.Notes;
using UNote.Services.Users;

namespace UNote.Web.Notes
{
    public partial class ContentInfo : Infrastructure.UserPage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();
        IUserVisitLogService _userVisitLogService = UPrimeEngine.Instance.Resolve<IUserVisitLogService>();

        protected ContentInfoModel Model = new ContentInfoModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearUserPage = true;
            Model.LoginedUser = GetLoginedUser();
            Model.Content = _contentService.GetById(Model.ContentId);
            if (Model.Content.NodeId > 0)
            {
                CurrentNav = Infrastructure.Navigation.Contents;
                CurrentNavNodeId = Model.Content.NodeId;
            }

            //更新浏览次数
            _contentService.AddVisitCount(0, Model.Content);
            _userVisitLogService.Visit(Model.LoginedUser, 2, null, Model.Content); //访问日志


            #region LoadDataToModel
            Model.LoginedUser = GetLoginedUser();

            if (Model.Content.Node != null)
            {
                if (Model.Content.Node.TeamId > 0)
                    Model.AllNodes = _nodeService.GetAllByTeam(Model.Content.TeamId);
                else
                    Model.AllNodes = _nodeService.GetAll(Model.LoginedUser.Id);

                if (Model.Content.Node != null && Model.Content.Node.ParentsPath.IsNotNullOrEmpty())
                {
                    string[] parentIds = Model.Content.Node.ParentsPath.Split(',');
                    foreach (var id in parentIds)
                    {
                        var node = Model.AllNodes.Where(x => x.Id == id.ToInt()).FirstOrDefault();
                        if (node != null)
                            Model.Parents.Add(node);
                    }
                }
            }
            #endregion
        }
    }

    public class ContentInfoModel
    {
        public ContentInfoModel()
        {
            Parents = new List<Node>();
        }

        public int ContentId { get { return WebHelper.GetInt("contentId", 0); } }

        public UNote.Domain.Notes.Content Content { get; set; }

        public User LoginedUser { get; set; }

        public IList<Node> Parents { get; set; }

        public IList<Node> AllNodes { get; set; }
    }
}