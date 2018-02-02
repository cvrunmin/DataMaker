using DataMaker.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;
using static DataMaker.Utils;

namespace DataMaker.Parsers
{
    public partial class NumberParser : UserControl, IParser
    {
        private string frameFileName;
        private decimal value;
        private decimal min = -2147483648;
        private decimal max = 2147483647;

        public event EventHandler ValueChanged;

        public NumberParser()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);
        }

        public string Key { get; set; }

        public decimal Default { get; set; }

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
                ValueChanged?.Invoke(this, new EventArgs());
                upDownValue.Value = value;
            }
        }

        public string Json
        {
            get
            {
                var result = GetJsonPreffix(Key, "");

                result += Value;

                result += GetJsonSuffix(Key, "");

                return result;
            }
            set
            {
                try
                {
                    var jobj = JsonConvert.DeserializeObject<JObject>(value);
                    if (jobj[Key] != null) Value = decimal.Parse(jobj[Key].ToString());
                    else Value = Default;

                    MainForm.ShowInfo("parsers_info_parsesuccessfully");
                }
                catch
                {
                    MainForm.ShowInfo("parsers_error_parsebad");
                }
            }
        }
        public decimal Max
        {
            get => max;
            set
            {
                max = value;
                upDownValue.Maximum = Max;
            }
        }
        public decimal Min
        {
            get => min;
            set
            {
                min = value;
                upDownValue.Minimum = Min;
            }
        }

        public void SetParser(string json)
        {
            try
            {
                var jobj = JsonConvert.DeserializeObject<JObject>(json);
                Key = jobj["key"].ToString();
                if (jobj["default"] != null)
                    Value = Default = decimal.Parse(jobj["default"].ToString());
                if (jobj["max"] != null)
                    Max = decimal.Parse(jobj["max"].ToString());
                if (jobj["min"] != null)
                    Min = decimal.Parse(jobj["min"].ToString());

                MainForm.ShowInfo("parsers_info_loadsuccessfully");
            }
            catch
            {
                MainForm.ShowInfo("parsers_error_loadbad");
            }
        }

        private void upDownValue_ValueChanged(object sender, EventArgs e)
        {
            if (upDownValue.Value < Min) upDownValue.Value = Min;
            else if (upDownValue.Value > Max) upDownValue.Value = Max;
            else Value = upDownValue.Value;
        }

        public void SetSize(int width)
        {
            Width = width;
            upDownValue.Left = lblKey.Left + lblKey.Width + upDownValue.Margin.Left;
            upDownValue.Width = Width - lblKey.Left - lblKey.Width - upDownValue.Margin.Left - 50;
            Height = Math.Max(upDownValue.Height, lblKey.Width);
        }
    }
}
