using U.CodeAnnotations;
namespace UNote
{
    public enum UserType
    {
        /// <summary>
        /// 普通用户名
        /// </summary>
        General = 1,
        /// <summary>
        /// 邮箱
        /// </summary>
        Email = 2
    }

    /// <summary>
    /// 目录类型
    /// </summary>
    public enum NodeType { 
        /// <summary>
        /// 普通(上传文件带editor）
        /// </summary>
        [EnumAlias("通用")]
        Normal = 1,
        /// <summary>
        /// HTML 插件展示 （JS或产品HTML展示）
        /// </summary>
        [EnumAlias("HTML组件")]
        Html = 2,
        /// <summary>
        /// Word组件
        /// </summary>
        [EnumAlias("Word组件")]
        Word = 3
    }

    /// <summary>
    /// 目录列表显示类型
    /// </summary>
    public enum NodeListShowType { 
        /// <summary>
        /// 普通列表
        /// </summary>
        List = 1,
        /// <summary>
        /// 网格
        /// </summary>
        Grid = 2
    }
}
