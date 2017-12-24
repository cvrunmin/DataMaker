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
    /* TODO: AddFile的已有重命名
     * Paste的已有重命名
     */


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
        JsonFile,
        MCFunctionFile,
        MCMetaFile,
        File
    }

    public partial class FileTree : Form
    {
        /// <summary>
        /// 数据包路径
        /// </summary>
        private string packPath;

        #region 定义函数
        /// <summary>
        /// 通过节点获取该节点对应的文件路径
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <param name="isSuccessful">获取是否成功</param>
        /// <returns>文件路径</returns>
        private string GetPathFromNode(TreeNode node, out bool isSuccessful)
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
            result = result.Replace(Lang("/设置"), "/pack");
            result = result.Replace(Lang("/"), "\\");
            // 加后缀
            result = result.Insert(result.Length, GetFileSuffix(node));

            if (Directory.Exists(result))
                isSuccessful = true;
            else
                isSuccessful = false;

            return result;
        }

        /// <summary>
        /// 通过节点获取该节点对应的文件路径
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <returns>文件路径</returns>
        private string GetPathFromNode(TreeNode node) 
            => GetPathFromNode(node, out var isSuccessful);

        /// <summary>
        /// 根据节点类型返回文件(或其子文件)的后缀名
        /// <para><seealso cref="GetFileSuffix(NodeType)"/></para>
        /// <para><seealso cref="GetFileSuffix(TreeNode)"/></para>
        /// <para><seealso cref="GetFileOrSubFileSuffix(TreeNode)"/></para>
        /// </summary>
        /// <param name="node">节点类型</param>
        /// <returns>文件(或其子文件)的后缀名</returns>
        private string GetFileOrSubFileSuffix(NodeType type)
        {
            string result;

            switch (type)
            {
                case NodeType.File:
                case NodeType.JsonFile:
                case NodeType.Advancements:
                case NodeType.AdvancementsRoot:
                case NodeType.LootTables:
                case NodeType.LootTablesRoot:
                case NodeType.Recipes:
                case NodeType.RecipesRoot:
                case NodeType.Tags:
                case NodeType.TagsRoot:
                    result = ".json";
                    break;
                case NodeType.MCFunctionFile:
                case NodeType.Functions:
                case NodeType.FunctionsRoot:
                    result = ".mcfunction";
                    break;
                case NodeType.MCMetaFile:
                    result = ".mcmeta";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }

        /// <summary>
        /// 根据节点返回文件(或其子文件)的后缀名
        /// <para><seealso cref="GetFileSuffix(NodeType)"/></para>
        /// <para><seealso cref="GetFileSuffix(TreeNode)"/></para>
        /// <para><seealso cref="GetFileOrSubFileSuffix(NodeType)"/></para>
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>文件(或其子文件)的后缀名</returns>
        private string GetFileOrSubFileSuffix(TreeNode node) 
            => GetFileOrSubFileSuffix((NodeType)node.Tag);

        /// <summary>
        /// 根据节点类型返回文件的后缀名
        /// <para><seealso cref="GetFileOrSubFileSuffix(NodeType)"/></para>
        /// <para><seealso cref="GetFileOrSubFileSuffix(TreeNode)"/></para>
        /// <para><seealso cref="GetFileSuffix(TreeNode)"/></para>
        /// </summary>
        /// <param name="node">节点类型</param>
        /// <returns>文件后缀名</returns>
        private string GetFileSuffix(NodeType type)
        {
            string result;

            switch (type)
            {
                case NodeType.File:
                case NodeType.JsonFile:
                    result = ".json";
                    break;
                case NodeType.MCFunctionFile:
                    result = ".mcfunction";
                    break;
                case NodeType.MCMetaFile:
                    result = ".mcmeta";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }

        /// <summary>
        /// 根据节点返回文件的后缀名
        /// <para><seealso cref="GetFileOrSubFileSuffix(NodeType)"/></para>
        /// <para><seealso cref="GetFileOrSubFileSuffix(TreeNode)"/></para>
        /// <para><seealso cref="GetFileSuffix(NodeType)"/></para>
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>文件后缀名</returns>
        private string GetFileSuffix(TreeNode node)
            => GetFileSuffix((NodeType)node.Tag);

        /// <summary>
        /// 根据节点类型返回该项是否是文件
        /// </summary>
        /// <param name="type">指定节点类型</param>
        /// <returns></returns>
        private bool IsFile(NodeType type)
        {
            switch (type)
            {
                case NodeType.File:
                case NodeType.JsonFile:
                case NodeType.MCMetaFile:
                case NodeType.MCFunctionFile:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 根据节点返回该项是否是文件
        /// </summary>
        /// <param name="type">指定节点</param>
        /// <returns></returns>
        private bool IsFile(TreeNode node)
            => IsFile((NodeType)node.Tag);

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

            // 获取DropEffect
            MemoryStream memo = (MemoryStream)data.GetData("Preferred DropEffect");
            var bytes = new byte[] { 0, 0, 0, 0 };
            memo.Read(bytes, 0, bytes.Length);

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

            StringCollection sc = Clipboard.GetFileDropList();
            List<string> list = new List<string>();

            for (int i = 0; i < sc.Count; i++)
            {
                string listfileName = sc[i];
                list.Add(listfileName);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 根据目录和文件名获取可用的文件名
        /// </summary>
        private string GetAvailableFileName(string path)
        {
            var file = path.Remove(0, path.LastIndexOf("\\") + 1);
            var dir = path.Remove(path.Length - file.Length - 1, path.Length - path.LastIndexOf("\\"));

            var result = file;

            for (int i = 1; File.Exists(dir + "\\" + result); i++)
                result = file + "_" + i;

            return result;
        }

        ///// <summary>
        ///// 根据节点获取可用的文件名
        ///// </summary>
        //private string GetAvailableFileName(TreeNode node)
        //    => GetAvailableFileName(GetPathFromNode(node));

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

        ///// <summary>
        ///// 根据节点获取可用的目录名
        ///// </summary>
        //private string GetAvailableDirName(TreeNode node)
        //    => GetAvailableDirName(GetPathFromNode(node));

        #endregion

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
            InitializeNode(rootNode, false);
            LoadDirectory(path, rootNode);
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
                var dirName = dir.LastIndexOf(@"\") + 1;
                var dirNode = node.Nodes.Add(dir.Remove(0, dirName));
                InitializeNode(dirNode, false);
                // 遍历
                LoadDirectory(dir, dirNode);
            }

            // 读取文件
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var fileName = file.Remove(0, file.LastIndexOf(@"\") + 1);
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

                if (node.Text.EndsWith(".json"))
                {
                    node.ImageKey = node.SelectedImageKey = "Json";
                    node.Tag = NodeType.JsonFile;
                    node.Text = node.Text.Remove(node.Text.Length - ".json".Length);
                }
                else if (node.Text.EndsWith(".mcfunction"))
                {
                    node.ImageKey = node.SelectedImageKey = "Function";
                    node.Tag = NodeType.MCFunctionFile;
                    node.Text = node.Text.Remove(node.Text.Length - ".mcfunction".Length);
                }
                else if (node.Level == 1 && node.Text.Equals("pack.mcmeta"))
                {
                    node.ImageKey = node.SelectedImageKey = "Setting";
                    node.Tag = NodeType.MCMetaFile;
                    node.Text = Lang("设置");
                }
                else
                    node.Tag = NodeType.File;
            }
            else
            {
                switch (node.Level)
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
            if (tvwFiles.SelectedNode != null)
            {
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
                        Lang("名称") + ": " + tvwFiles.SelectedNode.Text + "\n" +
                        Lang("类型") + ": " + tvwFiles.SelectedNode.Tag + "\n" +
                        Lang("路径") + ": " + GetPathFromNode(tvwFiles.SelectedNode) + "\n"
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
                    TreeNode node;
                    string name;
                    if (IsFile(tvwFiles.SelectedNode))
                    {
                        name = GetAvailableFileName(GetPathFromNode(tvwFiles.SelectedNode) + "\\new_folder");
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
                    { // 尝试创建目录
                        Directory.CreateDirectory(GetPathFromNode(node));
                        node.BeginEdit();
                    }
                    catch (Exception ex)
                    { // 出错
                        // 弹窗提示
                        MessageBox.Show(ex.Message);

                        if (!Directory.Exists(GetPathFromNode(node)))
                        { // 目录创建失败
                            // 把节点删除
                            tvwFiles.Nodes.Remove(node);
                        }
                    }
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

                    if (IsFile(tvwFiles.SelectedNode))
                    {   // 在文件级别右键

                        // 在父级目录下增加节点
                        node = tvwFiles.SelectedNode.Parent.Nodes.Add("new_file_" +
                            Environment.TickCount +
                            GetFileOrSubFileSuffix(tvwFiles.SelectedNode));

                        // 展开父级目录
                        tvwFiles.SelectedNode.Parent.Expand();
                    }
                    else
                    {   // 在目录级别右键

                        // 在本级目录下增加节点
                        node = tvwFiles.SelectedNode.Nodes.Add("new_file_" +
                            Environment.TickCount +
                            GetFileOrSubFileSuffix(tvwFiles.SelectedNode));

                        // 展开本级目录
                        tvwFiles.SelectedNode.Expand();
                    }

                    InitializeNode(node, true);

                    try
                    { // 尝试创建文件
                        File.WriteAllText(GetPathFromNode(node), "");
                        node.BeginEdit();
                    }
                    catch (Exception ex)
                    { // 出错
                        // 弹窗提示
                        MessageBox.Show(ex.Message);

                        if (!File.Exists(GetPathFromNode(node)))
                        { // 文件创建失败
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
                    var result = MessageBox.Show(this, Lang("你确定要删除 ") + tvwFiles.SelectedNode.Text + " ?", 
                        Application.ProductName, MessageBoxButtons.OKCancel);

                    if (result == DialogResult.OK)
                    {   // 确定删除

                        try
                        { // 尝试删除

                            if ((NodeType)tvwFiles.SelectedNode.Tag == NodeType.File ||
                                (NodeType)tvwFiles.SelectedNode.Tag == NodeType.JsonFile ||
                                (NodeType)tvwFiles.SelectedNode.Tag == NodeType.MCFunctionFile ||
                                (NodeType)tvwFiles.SelectedNode.Tag == NodeType.MCMetaFile)
                            {   // 删除文件
                                File.Delete(GetPathFromNode(tvwFiles.SelectedNode));
                            }
                            else
                            {   // 删除目录
                                Directory.Delete(GetPathFromNode(tvwFiles.SelectedNode), true);
                            }

                            // 移除节点
                            tvwFiles.Nodes.Remove(tvwFiles.SelectedNode);

                        }
                        catch (Exception ex)
                        { // 出错

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
                    var list = GetClipboardList(out var isCut);

                    Debug.Print("vvv - IsCut: " + isCut.ToString());
                    foreach (var i in list)
                    {
                        Debug.Print(i);

                        string destination;

                        if (IsFile(tvwFiles.SelectedNode))
                        {   // 选择的是文件
                            // 粘贴到上一级目录
                            destination = GetPathFromNode(tvwFiles.SelectedNode.Parent);
                        }
                        else
                        {   // 选择的是目录
                            // 粘贴到本级下面
                            destination = GetPathFromNode(tvwFiles.SelectedNode);
                        }
                        destination += "\\";
                        destination += i.Remove(0, i.LastIndexOf("\\"));

                        if (isCut)
                        {   // 剪切
                            File.Move(i, destination);
                        }
                        else
                        {   // 复制
                            File.Copy(i, destination);
                        }

                    }
                    Debug.Print("^^^");
                }
            }
        }

        #region 事件响应

        private void tvwFiles_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // 取消命名 后续命名由代码手动实现
            e.CancelEdit = true;

            if (e.Label != null)
            {
                var pattern = @"[^(a-z0-9\-_\.)]";

                if (Regex.IsMatch(e.Label, pattern))
                {
                    /* 
                     * 文件名不符合社会主义核心价值观
                     * 富强、民主、文明、和谐
                     * 自由、平等、公正、法治
                     * 爱国、敬业、诚信、友善
                     */
                    MessageBox.Show(Lang("文件名只能包含abcdefghijklmnopqrstuvwxyz0123456789.-_"));
                }
                else
                {
                    // 合法文件名
                    var before = GetPathFromNode(e.Node);
                    var backup = e.Node.Text;
                    e.Node.Text = e.Label;
                    var after = GetPathFromNode(e.Node);

                    switch ((NodeType)e.Node.Tag)
                    {
                        // 重命名文件
                        case NodeType.File:
                        case NodeType.JsonFile:
                        case NodeType.MCFunctionFile:
                        case NodeType.MCMetaFile:

                            if (File.Exists(after))
                            {
                                // 命名后的文件存在 这可坏了
                                MessageBox.Show(Lang("已存在" + ": " + e.Node.Text));
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

                            break;

                        // 重命名目录
                        default:
                            if (Directory.Exists(after))
                            {
                                // 命名后的目录存在 这可坏了
                                MessageBox.Show(Lang("已存在" + ": " + e.Node.Text));
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
                            break;
                    }
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

        #endregion

        #endregion
    }
}