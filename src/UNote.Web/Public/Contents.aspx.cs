using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using U;
using U.Utilities.Web;
using U.Application.Services.Dto;
using UNote.Domain.Notes;
using UNote.Services.Notes;
using UNote.Web.Infrastructure;

namespace UNote.Web.Public
{
    public partial class Contents : Infrastructure.UserPage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();

        protected PublicContentsModel Model = new PublicContentsModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentNav = Infrastructure.Navigation.UserHomePage;
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
            pageInfo.PageSize = 10;
            pageInfo.Url = url == "" ? WebHelper.GetUrl() : "Contents.aspx?" + url;

            //string keywords = tbKeywords.Text.Trim();

            var datas = _contentService.QueryPublicContents(pageInfo.PageIndex, pageInfo.PageSize, Model.Keywords);

            if (datas.TotalCount > 0)
            {
                Regex regPreview = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
                //设置预览图
                foreach (var content in datas.Items)
                {
                    Match match = regPreview.Match(content.Body);
                    if (match != null)
                        content.FormatPreviewUrl = match.Groups["imgUrl"].ToString();
                }
            }

            Model.ContentList = datas;

            pageInfo.TotalCount = datas.TotalCount;
            Model.PageHtml = new Paginations(pageInfo).GetPaging();
        }

        private string GetUrlParam()
        {
            string cdi = "";
            if (Model.Keywords.IsNotNullOrEmpty())
            {
                if (cdi != "")
                    cdi += "&";
                cdi += "wd=" + Model.Keywords;
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

    public class PublicContentsModel
    {
        public string Keywords { get { return WebHelper.GetString("wd"); } }

        public PagedResultDto<UNote.Domain.Notes.Content> ContentList { get; set; }

        public string PageHtml { get; set; }
    }

}