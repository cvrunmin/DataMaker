using System.Drawing;
using System.Windows.Forms;

namespace DataMaker.BetterControls
{
    class BetterMenuStripRenderer : ToolStripProfessionalRenderer
    {
        public BetterMenuStripRenderer() : base(new ProfessionalColorTable()) { }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            base.OnRenderSeparator(e);

            var back = new SolidBrush(Theme.BackColor);
            var line = new SolidBrush(Theme.ForeColor);
            var rectBack = new Rectangle(0, 0, e.Item.Width, e.Item.Height);
            var rectLine = new Rectangle(32, 3, e.Item.Width - 32, 1);
            e.Graphics.FillRectangle(back, rectBack);
            e.Graphics.FillRectangle(line, rectLine);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            var rc = new Rectangle(Point.Empty, e.Item.Size);
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Theme.HoverColor), rc);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(Theme.BackColor), rc);
            }
        }

    }
}
