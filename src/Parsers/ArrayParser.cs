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
        private bool isSettingValue;

        public event ValueChangedHandler ValueChanged;

        public ArrayParser()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);

            frameRoot.ValueChanged +=
                (object sender, EventArgs e) =>
                {
                    if (!isSettingValue) SaveEditedValue();
                };
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

        private List<string> Values
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
                listValues.SelectedIndex = 0;
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
        
        private void listValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetButtons();
            EditSelectedValue();
        }

        /// <summary>
        /// 保存正在编辑的Value
        /// </summary>
        private void SaveEditedValue()
        {
            // 用临时变量values来修改，再把values赋值给Values
            // 这样可以触发Values的setter
            var values = Values;
            values[listValues.SelectedIndex] = frameRoot.Json.Remove(frameRoot.Json.Length - 1);
            Values = values;
        }

        /// <summary>
        /// 编辑选中的Value
        /// </summary>
        private void EditSelectedValue()
        {
            isSettingValue = true;
            if (Values.Count > listValues.SelectedIndex && listValues.SelectedIndex >= 0)
                frameRoot.Json = Values[listValues.SelectedIndex];
            isSettingValue = false;
        }

        /// <summary>
        /// 设置按钮可用性
        /// </summary>
        private void SetButtons()
        {
            if (listValues.SelectedIndex >= 0)
            {
                btnRemove.Enabled = true;
                btnEdit.Enabled = true;
            }
            else
            {
                btnRemove.Enabled = false;
                btnEdit.Enabled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 赋一次值，这样会刷新列表
            var values = Values;
            values.Insert(listValues.SelectedIndex, "");
            Values = values;

            // 赋一次值，这样会读取新增的Value
            listValues.SelectedIndex = listValues.SelectedIndex;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // 赋一次值，这样会刷新列表
            var values = Values;
            values.Remove(Values[listValues.SelectedIndex]);
            Values = values;

            // 赋一次值，这样谁都不会读取
            listValues.SelectedIndex = -1;
        }

        private void btnEdit_Click(object sender, EventArgs e) => EditSelectedValue();
    }
}