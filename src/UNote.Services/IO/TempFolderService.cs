using System;
using System.IO;
using U.Utilities.Web;
using UNote.Configuration;

namespace UNote.Services.IO
{
    public class TempFolderService : ITempFolderService
    {
        private UNoteSettings _settings;
        public TempFolderService(UNoteSettings settings)
        {
            _settings = settings;
        }


        public string GetTempFileFolder()
        {

            string path = _settings.TempPath.TrimEnd("/") + "/files/" 
                                                          + (DateTime.Now.Year.ToString() + DateTime.Now.Month)
                                                          + "/" + DateTime.Now.Day;

            InsureDirectory(WebHelper.MapPath(path));

            return path;

        }


        private void InsureDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
