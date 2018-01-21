namespace DataMaker.Parsers
{
    partial class NumberParser
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblKey = new System.Windows.Forms.Label();
            this.upDownValue = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.upDownValue)).BeginInit();
            this.SuspendLayout();
            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblKey.Location = new System.Drawing.Point(0, 0);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(0, 12);
            this.lblKey.TabIndex = 0;
            // 
            // upDownValue
            // 
            this.upDownValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.upDownValue.Location = new System.Drawing.Point(0, 0);
            this.upDownValue.Margin = new System.Windows.Forms.Padding(0);
            this.upDownValue.Name = "upDownValue";
            this.upDownValue.Size = new System.Drawing.Size(65, 21);
            this.upDownValue.TabIndex = 1;
            this.upDownValue.ValueChanged += new System.EventHandler(this.upDownValue_ValueChanged);
            // 
            // UpDownParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.upDownValue);
            this.Controls.Add(this.lblKey);
            this.Name = "UpDownParser";
            this.Size = new System.Drawing.Size(67, 21);
            ((System.ComponentModel.ISupportInitialize)(this.upDownValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.NumericUpDown upDownValue;
    }
}
