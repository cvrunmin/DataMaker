using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using static DataMaker.Tools;

namespace DataMaker.Parsers
{
    public partial class TextParser : UserControl, IParser
    {
        private string frameFileName;
        private string value;
        private string key;

        public string FrameFileName
        {
            get => frameFileName;
            set
            {
                frameFileName = value;
                lblKey.Text = Lang("key_" + FrameFileName + "_" + Key);
                Width = textBoxValue.Width + lblKey.Width;
            }
        }
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
