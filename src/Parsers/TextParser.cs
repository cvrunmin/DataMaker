using DataMaker.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                // Note: ZoneTextParser替换变量
                Replaces:
                var isChanged = false;
                foreach (var i in zone)
                {
                    switch (i)
                    {
                        case "%Same%":
                            GetSameItems(i);
                            isChanged = true;
                            break;
                        case "%SameWithoutItself%":
                            GetSameItemsWithoutItself(i);
                            isChanged = true;
                            break;
                        case "%Similar%":
                            GetSimilarItems(i);
                            isChanged = true;
                            break;
                        case "%SimilarWithoutItself%":
                            GetSimilarItemsWithoutItself(i);
                            isChanged = true;
                            break;
                        default:
                            break;
                    }
                    // 集合修改了，重新继续替换
                    // 噢我的上帝，我竟然用上了goto
                    if (isChanged) goto Replaces;
                }

                // 加入列表项
                comboBoxValue.AllItems.Clear();
                comboBoxValue.AllItems.AddRange(zone.ToArray());
            }
        }

        private void GetSimilarItemsWithoutItself(string i)
        {
            GetSimilarItems(i);
            if (zone.Contains(MainForm.GetInstance().EditedNode.GetID()))
                zone.Remove(MainForm.GetInstance().EditedNode.GetID());
        }

        private void GetSimilarItems(string i)
        {
            GetSameItems(i);

            switch (((Item)MainForm.GetInstance().EditedNode).Sort)
            {
                case ItemSort.Function:
                case ItemSort.FunctionTag:
                    if (((Item)MainForm.GetInstance().EditedNode).Sort == ItemSort.Function)
                        zone.AddRange(FileTree.GetAllIds(ItemSort.FunctionTag, true));
                    else
                        zone.AddRange(FileTree.GetAllIds(ItemSort.Function, true));
                    break;
                default:
                    break;
            }
        }

        private void GetSameItemsWithoutItself(string i)
        {
            GetSameItems(i);
            if (zone.Contains(MainForm.GetInstance().EditedNode.GetID()))
                zone.Remove(MainForm.GetInstance().EditedNode.GetID());
        }

        private void GetSameItems(string i)
        {
            zone.Remove(i);
            zone.AddRange(FileTree.GetAllIds(
                    ((Item)MainForm.GetInstance().EditedNode).Sort, true));
        }

        public bool CanOutOfZone
        {
            get => canOutOfZone;
            set
            {
                canOutOfZone = value;
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

                var tempIndex = comboBoxValue.SelectionStart;
                var tempLength = comboBoxValue.SelectionLength;

                comboBoxValue.Text = value;

                comboBoxValue.SelectionStart = tempIndex;
                comboBoxValue.SelectionLength = tempLength;

                ValueChanged?.Invoke(this, new EventArgs());
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
                try
                {
                    var jobj = JsonConvert.DeserializeObject<JObject>(value);
                    if (jobj[Key] != null) Value = jobj[Key].ToString();
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
            try
            {
                var jObj = JsonConvert.DeserializeObject<JObject>(json);
                Key = jObj["key"].ToString();
                if (jObj["default"] != null)
                {
                    Value = Default = jObj["default"].ToString();
                }

                if (jObj["can_out_of_zone"] != null)
                {
                    CanOutOfZone = jObj["can_out_of_zone"].ToObject<bool>();
                }

                if (jObj["zone"] != null)
                {
                    Zone = ((JArray)jObj["zone"]).ToObject<List<string>>();
                }
                else
                {
                    Zone = new List<string>();
                }

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
            comboBoxValue.Left = lblKey.Left + lblKey.Width + comboBoxValue.Margin.Left;
            comboBoxValue.Width = Width - lblKey.Left - lblKey.Width - comboBoxValue.Margin.Left - 50;
            Height = 30;
        }

        private void comboBoxValue_TextChanged(object sender, EventArgs e)
        {
            // 不能出区域，不保存意外值
            // 能的话就直接保存
            if (CanOutOfZone || comboBoxValue.AllItems.Contains(comboBoxValue.Text))
                Value = comboBoxValue.Text;
            else
                MainForm.ShowInfoBar("textparser_warn_outofzone");
        }
    }
}
