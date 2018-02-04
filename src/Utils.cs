using DataMaker.Forms;
using DataMaker.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DataMaker
{
    public enum InfoType
    {
        Error,
        Warn,
        Info
    }

    public static class Utils
    {

        /// <summary>
        /// 依据指定key从资源文件读取文字
        /// </summary>
        /// <param name="key">指定key</param>
        public static string Lang(string key, params string[] replacers)
        {
            if (key.Contains("%")) return "";

            key = key.Replace("/", "_").Replace("\\", "");

            var rm = new System.Resources.ResourceManager("DataMaker.Languages." +
                "zh_cn", typeof(Resources).Assembly);

            var result = "";
            if (rm.GetString(key) != null) result = rm.GetString(key);
            else throw new ApplicationException("No Lang: " + key);

            for (int i = 0; i < replacers.Length; i++)
                result = result.Replace($"{{{i}}}", replacers[i]);

            return result;
        }

        /// <summary>
        /// 获取指定文件名是否合法
        /// </summary>
        /// <param name="str">文件名</param>
        public static bool IsNameLegal(string str)
        {
            // 匹配包含非法字符的字符串
            var pattern = @"[^(a-z0-9\-\._)]";

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

            if (!key.Contains("%NoKey%") && key.Contains("%NoBrackets%"))
                throw new ArgumentException("Just has a key, doesn't have brackets.");

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
        public static string GetFilePath(this TreeNode node)
        {
            var result = node.FullPath;
            result = result.Replace(Lang("global_datapack"), FileTree.DataPackPath)
                            .Replace("/" + Lang("global_data"), "/data")
                            .Replace("/" + Lang("global_advancement"), "/advancements")
                            .Replace("/" + Lang("global_function"), "/functions")
                            .Replace("/" + Lang("global_structure"), "/structures")
                            .Replace("/" + Lang("global_loottable"), "/loot_tables")
                            .Replace("/" + Lang("global_recipe"), "/recipes")
                            .Replace("/" + Lang("global_tag"), "/tags")
                            .Replace("/" + Lang("global_block"), "/blocks")
                            .Replace("/" + Lang("global_item"), "/items")
                            .Replace("/" + Lang("global_function"), "/functions")
                            .Replace("/" + Lang("global_settings"), "/pack.mcmeta")
                            .Replace("/", "\\");
            // 加后缀		
            result += ((Item)node).GetFileSuffix(false);

            return result.ToString();
        }

        /// <summary>
        /// 获取指定节点的ID名
        /// </summary>
        /// <param name="node">指定节点</param>
        public static string GetID(this TreeNode node)
        {
            var result = "";

            // (namespace):
            var ns = node.GetNamespace();
            result = $"{ns.ToolTipText}:";

            // foo/bar
            var rest = node.GetModulesNames();
            rest = rest.Remove(rest.LastIndexOf("."));

            // (namespace):foo/bar
            result += rest;

            // #(namespace):foo/bar
            if (((Item)node).Sort == ItemSort.FunctionTag ||
                ((Item)node).Sort == ItemSort.BlockTag ||
                ((Item)node).Sort == ItemSort.FunctionTag)
                result = $"#{result}";

            return result;
        }
        
        /// <summary>
        /// 获取指定节点的Namespace
        /// </summary>
        /// <param name="node">指定节点</param>
        public static TreeNode GetNamespace(this TreeNode node)
        {
            var result = new TreeNode();

            if (((Item)node).Type == ItemType.Namespace)
                result = node;
            else
                result = node.Parent.GetNamespace();

            return result;
        }

        /// <summary>
        /// 获取指定节点的所有模块名
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <returns>foo/bar.json</returns>
        public static string GetModulesNames(this TreeNode node, string before = "")
        {
            var result = before;

            if (((Item)node).Type == ItemType.Module || 
                ((Item)node).Type == ItemType.File)
            {
                var b = $"{node.ToolTipText}/{before}";
                result = node.Parent.GetModulesNames(b);
            }

            return result;
        }

        /// <summary>
        /// 获取指定节点对应的路径是否是文件
        /// </summary>
        /// <param name="node">指定节点</param>
        public static bool IsFile(this TreeNode node) => File.Exists(node.GetFilePath());
    }
}
