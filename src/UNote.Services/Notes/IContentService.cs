using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U.Application.Services.Dto;
using UNote.Domain.Notes;
using UNote.Domain.Users;

namespace UNote.Services.Notes
{
    /// <summary>
    /// “笔记内容”的应用服务
    /// </summary>
    public interface IContentService : U.Application.Services.IApplicationService
    {
        #region Contents
        /// <summary>
        /// 查询所有的笔记内容
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <param name="keywords"></param>
        /// <param name="nodeIds"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        PagedResultDto<Content> Query(int pageIndex, int pageSize = 20, int userId = -1, string keywords = "", IList<int> nodeIds = null, IList<int> teamIds = null, string tag = "");

        PagedResultDto<Content> QueryPublicContents(int pageIndex = 1, int pageSize = 20, string keywords = "");

        /// <summary>
        /// 获取某个用户最近的笔记，包含他所在的Team
        /// </summary>
        /// <param name="user"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IList<Content> QueryRecentNotes(User user, int count = 10);

        PagedResultDto<Content> QueryRecentNotes(User user, int pageIndex = 1, int pageSize = 20);

        int Count(int userId);

        /// <summary>
        /// 获取一条笔记内容
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        Content GetById(int contentId);

        /// <summary>
        /// 添加一条笔记内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        int Insert(Content content);

        /// <summary>
        /// 更新一条笔记内容
        /// </summary>
        /// <param name="content"></param>
        /// <param name="oldNodeId">只有更新目录节点时用到</param>
        void Update(Content content, int oldNodeId = 0);

        void Delete(int contentId);

        /// <summary>
        /// 删除一条笔记
        /// </summary>
        /// <param name="content"></param>
        void Delete(Content content);

        /// <summary>
        /// 浏览次数叠加
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="info"></param>
        /// <param name="userId"></param>
        void AddVisitCount(int contentId, Content info = null, int userId = 0);

        #endregion

        #region ContentItems
        void InsertItem(Content content, Content item);
        IList<Content> GetAllItems(int contentId);

        #endregion
    }
}
