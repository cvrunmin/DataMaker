using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DataMaker.BetterControls
{
    public partial class BetterComboBox : UserControl
    {
        public BetterComboBox()
        {
            InitializeComponent();
            DarkTheme.Initialize(this);
        }

        private List<string> allItems;

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            // 重新计时
            // 不着急立刻match，不然会卡
            timer1.Enabled = false;
            timer1.Enabled = true;
        }

        private void MatchPattern()
        {
            try
            {
                // 存储所有Item
                if (allItems == null)
                {
                    allItems = new List<string>();
                    allItems.AddRange(comboBox1.Items.Cast<string>());
                }

                // 筛选合格的到result
                var result = new List<string>();
                foreach (var i in allItems)
                    if (Regex.IsMatch(
                        i.ToString(),
                        $".*{comboBox1.Text}.*",
                        RegexOptions.IgnoreCase |
                        RegexOptions.IgnorePatternWhitespace)
                        )
                        result.Add(i.ToString());

                // 把合格的Items显示
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(result.ToArray());
            }
            catch (ArgumentException)
            {
                // 没毛病，用户的正则表达式没输完而已
            }
        }

        public ComboBox.ObjectCollection Items => comboBox1.Items;

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 记录光标位置
            var selectionStart = comboBox1.SelectionStart;

            // 匹配
            MatchPattern();

            // 把光标的位置复原
            comboBox1.SelectionStart = selectionStart;

            // 停止计时
            timer1.Enabled = false;
        }
    }
}
