using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static DataMaker.Utils;
using System;
using DataMaker.Forms;

namespace DataMaker.Parsers
{
    public partial class FrameParser : UserControl, IParser
    {
        private string frameFileName;

        public event EventHandler ValueChanged;

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
        public string Key { get; set; } /*= "%NoKey%";*/

        public FrameParser()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);
        }

        public ControlCollection PanelControls => mainPanel.Controls;

        public void SetSize(int width)
        {
            Width = width;
            foreach (var i in PanelControls) if (i is IParser) ((IParser)i).SetSize(width);
        }

        public string Json
        {
            get
            {
                var result = GetJsonPreffix(Key, "{");

                // 合并所有Parsers的Json
                if (PanelControls != null)
                    foreach (var parser in PanelControls)
                        if (parser is IParser)
                            result += ((IParser)parser).Json + ",";

                result += GetJsonSuffix(Key, "}");

                return result;
            }
            set
            {
                try
                {
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

                        foreach (var i in PanelControls)
                            if (i is IParser)
                                if (i is FrameParser)
                                    ((IParser)i).Json = jObj[((IParser)i).Key].ToString();
                                else ((IParser)i).Json = jObj.ToString();
                    }

                    MainForm.ShowInfo("parsers_info_parsesuccessfully");
                }
                catch
                {
                    MainForm.ShowInfo("parsers_error_parsebad");
                }
            }
        }

        public void SetParser(string json)
        {
            try
            {
                var jobj = JsonConvert.DeserializeObject<JObject>(json);
                Key = jobj["key"].ToString();
                var parsersJson = File.ReadAllText(Application.StartupPath + "/Jsons/" +
                    jobj["json"].ToString() + ".json");

                // 加载所有嵌套parsers
                IParser parser = null;
                PanelControls.Clear();

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
                            // 当 parser 的 ValueChanged 事件触发时触发当前 frame 的 ValueChanged 事件
                            // Amazing lambda【
                            parser.ValueChanged +=
                                (object sender, EventArgs e) => ValueChanged?.Invoke(sender, e);

                            PanelControls.Add((Control)parser);
                        }

                    MainForm.ShowInfo("parsers_info_loadsuccessfully");

                    // 设置Parsers的位置
                    SetSize(ClientSize.Width);
                }
            }
            catch
            {
                MainForm.ShowInfo("parsers_error_loadbad");
            }
        }
    }
}