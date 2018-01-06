using DataMaker.BetterControls;
using DataMaker.Forms;
using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using static DataMaker.Tools;

namespace DataMaker
{
    public struct Theme
    {
        public static Color PressedColor = Color.FromArgb(27, 27, 28);
        public static SolidBrush PressedBrush = new SolidBrush(PressedColor);
        public static Color BackColor = Color.FromArgb(37, 37, 37);
        public static SolidBrush BackBrush = new SolidBrush(BackColor);
        public static Color ForeColor = Color.FromArgb(245, 245, 245);
        public static SolidBrush ForeBrush = new SolidBrush(ForeColor);
        public static Color HoverColor = Color.FromArgb(51, 51, 52);
        public static SolidBrush HoverBrush = new SolidBrush(HoverColor);
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

    public partial class MainForm : Form
    {
        #region 设置主题样式
        private void SetTheme()
        {
            Theme.Initialize(this);
            menuTop.Renderer = new BetterMenuStripRenderer();
            lblFuckGdi.BackColor = Theme.HoverColor;

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

        private RawEditor rawEditor;
        private FileTree fileTree;
        private PropertyEditor propertyEditor;

        public RawEditor RawEditor
        {
            get
            {
                if (rawEditor == null)
                {
                    rawEditor = new RawEditor()
                    {
                        MdiParent = this
                    };
                }

                return rawEditor;
            }
        }

        public FileTree FileTree
        {
            get
            {
                if (fileTree == null)
                {
                    fileTree = new FileTree()
                    {
                        MdiParent = this
                    };
                }

                return fileTree;
            }
        }

        public PropertyEditor PropertyEditor
        {
            get
            {
                if (propertyEditor == null)
                {
                    propertyEditor = new PropertyEditor()
                    {
                        MdiParent = this
                    };
                }

                return propertyEditor;
            }
        }

        private MainForm()
        {
            InitializeComponent();
            SetTheme();

            // 显示文件树
            FileTree.Show();
            FileTree.Resize += Form_Resize;

            // 显示属性区
            PropertyEditor.Show();
            PropertyEditor.Resize += Form_Resize;

            // 显示原文本编辑器
            RawEditor.Show();

            LayoutForms();
        }

        private static MainForm mainForm;

        public static MainForm GetInstance()
        {
            if (mainForm == null)
                mainForm = new MainForm();

            return mainForm;
        }

        public void EditNode(TreeNode node)
        {
            PropertyEditor.SelectObject(GetDataClass(node));
            RawEditor.EditObject(GetDataClass(node));
        }

        #region 事件处理
        private void SetSmnus()
        {
            smnuExportZip.Enabled = true;
            if (FileTree.DatapackPath == null || FileTree.DatapackPath == "")
                smnuExportZip.Enabled = false;
        }

        private void RespondShortcutKeys(object sender, KeyEventArgs e)
        {
            // 别跟你的儿子们抢按键
            e.Handled = false;

            SetSmnus();

            if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
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
                        // TODO: Unzip.
                        break;
                    case Keys.E:
                        ExportZip();
                        break;
                    case Keys.L:
                        SelectDatapackFolder();
                        break;
                    case Keys.S:

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
                FileTree.DatapackPath = folderBrowser.SelectedPath;
        }

        private void ExportZip()
        {
            if (smnuExportZip.Enabled)
            {
                selectFile:
                zipSaver.Filter = Lang("global_datapack") + " | *.zip";
                zipSaver.FileName = Path.GetFileName(zipSaver.FileName);

                var result = zipSaver.ShowDialog();
                var name = zipSaver.FileName;
                if (result != DialogResult.Cancel)
                {
                    if (!IsNameLegal(Path.GetFileNameWithoutExtension(name)))
                    {
                        // 数据包名不合法
                        MessageBox.Show(Lang("formmain_msgbox_notalegalname")
                            .Replace("{0}", Path.GetFileNameWithoutExtension(name)));

                        goto selectFile;
                    }

                    // 已存在 删除
                    if (File.Exists(name))
                        File.Delete(name);

                    // 开始压缩
                    ZipFile.CreateFromDirectory(FileTree.DatapackPath, name);

                    // 通知
                    MessageBox.Show(Lang("formmain_msgbox_exportzipsuccessfully").Replace("{0}", name));
                }
            }
        }

        private void LayoutForms()
        {
            FileTree.Left = ClientSize.Width - FileTree.ClientSize.Width;
            FileTree.Top = PropertyEditor.Top = RawEditor.Top = 0;

            FileTree.Height = PropertyEditor.Height = RawEditor.Height = ClientSize.Height - menuTop.Height - 5;

            PropertyEditor.Left = 0;
            RawEditor.Left = PropertyEditor.Width;

            RawEditor.Width = ClientSize.Width - FileTree.Width - PropertyEditor.Width;
        }

        #region 响应事件
        private void smnuDataPack_DropDownOpening(object sender, EventArgs e) => SetSmnus();
        private void smnuExit_Click(object sender, EventArgs e) => ExitApplication();
        private void smnuAbout_Click(object sender, EventArgs e) => ShowAboutBox();
        private void smnuLoadFolder_Click(object sender, EventArgs e) => SelectDatapackFolder();
        private void smnuExportZip_Click(object sender, EventArgs e) => ExportZip();

        private void MainForm_Load(object sender, EventArgs e) => LayoutForms();
        private void Form_Resize(object sender, EventArgs e) => LayoutForms();
        #endregion

        #endregion
    }
}
