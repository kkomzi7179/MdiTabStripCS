#region Using

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace MdiTabStripCS.Designer {
	public partial class MdiTabStripDesignerForm : Form {
		private MdiTabTemplateControl withEventsField__template;
		private MdiTabTemplateControl _template {
			get { return this.withEventsField__template; }
			set {
				this.withEventsField__template = value;
				/*if(this.withEventsField__template != null) {
					this.withEventsField__template.TabSelected -= this._template_TabSelected;
				}
				this.withEventsField__template = value;
				if(this.withEventsField__template != null) {
					this.withEventsField__template.TabSelected += this._template_TabSelected;
				}*/
			}
		}
		public MdiTabTemplateControl TabTemplate {
			get { return this._template; }
			set {
				this._template = value;
				this._template.Location = new Point(this.SplitContainer1.Panel1.Width / 2 - this._template.Width / 2, this.SplitContainer1.Panel1.Height / 2 - this._template.Height / 2);
				this.SplitContainer1.Panel1.Controls.Clear();
				this.SplitContainer1.Panel1.Controls.Add(value);
				this.PropertyGrid1.SelectedObject = this._template.ActiveTabTemplate;
			}
		}
		public MdiTabStripDesignerForm() {
			this.InitializeComponent();
			this.withEventsField__template.TabSelected += this._template_TabSelected;
			this.cboTabType.SelectedValueChanged += this.cboTabType_SelectedValueChanged;
		}
		private void cboTabType_SelectedValueChanged(object sender, EventArgs e) {
			if(this._template != null) {
				if(ReferenceEquals(this.cboTabType.SelectedItem, "Active Tab")) {
					this.PropertyGrid1.SelectedObject = this._template.ActiveTabTemplate;
				} else if(ReferenceEquals(this.cboTabType.SelectedItem, "Inactive Tab")) {
					this.PropertyGrid1.SelectedObject = this._template.InactiveTabTemplate;
				} else {
					this.PropertyGrid1.SelectedObject = this._template.MouseOverTabTemplate;
				}
			}
		}
		private void _template_TabSelected(TabSelectedEventArgs e) {
			if(e.TabType == TabType.Active) {
				this.cboTabType.SelectedItem = "Active Tab";
			} else if(e.TabType == TabType.Inactive) {
				this.cboTabType.SelectedItem = "Inactive Tab";
			} else {
				this.cboTabType.SelectedItem = "Mouseover Tab";
			}
		}
	}
}