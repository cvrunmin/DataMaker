namespace DataMaker
{
    partial class FileTree
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileTree));
            this.tvwFiles = new System.Windows.Forms.TreeView();
            this.cmnuItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.smnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuOpenWith = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuVisualizationEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuJsonEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuFunctionEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuAddDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuAddFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.smnuRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.smnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.smnuProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.lblSizeChanger = new System.Windows.Forms.Label();
            this.cmnuItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvwFiles
            // 
            this.tvwFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tvwFiles, "tvwFiles");
            this.tvwFiles.HideSelection = false;
            this.tvwFiles.LabelEdit = true;
            this.tvwFiles.Name = "tvwFiles";
            this.tvwFiles.PathSeparator = "/";
            this.tvwFiles.ShowPlusMinus = false;
            this.tvwFiles.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvwFiles_AfterLabelEdit);
            this.tvwFiles.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvwFiles_AfterCollapse);
            this.tvwFiles.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ShowMenu);
            // 
            // cmnuItem
            // 
            this.cmnuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cmnuItem.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmnuItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smnuOpen,
            this.smnuOpenWith,
            this.smnuAdd,
            this.toolStripSeparator3,
            this.smnuRefresh,
            this.toolStripSeparator1,
            this.smnuCopy,
            this.smnuCut,
            this.smnuPaste,
            this.smnuDelete,
            this.smnuRename,
            this.toolStripSeparator2,
            this.smnuProperty});
            this.cmnuItem.Name = "cmnuItem";
            resources.ApplyResources(this.cmnuItem, "cmnuItem");
            // 
            // smnuOpen
            // 
            resources.ApplyResources(this.smnuOpen, "smnuOpen");
            this.smnuOpen.Name = "smnuOpen";
            this.smnuOpen.Click += new System.EventHandler(this.smnuOpen_Click);
            // 
            // smnuOpenWith
            // 
            resources.ApplyResources(this.smnuOpenWith, "smnuOpenWith");
            this.smnuOpenWith.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smnuVisualizationEditor,
            this.smnuJsonEditor,
            this.smnuFunctionEditor,
            this.smnuExplorer});
            this.smnuOpenWith.Name = "smnuOpenWith";
            // 
            // smnuVisualizationEditor
            // 
            resources.ApplyResources(this.smnuVisualizationEditor, "smnuVisualizationEditor");
            this.smnuVisualizationEditor.Name = "smnuVisualizationEditor";
            this.smnuVisualizationEditor.Click += new System.EventHandler(this.smnuVisualizationEditor_Click);
            // 
            // smnuJsonEditor
            // 
            resources.ApplyResources(this.smnuJsonEditor, "smnuJsonEditor");
            this.smnuJsonEditor.Name = "smnuJsonEditor";
            this.smnuJsonEditor.Click += new System.EventHandler(this.smnuJsonEditor_Click);
            // 
            // smnuFunctionEditor
            // 
            resources.ApplyResources(this.smnuFunctionEditor, "smnuFunctionEditor");
            this.smnuFunctionEditor.Name = "smnuFunctionEditor";
            this.smnuFunctionEditor.Click += new System.EventHandler(this.smnuFunctionEditor_Click);
            // 
            // smnuExplorer
            // 
            resources.ApplyResources(this.smnuExplorer, "smnuExplorer");
            this.smnuExplorer.Name = "smnuExplorer";
            this.smnuExplorer.Click += new System.EventHandler(this.smnuExplorer_Click);
            // 
            // smnuAdd
            // 
            resources.ApplyResources(this.smnuAdd, "smnuAdd");
            this.smnuAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smnuAddDirectory,
            this.smnuAddFile});
            this.smnuAdd.Name = "smnuAdd";
            // 
            // smnuAddDirectory
            // 
            resources.ApplyResources(this.smnuAddDirectory, "smnuAddDirectory");
            this.smnuAddDirectory.Name = "smnuAddDirectory";
            this.smnuAddDirectory.Click += new System.EventHandler(this.smnuAddDirectory_Click);
            // 
            // smnuAddFile
            // 
            resources.ApplyResources(this.smnuAddFile, "smnuAddFile");
            this.smnuAddFile.Name = "smnuAddFile";
            this.smnuAddFile.Click += new System.EventHandler(this.smnuAddFile_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // smnuRefresh
            // 
            resources.ApplyResources(this.smnuRefresh, "smnuRefresh");
            this.smnuRefresh.Name = "smnuRefresh";
            this.smnuRefresh.Click += new System.EventHandler(this.smnuRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // smnuCopy
            // 
            resources.ApplyResources(this.smnuCopy, "smnuCopy");
            this.smnuCopy.Name = "smnuCopy";
            this.smnuCopy.Click += new System.EventHandler(this.smnuCopy_Click);
            // 
            // smnuCut
            // 
            resources.ApplyResources(this.smnuCut, "smnuCut");
            this.smnuCut.Name = "smnuCut";
            this.smnuCut.Click += new System.EventHandler(this.smnuCut_Click);
            // 
            // smnuPaste
            // 
            resources.ApplyResources(this.smnuPaste, "smnuPaste");
            this.smnuPaste.Name = "smnuPaste";
            this.smnuPaste.Click += new System.EventHandler(this.smnuPaste_Click);
            // 
            // smnuDelete
            // 
            resources.ApplyResources(this.smnuDelete, "smnuDelete");
            this.smnuDelete.Name = "smnuDelete";
            this.smnuDelete.Click += new System.EventHandler(this.smnuDelete_Click);
            // 
            // smnuRename
            // 
            resources.ApplyResources(this.smnuRename, "smnuRename");
            this.smnuRename.Name = "smnuRename";
            this.smnuRename.Click += new System.EventHandler(this.smnuRename_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // smnuProperty
            // 
            resources.ApplyResources(this.smnuProperty, "smnuProperty");
            this.smnuProperty.Name = "smnuProperty";
            this.smnuProperty.Click += new System.EventHandler(this.smnuProperty_Click);
            // 
            // lblSizeChanger
            // 
            this.lblSizeChanger.BackColor = System.Drawing.Color.Aqua;
            this.lblSizeChanger.Cursor = System.Windows.Forms.Cursors.SizeWE;
            resources.ApplyResources(this.lblSizeChanger, "lblSizeChanger");
            this.lblSizeChanger.Name = "lblSizeChanger";
            this.lblSizeChanger.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblSizeChanger_MouseDown);
            this.lblSizeChanger.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblSizeChanger_MouseMove);
            this.lblSizeChanger.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblSizeChanger_MouseUp);
            // 
            // FileTree
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ControlBox = false;
            this.Controls.Add(this.lblSizeChanger);
            this.Controls.Add(this.tvwFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FileTree";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RespondShortcutKeys);
            this.cmnuItem.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.TreeView tvwFiles;
        private System.Windows.Forms.ContextMenuStrip cmnuItem;
        private System.Windows.Forms.ToolStripMenuItem smnuOpen;
        private System.Windows.Forms.ToolStripMenuItem smnuAdd;
        private System.Windows.Forms.ToolStripMenuItem smnuAddDirectory;
        private System.Windows.Forms.ToolStripMenuItem smnuAddFile;
        private System.Windows.Forms.ToolStripMenuItem smnuCopy;
        private System.Windows.Forms.ToolStripMenuItem smnuCut;
        private System.Windows.Forms.ToolStripMenuItem smnuPaste;
        private System.Windows.Forms.ToolStripMenuItem smnuRename;
        private System.Windows.Forms.ToolStripMenuItem smnuProperty;
        private System.Windows.Forms.ToolStripMenuItem smnuDelete;
        private System.Windows.Forms.Label lblSizeChanger;
        private System.Windows.Forms.ToolStripMenuItem smnuOpenWith;
        private System.Windows.Forms.ToolStripMenuItem smnuVisualizationEditor;
        private System.Windows.Forms.ToolStripMenuItem smnuJsonEditor;
        private System.Windows.Forms.ToolStripMenuItem smnuFunctionEditor;
        private System.Windows.Forms.ToolStripMenuItem smnuExplorer;
        private System.Windows.Forms.ToolStripMenuItem smnuRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}