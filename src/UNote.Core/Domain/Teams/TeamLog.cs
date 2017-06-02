using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNote.Domain.Teams
{
    /// <summary>
    /// 代表“团队日志”
    /// </summary>
    public class TeamLog 
    {
        public int MemberId { get; set; }

        public string Action { get; set; }

        public string Remark { get; set; }

        public string IP { get; set; }
    }

}
