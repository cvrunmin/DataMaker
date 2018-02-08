using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DataMaker.BetterControls
{
    public partial class BetterComboBox : UserControl
    {
        public new event EventHandler TextChanged;
        private List<string> allItems = new List<string>();

        public ComboBox.ObjectCollection Items => comboBoxContent.Items;
        public List<string> AllItems => allItems;
        public int SelectionStart
        {
            get => comboBoxContent.SelectionStart;
            set => comboBoxContent.SelectionStart = value;
        }
        public int SelectionLength
        {
            get => comboBoxContent.SelectionLength;
            set => comboBoxContent.SelectionLength = value;
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
        
        private void comboBoxContent_TextUpdated(object sender, EventArgs e)
        {
            // 显示下拉框
            comboBoxContent.DroppedDown = true;
            // Fuck microsoft, for it always hide my cursor.
            // Thx to https://stackoverflow.com/questions/1093067/why-combobox-hides-cursor-when-droppeddown-is-set
            comboBoxContent.Cursor = Cursors.Default;

            // 强制小写
            var selectionStart = comboBoxContent.SelectionStart;
            var selectionLength = comboBoxContent.SelectionLength;
            comboBoxContent.Text = comboBoxContent.Text.ToLower();
            comboBoxContent.SelectionStart = selectionStart;
            comboBoxContent.SelectionLength = selectionLength;

            // 重新计时
            // 不着急立刻match，不然会卡
            timerMatch.Stop();
            timerMatch.Start();
            
            // 触发事件
            TextChanged(this, new EventArgs());
        }

        private void comboBoxContent_SelectedIndexChanged(object sender, EventArgs e)
            => TextChanged(this, new EventArgs());

        private void MatchPattern()
        {
            try
            {
                // 存储所有Item
                if (allItems.Count == 0)
                {
                    allItems.AddRange(comboBoxContent.Items.Cast<string>());
                }

                // 筛选合格的到result
                var result = new List<string>();
                // 首先筛选完整包含的
                foreach (var i in allItems)
                {
                    if (i.ToString().Contains(comboBoxContent.Text))
                    {
                        result.Add(i.ToString());
                    }
                }
                // 然后筛选包含所有字符的
                foreach (var i in allItems)
                {
                    if (!result.Contains(i) && i.ToString().ContainsAllChars(comboBoxContent.Text))
                    {
                        result.Add(i.ToString());
                    }
                }

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
            var selectionLength = comboBoxContent.SelectionLength;

            // 匹配
            MatchPattern();

            // 把光标的位置复原
            comboBoxContent.SelectionStart = selectionStart;
            comboBoxContent.SelectionLength = selectionLength;

            // 停止计时
            timerMatch.Stop();
        }

        private void BetterComboBox_Resize(object sender, EventArgs e)
        {
            comboBoxContent.Height = Height;
            comboBoxContent.Width = Width;
        }
    }
}
