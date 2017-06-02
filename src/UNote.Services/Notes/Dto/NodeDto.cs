
namespace UNote.Services.Notes.Dto
{
    /// <summary>
    /// 代表一个“笔记分类”
    /// </summary>
    public class NodeDto : U.Application.Services.Dto.FullAuditedEntityDto
    {

        /// <summary>
        /// 分类名称
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 父节点Id
        /// </summary>
        public int ParentId { get; set; }

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

        public int NodeTypeId { get; set; }

        /// <summary>
        /// 父级信息
        /// </summary>
        public NodeDto Parent { get; set; }

        public NodeType NodeType { get; set; }
    }
}
