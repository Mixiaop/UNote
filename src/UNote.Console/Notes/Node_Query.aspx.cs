using System;
using System.Web.UI.WebControls;
using U;
using U.Utilities.Web;
using UZeroConsole.Web;
using UNote.Services.Notes;

namespace UNote.Console.Notes
{
    public partial class Node_Query : UI.NodePage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();

        protected NodeQueryModel Model = new NodeQueryModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSearch.Click += btnSearch_Click;
            if (!IsPostBack) {
                tbKeywords.Text = WebHelper.GetString("wd");
                ddlCreator.SelectedValue = WebHelper.GetString("isadmin");
                ddlPublic.SelectedValue = WebHelper.GetString("ispublic");
                BindPageDatas();
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            BindPageDatas(true);
        }

        protected void btnDelete_Click(object sender, EventArgs e) {
            try
            {
                LinkButton lb = (LinkButton)sender;
                var node = _nodeService.GetById(lb.CommandArgument.ToInt());
                _nodeService.Delete(node);
                ltlMessage.Text = AlertSuccess("删除成功");
                BindPageDatas();
            }
            catch (Exception ex) {
                ltlMessage.Text = AlertError(ex.Message);
            }
        }

        private void BindPageDatas(bool resetPage = false)
        {
            var url = GetUrlParam();
            PagingInfo pageInfo = new PagingInfo();
            pageInfo.PageIndex = resetPage ? 1 : WebHelper.GetInt("page", 1);
            pageInfo.PageSize = 20;
            pageInfo.Url = url == "" ? WebHelper.GetUrl() : "Node_Query.aspx?" + url;

            bool? isAdmin = null, isPublic = null;
            if (ddlCreator.SelectedValue.ToInt() == 1)
                isAdmin = true;
            else if (ddlCreator.SelectedValue.ToInt() == 0)
                isAdmin = false;

            if (ddlPublic.SelectedValue.ToInt() == 1)
                isPublic = true;
            else if (ddlPublic.SelectedValue.ToInt() == 0)
                isPublic = false;

            string keywords = tbKeywords.Text.Trim();


            var datas = _nodeService.Query(pageInfo.PageIndex, pageInfo.PageSize, tbKeywords.Text.Trim(), isAdmin, isPublic);
            rptDatas.DataSource = datas.Items;
            rptDatas.DataBind();
            pageInfo.TotalCount = datas.TotalCount;
            Model.PagerHtml = new Paginations(pageInfo).GetPaging();
        }

        private string GetUrlParam()
        {
            string cdi = "";
            if (tbKeywords.Text.IsNotNullOrEmpty())
            {
                if (cdi != "")
                    cdi += "&";
                cdi += "wd=" + tbKeywords.Text;
            }
            if (ddlCreator.SelectedValue != "-1")
            {
                if (cdi != "")
                    cdi += "&";
                cdi += "isadmin=" + ddlCreator.SelectedValue;
            }

            if (ddlPublic.SelectedValue != "-1")
            {
                if (cdi != "")
                    cdi += "&";
                cdi += "ispublic=" + ddlPublic.SelectedValue;
            }

            if (WebHelper.GetString("page") != "")
            {
                if (cdi != "")
                    cdi += "&";
                cdi += "page=" + WebHelper.GetString("page");
            }
            return cdi;
        }
    }

    public class NodeQueryModel {
        public string PagerHtml { get; set; }
    }
}