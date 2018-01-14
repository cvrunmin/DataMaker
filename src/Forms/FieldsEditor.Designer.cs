namespace DataMaker.Forms
{
    partial class FieldsEditor
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
        /// Required method for Designer support - why can't modify?
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.lblSizeChanger = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Left;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid.Size = new System.Drawing.Size(383, 791);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // lblSizeChanger
            // 
            this.lblSizeChanger.BackColor = System.Drawing.Color.Aqua;
            this.lblSizeChanger.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.lblSizeChanger.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSizeChanger.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSizeChanger.Location = new System.Drawing.Point(383, 0);
            this.lblSizeChanger.Margin = new System.Windows.Forms.Padding(0);
            this.lblSizeChanger.Name = "lblSizeChanger";
            this.lblSizeChanger.Size = new System.Drawing.Size(5, 791);
            this.lblSizeChanger.TabIndex = 2;
            this.lblSizeChanger.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblSizeChanger_MouseDown);
            this.lblSizeChanger.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblSizeChanger_MouseMove);
            this.lblSizeChanger.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblSizeChanger_MouseUp);
            // 
            // PropertyEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 791);
            this.ControlBox = false;
            this.Controls.Add(this.lblSizeChanger);
            this.Controls.Add(this.propertyGrid);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(100, 0);
            this.Name = "PropertyEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Label lblSizeChanger;
    }
}