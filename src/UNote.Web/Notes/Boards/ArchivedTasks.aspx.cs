using System;
using U;
using U.Utilities.Web;
using UNote.Services.Notes;
using UNote.Web.Models.Notes.Boards;
using UNote.Web.Infrastructure;

namespace UNote.Web.Notes.Boards
{
    /// <summary>
    /// 已归档的任务列表
    /// </summary>
    public partial class ArchivedTasks : NodePage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        IBoardService _boardService = UPrimeEngine.Instance.Resolve<IBoardService>();
        protected ArchivedTasksModel Model = new ArchivedTasksModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentNav = Infrastructure.Navigation.Contents;
            CurrentNavNodeId = Model.GetNodeId;
            LoadBase(Model);

            

            PagingInfo pageInfo = new PagingInfo();
            pageInfo.PageIndex = WebHelper.GetInt("page", 1);
            pageInfo.PageSize = 20;
            pageInfo.Url = WebHelper.GetUrl();

            Model.Result = _boardService.SearchArchivedTasks(Model.Node.Id, "", pageInfo.PageIndex, pageInfo.PageSize);

            pageInfo.TotalCount = Model.Result.TotalCount;
            Model.PagingHtml = new Paginations(pageInfo).GetPaging();
        }
    }
}