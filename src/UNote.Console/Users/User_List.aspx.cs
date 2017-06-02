using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U;
using U.Utilities.Web;
using UZeroConsole.Web;
using UNote.Services.Users;


namespace UNote.Console.Users
{
    public partial class User_List : Infrastructure.UNotePage
    {
        IUserService _userService = UPrimeEngine.Instance.Resolve<IUserService>();

        public string PagerHtml { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSearch.Click += btnSearch_Click;
            if (!IsPostBack)
            {
                tbKeywords.Text = WebHelper.GetString("wd");
                BindPageDatas();
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            BindPageDatas(true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lb = (LinkButton)sender;
                var user = _userService.GetById(lb.CommandArgument.ToInt());
                _userService.Delete(user);
                ltlMessage.Text = AlertSuccess("删除成功");
                BindPageDatas();
            }
            catch (Exception ex)
            {
                ltlMessage.Text = AlertError(ex.Message);
            }
        }

        private void BindPageDatas(bool resetPage = false)
        {
            var url = GetUrlParam();
            PagingInfo pageInfo = new PagingInfo();
            pageInfo.PageIndex = resetPage ? 1 : WebHelper.GetInt("page", 1);
            pageInfo.PageSize = 20;
            pageInfo.Url = url == "" ? WebHelper.GetUrl() : "User_List.aspx?" + url;


            string keywords = tbKeywords.Text.Trim();


            var datas = _userService.Query(pageInfo.PageIndex, pageInfo.PageSize, tbKeywords.Text.Trim());
            rptDatas.DataSource = datas.Items;
            rptDatas.DataBind();
            pageInfo.TotalCount = datas.TotalCount;
            PagerHtml = new Paginations(pageInfo).GetPaging();
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
            
            if (WebHelper.GetString("page") != "")
            {
                if (cdi != "")
                    cdi += "&";
                cdi += "page=" + WebHelper.GetString("page");
            }
            return cdi;
        }
    }
}