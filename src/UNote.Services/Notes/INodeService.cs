using System.Collections.Generic;
using U.Application.Services.Dto;
using UNote.Domain.Notes;

namespace UNote.Services.Notes
{
    /// <summary>
    /// “笔记的分类”应用服务
    /// </summary>
    public interface INodeService : U.Application.Services.IApplicationService
    {

        /// <summary>
        /// 通过关键字查询分类 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keywords"></param>
        /// <param name="isAdmin"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        PagedResultDto<Node> Query(int pageIndex, int pageSize = 20, string keywords = "", bool? isAdmin = null, bool? isPublic = null);

        /// <summary>
        /// 获取某个用户所有的分类 
        /// </summary>
        /// <param name="userId">0 = 管理员</param>
        /// <returns></returns>
        IList<Node> GetAll(int userId = 0);

        /// <summary>
        /// 获取团队的所有目录
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        IList<Node> GetAllByTeam(int teamId);

        IList<Node> GetAllByTeams(List<int> teamIds);

        /// <summary>
        /// 获取热门的分类，内容多的从高到低
        /// </summary>
        /// <param name="top">前多少条</param>
        /// <returns></returns>
        IList<Node> GetTopList(int top);

        /// <summary>
        /// 获取父类目录
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
       //List<Node> GetParents(Node node);

        /// <summary>
        /// 是否存在同样的分类名称
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        bool ExistsNodeName(string nodeName, int userId = 0);

        bool ExistsNodeNameByTeam(string nodeName, int teamId);

        /// <summary>
        /// 通过 Id 获取分类信息
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Node GetById(int nodeId);

        /// <summary>
        /// 添加一个新的分类
        /// </summary>
        /// <param name="node"></param>
        int Insert(Node node);

        /// <summary>
        /// 更新一个分类 
        /// </summary>
        /// <param name="node"></param>
        void Update(Node node);

        void Delete(int nodeId);

        void Delete(Node node);
    }
}
