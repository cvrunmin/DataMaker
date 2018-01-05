using DataMaker.DataClasses;
using DataMaker.Forms;
using DataMaker.Properties;
using Newtonsoft.Json;
using System;
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
    }

    public class Factories
    {
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

        public static IEditor GetEditor(TreeNode node)
        {
            IEditor result = null;

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
                    result = new JsonEditor();
                    break;
            }

            return result;
        }
    }
}
