using DataMaker.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static DataMaker.Utils;

namespace DataMaker.Parsers
{
    public partial class FrameParser : UserControl, IParser
    {
        private string frameFileName;

        public event EventHandler ValueChanged;

        #region 属性
        public string FrameFileName
        {
            get => frameFileName;
            set
            {
                frameFileName = value;
                if (FrameFileName != null)
                    lblKey.Text = Lang("key_" + FrameFileName + "_" + Key);
            }
        }

        public string Key
        {
            get;
            set;
        }

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
        #endregion

        public FrameParser()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);
        }
        
        public void SetSize(int width)
        {
            Width = width;
            foreach (var i in mainPanel.Controls) if (i is IParser) ((IParser)i).SetSize(width);
        }

        public string Json
        {
            get
            {
                var result = GetJsonPreffix(Key, "{");

                // 合并所有Parsers的Json
                if (mainPanel.Controls != null)
                {
                    foreach (var parser in mainPanel.Controls)
                    {
                        if (parser is IParser)
                        {
                            if (!((IParser)parser).Ignore)
                            {
                                result += ((IParser)parser).Json + ",";
                            }
                        }
                    }
                }

                result += GetJsonSuffix(Key, "}");

                return result;
            }
            set
            {
                //try
                //{
                    var json = value;
                    if (json != null)
                    {
                        // 补全Json为Parser能够读取的格式
                        var jObj = new JObject();
                        if (JsonConvert.DeserializeObject<JToken>(json) is JObject)
                            jObj = JsonConvert.DeserializeObject<JObject>(json);
                        else if (JsonConvert.DeserializeObject<JToken>(json) is JValue)
                            jObj = JsonConvert.DeserializeObject<JObject>(
                                "{\"%NoKey%\":" + json + "}");

                        foreach (var i in mainPanel.Controls)
                            if (i is IParser)
                                if (i is FrameParser)
                                    ((IParser)i).Json = jObj[((IParser)i).Key].ToString();
                                else ((IParser)i).Json = jObj.ToString();
                    }

                    MainForm.ShowInfoBar("parsers_info_parsesuccessfully");
                //}
                //catch
                //{
                //    MainForm.ShowInfoBar("parsers_error_parsebad");
                //}
            }
        }

        public ControlCollection PanelControls => mainPanel.Controls;

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

                var parsersJson = File.ReadAllText(Application.StartupPath + "/Jsons/" +
                    jObj["json"].ToString() + ".json");

                LoadAllParsers(parsersJson, jObj);

                LayoutAllParsers();
            //}
            //catch
            //{
            //    MainForm.ShowInfoBar("parsers_error_loadbad");
            //}
        }

        /// <summary>
        /// 排序所有内部Parsers
        /// </summary>
        public void LayoutAllParsers()
        {
            foreach (var i in mainPanel.Controls)
            {
                if (i is IParser)
                {
                    mainPanel.Controls.SetChildIndex((Control)i, ((IParser)i).ShowIndex);
                }
            }
        }

        /// <summary>
        /// 加载所有嵌套parsers
        /// </summary>
        /// <param name="json">被加载frame的Json</param>
        /// <param name="jObj">当前frame的JObject</param>
        private void LoadAllParsers(string json, JObject jObj)
        {
            IParser parser = null;
            mainPanel.Controls.Clear();

            foreach (var i in JsonConvert.DeserializeObject<JObject>(json).Children())
            {
                // 遍历 Frame, Text, Updown 等 Parser type
                foreach (var j in i)
                {
                    foreach (var k in j)
                    {
                        // 遍历 Parser type 下的每一个实例
                        switch (((JProperty)i).Name)
                        {
                            // NOTE: 新增 Parser 需要在此处增加代码
                            case "array":
                                parser = new ArrayParser();
                                break;
                            case "boolean":
                                parser = new BooleanParser();
                                break;
                            case "frame":
                                parser = new FrameParser();
                                break;
                            case "nullboolean":
                                parser = new NullBooleanParser();
                                break;
                            case "number":
                                parser = new NumberParser();
                                break;
                            case "text":
                                parser = new TextParser();
                                break;
                            default:
                                continue;
                        }

                        parser.SetParser(k.ToString());
                        parser.FrameFileName = jObj["json"].ToString();
                        // 注册事件
                        parser.ValueChanged += parser_ValueChanged;

                        mainPanel.Controls.Add((Control)parser);
                    }
                }

                MainForm.ShowInfoBar("parsers_info_loadsuccessfully");

                // 设置Parsers的位置
                SetSize(ClientSize.Width);
            }
        }

        private void parser_ValueChanged(object sender, EventArgs e)
        {
            // 触发此frame的ValueChanged事件
            ValueChanged?.Invoke(sender, e);

            // 判断各个parsers的Conditions是否符合
            // Conditions是一群or的and
            // [[A,B], [C]]
            // equals
            // if ((A | B) & C)
            // 遍历所有Parsers
            foreach (var parser in mainPanel.Controls)
            {
                if (parser is IParser)
                {
                    var enabled = true;

                    // 遍历所有Conditions
                    foreach (var j in ((IParser)parser).Conditions)
                    {
                        // 这一层是and
                        enabled = false;

                        foreach (var k in j)
                        {
                            // 这一层是or
                            if (Condition.Parse(k, this).IsTrue())
                            {
                                enabled = true;
                                break;
                            }
                        }

                        if (!enabled)
                        {
                            break;
                        }
                    }

                    // 决定是否可用
                    ((Control)parser).Enabled = enabled;
                }
            }
        }
    }
}