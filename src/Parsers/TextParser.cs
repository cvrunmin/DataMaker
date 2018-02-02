using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static DataMaker.Utils;

namespace DataMaker.Parsers
{
    public partial class TextParser : UserControl, IParser
    {
        private bool canOutOfZone = true;
        private string frameFileName;
        private string value;
        private List<string> zone;

        public event EventHandler ValueChanged;

        public TextParser()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);
        }

        public string Key { get; set; }
        public string Default { get; set; }
        public List<string> Zone
        {
            get => zone;
            set
            {
                zone = value;
                if (CanOutOfZone)
                    // 可以出界，无元素就显示文本框，有元素就显示下拉
                    if (Zone.Count == 0) comboBoxValue.DropDownStyle = ComboBoxStyle.Simple;
                    else comboBoxValue.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }
        public bool CanOutOfZone
        {
            get => canOutOfZone;
            set
            {
                canOutOfZone = value;
                if (!CanOutOfZone) comboBoxValue.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }
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
                ValueChanged?.Invoke(this, new EventArgs());
                comboBoxValue.Text = value;
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
                if (jobj[Key] != null) Value = jobj[Key].ToString();
                else Value = Default;
            }
        }

        public void SetParser(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Key = jobj["key"].ToString();
            if (jobj["default"] != null)
                Value = Default = jobj["default"].ToString();
            if (jobj["can_out_of_zone"] != null)
                CanOutOfZone = jobj["can_out_of_zone"].ToObject<bool>();
            if (jobj["zone"] != null)
                Zone = ((JArray)jobj["zone"]).ToObject<List<string>>();
            else
                Zone = new List<string>();
        }

        public void SetSize(int width)
        {
            Width = width;
            comboBoxValue.Left = lblKey.Left + lblKey.Width + comboBoxValue.Margin.Left;
            comboBoxValue.Width = Width - lblKey.Left - lblKey.Width - comboBoxValue.Margin.Left - 50;
            // FIXME: 高度Hardcode
            Height = 30;
        }

        private void comboBoxValue_TextChanged(object sender, EventArgs e)
        {
            Value = comboBoxValue.Text;
        }
    }
}
