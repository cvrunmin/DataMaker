using FastColoredTextBoxNS;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static DataMaker.Utils;

namespace DataMaker.Forms
{
    public partial class FunctionEditor : Form
    {
        public FunctionEditor()
        {
            InitializeComponent();
            
            SetTheme();
        }

        private void SetTheme()
        {
            DarkTheme.Initialize(this);

            fctbJson.BackColor = DarkTheme.PressedColor;
            fctbJson.ServiceColors.CollapseMarkerBackColor = DarkTheme.BackColor;
            fctbJson.ServiceColors.CollapseMarkerBorderColor = DarkTheme.ForeColor;
            fctbJson.ServiceColors.CollapseMarkerForeColor = DarkTheme.ForeColor;
            fctbJson.ServiceColors.ExpandMarkerBackColor = DarkTheme.BackColor;
            fctbJson.ServiceColors.ExpandMarkerBorderColor = DarkTheme.ForeColor;
            fctbJson.ServiceColors.ExpandMarkerBorderColor = DarkTheme.ForeColor;
            fctbJson.IndentBackColor = DarkTheme.BackColor;

            fctbJson.ChangeFontSize(6);
        }
        
        TextStyle stringStyle = new TextStyle(Brushes.Orange, null, FontStyle.Regular);
        TextStyle numberStyle = new TextStyle(Brushes.LightSkyBlue, null, FontStyle.Regular);
        TextStyle wrongStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);

        private void fctbJson_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 清除颜色样式
            e.ChangedRange.ClearStyle(stringStyle);
            e.ChangedRange.ClearStyle(numberStyle);
            e.ChangedRange.ClearStyle(wrongStyle);

            // 设置颜色样式
            // 匹配双引号内字符串
            e.ChangedRange.SetStyle(stringStyle, "\"[^\"]*\"");
            // 匹配数字
            e.ChangedRange.SetStyle(numberStyle, @"[0-9]+");

            // 清除缩进
            e.ChangedRange.ClearFoldingMarkers();

            // 设置缩进
            e.ChangedRange.SetFoldingMarkers("{", "}");
        }

        private void fctbJson_AutoIndentNeeded(object sender, AutoIndentEventArgs e)
        {
            /* 标准Json格式
             * 
             * {
             *     "criteria":{
             *         "a":{
             *             "trigger": "minecraft:tick"
             *         }
             *     },
             *     "rewards":{
             *         "function": "system:start"
             *     }
             * }
             * 
             */
            if (Regex.IsMatch(e.LineText, @"[}\]],?[{\[]?$"))
            {
                e.Shift = -e.TabLength;
                e.ShiftNextLines = -e.TabLength;
                return;
            }

            if (Regex.IsMatch(e.LineText, @"[{\[]$"))
            {
                e.ShiftNextLines = e.TabLength;
                return;
            }
        }

        public void UpdateContent()
        {
            //fctbJson.Text = SerializeObjectToJson(MainForm.GetInstance().EditedDataClass);
        }
    }
}
