using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataMaker.Parsers
{
    public partial class FrameParser : UserControl, IParser
    {
        public FrameParser()
        {
            InitializeComponent();
        }

        //public override Color BackColor { get => mainPanel.BackColor; set => mainPanel.BackColor = value; }
        //public override Color ForeColor { get => mainPanel.ForeColor; set => mainPanel.ForeColor = value; }
        //public override Font Font { get => base.Font; set => base.Font = value; }
        public new ControlCollection Controls => mainPanel.Controls;

        public string Key { get; set; }

        public string GetJson()
        {
            var json = "{";

            foreach (var parser in Controls)
                if (parser is IParser)
                    json += ((IParser)parser).GetJson() + ",";

            json += "}";

            return json;
        }

        public void SetParser(string json)
        {
            if (JsonConvert.DeserializeObject<JObject>(json)["frame"] != null)
                foreach (var i in JsonConvert.DeserializeObject<JObject>(json)["frame"])
                {
                    var parser = new FrameParser();
                    parser.SetParser(i.ToString());
                    Controls.Add(parser);
                }

            if (JsonConvert.DeserializeObject<JObject>(json)["text"] != null)
                foreach (var i in JsonConvert.DeserializeObject<JObject>(json)["text"])
                {
                    var parser = new TextParser();
                    parser.SetParser(i.ToString());
                    Controls.Add(parser);
                }
        }

        public void SetJson(string json)
        {
            foreach (var i in Controls)
            {
                if (i is IParser)
                    ((IParser)i).SetJson(json);
            }
        }
    }
}
