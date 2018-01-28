using Newtonsoft.Json;
using System;
using System.Windows.Forms;

namespace DataMaker.Forms
{
    public partial class Editor : Form
    {
        private bool isSettingJson;

        public Editor()
        {
            InitializeComponent();

            DarkTheme.Initialize(this);

            // FIXME:
            // 你tm还知道每行100字符啊！
            // 有本事写在一行啊！！！
            // 真tm的乱
            frameParserRoot.ValueChanged +=
                (object sender, EventArgs e) =>
                {
                    if (!isSettingJson) MainForm.GetInstance().IsChanged = true;
                    else isSettingJson = false;
                };
        }

        public void SetEditor(string json) => frameParserRoot.SetParser(json);

        public string Json
        {
            set
            {
                frameParserRoot.Json = value;
                isSettingJson = true;
            }
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

        private void Editor_Resize(object sender, EventArgs e) 
            => frameParserRoot.SetSize(ClientSize.Width);
    }
}
