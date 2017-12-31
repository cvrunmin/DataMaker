using DataMaker.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DataMaker
{
    public struct Theme
    {
        public static Color PressedColor = Color.FromArgb(27, 27, 28);
        public static Color BackColor = Color.FromArgb(37, 37, 37);
        public static Color ForeColor = Color.FromArgb(245, 245, 245);
        public static Color HoverColor = Color.FromArgb(51, 51, 52);
        public static Font Font = new Font("微软雅黑", 10f, GraphicsUnit.Point);

        public static void Initialize(ContainerControl container)
        {
            container.BackColor = BackColor;
            container.ForeColor = ForeColor;
            container.Font = Font;
            foreach (Control control in container.Controls)
            {
                if (control is ContainerControl)
                {
                    Initialize(control as ContainerControl);
                }
                else
                {
                    control.BackColor = BackColor;
                    control.ForeColor = ForeColor;
                    control.Font = Font;
                }
            }
        }
    }

    public partial class Main : Form
    {
        #region 公共函数
        /// <summary>
        /// 依据指定key从资源文件读取文字
        /// </summary>
        /// <param name="key">指定key</param>
        public static string Lang(string key)
        {

            var rm = new System.Resources.ResourceManager("DataMaker.Languages." + "zh_cn", typeof(Resources).Assembly);

            if (rm.GetString(key) != null)
            {
                return rm.GetString(key);
            }

            throw new ApplicationException("No Lang: " + key);
        }
        #endregion

        #region 设置主题样式
        private void SetTheme()
        {
            Theme.Initialize(this);
            menuTop.Renderer = new ThemeRenderer();

            foreach (ToolStripItem i in menuTop.Items)
            {
                i.BackColor = BackColor;
                i.ForeColor = ForeColor;
            }
            foreach (ToolStripItem i in smnuDataPack.DropDownItems)
            {
                i.BackColor = BackColor;
                i.ForeColor = ForeColor;
            }
            foreach (ToolStripItem i in smnuHelp.DropDownItems)
            {
                i.BackColor = BackColor;
                i.ForeColor = ForeColor;
            }
        }

        private class ThemeRenderer : ToolStripProfessionalRenderer
        {
            public ThemeRenderer() : base(new ProfessionalColorTable()) { }

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

        /// <summary>
        /// 重载OnLoad 去边框
        /// FIXME: 必须要最大化时启动才有效…
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            // https://stackoverflow.com/questions/7752696/how-to-remove-3d-border-sunken-from-mdiclient-component-in-mdi-parent-form
            var mdiclient = Controls.OfType<MdiClient>().Single();
            SuspendLayout();
            mdiclient.SuspendLayout();
            var hdiff = mdiclient.Size.Width - mdiclient.ClientSize.Width;
            var vdiff = mdiclient.Size.Height - mdiclient.ClientSize.Height;
            var size = new Size(mdiclient.Width + hdiff, mdiclient.Height + vdiff);
            var location = new Point(mdiclient.Left - (hdiff / 2), mdiclient.Top - (vdiff / 2));
            mdiclient.Dock = DockStyle.None;
            mdiclient.Size = size;
            mdiclient.Location = location;
            mdiclient.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            mdiclient.ResumeLayout(true);
            ResumeLayout(true);
            base.OnLoad(e);
        }
        #endregion

        private FileTree fileTree;

        public Main()
        {
            InitializeComponent();
            SetTheme();

            // 显示文件树
            fileTree = new FileTree()
            {
                MdiParent = this,
                Dock = DockStyle.Right
            };

            fileTree.Show();
        }

        #region 事件处理
        private void RespondShortcutKeys(object sender, KeyEventArgs e)
        {
            if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        e.SuppressKeyPress = true;
                        ExitApplication();
                        break;
                    default:
                        break;
                }
            }
            else if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.D:
                        e.SuppressKeyPress = true;

                        break;
                    case Keys.E:
                        e.SuppressKeyPress = true;

                        break;
                    case Keys.L:
                        e.SuppressKeyPress = true;
                        SelectDatapackFolder();
                        break;
                    case Keys.S:
                        e.SuppressKeyPress = true;

                        break;
                    default:
                        break;
                }
            }
        }

        private void ShowAboutBox()
        {
            new AboutBox().Show();
        }

        private static void ExitApplication()
        {
            Application.Exit();
        }

        private void SelectDatapackFolder()
        {
            var result = folderBrowser.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                fileTree.Datapack = folderBrowser.SelectedPath;
            }
        }

        #region 响应事件
        private void smnuExit_Click(object sender, EventArgs e) => ExitApplication();
        private void smnuAbout_Click(object sender, EventArgs e) => ShowAboutBox();
        private void smnuLoadFolder_Click(object sender, EventArgs e) => SelectDatapackFolder();
        #endregion
        #endregion

    }
}
