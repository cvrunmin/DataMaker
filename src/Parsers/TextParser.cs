using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;
using static DataMaker.Tools;

namespace DataMaker.Parsers
{
    public partial class TextParser : UserControl, IParser
    {
        private string frameFileName;
        private string value;
        private string key;

        public string FrameFileName
        {
            get => frameFileName;
            set
            {
                frameFileName = value;
                lblKey.Text = Lang("key_" + FrameFileName + "_" + Key);
                SetSize();
            }
        }
        public string Value
        {
            get => value;
            set
            {
                this.value = value;
                textBoxValue.Text = value;
                MainForm.GetInstance().IsChanged = true;
            }
        }
        public string Key
        {
            get => key;
            set => key = value;
        }

        public TextParser()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);
        }

        public string Json
        {
            get => $@"""{Key}"":""{Value}""";
            set
            {
                var jobj = JsonConvert.DeserializeObject<JObject>(value);
                if (jobj[Key] != null)
                    Value = jobj[Key].ToString();
            }
        }

        public void SetParser(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Key = jobj["key"].ToString();
            if (jobj["default"] != null)
                Value = jobj["default"].ToString();
        }

        private void textBoxValue_TextChanged(object sender, EventArgs e)
            => Value = textBoxValue.Text;

        public void SetSize()
        {
            Width = textBoxValue.Width + lblKey.Width;
            Height = textBoxValue.Height;
        }
    }
}
