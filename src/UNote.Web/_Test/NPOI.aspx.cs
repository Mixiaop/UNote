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

namespace UNote.Web._Test
{
    public partial class NPOI : System.Web.UI.Page
    {
        private IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Click += btnSave_Click;
            var path = WebHelper.MapPath("/_test/中学生自我控制能力测评（基础报告）.doc");

            //spire.doc
            //Document document = new Document();
            //document.LoadFromFile(path);

            //document.SaveToFile(WebHelper.MapPath("/_test/sample.html"), FileFormat.Html);

            //var str = WordHelper.ReadWordText(path);
            //Response.Write(str);

            //aspose.words
            //Document doc = new Document(path);
            //HtmlSaveOptions options = new HtmlSaveOptions();

            ////options.ExportRoundtripInformation = true;
            //doc.Save(WebHelper.MapPath("/_test/sample/1.html"), options);

            //Document doc = new Document(path);

            //doc.Save(WebHelper.MapPath("/_test/sample/1.txt"));

            //Response.Write(str);

            //npoi excel to html
           //IWorkbook excel = new HSSFWorkbook()


            var content = _contentService.GetById(43);
            //Document doc = new Document();
            Document doc = new Document(WebHelper.MapPath("/_test/1.doc"));

            //DocumentBuilder builder = new DocumentBuilder(doc);
            ////builder.Write(content.Body);
            //builder.InsertHtml(content.Body);
            //if (!File.Exists(WebHelper.MapPath("/_tes/1.doc"))) { 
            //    File.
            //}
            //doc.Save(WebHelper.MapPath("/_test/1.pdf"));
            //Response.Redirect("/_test/1.pdf");

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            IContentPluginService contentFormatService = UPrimeEngine.Instance.Resolve<IContentPluginService>();

             var result = contentFormatService.UploadWord(fuFile.PostedFile);

             Response.Write(result.Html);
        }

        private void WordDocViewer(string fileName)
        {
            try
            {
                System.Diagnostics.Process.Start(fileName);
            }
            catch { }

        }
    }
}