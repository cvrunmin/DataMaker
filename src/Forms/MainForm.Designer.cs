namespace DataMaker
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuTop = new System.Windows.Forms.MenuStrip();
            this.smnuDataPack = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuLoadFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new DataMaker.BetterControls.BetterToolStripSeparator();
            this.smnuSaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new DataMaker.BetterControls.BetterToolStripSeparator();
            this.smnuUnzip = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuExportZip = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new DataMaker.BetterControls.BetterToolStripSeparator();
            this.smnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.smnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.menuTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuTop
            // 
            resources.ApplyResources(this.menuTop, "menuTop");
            this.menuTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smnuDataPack,
            this.smnuHelp});
            this.menuTop.Name = "menuTop";
            // 
            // smnuDataPack
            // 
            resources.ApplyResources(this.smnuDataPack, "smnuDataPack");
            this.smnuDataPack.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smnuLoadFolder,
            this.toolStripSeparator1,
            this.smnuSaveFile,
            this.toolStripSeparator2,
            this.smnuUnzip,
            this.smnuExportZip,
            this.toolStripSeparator3,
            this.smnuExit});
            this.smnuDataPack.Name = "smnuDataPack";
            // 
            // smnuLoadFolder
            // 
            resources.ApplyResources(this.smnuLoadFolder, "smnuLoadFolder");
            this.smnuLoadFolder.Name = "smnuLoadFolder";
            this.smnuLoadFolder.Click += new System.EventHandler(this.smnuLoadFolder_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // smnuSaveFile
            // 
            resources.ApplyResources(this.smnuSaveFile, "smnuSaveFile");
            this.smnuSaveFile.Name = "smnuSaveFile";
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // smnuUnzip
            // 
            resources.ApplyResources(this.smnuUnzip, "smnuUnzip");
            this.smnuUnzip.Name = "smnuUnzip";
            // 
            // smnuExportZip
            // 
            resources.ApplyResources(this.smnuExportZip, "smnuExportZip");
            this.smnuExportZip.Name = "smnuExportZip";
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // smnuExit
            // 
            resources.ApplyResources(this.smnuExit, "smnuExit");
            this.smnuExit.Name = "smnuExit";
            this.smnuExit.Click += new System.EventHandler(this.smnuExit_Click);
            // 
            // smnuHelp
            // 
            resources.ApplyResources(this.smnuHelp, "smnuHelp");
            this.smnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smnuAbout});
            this.smnuHelp.Name = "smnuHelp";
            // 
            // smnuAbout
            // 
            resources.ApplyResources(this.smnuAbout, "smnuAbout");
            this.smnuAbout.Name = "smnuAbout";
            this.smnuAbout.Click += new System.EventHandler(this.smnuAbout_Click);
            // 
            // folderBrowser
            // 
            resources.ApplyResources(this.folderBrowser, "folderBrowser");
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuTop);
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuTop;
            this.Name = "Main";
            this.Text = this.ProductName;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.RespondShortcutKeys);
            this.menuTop.ResumeLayout(false);
            this.menuTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuTop;
        private System.Windows.Forms.ToolStripMenuItem smnuDataPack;
        private System.Windows.Forms.ToolStripMenuItem smnuUnzip;
        private System.Windows.Forms.ToolStripMenuItem smnuLoadFolder;
        private System.Windows.Forms.ToolStripMenuItem smnuExit;
        private BetterControls.BetterToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem smnuExportZip;
        private BetterControls.BetterToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem smnuSaveFile;
        private BetterControls.BetterToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem smnuHelp;
        private System.Windows.Forms.ToolStripMenuItem smnuAbout;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
    }
}

