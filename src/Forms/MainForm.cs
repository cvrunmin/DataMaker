using DataMaker.BetterControls;
using DataMaker.Forms;
using DataMaker.Properties;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

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

        /// <summary>
        /// 将指定对象序列化为Json文本
        /// </summary>
        /// <param name="obj">指定对象</param>
        /// <returns></returns>
        public static string SerializeToJson(object obj) => JsonConvert.SerializeObject(obj, Formatting.Indented);
        #endregion

        #region 设置主题样式
        private void SetTheme()
        {
            Theme.Initialize(this);
            menuTop.Renderer = new BetterMenuStripRenderer();

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

        public Form WorkSpace;

        private FileTree fileTree;
        private PropertyEditor propertyEditor;

        public MainForm()
        {
            InitializeComponent();
            SetTheme();

            // 显示文件树
            fileTree = new FileTree()
            {
                MdiParent = this
            };
            fileTree.Show();
            fileTree.Resize += Form_Resize;

            // 显示属性列表
            propertyEditor = new PropertyEditor()
            {
                MdiParent = this
            };
            propertyEditor.Show();
            propertyEditor.Resize += Form_Resize;

            // 显示工作区
            WorkSpace = new JsonEditor()
            {
                MdiParent = this
            };
            WorkSpace.Show();

            // 手动排列窗体
            LayoutForms();
        }

        #region 事件处理
        private void SetSmnus()
        {
            smnuExportZip.Enabled = true;
            if (fileTree.DatapackPath == null || fileTree.DatapackPath == "")
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
                fileTree.DatapackPath = folderBrowser.SelectedPath;
        }

        private void ExportZip()
        {
            if (smnuExportZip.Enabled)
            {
                zipSaver.Filter = Lang("global_datapack") + " | *.zip";
                zipSaver.FileName = "unnamed.zip";

                var result = zipSaver.ShowDialog();
                var name = zipSaver.FileName;
                if (result != DialogResult.Cancel)
                {
                    // 已存在 删除
                    if (File.Exists(name))
                    {
                        File.Delete(name);
                    }

                    // 开始压缩
                    ZipFile.CreateFromDirectory(fileTree.DatapackPath, name);

                    // 通知
                    MessageBox.Show(Lang("formmain_msgbox_exportzipsuccessfully").Replace("{0}", name));
                }
            }
        }

        private void LayoutForms()
        {
            if (fileTree != null)
            {
                fileTree.Left = ClientSize.Width - fileTree.ClientSize.Width;
                fileTree.Top = 0;
                fileTree.Height = ClientSize.Height - menuTop.Height;

                propertyEditor.Left = 0;
                propertyEditor.Top = 0;
                propertyEditor.Height = fileTree.Height;

                WorkSpace.Width = ClientSize.Width - fileTree.Width - propertyEditor.Width;
                WorkSpace.Left = propertyEditor.Width;
                WorkSpace.Top = 0;
                WorkSpace.Height = fileTree.Height;
            }
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
