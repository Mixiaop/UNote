using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using U;
using U.Utilities.Web;
using UNote.Configuration;
using UNote.Domain.Notes;
using UNote.Domain.Users;
using UNote.Services.Notes;
using UNote.Services.Users;
using UZeroMedia.Client.Services;
using UZeroMedia.Client.Services.Dto;

namespace UNote.Web.Notes
{
    public partial class EditContent : NodePage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();
        IContentFollowerService _followerService = UPrimeEngine.Instance.Resolve<IContentFollowerService>();
        ITagService _tagService = UPrimeEngine.Instance.Resolve<ITagService>();
        IUserService _userService = UPrimeEngine.Instance.Resolve<IUserService>();

        PictureClientService _pictureClientService = UPrimeEngine.Instance.Resolve<PictureClientService>();
        UNoteSettings _settings = UPrimeEngine.Instance.Resolve<UNoteSettings>();

        protected EditContentModel Model = new EditContentModel();

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Click += btnSave_Click;
            Model.Content = _contentService.GetById(Model.ContentId);
            if (Model.Content.NodeId > 0)
            {
                CurrentNav = Infrastructure.Navigation.Contents;
                CurrentNavNodeId = Model.Content.NodeId;
            }
            else
                CurrentNav = Infrastructure.Navigation.AddContent;

            if (Model.Content == null)
                Invoke404();

            Model.Tags = _tagService.QueryTags(Model.Content.NodeId);

            if (!IsPostBack)
            {
                #region 目录
                //ddlNode.Items.Clear();
                //ddlNode.Enabled = false;
                if (Model.Content != null && Model.Content.Node.TeamId > 0)
                    Model.AllNodes = _nodeService.GetAllByTeam(Model.Content.Node.TeamId);
                else
                    Model.AllNodes = _nodeService.GetAll(GetLoginedUser().Id);


                if (Model.Content.Node.ParentsPath.IsNotNullOrEmpty())
                {
                    string[] parentIds = Model.Content.Node.ParentsPath.Split(',');
                    foreach (var id in parentIds)
                    {
                        var node = Model.AllNodes.Where(x => x.Id == id.ToInt()).FirstOrDefault();
                        if (node != null)
                            Model.Parents.Add(node);
                    }
                }
                //LoadNodeListItemToDLL(ddlNode, Model.AllNodes);

                //if (Model.Content.NodeId > 0)
                //    ddlNode.SelectedValue = Model.Content.NodeId.ToString();
                #endregion

                #region 载入数据
                tbTitle.Text = Model.Content.Title;
                tbContent.Text = Model.Content.Body;
                //ddlNode.SelectedValue = Model.Content.NodeId.ToString();
                tbTags.Text = Model.Content.Tag.Trim();
                //ddlPublic.SelectedValue = Model.Content.Public ? "1" : "0";
                hfFileId.Value = Model.Content.FileId.ToString();
                hfFileSize.Value = Model.Content.FileSize.ToString();
                hfFileUrl.Value = Model.Content.FileUrl.ToString();
                Model.CurrentNodeType = Model.Content.FormatNodeType;
                hfPreviewId.Value = Model.Content.PreviewId.ToString();
                hfPreviewUrl.Value = Model.Content.PreviewUrl;
                ddlIsTop.SelectedValue = Model.Content.IsTop ? "1" : "0";
                switch (Model.CurrentNodeType)
                {
                    case NodeType.Html:
                        #region HTML
                        tbHtmlBody.Text = Model.Content.Body;
                        hfHtmlFileId.Value = Model.Content.FileId.ToString();
                        hfHtmlFileSize.Value = Model.Content.FileSize.ToString();
                        hfHtmlFileUrl.Value = Model.Content.FileUrl.ToString();
                        hfHtmlCode.Value = Model.Content.NodeHtmlFileCode;
                        tbHtmlHomePage.Text = Model.Content.NodeHtmlHomePage;
                        #endregion
                        break;
                    case NodeType.Word:
                        tbWordContent.Text = Model.Content.Body;
                        break;

                }

                //关注
                if (Model.Content.TeamId > 0) {
                    var followers = _followerService.GetAllFollowers(Model.ContentId);
                    if (followers != null && followers.Count > 0) {
                        var ids = "";
                        foreach (var follower in followers) {
                            ids += follower.UserId + ":" + follower.User.FormatNickName + ",";
                        }
                        if (ids.IsNotNullOrEmpty()) {
                            ids = ids.Substring(0, ids.Length - 1);
                        }

                        hfFollowerIds.Value = ids;
                    }
                }
                #endregion

            }

