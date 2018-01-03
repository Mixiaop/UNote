using System;
using System.Collections.Generic;
using AjaxPro;
using U;
using U.AutoMapper;
using U.Web.Models;
using UNote.Services.Notes;
using UNote.Services.Notes.Dto;

namespace UNote.Web.AjaxServices
{
    [AjaxNamespace("BoardService")]
    /// <summary>
    /// Note任务板模式使用的Ajax service
    /// </summary>
    public partial class BoardService : Infrastructure.AjaxPageBase
    {
        IBoardService _boardService = UPrimeEngine.Instance.Resolve<IBoardService>();
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Columns
        /// <summary>
        /// 获取所有任务列表
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        [AjaxMethod]
        public AjaxResponse<IList<BoardColumnDto>> GetAllColumns(int nodeId)
        {
            AjaxResponse<IList<BoardColumnDto>> res = new AjaxResponse<IList<BoardColumnDto>>();

            try
            {
                var result = _boardService.GetAllColumns(nodeId);

                res.Result = result.MapTo<IList<BoardColumnDto>>();
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 创建一个任务列表
        /// </summary>
        /// <param name="nodeId">分类Id</param>
        /// <param name="title">标题</param>
        /// <returns></returns>
        [AjaxMethod]
        public AjaxResponse<BoardColumnDto> CreateColumn(int nodeId, string title, string styleColor)
        {
            AjaxResponse<BoardColumnDto> res = new AjaxResponse<BoardColumnDto>();
            var result = _boardService.CreateColumn(nodeId, title.Trim(), styleColor);
            if (result.Success)
            {
                res.Result = result.Items.MapTo<BoardColumnDto>();
            }
            else
            {
                res.Success = false;
                res.Error = new ErrorInfo(result.Errors[0]);
            }
            return res;
        }

        [AjaxMethod]
        public AjaxResponse ResetColumnOrders(int[] columnIds)
        {
            AjaxResponse res = new AjaxResponse();

            try
            {
                var result = _boardService.ResetColumnOrders(columnIds);
                if (!result.Success)
                {
                    res.Success = false;
                    res.Error = new ErrorInfo(result.Errors[0]);
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }

            return res;
        }

        [AjaxMethod]
        public AjaxResponse DeleteColumn(int columnId)
        {
            AjaxResponse res = new AjaxResponse();

            try
            {
                var result = _boardService.DeleteColumn(columnId);
                if (!result.Success)
                {
                    res.Success = false;
                    res.Error = new ErrorInfo(result.Errors[0]);
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }

            return res;
        }
        #endregion

        #region Tasks
        [AjaxMethod]
        public AjaxResponse<IList<BoardTaskBriefDto>> GetAllTasks(int columnId)
        {
            AjaxResponse<IList<BoardTaskBriefDto>> res = new AjaxResponse<IList<BoardTaskBriefDto>>();

            try
            {
                res.Result = _boardService.GetAllTasks(columnId); ;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }

        [AjaxMethod]
        public AjaxResponse<BoardTaskDto> GetTask(int id) {
            AjaxResponse<BoardTaskDto> res = new AjaxResponse<BoardTaskDto>();
            res.Result = _boardService.GetTask(id);
            res.Result.FormatCreationTime = CommonHelper.FormatTimeNote(res.Result.CreationTime, res.Result.CreationTime.ToString("yyyy-MM-dd HH:mm"));
            // res.Result.CreationTime.ToString("yyyy-MM-dd HH:mm");
            return res;
        }


        [AjaxMethod]
        public AjaxResponse<BoardTaskBriefDto> AddTask(int nodeId, int columnId, string title)
        {
            AjaxResponse<BoardTaskBriefDto> res = new AjaxResponse<BoardTaskBriefDto>();
            var result = _boardService.AddTask(nodeId, columnId, title.Trim());
            if (result.Success)
            {
                res.Result = result.Items.MapTo<BoardTaskBriefDto>();
            }
            else
            {
                res.Success = false;
                res.Error = new ErrorInfo(result.Errors[0]);
            }
            return res;
        }

        [AjaxMethod]
        public AjaxResponse ResetTaskOrders(int targetColumnId, int[] itemIds)
        {
            AjaxResponse res = new AjaxResponse();

            try
            {
                var result = _boardService.ResetTaskOrders(targetColumnId, itemIds);
                if (!result.Success)
                {
                    res.Success = false;
                    res.Error = new ErrorInfo(result.Errors[0]);
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }

            return res;
        }

        [AjaxMethod]
        public AjaxResponse FinishTask(int taskId)
        {
            AjaxResponse res = new AjaxResponse();

            try
            {
                var result = _boardService.FinishTask(taskId, GetLoginedUser().Id);
                if (!result.Success)
                {
                    res.Success = false;
                    res.Error = new ErrorInfo(result.Errors[0]);
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }

            return res;
        }

        [AjaxMethod]
        public AjaxResponse CancelFinishTask(int taskId)
        {
            AjaxResponse res = new AjaxResponse();

            try
            {

                var result = _boardService.CancelFinishTask(taskId, GetLoginedUser().Id);
                if (!result.Success)
                {
                    res.Success = false;
                    res.Error = new ErrorInfo(result.Errors[0]);
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }

            return res;
        }

        [AjaxMethod]
        public AjaxResponse UpdateTaskTitle(int taskId, string newTitle)
        {
            AjaxResponse res = new AjaxResponse();

            try
            {
                var result = _boardService.UpdateTaskTitle(taskId, newTitle);
                if (!result.Success)
                {
                    res.Success = false;
                    res.Error = new ErrorInfo(result.Errors[0]);
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }

            return res;
        }

        [AjaxMethod]
        public AjaxResponse UpdateTaskBody(int taskId, string newBody)
        {
            AjaxResponse res = new AjaxResponse();

            try
            {
                var result = _boardService.UpdateTaskBody(taskId, newBody);
                if (!result.Success)
                {
                    res.Success = false;
                    res.Error = new ErrorInfo(result.Errors[0]);
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }

            return res;
        }

        [AjaxMethod]
        public AjaxResponse DeleteTask(int taskId)
        {
            AjaxResponse res = new AjaxResponse();

            try
            {
                var result = _boardService.DeleteTask(taskId);
                if (!result.Success)
                {
                    res.Success = false;
                    res.Error = new ErrorInfo(result.Errors[0]);
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }

            return res;
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="tags">多个之间，逗号“,”分割</param>
        /// <returns></returns>
        [AjaxMethod]
        public AjaxResponse UpdateTaskTags(int taskId, string tags) {
            AjaxResponse res = new AjaxResponse();

            try
            {
                var result = _boardService.UpdateTaskTags(taskId, tags);
                if (!result.Success)
                {
                    res.Success = false;
                    res.Error = new ErrorInfo(result.Errors[0]);
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }

            return res;
        }

        [AjaxMethod]
        public AjaxResponse AddTaskFollower(int taskId, int userId) {
            AjaxResponse res = new AjaxResponse();

            try
            {
                _boardService.AddTaskFollower(taskId, userId);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }

            return res;
        }

        [AjaxMethod]
        public AjaxResponse DeleteTaskFollower(int taskId, int userId)
        {
            AjaxResponse res = new AjaxResponse();

            try
            {
                _boardService.DeleteTaskFollower(taskId, userId);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }

            return res;
        }

        [AjaxMethod]
        public AjaxResponse<IList<BoardTaskLogDto>> GetRecentTaskLogs(int taskId) {
            AjaxResponse<IList<BoardTaskLogDto>> res = new AjaxResponse<IList<BoardTaskLogDto>>();

            try
            {
                res.Result = _boardService.GetAllTaskLogs(taskId);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }

            return res;
        }
        #endregion
    }
}