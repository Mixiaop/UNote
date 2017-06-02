using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using U;
using U.Utilities.Web;
using U.Application.Services.Dto;
using UNote.Domain.Users;
using UNote.Domain.Notes;
using UNote.Services.Notes;
using UNote.Services.Users;
using UNote.Web.Infrastructure;

namespace UNote.Web.Notes
{
    public partial class Contents : Infrastructure.UserPage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();
        ITagService _tagService = UPrimeEngine.Instance.Resolve<ITagService>();
        IUserVisitLogService _userVisitLogService = UPrimeEngine.Instance.Resolve<IUserVisitLogService>();

        protected ContentsModel Model = new ContentsModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentNav = Infrastructure.Navigation.Contents;
            CurrentNavNodeId = Model.GetNodeId;
            Model.Node = _nodeService.GetById(Model.GetNodeId);
            Model.LoginedUser = GetLoginedUser();

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

            _userVisitLogService.Visit(Model.LoginedUser, 1, Model.Node); //访问日志

            if (!IsPostBack)
            {
                BindPageDatas();
            }

        }

        private void BindPageDatas(bool resetPage = false)
        {
            var url = GetUrlParam();
            PagingInfo pageInfo = new PagingInfo();
            pageInfo.PageIndex = resetPage ? 1 : WebHelper.GetInt("page", 1);
            pageInfo.PageSize = 12;
            if (Model.Node.ListShowType == NodeListShowType.Grid)
            {
                pageInfo.PageSize = 24;
            }

            pageInfo.Url = url == "" ? WebHelper.GetUrl() : url;

            //string keywords = tbKeywords.Text.Trim();

            //目录列表
            List<int> nodeIds = new List<int>();
            nodeIds.Add(Model.GetNodeId);
            if (Model.Node.ContentTotal == 0)
            {
                var childs2 = Model.MyNodes.Where(x => x.ParentId == Model.GetNodeId);
                if (childs2 != null)
                {
                    foreach (var node2 in childs2)
                    {
                        nodeIds.Add(node2.Id);
                        var childs3 = Model.MyNodes.Where(x => x.ParentId == node2.Id);
                        if (childs3 != null)
                        {
                            foreach (var node3 in childs3)
                            {
                                nodeIds.Add(node3.Id);
                            }
                        }
                    }
                }
            }

            Model.Tags = _tagService.QueryTags(Model.Node.Id);

            int userId = GetLoginedUser().Id;
            if (Model.Node.TeamId > 0)
                userId = 0;
            var datas = _contentService.Query(pageInfo.PageIndex, pageInfo.PageSize, userId, Model.GetKeywords, nodeIds, null, Model.GetTag);
            Model.ContentList = datas;

            pageInfo.TotalCount = datas.TotalCount;
            Model.PageHtml = new Paginations(pageInfo).GetPaging();
        }

        public string GetUrlParam()
        {
            //string cdi = "nodeid=" + Model.GetNodeId;
            //if (Model.GetKeywords.IsNotNullOrEmpty())
            //{
            //    if (cdi != "")
            //        cdi += "&";
            //    cdi += "wd=" + Model.GetKeywords;
            //}

            //if (Model.GetTag.IsNotNullOrEmpty())
            //{
            //    if (cdi != "")
            //        cdi += "&";
            //    cdi += "wd=" + Model.GetTag;
            //}


            //if (WebHelper.GetString("page") != "")
            //{
            //    if (cdi != "")
            //        cdi += "&";
            //    cdi += "page=" + WebHelper.GetString("page");
            //}

            return RouteContext.GetRouteUrl("Notes.Contents", Model.GetNodeId, Model.GetKeywords, Model.GetTag, WebHelper.GetInt("page", 1));
        }

        public string GetTag(string name)
        {
            return name;
            string tagName = string.IsNullOrEmpty(Model.GetTag) ? name : Model.GetTag;
            string url = string.Format("{0}",
                                      (Model.GetTag.IsNotNullOrEmpty() && !Model.GetTag.Contains(name) ? Model.GetTag + "_" + name : tagName));
            return url;
        }
    }

    public class ContentsModel
    {
        public ContentsModel()
        {
            Parents = new List<Node>();
        }

        public int GetNodeId { get { return WebHelper.GetInt("nodeId", 0); } }

        public string GetKeywords { get { return WebHelper.GetString("wd"); } }

        public string GetTag { get { return WebHelper.GetString("tag"); } }

        public IList<Node> MyNodes { get; set; }

        public Node Node { get; set; }

        public IList<Node> Parents { get; set; }

        public IList<Tag> Tags { get; set; }

        public PagedResultDto<UNote.Domain.Notes.Content> ContentList { get; set; }

        public string PageHtml { get; set; }

        public User LoginedUser { get; set; }
    }
}