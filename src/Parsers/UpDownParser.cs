using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using static DataMaker.Tools;
using System;

namespace DataMaker.Parsers
{
    public partial class UpDownParser : UserControl, IParser
    {
        private string frameFileName;
        private decimal value;
        private string key;

        public string FrameFileName
        {
            get => frameFileName;
            set
            {
                frameFileName = value;
                lblKey.Text = Lang("key_" + FrameFileName + "_" + Key);
                Width = upDownValue.Width + lblKey.Width;
            }
        }
        public string Key
        {
            get => key;
            set => key = value;
        }
        public decimal Value
        {
            get => value;
            set
            {
                this.value = value;
                upDownValue.Value = value;
                MainForm.GetInstance().IsChanged = true;
            }
        }
        public decimal Max { get; set; } = 2147483647;
        public decimal Min { get; set; } = -2147483648;

        public UpDownParser()
        {
            InitializeComponent();
        }

        public string GetJson()
        {
            return $@"""{Key}"":{Value}";
        }

        public void SetJson(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            if (jobj[Key] != null)
                Value = decimal.Parse(jobj[Key].ToString());
        }

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
        }
    }
}
