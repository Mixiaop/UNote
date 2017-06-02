using System.Web;
using UNote.Domain.Notes;
using UNote.Services.Notes.Dto;

namespace UNote.Services.Notes
{
    /// <summary>
    /// 笔记内容的扩展服务 
    /// </summary>
    public interface IContentPluginService : U.Application.Services.IApplicationService
    {
        /// <summary>
        /// 上传并解压包文件返回新的相对路径，如：/html/xajeka
        /// </summary>
        /// <returns></returns>
        UploadHtmlPkgOutput UploadHtmlPkg(HttpPostedFile postedFile);

        /// <summary>
        /// 上传word并解析里面的内容到text
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        UploadWordOutput UploadWord(HttpPostedFile postedFile);

        /// <summary>
        /// 通过笔记生成Doc文件（在放临时目录）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        GenerateFileOutput GenerateDoc(Content content);

        /// <summary>
        /// 通过笔记生成Doc文件（在放临时目录）
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        GenerateFileOutput GenerateDoc(int contentId);

        /// <summary>
        /// 通过笔记生成Pdf文件（在放临时目录）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        GenerateFileOutput GeneratePdf(int contentId);

        /// <summary>
        /// 通过笔记生成Pdf文件（在放临时目录）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        GenerateFileOutput GeneratePdf(Content content);
    }
}
