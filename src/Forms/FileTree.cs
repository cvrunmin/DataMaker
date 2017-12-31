using DataMaker.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static DataMaker.Main;

namespace DataMaker
{
    public partial class FileTree : Form
    {
        /// <summary>
        /// 要加载的文件夹路径
        /// </summary>
        public string Datapack
        {
            get => packPath;
            set
            {
                packPath = value;
                LoadFileTree(packPath);
            }
        }

        private enum Type { DataPack, Data, Namespace, Root, Directory, File, Unknown }

        private enum Sort { Advancement, Function, LootTable, Recipe, Structure, Tag, None }
        
        private struct Item
        {
            public Type Type { get; set; }
            public Sort Sort { get; set; }
            
            public Item(Type type, Sort sort)
            {
                Type = type;
                Sort = sort;
            }

            public override string ToString()
            {
                return this;
            }

            /// <summary>
            ///  获取文件后缀名
            /// </summary>
            /// <param name="canFolder">指定是否可以是一个文件夹</param>
            public string GetFileSuffix(bool checkSubFile)
            {
                var suffix = "";

                if (!checkSubFile && Type != Type.File)
                {
                    return suffix;
                }

                switch (Sort)
                {
                    case Sort.Advancement:
                    case Sort.LootTable:
                    case Sort.Recipe:
                    case Sort.Tag:
                        suffix = ".json";
                        break;
                    case Sort.Function:
                        suffix = ".mcfunction";
                        break;
                    case Sort.Structure:
                        suffix = ".nbt";
                        break;
                }

                return suffix;
            }

            public static implicit operator string(Item item)
            {
                return $"{item.Type}|{item.Sort}";
            }

            public static explicit operator Item(TreeNode node)
            {
                if (node.Tag == null)
                {
                    return new Item(Type.Unknown, Sort.None);
                }
                var type = (Type)Enum.Parse(typeof(Type), node.Tag.ToString().Split('|')[0]);
                var sort = (Sort)Enum.Parse(typeof(Sort), node.Tag.ToString().Split('|')[1]);

                return new Item(type, sort);
            }

            public static explicit operator Item(string str)
            {
                var type = (Type)Enum.Parse(typeof(Type), str.Split('|')[0]);
                var sort = (Sort)Enum.Parse(typeof(Sort), str.Split('|')[1]);

                return new Item(type, sort);
            }
        }
        
        private string packPath;

        /// <summary>
        /// 是否正在新建文件夹
        /// </summary>
        private bool isAddingFolder;

        #region 定义函数
        /// <summary>
        /// 复制文件夹及文件夹下所有子文件夹和文件
        /// </summary>
        /// <param name="sourcePath">待复制的文件夹路径</param>
        /// <param name="destinationPath">目标路径</param>
        public static void CopyDirectory(string sourcePath, string destinationPath)
        {
            // 防止复制到自身
            if ((destinationPath + "\\").Contains(sourcePath + "\\"))
                throw new IOException("Try copying a folder to itself!");

            // 复制
            var info = new DirectoryInfo(sourcePath);
            Directory.CreateDirectory(destinationPath);

            foreach (var i in info.GetFileSystemInfos())
            {
                var destPath = destinationPath + "\\" + i.Name;

                if (i is FileInfo)
                {
                    // 是文件
                    File.Copy(i.FullName, destPath);
                }
                else
                {
                    // 是文件夹
                    Directory.CreateDirectory(destPath);
                    // 递归
                    CopyDirectory(i.FullName, destPath);
                }
            }
        }

        /// <summary>
        /// 通过节点获取该节点对应的文件路径
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <returns>文件路径</returns>
        private string GetPathFromNode(TreeNode node)
        {
            var result = node.FullPath;
            result = result.Replace(Lang("global_datapack"), packPath)
                .Replace("/" + Lang("global_data"), "/data")
                .Replace("/" + Lang("global_advancement"), "/advancements")
                .Replace("/" + Lang("global_function"), "/functions")
                .Replace("/" + Lang("global_structure"), "/structures")
                .Replace("/" + Lang("global_loottable"), "/loot_tables")
                .Replace("/" + Lang("global_recipe"), "/recipes")
                .Replace("/" + Lang("global_tag"), "/tags")
                .Replace("/" + Lang("global_block"), "/blocks")
                .Replace("/" + Lang("global_item"), "/items")
                .Replace("/" + Lang("global_function"), "/functions")
                .Replace("/" + Lang("global_settings"), "/pack.mcmeta")
                .Replace("/", "\\");
            // 加后缀
            result += ((Item)node).GetFileSuffix(false);

            return result;
        }

