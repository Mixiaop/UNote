using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UNote
{
    public class HtmlHelper
    {
        /// <summary>
        /// 从字符串去除图片标签
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns></returns>
        public static string ClearImgs(string input)
        {
            string pattern = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
            string pattern2 = @"<input\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
            string sReturn = Regex.Replace(input, pattern, "", RegexOptions.IgnoreCase);
            return Regex.Replace(sReturn, pattern2, "", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<string> GetRemoteImgUrls(string text)
        {
            List<string> list = new List<string>();
            string pat = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
            Regex r = new Regex(pat, RegexOptions.Compiled);
            Match m = r.Match(text);
            //int matchCount = 0;
            while (m.Success)
            {
                if (m.Groups["imgUrl"].Value.Trim().IndexOf("http://") != -1 ||
                    m.Groups["imgUrl"].Value.Trim().IndexOf("https://") != -1)
                {
                    list.Add(m.Groups["imgUrl"].Value.Trim());
                }
                m = m.NextMatch();
            }
            return list;
        }

        /// <summary>
        /// 获取图片URL
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<string> GetImgUrls(string text)
        {
            List<string> list = new List<string>();
            string pat = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
            Regex r = new Regex(pat, RegexOptions.Compiled);
            Match m = r.Match(text);
            //int matchCount = 0;
            while (m.Success)
            {
                list.Add(m.Groups["imgUrl"].Value.Trim());
                m = m.NextMatch();
            }
            return list;
        }
    }
}
