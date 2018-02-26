using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using SevenZip;
using Aspose.Words;
using Aspose.Words.Saving;
using U.Utilities.Web;
using UZeroMedia.Client.Services;
using UNote.Configuration;
using UNote.Domain.Notes;
using UNote.Services.IO;
using UNote.Services.Notes.Dto;

namespace UNote.Services.Notes
{
    public class ContentPluginService : IContentPluginService
    {
        private UNoteSettings _settings;
        private PictureClientService _pictureClientService;
        private IContentService _contentService;
        private ITempFolderService _tempFolderService;
        public ContentPluginService(UNoteSettings settings, PictureClientService pictureClientService, IContentService contentService, ITempFolderService tempFolderService)
        {
            _settings = settings;
            _pictureClientService = pictureClientService;
            _contentService = contentService;
            _tempFolderService = tempFolderService;
        }

        public UploadHtmlPkgOutput UploadHtmlPkg(HttpPostedFile postedFile)
        {
            UploadHtmlPkgOutput result = new UploadHtmlPkgOutput();
            InsureDirectory(WebHelper.MapPath(_settings.NodeHtmlFilePath));


            result.Code = Guid.NewGuid().ToString().Replace("-", "");
            result.RelativePath = _settings.NodeHtmlFilePath;
            if (!_settings.NodeHtmlFilePath.EndsWith("/"))
                result.RelativePath += "/";
            result.RelativePath += result.Code;

            InsureDirectory(WebHelper.MapPath(result.RelativePath));

            #region 保存文件，解压完后删除
            string tempPkgPath = result.RelativePath + "/" + GetPkgTempName(postedFile.FileName);
            postedFile.SaveAs(WebHelper.MapPath(tempPkgPath));

            #region 解压文件
            if (tempPkgPath.Contains("7z"))
            {
                //7z
                if (IntPtr.Size == 4)
                {
                    SevenZipCompressor.SetLibraryPath(WebHelper.MapPath("/bin/x86/7z.dll"));
                }
                else
                {
                    SevenZipCompressor.SetLibraryPath(WebHelper.MapPath("/bin/x64/7z.dll"));
                }
                SevenZipExtractor extractor = new SevenZipExtractor(WebHelper.MapPath(tempPkgPath));
                extractor.ExtractArchive(WebHelper.MapPath(result.RelativePath));
            }
            else
            {
                //zip gz
                CommonHelper.UnZip(WebHelper.MapPath(tempPkgPath), WebHelper.MapPath(result.RelativePath));
            }
            
            #endregion
            //删除默认文件
            File.Delete(WebHelper.MapPath(tempPkgPath));
            #endregion
            var folders = Directory.GetDirectories(WebHelper.MapPath(result.RelativePath));
            if (folders.Count() == 1)
            {
                result.RelativePath += "/" + Path.GetFileName(folders[0]);
            }

            result.RelativeHomePage = result.RelativePath + "/index.html";
            return result;
        }

        /// <summary>
        /// 上传word并解析里面的内容到text
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        public UploadWordOutput UploadWord(HttpPostedFile postedFile)
        {
            UploadWordOutput result = new UploadWordOutput();
            InsureDirectory(WebHelper.MapPath(_settings.TempPath));

            var code = Guid.NewGuid().ToString().Replace("-", "");
            var relativePath = _settings.TempPath;
            if (!_settings.TempPath.EndsWith("/"))
                relativePath += "/";
            relativePath += code;

            InsureDirectory(WebHelper.MapPath(relativePath));

            #region word保存到html
            string tempHtmlPath = WebHelper.MapPath(relativePath + "/w.html");
            string tempWordPath = WebHelper.MapPath(relativePath + "/w" + Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(tempWordPath);

            Document doc = new Document(tempWordPath);
            HtmlSaveOptions options = new HtmlSaveOptions();

            options.ExportRoundtripInformation = true;
            doc.Save(tempHtmlPath, options);

            //从w.html 获取字符串
            result.Html = File.ReadAllText(tempHtmlPath);
            //处理图片
            var imgs = HtmlHelper.GetImgUrls(result.Html);
            if (imgs != null)
            {
                foreach (var img in imgs)
                {
                    //上传图片并替换
                    string imgUrl = relativePath + "/" + img;
                    var resPicture = _pictureClientService.Upload(WebHelper.MapPath(imgUrl));
                    if (resPicture.IsSuccess())
                    {
                        if (resPicture.Results != null)
                        {
                            result.Html = result.Html.Replace(img, resPicture.Results.ShowPictureUrl);
                        }
                    }
                }
            }

            RegexOptions regOptions = RegexOptions.None;
            Regex reg = new Regex(@"<body([\s\S^>]+?)>([\s\S]+?)</body>", regOptions);
            result.Html = reg.Match(result.Html).Groups[0].ToString();
            //Aspose
            result.Html = result.Html.Replace("Evaluation Only. Created with Aspose.Words. Copyright 2003-2016 Aspose Pty Ltd.", "");
            result.Html = result.Html.Replace("This document was truncated here because it was created in the Evaluation Mode.", "");

            //删除临时文件
            Directory.Delete(WebHelper.MapPath(relativePath), true);
            #endregion
            return result;
        }

        /// <summary>
        /// 通过笔记生成Doc文件（在放临时目录）
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public GenerateFileOutput GenerateDoc(int contentId)
        {
            var content = _contentService.GetById(contentId);
            return GenerateDoc(content);
        }

        /// <summary>
        /// 通过笔记生成Doc文件（在放临时目录）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public GenerateFileOutput GenerateDoc(Content content)
        {
            GenerateFileOutput result = new GenerateFileOutput();
            if (content != null)
            {
                result.FileName = content.Title + ".doc";
                result.Url = _tempFolderService.GetTempFileFolder().TrimEnd('/') + "/" + result.FileName;
                result.Path = WebHelper.MapPath(result.Url);

                //保存word
                Document doc = new Document();
                DocumentBuilder builder = new DocumentBuilder(doc);
                builder.InsertHtml(content.Body);
                doc.Save(result.Path);
            }
            return result;
        }

        /// <summary>
        /// 通过笔记生成Pdf文件（在放临时目录）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public GenerateFileOutput GeneratePdf(int contentId)
        {
            var content = _contentService.GetById(contentId);
            return GeneratePdf(content);


        }

        /// <summary>
        /// 通过笔记生成Pdf文件（在放临时目录）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public GenerateFileOutput GeneratePdf(Content content)
        {
            GenerateFileOutput result = new GenerateFileOutput();
            if (content != null)
            {
                result.FileName = content.Title + ".pdf";
                result.Url = _tempFolderService.GetTempFileFolder().TrimEnd('/') + "/" + result.FileName;
                result.Path = WebHelper.MapPath(result.Url);

                var docUrl = GenerateDoc(content);
                Document doc = new Document(docUrl.Path);
                doc.Save(result.Path);
            }
            return result;   
        }

        #region Utilities
        private void InsureDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void InsureFile(string path)
        {
            if (!File.Exists(path))
            {
            }
        }

        private string GetPkgTempName(string fileName)
        {
            string tempName = "temp";
            if (fileName.ToLower().Contains(".zip"))
            {
                tempName += ".zip";
            }
            else if (fileName.ToLower().Contains(".gz"))
            {
                tempName += ".gz";
            }
            else if (fileName.ToLower().Contains(".7z")) {
                tempName += ".7z";
            }
            else
            {
                throw new Exception("pkg包后缀有误[zip、gz]");
            }

            return tempName;
        }
        #endregion
    }
}
