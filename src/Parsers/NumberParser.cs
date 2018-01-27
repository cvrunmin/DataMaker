using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;
using static DataMaker.Tools;

namespace DataMaker.Parsers
{
    public partial class NumberParser : UserControl, IParser
    {
        private string frameFileName;
        private decimal value;

        public NumberParser()
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

        public decimal Value
        {
            get => value;
            set
            {
                this.value = value;
                upDownValue.Value = value;
            }
        }

        public string Json
        {
            get => $@"""{Key}"":{Value}";
            set
            {
                var jobj = JsonConvert.DeserializeObject<JObject>(value);
                if (jobj[Key] != null)
                    Value = decimal.Parse(jobj[Key].ToString());
            }
        }
        public decimal Max { get; set; } = 2147483647;
        public decimal Min { get; set; } = -2147483648;
        
        public void SetParser(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Key = jobj["key"].ToString();
            if (jobj["default"] != null)
                Value = decimal.Parse(jobj["default"].ToString());
            if (jobj["max"] != null)
                Max = decimal.Parse(jobj["max"].ToString());
            if (jobj["min"] != null)
                Min = decimal.Parse(jobj["min"].ToString());
        }

        private void upDownValue_ValueChanged(object sender, EventArgs e)
        {
            if (upDownValue.Value < Min) upDownValue.Value = Min;
            else if (upDownValue.Value > Max) upDownValue.Value = Max;
            else Value = upDownValue.Value;
            MainForm.GetInstance().IsChanged = true;
        }

        public void SetSize(int width)
        {
            Width = width;
            upDownValue.Left = lblKey.Left + lblKey.Width + upDownValue.Margin.Left;
            upDownValue.Width = Width - lblKey.Left - lblKey.Width - upDownValue.Margin.Left;
            Height = Math.Max(upDownValue.Height, lblKey.Width);
        }
    }
}
