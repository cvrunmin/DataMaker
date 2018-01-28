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

            frameParserRoot.ValueChanged += FrameParserRoot_ValueChanged;
        }

        private void FrameParserRoot_ValueChanged(object sender, EventArgs e)
        {
            if (!isSettingJson) MainForm.GetInstance().IsChanged = true;
        }

        public void SetEditor(string json) => frameParserRoot.SetParser(json);

        public string Json
        {
            set
            {
                isSettingJson = true;
                frameParserRoot.Json = value;
                isSettingJson = false;
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
