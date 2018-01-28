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
                ValueChanged?.Invoke(this, new EventArgs());
                var selectedIndex = listValues.SelectedIndex;

                // 把Values加入listValues
                listValues.Items.Clear();
                foreach (var i in Values) listValues.Items.Add(i);

                // 恢复选择的索引
                listValues.SelectedIndex = selectedIndex;
            }
        }

        public string Json
        {
            get
            {
                var result = "";

                if (Key != null)
                {
                    result = GetJsonPreffix(Key, "[");

                    if (Values != null) foreach (var i in Values) result += i + ",";

                    result += GetJsonSuffix(Key, "]");
                }

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
                // 开始编辑第0个
                EditedIndex = 0;
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

        private void listValues_DoubleClick(object sender, EventArgs e)
        {
            // 保存当前Value
            SaveEditedValue();
            // 加载选中的Value
            EditedIndex = listValues.SelectedIndex;
        }

        private int editedIndex;

        public event ValueChangedHandler ValueChanged;

        private int EditedIndex
        {
            get => editedIndex;
            set
            {
                editedIndex = value;
                if (Values.Count > EditedIndex && EditedIndex >= 0)
                    frameRoot.Json = Values[EditedIndex];
            }
        }

        /// <summary>
        /// 保存正在编辑的Value
        /// </summary>
        private void SaveEditedValue()
        {
            // 用临时变量values来修改，再把values赋值给Values
            // 这样可以触发Values的setter
            var values = Values;
            values[EditedIndex] = frameRoot.Json.Remove(frameRoot.Json.Length - 1);
            Values = values;
        }
    }
}