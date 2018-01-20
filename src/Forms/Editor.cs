using Newtonsoft.Json;
using System.Windows.Forms;

namespace DataMaker.Forms
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();

            DarkTheme.Initialize(this);
        }

        public void SetEditor(string json) => frameParserRoot.SetParser(json);

        public void SetJson(string json) => frameParserRoot.SetJson(json);

        public string GetJson()
        {
            var json = frameParserRoot.GetJson();

            // 以奇异的方式去除"%NaN%":
            json = json.Replace("\"%NaN%\":", "");

            // 以奇异的方式格式化Json
            var parsedJson = JsonConvert.DeserializeObject(json);
            var result = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);

            // TODO: 自定义缩进
            //return result.Replace("  ", " " * n);
            return result;
        }
    }
}
