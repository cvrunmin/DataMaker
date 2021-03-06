﻿using DataMaker.Forms;
using DataMaker.Parsers;
using DataMaker.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static DataMaker.Utils;

namespace DataMaker
{
    public enum ItemType { DataPack, Data, Namespace, SortRoot, Module, File, Unknown }

    public enum ItemSort { Advancement, Function, LootTable, Recipe, Structure, TagRoot , BlockTag, ItemTag, FunctionTag, PackMcmeta, None }

    /// <summary>
    /// 代表一个项目(文件/文件夹)
    /// </summary>
    public struct Item
    {
        public ItemType Type { get; set; }
        public ItemSort Sort { get; set; }

        public Item(ItemType type, ItemSort sort)
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

            if (!checkSubFile && Type != ItemType.File)
            {
                return suffix;
            }

            switch (Sort)
            {
                case ItemSort.Advancement:
                case ItemSort.LootTable:
                case ItemSort.Recipe:
                case ItemSort.BlockTag:
                case ItemSort.ItemTag:
                case ItemSort.FunctionTag:
                    suffix = ".json";
                    break;
                case ItemSort.Function:
                    suffix = ".mcfunction";
                    break;
                case ItemSort.Structure:
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
                return new Item(ItemType.Unknown, ItemSort.None);
            }
            var type = (ItemType)Enum.Parse(typeof(ItemType), node.Tag.ToString().Split('|')[0]);
            var sort = (ItemSort)Enum.Parse(typeof(ItemSort), node.Tag.ToString().Split('|')[1]);

            return new Item(type, sort);
        }

