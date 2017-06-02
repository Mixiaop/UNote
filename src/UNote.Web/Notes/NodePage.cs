using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using U;
using UNote.Domain.Notes;
using UNote.Services.Notes;

namespace UNote.Web.Notes
{
    public class NodePage : Infrastructure.UserPage
    {
        INodeService _nodeService = UPrimeEngine.Instance.Resolve<INodeService>();


        /// <summary>
        /// 获取 dropdownlist 的 listitem 层级排序，并加载到指定的 dropdownlist 中
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="allNodes"></param>
        /// <param name="currentNodes"></param>
        /// <returns></returns>
        public void LoadNodeListItemToDLL(DropDownList ddl, IList<Node> allNodes)
        {
            var roots = allNodes.Where(x => x.ParentId == 0);
            roots.ToList().ForEach((node) =>
            {
                ddl.Items.Add(new ListItem(node.NodeName + " (" + node.ContentTotal + ")", node.Id.ToString()));
                AddNoteListItem(ddl, allNodes, node);
            });
        }

        void AddNoteListItem(DropDownList ddl, IList<Node> allNodes, Node currentNode)
        {
            var nbsp = System.Web.HttpUtility.HtmlDecode("&nbsp;");
            var childNodes = allNodes.Where(x => x.ParentId == currentNode.Id).ToList();
            childNodes.ForEach((node) =>
            {
                var blank = "";
                var parentCount = node.ParentsPath.Split(',').Count();
                for (int i = 0; i < parentCount; i++)
                {
                    blank += nbsp + nbsp + nbsp + nbsp + nbsp + nbsp;
                }

                ddl.Items.Add(new ListItem(blank + node.NodeName + " (" + node.ContentTotal + ")", node.Id.ToString()));
                AddNoteListItem(ddl, allNodes, node);
            });
        }
    }
}