using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Web.UI.WebControls;
using System.Linq;
using U;
using U.Utilities.Web;
using UZeroMedia.Client.Services;
using UZeroMedia.Client.Services.Dto;
using UNote.Configuration;
using UNote.Domain.Notes;
using UNote.Domain.Users;
using UNote.Services.Notes;
using UNote.Services.Users;


namespace UNote.Web.Notes
{
    public partial class AddContent : NodePage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();
        IContentFollowerService _contentFollowerService = UPrimeEngine.Instance.Resolve<IContentFollowerService>();
        ITagService _tagService = UPrimeEngine.Instance.Resolve<ITagService>();
        IUserService _userService = UPrimeEngine.Instance.Resolve<IUserService>();
        
        PictureClientService _pictureClientService = UPrimeEngine.Instance.Resolve<PictureClientService>();
        UNoteSettings _settings = UPrimeEngine.Instance.Resolve<UNoteSettings>();
        
        protected AddContentModel Model = new AddContentModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Click += btnSave_Click;
            if (Model.NodeId > 0)
            {
                CurrentNav = Infrastructure.Navigation.Contents;
                CurrentNavNodeId = Model.NodeId;
                Model.Node = _nodeService.GetById(Model.NodeId);
            }
            else
                Response.Redirect(RouteContext.GetRouteUrl("Users.Home"));

            if (Model.GetContentId > 0) {
                Model.Parent = _contentService.GetById(Model.GetContentId);
            }

            if (!IsPostBack)
            {
                
                if (Model.Node != null && Model.Node.TeamId > 0)
                    Model.AllNodes = _nodeService.GetAllByTeam(Model.Node.TeamId);
                else
                    Model.AllNodes = _nodeService.GetAll(GetLoginedUser().Id);

                if (Model.Node.ParentsPath.IsNotNullOrEmpty())
                {
                    string[] parentIds = Model.Node.ParentsPath.Split(',');
                    foreach (var id in parentIds)
                    {
                        var node = Model.AllNodes.Where(x => x.Id == id.ToInt()).FirstOrDefault();
                        if (node != null)
                            Model.Parents.Add(node);
                    }
                }

                Model.Tags = _tagService.QueryTags(Model.Node.Id);

                #region ddlNode
                //ddlNode.Enabled = false;
                //ddlNode.Items.Clear();
                //ddlNode.Items.Add(new ListItem("无", "0"));
                //if (Model.AllNodes == null || (Model.AllNodes != null && Model.AllNodes.Count == 0))
                //{
                //    phCreateNode.Visible = true;
                //    Response.Redirect("ManageNode.aspx");
                //}
                //else
                //{
                //    phCreateNode.Visible = false;
                //}

                //LoadNodeListItemToDLL(ddlNode, Model.AllNodes);
                
                //if (Model.NodeId > 0)
                //    ddlNode.SelectedValue = Model.NodeId.ToString();
                //foreach (var p in parents)
                //  ddlParentId.Items.Add(new ListItem(p.NodeName, p.Id.ToString()));
                #endregion
            }
        }



        void btnSave_Click(object sender, EventArgs e)
        {
            #region 添加
            var content = new UNote.Domain.Notes.Content();
            //if (Model.AllNodes == null || (Model.AllNodes != null && Model.AllNodes.Count == 0))
            //{
            //    ltlMessage.Text = AlertError("请先至少创建一个目录");
            //    return;
            //}
            try
            {
                content.Title = tbTitle.Text.Trim();
                content.Body = tbContent.Text.Trim();
                content.FileId = hfFileId.Value.ToInt();
                content.FileUrl = hfFileUrl.Value;
                content.FileSize = hfFileSize.Value.ToInt();
                content.NodeId = Model.NodeId;
                content.Tag = tbTags.Text.Trim();
                content.NodeTypeId = (int)Model.CurrentNodeType;
                //content.Public = ddlPublic.SelectedValue.ToInt() == 1;
                content.UserId = GetLoginedUser().Id;
                if (Model.Node != null && Model.Node.TeamId > 0)
                    content.TeamId = Model.Node.TeamId;

                content.PreviewId = hfPreviewId.Value.ToInt();
                content.PreviewUrl = hfPreviewUrl.Value;
                content.IsTop = ddlIsTop.SelectedValue.ToInt() == 1 ? true : false;

                switch (Model.CurrentNodeType)
                { 
                    case NodeType.Normal:
                        break;
                    case NodeType.Html:
                        content.FileId = hfHtmlFileId.Value.ToInt();
                        content.FileSize = hfHtmlFileSize.Value.ToInt();
                        content.FileUrl = hfHtmlFileUrl.Value;
                        content.NodeHtmlFileCode = hfHtmlCode.Value;
                        content.NodeHtmlHomePage = tbHtmlHomePage.Text.Trim();
                        content.Body = tbHtmlBody.Text.Trim();
                        break;
                    case NodeType.Word:
                        content.Body = tbWordContent.Text.Trim();
                        break;
                }

                #region 上传远程图片
                var imgs = HtmlHelper.GetRemoteImgUrls(content.Body);
                WebClient wc = new WebClient();
                foreach (string img in imgs) {
                    if (img.IsNotNullOrEmpty() && !img.Contains(_settings.UZeroMediaHost.Replace("http://","")))
                    {
                        try
                        {
                            byte[] buf = wc.DownloadData(img);
                            string filename = Path.GetFileName(img);

                            var res = _pictureClientService.Upload(filename, buf);
                            if (res != null && res.Results != null)
                            {
                                content.Body = content.Body.Replace(img, res.Results.PictureUrl);
                            }
                        }
                        catch (Exception ex) {
                            //var a = ex.Message;
                        }
                    }
                }
                #endregion
                
                //关注的人
                if (Model.Node.TeamId > 0 && hfFollowerIds.Value.IsNotNullOrEmpty())
                {
                    content.Followers = new List<ContentFollower>();
                    var ids = hfFollowerIds.Value.Split(',');
                    foreach (var id in ids)
                    {
                        var user = _userService.GetById(id.ToInt());
                        content.Followers.Add(new ContentFollower() { ContentId = 0, UserId = user.Id, Content = content, User = user });
                    }
                }

                if (Model.GetContentId > 0)
                {
                    //添加内容项
                    _contentService.InsertItem(Model.Parent, content);
                }
                else {
                    //添加内容
                    var contentId = _contentService.Insert(content);
                }
                ltlMessage.Text = AlertSuccess("添加内容成功");
                this.RedirectByTime(RouteContext.GetRouteUrl("Notes.Contents", content.NodeId), 1000);
            }
            catch (Exception ex)
            {
                ltlMessage.Text = AlertError(ex.Message);
            }
            #endregion
        }


    }

    public class AddContentModel
    {
        public AddContentModel() {
            Parents = new List<Node>();
            Tags = new List<Tag>();
        }

        public int NodeId { get { return WebHelper.GetInt("nodeId", 0); } }

        public int GetContentId { get { return WebHelper.GetInt("contentId", 0); } }

        public Node Node { get; set; }

        public UNote.Domain.Notes.Content Parent { get; set; }

        public IList<Tag> Tags { get; set; }

        public IList<Node> Parents { get; set; }

        public IList<Node> AllNodes { get; set; }

        public NodeType CurrentNodeType {
            get {
                int nodeTypeId = WebHelper.GetInt("nodetype", 0);
                if (nodeTypeId == 0)
                    nodeTypeId = Node.NodeTypeId;

                return (NodeType)nodeTypeId;

            }
        }
    }
}