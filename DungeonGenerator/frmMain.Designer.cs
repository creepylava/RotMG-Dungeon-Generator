﻿namespace DungeonGenerator {
	partial class frmMain {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.box = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.stepsPane = new System.Windows.Forms.FlowLayoutPanel();
			this.btnNew = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.box)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// box
			// 
			this.box.Location = new System.Drawing.Point(0, 0);
			this.box.Name = "box";
			this.box.Size = new System.Drawing.Size(10, 10);
			this.box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.box.TabIndex = 0;
			this.box.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.box);
			this.panel1.Location = new System.Drawing.Point(12, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(560, 355);
			this.panel1.TabIndex = 1;
			// 
			// stepsPane
			// 
			this.stepsPane.Location = new System.Drawing.Point(121, 376);
			this.stepsPane.Name = "stepsPane";
			this.stepsPane.Size = new System.Drawing.Size(451, 53);
			this.stepsPane.TabIndex = 2;
			// 
			// btnNew
			// 
			this.btnNew.Location = new System.Drawing.Point(12, 373);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(103, 28);
			this.btnNew.TabIndex = 3;
			this.btnNew.Text = "New";
			this.btnNew.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(12, 404);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(103, 25);
			this.label1.TabIndex = 4;
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 441);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnNew);
			this.Controls.Add(this.stepsPane);
			this.Controls.Add(this.panel1);
			this.Name = "frmMain";
			this.Text = "RotMG Dungeon Generator";
			this.Load += new System.EventHandler(this.frmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.box)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox box;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.FlowLayoutPanel stepsPane;
		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.Label label1;
	}
}