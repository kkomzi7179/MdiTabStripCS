using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace MdiTabStripCS.Designer
{

	partial class MdiTabStripDesignerForm
	{

		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
				base.Dispose(disposing);
			}
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MdiTabStripDesignerForm));
			this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
			this.cboTabType = new System.Windows.Forms.ComboBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.PropertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.Label1 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.PictureBox1 = new System.Windows.Forms.PictureBox();
			this.SplitContainer1.Panel2.SuspendLayout();
			this.SplitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// SplitContainer1
			// 
			this.SplitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.SplitContainer1.BackColor = System.Drawing.Color.White;
			this.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SplitContainer1.Location = new System.Drawing.Point(0, 103);
			this.SplitContainer1.Name = "SplitContainer1";
			// 
			// SplitContainer1.Panel1
			// 
			this.SplitContainer1.Panel1.AutoScroll = true;
			this.SplitContainer1.Panel1.BackColor = System.Drawing.Color.White;
			// 
			// SplitContainer1.Panel2
			// 
			this.SplitContainer1.Panel2.Controls.Add(this.cboTabType);
			this.SplitContainer1.Panel2.Controls.Add(this.Label2);
			this.SplitContainer1.Panel2.Controls.Add(this.PropertyGrid1);
			this.SplitContainer1.Size = new System.Drawing.Size(891, 259);
			this.SplitContainer1.SplitterDistance = 592;
			this.SplitContainer1.SplitterWidth = 5;
			this.SplitContainer1.TabIndex = 0;
			// 
			// cboTabType
			// 
			this.cboTabType.BackColor = System.Drawing.SystemColors.Window;
			this.cboTabType.Dock = System.Windows.Forms.DockStyle.Top;
			this.cboTabType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cboTabType.FormattingEnabled = true;
			this.cboTabType.Items.AddRange(new object[] {
            "Active Tab",
            "Inactive Tab",
            "Mouseover Tab"});
			this.cboTabType.Location = new System.Drawing.Point(0, 18);
			this.cboTabType.Margin = new System.Windows.Forms.Padding(0);
			this.cboTabType.Name = "cboTabType";
			this.cboTabType.Size = new System.Drawing.Size(292, 20);
			this.cboTabType.TabIndex = 4;
			this.cboTabType.Text = "Active Tab";
			// 
			// Label2
			// 
			this.Label2.BackColor = System.Drawing.Color.LightGray;
			this.Label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.Label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.Label2.ForeColor = System.Drawing.Color.Black;
			this.Label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Label2.Location = new System.Drawing.Point(0, 0);
			this.Label2.Margin = new System.Windows.Forms.Padding(0);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(292, 18);
			this.Label2.TabIndex = 5;
			this.Label2.Text = "Tab Properties";
			this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PropertyGrid1
			// 
			this.PropertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.PropertyGrid1.BackColor = System.Drawing.SystemColors.Control;
			this.PropertyGrid1.CommandsVisibleIfAvailable = false;
			this.PropertyGrid1.Location = new System.Drawing.Point(0, 38);
			this.PropertyGrid1.Margin = new System.Windows.Forms.Padding(0);
			this.PropertyGrid1.Name = "PropertyGrid1";
			this.PropertyGrid1.Size = new System.Drawing.Size(292, 219);
			this.PropertyGrid1.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(790, 375);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(87, 21);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(695, 375);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(87, 21);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// Label1
			// 
			this.Label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.Label1.AutoEllipsis = true;
			this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label1.Location = new System.Drawing.Point(26, 66);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(834, 32);
			this.Label1.TabIndex = 3;
			this.Label1.Text = "To edit the properties of a tab select it from the tab properties drop-down or cl" +
				"ick on the tab in the preview window. Move the cursor over the \'Close\' button to" +
				" see how it looks when moused over.";
			// 
			// Label3
			// 
			this.Label3.AutoSize = true;
			this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label3.Location = new System.Drawing.Point(190, 13);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(200, 29);
			this.Label3.TabIndex = 4;
			this.Label3.Text = "MdiTab Designer";
			// 
			// PictureBox1
			// 
			this.PictureBox1.Image = global::MdiTabStripCS.Properties.Resources.TabDesigner;
			this.PictureBox1.InitialImage = null;
			this.PictureBox1.Location = new System.Drawing.Point(14, 8);
			this.PictureBox1.Name = "PictureBox1";
			this.PictureBox1.Size = new System.Drawing.Size(145, 30);
			this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.PictureBox1.TabIndex = 5;
			this.PictureBox1.TabStop = false;
			// 
			// MdiTabStripDesignerForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(891, 407);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.PictureBox1);
			this.Controls.Add(this.Label3);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.SplitContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MdiTabStripDesignerForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MdiTab Designer";
			this.SplitContainer1.Panel2.ResumeLayout(false);
			this.SplitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		internal System.Windows.Forms.SplitContainer SplitContainer1;
		internal System.Windows.Forms.PropertyGrid PropertyGrid1;
		internal System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.ComboBox cboTabType;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.PictureBox PictureBox1;
	}
}