        /// <summary>
        /// 复制或剪切文件路径数组至剪贴板
        /// </summary>
        /// <param name="files">文件路径数组</param>
        /// <param name="cut">是否剪切</param>
        private void SetClipboardList(string[] files, bool cut)
        {
            if (files != null)
            {
                // 设置DropEffect
                IDataObject data = new DataObject(DataFormats.FileDrop, files);
                MemoryStream memo = new MemoryStream(4);
                var bytes = new byte[] { (byte)(cut ? 2 : 5), 0, 0, 0 };
                memo.Write(bytes, 0, bytes.Length);

                // 设置剪贴板存储对象
                data.SetData("Preferred DropEffect", memo);
                Clipboard.SetDataObject(data);
            }
        }

        /// <summary>
        /// 获取剪贴板中的文件路径数组
        /// </summary>
        /// <returns>剪切板中文件路径数组</returns>
        private string[] GetClipboardList(out bool isCut)
        {
            // 获取剪贴板存储对象
            IDataObject data = Clipboard.GetDataObject();

            if (!(data is null))
            {
                // 获取DropEffect
                MemoryStream ms = (MemoryStream)data.GetData("Preferred DropEffect");
                if (!(ms is null))
                {
                    // 判断DropEffect
                    var bytes = new byte[] { 0, 0, 0, 0 };
                    ms.Read(bytes, 0, bytes.Length);

                    var cutBytes = new byte[] { 2, 0, 0, 0 };

                    if (bytes[0] == cutBytes[0] &&
                        bytes[1] == cutBytes[1] &&
                        bytes[2] == cutBytes[2] &&
                        bytes[3] == cutBytes[3])
                    {
                        isCut = true;
                    }
                    else
                    {
                        isCut = false;
                    }

                    // 获取文件名列表
                    StringCollection sc = Clipboard.GetFileDropList();
                    List<string> list = new List<string>();

                    for (int i = 0; i < sc.Count; i++)
                    {
                        string listfileName = sc[i];
                        list.Add(listfileName);
                    }

                    return list.ToArray();
                }
            }

            isCut = false;
            return null;
        }

        /// <summary>
        /// 根据目录和文件名获取可用的文件名
        /// </summary>
        private string GetAvailableFileName(string path, string suffix)
        {
            var file = path.Remove(0, path.LastIndexOf("\\") + 1);
            var dir = path.Remove(path.Length - file.Length - 1, path.Length - path.LastIndexOf("\\"));

            var result = file;

            for (int i = 1; File.Exists(dir + "\\" + result + suffix); i++)
                result = file + "_" + i;

            return result + suffix;
        }

        /// <summary>
        /// 根据文件路径获取可用的文件名
        /// </summary>
        private string GetAvailableFileName(string pathDir)
        {
            // 根据完整路径取得后缀和路径
            var suffix = pathDir.Remove(0, pathDir.LastIndexOf("."));
            var path = pathDir.Remove(pathDir.LastIndexOf("."));

            return GetAvailableFileName(path, suffix);
        }

        /// <summary>
        /// 根据目录和目录名获取可用的目录名
        /// </summary>
        private string GetAvailableDirName(string path)
        {
            var subDir = path.Remove(0, path.LastIndexOf("\\") + 1);
            var dir = path.Remove(path.Length - subDir.Length - 1, path.Length - path.LastIndexOf("\\"));

            var result = subDir;

            for (int i = 1; Directory.Exists(dir + "\\" + result); i++)
                result = subDir + "_" + i;

            return result;
        }

