using System;
using System.Web;
using System.IO;
using System.Text;
using U;
using U.Utilities.Web;
using UNote.Services.Notes;
using UNote.Services.Notes.Dto;

namespace UNote.Web.Notes
{
    public partial class Download : Infrastructure.UserPage
    {
        private IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();
        private IContentPluginService _contentPluginService = UPrimeEngine.Instance.Resolve<IContentPluginService>();
        protected void Page_Load(object sender, EventArgs e)
        {
            var contentId = WebHelper.GetInt("contentId", 0);
            var type = WebHelper.GetString("type");
            try
            {
                var content = _contentService.GetById(contentId);
                GenerateFileOutput result = null;
                if (type == "doc")
                {
                    result = _contentPluginService.GenerateDoc(content);
                }
                else if (type == "pdf")
                {
                    result = _contentPluginService.GeneratePdf(content);
                }
                else {
                    throw new Exception("下载类型有误");
                }


                FileStream fileStream = new FileStream(result.Path, FileMode.Open);
                long fileSize = fileStream.Length;
                byte[] fileBuffer = new byte[fileSize];
                fileStream.Read(fileBuffer, 0, (int)fileSize);
                fileStream.Close();

                Context.Response.ContentType = "application/octet-stream";
                Context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(result.FileName, Encoding.UTF8));
                Context.Response.AddHeader("Content-Length", fileSize.ToString());

                Context.Response.BinaryWrite(fileBuffer);
                Context.Response.End();
                Context.Response.Close();

            }
            catch (Exception ex)
            {
                Response.Write("请检查笔记标识【contentId：" + contentId + "】是否存在");
            }
        }
    }
}