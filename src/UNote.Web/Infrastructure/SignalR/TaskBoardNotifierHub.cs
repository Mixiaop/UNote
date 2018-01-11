using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Task JoinBoardRoom(string nodeRoomId)
        {
            return Groups.Add(Context.ConnectionId, nodeRoomId);
        }

        public Task LeaveBoardRoom(string nodeRoomId)
        {
            return Groups.Remove(Context.ConnectionId, nodeRoomId);
        }


        #region Columns
        public void CreateColumn(string nodeRoomId, BoardColumnDto column)
        {
            Clients.Group(nodeRoomId).createColumn(column);
        }

        public void ResetColumnOrders(string nodeRoomId, int[] columnIds)
        {
            Clients.Group(nodeRoomId).resetColumnOrders(columnIds);
        }

        public void DeleteColumn(string nodeRoomId, int columnId)
        {
            Clients.Group(nodeRoomId).deleteColumn(columnId);
        }
        #endregion

        #region Tasks
        public void AddTask(string nodeRoomId, BoardTaskBriefDto task)
        {
            Clients.Group(nodeRoomId).addTask(task);
        }

        public void FinishTask(string nodeRoomId, int taskId)
        {
            Clients.Group(nodeRoomId).finishTask(taskId);
        }

        public void CancelTask(string nodeRoomId, int taskId)
        {
            Clients.Group(nodeRoomId).cancelTask(taskId);
        }

        public void UpdateTaskTitle(string nodeRoomId, int taskId, string newTitle)
        {
            Clients.Group(nodeRoomId).updateTaskTitle(taskId, newTitle);
        }

        public void UpdateTaskBody(string nodeRoomId, int taskId, string haveBody)
        {
            Clients.Group(nodeRoomId).updateTaskBody(taskId, haveBody);
        }

        public void UpdateTaskExpirationDate(string nodeRoomId, int taskId, string date)
        {
            Clients.Group(nodeRoomId).updateTaskExpirationDate(taskId, date);
        }

        public void UpdateTaskTags(string nodeRoomId, int taskId, List<string> tagList)
        {
            Clients.Group(nodeRoomId).updateTaskTags(taskId, tagList);
        }
        public void ResetTaskOrders(string nodeRoomId, int columnId, int[] taskIds)
        {
            Clients.Group(nodeRoomId).resetTaskOrders(columnId, taskIds);
        }
        public void DeleteTask(string nodeRoomId, int taskId)
        {
            Clients.Group(nodeRoomId).deleteTask(taskId);
        }
        #endregion

        #region Task Followers
        public void UpdateTaskFollowers(string nodeRoomId, int taskId, List<BoardTaskFollowerDto> users)
        {
            Clients.Group(nodeRoomId).updateTaskFollowers(taskId, users);
        }
        #endregion
    }
}