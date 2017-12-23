using System;
using System.Drawing;
using System.IO;
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
        /// 右击选中的节点
        /// </summary>
        private TreeNode rightClickedNode = null;

        public FileTree()
        {
            InitializeComponent();
            Theme.Initialize(this);

            LoadImages();
            LoadFileTree($"{Environment.CurrentDirectory}/我是正经数据包");
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
        /// 将指定目录所有文件读取到 <see cref="tvwFiles"/>
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
                    node.Text = GetText("设置");
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
                        node.Text = GetText("数据包");
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
                                node.Text = GetText("进度");
                                node.Tag = NodeType.AdvancementsRoot;
                                node.Expand();
                                break;
                            case "functions":
                                node.ImageKey = node.SelectedImageKey = "Functions";
                                node.Text = GetText("函数");
                                node.Tag = NodeType.FunctionsRoot;
                                node.Expand();
                                break;
                            case "loot_tables":
                                node.ImageKey = node.SelectedImageKey = "LootTables";
                                node.Text = GetText("战利品表");
                                node.Tag = NodeType.LootTablesRoot;
                                node.Expand();
                                break;
                            case "structures":
                                node.ImageKey = node.SelectedImageKey = "Structures";
                                node.Text = GetText("结构");
                                node.Tag = NodeType.StructuresRoot;
                                node.Expand();
                                break;
                            case "recipes":
                                node.ImageKey = node.SelectedImageKey = "Recipes";
                                node.Text = GetText("配方");
                                node.Tag = NodeType.RecipesRoot;
                                node.Expand();
                                break;
                            case "tags":
                                node.ImageKey = node.SelectedImageKey = "Tags";
                                node.Text = GetText("标签");
                                node.Tag = NodeType.RecipesRoot;
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

        /// <summary>
        /// 初始化菜单各选项的有效性
        /// </summary>
        private void InitializeSmnus()
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
            smnuCopy.Text = GetText("复制");
            smnuCut.Text = GetText("剪切");
            smnuDelete.Text = GetText("删除");
            smnuAdd.Text = GetText("添加");
            smnuAddDirectory.Text = GetText("文件夹");
            smnuAddFile.Text = GetText("文件");
            smnuPaste.Text = GetText("粘贴");
            smnuProperty.Text = GetText("属性");
            smnuRename.Text = GetText("重命名");
            if (rightClickedNode.IsExpanded)
                smnuOpen.Text = GetText("收起");
            else
                smnuOpen.Text = GetText("展开");
        }

        /// <summary>
        /// 显示右键菜单
        /// </summary>
        private void tvwFiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                rightClickedNode = e.Node;

                InitializeSmnus();

                switch ((NodeType)rightClickedNode.Tag)
                {
                    case NodeType.DataPack:
                        smnuDelete.Enabled = false;
                        smnuAdd.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case NodeType.Data:
                        smnuAddDirectory.Text = GetText("命名空间");
                        smnuDelete.Enabled = false;
                        smnuAddFile.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case NodeType.Namespace:
                        smnuAddFile.Enabled = false;
                        break;
                    case NodeType.Advancements:
                        smnuAddFile.Text = GetText("进度文件");
                        break;
                    case NodeType.Functions:
                        smnuAddFile.Text = GetText("函数文件");
                        break;
                    case NodeType.LootTables:
                        smnuAddFile.Text = GetText("战利品表文件");
                        break;
                    case NodeType.Recipes:
                        smnuAddFile.Text = GetText("配方文件");
                        break;
                    case NodeType.Structures:
                        smnuAddFile.Text = GetText("结构文件");
                        break;
                    case NodeType.Tags:
                        smnuAddFile.Text = GetText("标签文件");
                        break;
                    case NodeType.AdvancementsRoot:
                        smnuAddFile.Text = GetText("进度文件");
                        smnuDelete.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case NodeType.FunctionsRoot:
                        smnuAddFile.Text = GetText("函数文件");
                        smnuDelete.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case NodeType.LootTablesRoot:
                        smnuAddFile.Text = GetText("战利品表文件");
                        smnuDelete.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case NodeType.RecipesRoot:
                        smnuAddFile.Text = GetText("配方文件");
                        smnuDelete.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case NodeType.StructuresRoot:
                        smnuAddFile.Text = GetText("结构文件");
                        smnuDelete.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case NodeType.TagsRoot:
                        smnuAddFile.Text = GetText("标签文件");
                        smnuDelete.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case NodeType.Directory:
                        break;
                    case NodeType.File:
                        smnuOpen.Text = GetText("编辑");
                        break;
                    default:
                        break;
                }

                cmnuItem.Show(MousePosition);
            }
        }

        private void smnuOpen_Click(object sender, EventArgs e)
        {
            if (rightClickedNode != null)
            {
                if (rightClickedNode.IsExpanded)
                    rightClickedNode.Collapse(true);
                else
                    rightClickedNode.Expand();
            }
        }

        private void smnuRename_Click(object sender, EventArgs e)
        {
            if (rightClickedNode != null)
            {
                rightClickedNode.Expand();
            }
        }
    }
}
