using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U.Application.Services.Dto;
using UNote.Domain.Notes;

namespace UNote.Services.Notes
{
    public interface ITagService : U.Application.Services.IApplicationService
    {
        /// <summary>
        /// 查询标识
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="count"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>

        IList<Tag> QueryTags(int nodeId, int count = 0, QueryTagOrderBy orderBy = QueryTagOrderBy.CountDesc);

        /// <summary>
        /// 获取多少条热门的标签
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        IList<Tag> GetTopList(int top);

        /// <summary>
        /// 通过Id获取信息
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        Tag GetById(int tagId);

        /// <summary>
        /// 通过名称获取标签信息
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        Tag GetByName(int nodeId, string tagName);

        /// <summary>
        /// 是否存在标签名称
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        bool ExistsTagName(int nodeId, string tagName);

        /// <summary>
        /// 更新多个标签（如果不存在则添加，如果已存在则数量+1）
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="tagNames">多个标签用英文逗号（,）分开</param>
        /// <param name="styleColor"></param>
        /// <param name="userId"></param>
        void CreateOrUpdateTags(int nodeId, string tagNames, string styleColor = "", int userId = 0);


        /// <summary>
        /// 通过标签Id更新标签
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="tagName"></param>
        /// <param name="styleColor"></param>
        void UpdateTag(int tagId, string tagName, string styleColor);

        /// <summary>
        /// 删除标签 
        /// </summary>
        /// <param name="tagId"></param>
        void Delete(int tagId);
    }
}
