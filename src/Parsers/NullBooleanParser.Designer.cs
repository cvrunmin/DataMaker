namespace DataMaker.Parsers
{
    partial class NullBooleanParser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NullBooleanParser));
            this.lblKey = new System.Windows.Forms.Label();
            this.rbtnTrue = new System.Windows.Forms.RadioButton();
            this.rbtnFalse = new System.Windows.Forms.RadioButton();
            this.rbtnNull = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // lblKey
            // 
            resources.ApplyResources(this.lblKey, "lblKey");
            this.lblKey.Name = "lblKey";
            // 
            // rbtnTrue
            // 
            resources.ApplyResources(this.rbtnTrue, "rbtnTrue");
            this.rbtnTrue.Name = "rbtnTrue";
            this.rbtnTrue.UseVisualStyleBackColor = true;
            this.rbtnTrue.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // rbtnFalse
            // 
            resources.ApplyResources(this.rbtnFalse, "rbtnFalse");
            this.rbtnFalse.Name = "rbtnFalse";
            this.rbtnFalse.UseVisualStyleBackColor = true;
            this.rbtnFalse.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // rbtnNull
            // 
            this.rbtnNull.Checked = true;
            resources.ApplyResources(this.rbtnNull, "rbtnNull");
            this.rbtnNull.Name = "rbtnNull";
            this.rbtnNull.TabStop = true;
            this.rbtnNull.UseVisualStyleBackColor = true;
            this.rbtnNull.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // NullBooleanParser
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbtnNull);
            this.Controls.Add(this.rbtnFalse);
            this.Controls.Add(this.rbtnTrue);
            this.Controls.Add(this.lblKey);
            this.Name = "NullBooleanParser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.RadioButton rbtnTrue;
        private System.Windows.Forms.RadioButton rbtnFalse;
        private System.Windows.Forms.RadioButton rbtnNull;
    }
}
