using System.Collections.Generic;
using System.Linq;
using U.Application.Services.Dto;
using U.UI;
using UNote.Domain.Notes;
using UNote.Domain.Teams;
using UNote.Services.Teams;

namespace UNote.Services.Notes
{
    public class NodeService : INodeService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly ITeamService _teamService;

        public NodeService(INodeRepository nodeRepository, ITeamService teamService)
        {
            _nodeRepository = nodeRepository;
            _teamService = teamService;
        }


        public PagedResultDto<Node> Query(int pageIndex, int pageSize = 20, string keywords = "", bool? isAdmin = null, bool? isPublic = null)
        {
            var query = _nodeRepository.GetAll();
            if (keywords.IsNotNullOrEmpty())
            {
                query = query.Where(x => x.NodeName.Contains(keywords));
            }
            if (isAdmin.HasValue)
            {
                if (isAdmin.Value)
                    query = query.Where(x => x.UserId == 0);
                else
                    query = query.Where(x => x.UserId > 0);
            }

            if (isPublic.HasValue)
            {
                query = query.Where(x => x.Public == isPublic);
            }

            var list = query.OrderByDescending(x => x.CreationTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var count = query.Count();
            return new PagedResultDto<Node>(count, (List<Node>)list);
        }

        /// <summary>
        /// 获取热门的分类，内容多的从高到低
        /// </summary>
        /// <param name="top">前多少条</param>
        /// <returns></returns>
        public IList<Node> GetTopList(int top)
        {
            var query = _nodeRepository.GetAll();

            var list = query.OrderByDescending(x => x.ContentTotal).Take(top).ToList();
            return list;
        }

        public IList<Node> GetAll(int userId = 0)
        {
            var list = _nodeRepository.GetAllList(x => x.UserId == userId && x.TeamId == 0).OrderBy(x => x.Order).ToList();

            return list;
        }

        public IList<Node> GetAllByTeam(int teamId) {
            var list = _nodeRepository.GetAllList(x => x.TeamId == teamId).OrderBy(x => x.Order).ToList();

            return list;
        }

        public IList<Node> GetAllByTeams(List<int> teamIds) {
            var list = _nodeRepository.GetAllList(x => teamIds.Contains(x.TeamId)).OrderBy(x => x.Order).ToList();

            return list;
        }

        //public IList<Node> GetParents(Node node) { 
        
        //}

        public bool ExistsNodeName(string nodeName, int userId = 0)
        {
            var count = _nodeRepository.Count(x => x.UserId == userId && x.NodeName == nodeName);
            return count > 0;
        }

        public bool ExistsNodeNameByTeam(string nodeName, int teamId) {
            var count = _nodeRepository.Count(x => x.TeamId == teamId && x.NodeName == nodeName);
            return count > 0;
        }

        public Node GetById(int nodeId)
        {
            var node = _nodeRepository.Get(nodeId);
            if (node == null)
                throw new UserFriendlyException("Node is null");
            return node;
        }

        public int Insert(Node node)
        {
            if (node.NodeName.IsNullOrEmpty())
            {
                throw new UserFriendlyException("分类名称不能为空");
            }

            if (node.TeamId > 0)
            {
                if (ExistsNodeNameByTeam(node.NodeName, node.TeamId))
                    throw new UserFriendlyException("已存在同名的分类名称 [" + node.NodeName + "]");
            }
            else {
                if (ExistsNodeName(node.NodeName, node.UserId))
                    throw new UserFriendlyException("已存在同名的分类名称 [" + node.NodeName + "]");
            }
            

            int parentSonCount = 0;
            //如果有父节点
            if (node.ParentId > 0)
            {
                var parent = GetById(node.ParentId);
                if (parent == null)
                    throw new UserFriendlyException("父节点未找到 [ParentId " + node.ParentId + "]");

                parentSonCount = _nodeRepository.Count(x => x.ParentId == parent.Id && x.UserId == node.UserId);

                if (parent.ParentsPath.IsNotNullOrEmpty())
                    node.ParentsPath = parent.ParentsPath + "," + parent.Id;
                else
                    node.ParentsPath = parent.Id.ToString();

            }
            else
            {
                parentSonCount = _nodeRepository.Count(x => x.UserId == node.UserId && x.ParentId == 0);
            }

            //团队
            if (node.TeamId > 0) { 
                var team= _teamService.GetById(node.TeamId);
                if (team != null) {
                    team.NodeCount++;
                    _teamService.Update(team);
                }
            }
            //node.Order = ++parentSonCount;

            node.Id = _nodeRepository.InsertAndGetId(node);
            return node.Id;
        }

        public void Update(Node node)
        {
            if (node.NodeName.IsNullOrEmpty())
            {
                throw new UserFriendlyException("NodeName is invalid");
            }

            _nodeRepository.Update(node);
        }

        public void Delete(int nodeId)
        {
            var node = GetById(nodeId);
            
            Delete(node);
        }
        public void Delete(Node node)
        {

            if (node.ContentTotal > 0)
                throw new UserFriendlyException("节点 [" + node.NodeName + "] 下还有内容，请先删除后再操作。");

            //团队
            if (node.TeamId > 0)
            {
                var team = _teamService.GetById(node.TeamId);
                if (team != null)
                {
                    team.NodeCount--;
                    _teamService.Update(team);
                }
            }

            _nodeRepository.Delete(node);
        }
    }
}
