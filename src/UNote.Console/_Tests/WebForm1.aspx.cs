using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using U;
using UZeroMedia.Client.Services;

namespace UNote.Console._Tests
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(SignatureHelper.GetEncodeSign());
            //var mediaSettings = UPrimeEngine.Instance.Resolve<DatabaseSettings>();
            //var pictureService = UPrimeEngine.Instance.Resolve<UZeroMedia.Domain.IPictureRepository>();
            //pictureService.Insert(new Domain.Picture());
            Response.Write(System.Web.HttpUtility.UrlEncode("你好"));
            btnUpload.Click += BtnUpload_Click;
            Response.Write(CommonHelper.FormatFileSize(481512));
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            //PictureService pictureService = UPrimeEngine.Instance.Resolve<PictureService>();
            //var result = pictureService.Upload(fuUpload.PostedFile);
            FileClientService fileService = UPrimeEngine.Instance.Resolve<FileClientService>();
            var result = fileService.Upload(fuUpload.PostedFile);
            Response.Write(JsonConvert.SerializeObject(result));
        }
    }
}