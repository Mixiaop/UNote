using System;
using System.Collections.Generic;
using U.Domain.Entities.Auditing;
using UNote.Domain.Users;
using UNote.Domain.Teams;

namespace UNote.Domain.Notes
{
    /// <summary>
    /// 代表一个“内容”
    /// </summary>
    public class Content : FullAuditedEntity
    {
        public Content()
        {
            Title = "";
            Body = "";
            PreviewId = 0;
            PreviewUrl = "";
            FileId = 0;
            FileUrl = "";
            FileSize = 0;
            NodeId = 0;
            VisitCount = 1;
            DownloadCount = 0;
            Tag = "";
            Public = false;
            UserId = 0;
            TeamId = 0;
            NodeHtmlFileCode = "";
            NodeHtmlHomePage = "";
            IsTop = false;
        }

        /// <summary>
        /// 团队Id
        /// </summary>
        public int TeamId { get; set; }

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
        /// HTML目录文件标识，如（F0001）
        /// </summary>
        public string NodeHtmlFileCode { get; set; }

        /// <summary>
        /// 首页相对路径（HTML目录）
        /// </summary>
        public string NodeHtmlHomePage { get; set; }

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
        /// 目录类型Id（默认为继承上级目录）
        /// </summary>
        public int NodeTypeId { get; set; }

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
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 父内容Id（0=内容，1=内容项）
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 内容项数量 
        /// </summary>
        public int ContentItemCount { get; set; }

        #region Navigation Properties
        /// <summary>
        /// 所属分类信息
        /// </summary>
        public virtual Node Node { get; set; }

        public virtual Team Team { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public virtual User User { get; set; }
        #endregion

        #region Custom Properties
        /// <summary>
        /// 目录类型
        /// </summary>
        public NodeType FormatNodeType
        {
            get
            {
                if (NodeTypeId == 0 && Node != null)
                    return Node.NodeType;
                else
                    return (NodeType)NodeTypeId;
            }
            set { NodeTypeId = (int)value; }
        }

        /// <summary>
        /// 格式化文件大小（如：0.5M）
        /// </summary>
        public string FormatFileSize
        {
            get { return CommonHelper.FormatFileSize(FileSize); }
        }

        public string FormatPreviewUrl { get; set; }

        /// <summary>
        /// 显示友好名称
        /// </summary>
        public string FormatCreationTime
        {
            get { return CommonHelper.FormatTimeNote(CreationTime, CreationTime.ToString("yy-MM-dd HH:mm")); }
        }

        public string FormatLastModificationTime
        {
            get
            {
                return LastModificationTime.HasValue ?
                    CommonHelper.FormatTimeNote(LastModificationTime.Value, LastModificationTime.Value.ToString("yy-MM-dd HH:mm")) : "";
            }
        }

        private IList<string> _formatTags = new List<string>();
        /// <summary>
        /// 格式化标签
        /// </summary>
        public IList<string> FormatTags
        {
            get
            {
                _formatTags.Clear();
                if (Tag.IsNotNullOrEmpty())
                {
                    string[] list = Tag.Split(',');
                    foreach (var s in list)
                        _formatTags.Add(s);
                }

                return _formatTags;
            }
        }

        /// <summary>
        /// 关注的人
        /// </summary>
        public IList<ContentFollower> Followers { get; set; }

        /// <summary>
        /// 内容项
        /// </summary>
        public IList<Content> ContentItems { get; set; }
        #endregion
    }
}
