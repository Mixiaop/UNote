using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U.Application.Services.Dto;
using UNote.Domain.Notes;
using UNote.Services.Notes.Dto;

namespace UNote.Services.Notes
{
    /// <summary>
    /// 内容任务板模式应用服务
    ///（Node开启看板模式时使用）
    /// </summary>
    public interface IBoardService : IService
    {
        #region Columns
        /// <summary>
        /// 获取对应分类的所有内容列（有序）
        /// </summary>
        /// <param name="nodeId">栏目Id</param>
        /// <returns></returns>
        IList<ContentColumn> GetAllColumns(int nodeId);

        /// <summary>
        /// 通过Id获取单列信息
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        ContentColumn GetColumn(int columnId);

        /// <summary>
        /// 创建新的内容列表
        /// </summary>
        /// <param name="nodeId">所属分类Id</param>
        /// <param name="title">标题</param>
        /// <param name="css">自定义样式（颜色）</param>
        /// <returns></returns>
        StateOutput<ContentColumn> CreateColumn(int nodeId, string title, string css = "");

        /// <summary>
        /// 编辑内容列表
        /// </summary>
        /// <param name="columnId">内容列Id</param>
        /// <param name="title">新标题</param>
        /// <param name="css">自定义样式</param>
        /// <returns></returns>
        StateOutput UpdateColumn(int columnId, string title, string css = "");

        StateOutput UpdateColumnTitle(int columnId, string title);

        /// <summary>
        /// 根据数组重置列排序
        /// </summary>
        /// <param name="columnIds"></param>
        /// <param name="columnList">如果此List不为空，则使用此List的数据直接更新</param>
        /// <returns></returns>
        StateOutput ResetColumnOrders(int[] columnIds, List<ContentColumn> columnList = null);

        /// <summary>
        /// 删除内容列表
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        StateOutput DeleteColumn(int columnId);
        #endregion

        #region Tasks
        /// <summary>
        /// 获取对应列的所有内容项（倒序）
        /// </summary>
        /// <param name="columnId">列Id</param>
        /// <returns></returns>
        IList<BoardTaskBriefDto> GetAllTasks(int columnId);

        /// <summary>
        /// 通过Id获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BoardTaskDto GetTask(int id);

        /// <summary>
        /// 添加内容项
        /// </summary>
        /// <param name="nodeId">分类Id</param>
        /// <param name="columnId">列Id</param>
        /// <param name="title"></param>
        /// <returns></returns>
        StateOutput<Content> AddTask(int nodeId, int columnId, string title);

        /// <summary>
        /// 更新任务内容
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        //StateOutput UpdateTaskContent(int taskId, string content);

        /// <summary>
        /// 根据数组重置列的任务项排序
        /// </summary>
        /// <param name="targetColumnId">目标列</param>
        /// <param name="itemIds">内容项Ids</param>
        /// <returns></returns>
        StateOutput ResetTaskOrders(int targetColumnId, int[] itemIds);

        /// <summary>
        /// 完成任务（应该用任务动态来保存信息）
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="finishUserId"></param>
        /// <returns></returns>
        StateOutput FinishTask(int taskId, int finishUserId);

        /// <summary>
        /// 取消完成任务（应用使用任务动态来保存信息）
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="finishUserId"></param>
        /// <returns></returns>
        StateOutput CancelFinishTask(int taskId, int finishUserId);

        StateOutput UpdateTaskTitle(int taskId, string newTitle);

        StateOutput UpdateTaskBody(int taskId, string newBody);

        StateOutput UpdateTaskExpirationDate(int taskId, string date);

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="tags">标签之间逗号“,”分割</param>
        /// <returns></returns>
        StateOutput UpdateTaskTags(int taskId, string tags);

        /// <summary>
        /// 通过任务列表归档任务（状态已完成）
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        StateOutput ArchiveTasks(List<int> taskIds);

        StateOutput DeleteTask(int taskId);
        #endregion

        #region Task Followers
        void AddTaskFollower(int taskId, int userId);

        void DeleteTaskFollower(int taskId, int userId);
        #endregion

        #region Task Logs
        /// <summary>
        /// 获取所有任务跟踪
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IList<BoardTaskLogDto> GetAllTaskLogs(int taskId, int count = 0);
        #endregion
    }
}
