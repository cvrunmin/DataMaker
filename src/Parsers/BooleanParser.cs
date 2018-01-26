using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;
using static DataMaker.Tools;

namespace DataMaker.Parsers
{
    public partial class BooleanParser : UserControl, IParser
    {
        private string frameFileName;
        private bool value;

        public BooleanParser()
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
                checkBoxValue.Text = Lang("key_" + FrameFileName + "_" + Key);
                SetSize();
            }
        }

        public bool Value
        {
            get => value;
            set
            {
                this.value = value;
                checkBoxValue.Checked = Value;
                MainForm.GetInstance().IsChanged = true;
            }
        }

        public string Json
        {
            get => $@"""{Key}"":{Value.ToString().ToLower()}";
            set
            {
                var jobj = JsonConvert.DeserializeObject<JObject>(value);
                if (jobj[Key] != null)
                    Value = bool.Parse(jobj[Key].ToString());
            }
        }

        public void SetParser(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Key = jobj["key"].ToString();
            if (jobj["default"] != null)
                Value = bool.Parse(jobj["default"].ToString());
        }

        private void checkBoxValue_CheckedChanged(object sender, EventArgs e)
            => Value = checkBoxValue.Checked;

        public void SetSize()
        {
            Width = checkBoxValue.Width;
            Height = checkBoxValue.Height;
        }
    }
}
