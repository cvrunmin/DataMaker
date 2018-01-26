using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static DataMaker.Tools;

namespace DataMaker.Parsers
{
    public partial class ArrayParser : UserControl, IParser
    {
        private string frameFileName;
        private string[] values;

        public ArrayParser()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);
        }

        public string Key { get; set; }

        public string FrameFileName
        {
            get => frameFileName;
            set
            {
                frameFileName = value;
                lblKey.Text = Lang("key_" + FrameFileName + "_" + Key);
                SetSize();
            }
        }

        public string[] Values
        {
            get => values;
            set
            {
                values = value;
            }
        }

        public string Json
        {
            get
            {
                var result = "";

                foreach (var i in Values)
                    result += i + ",";

                return result;
            }
            set
            {
                var jobj = JsonConvert.DeserializeObject<JObject>(value);
                if (jobj["key"] != null)
                {
                    var jary = (JArray)jobj["key"];
                    Values = jary.Values<string>().ToArray();
                }
            }
        }

        public void SetParser(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Key = jobj["key"].ToString();
            frameRoot.SetParser(File.ReadAllText(jobj["json"].ToString()));
        }

        // TODO:
        //      MainForm.GetInstance().IsChanged = true

        public void SetSize()
        {
            //Width = upDownValue.Width + lblKey.Width;
            //Height = upDownValue.Height;
        }

        private void listValues_DoubleClick(object sender, System.EventArgs e)
        {
            if (listValues.SelectedIndex >= 0)
            {
                frameRoot.Json = Values[listValues.SelectedIndex];
            }
        }
    }
}
