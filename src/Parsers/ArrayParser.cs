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
            }
        }

        public string[] Values
        {
            get => values;
            set
            {
                values = value;
                listValues.Items.Clear();
                foreach (var i in Values) listValues.Items.Add(i);
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
                if (jobj[Key] != null)
                {
                    var jary = (JArray)jobj[Key];
                    Values = jary.Values<string>().ToArray();
                }
            }
        }

        public void SetParser(string json)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            Key = jobj["key"].ToString();
            var rootParserJson = 
$@"{{
    ""key"": ""%SameLevel%"",
    ""json"": ""{jobj["json"].ToString()}""
}}";
            frameRoot.SetParser(rootParserJson);
        }

        //TODO:
        //      MainForm.GetInstance().IsChanged = true

        public void SetSize(int width)
        {
            Width = width;
            // 左侧大小调整
            listValues.Width = Width / 3;
            btnAdd.Width = btnEdit.Width = btnRemove.Width = (listValues.Width - 6) / 3;
            btnRemove.Left = btnAdd.Left + btnAdd.Width + btnRemove.Margin.Left;
            btnEdit.Left = btnRemove.Left + btnRemove.Width + btnEdit.Margin.Left;

            // 右侧大小调整
            frameRoot.Width = Width / 3 * 2;
            frameRoot.Left = listValues.Left + listValues.Width + frameRoot.Margin.Left;
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