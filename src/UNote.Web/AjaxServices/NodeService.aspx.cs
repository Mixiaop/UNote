using System;
using AjaxPro;
using U;
using U.UI;
using U.AutoMapper;
using U.Web.Models;
using UNote.Domain.Notes;
using UNote.Domain.Teams;
using UNote.Services.Notes;
using UNote.Services.Teams;
using UNote.Services.Notes.Dto;
using UNote.Web.AjaxServices.Dto;

namespace UNote.Web.AjaxServices
{
    [AjaxNamespace("NodeService")]
    public partial class NodeService : Infrastructure.AjaxPageBase
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        ITeamService _teamService = UPrimeEngine.Instance.Resolve<ITeamService>();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [AjaxMethod]
        public AjaxResponse<NodeDto> Get(int nodeId) {
            AjaxResponse<NodeDto> res = new AjaxResponse<NodeDto>();

            try
            {
                var node =_nodeService.GetById(nodeId);
                if (node != null && node.ParentId > 0)
                    node.Parent = _nodeService.GetById(node.ParentId);
                res.Result = node.MapTo<NodeDto>();
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }

        [AjaxMethod]
        public AjaxResponse<int> Update(InsertOrUpdateNodeInput input) {
            AjaxResponse<int> res = new AjaxResponse<int>();

            try
            {
                if (IsLogined())
                {
                    var node = _nodeService.GetById(input.Id);
                    if (node != null)
                    {
                        node.ParentId = input.ParentId;
                        node.NodeName = input.Name;
                        node.Icon = input.Icon;
                        node.Description = input.Des;
                        node.Order = input.Order;
                        node.Public = input.Public;
                        node.NodeTypeId = input.NodeTypeId;
                        _nodeService.Update(node);
                    }
                }
                else
                {
                    res = GetUserTimeoutErrorInfo<int>();
                }
            }
            catch (UserFriendlyException ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }


        /// <summary>
        /// 添加一个笔记目录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AjaxMethod]
        public AjaxResponse<int> Insert(InsertOrUpdateNodeInput input) {
            AjaxResponse<int> res = new AjaxResponse<int>();
            
            try {
                if (IsLogined())
                {
                    var node = new Node();
                    node.ParentId = input.ParentId;
                    node.NodeName = input.Name;
                    node.Icon = input.Icon;
                    node.Description = input.Des;
                    node.Order = input.Order;
                    node.Public = input.Public;
                    node.UserId = GetLoginedUser().Id;
                    node.NodeTypeId = input.NodeTypeId;
                    node.CustomUrl = input.CustomUrl;

                    if (input.TeamKey.IsNotNullOrEmpty())
                    {
                        var team = _teamService.GetByKey(input.TeamKey);
                        if (team != null)
                            node.TeamId = team.Id;
                    }
                    var nodeId = _nodeService.Insert(node);
                    res.Result = nodeId;
                }
                else {
                    res = GetUserTimeoutErrorInfo<int>();
                }
            }
            catch (UserFriendlyException ex) {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }

        [AjaxMethod]
        public AjaxResponse Delete(int nodeId) {
            AjaxResponse res = new AjaxResponse();
            try {
                _nodeService.Delete(nodeId);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 1=list 2=grid
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="listShowType"></param>
        /// <returns></returns>
        [AjaxMethod]
        public AjaxResponse SetListShowType(int nodeId, int listShowType) {
            AjaxResponse res = new AjaxResponse();
            try
            {
                if (listShowType != 1 && listShowType != 2) listShowType = 1;

                var node = _nodeService.GetById(nodeId);
                node.ListShowTypeId = listShowType;
                _nodeService.Update(node);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }
    }
    
}