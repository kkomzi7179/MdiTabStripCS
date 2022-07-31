using RuntimeProperty;

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

			this.toolStrip1.Items.Add(new ToolStripButton("Add new child")
			{
				Name = "tsbNew"
			});
			this.toolStrip1.Items.Add(new ToolStripButton("Runtime Property")
			{
				Name = "runTimeProperty"
			});

			this.toolStrip1.ItemClicked += (s, e) => {
				if (e.ClickedItem.Name.Equals("tsbNew"))
				{
					Form frm = new Form();
					frm.Controls.Add(new Button()
					{
						Name = Text = "test" + this.MdiChildren.Length
					});
					frm.Name = frm.Text = "Child " + this.MdiChildren.Length;
					frm.MdiParent = this;
					frm.Show();
				} else if (e.ClickedItem.Name.Equals("runTimeProperty"))
				{
					this.ShowProperty();
				}
			};

			this.MdiChildActivate += (s, e) => {
				var actMdiChild = this.ActiveMdiChild;
				if (actMdiChild != null)
				{
					this.Text = string.Format("Activated child : {0}", actMdiChild.Text);
				}
			};
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.Control | Keys.Shift | Keys.P))
			{
				this.ShowProperty();
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
