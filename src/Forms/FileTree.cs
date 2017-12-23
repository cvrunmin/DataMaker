using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static DataMaker.Main;

namespace DataMaker
{
    public enum NodeType
    {
        DataPack,
        Data,
        Namespace,
        AdvancementsRoot,
        Advancements,
        FunctionsRoot,
        Functions,
        LootTablesRoot,
        LootTables,
        RecipesRoot,
        Recipes,
        StructuresRoot,
        Structures,
        TagsRoot,
        Tags,
        Directory,
        File
    }

    public partial class FileTree : Form
    {
        /// <summary>
        /// 右键选中的节点
        /// </summary>
        private string packPath;

        /// <summary>
        /// 通过节点获取该节点对应的文件路径
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <returns>文件路径</returns>
        private string GetPathFromNode(TreeNode node)
        {
            var result = node.FullPath;

            result = result.Replace(Lang("数据包") + "/", packPath + "/");
            result = result.Replace("/" + Lang("数据"), "/data");
            result = result.Replace("/" + Lang("进度"), "/advancements");
            result = result.Replace("/" + Lang("函数"), "/functions");
            result = result.Replace("/" + Lang("结构"), "/structures");
            result = result.Replace("/" + Lang("标签"), "/tags");
            result = result.Replace("/" + Lang("战利品表"), "/loot_tables");
            result = result.Replace(Lang("/配方/"), "/recipes/");
            result = result.Replace(Lang("/设置"), "/pack.mcmeta");
            result = result.Replace(Lang("/"), "\\");

            return result;
        }

        #region 启动时初始化
        public FileTree()
        {
            InitializeComponent();
            Theme.Initialize(this);

            LoadImages();
            packPath = $"{Environment.CurrentDirectory}/我是正经数据包";
            LoadFileTree(packPath);
        }

        private void LoadImages()
        {
            var rm = new System.Resources.ResourceManager("DataMaker.GlobalResource", GetType().Assembly);
            tvwFiles.ImageList = new ImageList();
            tvwFiles.ImageList.Images.Add("Misc", (Image)rm.GetObject("Misc"));
            tvwFiles.ImageList.Images.Add("Directory", (Image)rm.GetObject("Directory"));
            tvwFiles.ImageList.Images.Add("Function", (Image)rm.GetObject("Function"));
            tvwFiles.ImageList.Images.Add("Json", (Image)rm.GetObject("Json"));
            tvwFiles.ImageList.Images.Add("Picture", (Image)rm.GetObject("Picture"));
            tvwFiles.ImageList.Images.Add("Setting", (Image)rm.GetObject("Setting"));
            tvwFiles.ImageList.Images.Add("Namespace", (Image)rm.GetObject("Namespace"));
            tvwFiles.ImageList.Images.Add("Advancements", (Image)rm.GetObject("Advancements"));
            tvwFiles.ImageList.Images.Add("Functions", (Image)rm.GetObject("Functions"));
            tvwFiles.ImageList.Images.Add("LootTables", (Image)rm.GetObject("LootTables"));
            tvwFiles.ImageList.Images.Add("Recipes", (Image)rm.GetObject("Recipes"));
            tvwFiles.ImageList.Images.Add("Structures", (Image)rm.GetObject("Structures"));
            tvwFiles.ImageList.Images.Add("Tags", (Image)rm.GetObject("Tags"));
        }

        /// <summary>
        /// 将指定目录所有文件与子文件读取到 <see cref="tvwFiles"/>
        /// </summary>
        /// <param name="path"></param>
        private void LoadFileTree(string path)
        {
            tvwFiles.Nodes.Clear();
            var rootNode = tvwFiles.Nodes.Add("dataPack");
            InitializeNode(rootNode, 0, false);
            LoadDirectory(path, rootNode, 0);
        }

