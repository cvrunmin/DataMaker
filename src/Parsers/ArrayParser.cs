using DataMaker.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static DataMaker.Utils;

namespace DataMaker.Parsers
{
    public partial class ArrayParser : UserControl, IParser
    {
        private string frameFileName;
        private List<string> values;
        private bool isSettingValue;
        private int editedIndex;

        public event EventHandler ValueChanged;

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
        
        private int EditedIndex
        {
            get => editedIndex;
            set
            {
                editedIndex = value;
                if (Values.Count - 1 >= EditedIndex)
                    listValues.SelectedIndex = EditedIndex;
                SetButtons();
                EditValue();
            }
        }

        private List<string> Values
        {
            get => values;
            set
            {
                values = value;
                ValueChanged?.Invoke(this, new EventArgs());
                var selectedIndex = EditedIndex;

                // 把Values加入listValues
                listValues.Items.Clear();
                foreach (var i in Values) listValues.Items.Add(i);

                // 恢复选择的索引
                if (selectedIndex >= 0) EditedIndex = selectedIndex;
                else EditedIndex = 0;
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
                try
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
                var jobj = JsonConvert.DeserializeObject<JObject>(json);
                Key = jobj["key"].ToString();
                var rootParserJson =
$@"{{
    ""key"": ""%NoKey%%NoBrackets%"",
    ""json"": ""{jobj["json"].ToString()}""
}}";
                frameRoot.SetParser(rootParserJson);

                MainForm.ShowInfoBar("parsers_info_loadsuccessfully");
            }
            catch
            {
                MainForm.ShowInfoBar("parsers_error_loadbad");
            }
        }

        public void SetSize(int width)
        {
            Width = width;
            // 左侧大小调整
            listValues.Width = Width / 3;
            btnAdd.Width = btnRemove.Width = (listValues.Width - 6) / 2;
            btnRemove.Left = btnAdd.Left + btnAdd.Width + btnRemove.Margin.Left;

            // 右侧大小调整
            frameRoot.SetSize(Width / 3 * 2);
            frameRoot.Left = listValues.Left + listValues.Width + frameRoot.Margin.Left;
        }

        /// <summary>
        /// 保存正在编辑的Value
        /// </summary>
        private void SaveEditedValue()
        {
            Values[EditedIndex] = frameRoot.Json.Remove(frameRoot.Json.Length - 1);
            // 触发一次 Setter
            Values = Values;
        }

        /// <summary>
        /// 编辑EditedIndex上的Value
        /// </summary>
        private void EditValue()
        {
            isSettingValue = true;

            if (Values.Count - 1 >= EditedIndex && EditedIndex >= 0)
                frameRoot.Json = Values[EditedIndex];
            else frameRoot.Json = "{}";

            isSettingValue = false;
        }

        /// <summary>
        /// 设置按钮可用性
        /// </summary>
        private void SetButtons()
        {
            if (EditedIndex >= 0)
            {
                btnRemove.Enabled = true;
            }
            else
            {
                btnRemove.Enabled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Values.Count - 1 >= EditedIndex && EditedIndex >= 0)
                Values.Insert(EditedIndex, "");
            else
                Values.Insert(0, "");

            // 触发 Setter
            Values = Values;
            EditedIndex = EditedIndex;

            // 设置焦点
            frameRoot.Focus();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            Values.Remove(Values[EditedIndex]);
            // 触发 Setter
            Values = Values;

            // 读取删除后的上一个位置
            if (EditedIndex > 0) EditedIndex = EditedIndex - 1;
            else if (EditedIndex == 0) EditedIndex = 0;
            // 如果没东西读取的话老子就不读了
            if (listValues.Items.Count == 0) EditedIndex = -1;

            // 设置焦点
            frameRoot.Focus();
        }

        // 当赋值后值一样时并不会触发此事件
        private void listValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditedIndex = listValues.SelectedIndex;
        }
    }
}