using System.Web;
using System.IO;
using Newtonsoft.Json;
using U;
using UZeroMedia.Client.Services;
using UNote.Services.Notes;
using UNote.Services.Notes.Dto;

namespace UNote.Web.AjaxServices.Notes
{
    /// <summary>
    /// UploadWordContent 的摘要说明
    /// </summary>
    public class UploadWordContent : IHttpHandler
    {
        private IContentPluginService _contentFormatService = UPrimeEngine.Instance.Resolve<IContentPluginService>();
        public void ProcessRequest(HttpContext context)
        {
            HttpPostedFile qqfile = context.Request.Files["qqfile"];

            StringWriter sw = new StringWriter();
            using (JsonWriter w = new JsonTextWriter(sw))
            {
                w.WriteStartObject();
                if (qqfile == null)
                    context.Response.End();

                bool error = false;
                if (!qqfile.FileName.ToLowerInvariant().Contains(".doc") && !qqfile.FileName.ToLowerInvariant().Contains(".docx"))
                {
                    w.WritePropertyName("error");
                    w.WriteValue("压缩包格式必须为 .doc .docx");
                    w.WriteEndObject();
                    error = true;
                }

                if (!error)
                {

                    var result = _contentFormatService.UploadWord(qqfile);

                    w.WritePropertyName("success");
                    w.WriteValue(true);
                    w.WritePropertyName("html");
                    w.WriteValue(result.Html);
                }

            }
            sw.Close();
            context.Response.Write(sw.GetStringBuilder().ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}