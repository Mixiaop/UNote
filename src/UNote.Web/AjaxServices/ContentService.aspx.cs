using System;
using System.Collections.Generic;
using AjaxPro;
using U;
using U.UI;
using U.AutoMapper;
using U.Web.Models;
using UNote.Domain.Users;
using UNote.Domain.Notes;
using UNote.Services.Notes;
using UNote.Services.Notes.Dto;
using UNote.Services.Users;
using UNote.Services.Users.Dto;
using UNote.Web.AjaxServices.Dto;

namespace UNote.Web.AjaxServices
{
    [AjaxNamespace("ContentService")]
    public partial class ContentService : Infrastructure.AjaxPageBase
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();
        IContentService _contentService = UPrimeEngine.Instance.Resolve<IContentService>();
        IContentFollowerService _contentFollowerService = UPrimeEngine.Instance.Resolve<IContentFollowerService>();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Content
        [AjaxMethod]
        public AjaxResponse<ContentDto> Get(int contentId)
        {
            AjaxResponse<ContentDto> res = new AjaxResponse<ContentDto>();

            try
            {

                var content = _contentService.GetById(contentId);

                res.Result = content.MapTo<ContentDto>();
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }

        [AjaxMethod]
        public AjaxResponse Delete(int contentId)
        {
            AjaxResponse res = new AjaxResponse();

            try
            {
                _contentService.Delete(contentId);

            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }

        [AjaxMethod]
        public AjaxResponse AddVisitCount(int contentId)
        {

            AjaxResponse res = new AjaxResponse();

            try
            {
                _contentService.AddVisitCount(contentId);

            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }
        #endregion

        #region ContentFollower
        //public AjaxResponse AddFollowers(int teamId,)
        //public AjaxResponse<IList<User>> GetAllFollowers(int contentId) { 
        
        //}
        #endregion
    }
}