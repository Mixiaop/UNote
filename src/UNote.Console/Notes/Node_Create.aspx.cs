using System;
using System.Web.UI.WebControls;
using U;
using UNote.Domain.Notes;
using UNote.Services.Notes;

namespace UNote.Console.Notes
{
    public partial class Node_Create : UI.NodePage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Click += btnSave_Click;
            if (!IsPostBack) {
                ddlParentId.Items.Clear();
                ddlParentId.Items.Add(new ListItem("无", "0"));
                var allNodes = _nodeService.GetAll();
                LoadNodeListItemToDLL(ddlParentId, allNodes);
                //foreach (var p in parents)
                  //  ddlParentId.Items.Add(new ListItem(p.NodeName, p.Id.ToString()));

            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            #region 添加新分类
            try
            {
                var node = new Node();
                node.NodeName = tbNodeName.Text.Trim();
                node.Alias = tbAlias.Text.Trim();
                node.ParentId = ddlParentId.SelectedValue.ToInt();
                node.Description = tbDescription.Text.Trim();
                node.Public = ddlPublic.SelectedValue.ToInt() == 1;

                _nodeService.Insert(node);

                ltlMessage.Text = AlertSuccess("添加新分类成功");
                this.RedirectByTime("Node_Query.aspx", 1000);
            }
            catch (Exception ex) {
                ltlMessage.Text = AlertError(ex.Message);
            }
            #endregion
        }
    }
}