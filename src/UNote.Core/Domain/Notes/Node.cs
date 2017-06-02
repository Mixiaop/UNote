using U.Domain.Entities.Auditing;
using UNote.Domain.Teams;

namespace UNote.Domain.Notes
{
    /// <summary>
    /// 代表一个“笔记分类”
    /// </summary>
    public class Node : FullAuditedEntity
    {
        public Node() {
            NodeName = "";
            Alias = "";
            Icon = "";
            TeamId = 0;
            ParentId = 0;
            ParentsPath = "";
            ContentTotal = 0;
            Description = "";
            Order = 0;
            Public = false;
            UserId = 0;
            NodeType = UNote.NodeType.Normal;
            ListShowType = NodeListShowType.List;
            CustomUrl = "";
        }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 团队Id
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// 父节点Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 目录类型Id
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// 列表显示类型Id
        /// </summary>
        public int ListShowTypeId { get; set; }

        /// <summary>
        /// icon
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 父节点路径（多级时代表的几级类，如：1,2）
        /// </summary>
        public string ParentsPath { get; set; }

        /// <summary>
        /// 当前分类的笔记总数
        /// </summary>
        public int ContentTotal { get; set; }

        /// <summary>
        /// 描述介绍
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否公开（默认 true）
        /// false = 只有自己的帐号能看到
        /// </summary>
        public bool Public { get; set; }

        /// <summary>
        /// 创建者用户Id
        /// 小于等于0： 为管理者添加
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 自定义链接
        /// </summary>
        public string CustomUrl { get; set; }

        #region Navigation Properties

        public virtual Team Team { get; set; }

        #endregion

        #region Custom Properties
        /// <summary>
        /// 父级信息
        /// </summary>
        public Node Parent { get; set; }

        /// <summary>
        /// 目录类型
        /// </summary>
        public NodeType NodeType {
            get { return (NodeType)NodeTypeId; }
            set { NodeTypeId = (int)value; }
        }

        /// <summary>
        /// 列表显示类型
        /// </summary>
        public NodeListShowType ListShowType {
            get { return (NodeListShowType)ListShowTypeId; }
            set { ListShowTypeId = (int)value; }
        }
        #endregion
    }
}
