using U.Settings;

namespace UNote.Configuration
{
    [USettingsPathArribute("UNoteSettings.json", "/Config/UNote/")]
    public class UNoteSettings : USettings<UNoteSettings>
    {
        /// <summary>
        /// 记住用户过期时间（分）
        /// </summary>
        public int RememberUserExpiresTime { get; set; }

        /// <summary>
        /// 默认30秒(单位秒)
        /// </summary>
        public int UserHeartbeatTime { get; set; }

        /// <summary>
        /// 在线用户的统计时间（单位秒），如30秒前
        /// </summary>
        public int OnlineUserTime { get; set; }

        /// <summary>
        /// HTML插件根目录路径
        /// </summary>
        public string NodeHtmlFilePath { get; set; }

        /// <summary>
        /// 临时文件夹路径
        /// </summary>
        public string TempPath { get; set; }

        public string UZeroMediaHost { get; set; }
    }
}