        /// <summary>
        /// 获取指定文件名是否合法
        /// </summary>
        /// <param name="str">文件名</param>
        /// <returns></returns>
        private bool IsNameLegal(string str)
        {
            // 匹配包含非法字符的字符串
            var pattern = @"[^(a-z0-9\-_\.)]";

            if (Regex.IsMatch(str, pattern))
            {
                /* 
                 * 文件名不符合社会主义核心价值观
                 * 富强、民主、文明、和谐
                 * 自由、平等、公正、法治
                 * 爱国、敬业、诚信、友善
                 */
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取被选中的节点对应的Item
        /// </summary>
        private Item SelectedItem { get => (Item)tvwFiles.SelectedNode; }

        /// <summary>
        /// 获取指定节点对应的路径是否是文件
        /// </summary>
        /// <param name="node">指定节点</param>
        private bool IsFile(TreeNode node)
        {
            return File.Exists(GetPathFromNode(node));
        }
        #endregion

        #region 启动时初始化
        public FileTree()
        {
            InitializeComponent();
            Theme.Initialize(this);

            LoadImages();
        }

        private void LoadImages()
        {
            var rm = new System.Resources.ResourceManager("DataMaker.GlobalResource", typeof(Resources).Assembly);
            tvwFiles.ImageList = new ImageList();
            tvwFiles.ImageList.Images.Add("Misc", (Image)rm.GetObject("Misc"));
            tvwFiles.ImageList.Images.Add("Directory", (Image)rm.GetObject("Directory"));
            tvwFiles.ImageList.Images.Add("DataPack", (Image)rm.GetObject("DataPack"));
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
            tvwFiles.ImageList.Images.Add("Nbt", (Image)rm.GetObject("Nbt"));
        }

        /// <summary>
        /// 将指定目录所有文件与子文件读取到 <see cref="tvwFiles"/>
        /// </summary>
        /// <param name="path"></param>
        private void LoadFileTree(string path)
        {
            // 根目录
            tvwFiles.Nodes.Clear();
            var rootNode = tvwFiles.Nodes.Add("dataPack");
            InitializeNode(rootNode, false);

            // 补全目录
            CompleteDirectory();

            // 开始递归读取
            LoadDirectory(path, rootNode);
        }

        /// <summary>
        /// 补全必要的目录们
        /// </summary>
        private void CompleteDirectory()
        {
            // 一级
            Directory.CreateDirectory(packPath + @"\data");

            // 补全命名空间下的目录
            foreach (var i in Directory.GetDirectories(packPath + @"\data"))
            {
                Directory.CreateDirectory(i + @"\advancements");
                Directory.CreateDirectory(i + @"\functions");
                Directory.CreateDirectory(i + @"\recipes");
                Directory.CreateDirectory(i + @"\structures");
                Directory.CreateDirectory(i + @"\tags\blocks");
                Directory.CreateDirectory(i + @"\tags\items");
                Directory.CreateDirectory(i + @"\tags\functions");
            }
        }

        /// <summary>
        /// 加载指定目录的文件与目录到 <see cref="fileTree"/> 的指定节点
        /// </summary>
        /// <param name="path">指定目录</param>
        /// <param name="node">该目录对应的节点</param>
        /// 
        private void LoadDirectory(string path, TreeNode node)
        {
            // 读取目录
            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                var dirName = dir.LastIndexOf("\\") + 1;
                var dirNode = node.Nodes.Add(dir.Remove(0, dirName));
                InitializeNode(dirNode, false);
                // 递归
                LoadDirectory(dir, dirNode);
            }

            // 读取文件
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var fileName = file.Remove(0, file.LastIndexOf("\\") + 1);
                var fileNode = node.Nodes.Add(fileName);
                InitializeNode(fileNode, true);
            }
        }

