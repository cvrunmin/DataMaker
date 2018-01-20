using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Forms;

namespace DataMaker.Parsers
{
    public partial class FrameParser : UserControl, IParser
    {
        public string Key { get; set; }

        public FrameParser()
        {
            InitializeComponent();
        }

        //public override Color BackColor { get => mainPanel.BackColor; set => mainPanel.BackColor = value; }
        //public override Color ForeColor { get => mainPanel.ForeColor; set => mainPanel.ForeColor = value; }
        //public override Font Font { get => base.Font; set => base.Font = value; }
        public new ControlCollection Controls => mainPanel.Controls;


        public string GetJson()
        {
            var json = "\"" + Key + "\":" + "{";

            foreach (var parser in Controls)
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

            if (JsonConvert.DeserializeObject<JObject>(parsersJson)["frame"] != null)
                foreach (var i in JsonConvert.DeserializeObject<JObject>(parsersJson)["frame"])
                {
                    var parser = new FrameParser();
                    parser.SetParser(i.ToString());
                    Controls.Add(parser);
                }

            if (JsonConvert.DeserializeObject<JObject>(parsersJson)["text"] != null)
                foreach (var i in JsonConvert.DeserializeObject<JObject>(parsersJson)["text"])
                {
                    var parser = new TextParser();
                    parser.SetParser(i.ToString());
                    Controls.Add(parser);
                }
        }

        public void SetJson(string json)
        {
            var jObj = JsonConvert.DeserializeObject<JObject>(json);
            foreach (var i in Controls)
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
