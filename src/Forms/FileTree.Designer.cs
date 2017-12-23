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
            this.tvwFiles = new System.Windows.Forms.TreeView();
            this.cmnuItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.smnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuAddDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuAddFile = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuRename = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvwFiles
            // 
            this.tvwFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwFiles.HideSelection = false;
            this.tvwFiles.LabelEdit = true;
            this.tvwFiles.Location = new System.Drawing.Point(0, 0);
            this.tvwFiles.Name = "tvwFiles";
            this.tvwFiles.PathSeparator = "/";
            this.tvwFiles.Size = new System.Drawing.Size(298, 467);
            this.tvwFiles.TabIndex = 0;
            this.tvwFiles.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwFiles_NodeMouseClick);
            // 
            // cmnuItem
            // 
            this.cmnuItem.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmnuItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smnuOpen,
            this.smnuAdd,
            this.smnuCopy,
            this.smnuCut,
            this.smnuPaste,
            this.smnuDelete,
            this.smnuRename,
            this.smnuProperty});
            this.cmnuItem.Name = "cmnuItem";
            this.cmnuItem.Size = new System.Drawing.Size(146, 180);
            // 
            // smnuOpen
            // 
            this.smnuOpen.Name = "smnuOpen";
            this.smnuOpen.Size = new System.Drawing.Size(145, 22);
            this.smnuOpen.Text = "打开";
            this.smnuOpen.Click += new System.EventHandler(this.smnuOpen_Click);
            // 
            // smnuAdd
            // 
            this.smnuAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smnuAddDirectory,
            this.smnuAddFile});
            this.smnuAdd.Name = "smnuAdd";
            this.smnuAdd.Size = new System.Drawing.Size(145, 22);
            this.smnuAdd.Text = "添加";
            // 
            // smnuAddDirectory
            // 
            this.smnuAddDirectory.Name = "smnuAddDirectory";
            this.smnuAddDirectory.Size = new System.Drawing.Size(112, 22);
            this.smnuAddDirectory.Text = "文件夹";
            // 
            // smnuAddFile
            // 
            this.smnuAddFile.Name = "smnuAddFile";
            this.smnuAddFile.Size = new System.Drawing.Size(112, 22);
            this.smnuAddFile.Text = "文件";
            // 
            // smnuCopy
            // 
            this.smnuCopy.Name = "smnuCopy";
            this.smnuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.smnuCopy.Size = new System.Drawing.Size(145, 22);
            this.smnuCopy.Text = "复制";
            // 
            // smnuCut
            // 
            this.smnuCut.Name = "smnuCut";
            this.smnuCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.smnuCut.Size = new System.Drawing.Size(145, 22);
            this.smnuCut.Text = "剪切";
            // 
            // smnuPaste
            // 
            this.smnuPaste.Name = "smnuPaste";
            this.smnuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.smnuPaste.Size = new System.Drawing.Size(145, 22);
            this.smnuPaste.Text = "粘贴";
            // 
            // smnuDelete
            // 
            this.smnuDelete.Name = "smnuDelete";
            this.smnuDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.smnuDelete.Size = new System.Drawing.Size(145, 22);
            this.smnuDelete.Text = "删除";
            // 
            // smnuRename
            // 
            this.smnuRename.Name = "smnuRename";
            this.smnuRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.smnuRename.Size = new System.Drawing.Size(145, 22);
            this.smnuRename.Text = "重命名";
            this.smnuRename.Click += new System.EventHandler(this.smnuRename_Click);
            // 
            // smnuProperty
            // 
            this.smnuProperty.Name = "smnuProperty";
            this.smnuProperty.Size = new System.Drawing.Size(145, 22);
            this.smnuProperty.Text = "属性";
            // 
            // FileTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 467);
            this.Controls.Add(this.tvwFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FileTree";
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
    }
}