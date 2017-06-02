using System;
using System.Web.UI.WebControls;
using U;
using U.Utilities.Web;
using UNote.Domain.Notes;
using UNote.Services.Notes;

namespace UNote.Console.Notes
{
    public partial class Node_Update : UI.NodePage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        protected Node nodeInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            nodeInfo = _nodeService.GetById(WebHelper.GetInt("nodeId", 0));
            if (nodeInfo == null) Response.Redirect("Node_Query.aspx");

            btnSave.Click += btnSave_Click;
            if (!IsPostBack) {
                ddlParentId.Items.Clear();
                ddlParentId.Items.Add(new ListItem("无", "0"));
                var allNodes = _nodeService.GetAll();
                LoadNodeListItemToDLL(ddlParentId, allNodes);

                tbNodeName.Text = nodeInfo.NodeName;
                tbAlias.Text = nodeInfo.Alias;
                tbDescription.Text = nodeInfo.Description;
                ddlPublic.SelectedValue = nodeInfo.Public ? "1" : "0";
                ddlParentId.SelectedValue = nodeInfo.ParentId.ToString();
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                nodeInfo.NodeName = tbNodeName.Text.Trim();
                nodeInfo.Alias = tbAlias.Text.Trim();
                nodeInfo.ParentId = ddlParentId.SelectedValue.ToInt();
                nodeInfo.Description = tbDescription.Text.Trim();
                nodeInfo.Public = ddlPublic.SelectedValue.ToInt() == 1;

                _nodeService.Update(nodeInfo);

                ltlMessage.Text = AlertSuccess("更新分类成功");
                this.RedirectByTime("Node_Query.aspx", 1000);
            }
            catch (Exception ex)
            {
                ltlMessage.Text = AlertError(ex.Message);
            }
        }
    }
}