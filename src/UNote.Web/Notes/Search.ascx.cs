using System;
using System.ComponentModel;
using U.Utilities.Web;

namespace UNote.Web.Notes
{
    public partial class Search : Infrastructure.UserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string Keywords {
            get { return WebHelper.GetString("wd"); }
        }

        private string _searchUrl = "";
        [Browsable(true)]
        public string SearchUrl
        {
            get
            {
                if (_searchUrl.IsNullOrEmpty())
                {
                    _searchUrl = WebHelper.GetThisPageUrl(false);
                }

                if (NodeId > 0)
                {
                    _searchUrl += "?nodeid=" + NodeId + "&wd=";
                }
                else
                {
                    _searchUrl += "?wd=";
                }

                return _searchUrl;
            }
            set { _searchUrl = value; }
        }

        private int _nodeId = 0;
        [Browsable(true)]
        public int NodeId
        {
            get
            {
                if (_nodeId == 0)
                    _nodeId = WebHelper.GetInt("nodeid", 0);

                return _nodeId;
            }
            set { _nodeId = value; }
        }
    }
}