using System.Collections.Generic;
using U.Utilities.Web;
using UNote.Domain.Notes;

namespace UNote.Web.Models.Notes
{
    public class BaseNodeModel 
    {
        public BaseNodeModel()
        {
            Parents = new List<Node>();
        }

        public int GetNodeId { get { return WebHelper.GetInt("nodeId", 0); } }

        public Node Node { get; set; }

        public IList<Node> MyNodes { get; set; }

        public IList<Node> Parents { get; set; }
    }
}