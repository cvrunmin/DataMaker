namespace DataMaker.Forms
{
    partial class JsonEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JsonEditor));
            this.fctbJson = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.fctbJson)).BeginInit();
            this.SuspendLayout();
            // 
            // fctbJson
            // 
            this.fctbJson.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fctbJson.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.fctbJson.BackBrush = null;
            this.fctbJson.CharHeight = 14;
            this.fctbJson.CharWidth = 8;
            this.fctbJson.CommentPrefix = "";
            this.fctbJson.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctbJson.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctbJson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fctbJson.ImeMode = System.Windows.Forms.ImeMode.On;
            this.fctbJson.IsReplaceMode = false;
            this.fctbJson.LeftBracket = '{';
            this.fctbJson.LeftBracket2 = '[';
            this.fctbJson.Location = new System.Drawing.Point(0, 0);
            this.fctbJson.Name = "fctbJson";
            this.fctbJson.Paddings = new System.Windows.Forms.Padding(0);
            this.fctbJson.RightBracket = '}';
            this.fctbJson.RightBracket2 = ']';
            this.fctbJson.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctbJson.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbJson.ServiceColors")));
            this.fctbJson.ShowFoldingLines = true;
            this.fctbJson.Size = new System.Drawing.Size(893, 512);
            this.fctbJson.TabIndex = 0;
            this.fctbJson.Zoom = 100;
            this.fctbJson.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fctbJson_TextChanged);
            this.fctbJson.AutoIndentNeeded += new System.EventHandler<FastColoredTextBoxNS.AutoIndentEventArgs>(this.fctbJson_AutoIndentNeeded);
            // 
            // JsonEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 512);
            this.ControlBox = false;
            this.Controls.Add(this.fctbJson);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.Name = "JsonEditor";
            ((System.ComponentModel.ISupportInitialize)(this.fctbJson)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox fctbJson;
    }
}