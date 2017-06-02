using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNote.Services.IO
{
    /// <summary>
    /// 临时文件夹管理
    /// </summary>
    public interface ITempFolderService : U.Application.Services.IApplicationService
    {
        string GetTempFileFolder();
    }
}
