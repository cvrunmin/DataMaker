using DataMaker.Forms;
using DataMaker.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DataMaker
{
    public static class Utils
    {
        /// <summary>
        /// 依据指定key从资源文件读取文字
        /// </summary>
        /// <param name="key">指定key</param>
        public static string Lang(string key)
        {
            if (key.Contains("%")) return "";

            key = key.Replace("/", "_").Replace("\\", "");

            var rm = new System.Resources.ResourceManager("DataMaker.Languages." +
                "zh_cn", typeof(Resources).Assembly);

            if (rm.GetString(key) != null) return rm.GetString(key);

            throw new ApplicationException("No Lang: " + key);
        }

        /// <summary>
        /// 将指定对象序列化为Json文本
        /// </summary>
        /// <param name="obj">指定对象</param>
        /// <returns></returns>
        public static string SerializeObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        /// <summary>
        /// 获取指定文件名是否合法
        /// </summary>
        /// <param name="str">文件名</param>
        public static bool IsNameLegal(string str)
        {
            // 匹配包含非法字符的字符串
            var pattern = @"[^(a-z0-9\-_)]";

            if (Regex.IsMatch(str, pattern))
            {
                /* 
                 * 文件名不符合社会主义核心价值观
                 * 富强、民主、文明、和谐
                 * 自由、平等、公正、法治
                 * 爱国、敬业、诚信、友善
                 */
                return false;
            }

            return true;
        }

        /// <summary>
        /// 显示MessageBox
        /// </summary>
        /// <param name="text">显示的文字</param>
        /// <param name="buttons">显示的按钮</param>
        /// <param name="replacers">要替换文字中{0}{1}...的参数</param>
        /// <returns></returns>
        public static DialogResult ShowMessagebox(string text,
            MessageBoxButtons buttons = MessageBoxButtons.OK, params string[] replacers)
        {
            var showText = text;
            for (int i = 0; i < replacers.Length; i++)
                showText = showText.Replace($"{{{i}}}", replacers[i]);

            return MessageBox.Show(MainForm.GetInstance(), showText, Application.ProductName, buttons);
        }

        /// <summary>
        /// 显示MessageBox
        /// </summary>
        /// <param name="text">显示的文字</param>
        /// <param name="replacers">要替换文字中{0}{1}...的参数</param>
        /// <returns></returns>
        public static DialogResult ShowMessagebox(string text, params string[] replacers)
            => ShowMessagebox(text, MessageBoxButtons.OK, replacers);

        /// <summary>
        /// 根据指定key获取该Json的前缀
        /// </summary>
        /// <param name="key">指定key</param>
        /// <param name="brackets">包围值的符号</param>
        /// <returns>该Json的前缀</returns>
        public static string GetJsonPreffix(string key, string brackets)
        {
            var result = "";

            if (!key.Contains("%NoKey%"))
                result += $"\"{key}\":";
            if (!key.Contains("%NoBrackets%"))
                result += brackets;

            return result;
        }

        /// <summary>
        /// 根据指定key获取该Json的后缀
        /// </summary>
        /// <param name="key">指定key</param>
        /// <param name="brackets">包围值的符号</param>
        /// <returns>该Json的前缀</returns>
        public static string GetJsonSuffix(string key, string brackets)
        {
            var result = "";
            
            if (!key.Contains("%NoBrackets%"))
                result += brackets;

            return result;
        }

        /// <summary>
        /// 根据指定JToken获得左括号
        /// </summary>
        /// <param name="token">指定JToken</param>
        /// <returns>对应的左括号</returns>
        public static string GetLeftBrackets(JToken token)
        {
            var result = "";

            switch (token.Type)
            {
                case JTokenType.Array:
                    result = "[";
                    break;
                case JTokenType.Object:
                    result = "{";
                    break;
                case JTokenType.String:
                    result = "\"";
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 根据指定JToken获得右括号
        /// </summary>
        /// <param name="token">指定JToken</param>
        /// <returns>对应的右括号</returns>
        public static string GetRightBrackets(JToken token)
        {
            var result = "";

            switch (token.Type)
            {
                case JTokenType.Array:
                    result = "]";
                    break;
                case JTokenType.Object:
                    result = "}";
                    break;
                case JTokenType.String:
                    result = "\"";
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 通过节点获取该节点对应的文件路径
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <returns>文件路径</returns>
        public static string GetPath(this TreeNode node)
        {
            var result = node.Name;

            return result.ToString();
        }
    }
}