        /// <summary>
        /// 初始化指定节点的状态
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <param name="layers">指定节点的层数</param>
        /// <param name="isFile">指示是否是一个文件</param>
        private void InitializeNode(TreeNode node, bool isFile)
        {
            if (isFile)
            {
                // 是文件

                node.Tag = new Item(Type.File, ((Item)node.Parent).Sort);

                // 设置显示文字和图片
                var suffix = ((Item)node).GetFileSuffix(false);
                if (node.Text.EndsWith(suffix))
                {
                    // 文件类型正确

                    // 判断文件具体类型
                    if (node.Level >= 4 && suffix == ".json")
                    {
                        node.ImageKey = node.SelectedImageKey = "Json";
                        node.Text = node.Text.Remove(node.Text.Length - ".json".Length);
                    }
                    else if (node.Level >= 4 && suffix == ".mcfunction")
                    {
                        node.ImageKey = node.SelectedImageKey = "Function";
                        node.Text = node.Text.Remove(node.Text.Length - ".mcfunction".Length);
                    }
                    else if (node.Level >= 4 && suffix == ".nbt")
                    {
                        node.ImageKey = node.SelectedImageKey = "Nbt";
                        node.Text = node.Text.Remove(node.Text.Length - ".nbt".Length);
                    }
                    else if (node.Level == 1 && node.Text == "pack.mcmeta")
                    {
                        node.ImageKey = node.SelectedImageKey = "Setting";
                        node.Text = Lang("global_settings");
                    }
                    else
                    {
                        node.ImageKey = node.SelectedImageKey = "Misc";
                        node.Tag = new Item(Type.Unknown, Sort.None);
                    }
                }
                else
                {
                    // 文件类型不正确
                    node.ImageKey = node.SelectedImageKey = "Misc";
                    node.Tag = new Item(Type.Unknown, Sort.None);
                }
            }
            else
            {
                // 是目录

                switch (node.Level)
                {
                    case 0:
                        // 根目录
                        node.ImageKey = node.SelectedImageKey = "DataPack";
                        node.Text = Lang("global_datapack");
                        node.Tag = new Item(Type.DataPack, Sort.None);
                        //node.Expand();
                        break;

                    case 1:
                        // TODO: 数据包、数据的图标

                        // 数据
                        if (node.Text.Equals("data"))
                        {
                            node.ImageKey = node.SelectedImageKey = "Directory";
                            node.Text = Lang("global_data");
                            node.Tag = new Item(Type.Data, Sort.None);
                            //node.Expand();
                        }
                        else
                        {
                            node.ImageKey = node.SelectedImageKey = "Misc";
                            node.Tag = new Item(Type.Unknown, Sort.None);
                        }
                        break;

                    case 2:
                        // 命名空间
                        node.ImageKey = node.SelectedImageKey = "Namespace";
                        node.Tag = new Item(Type.Namespace, Sort.None);
                        //node.Expand();
                        break;

                    case 3:
                        // 细分类
                        switch (node.Text)
                        {
                            case "advancements":
                                node.ImageKey = node.SelectedImageKey = "Advancements";
                                node.Text = Lang("global_advancement");
                                node.Tag = new Item(Type.Root, Sort.Advancement);
                                //node.Expand();
                                break;
                            case "functions":
                                node.ImageKey = node.SelectedImageKey = "Functions";
                                node.Text = Lang("global_function");
                                node.Tag = new Item(Type.Root, Sort.Function);
                                //node.Expand();
                                break;
                            case "loot_tables":
                                node.ImageKey = node.SelectedImageKey = "LootTables";
                                node.Text = Lang("global_loottable");
                                node.Tag = new Item(Type.Root, Sort.LootTable);
                                //node.Expand();
                                break;
                            case "structures":
                                node.ImageKey = node.SelectedImageKey = "Structures";
                                node.Text = Lang("global_structure");
                                node.Tag = new Item(Type.Root, Sort.Structure);
                                //node.Expand();
                                break;
                            case "recipes":
                                node.ImageKey = node.SelectedImageKey = "Recipes";
                                node.Text = Lang("global_recipe");
                                node.Tag = new Item(Type.Root, Sort.Recipe);
                                //node.Expand();
                                break;
                            case "tags":
                                node.ImageKey = node.SelectedImageKey = "Tags";
                                node.Text = Lang("global_tag");
                                node.Tag = new Item(Type.Root, Sort.Tag);
                                //node.Expand();
                                break;
                            default:
                                node.ImageKey = node.SelectedImageKey = "Misc";
                                node.Tag = new Item(Type.Unknown, Sort.None);
                                break;
                        }
                        break;

                    case 4:
                        // tags下细分类
                        if (((Item)node.Parent).Sort == Sort.Tag)
                        {
                            node.ImageKey = node.SelectedImageKey = "Directory";
                            node.Tag = new Item(Type.Root, Sort.Tag);
                            switch (node.Text)
                            {
                                case "blocks":
                                    node.Text = Lang("global_block");
                                    break;
                                case "items":
                                    node.Text = Lang("global_item");
                                    break;
                                case "functions":
                                    node.Text = Lang("global_function");
                                    break;
                                default:
                                    node.ImageKey = node.SelectedImageKey = "Misc";
                                    node.Tag = new Item(Type.Unknown, Sort.None);
                                    break;
                            }
                        }
                        else
                        {
                            goto default;
                        }
                        break;

                    default:
                        // 其他层级

                        // 判断是否合法
                        if (IsNameLegal(node.Text))
                        {
                            // 合法
                            // 跟随父级目录的属性
                            node.ImageKey = node.SelectedImageKey = "Directory";
                            node.Tag = new Item(Type.Directory, ((Item)node.Parent).Sort);
                        }
                        else
                        {
                            // 非法
                            node.ImageKey = node.SelectedImageKey = "Misc";
                            node.Tag = new Item(Type.Unknown, Sort.None);
                        }

                        break;
                }
            }

            // 全部展开
            tvwFiles.ExpandAll();
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
            smnuRefresh.Enabled = true;
            smnuExplorer.Enabled = true;
            smnuVisualizationEditor.Enabled = true;
            smnuJsonEditor.Enabled = true;
            smnuFunctionEditor.Enabled = true;
            smnuCopy.Text = Lang("filetree_copy");
            smnuCut.Text = Lang("filetree_cut");
            smnuDelete.Text = Lang("filetree_delete");
            smnuAdd.Text = Lang("filetree_add");
            smnuAddDirectory.Text = Lang("filetree_folder");
            smnuAddFile.Text = Lang("filetree_file");
            smnuPaste.Text = Lang("filetree_paste");
            smnuProperty.Text = Lang("filetree_property");
            smnuRename.Text = Lang("filetree_rename");


            if (tvwFiles.SelectedNode != null)
            {
                // 更换“收起”“展开”
                //if (tvwFiles.SelectedNode.IsExpanded)
                //    smnuOpen.Text = Lang("filetree_collapse");
                //else
                //    smnuOpen.Text = Lang("filetree_expand");
                smnuOpen.Enabled = false;

                // 更改普遍可用性
                switch (SelectedItem.Type)
                {
                    case Type.DataPack:
                        smnuDelete.Enabled = false;
                        smnuAdd.Enabled = false;
                        smnuAddFile.Enabled = false;
                        smnuAddDirectory.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case Type.Data:
                        smnuAddDirectory.Text = Lang("global_namespace");
                        smnuDelete.Enabled = false;
                        smnuAddFile.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case Type.Namespace:
                        smnuAdd.Enabled = false;
                        smnuAddFile.Enabled = false;
                        smnuAddDirectory.Enabled = false;
                        break;
                    case Type.Root:
                        smnuDelete.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case Type.File:
                        smnuOpen.Text = Lang("filetree_edit");
                        smnuOpen.Enabled = true;
                        break;
                    default:
                        break;
                }

                // 更改“粘贴”可用性
                if (GetClipboardList(out var isCut) is null)
                {
                    smnuPaste.Enabled = false;
                }

                // 更改“添加文件”名字
                switch (SelectedItem.Sort)
                {
                    case Sort.Advancement:
                        smnuAddFile.Text = Lang("global_advancement");
                        break;
                    case Sort.Function:
                        smnuAddFile.Text = Lang("global_function");
                        break;
                    case Sort.LootTable:
                        smnuAddFile.Text = Lang("global_loottable");
                        break;
                    case Sort.Recipe:
                        smnuAddFile.Text = Lang("global_recipe");
                        break;
                    case Sort.Structure:
                        smnuAddFile.Text = Lang("global_structure");
                        break;
                    case Sort.Tag:
                        smnuAddFile.Text = Lang("global_tag");
                        break;
                    default:
                        break;
                }

                // 更改编辑器可用性
                switch (SelectedItem.Type)
                {
                    case Type.File:
                        switch (SelectedItem.Sort)
                        {
                            case Sort.Advancement:
                            case Sort.LootTable:
                            case Sort.Recipe:
                            case Sort.Tag:
                                smnuFunctionEditor.Enabled = false;
                                break;
                            case Sort.Function:
                                smnuJsonEditor.Enabled = false;
                                break;
                            default:
                                smnuVisualizationEditor.Enabled = false;
                                smnuJsonEditor.Enabled = false;
                                smnuFunctionEditor.Enabled = false;
                                break;
                        }
                        break;
                    default:
                        smnuVisualizationEditor.Enabled = false;
                        smnuJsonEditor.Enabled = false;
                        smnuFunctionEditor.Enabled = false;
                        break;
                }
            }
        }

