using System;
using System.Collections.Generic;
using System.Linq;
using U.AutoMapper;
using U.UI;
using U.Application.Services.Dto;
using UNote.Domain.Notes;
using UNote.Services.Notes.Dto;

namespace UNote.Services.Notes.Impl
{
    public class BoardService : ServiceBase, IBoardService
    {
        #region Fields & Ctor
        private readonly IContentColumnRepository _columnRepository;
        private readonly IContentRepository _contentRepository;

        private readonly IContentService _contentService;
        private readonly INodeService _nodeService;

        public BoardService(IContentColumnRepository columnRepository, IContentRepository contentRepository, IContentService contentService, INodeService nodeService)
        {
            _columnRepository = columnRepository;
            _contentRepository = contentRepository;
            _contentService = contentService;
            _nodeService = nodeService;
        }
        #endregion

        #region Columns
        /// <summary>
        /// 获取对应分类的所有内容列（有序）
        /// </summary>
        /// <param name="nodeId">栏目Id</param>
        /// <returns></returns>
        public IList<ContentColumn> GetAllColumns(int nodeId)
        {
            if (nodeId == 0)
                throw new UserFriendlyException("nodeId must be greater than zero");
            var query = _columnRepository.GetAll()
                                         .Where(x => x.NodeId == nodeId);

            var list = query.OrderBy(x => x.Order);
            return list.ToList();
        }

        /// <summary>
        /// 通过Id获取单列信息
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public ContentColumn GetColumn(int columnId)
        {
            return _columnRepository.Get(columnId);
        }

        /// <summary>
        /// 创建新的内容列表
        /// </summary>
        /// <param name="nodeId">所属分类Id</param>
        /// <param name="title">标题</param>
        /// <param name="css">自定义样式（颜色）</param>
        /// <returns></returns>
        public StateOutput<ContentColumn> CreateColumn(int nodeId, string title, string css = "")
        {
            StateOutput<ContentColumn> res = new StateOutput<ContentColumn>();
            title = title.Trim();

            var node = _nodeService.GetById(nodeId);
            if (node == null)
            {
                res.AddError("node not found");
                return res;
            }
            var info = new ContentColumn();
            info.TeamId = node.TeamId;
            info.NodeId = node.Id;
            info.Class = css;
            info.Title = title;
            info.Order = 1;
            try
            {
                var prev = _columnRepository.GetAll()
                                            .Where(x => x.NodeId == nodeId)
                                            .OrderByDescending(x => x.Order)
                                            .Take(1)
                                            .FirstOrDefault();
                if (prev != null)
                {
                    info.Order = prev.Order + 1;
                }
                info.Id = _columnRepository.InsertAndGetId(info);
            }
            catch (Exception ex)
            {
                res.AddError(ex.Message);
            }
            res.Items = info;
            return res;
        }

        /// <summary>
        /// 编辑内容列表
        /// </summary>
        /// <param name="columnId">内容列Id</param>
        /// <param name="title">新标题</param>
        /// <param name="css">自定义样式</param>
        /// <returns></returns>
        public StateOutput UpdateColumn(int columnId, string title, string css = "")
        {
            var res = new StateOutput();
            try
            {
                var column = this.GetColumn(columnId);
                if (column != null)
                {
                    column.Title = title;
                    column.Class = css;
                    _columnRepository.Update(column);
                }
            }
            catch (Exception ex)
            {
                res.AddError(ex.Message);
            }

            return res;
        }

