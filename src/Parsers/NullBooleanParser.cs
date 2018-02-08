using DataMaker.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static DataMaker.Utils;

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

        public event EventHandler ValueChanged;

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
                ValueChanged?.Invoke(this, new EventArgs());
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
                try
                {
                    var jObj = JsonConvert.DeserializeObject<JObject>(value);
                    if (jObj[Key] != null)
                        Value = bool.Parse(jObj[Key].ToString());
                    else
                        Value = null;

                    MainForm.ShowInfoBar("parsers_info_parsesuccessfully");
                }
                catch
                {
                    MainForm.ShowInfoBar("parsers_error_parsebad");
                }
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
                    Value = bool.Parse(jObj["default"].ToString());
                }

                MainForm.ShowInfoBar("parsers_info_loadsuccessfully");
            }
            catch
            {
                MainForm.ShowInfoBar("parsers_error_loadbad");
            }
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
