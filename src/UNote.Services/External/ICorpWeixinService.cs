using System.Collections.Generic;
using UNote.Services.External.Dto;

namespace UNote.Services.External
{
    /// <summary>
    /// 企业微信服务
    /// </summary>
    public interface ICorpWeixinService : IService
    {
        /// <summary>
        /// 获取访问令牌
        /// </summary>
        /// <returns></returns>
        GetCorpWexinAccessTokenOutput GetAccessToken();

        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <param name="code">回调code</param>
        /// <returns></returns>
        GetCorpWeixinUserIdOutput GetUserId(string code);

        /// <summary>
        /// 发送消息到对应的用户列表
        /// </summary>
        /// <param name="userIdList"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        CorpWeixinSendMessageOutput SendMessage(List<string> userIdList, string content);
    }
}