        /// <summary>
        /// 加载指定目录的文件与目录到 <see cref="fileTree"/> 的指定节点
        /// </summary>
        /// <param name="path">指定目录</param>
        /// <param name="node">该目录对应的节点</param>
        /// <param name="layers">指定节点的层数</param>
        private void LoadDirectory(string path, TreeNode node, int layers)
        {
            // 读取目录
            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                var dirName = dir.LastIndexOf(@"\") + 1;
                var dirNode = node.Nodes.Add(dir.Remove(0, dirName));
                InitializeNode(dirNode, layers + 1, false);
                // 遍历
                LoadDirectory(dir, dirNode, layers + 1);
            }

            // 读取文件
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var fileName = file.Remove(0, file.LastIndexOf(@"\") + 1);
                var fileNode = node.Nodes.Add(fileName);
                InitializeNode(fileNode, layers + 1, true);
            }
        }

        /// <summary>
        /// 初始化指定节点的状态
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <param name="layers">指定节点的层数</param>
        /// <param name="isFile">指示是否是一个文件</param>
        private void InitializeNode(TreeNode node, int layers, bool isFile)
        {
            if (isFile)
            {
                if (node.Text.EndsWith(".json"))
                    node.ImageKey = node.SelectedImageKey = "Json";
                else if (node.Text.EndsWith(".mcfunction"))
                    node.ImageKey = node.SelectedImageKey = "Function";
                else if (node.Text.Equals("pack.mcmeta"))
                {
                    node.ImageKey = node.SelectedImageKey = "Setting";
                    node.Text = Lang("设置");
                }
                node.Tag = NodeType.File;
            }
            else
            {
                switch (layers)
                {
                    case 0:
                        // 根目录
                        node.ImageKey = node.SelectedImageKey = "Directory";
                        node.Text = Lang("数据包");
                        node.Tag = NodeType.DataPack;
                        node.Expand();
                        break;
                    case 1:
                        // TODO: 数据包、数据的图标

                        // 数据
                        if (node.Text.Equals("data"))
                        {
                            node.ImageKey = node.SelectedImageKey = "Directory";
                            node.Text = "数据";
                            node.Tag = NodeType.Data;
                            node.Expand();
                        }
                        break;
                    case 2:
                        // 命名空间
                        node.ImageKey = node.SelectedImageKey = "Namespace";
                        node.Tag = NodeType.Namespace;
                        node.Expand();
                        break;
                    case 3:
                        // 细分类
                        switch (node.Text)
                        {
                            case "advancements":
                                node.ImageKey = node.SelectedImageKey = "Advancements";
                                node.Text = Lang("进度");
                                node.Tag = NodeType.AdvancementsRoot;
                                node.Expand();
                                break;
                            case "functions":
                                node.ImageKey = node.SelectedImageKey = "Functions";
                                node.Text = Lang("函数");
                                node.Tag = NodeType.FunctionsRoot;
                                node.Expand();
                                break;
                            case "loot_tables":
                                node.ImageKey = node.SelectedImageKey = "LootTables";
                                node.Text = Lang("战利品表");
                                node.Tag = NodeType.LootTablesRoot;
                                node.Expand();
                                break;
                            case "structures":
                                node.ImageKey = node.SelectedImageKey = "Structures";
                                node.Text = Lang("结构");
                                node.Tag = NodeType.StructuresRoot;
                                node.Expand();
                                break;
                            case "recipes":
                                node.ImageKey = node.SelectedImageKey = "Recipes";
                                node.Text = Lang("配方");
                                node.Tag = NodeType.RecipesRoot;
                                node.Expand();
                                break;
                            case "tags":
                                node.ImageKey = node.SelectedImageKey = "Tags";
                                node.Text = Lang("标签");
                                node.Tag = NodeType.TagsRoot;
                                node.Expand();
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        // 其他层级 跟随父级目录的属性
                        node.ImageKey = node.SelectedImageKey = "Directory";

                        switch ((NodeType)node.Parent.Tag)
                        {
                            case NodeType.AdvancementsRoot:
                            case NodeType.Advancements:
                                node.Tag = NodeType.Advancements;
                                break;
                            case NodeType.FunctionsRoot:
                            case NodeType.Functions:
                                node.Tag = NodeType.Functions;
                                break;
                            case NodeType.LootTablesRoot:
                            case NodeType.LootTables:
                                node.Tag = NodeType.LootTables;
                                break;
                            case NodeType.RecipesRoot:
                            case NodeType.Recipes:
                                node.Tag = NodeType.Recipes;
                                break;
                            case NodeType.StructuresRoot:
                            case NodeType.Structures:
                                node.Tag = NodeType.Structures;
                                break;
                            case NodeType.TagsRoot:
                            case NodeType.Tags:
                                node.Tag = NodeType.Tags;
                                break;
                            default:
                                // 普通文件夹
                                node.Tag = NodeType.Directory;
                                break;
                        }

                        break;
                }
            }
        }
        #endregion

        #region 修改窗体大小
        private int initialX;
        private bool isResizing;

        private void lblSizeChanger_MouseDown(object sender, MouseEventArgs e)
        {
            initialX = e.X;
            isResizing = true;
        }

        private void lblSizeChanger_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                Width += initialX - e.X;
                Width = Width < MinimumSize.Width ? MinimumSize.Width : Width;
                tvwFiles.Width = Width - lblSizeChanger.Width;
            }
        }

        private void lblSizeChanger_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing = false;
        }
        #endregion

