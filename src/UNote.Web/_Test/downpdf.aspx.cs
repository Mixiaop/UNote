using System;
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
    public partial class downpdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string sFileName = WebHelper.MapPath("/_test/1.pdf");
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

            Response.Write(Get(1, (id) =>
            {
                return new Animal() { Name = "haha" + id };
            }).Name);
        }

        public Animal Get(int id, Func<int, Animal> func)
        {
            return func(id);
        }

        public class Animal
        {
            public string Name { get; set; }
        }
    }
}