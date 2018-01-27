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

        public TextParser()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);
        }

        public string Key { get; set; }

        public string FrameFileName
        {
            get => frameFileName;
            set
            {
                frameFileName = value;
                lblKey.Text = Lang("key_" + FrameFileName + "_" + Key);
            }
        }

        public string Value
        {
            get => value;
            set
            {
                this.value = value;
                textBoxValue.Text = value;
            }
        }

        public string Json
        {
            get
            {
                var result = GetJsonPreffix(Key, "\"");

                result += Value;

                result += GetJsonSuffix(Key, "\"");

                return result;
            }
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
        {
            Value = textBoxValue.Text;
            MainForm.GetInstance().IsChanged = true;
        }

        public void SetSize(int width)
        {
            Width = width;
            textBoxValue.Left = lblKey.Left + lblKey.Width + textBoxValue.Margin.Left;
            textBoxValue.Width = Width - lblKey.Left - lblKey.Width - textBoxValue.Margin.Left;
            Height = Math.Max(textBoxValue.Height, lblKey.Height);
        }
    }
}
