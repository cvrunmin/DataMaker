using DataMaker.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

        public int ShowIndex
        {
            get;
            set;
        }

        public bool Ignore
        {
            get;
            set;
        }

        public List<List<string>> Conditions
        {
            get;
            set;
        } = new List<List<string>>();

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
                //try
                //{
                    var jObj = JsonConvert.DeserializeObject<JObject>(value);
                    if (jObj[Key] != null) Value = decimal.Parse(jObj[Key].ToString());
                    else Value = Default;

                    MainForm.ShowInfoBar("parsers_info_parsesuccessfully");
                //}
                //catch
                //{
                //    MainForm.ShowInfoBar("parsers_error_parsebad");
                //}
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
                var jObj = JsonConvert.DeserializeObject<JObject>(json);
                Key = jObj["key"].ToString();
                ShowIndex = int.Parse(jObj["show_index"].ToString());
                if (jObj["ignore"] != null)
                {
                    Ignore = jObj["ignore"].ToObject<bool>();
                }
                if (jObj["conditions"] != null)
                {
                    Conditions = jObj["conditions"].ToObject<List<List<string>>>();
                }
                if (jObj["default"] != null)
                {
                    Value = Default = decimal.Parse(jObj["default"].ToString());
                }
                if (jObj["max"] != null)
                {
                    Max = decimal.Parse(jObj["max"].ToString());
                }
                if (jObj["min"] != null)
                {
                    Min = decimal.Parse(jObj["min"].ToString());
                }

                MainForm.ShowInfoBar("parsers_info_loadsuccessfully");
            }
            catch
            {
                MainForm.ShowInfoBar("parsers_error_loadbad");
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
