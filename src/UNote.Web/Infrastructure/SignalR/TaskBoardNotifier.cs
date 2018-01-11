using System;
using U;
using U.AutoMapper;
using U.Web.Models;
using UNote.Services.Notes;
using UNote.Services.Notes.Dto;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace UNote.Web.Infrastructure.SignalR
{
    /// <summary>
    /// 任务板客户端更新通知管理器
    /// </summary>
    public class TaskBoardNotifier
    {
        private readonly static Lazy<TaskBoardNotifier> _instance = new Lazy<TaskBoardNotifier>(
            () => new TaskBoardNotifier(GlobalHost.ConnectionManager.GetHubContext<TaskBoardNotifierHub>().Clients));

        private IBoardService _boardService = UPrimeEngine.Instance.Resolve<IBoardService>();

        private TaskBoardNotifier(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        public static TaskBoardNotifier Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        #region Columns
        public AjaxResponse<BoardColumnDto> CreateColumn(int nodeId, string title, string styleColor)
        {
            AjaxResponse<BoardColumnDto> res = new AjaxResponse<BoardColumnDto>();
            var result = _boardService.CreateColumn(nodeId, title.Trim(), styleColor);
            if (result.Success)
            {
                res.Result = result.Items.MapTo<BoardColumnDto>();
                Clients.All.createColumn(result);
            }
            else
            {
                res.Success = false;
                res.Error = new ErrorInfo(result.Errors[0]);
            }
            return res;
        }
        #endregion

        #region Tasks
        #endregion
    }
}