        #region 右键菜单相关
        /// <summary>
        /// 设置菜单各选项的有效性
        /// </summary>
        private void SetSmnus()
        {
            smnuCopy.Enabled = true;
            smnuCut.Enabled = true;
            smnuDelete.Enabled = true;
            smnuAdd.Enabled = true;
            smnuAddDirectory.Enabled = true;
            smnuAddFile.Enabled = true;
            smnuOpen.Enabled = true;
            smnuPaste.Enabled = true;
            smnuProperty.Enabled = true;
            smnuRename.Enabled = true;
            smnuCopy.Text = Lang("复制");
            smnuCut.Text = Lang("剪切");
            smnuDelete.Text = Lang("删除");
            smnuAdd.Text = Lang("添加");
            smnuAddDirectory.Text = Lang("文件夹");
            smnuAddFile.Text = Lang("文件");
            smnuPaste.Text = Lang("粘贴");
            smnuProperty.Text = Lang("属性");
            smnuRename.Text = Lang("重命名");
            if (tvwFiles.SelectedNode.IsExpanded)
                smnuOpen.Text = Lang("收起");
            else
                smnuOpen.Text = Lang("展开");

            switch ((NodeType)tvwFiles.SelectedNode.Tag)
            {
                case NodeType.DataPack:
                    smnuDelete.Enabled = false;
                    smnuAdd.Enabled = false;
                    smnuRename.Enabled = false;
                    break;
                case NodeType.Data:
                    smnuAddDirectory.Text = Lang("命名空间");
                    smnuDelete.Enabled = false;
                    smnuAddFile.Enabled = false;
                    smnuRename.Enabled = false;
                    break;
                case NodeType.Namespace:
                    smnuAddFile.Enabled = false;
                    break;
                case NodeType.Advancements:
                    smnuAddFile.Text = Lang("进度文件");
                    break;
                case NodeType.Functions:
                    smnuAddFile.Text = Lang("函数文件");
                    break;
                case NodeType.LootTables:
                    smnuAddFile.Text = Lang("战利品表文件");
                    break;
                case NodeType.Recipes:
                    smnuAddFile.Text = Lang("配方文件");
                    break;
                case NodeType.Structures:
                    smnuAddFile.Text = Lang("结构文件");
                    break;
                case NodeType.Tags:
                    smnuAddFile.Text = Lang("标签文件");
                    break;
                case NodeType.AdvancementsRoot:
                    smnuAddFile.Text = Lang("进度文件");
                    smnuDelete.Enabled = false;
                    smnuRename.Enabled = false;
                    break;
                case NodeType.FunctionsRoot:
                    smnuAddFile.Text = Lang("函数文件");
                    smnuDelete.Enabled = false;
                    smnuRename.Enabled = false;
                    break;
                case NodeType.LootTablesRoot:
                    smnuAddFile.Text = Lang("战利品表文件");
                    smnuDelete.Enabled = false;
                    smnuRename.Enabled = false;
                    break;
                case NodeType.RecipesRoot:
                    smnuAddFile.Text = Lang("配方文件");
                    smnuDelete.Enabled = false;
                    smnuRename.Enabled = false;
                    break;
                case NodeType.StructuresRoot:
                    smnuAddFile.Text = Lang("结构文件");
                    smnuDelete.Enabled = false;
                    smnuRename.Enabled = false;
                    break;
                case NodeType.TagsRoot:
                    smnuAddFile.Text = Lang("标签文件");
                    smnuDelete.Enabled = false;
                    smnuRename.Enabled = false;
                    break;
                case NodeType.Directory:
                    break;
                case NodeType.File:
                    smnuOpen.Text = Lang("编辑");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 菜单快捷键
        /// </summary>
        private void FileTree_KeyDown(object sender, KeyEventArgs e)
        {
            SetSmnus();
            switch (e.KeyCode)
            {
                case Keys.C:
                    if (e.Control)
                    {

                    }
                    break;
                case Keys.F1:
                    SeeProperty();
                    break;
                case Keys.F2:
                    RenameNode();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 显示右键菜单
        /// </summary>
        private void tvwFiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tvwFiles.SelectedNode = e.Node;

            if (e.Button == MouseButtons.Right)
            {
                SetSmnus();
                cmnuItem.Show(MousePosition);
            }
        }

        private void smnuOpen_Click(object sender, EventArgs e)
        {
            OpenNode();
        }

        private void OpenNode()
        {
            if (smnuOpen.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    if (tvwFiles.SelectedNode.IsExpanded)
                        tvwFiles.SelectedNode.Collapse(true);
                    else
                        tvwFiles.SelectedNode.Expand();
                }
            }
        }
        
        private void smnuRename_Click(object sender, EventArgs e)
        {
            RenameNode();
        }

        private void RenameNode()
        {
            if (smnuRename.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    tvwFiles.SelectedNode.BeginEdit();
                }
            }
        }

        private void tvwFiles_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                var pattern = @"[^(a-z0-9\-_\.)]";

                if (Regex.IsMatch(e.Label, pattern))
                {
                    MessageBox.Show(Lang("文件名只能包含 abcdefghijklmnopqrstuvwxyz0123456789-_. "));
                    e.CancelEdit = true;
                }
            }
        }

        private void smnuProperty_Click(object sender, EventArgs e)
        {
            SeeProperty();
        }

        private void SeeProperty()
        {
            if (smnuProperty.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    MessageBox.Show(
                        Lang("名称") + ": " + tvwFiles.SelectedNode.Text + "\n" +
                        Lang("类型") + ": " + tvwFiles.SelectedNode.Tag + "\n" +
                        Lang("路径") + ": " + GetPathFromNode(tvwFiles.SelectedNode) + "\n"
                        );
                }
            }
        }
        #endregion
    }
}