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
    /// 上传HTML压缩包内容，并解析到/html 目录下
    /// </summary>
    public class UploadHtmlContent : IHttpHandler
    {
        private IContentPluginService _htmlService = UPrimeEngine.Instance.Resolve<IContentPluginService>();
        public void ProcessRequest(HttpContext context)
        {
            FileClientService fileService = new FileClientService();
            HttpPostedFile qqfile = context.Request.Files["qqfile"];

            StringWriter sw = new StringWriter();
            using (JsonWriter w = new JsonTextWriter(sw))
            {
                w.WriteStartObject();
                if (qqfile == null)
                    context.Response.End();

                bool error = false;
                if (!qqfile.FileName.ToLowerInvariant().Contains(".zip") && !qqfile.FileName.ToLowerInvariant().Contains(".gz") && !qqfile.FileName.ToLowerInvariant().Contains(".7z"))
                {
                    w.WritePropertyName("error");
                    w.WriteValue("压缩包格式必须为 .zip .gz");
                    w.WriteEndObject();
                    error = true;
                }

                if (!error)
                {
                    

                    var file = fileService.Upload(qqfile);

                    if (file != null && file.Results != null)
                    {
                        w.WritePropertyName("fileId");
                        w.WriteValue(file.Results.FileId);
                        w.WritePropertyName("fileUrl");
                        w.WriteValue(file.Results.FileUrl);
                        w.WritePropertyName("fileSize");
                        w.WriteValue(qqfile.ContentLength.ToString());
                        w.WritePropertyName("success");
                        w.WriteValue(true);
                        
                        //上传并解压包文件
                        var result = _htmlService.UploadHtmlPkg(qqfile);
                        w.WritePropertyName("htmlHomePage");
                        w.WriteValue(result.RelativeHomePage);
                        w.WritePropertyName("htmlCode");
                        w.WriteValue(result.Code);
                        w.WriteEndObject();
                    }
                    else
                    {
                        w.WritePropertyName("fileId");
                        w.WriteValue("");
                        w.WritePropertyName("fileUrl");
                        w.WriteValue("");
                        w.WritePropertyName("fileSize");
                        w.WriteValue("");
                        w.WriteEndObject();
                    }
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