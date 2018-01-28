using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;
using static DataMaker.Tools;

namespace DataMaker.Parsers
{
    public partial class NullBooleanParser : UserControl, IParser
    {
        public NullBooleanParser()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);
        }

        private string frameFileName;
        private bool? value;

        public event ValueChangedHandler ValueChanged;

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
        public bool? Value
        {
            get => value;
            set
            {
                this.value = value;
                ValueChanged(this, new EventArgs());
                if (Value.HasValue)
                    if (Value.Value)
                        rbtnTrue.Checked = true;
                    else
                        rbtnFalse.Checked = true;
                else rbtnNull.Checked = true;
            }
        }
        public string Json
        {
            get
            {
                var result = "";

                if (Value.HasValue)
                {
                    result = GetJsonPreffix(Key, "");

                    result += Value.ToString().ToLower();

                    result += GetJsonSuffix(Key, "");
                }

                return result;
            }
            set
            {
                var jobj = JsonConvert.DeserializeObject<JObject>(value);
                if (jobj[Key] != null)
                    Value = bool.Parse(jobj[Key].ToString());
                else
                    Value = null;
            }
        }

        public void SetParser(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Key = jobj["key"].ToString();
            if (jobj["default"] != null)
                Value = bool.Parse(jobj["default"].ToString());
        }

        private void rbtn_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnTrue.Checked)
                Value = true;
            else if (rbtnFalse.Checked)
                Value = false;
            else if (rbtnNull.Checked)
                Value = null;
        }

        public void SetSize(int width)
        {
            Width = width;
            rbtnTrue.Width = rbtnFalse.Width = rbtnNull.Width = 
                (Width - lblKey.Width - rbtnTrue.Margin.Left * 3) / 3;
            rbtnTrue.Left = lblKey.Left + lblKey.Width + rbtnTrue.Margin.Left;
            rbtnFalse.Left = rbtnTrue.Left + rbtnTrue.Width + rbtnFalse.Margin.Left;
            rbtnNull.Left = rbtnFalse.Left + rbtnFalse.Width + rbtnNull.Margin.Left;
        }
    }
}