        /// <summary>
        /// 响应菜单快捷键
        /// </summary>
        private void RespondShortcutKeys(object sender, KeyEventArgs e)
        {
            SetSmnus();
            switch (e.KeyCode)
            {
                case Keys.C:
                    if (e.Control)
                    {
                        // 避免按键后的警报声
                        e.SuppressKeyPress = true;

                        CopyItem();
                    }
                    break;
                case Keys.Delete:
                    e.SuppressKeyPress = true;
                    DeleteItem();
                    break;
                case Keys.Enter:
                    e.SuppressKeyPress = true;
                    OpenItem();
                    break;
                case Keys.F1:
                    e.SuppressKeyPress = true;
                    SeeProperty();
                    break;
                case Keys.F2:
                    e.SuppressKeyPress = true;
                    RenameItem();
                    break;
                case Keys.F5:
                    e.SuppressKeyPress = true;
                    RefreshItems();
                    break;
                case Keys.V:
                    if (e.Control)
                    {
                        e.SuppressKeyPress = true;
                        PasteItem();
                    }
                    break;
                case Keys.X:
                    if (e.Control)
                    {
                        e.SuppressKeyPress = true;
                        CutItem();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 显示右键菜单
        /// </summary>
        private void ShowMenu(object sender, TreeNodeMouseClickEventArgs e)
        {
            tvwFiles.SelectedNode = e.Node;

            if (e.Button == MouseButtons.Right)
            {
                SetSmnus();
                cmnuItem.Show(MousePosition);
            }
        }

        /// <summary>
        /// 展开/编辑项目
        /// </summary>
        private void OpenItem()
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

        /// <summary>
        /// 重命名项目
        /// </summary>
        private void RenameItem()
        {
            if (smnuRename.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    tvwFiles.SelectedNode.BeginEdit();
                }
            }
        }

        /// <summary>
        /// 查看项目属性
        /// </summary>
        private void SeeProperty()
        {
            if (smnuProperty.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    MessageBox.Show(
                        Lang("filetree_properties_name") + ": " + tvwFiles.SelectedNode.Text + "\n" +
                        Lang("filetree_properties_type") + ": " + tvwFiles.SelectedNode.Tag + "\n" +
                        Lang("filetree_properties_path") + ": " + GetPathFromNode(tvwFiles.SelectedNode) + "\n"
                        );
                }
            }
        }

        /// <summary>
        /// 在项目下新建文件夹
        /// </summary>
        private void AddDirectory()
        {
            if (smnuAddDirectory.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    isAddingFolder = true;

                    TreeNode node;
                    string name;
                    if (IsFile(tvwFiles.SelectedNode))
                    {
                        name = GetAvailableDirName(GetPathFromNode(tvwFiles.SelectedNode) + "\\new_folder");
                        node = tvwFiles.SelectedNode.Parent.Nodes.Add(name);
                        tvwFiles.SelectedNode.Parent.Expand();
                    }
                    else
                    {
                        name = GetAvailableDirName(GetPathFromNode(tvwFiles.SelectedNode) + "\\new_folder");
                        node = tvwFiles.SelectedNode.Nodes.Add(name);
                        tvwFiles.SelectedNode.Expand();
                    }

                    InitializeNode(node, false);

                    try
                    {
                        // 尝试创建目录
                        Directory.CreateDirectory(GetPathFromNode(node));
                        node.BeginEdit();
                    }
                    catch (Exception ex)
                    {
                        // 出错

                        // 弹窗提示
                        MessageBox.Show(ex.Message);

                        if (!Directory.Exists(GetPathFromNode(node)))
                        {
                            // 目录创建失败
                            // 把节点删除
                            tvwFiles.Nodes.Remove(node);
                        }
                    }

                    // 补全目录
                    CompleteDirectory();
                }
            }
        }


