using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

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
                textBoxValue.Text = value;
                Width = textBoxValue.Width + lblKey.Width;
            }
        }
        public string Key
        {
            get => key;
            set
            {
                key = value;
                lblKey.Text = value;
                Width = textBoxValue.Width + lblKey.Width;
            }
        }

        public TextParser()
        {
            InitializeComponent();
        }

        public string GetJson()
        {
            return $@"""{Key}"":""{Value}""";
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

        private void textBoxValue_TextChanged(object sender, System.EventArgs e)
            => Value = textBoxValue.Text;
    }
}
