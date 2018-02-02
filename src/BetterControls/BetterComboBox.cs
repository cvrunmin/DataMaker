using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DataMaker.BetterControls
{
    public partial class BetterComboBox : UserControl
    {
        public new event EventHandler TextChanged;

        public ComboBox.ObjectCollection Items => comboBoxContent.Items;
        public List<string> AllItems
        {
            get => allItems;
            set => allItems = value;
        }
        public int SelectedIndex
        {
            get => comboBoxContent.SelectedIndex;
            set => comboBoxContent.SelectedIndex = value;
        }
        public override string Text
        {
            get => comboBoxContent.Text;
            set => comboBoxContent.Text = value;
        }
        public ComboBoxStyle DropDownStyle
        {
            get => comboBoxContent.DropDownStyle;
            set => comboBoxContent.DropDownStyle = value;
        }

        public BetterComboBox()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);
        }

        private List<string> allItems = new List<string>();

        private void comboBoxContent_TextChanged(object sender, EventArgs e)
        {
            // 重新计时
            // 不着急立刻match，不然会卡
            timerMatch.Enabled = false;
            timerMatch.Enabled = true;
            
            // 触发事件
            TextChanged(this, new EventArgs());
        }

        private void MatchPattern()
        {
            try
            {
                // 存储所有Item
                if (allItems == null)
                {
                    allItems.AddRange(comboBoxContent.Items.Cast<string>());
                }

                // 筛选合格的到result
                var result = new List<string>();
                foreach (var i in allItems)
                    if (Regex.IsMatch(
                        i.ToString(),
                        $".*{comboBoxContent.Text}.*",
                        RegexOptions.IgnoreCase |
                        RegexOptions.IgnorePatternWhitespace)
                        )
                        result.Add(i.ToString());

                // 把合格的Items显示
                comboBoxContent.Items.Clear();
                comboBoxContent.Items.AddRange(result.ToArray());
            }
            catch (ArgumentException)
            {
                // 没毛病，用户的正则表达式没输完而已
            }
        }
        
        private void timerMatch_Tick(object sender, EventArgs e)
        {
            // 记录光标位置
            var selectionStart = comboBoxContent.SelectionStart;
            
            // 匹配
            MatchPattern();

            // 把光标的位置复原
            comboBoxContent.SelectionStart = selectionStart;

            // 停止计时
            timerMatch.Enabled = false;
        }

        private void BetterComboBox_Resize(object sender, EventArgs e)
        {
            comboBoxContent.Height = Height;
            comboBoxContent.Width = Width;
        }
    }
}
