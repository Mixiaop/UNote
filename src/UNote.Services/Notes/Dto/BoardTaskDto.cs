﻿using System.Collections.Generic;
using UNote.Services.Teams.Dto;

namespace UNote.Services.Notes.Dto
{
    /// <summary>
    /// 列表项（任务板模式）
    /// </summary>
    public class BoardTaskDto : BoardTaskBriefDto
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 任务相关人员内容
        /// </summary>
        public string BodyPersons { get; set; }

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

        public BoardColumnBriefDto Column { get; set; }

        /// <summary>
        /// 格式化文件大小（如：0.5M）
        /// </summary>
        public string FormatFileSize { get; set; }

        public string FormatPreviewUrl { get; set; }
    
        public string FormatCreationTime { get; set; }
    }
}
