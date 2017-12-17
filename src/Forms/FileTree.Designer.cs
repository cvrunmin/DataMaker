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
            this.tvwFiles = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // tvwFiles
            // 
            this.tvwFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwFiles.Location = new System.Drawing.Point(0, 0);
            this.tvwFiles.Name = "tvwFiles";
            this.tvwFiles.PathSeparator = "/";
            this.tvwFiles.Size = new System.Drawing.Size(298, 467);
            this.tvwFiles.TabIndex = 0;
            // 
            // FileTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 467);
            this.Controls.Add(this.tvwFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FileTree";
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.TreeView tvwFiles;
    }
}