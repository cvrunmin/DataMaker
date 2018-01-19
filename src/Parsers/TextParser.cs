using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataMaker.Parsers
{
    public partial class TextParser : UserControl, IParser
    {
        private string value;
        private string key;

        public string Value
        {
            get => value;
            set
            {
                this.value = value;
                textBox1.Text = value;
                Width = textBox1.Width + lblKey.Width;
            }
        }
        public string Key
        {
            get => key;
            set
            {
                key = value;
                lblKey.Text = value;
                Width = textBox1.Width + lblKey.Width;
            }
        }

        public TextParser()
        {
            InitializeComponent();
        }

        public string GetJson()
        {
            return $@"""{Key}"": ""{Value}""";
        }

        public void SetJson(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Value = jobj[Key].ToString();
        }

        public void SetParser(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Key = jobj["key"].ToString();
            Value = jobj["default"].ToString();
        }
    }
}
