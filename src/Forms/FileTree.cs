using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DataMaker
{
    public partial class FileTree : Form
    {
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
            tvwFiles.ImageList.Images.Add("Advancement", (Image)rm.GetObject("Advancement"));
            tvwFiles.ImageList.Images.Add("Directory", (Image)rm.GetObject("Directory"));
            tvwFiles.ImageList.Images.Add("Function", (Image)rm.GetObject("Function"));
            tvwFiles.ImageList.Images.Add("Json", (Image)rm.GetObject("Json"));
            tvwFiles.ImageList.Images.Add("LootTable", (Image)rm.GetObject("LootTable"));
            tvwFiles.ImageList.Images.Add("Picture", (Image)rm.GetObject("Picture"));
            tvwFiles.ImageList.Images.Add("Recipe", (Image)rm.GetObject("Recipe"));
            tvwFiles.ImageList.Images.Add("Setting", (Image)rm.GetObject("Setting"));
            tvwFiles.ImageList.Images.Add("Structure", (Image)rm.GetObject("Structure"));
            tvwFiles.ImageList.Images.Add("Namespace", (Image)rm.GetObject("Namespace"));
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
        /// 加载指定目录的文件与目录到 <see cref="fileTree"/>
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
                if (node.Text.Contains(".json"))
                    node.ImageKey = node.SelectedImageKey = "Json";
                else if (node.Text.Contains(".mcfunction"))
                    node.ImageKey = node.SelectedImageKey = "Function";
            }
            else
            {
                switch (layers)
                {
                    case 0:
                        // 根目录
                        node.ImageKey = node.SelectedImageKey = "Directory";
                        node.Text = "数据包";
                        node.Expand();
                        break;
                    case 1:
                        // 数据
                        // TODO: 数据包、数据的图标
                        node.ImageKey = node.SelectedImageKey = "Directory";
                        node.Text = "数据";
                        node.Expand();
                        break;
                    case 2:
                        // 命名空间
                        node.ImageKey = node.SelectedImageKey = "Namespace";
                        node.Expand();
                        break;
                    case 3:
                        // 分类
                        switch (node.Text)
                        {
                            case "advancements":
                                node.ImageKey = node.SelectedImageKey = "Advancement";
                                node.Text = "进度";
                                node.Expand();
                                break;
                            case "functions":
                                node.ImageKey = node.SelectedImageKey = "Function";
                                node.Text = "函数";
                                node.Expand();
                                break;
                            case "loot_tables":
                                node.ImageKey = node.SelectedImageKey = "LootTable";
                                node.Text = "战利品表";
                                node.Expand();
                                break;
                            case "structures":
                                node.ImageKey = node.SelectedImageKey = "Structure";
                                node.Text = "结构";
                                node.Expand();
                                break;
                            case "recipes":
                                node.ImageKey = node.SelectedImageKey = "Recipe";
                                node.Text = "配方";
                                node.Expand();
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        // 普通文件夹
                        node.ImageKey = node.SelectedImageKey = "Directory";
                        break;
                }
            }
        }
    }
}
