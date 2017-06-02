using System;
using System.Web.UI.WebControls;
using U;
using UNote.Domain.Notes;
using UNote.Services.Notes;

namespace UNote.Console.Notes
{
    public partial class Content_Add : UI.NodePage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Click += btnSave_Click;
            if (!IsPostBack)
            {
                ddlNode.Items.Clear();
                ddlNode.Items.Add(new ListItem("无", "0"));
                var allNodes = _nodeService.GetAll();
                LoadNodeListItemToDLL(ddlNode, allNodes);
                //foreach (var p in parents)
                //  ddlParentId.Items.Add(new ListItem(p.NodeName, p.Id.ToString()));

            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            var content = new UNote.Domain.Notes.Content();

            try
            {
                content.Title = tbTitle.Text.Trim();
                content.Body = tbContent.Text.Trim();
                content.FileId = hfFileId.Value.ToInt();
                content.FileUrl = hfFileUrl.Value;
                content.FileSize = hfFileSize.Value.ToInt();
                content.NodeId = ddlNode.SelectedValue.ToInt();
                content.Tag = tbTags.Text.Trim();
                content.Public = ddlPublic.SelectedValue.ToInt() == 1;
                _contentService.Insert(content);

                ltlMessage.Text = AlertSuccess("添加内容成功");
                this.RedirectByTime("Content_Query.aspx", 1000);
            }
            catch (Exception ex) {
                ltlMessage.Text = AlertError(ex.Message);
            }
        }
    }
}