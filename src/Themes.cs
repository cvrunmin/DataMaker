using DataMaker.Parsers;
using System.Drawing;
using System.Windows.Forms;

namespace DataMaker
{
    public class DarkTheme
    {
        public static Color PressedColor = Color.FromArgb(27, 27, 28);
        public static Color BackColor = Color.FromArgb(37, 37, 37);
        public static Color ForeColor = Color.FromArgb(245, 245, 245);
        public static Color HoverColor = Color.FromArgb(51, 51, 52);
        public static Font Font = new Font("微软雅黑", 12f, GraphicsUnit.Point);

        public static void Initialize(Control container)
        {
            container.BackColor = BackColor;
            container.ForeColor = ForeColor;
            container.Font = Font;
            foreach (Control control in container.Controls)
            {
                Initialize(control);
                control.BackColor = BackColor;
                control.ForeColor = ForeColor;
                control.Font = Font;
            }
        }
    }
}