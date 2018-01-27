namespace DataMaker.Parsers
{
    partial class BooleanParser
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
            this.checkBoxValue = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBoxValue
            // 
            this.checkBoxValue.Location = new System.Drawing.Point(-2, 0);
            this.checkBoxValue.Name = "checkBoxValue";
            this.checkBoxValue.Size = new System.Drawing.Size(78, 16);
            this.checkBoxValue.TabIndex = 1;
            this.checkBoxValue.Text = "checkBox1";
            this.checkBoxValue.UseVisualStyleBackColor = true;
            this.checkBoxValue.CheckedChanged += new System.EventHandler(this.checkBoxValue_CheckedChanged);
            // 
            // BooleanParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxValue);
            this.Name = "BooleanParser";
            this.Size = new System.Drawing.Size(99, 18);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxValue;
    }
}
