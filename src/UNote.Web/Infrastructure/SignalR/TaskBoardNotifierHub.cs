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
    [HubName("TaskBoardNotifier")]
    public class TaskBoardNotifierHub : Hub
    {
        public TaskBoardNotifierHub()
        {
        }

        #region Columns
        public void CreateColumn(BoardColumnDto column)
        {
            Clients.All.createColumn(column);
        }

        public void ResetColumnOrders(int[] columnIds)
        {
            Clients.All.resetColumnOrders(columnIds);
        }

        public void DeleteColumn(int columnId)
        {
            Clients.All.deleteColumn(columnId);
        }
        #endregion

        #region Tasks
        public void AddTask(BoardTaskBriefDto task)
        {
            Clients.All.addTask(task);
        }

        public void DeleteTask(int taskId) {
            Clients.All.deleteTask(taskId);
        }
        #endregion
    }
}