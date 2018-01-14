using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static DataMaker.Tools;

namespace DataMaker.DataStructures
{
    /// <summary>
    /// 数据结构基类
    /// </summary>
    public class DataStructureBase
    {
        protected string Json { get; set; }

        protected DataStructureBase(string json)
        {
            Json = json;
        }

        public string LangKey => GetJToken(Json)["LangKey"].ToString();
        public string DisplayName => Lang("displayname_" + LangKey);
        public string Description => Lang("description_" + LangKey);
        public string Name => GetJToken(Json)["Name"].ToString();
        public string Type => GetJToken(Json)["Type"].ToString();
    }

    /// <summary>
    /// 数据结构Json类
    /// </summary>
    public class JsonDataStructure : DataStructureBase
    {
        protected JsonDataStructure(string json) : base(json) { }

        public JToken[] Children => GetJToken(Json)["Children"].ToObject<JToken[]>();
    }
}
