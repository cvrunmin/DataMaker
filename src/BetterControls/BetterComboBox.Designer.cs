﻿namespace DataMaker.BetterControls
{
    partial class BetterComboBox
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
            this.components = new System.ComponentModel.Container();
            this.comboBoxContent = new System.Windows.Forms.ComboBox();
            this.timerMatch = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // comboBoxContent
            // 
            this.comboBoxContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxContent.Location = new System.Drawing.Point(0, 0);
            this.comboBoxContent.MaxDropDownItems = 16;
            this.comboBoxContent.Name = "comboBoxContent";
            this.comboBoxContent.Size = new System.Drawing.Size(100, 20);
            this.comboBoxContent.Sorted = true;
            this.comboBoxContent.TabIndex = 0;
            this.comboBoxContent.TextChanged += new System.EventHandler(this.comboBoxContent_TextChanged);
            // 
            // timerMatch
            // 
            this.timerMatch.Interval = 250;
            this.timerMatch.Tick += new System.EventHandler(this.timerMatch_Tick);
            // 
            // BetterComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxContent);
            this.Font = new System.Drawing.Font("宋体", 9F);
            this.Name = "BetterComboBox";
            this.Size = new System.Drawing.Size(100, 19);
            this.Resize += new System.EventHandler(this.BetterComboBox_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxContent;
        private System.Windows.Forms.Timer timerMatch;
    }
}
