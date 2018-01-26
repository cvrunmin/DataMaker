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

        public string Json
        {
            set => frameParserRoot.Json = value;
            get
            {
                var json = frameParserRoot.Json;

                // 以奇异的方式格式化Json
                var parsedJson = JsonConvert.DeserializeObject(json);
                var result = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);

                // TODO: 自定义缩进
                //return result.Replace("  ", " " * n);
                return result;
            }
        }
    }
}