            Model.OldNodeId = Model.Content.NodeId;
        }



        void btnSave_Click(object sender, EventArgs e)
        {

            //if (Model.AllNodes == null || Model.AllNodes.Count == 0)
            //{
            //    ltlMessage.Text = AlertError("请先至少创建一个目录");
            //}
            try
            {
                Model.Content.Title = tbTitle.Text.Trim();
                Model.Content.Body = tbContent.Text.Trim();
                Model.Content.FileId = hfFileId.Value.ToInt();
                Model.Content.FileUrl = hfFileUrl.Value;
                Model.Content.FileSize = hfFileSize.Value.ToInt();
                Model.Content.Tag = tbTags.Text.Trim();
                Model.Content.LastModifierUserId = GetLoginedUser().Id;
                Model.Content.NodeTypeId = (int)Model.CurrentNodeType;
                Model.Content.PreviewId = hfPreviewId.Value.ToInt();
                Model.Content.PreviewUrl = hfPreviewUrl.Value;
                Model.Content.IsTop = ddlIsTop.SelectedValue.ToInt() == 1 ? true : false;
                //Model.Content.NodeId = ddlNode.SelectedValue.ToInt();
                //Model.Content.Public = ddlPublic.SelectedValue.ToInt() == 1;
                //Model.Content.UserId = GetLoginedUser().Id;

                switch (Model.CurrentNodeType)
                {
                    case NodeType.Normal:
                        break;
                    case NodeType.Html:
                        Model.Content.FileId = hfHtmlFileId.Value.ToInt();
                        Model.Content.FileSize = hfHtmlFileSize.Value.ToInt();
                        Model.Content.FileUrl = hfHtmlFileUrl.Value;
                        Model.Content.NodeHtmlFileCode = hfHtmlCode.Value;
                        Model.Content.NodeHtmlHomePage = tbHtmlHomePage.Text.Trim();
                        Model.Content.Body = tbHtmlBody.Text.Trim();
                        break;
                    case NodeType.Word:
                        Model.Content.Body = tbWordContent.Text.Trim();
                        break;
                }

                #region 上传远程图片
                var imgs = HtmlHelper.GetRemoteImgUrls(Model.Content.Body);
                WebClient wc = new WebClient();
                foreach (string img in imgs)
                {
                    if (img.IsNotNullOrEmpty() && !img.Contains(_settings.UZeroMediaHost.Replace("http://", "")))
                    {
                        try
                        {
                            byte[] buf = wc.DownloadData(img);
                            string filename = Path.GetFileName(img);

                            var res = _pictureClientService.Upload(filename, buf);
                            if (res != null && res.Results != null)
                            {
                                Model.Content.Body = Model.Content.Body.Replace(img, res.Results.PictureUrl);
                            }
                        }
                        catch (Exception ex)
                        {
                            //var a = ex.Message;
                        }
                    }
                }
                #endregion

                //关注的人
                if (Model.Content.TeamId > 0 && hfFollowerIds.Value.IsNotNullOrEmpty())
                {
                    Model.Content.Followers = new List<ContentFollower>();
                    var ids = hfFollowerIds.Value.Split(',');
                    foreach (var id in ids)
                    {
                        var user = _userService.GetById(id.ToInt());
                        Model.Content.Followers.Add(new ContentFollower() { ContentId = 0, UserId = user.Id, Content = Model.Content, User = user });
                    }
                }

                _contentService.Update(Model.Content, Model.OldNodeId);


                ltlMessage.Text = AlertSuccess("保存内容成功");
                if (Model.GoUrl.IsNullOrEmpty())
                    this.RedirectByTime(RouteContext.GetRouteUrl("Notes.ContentInfo",Model.ContentId), 1000);
                else
                    this.RedirectByTime(Model.GoUrl.DecodeUTF8Base64(), 1000);
            }
            catch (Exception ex)
            {
                ltlMessage.Text = AlertError(ex.Message);
            }
        }
    }

    public class EditContentModel
    {
        public EditContentModel()
        {
            Parents = new List<Node>();
        }

        public int ContentId { get { return WebHelper.GetInt("contentId", 0); } }

        public UNote.Domain.Notes.Content Content { get; set; }

        public IList<Tag> Tags { get; set; }

        public IList<Node> AllNodes { get; set; }

        public IList<Node> Parents { get; set; }

        public int OldNodeId { get; set; }

        public string GoUrl
        {
            get { return WebHelper.GetString("gourl"); }
        }

        private int _nodeTypeId;
        public NodeType CurrentNodeType
        {
            get
            {
                int getTypeId = WebHelper.GetInt("nodetype", 0);
                if (getTypeId != 0)
                {
                    _nodeTypeId = getTypeId;
                }
                else {
                    _nodeTypeId = (int)Content.FormatNodeType;
                }

                return (NodeType)_nodeTypeId;

            }
            set { _nodeTypeId = (int)value; }
        }
    }
}