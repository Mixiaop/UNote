using System;
using System.Collections.Generic;
using System.Linq;
using U.AutoMapper;
using U.UI;
using U.Application.Services.Dto;
using UNote.Domain.Notes;
using UNote.Services.Events;
using UNote.Services.Users;
using UNote.Services.Notes.Dto;

namespace UNote.Services.Notes.Impl
{
    public class BoardService : ServiceBase, IBoardService
    {
        #region Fields & Ctor
        private readonly IContentColumnRepository _columnRepository;
        private readonly IContentRepository _contentRepository;
        private readonly IContentLogRepository _contentLogRepository;

        private readonly IContentService _contentService;
        private readonly IContentFollowerService _contentFollowerService;
        private readonly INodeService _nodeService;
        private readonly IUserService _userService;

        public BoardService(IContentColumnRepository columnRepository, IContentRepository contentRepository, IContentLogRepository contentLogRepository,
            IContentService contentService, IContentFollowerService contentFollowerService, INodeService nodeService, IUserService userService)
        {
            _columnRepository = columnRepository;
            _contentRepository = contentRepository;
            _contentLogRepository = contentLogRepository;

            _contentService = contentService;
            _contentFollowerService = contentFollowerService;
            _nodeService = nodeService;
            _userService = userService;
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

        public StateOutput UpdateColumnTitle(int columnId, string title)
        {
            var res = new StateOutput();
            try
            {
                var column = this.GetColumn(columnId);
                if (column != null)
                {
                    column.Title = title;
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
        public StateOutput ResetColumnOrders(int[] columnIds, List<ContentColumn> columnList = null)
        {
            StateOutput res = new StateOutput();
            if (columnIds != null && columnIds.Length > 0)
            {
                List<ContentColumn> columns = null;
                if (columnList != null && columnList.Count > 0)
                {
                    columns = columnList;
                }
                else
                {
                    columns = _columnRepository.GetAll().Where(x => columnIds.Contains(x.Id)).ToList();
                }

                if (columns != null && columns.Count > 0)
                {
                    var index = 1;
                    foreach (int id in columnIds)
                    {
                        var item = columns.Where(x => x.Id == id).FirstOrDefault();
                        if (item != null)
                        {
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
                if (count > 0)
                {
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
                                         .Where(x => x.ColumnId == columnId && x.Archived == false);

            var list = query.OrderBy(x => x.ColumnOrder).ToList();
            List<BoardTaskBriefDto> result = new List<BoardTaskBriefDto>();

            if (list != null)
            {
                foreach (var info in list)
                {
                    var item = info.MapTo<BoardTaskBriefDto>();
                    if (info.Body.IsNotNullOrEmpty())
                    {
                        item.ExistsBody = true;
                    }
                    result.Add(item);
                }

                LoadFollowers(result);
            }

            return result;
        }

        /// <summary>
        /// 搜索已归档的任务列表并分页返回
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedResultDto<BoardTaskBriefDto> SearchArchivedTasks(int nodeId, string keywords = "", int pageIndex = 1, int pageSize = 20)
        {
            if (nodeId > 0)
            {
                var query = _contentRepository.GetAll()
                                              .Where(x => x.NodeId == nodeId && x.Archived == true);

                if (keywords.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Title.Contains(keywords));
                }

                query = query.OrderByDescending(x => x.CreationTime);
                var count = query.Count();
                var list = query.ToList().MapTo<List<BoardTaskBriefDto>>();

                LoadFollowers(list);
                return new PagedResultDto<BoardTaskBriefDto>(count, list);
            }
            else
            {
                return new PagedResultDto<BoardTaskBriefDto>();
            }
        }

        /// <summary>
        /// 通过Id获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BoardTaskDto GetTask(int id)
        {
            var content = _contentService.GetById(id);

            var task = content.MapTo<BoardTaskDto>();

            LoadFollowers(task);

            return task;
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
                content.UserId = GetLoginedUserId();
                var prev = _contentRepository.GetAll()
                                             .Where(x => x.ColumnId == columnId)
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

                //log
                AddTaskLog(content.Id, content.UserId, "创建了任务");

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
        public StateOutput ResetTaskOrders(int targetColumnId, int[] itemIds)
        {
            StateOutput res = new StateOutput();
            if (itemIds != null && itemIds.Length > 0 && targetColumnId > 0)
            {
                ContentColumn oldColumn = new ContentColumn();
                List<Content> contents = _contentRepository.GetAll().Where(x => itemIds.Contains(x.Id)).ToList();

                if (contents != null && contents.Count > 0)
                {
                    var index = itemIds.Length;
                    foreach (int id in itemIds)
                    {
                        var item = contents.Where(x => x.Id == id).FirstOrDefault();
                        if (item != null)
                        {
                            if (item.ColumnId != targetColumnId)
                            {
                                oldColumn = item.Column;
                            }

                            item.ColumnId = targetColumnId;
                            item.ColumnOrder = index;
                            _contentRepository.Update(item);
                            index--;
                        }
                    }
                }

                if (oldColumn.Id > 0 && oldColumn.Id != targetColumnId)
                {
                    //任务列表变动
                    //var targetColumn = _contentRepository.Get(targetColumnId);
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
        public StateOutput FinishTask(int taskId, int finishUserId)
        {
            StateOutput res = new StateOutput();
            var task = _contentService.GetById(taskId);
            if (task == null)
            {
                res.AddError("task not found");
                return res;
            }
            if (!task.ColumnTaskFinished)
            {
                task.ColumnTaskFinished = true;
                task.ColumnTaskFinishedUserId = finishUserId;
                task.ColumnTaskFinishedTime = DateTime.Now;
                _contentService.Update(task);

                AddTaskLog(task.Id, GetLoginedUserId(), "完成了任务");

                var taskDto = task.MapTo<BoardTaskDto>();
                LoadFollowers(taskDto);
                EventPublisher.Publish(new TaskFinishedEvent(taskDto, GetLoginedUserNickName()));
            }
            return res;
        }

        /// <summary>
        /// 取消完成任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="finishUserId"></param>
        /// <returns></returns>
        public StateOutput CancelFinishTask(int taskId, int finishUserId)
        {
            StateOutput res = new StateOutput();
            var task = _contentService.GetById(taskId);
            if (task == null)
            {
                res.AddError("task not found");
                return res;
            }

            if (task.ColumnTaskFinished)
            {
                task.ColumnTaskFinished = false;
                task.ColumnTaskFinishedUserId = 0;
                task.ColumnTaskFinishedTime = null;
                _contentService.Update(task);

                AddTaskLog(task.Id, GetLoginedUserId(), "重做了任务");

                var taskDto = task.MapTo<BoardTaskDto>();
                LoadFollowers(taskDto);
                EventPublisher.Publish(new TaskCanceledEvent(taskDto, GetLoginedUserNickName()));
            }
            return res;
        }

        public StateOutput UpdateTaskTitle(int taskId, string newTitle)
        {
            StateOutput res = new StateOutput();
            var task = _contentService.GetById(taskId);
            if (task == null)
            {
                res.AddError("task not found");
                return res;
            }

            task.Title = newTitle;
            _contentService.Update(task);

            AddTaskLog(task.Id, GetLoginedUserId(), "更新了任务标题");
            return res;
        }

        public StateOutput UpdateTaskBody(int taskId, string newBody)
        {
            StateOutput res = new StateOutput();
            var task = _contentService.GetById(taskId);
            if (task == null)
            {
                res.AddError("task not found");
                return res;
            }
            task.Body = newBody;
            _contentService.Update(task);

            AddTaskLog(task.Id, GetLoginedUserId(), "更新了任务内容");

            var taskDto = task.MapTo<BoardTaskDto>();
            LoadFollowers(taskDto);

            EventPublisher.Publish(new TaskContentUpdatedEvent(taskDto, GetLoginedUserNickName()));
            return res;
        }

        public StateOutput UpdateTaskExpirationDate(int taskId, string date)
        {
            StateOutput res = new StateOutput();
            var task = _contentService.GetById(taskId);
            if (task == null)
            {
                res.AddError("task not found");
                return res;
            }
            bool again = false;
            if (task.ColumnTaskExpirationDate.IsNotNullOrEmpty())
            {
                again = true;
            }
            task.ColumnTaskExpirationDate = date;
            _contentService.Update(task);

            AddTaskLog(task.Id, GetLoginedUserId(), (date.IsNotNullOrEmpty() ? (again ? "重设" : "设置") : "取消") + "了截止时间");

            var taskDto = task.MapTo<BoardTaskDto>();
            LoadFollowers(taskDto);
            EventPublisher.Publish(new TaskExpirationDateUpdatedEvent(taskDto, GetLoginedUserNickName()));
            return res;
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="tags">标签之间逗号“,”分割</param>
        /// <returns></returns>
        public StateOutput UpdateTaskTags(int taskId, string tags)
        {
            StateOutput res = new StateOutput();
            var task = _contentService.GetById(taskId);
            if (task == null)
            {
                res.AddError("task not found");
                return res;
            }
            task.Tag = tags;
            _contentService.Update(task);

            AddTaskLog(task.Id, GetLoginedUserId(), "设置了标签【" + tags + "】");
            return res;
        }


        /// <summary>
        /// 替换分类下（未归档）所有任务的标签名称
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="originalTagName"></param>
        /// <param name="newTagName"></param>
        /// <returns></returns>
        public StateOutput ReplaceAllTaskTags(int nodeId, string originalTagName, string newTagName)
        {
            StateOutput res = new StateOutput();
            try
            {
                var tasks = _contentRepository.GetAll()
                                              .Where(x => x.NodeId == nodeId && x.Archived == false && x.Tag.Contains(originalTagName))
                                              .OrderByDescending(x => x.CreationTime)
                                              .ToList();
                tasks.ForEach((t) =>
                {
                    var tagList = t.Tag.Split(",");
                    t.Tag = "";
                    foreach (var s in tagList)
                    {
                        if (s.IsNotNullOrEmpty() && s.ToLower() == originalTagName.ToLower())
                        {
                            t.Tag += newTagName + ",";
                        }
                        else
                        {
                            t.Tag += s + ",";
                        }
                    }

                    if (t.Tag.IsNotNullOrEmpty())
                    {
                        t.Tag = t.Tag.TrimEnd(",");

                        _contentRepository.Update(t);
                    }
                });
            }
            catch (Exception ex)
            {
                res.AddError(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 通过任务列表归档任务（状态已完成）
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public StateOutput ArchiveTasks(List<int> taskIds)
        {
            StateOutput res = new StateOutput();
            if (taskIds != null && taskIds.Count > 0)
            {
                var query = _contentRepository.GetAll().Where(x => taskIds.Contains(x.Id) && x.ColumnTaskFinished == true && x.Archived == false)
                                                       .OrderBy(x => x.CreationTime);
                var list = query.ToList();

                list.ForEach((task) =>
                {
                    task.Archived = true;
                    task.ArchivedDate = DateTime.Now;
                    _contentRepository.Update(task);
                });
            }


            return res;
        }

        public StateOutput DeleteTask(int taskId)
        {
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

        #region Task Followers
        public void AddTaskFollower(int taskId, int userId)
        {
            var task = _contentService.GetById(taskId);
            var user = _userService.GetById(userId);

            _contentFollowerService.AddFollower(task, user);
            AddTaskLog(task.Id, GetLoginedUserId(), "添加了参与者【" + user.NickName + "】");

            EventPublisher.Publish(new TaskFollowerAddedEvent() { Task = task.MapTo<BoardTaskDto>(), OperatorName = GetLoginedUserNickName(), User = user });
        }

        public void DeleteTaskFollower(int taskId, int userId)
        {
            var task = _contentService.GetById(taskId);
            var user = _userService.GetById(userId);

            _contentFollowerService.RemoveFollower(task, user);
            AddTaskLog(task.Id, GetLoginedUserId(), "移除了参与者【" + user.NickName + "】");

            EventPublisher.Publish(new TaskFollowerRemovedEvent() { Task = task.MapTo<BoardTaskDto>(), OperatorName = GetLoginedUserNickName(), User = user });
        }
        #endregion

        #region Task Logs

        /// <summary>
        /// 获取所有任务跟踪，默认最新10条
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IList<BoardTaskLogDto> GetAllTaskLogs(int taskId, int count = 0)
        {
            var query = _contentLogRepository.GetAll();
            if (taskId > 0)
            {
                query = query.Where(x => x.ContentId == taskId)
                             .OrderByDescending(x => x.CreationTime);

                if (count > 0)
                {
                    query = query.Take(count);
                }

                var list = query.ToList();

                var logs = list.MapTo<IList<BoardTaskLogDto>>();
                if (logs != null)
                {
                    logs.ForEach((log) =>
                    {
                        log.FormatCreationTime = CommonHelper.FormatTimeNote(log.CreationTime, log.CreationTime.ToString("yyyy-MM-dd HH:mm"));
                    });
                }

                return logs;

            }
            else
            {
                return new List<BoardTaskLogDto>();
            }
        }

        private void AddTaskLog(int taskId, int creatorId, string desc)
        {
            ContentLog log = new ContentLog();
            log.ContentId = taskId;
            log.UserId = creatorId;
            log.Desc = desc;
            _contentLogRepository.Insert(log);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 为笔记批量载入关注的人
        /// </summary>
        /// <param name="contents"></param>
        private void LoadFollowers(IList<BoardTaskBriefDto> tasks)
        {
            if (tasks != null)
            {
                List<int> ids = new List<int>();
                tasks.ForEach((x) =>
                {
                    ids.Add(x.Id);
                });
                var allFollowers = _contentFollowerService.GetAllFollowers(ids);

                if (allFollowers != null && allFollowers.Count > 0)
                {
                    foreach (var task in tasks)
                    {
                        if (task.Followers == null)
                            task.Followers = new List<BoardTaskFollowerDto>();


                        var followers = allFollowers.Where(x => x.ContentId == task.Id).ToList();
                        if (followers != null)
                        {
                            followers.ForEach((f) =>
                            {
                                if (f != null && f.UserId > 0)
                                {
                                    task.Followers.Add(new BoardTaskFollowerDto() { UserId = f.UserId, NickName = f.User.NickName, PinYin = f.User.PinYin, CorpWeixinUserId = f.User.CorpWeixinUserId });
                                }
                            });
                        }

                    }
                }
            }
        }

        private void LoadFollowers(BoardTaskBriefDto task)
        {
            if (task != null)
            {
                List<int> ids = new List<int>();
                ids.Add(task.Id);

                var allFollowers = _contentFollowerService.GetAllFollowers(ids);

                if (allFollowers != null && allFollowers.Count > 0)
                {
                    if (task.Followers == null)
                        task.Followers = new List<BoardTaskFollowerDto>();

                    if (allFollowers != null)
                    {
                        allFollowers.ForEach((f) =>
                        {
                            if (f != null && f.UserId > 0)
                            {
                                task.Followers.Add(new BoardTaskFollowerDto() { UserId = f.UserId, NickName = f.User.NickName, PinYin = f.User.PinYin, CorpWeixinUserId = f.User.CorpWeixinUserId });
                            }
                        });
                    }
                }
            }
        }
        #endregion
    }
}
