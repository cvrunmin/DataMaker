using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static DataMaker.Tools;

namespace DataMaker.Parsers
{
    public partial class ArrayParser : UserControl, IParser
    {
        private string frameFileName;
        private List<string> values;

        public ArrayParser()
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

        public List<string> Values
        {
            get => values;
            set
            {
                values = value;
                listValues.Items.Clear();
                foreach (var i in Values) listValues.Items.Add(i);
            }
        }

        public string Json
        {
            get
            {
                var result = GetJsonPreffix(Key, "[");

                if (Values != null) foreach(var i in Values) result += i + ",";

                result += GetJsonSuffix(Key, "]");

                return result;
            }
            set
            {
                var jObj = JsonConvert.DeserializeObject<JObject>(value);
                if (jObj[Key] != null)
                {
                    // 加载数组
                    var JAry = (JArray)jObj[Key];
                    var values = new List<string>();
                    // 把数组内容的Json加入Value
                    foreach (var i in JAry)
                        values.Add(GetLeftBrackets(i) + i.ToString() + GetRightBrackets(i));
                    Values = values;
                }
            }
        }

        public void SetParser(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Key = jobj["key"].ToString();
            var rootParserJson = 
$@"{{
    ""key"": ""%NoKey%%NoBrackets%"",
    ""json"": ""{jobj["json"].ToString()}""
}}";
            frameRoot.SetParser(rootParserJson);
        }

        //TODO:
        //      MainForm.GetInstance().IsChanged = true

        public void SetSize(int width)
        {
            Width = width;
            // 左侧大小调整
            listValues.Width = Width / 3;
            btnAdd.Width = btnEdit.Width = btnRemove.Width = (listValues.Width - 6) / 3;
            btnRemove.Left = btnAdd.Left + btnAdd.Width + btnRemove.Margin.Left;
            btnEdit.Left = btnRemove.Left + btnRemove.Width + btnEdit.Margin.Left;

            // 右侧大小调整
            frameRoot.SetSize(Width / 3 * 2);
            frameRoot.Left = listValues.Left + listValues.Width + frameRoot.Margin.Left;
        }

        private void listValues_DoubleClick(object sender, System.EventArgs e)
        {
            if (listValues.SelectedIndex >= 0)
            {
                frameRoot.Json = Values[listValues.SelectedIndex];
            }
        }
    }
}