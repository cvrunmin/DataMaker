using System.Drawing;
using System.Windows.Forms;

namespace DataMaker.BetterControls
{
    class BetterToolStripSeparator : ToolStripSeparator
    {
        public BetterToolStripSeparator()
        {
            Paint += BetterToolStripSeparator_Paint;
        }

        private void BetterToolStripSeparator_Paint(object sender, PaintEventArgs e)
        {
            ToolStripSeparator toolStripSeparator = (ToolStripSeparator)sender;
            int width = toolStripSeparator.Width;
            int height = toolStripSeparator.Height;

            Color foreColor = toolStripSeparator.ForeColor;
            Color backColor = toolStripSeparator.BackColor;

            e.Graphics.FillRectangle(new SolidBrush(backColor), 0, 0, width, height);
            e.Graphics.DrawLine(new Pen(foreColor), 4, height / 2, width - 4, height / 2);
        }

        ~BetterToolStripSeparator()
        {
            Paint -= BetterToolStripSeparator_Paint;
        }
    }
}
