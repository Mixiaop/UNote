using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using U;
using UNote.Services.External;

namespace UNote.Web._Test
{
    public partial class CorpWeixinNotifyTests : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ICorpWeixinService weixinService = UPrimeEngine.Instance.Resolve<ICorpWeixinService>();
            var userIdList = new List<string>();
            userIdList.Add("15800448791");
            var res = weixinService.SendMessage(userIdList, "测试发送通知<a href='http://note.youzy.cn'>查看</a>");
            Response.Write(JsonConvert.SerializeObject(res));
            //Response.Write("sus");
        }
    }
}