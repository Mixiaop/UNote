using System;
using U;
using UNote.Services.Notes;
using UNote.Web.Models.Notes.Boards;

namespace UNote.Web.Notes.Boards
{
    /// <summary>
    /// 已归档的任务列表
    /// </summary>
    public partial class ArchivedTasks : NodePage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();

        protected ArchivedTasksModel Model = new ArchivedTasksModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentNav = Infrastructure.Navigation.Contents;
            CurrentNavNodeId = Model.GetNodeId;
            LoadBase(Model);  
        }
    }
}