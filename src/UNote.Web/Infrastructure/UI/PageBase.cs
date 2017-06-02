using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using U;
using U.Utilities.Web;
using U.FakeMvc.Routes;
using UNote.Domain.Users;
using UNote.Services.Users;
using UNote.Services.Authrization;
using UNote.Web.Infrastructure;

namespace UNote.Web.Infrastructure
{
    public class PageBase : System.Web.UI.Page
    {
        protected IAuthenticationService authenticationService = UPrimeEngine.Instance.Resolve<IAuthenticationService>();
        public RouteContext RouteContext = RouteContext.Instance;
        public PageBase() { 
        
        }

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        protected bool IsLogined(){
            return authenticationService.GetAuthenticatedUser() != null;
        }

        #region Method
        #region Alert

        public string AlertSuccess(string message, string title = "", int timeoutByClose = 0)
        {
            return GetMessage(1, title, message, timeoutByClose);
        }

        public string AlertError(string message, string title = "", int timeoutByClose = 0)
        {
            return GetMessage(2, title, message, timeoutByClose);
        }

        /// <summary>
        /// 获取消息HTML
        /// </summary>
        /// <param name="type">1-成功,2-错误,3-警告</param>
        /// <param name="message"></param>
        /// <param name="timeClose">1秒=1000</param>
        /// <returns></returns>
        private string GetMessage(int type, string title, string message, int timeClose)
        {
            string html = "";
            switch (type)
            {
                case 1:
                    html = "<div class=\"alert alert-success\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\">&times;</button>" + (title != "" ? "<strong>" + title + "</strong> " : "") + "" + message + "</div>";
                    break;
                case 2:
                    html = "<div class=\"alert alert-danger\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\">&times;</button>" + (title != "" ? "<strong>" + title + "</strong> " : "") + "" + message + "</div>";
                    break;
                case 3:
                    html = "<div class=\"alert alert-warning\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\">&times;</button>" + (title != "" ? "<strong>" + title + "</strong> " : "") + "" + message + "</div>";
                    break;
            }

            if (timeClose > 0)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "alert-hidden", "<script>setTimeout(function(){$('.alert').remove();}," + timeClose + ");</script>");
            }

            return html;
        }
        #endregion

        /// <summary>
        /// 客服端延时跳转
        /// </summary>
        /// <param name="url">跳转地址</param>
        /// <param name="time">时间1000:1秒</param>
        public void RedirectByTime(string url, int time)
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "RedirectByTime", "<script>setTimeout(function(){window.location='" + url + "';}," + time + ");</script>");
        }

        protected void RegisterScripts(string key, string scripts)
        {
            this.ClientScript.RegisterStartupScript(GetType(), key, scripts, true);
        }

        public string SetUrlParam(string paramName, object paramValue, string defaultUrl = "") {
            string url = "";
            if (defaultUrl.IsNullOrEmpty())
            {
                url = WebHelper.GetUrl();
            }
            else
                url = defaultUrl;
            
            if (paramName.IndexOf("=") == -1)
                paramName += "=";

            if (url.IndexOf(paramName) == -1)
            {
                if (url.IndexOf("?") == -1)
                {
                    url += "?";
                }
                else
                {
                    url += "&";
                }
                url += paramName + paramValue;
            }
            else
            {
                url = System.Text.RegularExpressions.Regex.Replace(url, paramName + @"[\da-z-A-Z]{0,10}", (paramName + paramValue.ToString()));
            }

            return url;
        }


        public void Invoke404() {
            Response.Redirect("/404.html");
        }
        #endregion
    }
}