        /// <summary>
        /// 根据数组重置列排序
        /// </summary>
        /// <param name="columnIds"></param>
        /// <param name="columnList"></param>
        /// <returns></returns>
        public StateOutput ResetColumnOrders(int[] columnIds, List<ContentColumn> columnList = null) {
            StateOutput res = new StateOutput();
            if (columnIds != null && columnIds.Length > 0) {
                List<ContentColumn> columns = null;
                if (columnList != null && columnList.Count > 0)
                {
                    columns = columnList;
                }
                else {
                    columns = _columnRepository.GetAll().Where(x => columnIds.Contains(x.Id)).ToList();
                }

                if (columns != null && columns.Count > 0) {
                    var index = 1;
                    foreach (int id in columnIds) {
                        var item = columns.Where(x => x.Id == id).FirstOrDefault();
                        if (item != null) {
                            item.Order = index;
                            _columnRepository.Update(item);
                            index++;
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 删除内容列表
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public StateOutput DeleteColumn(int columnId)
        {
            var res = new StateOutput();
            try
            {
                var column = GetColumn(columnId);
                if (column == null)
                {
                    res.AddError("指定的列表不存生或已被删除");
                    return res;
                }

                var count = _contentRepository.Count(x => x.ColumnId == columnId);
                if (count > 0) {
                    res.AddError("请先清空此列表上的任务。");
                    return res;
                }

                //not updating sort is ok?
                _columnRepository.Delete(column);
            }
            catch (Exception ex)
            {
                res.AddError(ex.Message);
            }

            return res;
        }
        #endregion

        #region Tasks
        /// <summary>
        /// 获取对应列的所有内容项（倒序）
        /// </summary>
        /// <param name="columnId">列Id</param>
        /// <returns></returns>
        public IList<BoardTaskBriefDto> GetAllTasks(int columnId)
        {
            if (columnId == 0)
                throw new UserFriendlyException("columnId must be greater than zero");

            var query = _contentRepository.GetAll()
                                         .Where(x => x.ColumnId == columnId);

            var list = query.OrderBy(x => x.ColumnOrder).ToList();
            List<BoardTaskBriefDto> result = new List<BoardTaskBriefDto>();

            if (list != null) {
                foreach (var info in list) {
                    var item = info.MapTo<BoardTaskBriefDto>();
                    if (info.Body.IsNotNullOrEmpty()) {
                        item.ExistsBody = true;
                    }
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// 通过Id获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BoardTaskDto GetTask(int id) {
            var content = _contentService.GetById(id);

            return content.MapTo<BoardTaskDto>();
        }

        /// <summary>
        /// 添加内容项
        /// </summary>
        /// <param name="nodeId">分类Id</param>
        /// <param name="columnId">列Id</param>
        /// <param name="title"></param>
        /// <returns></returns>
        public StateOutput<Content> AddTask(int nodeId, int columnId, string title)
        {
            StateOutput<Content> res = new StateOutput<Content>();
            title = title.Trim();

            try
            {
                var node = _nodeService.GetById(nodeId);
                if (node == null)
                {
                    res.AddError("node not found");
                    return res;
                }

                var column = GetColumn(columnId);
                if (column == null)
                {
                    res.AddError("column not found");
                    return res;
                }

                if (title.IsNullOrEmpty())
                {
                    res.AddError("title can not be empty");
                    return res;
                }
                Content content = new Content();
                content.TeamId = node.TeamId;
                content.NodeId = nodeId;
                content.ColumnId = columnId;
                content.Title = title;
                content.ColumnOrder = 1;

                var prev = _contentRepository.GetAll()
                                             .Where(x=>x.ColumnId == columnId)
                                             .OrderByDescending(x => x.ColumnOrder)
                                             .Take(1)
                                             .FirstOrDefault();
                if (prev != null)
                {
                    content.ColumnOrder = prev.ColumnOrder + 1;
                }

                //update content count of column
                //column.ContentCount++;
                _columnRepository.Update(column);
                content.Id = _contentService.Insert(content);

                res.Items = content;
            }
            catch (Exception ex)
            {
                res.AddError(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 根据数组重置列的任务项排序
        /// </summary>
        /// <param name="targetColumnId">目标列</param>
        /// <param name="itemIds">内容项Ids</param>
        /// <returns></returns>
        public StateOutput ResetTaskOrders(int targetColumnId, int[] itemIds) {
            StateOutput res = new StateOutput();
            if (itemIds != null && itemIds.Length > 0 && targetColumnId > 0)
            {
                List<Content> contents = null;
                contents = _contentRepository.GetAll().Where(x => itemIds.Contains(x.Id)).ToList();

                if (contents != null && contents.Count > 0)
                {
                    var index = itemIds.Length;
                    foreach (int id in itemIds)
                    {
                        var item = contents.Where(x => x.Id == id).FirstOrDefault();
                        if (item != null)
                        {
                            item.ColumnId = targetColumnId;
                            item.ColumnOrder = index;
                            _contentRepository.Update(item);
                            index--;
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="finishUserId"></param>
        /// <returns></returns>
        public StateOutput FinishTask(int taskId, int finishUserId) {
            StateOutput res = new StateOutput();
            var task = _contentService.GetById(taskId);
            if (task == null) {
                res.AddError("task not found");
                return res;
            }

            task.ColumnTaskFinished = true;
            task.ColumnTaskFinishedUserId = finishUserId;
            task.ColumnTaskFinishedTime = DateTime.Now;
            _contentService.Update(task);
            return res;
        }

        /// <summary>
        /// 取消完成任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="finishUserId"></param>
        /// <returns></returns>
        public StateOutput CancelFinishTask(int taskId, int finishUserId) {
            StateOutput res = new StateOutput();
            var task = _contentService.GetById(taskId);
            if (task == null)
            {
                res.AddError("task not found");
                return res;
            }

            task.ColumnTaskFinished = false;
            task.ColumnTaskFinishedUserId = 0;
            task.ColumnTaskFinishedTime = null;
            _contentService.Update(task);
            return res;
        }

        public StateOutput UpdateTaskTitle(int taskId, string newTitle) {
            StateOutput res = new StateOutput();
            var task = _contentService.GetById(taskId);
            if (task == null)
            {
                res.AddError("task not found");
                return res;
            }

            task.Title = newTitle;
            _contentService.Update(task);

            return res;
        }

        public StateOutput UpdateTaskBody(int taskId, string newBody) {
            StateOutput res = new StateOutput();
            var task = _contentService.GetById(taskId);
            if (task == null)
            {
                res.AddError("task not found");
                return res;
            }
            task.Body = newBody;
            _contentService.Update(task);

            return res;
        }

        public StateOutput DeleteTask(int taskId) {
            StateOutput res = new StateOutput();
            var task = _contentService.GetById(taskId);
            if (task == null)
            {
                res.AddError("task not found");
                return res;
            }

            _contentService.Delete(task);

            return res;
        }
        #endregion
    }
}
