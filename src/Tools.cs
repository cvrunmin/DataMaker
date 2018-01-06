using DataMaker.DataClasses;
using DataMaker.Forms;
using DataMaker.Properties;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static DataMaker.FileTree;

namespace DataMaker
{
    public class Tools
    {
        /// <summary>
        /// 依据指定key从资源文件读取文字
        /// </summary>
        /// <param name="key">指定key</param>
        public static string Lang(string key)
        {

            var rm = new System.Resources.ResourceManager("DataMaker.Languages." + "zh_cn", typeof(Resources).Assembly);

            if (rm.GetString(key) != null)
            {
                return rm.GetString(key);
            }

            throw new ApplicationException("No Lang: " + key);
        }

        /// <summary>
        /// 将指定对象序列化为Json文本
        /// </summary>
        /// <param name="obj">指定对象</param>
        /// <returns></returns>
        public static string SerializeToJson(object obj) => JsonConvert.SerializeObject(obj, Formatting.Indented);

        /// <summary>
        /// 根据指定节点获取数据类
        /// </summary>
        /// <param name="node">指定节点</param>
        public static IDataClass GetDataClass(TreeNode node)
        {
            IDataClass result = null;

            switch (((Item)node).Sort)
            {
                case Sort.Advancement:
                    break;
                case Sort.Function:
                    break;
                case Sort.LootTable:
                    break;
                case Sort.Recipe:
                    break;
                case Sort.Structure:
                    break;
                case Sort.Tag:
                    break;
                case Sort.PackMcmeta:
                    result = new PackMcmeta();
                    break;
            }

            return result;
        }

        /// <summary>
        /// 根据指定节点获取应用的编辑器
        /// </summary>
        /// <param name="node">指定节点</param>
        public static Form GetEditor(TreeNode node)
        {
            Form result = null;

            switch (((Item)node).Sort)
            {
                case Sort.Advancement:
                    break;
                case Sort.Function:
                    break;
                case Sort.LootTable:
                    break;
                case Sort.Recipe:
                    break;
                case Sort.Structure:
                    break;
                case Sort.Tag:
                    break;
                case Sort.PackMcmeta:
                    result = new RawEditor()
                    {
                        MdiParent = MainForm.GetInstance()
                    };
                    break;
            }

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
    }
}
