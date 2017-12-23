using System.Drawing;
using System.Windows.Forms;

namespace DataMaker
{
    public struct Theme
    {
        public static Color BackColor = Color.FromArgb(37, 37, 37);
        public static Color ForeColor = Color.FromArgb(245, 245, 245);

        public static void Initialize(Form form)
        {
            form.BackColor = BackColor;
            form.ForeColor = ForeColor;
            foreach (Control control in form.Controls)
            {
                control.BackColor = BackColor;
                control.ForeColor = ForeColor;
            }
        }
    }

    public partial class Main : Form
    {
        public static string Lang(string key)
        {
            return key;
        }

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
