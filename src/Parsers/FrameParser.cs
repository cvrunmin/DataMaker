﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static DataMaker.Tools;
using System;

namespace DataMaker.Parsers
{
    public partial class FrameParser : UserControl, IParser
    {
        private string frameFileName;

        public string FrameFileName
        {
            get => frameFileName;
            set
            {
                frameFileName = value;
                lblKey.Text = Lang("key_" + FrameFileName + "_" + Key);
            }
        }
        public string Key { get; set; }

        public FrameParser()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);
        }

        public ControlCollection PanelControls => mainPanel.Controls;

        public void SetSize(int width)
        {
            foreach (var i in PanelControls)
            {
                if (i is IParser)
                ((IParser)i).SetSize(width);
            }
        }

        public string Json
        {
            get
            {
                var result = GetJsonPreffix(Key, "{");

                // 合并所有Parsers的Json
                foreach (var parser in PanelControls)
                    if (parser is IParser)
                        result += ((IParser)parser).Json + ",";

                result += GetJsonSuffix(Key, "}");

                return result;
            }
            set
            {
                var jObj = JsonConvert.DeserializeObject<JObject>(value);
                foreach (var i in PanelControls)
                {
                    if (i is IParser)
                        if (i is FrameParser)
                            ((IParser)i).Json = jObj[((IParser)i).Key].ToString();
                        else
                            ((IParser)i).Json = jObj.ToString();
                }
            }
        }

        public void SetParser(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Key = jobj["key"].ToString();
            var parsersJson = File.ReadAllText(Application.StartupPath + "/Jsons/" +
                jobj["json"].ToString() + ".json");

            // 加载所有嵌套parsers
            IParser parser = null;

            foreach (var i in JsonConvert.DeserializeObject<JObject>(parsersJson).Children())
            {
                // 遍历 Frame, Text, Updown 等 Parser type
                foreach (var j in i)
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
                        parser.FrameFileName = jobj["json"].ToString();

                        //((Control)parser).Dock = DockStyle.Top;
                        PanelControls.Add((Control)parser);
                    }
            }

            // 设置Parsers的位置
            SetSize(ClientSize.Width);
        }
    }
}