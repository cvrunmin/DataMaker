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
            this.lblSizeChanger = new System.Windows.Forms.Label();
            this.cmnuItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvwFiles
            // 
            this.tvwFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvwFiles.Dock = System.Windows.Forms.DockStyle.Right;
            this.tvwFiles.HideSelection = false;
            this.tvwFiles.LabelEdit = true;
            this.tvwFiles.Location = new System.Drawing.Point(5, 0);
            this.tvwFiles.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.tvwFiles.Name = "tvwFiles";
            this.tvwFiles.PathSeparator = "/";
            this.tvwFiles.Size = new System.Drawing.Size(293, 467);
            this.tvwFiles.TabIndex = 0;
            this.tvwFiles.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvwFiles_AfterLabelEdit);
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
            this.smnuAddDirectory.Size = new System.Drawing.Size(152, 22);
            this.smnuAddDirectory.Text = "文件夹";
            this.smnuAddDirectory.Click += new System.EventHandler(this.smnuAddDirectory_Click);
            // 
            // smnuAddFile
            // 
            this.smnuAddFile.Name = "smnuAddFile";
            this.smnuAddFile.Size = new System.Drawing.Size(152, 22);
            this.smnuAddFile.Text = "文件";
            this.smnuAddFile.Click += new System.EventHandler(this.smnuAddFile_Click);
            // 
            // smnuCopy
            // 
            this.smnuCopy.Name = "smnuCopy";
            this.smnuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.smnuCopy.Size = new System.Drawing.Size(145, 22);
            this.smnuCopy.Text = "复制";
            this.smnuCopy.Click += new System.EventHandler(this.smnuCopy_Click);
            // 
            // smnuCut
            // 
            this.smnuCut.Name = "smnuCut";
            this.smnuCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.smnuCut.Size = new System.Drawing.Size(145, 22);
            this.smnuCut.Text = "剪切";
            this.smnuCut.Click += new System.EventHandler(this.smnuCut_Click);
            // 
            // smnuPaste
            // 
            this.smnuPaste.Name = "smnuPaste";
            this.smnuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.smnuPaste.Size = new System.Drawing.Size(145, 22);
            this.smnuPaste.Text = "粘贴";
            this.smnuPaste.Click += new System.EventHandler(this.smnuPaste_Click);
            // 
            // smnuDelete
            // 
            this.smnuDelete.Name = "smnuDelete";
            this.smnuDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.smnuDelete.Size = new System.Drawing.Size(145, 22);
            this.smnuDelete.Text = "删除";
            this.smnuDelete.Click += new System.EventHandler(this.smnuDelete_Click);
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
            this.smnuProperty.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.smnuProperty.Size = new System.Drawing.Size(145, 22);
            this.smnuProperty.Text = "属性";
            this.smnuProperty.Click += new System.EventHandler(this.smnuProperty_Click);
            // 
            // lblSizeChanger
            // 
            this.lblSizeChanger.BackColor = System.Drawing.Color.Aqua;
            this.lblSizeChanger.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.lblSizeChanger.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSizeChanger.Location = new System.Drawing.Point(0, 0);
            this.lblSizeChanger.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblSizeChanger.Name = "lblSizeChanger";
            this.lblSizeChanger.Size = new System.Drawing.Size(5, 467);
            this.lblSizeChanger.TabIndex = 1;
            this.lblSizeChanger.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblSizeChanger_MouseDown);
            this.lblSizeChanger.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblSizeChanger_MouseMove);
            this.lblSizeChanger.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblSizeChanger_MouseUp);
            // 
            // FileTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(298, 467);
            this.ControlBox = false;
            this.Controls.Add(this.lblSizeChanger);
            this.Controls.Add(this.tvwFiles);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(100, 0);
            this.Name = "FileTree";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FileTree_KeyDown);
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
    }
}