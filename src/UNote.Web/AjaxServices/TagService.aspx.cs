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
    /// <summary>
    /// 标签服务 
    /// </summary>
    [AjaxNamespace("TagService")]
    public partial class TagService : Infrastructure.AjaxPageBase
    {
        ITagService _tagService = UPrimeEngine.Instance.Resolve<ITagService>();
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [AjaxMethod]
        public AjaxResponse<IList<TagDto>> GetAllTags(int nodeId) {
            AjaxResponse<IList<TagDto>> res = new AjaxResponse<IList<TagDto>>();

            var tags = _tagService.QueryTags(nodeId, 0, QueryTagOrderBy.Default);
            res.Result = tags.MapTo<IList<TagDto>>();
            return res;
        }

        [AjaxMethod]
        public AjaxResponse<TagDto> GetTag(int tagId) {
            AjaxResponse<TagDto> res = new AjaxResponse<TagDto>();

            var tag = _tagService.GetById(tagId);

            res.Result = tag.MapTo<TagDto>();

            return res;
        }

        [AjaxMethod]
        public AjaxResponse CreateTag(int nodeId, string tagName, string styleColor)
        {
            AjaxResponse res = new AjaxResponse();
            try
            {
                _tagService.CreateOrUpdateTags(nodeId, tagName, styleColor);
            }
            catch (Exception ex) {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }

        [AjaxMethod]
        public AjaxResponse UpdateTag(int tagId, string tagName, string styleColor)
        {
            AjaxResponse res = new AjaxResponse();
            try
            {
                _tagService.UpdateTag(tagId, tagName, styleColor);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }

        [AjaxMethod]
        public AjaxResponse DeleteTag(int tagId) {
            AjaxResponse res = new AjaxResponse();
            try
            {
                _tagService.Delete(tagId);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Error = new ErrorInfo(ex.Message);
            }
            return res;
        }
        
    }
}