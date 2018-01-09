using System.Collections.Generic;
using U.Application.Services.Dto;
using UNote.Services.Notes.Dto;

namespace UNote.Web.Models.Notes.Boards
{
    public class ArchivedTasksModel : BaseNodeModel
    {
        public PagedResultDto<BoardTaskBriefDto> Result { get; set; }

        public string PagingHtml { get; set; }
    }
}