        public static explicit operator Item(string str)
        {
            var type = (ItemType)Enum.Parse(typeof(ItemType), str.Split('|')[0]);
            var sort = (ItemSort)Enum.Parse(typeof(ItemSort), str.Split('|')[1]);

            return new Item(type, sort);
        }
    }

    public partial class FileTree : Form
    {
        /* 使用 TreeNode 的各个属性储存不同信息
         * Tag: Sort 与 Type
         * Text: 显示名称
         * ToolTipText: 实际文件名
         * Name: 实际目录名
         */
        private static string dataPackPath;

        /// <summary>
        /// 是否正在新建文件夹
        /// </summary>
        private static bool isAddingFolder;

        /// <summary>
        /// 要加载的文件夹路径
        /// </summary>
        public string DatapackPath
        {
            get => DataPackPath;
            set
            {
                if (!File.Exists(value + @"\pack.mcmeta") && (
                    Directory.GetDirectories(value).Length > 0 ||
                    Directory.GetFiles(value).Length > 0)
                    )
                {
                    // 不是空的，且不像是数据包
                    var result = MessageBox.Show(this,
                        Lang("filetree_msgbox_notadatapack").Replace("{0}", value),
                        Application.ProductName, MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                        return;
                }
                DataPackPath = value;
                LoadFileTree(DatapackPath);
            }
        }

        #region 各依赖项属性
        public static List<string> DependentAdvancements
        {
            get;
            set;
        } = new List<string>();

        public static List<string> DependentRecipes
        {
            get;
            set;
        } = new List<string>();

        public static List<string> DependentBlockTags
        {
            get;
            set;
        } = new List<string>();

        public static List<string> DependentFunctionTags
        {
            get;
            set;
        } = new List<string>();

        public static List<string> DependentItemTags
        {
            get;
            set;
        } = new List<string>();

        public static List<string> DependentFunctions
        {
            get;
            set;
        } = new List<string>();

        public static List<string> DependentLootTables
        {
            get;
            set;
        } = new List<string>();

        public static List<string> DependentStructures
        {
            get;
            set;
        } = new List<string>();
        #endregion

        #region Single instance
        private static FileTree fileTree;

        /// <summary>
        /// 获取 <see cref="FileTree"/> 的唯一实例
        /// </summary>
        public static FileTree GetInstance()
        {
            if (fileTree == null)
                fileTree = new FileTree();

            return fileTree;
        }
        #endregion

        #region 定义函数
        /// <summary>
        /// 获取全部指定类型文件的ID
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<string> GetAllIds
            (ItemSort sort, bool containsDependencies, 
            ItemType type = ItemType.File,
            TreeNodeCollection nodes = null)
        {
            // 默认值为根Nodes
            if (nodes == null) nodes = GetInstance().tvwFiles.Nodes;

            var result = new List<string>();

            // 加入项目中已有的
            foreach (var i in nodes)
            {
                var node = (TreeNode)i;
                // 当前node符合，加入列表
                if (((Item)node).Type == type && ((Item)node).Sort == sort)
                    result.Add(node.GetID());
                // 递归
                result.AddRange(GetAllIds(sort, false, type, node.Nodes));
            }

            if (containsDependencies)
            {
                // 加入依赖项中有的
                var target = new List<string>();
                switch (sort)
                {
                    case ItemSort.Advancement:
                        target = DependentAdvancements;
                        break;
                    case ItemSort.Function:
                        target = DependentFunctions;
                        break;
                    case ItemSort.LootTable:
                        target = DependentLootTables;
                        break;
                    case ItemSort.Recipe:
                        target = DependentRecipes;
                        break;
                    case ItemSort.Structure:
                        target = DependentStructures;
                        break;
                    case ItemSort.BlockTag:
                        target = DependentBlockTags;
                        break;
                    case ItemSort.ItemTag:
                        target = DependentItemTags;
                        break;
                    case ItemSort.FunctionTag:
                        target = DependentFunctionTags;
                        break;
                    default:
                        break;
                }
                foreach (var i in target) result.Add(i);
            }

            return result;
        }
        
        /// <summary>
        /// 根据指定节点获取Editor
        /// </summary>
        /// <param name="node">指定节点</param>
        public static Editor GetEditor(TreeNode node)
        {
            // 设置 editor
            var editor = new Editor();
            var json = GetRootParserJson(((Item)node).Sort);
            editor.SetEditor(json);

            // 用该 editor 读取已有 json
            editor.Json = File.ReadAllText(node.GetFilePath());

            return editor;
        }

        /// <summary>
        /// 根据指定 Sort 获得该文件的 Parser 的 RootFrame json .
        /// </summary>
        /// <param name="sort">指定 Sort </param>
        /// <returns>RootFrame json.</returns>
        private static string GetRootParserJson(ItemSort sort)
        {
            var rootParserFileName = "";

            switch (sort)
            {
                case ItemSort.Advancement:
                    break;
                case ItemSort.Function:
                    break;
                case ItemSort.LootTable:
                    break;
                case ItemSort.Recipe:
                    rootParserFileName = "recipe/root";
                    break;
                case ItemSort.Structure:
                    break;
                case ItemSort.BlockTag:
                case ItemSort.ItemTag:
                case ItemSort.FunctionTag:
                    rootParserFileName = "tag/root";
                    break;
                case ItemSort.PackMcmeta:
                    rootParserFileName = "packmcmeta/root";
                    break;
            }

            // 设置RootParserJson
            var json =
$@"{{
    ""key"": ""%NoKey%"",
    ""show_index"": 0,
    ""json"": ""{rootParserFileName}""
}}";
            return json;
        }

        /// <summary>
        /// 将指定数据类的内容保存到指定节点
        /// </summary>
        /// <param name="editor">指定数据类</param>
        public static void SaveFile(Editor editor, TreeNode node)
        {
            var path = node.GetFilePath();
            File.WriteAllText(path, editor.Json);
        }

        /// <summary>
        /// 复制文件夹及文件夹下所有子文件夹和文件
        /// </summary>
        /// <param name="sourcePath">待复制的文件夹路径</param>
        /// <param name="destinationPath">目标路径</param>
        private static void CopyDirectory(string sourcePath, string destinationPath)
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
        /// 复制或剪切文件路径数组至剪贴板
        /// </summary>
        /// <param name="files">文件路径数组</param>
        /// <param name="cut">是否剪切</param>
        private static void SetClipboardList(string[] files, bool cut)
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
        private static string[] GetClipboardList(out bool isCut)
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
        private static string GetAvailableFileName(string path, string suffix)
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
        private static string GetAvailableFileName(string pathDir)
        {
            // 根据完整路径取得后缀和路径
            var suffix = pathDir.Remove(0, pathDir.LastIndexOf("."));
            var path = pathDir.Remove(pathDir.LastIndexOf("."));

            return GetAvailableFileName(path, suffix);
        }

        /// <summary>
        /// 根据目录和目录名获取可用的目录名
        /// </summary>
        private static string GetAvailableDirName(string path)
        {
            var subDir = path.Remove(0, path.LastIndexOf("\\") + 1);
            var dir = path.Remove(path.Length - subDir.Length - 1, path.Length - path.LastIndexOf("\\"));

            var result = subDir;

            for (int i = 1; Directory.Exists(dir + "\\" + result); i++)
                result = subDir + "_" + i;

            return result;
        }

        /// <summary>
        /// 获取被选中的节点对应的Item
        /// </summary>
        private static Item SelectedItem => (Item)GetInstance().tvwFiles.SelectedNode;

        public static string DataPackPath
        {
            get => dataPackPath;
            set => dataPackPath = value;
        }
        #endregion

        #region 启动时初始化
        private FileTree()
        {
            InitializeComponent();

            DarkTheme.Initialize(this);
            lblSizeChanger.BackColor = DarkTheme.HoverColor;

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
            tvwFiles.ImageList.Images.Add("BlockTags", (Image)rm.GetObject("BlockTags"));
            tvwFiles.ImageList.Images.Add("ItemTags", (Image)rm.GetObject("ItemTags"));
            tvwFiles.ImageList.Images.Add("FunctionTags", (Image)rm.GetObject("FunctionTags"));
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
            rootNode.Name = path;
            InitializeNode(rootNode, false);

            // 补全目录
            CompleteDirectory();

            // 开始递归读取
            LoadDirectory(path, rootNode);

            // 加载依赖项
            var content = File.ReadAllText($@"{dataPackPath}\!dependencies.json");
            var jObj = JsonConvert.DeserializeObject<JObject>(content);
            foreach (JProperty i in jObj.Children())
            {
                if (i.Value["advancements"] != null)
                {
                    DependentAdvancements.AddRange(i.Value["advancements"].ToObject<List<string>>());
                }

                if (i.Value["recipes"] != null)
                {
                    DependentRecipes.AddRange(i.Value["recipes"].ToObject<List<string>>());
                }

                if (i.Value["block_tags"] != null)
                {
                    DependentBlockTags.AddRange(i.Value["block_tags"].ToObject<List<string>>());
                }

                if (i.Value["item_tags"] != null)
                {
                    DependentItemTags.AddRange(i.Value["item_tags"].ToObject<List<string>>());
                }

                if (i.Value["function_tags"] != null)
                {
                    DependentFunctionTags.AddRange(i.Value["function_tags"].ToObject<List<string>>());
                }

                if (i.Value["structures"] != null)
                {
                    DependentStructures.AddRange(i.Value["structures"].ToObject<List<string>>());
                }

                if (i.Value["functions"] != null)
                {
                    DependentFunctions.AddRange(i.Value["functions"].ToObject<List<string>>());
                }

                if (i.Value["loot_tables"] != null)
                {
                    DependentLootTables.AddRange(i.Value["loot_tables"].ToObject<List<string>>());
                }
            }
        }

        /// <summary>
        /// 补全必要的目录们
        /// </summary>
        private void CompleteDirectory()
        {
            // 一级
            Directory.CreateDirectory(DataPackPath + @"\data");
            if (!File.Exists(DataPackPath + @"\pack.mcmeta"))
            {
                // 创建默认的 pack.mcmeta 文件
                using (var editor = new Editor())
                {
                    editor.SetEditor(GetRootParserJson(ItemSort.PackMcmeta));
                    File.WriteAllText(DataPackPath + @"\pack.mcmeta", editor.Json);
                }
            }
            if (!File.Exists(DataPackPath + @"\!dependencies.json"))
            {
                // 创建默认的 !dependencies.json 文件
                // 依赖默认
                File.Copy(@"Jsons\!dependencies.json", DataPackPath + @"\!dependencies.json");
            }

            // 补全命名空间下的目录
            foreach (var i in Directory.GetDirectories(DataPackPath + @"\data"))
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
                var dirName = dir.Remove(0, dir.LastIndexOf("\\") + 1);
                if (IsNameLegal(dirName))
                {
                    var dirNode = node.Nodes.Add(dirName);
                    dirNode.Name = dir;
                    dirNode.ToolTipText = dirName;
                    InitializeNode(dirNode, false);
                    // 递归
                    LoadDirectory(dir, dirNode);
                }
            }

            // 读取文件
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var fileName = file.Remove(0, file.LastIndexOf("\\") + 1);
                if (IsNameLegal(fileName))
                {
                    var fileNode = node.Nodes.Add(fileName);
                    fileNode.Name = file;
                    fileNode.ToolTipText = fileName;
                    InitializeNode(fileNode, true);
                }
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

                node.Tag = new Item(ItemType.File, ((Item)node.Parent).Sort);

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
                        node.Tag = new Item(ItemType.File, ItemSort.PackMcmeta);
                        node.Text = Lang("global_settings");
                    }
                    else
                    {
                        node.ImageKey = node.SelectedImageKey = "Misc";
                        node.Tag = new Item(ItemType.Unknown, ItemSort.None);
                    }
                }
                else
                {
                    // 文件类型不正确
                    node.ImageKey = node.SelectedImageKey = "Misc";
                    node.Tag = new Item(ItemType.Unknown, ItemSort.None);
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
                        node.Tag = new Item(ItemType.DataPack, ItemSort.None);
                        //node.Expand();
                        break;

                    case 1:
                        // 数据
                        if (node.Text.Equals("data"))
                        {
                            node.ImageKey = node.SelectedImageKey = "Directory";
                            node.Text = Lang("global_data");
                            node.Tag = new Item(ItemType.Data, ItemSort.None);
                            //node.Expand();
                        }
                        else
                        {
                            node.ImageKey = node.SelectedImageKey = "Misc";
                            node.Tag = new Item(ItemType.Unknown, ItemSort.None);
                        }
                        break;

                    case 2:
                        // 命名空间
                        node.ImageKey = node.SelectedImageKey = "Namespace";
                        node.Tag = new Item(ItemType.Namespace, ItemSort.None);
                        //node.Expand();
                        break;

                    case 3:
                        // 细分类
                        switch (node.Text)
                        {
                            case "advancements":
                                node.ImageKey = node.SelectedImageKey = "Advancements";
                                node.Text = Lang("global_advancement");
                                node.Tag = new Item(ItemType.SortRoot, ItemSort.Advancement);
                                //node.Expand();
                                break;
                            case "functions":
                                node.ImageKey = node.SelectedImageKey = "Functions";
                                node.Text = Lang("global_function");
                                node.Tag = new Item(ItemType.SortRoot, ItemSort.Function);
                                //node.Expand();
                                break;
                            case "loot_tables":
                                node.ImageKey = node.SelectedImageKey = "LootTables";
                                node.Text = Lang("global_loottable");
                                node.Tag = new Item(ItemType.SortRoot, ItemSort.LootTable);
                                //node.Expand();
                                break;
                            case "structures":
                                node.ImageKey = node.SelectedImageKey = "Structures";
                                node.Text = Lang("global_structure");
                                node.Tag = new Item(ItemType.SortRoot, ItemSort.Structure);
                                //node.Expand();
                                break;
                            case "recipes":
                                node.ImageKey = node.SelectedImageKey = "Recipes";
                                node.Text = Lang("global_recipe");
                                node.Tag = new Item(ItemType.SortRoot, ItemSort.Recipe);
                                //node.Expand();
                                break;
                            case "tags":
                                node.ImageKey = node.SelectedImageKey = "Tags";
                                node.Text = Lang("global_tag");
                                node.Tag = new Item(ItemType.SortRoot, ItemSort.TagRoot);
                                //node.Expand();
                                break;
                            default:
                                node.ImageKey = node.SelectedImageKey = "Misc";
                                node.Tag = new Item(ItemType.Unknown, ItemSort.None);
                                break;
                        }
                        break;

                    case 4:
                        // tags下细分类
                        if (((Item)node.Parent).Sort == ItemSort.TagRoot)
                        {
                            node.ImageKey = node.SelectedImageKey = "Directory";
                            switch (node.Text)
                            {
                                case "blocks":
                                    node.ImageKey = node.SelectedImageKey = "BlockTags";
                                    node.Tag = new Item(ItemType.SortRoot, ItemSort.BlockTag);
                                    node.Text = Lang("global_block");
                                    break;
                                case "items":
                                    node.ImageKey = node.SelectedImageKey = "ItemTags";
                                    node.Tag = new Item(ItemType.SortRoot, ItemSort.ItemTag);
                                    node.Text = Lang("global_item");
                                    break;
                                case "functions":
                                    node.ImageKey = node.SelectedImageKey = "FunctionTags";
                                    node.Tag = new Item(ItemType.SortRoot, ItemSort.FunctionTag);
                                    node.Text = Lang("global_function");
                                    break;
                                default:
                                    node.ImageKey = node.SelectedImageKey = "Misc";
                                    node.Tag = new Item(ItemType.Unknown, ItemSort.None);
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
                            node.Tag = new Item(ItemType.Module, ((Item)node.Parent).Sort);
                        }
                        else
                        {
                            // 非法
                            node.ImageKey = node.SelectedImageKey = "Misc";
                            node.Tag = new Item(ItemType.Unknown, ItemSort.None);
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
                    case ItemType.DataPack:
                        smnuDelete.Enabled = false;
                        smnuAdd.Enabled = false;
                        smnuAddFile.Enabled = false;
                        smnuAddDirectory.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case ItemType.Data:
                        smnuAddDirectory.Text = Lang("global_namespace");
                        smnuDelete.Enabled = false;
                        smnuAddFile.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case ItemType.Namespace:
                        smnuAdd.Enabled = false;
                        smnuAddFile.Enabled = false;
                        smnuAddDirectory.Enabled = false;
                        break;
                    case ItemType.SortRoot:
                        smnuDelete.Enabled = false;
                        smnuRename.Enabled = false;
                        break;
                    case ItemType.File:
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
                    case ItemSort.Advancement:
                        smnuAddFile.Text = Lang("global_advancement");
                        break;
                    case ItemSort.Function:
                        smnuAddFile.Text = Lang("global_function");
                        break;
                    case ItemSort.LootTable:
                        smnuAddFile.Text = Lang("global_loottable");
                        break;
                    case ItemSort.Recipe:
                        smnuAddFile.Text = Lang("global_recipe");
                        break;
                    case ItemSort.Structure:
                        smnuAddFile.Text = Lang("global_structure");
                        break;
                    case ItemSort.BlockTag:
                    case ItemSort.ItemTag:
                    case ItemSort.FunctionTag:
                        smnuAddFile.Text = Lang("global_tag");
                        break;
                    // 不允许删除配置文件
                    case ItemSort.PackMcmeta:
                        smnuDelete.Enabled = false;
                        break;
                    // 不允许在Tags下新建
                    case ItemSort.TagRoot:
                        smnuAdd.Enabled = false;
                        smnuAddDirectory.Enabled = false;
                        smnuAddFile.Enabled = false;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 响应菜单快捷键
        /// </summary>
        private void RespondShortcutKeys(object sender, KeyEventArgs e)
        {
            // 不要瞎出声
            e.SuppressKeyPress = true;
            // 不要跟你爸爸 FormMain 抢事件
            e.Handled = false;

            SetSmnus();
            switch (e.KeyCode)
            {
                case Keys.C:
                    if (e.Control)
                    {
                        CopyItem();
                    }
                    break;
                case Keys.Delete:
                    DeleteItem();
                    break;
                case Keys.Enter:
                    OpenItem();
                    break;
                case Keys.F1:
                    SeeProperty();
                    break;
                case Keys.F2:
                    RenameItem();
                    break;
                case Keys.F5:
                    RefreshItems();
                    break;
                case Keys.V:
                    if (e.Control)
                    {
                        PasteItem();
                    }
                    break;
                case Keys.X:
                    if (e.Control)
                    {
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
                    if (((Item)tvwFiles.SelectedNode).Type == ItemType.File)
                    {
                        MainForm.GetInstance().EditNode(tvwFiles.SelectedNode);
                    }
                    else
                    {
                        //if (tvwFiles.SelectedNode.IsExpanded)
                        //    tvwFiles.SelectedNode.Collapse(true);
                        //else
                        //    tvwFiles.SelectedNode.Expand();
                    }
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
                        Lang("filetree_properties_path") + ": " + tvwFiles.SelectedNode.GetFilePath() + "\n"
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
                    if (tvwFiles.SelectedNode.IsFile())
                    {
                        name = GetAvailableDirName(tvwFiles.SelectedNode.GetFilePath() + "\\new_folder");
                        node = tvwFiles.SelectedNode.Parent.Nodes.Add(name);
                        tvwFiles.SelectedNode.Parent.Expand();
                    }
                    else
                    {
                        name = GetAvailableDirName(tvwFiles.SelectedNode.GetFilePath() + "\\new_folder");
                        node = tvwFiles.SelectedNode.Nodes.Add(name);
                        tvwFiles.SelectedNode.Expand();
                    }

                    InitializeNode(node, false);

                    try
                    {
                        // 尝试创建目录
                        Directory.CreateDirectory(node.GetFilePath());
                        node.BeginEdit();
                    }
                    catch (Exception ex)
                    {
                        // 出错

                        // 弹窗提示
                        MessageBox.Show(ex.Message);

                        if (!Directory.Exists(node.GetFilePath()))
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
                        tvwFiles.SelectedNode.GetFilePath() + "\\new_file",
                        SelectedItem.GetFileSuffix(true)
                        );
                    if (tvwFiles.SelectedNode.IsFile())
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
                    MainForm.GetInstance().EditedNode = node;

                    try
                    {
                        // 尝试创建文件
                        using (var editor = new Editor())
                        {
                            editor.SetEditor(GetRootParserJson(((Item)node).Sort));
                            File.WriteAllText(node.GetFilePath(), editor.Json);
                        }

                        node.BeginEdit();
                    }
                    catch (Exception ex)
                    {
                        // 出错
                        // 弹窗提示
                        MessageBox.Show(ex.Message);

                        if (!File.Exists(node.GetFilePath()))
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

                            if (File.Exists(tvwFiles.SelectedNode.GetFilePath()))
                                // 删除文件
                                File.Delete(tvwFiles.SelectedNode.GetFilePath());
                            else
                                // 删除目录
                                Directory.Delete(tvwFiles.SelectedNode.GetFilePath(), true);

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
                    SetClipboardList(new[] { tvwFiles.SelectedNode.GetFilePath() }, false);
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
                    SetClipboardList(new[] { tvwFiles.SelectedNode.GetFilePath() }, true);
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
                if (tvwFiles.SelectedNode.IsFile())
                {
                    // 选择的是文件
                    // 粘贴到上一级目录
                    destination = tvwFiles.SelectedNode.Parent.GetFilePath();
                }
                else
                {
                    // 选择的是目录
                    // 粘贴到本级下面
                    destination = tvwFiles.SelectedNode.GetFilePath();
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
                string selectedFullPath = null;

                if (tvwFiles.SelectedNode != null)
                    // 记录当前选中的节点信息
                    selectedFullPath = tvwFiles.SelectedNode.FullPath;

                // 重新加载文件
                LoadFileTree(DatapackPath);

                if (selectedFullPath != null)
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
        /// 用文件资源管理器打开
        /// </summary>
        private void OpenWithExplorer()
        {
            if (smnuExplorer.Enabled)
            {
                if (tvwFiles.SelectedNode != null)
                {
                    ProcessStartInfo info = new ProcessStartInfo("explorer.exe");
                    if (tvwFiles.SelectedNode.IsFile())
                    {
                        // 选中的是文件
                        // 打开explorer.exe, 将光标定位在此文件
                        info.Arguments = "/e,/select," + tvwFiles.SelectedNode.GetFilePath();
                    }
                    else
                    {
                        // 选中的是文件夹
                        // 以此文件夹作为根目录打开explorer.exe
                        info.Arguments = "/root," + tvwFiles.SelectedNode.GetFilePath();
                    }
                    Process.Start(info);
                }
            }
        }

        #region 事件响应

        private void tvwFiles_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                // 取消命名 后续命名由代码手动实现
                e.CancelEdit = true;

                if (e.Label != null && e.Label != e.Node.Text)
                {
                    // 确实改名了

                    if (!IsNameLegal(e.Label))
                    {
                        MessageBox.Show(Lang("filetree_msgbox_notalegalname").Replace("{0}", e.Label));
                    }
                    else
                    {
                        // 合法文件名

                        var before = e.Node.GetFilePath();
                        var backup = e.Node.Text;
                        var isFile = e.Node.IsFile();
                        e.Node.Text = e.Label;
                        var after = e.Node.GetFilePath();

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
                                // 命名前的文件并不存在 创建命名后的文件
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
            }
            catch (Exception ex)
            {
                ShowMessagebox(ex.Message);
            }
            finally
            {
                if (isAddingFolder)
                {
                    RefreshItems();
                    isAddingFolder = false;
                }
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

        private void tvwFiles_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            OpenItem();
        }
        #endregion

        #endregion
    }
}