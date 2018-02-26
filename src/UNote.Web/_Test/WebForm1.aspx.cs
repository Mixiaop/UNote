using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

using U;
using U.Net.Mail;
using U.Utilities.Web;
using UNote.Domain.Notes;
using UNote.Services.Notes;
using UNote.Services.Users;
using UNote.Services.Authrization;
using UNote.Services.IO;
using U.Net.Mail;
using U.Net.Mail.Smtp;
using SevenZip;

namespace UNote.Web._Test
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //INodeService nodeService = UPrimeEngine.Instance.Resolve<INodeService>();

            ////var node = nodeService.GetById(9);

            ////Response.Write(JsonConvert.SerializeObject(node));
            //var node = new Node();
            //node.ParentId = 0;
            //node.NodeName = "架构设计";
            //node.Icon = "si-plus";
            //node.Order = 2;
            //node.Public = false;
            //nodeService.Insert(node);

            //var emailSender = UPrimeEngine.Instance.Resolve<IEmailSender>();
            //lSender.Send("zhanghp@eagersoft.cn", "测试主题", "测试内容");
            //sponse.Write("成功了");

            //Response.Write(Guid.NewGuid().ToString().Replace("-", ""));

            //var url = "/_test/test.zip";
            //var path = WebHelper.MapPath(url);

            //CommonHelper.UnZip(path, WebHelper.MapPath("/_test/test"));

            //Response.Write("成功了");

            //Response.Write(System.IO.Path.GetRandomFileName());

            //IOnlineUserService onlineUserService = UPrimeEngine.Instance.Resolve<IOnlineUserService>();
            //IAuthenticationService authenticationService = UPrimeEngine.Instance.Resolve<IAuthenticationService>();

            //if (authenticationService.GetAuthenticatedUser() != null)
            //{
            //    onlineUserService.Join(authenticationService.GetAuthenticatedUser());
            //}
            //Response.Write("在线用户：<br />");
            //foreach (var user in onlineUserService.GetOnlineUsers())
            //{
            //    Response.Write(user.NickName + "<br />");
            //}

            //ITempFolderService _tempFolderService = UPrimeEngine.Instance.Resolve<ITempFolderService>();

            //Response.Write(_tempFolderService.GetTempFileFolder());

            //var emailSender = UPrimeEngine.Instance.Resolve<IEmailSender>();
            //emailSender.Send("zhanghp@eagersoft.cn", "【需求&BUG跟踪】【B端PC、H5】【测评、第三方登录】跟踪", "笔记更新 => <a href='http://note.youzy.cn/Notes/ContentInfo.aspx?contentid=682' target='_blank'>【B端PC、H5】【测评、第三方登录】跟踪】</a>");
            //Response.Write("成功了");
            //CommonHelper.UnZip(WebHelper.MapPath("/_Test/pic.7z"), WebHelper.MapPath("/_Test/pic/"));

            //   SevenZipCompressor compressor = new SevenZipCompressor();
            //if (IntPtr.Size == 4)
            //{
            //    SevenZipCompressor.SetLibraryPath(@"7z.dll");
            //}
            //else
            //{
            //SevenZipCompressor.SetLibraryPath(WebHelper.MapPath("/bin/x64/7z.dll"));
            //}

            if (IntPtr.Size == 4)
            {
                SevenZipCompressor.SetLibraryPath(WebHelper.MapPath("/bin/x86/7z.dll"));
            }
            else
            {
                SevenZipCompressor.SetLibraryPath(WebHelper.MapPath("/bin/x64/7z.dll"));
            }
            SevenZipExtractor extractor = new SevenZipExtractor(WebHelper.MapPath("/_Test/pic.7z"));
            //extractor.ExtractFiles()
            //extractor.ExtractArchive(WebHelper.MapPath("/_test/pic/"));
            //extractor.ExtractFiles(WebHelper.MapPath("/_Test/pic.7z"), WebHelper.MapPath("/_test/pic/"));
        }
    }
}