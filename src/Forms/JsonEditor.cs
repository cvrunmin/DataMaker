using FastColoredTextBoxNS;
using System.Collections;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DataMaker.Forms
{
    public partial class JsonEditor : Form, IEditor
    {
        public JsonEditor()
        {
            InitializeComponent();
            
            SetTheme();
        }

        private void SetTheme()
        {
            Theme.Initialize(this);

            fctbJson.BackColor = Theme.PressedColor;
            fctbJson.ServiceColors.CollapseMarkerBackColor = Theme.BackColor;
            fctbJson.ServiceColors.CollapseMarkerBorderColor = Theme.ForeColor;
            fctbJson.ServiceColors.CollapseMarkerForeColor = Theme.ForeColor;
            fctbJson.ServiceColors.ExpandMarkerBackColor = Theme.BackColor;
            fctbJson.ServiceColors.ExpandMarkerBorderColor = Theme.ForeColor;
            fctbJson.ServiceColors.ExpandMarkerBorderColor = Theme.ForeColor;
            fctbJson.IndentBackColor = Theme.BackColor;
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
            var beginPattern = @".*{$";
            var endPattern = @"^\s*}.*";

            if (Regex.IsMatch(e.LineText, beginPattern))
            {
                e.ShiftNextLines = e.TabLength;
                return;
            }

            if (Regex.IsMatch(e.LineText, endPattern))
            {
                e.Shift = -e.TabLength;
                e.ShiftNextLines = -e.TabLength;
                return;
            }
        }
    }
}
