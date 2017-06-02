
namespace UNote.Services.Notes.Dto
{
    public class UploadHtmlPkgOutput : U.Application.Services.Dto.IOutputDto
    {
        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// 相对的默认首页
        /// </summary>
        public string RelativeHomePage { get; set; }

        /// <summary>
        /// 文件标识码
        /// </summary>
        public string Code { get; set; }
    }
}
