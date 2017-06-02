using System.Collections.Generic;
using UNote.Domain.Users;
using UNote.Domain.Notes;

namespace UNote.Services.Users
{
    /// <summary>
    /// 用户访问日志服务 
    /// </summary>
    public interface IUserVisitLogService : U.Application.Services.IApplicationService
    {
        /// <summary>
        /// 访问（栏目、内容）记录
        /// 如果访问过则会更新最近访问时间
        /// </summary>
        /// <param name="user"></param>
        /// <param name="typeId">类型（1=栏目 2=内容）</param>
        /// <param name="node"></param>
        /// <param name="content"></param>
        void Visit(User user, int typeId, Node node = null, Content content = null);

        /// <summary>
        /// 最近的访问日志
        /// </summary>
        /// <param name="user"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IList<UserVisitLog> QueryLastVisitLogs(User user, int count = 0);
    }
}
