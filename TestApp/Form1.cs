using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestApp
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			this.IsMdiContainer = true;
			this.Width = 800;

			this.toolStrip1.Items.Add(new ToolStripButton("Add new child") {
				Name = "tsbNew"
			});
			
			this.toolStrip1.ItemClicked += (s, e) => {
				if (e.ClickedItem.Name.Equals("tsbNew")) {
					Form frm = new Form();
					frm.MdiParent = this;
					frm.Text = "Child " + this.MdiChildren.Length;
					frm.Show();
				}
			};

			this.MdiChildActivate += (s, e) => {
				var actMdiChild = this.ActiveMdiChild;
				if (actMdiChild != null) {
					this.Text = string.Format("Activated child : {0}", actMdiChild.Text);
				}
			};
		}
	}
}
