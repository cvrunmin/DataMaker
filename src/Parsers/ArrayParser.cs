using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Forms;
using static DataMaker.Tools;

namespace DataMaker.Parsers
{
    public partial class ArrayParser : UserControl, IParser
    {

        public ArrayParser()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);
        }

        private string frameFileName;
        private string[] values;

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
        public string Key { get; set; }
        public string[] Values
        {
            get => values;
            set
            {
                values = value;
                //upDownValue.Value = value;
                MainForm.GetInstance().IsChanged = true;
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
                //if (jobj[Key] != null)
                    //Values = decimal.Parse(jobj[Key].ToString());
            }
        }

        public void SetParser(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Key = jobj["key"].ToString();

            
        }

        public void SetSize()
        {
            //Width = upDownValue.Width + lblKey.Width;
            //Height = upDownValue.Height;
        }
    }
}