        /// <summary>
        /// 在项目下新建文件
        /// </summary>
        private void AddFile()
        {
            if (smnuAddFile.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    TreeNode node;
                    var name = GetAvailableFileName(
                        GetPathFromNode(tvwFiles.SelectedNode) + "\\new_file",
                        SelectedItem.GetFileSuffix(true)
                        );
                    if (IsFile(tvwFiles.SelectedNode))
                    {
                        // 在文件级别右键

                        // 在父级目录下增加节点
                        node = tvwFiles.SelectedNode.Parent.Nodes.Add(name);

                        // 展开父级目录
                        tvwFiles.SelectedNode.Parent.Expand();
                    }
                    else
                    {
                        // 在目录级别右键

                        // 在本级目录下增加节点
                        node = tvwFiles.SelectedNode.Nodes.Add(name);

                        // 展开本级目录
                        tvwFiles.SelectedNode.Expand();
                    }

                    InitializeNode(node, true);

                    try
                    {
                        // 尝试创建文件
                        File.WriteAllText(GetPathFromNode(node), "");
                        node.BeginEdit();
                    }
                    catch (Exception ex)
                    {
                        // 出错
                        // 弹窗提示
                        MessageBox.Show(ex.Message);

                        if (!File.Exists(GetPathFromNode(node)))
                        {
                            // 文件创建失败
                            // 把节点删除
                            tvwFiles.Nodes.Remove(node);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        private void DeleteItem()
        {
            if (smnuDelete.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    // 提示
                    var result = MessageBox.Show(this, Lang("filetree_msgbox_suretodelete")
                        .Replace("{0}", tvwFiles.SelectedNode.Text),
                        Application.ProductName, MessageBoxButtons.OKCancel);

                    if (result == DialogResult.OK)
                    {
                        // 确定删除

                        try
                        {
                            // 尝试删除

                            if (File.Exists(GetPathFromNode(tvwFiles.SelectedNode)))
                                // 删除文件
                                File.Delete(GetPathFromNode(tvwFiles.SelectedNode));
                            else
                                // 删除目录
                                Directory.Delete(GetPathFromNode(tvwFiles.SelectedNode), true);

                            // 移除节点
                            tvwFiles.Nodes.Remove(tvwFiles.SelectedNode);

                        }
                        catch (Exception ex)
                        {
                            // 出错

                            // 弹窗提示
                            MessageBox.Show(ex.Message);

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 复制项目
        /// </summary>
        private void CopyItem()
        {
            if (smnuCopy.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    SetClipboardList(new[] { GetPathFromNode(tvwFiles.SelectedNode) }, false);
                }
            }
        }

        /// <summary>
        /// 剪切项目
        /// </summary>
        private void CutItem()
        {
            if (smnuCut.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    SetClipboardList(new[] { GetPathFromNode(tvwFiles.SelectedNode) }, true);
                }
            }
        }

        /// <summary>
        /// 粘贴项目
        /// </summary>
        private void PasteItem()
        {
            if (smnuPaste.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    // 获取列表
                    var list = GetClipboardList(out var isCut);
                    if (list is null)
                    {
                        return;
                    }

                    // 尝试粘贴
                    try
                    {
                        PasteInDisk(list, isCut);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    // 刷新
                    RefreshItems();
                }
            }
        }

        /// <summary>
        /// 在磁盘目录上粘贴文件或目录
        /// </summary>
        /// <param name="list">被粘贴的文件路径数组</param>
        /// <param name="isCut">指定是否是剪切</param>
        private void PasteInDisk(string[] list, bool isCut)
        {
            foreach (var source in list)
            {
                string destination;

                // 设定粘贴路径
                if (IsFile(tvwFiles.SelectedNode))
                {
                    // 选择的是文件
                    // 粘贴到上一级目录
                    destination = GetPathFromNode(tvwFiles.SelectedNode.Parent);
                }
                else
                {
                    // 选择的是目录
                    // 粘贴到本级下面
                    destination = GetPathFromNode(tvwFiles.SelectedNode);
                }
                destination += source.Remove(0, source.LastIndexOf("\\"));
                if (File.Exists(source))
                {
                    // 是文件

                    destination = destination.Remove(destination.LastIndexOf("\\") + 1) + GetAvailableFileName(destination);

                    if (isCut)
                    {
                        // 剪切文件
                        File.Move(source, destination);
                    }
                    else
                    {
                        // 复制文件
                        File.Copy(source, destination);
                    }
                }
                else if (Directory.Exists(source))
                {
                    // 是目录

                    destination = destination.Remove(destination.LastIndexOf("\\") + 1) + GetAvailableDirName(destination);

                    if (isCut)
                    {
                        // 剪切目录
                        Directory.Move(source, destination);
                    }
                    else
                    {
                        // 复制目录
                        CopyDirectory(source, destination);
                    }
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshItems()
        {
            if (smnuRefresh.Enabled)
            {
                // 记录当前选中的节点信息
                var selectedFullPath = tvwFiles.SelectedNode.FullPath;

                // 重新加载文件
                LoadFileTree(packPath);

                // 选择刚刚选中的节点
                SelectNodeFromTextAndLevel(selectedFullPath);
            }
        }

        /// <summary>
        /// 根据指定文字和所在层级选中节点
        /// </summary>
        /// <param name="text">指定文字</param>
        /// <param name="level">指定层级</param>
        private void SelectNodeFromTextAndLevel(string fullPath)
        {
            foreach (TreeNode i in tvwFiles.Nodes)
            {
                if (i.FullPath == fullPath)
                {
                    tvwFiles.SelectedNode = i;
                    return;
                }
                SelectNodeFromTextAndLevel(fullPath, i);
            }
        }

        /// <summary>
        /// 根据指定文字和所在层级选中节点
        /// </summary>
        /// <param name="text">指定文字</param>
        /// <param name="level">指定层级</param>
        private void SelectNodeFromTextAndLevel(string fullPath, TreeNode node)
        {
            foreach (TreeNode i in node.Nodes)
            {
                if (i.FullPath == fullPath)
                {
                    tvwFiles.SelectedNode = i;
                    return;
                }
                SelectNodeFromTextAndLevel(fullPath, i);
            }
        }

        /// <summary>
        /// 用可视编辑器打开
        /// </summary>
        private void OpenWithVisualizatonEditor()
        {
            if (smnuVisualizationEditor.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {

                }
            }
        }

        /// <summary>
        /// 用Json编辑器打开
        /// </summary>
        private void OpenWithJsonEditor()
        {
            if (smnuJsonEditor.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {

                }
            }
        }

        /// <summary>
        /// 用函数编辑器打开
        /// </summary>
        private void OpenWithFunctionEditor()
        {
            if (smnuFunctionEditor.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {

                }
            }
        }

        /// <summary>
        /// 用文件资源管理器打开
        /// </summary>
        private void OpenWithExplorer()
        {
            if (smnuExplorer.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    ProcessStartInfo info = new ProcessStartInfo("explorer.exe");
                    if (IsFile(tvwFiles.SelectedNode))
                    {
                        // 选中的是文件
                        // 打开explorer.exe, 将光标定位在此文件
                        info.Arguments = "/e,/select," + GetPathFromNode(tvwFiles.SelectedNode);
                    }
                    else
                    {
                        // 选中的是文件夹
                        // 以此文件夹作为根目录打开explorer.exe
                        info.Arguments = "/root," + GetPathFromNode(tvwFiles.SelectedNode);
                    }
                    Process.Start(info);
                }
            }
        }

        #region 事件响应

        private void tvwFiles_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // 取消命名 后续命名由代码手动实现
            e.CancelEdit = true;

            if (e.Label != null && e.Label != e.Node.Text)
            {
                // 确实改名了

                if (!IsNameLegal(e.Label))
                {
                    MessageBox.Show(Lang("filetree_msgbox_name"));
                }
                else
                {
                    // 合法文件名

                    var before = GetPathFromNode(e.Node);
                    var backup = e.Node.Text;
                    var isFile = IsFile(e.Node);
                    e.Node.Text = e.Label;
                    var after = GetPathFromNode(e.Node);

                    if (isFile)
                    {
                        // 重命名文件

                        if (File.Exists(after))
                        {
                            // 命名后的文件存在 这可坏了
                            MessageBox.Show(Lang("filetree_msgbox_exist").Replace("{0}", e.Node.Text));
                            e.Node.Text = backup;
                        }
                        else if (File.Exists(before))
                        {
                            // 命名前的文件存在 直接重命名
                            File.Move(before, after);
                        }
                        else
                        {
                            // 命名前的文件并不存在 创建命名后的目录
                            File.WriteAllText(after, "");
                        }
                    }
                    else
                    {
                        // 重命名目录

                        if (Directory.Exists(after))
                        {
                            // 命名后的目录存在 这可坏了
                            MessageBox.Show(Lang("filetree_msgbox_exist").Replace("{0}", e.Node.Text));
                            e.Node.Text = backup;
                        }
                        else if (Directory.Exists(before))
                        {
                            // 命名前的目录存在 直接重命名
                            Directory.Move(before, after);
                        }
                        else
                        {
                            // 命名前的目录并不存在 创建命名后的目录
                            Directory.CreateDirectory(after);
                        }
                        InitializeNode(e.Node, false);
                    }
                }
            }

            if (isAddingFolder)
            {
                RefreshItems();
                isAddingFolder = false;
            }
        }

        private void smnuOpen_Click(object sender, EventArgs e)
        {
            OpenItem();
        }

        private void smnuRename_Click(object sender, EventArgs e)
        {
            RenameItem();
        }

        private void smnuProperty_Click(object sender, EventArgs e)
        {
            SeeProperty();
        }

        private void smnuAddDirectory_Click(object sender, EventArgs e)
        {
            AddDirectory();
        }

        private void smnuAddFile_Click(object sender, EventArgs e)
        {
            AddFile();
        }

        private void smnuCopy_Click(object sender, EventArgs e)
        {
            CopyItem();
        }

        private void smnuCut_Click(object sender, EventArgs e)
        {
            CutItem();
        }

        private void smnuPaste_Click(object sender, EventArgs e)
        {
            PasteItem();
        }

        private void smnuDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }

        private void smnuVisualizationEditor_Click(object sender, EventArgs e)
        {
            OpenWithVisualizatonEditor();
        }

        private void smnuJsonEditor_Click(object sender, EventArgs e)
        {
            OpenWithJsonEditor();
        }

        private void smnuFunctionEditor_Click(object sender, EventArgs e)
        {
            OpenWithFunctionEditor();
        }

        private void smnuExplorer_Click(object sender, EventArgs e)
        {
            OpenWithExplorer();
        }

        private void smnuRefresh_Click(object sender, EventArgs e)
        {
            RefreshItems();
        }

        private void tvwFiles_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            // 想收起？不存在的
            e.Node.Expand();
        }
        #endregion

        #endregion
    }
}