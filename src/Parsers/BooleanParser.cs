using DataMaker.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static DataMaker.Utils;

namespace DataMaker.Parsers
{
    public partial class BooleanParser : UserControl, IParser
    {
        private string frameFileName;
        private bool value;

        public event EventHandler ValueChanged;

        public BooleanParser()
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

        public bool Default { get; set; }

        public string FrameFileName
        {
            get => frameFileName;
            set
            {
                frameFileName = value;
                checkBoxValue.Text = Lang("key_" + FrameFileName + "_" + Key);
            }
        }

        public bool Value
        {
            get => value;
            set
            {
                this.value = value;
                ValueChanged?.Invoke(this, new EventArgs());
                checkBoxValue.Checked = Value;
            }
        }

        public string Json
        {
            get
            {
                var result = GetJsonPreffix(Key, "");

                result += Value.ToString().ToLower();

                result += GetJsonSuffix(Key, "");

                return result;
            }
            set
            {
                try
                {
                    var jObj = JsonConvert.DeserializeObject<JObject>(value);
                    if (jObj[Key] != null) Value = bool.Parse(jObj[Key].ToString());
                    else Value = Default;

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
            //try
            //{
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
                    Value = Default = bool.Parse(jObj["default"].ToString());

                MainForm.ShowInfoBar("parsers_info_loadsuccessfully");
            //}
            //catch
            //{
            //    MainForm.ShowInfoBar("parsers_error_loadbad");
            //}
        }

        private void checkBoxValue_CheckedChanged(object sender, EventArgs e)
        {
            Value = checkBoxValue.Checked;
        }

        public void SetSize(int width)
        {
            Width = width;
            checkBoxValue.Width = Width - 50;
        }
    }
}
