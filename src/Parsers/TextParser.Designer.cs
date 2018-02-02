namespace DataMaker.Parsers
{
    partial class TextParser
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
            this.comboBoxValue = new DataMaker.BetterControls.BetterComboBox();
            this.SuspendLayout();
            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblKey.Location = new System.Drawing.Point(0, 0);
            this.lblKey.Margin = new System.Windows.Forms.Padding(0);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(0, 20);
            this.lblKey.TabIndex = 0;
            // 
            // comboBoxValue
            // 
            this.comboBoxValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.comboBoxValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.comboBoxValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.comboBoxValue.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.comboBoxValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.comboBoxValue.Location = new System.Drawing.Point(0, 0);
            this.comboBoxValue.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.comboBoxValue.Name = "comboBoxValue";
            this.comboBoxValue.Size = new System.Drawing.Size(326, 25);
            this.comboBoxValue.TabIndex = 2;
            this.comboBoxValue.TextChanged += new System.EventHandler(this.comboBoxValue_TextChanged);
            // 
            // TextParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxValue);
            this.Controls.Add(this.lblKey);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TextParser";
            this.Size = new System.Drawing.Size(326, 25);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblKey;
        private BetterControls.BetterComboBox comboBoxValue;
    }
}
