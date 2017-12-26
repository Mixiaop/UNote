using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using U.Utilities.Web;
//using Spire.Doc;
using Aspose.Words;
using Aspose.Words.Saving;
using U;
using UNote.Services.Notes;
using Aspose.Words;

namespace UNote.Web._Test
{
    public partial class downword : System.Web.UI.Page
    {
        private IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();
        protected void Page_Load(object sender, EventArgs e)
        {
            //string sFileName = WebHelper.MapPath("/_test/1.doc");
            //FileStream fileStream = new FileStream(sFileName, FileMode.Open);
            //long fileSize = fileStream.Length;
            //byte[] fileBuffer = new byte[fileSize];
            //fileStream.Read(fileBuffer, 0, (int)fileSize);
            //fileStream.Close();

            //Context.Response.ContentType = "application/octet-stream";
            //Context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(sFileName, Encoding.UTF8));
            //Context.Response.AddHeader("Content-Length", fileSize.ToString());

            //Context.Response.BinaryWrite(fileBuffer);
            //Context.Response.End();
            //Context.Response.Close();
            Document doc = new Document(@"D:\_海鹏的工作日志\2017 12月\江苏省溧阳市竹箦中学2017届高三上学期期初考试英语试题 Word版含答案.doc");
            foreach (Section section in doc)
            {
                HeaderFooter footer;
                section.ClearHeadersFooters();
               


            }


            // Save the document.
            doc.Save(@"D:\_海鹏的工作日志\2017 12月\11_out.doc");
            Response.Write("sus");
        }
    }
}