using DataMaker.BetterControls;
using DataMaker.DataClasses;
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
    public partial class MainForm : Form
    {
        #region 设置主题样式
        private void SetTheme()
        {
            DarkTheme.Initialize(this);
            menuTop.Renderer = new BetterMenuStripRenderer();
            lblFuckGdi.BackColor = DarkTheme.HoverColor;

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

        private DataClass editedDataClass;
        private TreeNode editedNode;
        private bool isChanged;
        private string title;

        public DataClass EditedDataClass
        {
            get => editedDataClass;
            set
            {
                editedDataClass = value;
                PropertyEditor.GetInstance().SelectObject();
            }
        }
        public TreeNode EditedNode
        {
            get => editedNode;
            set
            {
                editedNode = value;
                title = EditedNode.Text + " - " + Application.CompanyName + " " + Application.ProductName;
            }
        }
        public bool IsChanged
        {
            get => isChanged;
            set
            {
                isChanged = value;
                if (IsChanged) Text = title + "*";
                else Text = title;
            }
        }

        private MainForm()
        {
            InitializeComponent();
            SetTheme();

            // 设置标题
            title = Application.CompanyName + " " + Application.ProductName;
            IsChanged = false;

            // 显示文件树
            FileTree.GetInstance().MdiParent = this;
            FileTree.GetInstance().Show();
            FileTree.GetInstance().Resize += Form_Resize;

            // 显示属性区
            PropertyEditor.GetInstance().MdiParent = this;
            PropertyEditor.GetInstance().Show();
            PropertyEditor.GetInstance().Resize += Form_Resize;

            LayoutForms();
        }

        #region 单例模式
        private static MainForm mainForm;

        /// <summary>
        /// 获取 <see cref="MainForm"/> 的唯一实例
        /// </summary>
        public static MainForm GetInstance()
        {
            if (mainForm == null)
                mainForm = new MainForm();

            return mainForm;
        }
        #endregion

        public void EditNode(TreeNode node)
        {
            if (IsChanged)
            {
                var result = ShowMessagebox(
                    Lang("mainform_msgbox_savechangedornot"), 
                    MessageBoxButtons.YesNoCancel,
                    EditedNode.Text);

                switch (result)
                {
                    case DialogResult.Yes:
                        // 保存
                        SaveFile();
                        break;
                    case DialogResult.Cancel:
                        // 取消读取
                        return;
                    default:
                        break;
                }
            }

            // 关闭原有，编辑新的
            EditedNode = node;
            EditedDataClass = FileTree.GetDataClass(node);
        }

        #region 事件处理
        private void SetSmnus()
        {
            smnuExportZip.Enabled = true;
            if (FileTree.GetInstance().DatapackPath == null || FileTree.GetInstance().DatapackPath == "")
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
                        SaveFile();
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
                FileTree.GetInstance().DatapackPath = folderBrowser.SelectedPath;
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
                        ShowMessagebox(
                            Lang("mainform_msgbox_notalegalname"), 
                            Path.GetFileNameWithoutExtension(name));

                        goto selectFile;
                    }

                    // 已存在 删除
                    if (File.Exists(name))
                        File.Delete(name);

                    // 开始压缩
                    ZipFile.CreateFromDirectory(FileTree.GetInstance().DatapackPath, name);

                    // 通知
                    ShowMessagebox(
                        Lang("mainform_msgbox_exportzipsuccessfully"), 
                        name);
                }
            }
        }

        private void LayoutForms()
        {
            FileTree.GetInstance().Left = ClientSize.Width - FileTree.GetInstance().ClientSize.Width;
            FileTree.GetInstance().Top = PropertyEditor.GetInstance().Top = 0;

            FileTree.GetInstance().Height = PropertyEditor.GetInstance().Height = ClientSize.Height - menuTop.Height - 5;

            PropertyEditor.GetInstance().Left = 0;
        }

        private void SaveFile()
        {
            FileTree.SaveFile(EditedDataClass, EditedNode);
            IsChanged = false;
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

        private void smnuSaveFile_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
    }
}
