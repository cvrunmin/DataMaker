using System.Drawing;
using System.Windows.Forms;

namespace DataMaker
{
    public struct Theme
    {
        public static Color BackColor = Color.DarkGray;

        public static void Initialize(Form form)
        {
            foreach (Control control in form.Controls)
            {
                control.BackColor = Theme.BackColor;
            }
        }
    }

    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Theme.Initialize(this);

            // 显示文件树
            new FileTree() { MdiParent = this, Dock = DockStyle.Right }.Show();
        }

        private void Main_Load(object sender, System.EventArgs e)
        {

        }
    }
}
