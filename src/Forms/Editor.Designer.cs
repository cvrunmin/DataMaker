﻿namespace DataMaker.Forms
{
    partial class Editor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.frameParserRoot = new DataMaker.Parsers.FrameParser();
            this.SuspendLayout();
            // 
            // frameParser1
            // 
            resources.ApplyResources(this.frameParserRoot, "frameParser1");
            this.frameParserRoot.Key = null;
            this.frameParserRoot.Name = "frameParser1";
            // 
            // Editor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.frameParserRoot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private Parsers.FrameParser frameParserRoot;
    }
}