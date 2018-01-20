using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Forms;
using static DataMaker.Tools;

namespace DataMaker.Parsers
{
    public partial class FrameParser : UserControl, IParser
    {
        private string frameFileName;
        private string key;

        public string FrameFileName
        {
            get => frameFileName;
            set
            {
                frameFileName = value;
                lblKey.Text = Lang("key_" + FrameFileName + "_" + Key);
            }
        }
        public string Key
        {
            get => key;
            set
            {
                key = value;
            }
        }

        public FrameParser()
        {
            InitializeComponent();
        }

        //public override Color BackColor { get => mainPanel.BackColor; set => mainPanel.BackColor = value; }
        //public override Color ForeColor { get => mainPanel.ForeColor; set => mainPanel.ForeColor = value; }
        //public override Font Font { get => base.Font; set => base.Font = value; }
        public ControlCollection PanelControls => mainPanel.Controls;

        public string GetJson()
        {
            var json = "\"" + Key + "\":" + "{";

            foreach (var parser in PanelControls)
                if (parser is IParser)
                    json += ((IParser)parser).GetJson() + ",";

            json += "}";

            return json;
        }

        public void SetParser(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Key = jobj["key"].ToString();
            var parsersJson = File.ReadAllText(Application.StartupPath + "/Jsons/" + jobj["json"].ToString() + ".json");

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
                            case "frame":
                                parser = new FrameParser();
                                break;
                            case "text":
                                parser = new TextParser();
                                break;
                            case "updown":
                                parser = new UpDownParser();
                                break;
                            default:
                                continue;
                        }

                        parser.SetParser(k.ToString());
                        parser.FrameFileName = jobj["json"].ToString();
                        PanelControls.Add((Control)parser);
                    }
            }
        }

        public void SetJson(string json)
        {
            var jObj = JsonConvert.DeserializeObject<JObject>(json);
            foreach (var i in PanelControls)
            {
                if (i is IParser)
                    if (i is FrameParser)
                        ((IParser)i).SetJson(jObj[((IParser)i).Key].ToString());
                    else
                        ((IParser)i).SetJson(jObj.ToString());
            }
        }
    }
}