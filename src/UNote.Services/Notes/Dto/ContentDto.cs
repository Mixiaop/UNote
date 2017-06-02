using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNote.Services.Teams.Dto;

namespace UNote.Services.Notes.Dto
{
    public class ContentDto : U.Application.Services.Dto.FullAuditedEntityDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 预览图Id
        /// </summary>
        public int PreviewId { get; set; }

        /// <summary>
        /// 预览图远程路径
        /// </summary>
        public string PreviewUrl { get; set; }

        /// <summary>
        /// 文件Id
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// 文件远程路径
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// 文件大小（字节byte）
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// 文件点击下载次数
        /// </summary>
        public int DownloadCount { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int VisitCount { get; set; }

        /// <summary>
        /// 所属分类 
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// 标签
        /// 多个标签用英文逗号（,）分开
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool Public { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 所属分类信息
        /// </summary>
        public NodeDto Node { get; set; }

        public TeamDto Team { get; set; }

        /// <summary>
        /// 格式化文件大小（如：0.5M）
        /// </summary>
        public string FormatFileSize { get; set; }

        public string FormatPreviewUrl { get; set; }
    }
}
