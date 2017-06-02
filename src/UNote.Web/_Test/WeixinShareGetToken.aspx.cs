using System;
using System.IO;
using Newtonsoft.Json;
using U;
using UZeroSDK.Weixin.Services;

namespace UNote.Web._Test
{
    public partial class WeixinShareGetToken : System.Web.UI.Page
    {
        IWeixinShareService _weixinShareService = UPrimeEngine.Instance.Resolve<IWeixinShareService>();
        protected void Page_Load(object sender, EventArgs e)
        {
            StringWriter sw = new StringWriter();
            var dto = _weixinShareService.GetTicket("");
            using (JsonWriter w = new JsonTextWriter(sw))
            {
                w.WriteStartObject();
                w.WritePropertyName("appId");
                w.WriteValue(dto.Settings.AppId);
                w.WritePropertyName("timestamp");
                w.WriteValue(dto.Timestamp);
                w.WritePropertyName("nonceStr");
                w.WriteValue(dto.NonceStr);
                w.WritePropertyName("signature");
                w.WriteValue(dto.Signature);
                w.WriteEndObject();
            }
            sw.Close();
            Response.Write(sw.GetStringBuilder().ToString());
        }
    }
}