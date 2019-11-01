#region Using

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using MdiTabStripCS.Properties;

#endregion

namespace MdiTabStripCS.Designer {
	public class MdiTabStripDesigner : ControlDesigner {
		private DesignerActionListCollection m_actionLists;
		public override DesignerActionListCollection ActionLists {
			get {
				if(this.m_actionLists == null) {
					this.m_actionLists = new DesignerActionListCollection();
					this.m_actionLists.Add(new MdiTabStripDesignerActionList(Component));
				}
				return this.m_actionLists;
			}
		}
		public override void Initialize(IComponent component) {
			base.Initialize(component);
			MdiTabStrip tabStrip = (MdiTabStrip) Control;
			MdiTab activeTab = new MdiTab(tabStrip);
			MdiTab inactiveTab = new MdiTab(tabStrip);
			MdiTab mouseOverTab = new MdiTab(tabStrip);
			tabStrip.LeftScrollTab.Visible = true;
			tabStrip.RightScrollTab.Visible = true;
			tabStrip.DropDownTab.Visible = true;
			activeTab.Form = new Form1();
			tabStrip.ActiveTab = activeTab;
			tabStrip.Tabs.Add(activeTab);
			inactiveTab.Form = new Form2();
			tabStrip.Tabs.Add(inactiveTab);
			mouseOverTab.Form = new Form3();
			mouseOverTab.IsMouseOver = true;
			tabStrip.Tabs.Add(mouseOverTab);
			tabStrip.PerformLayout();
		}

		private class Form1 : Form {
			public Form1() {
				Text = "Active Tab";
				Icon = Resources.document;
			}
		}

		private class Form2 : Form {
			public Form2() {
				Text = "Inactive Tab";
				Icon = Resources.document;
			}
		}

		private class Form3 : Form {
			public Form3() {
				Text = "Moused Over Tab";
				Icon = Resources.document;
			}
		}
	}

	public class MdiTabStripDesignerActionList : DesignerActionList {
		private DesignerActionUIService _uiService;
		private DesignerActionItemCollection _actionItems;
		public MdiTabStripDesignerActionList(IComponent component) : base(component) { this._uiService = (DesignerActionUIService) GetService(typeof(DesignerActionUIService)); }
		public override DesignerActionItemCollection GetSortedActionItems() {
			if(this._actionItems == null) {
				this._actionItems = new DesignerActionItemCollection();
				if(this.TabStrip != null) {
					this._actionItems.Add(new DesignerActionMethodItem(this, "OpenInactiveTabEditor", "Design Tabs", "Appearance", "Opens the MdiTab Designer window."));
					this._actionItems.Add(new DesignerActionHeaderItem("Behavior"));
					this._actionItems.Add(new DesignerActionPropertyItem("TabPermanence", "Tab permanence", this.GetCategory(this.TabStrip, "TabPermanence"), this.GetDescription(this.TabStrip, "TabPermanence")));
					this._actionItems.Add(new DesignerActionPropertyItem("Animate", "Perform fade animation on mouse over", this.GetCategory(this.TabStrip, "Animate"), this.GetDescription(this.TabStrip, "Animate")));
					this._actionItems.Add(new DesignerActionPropertyItem("DisplayFormIcon", "Display the form icon", "Behavior", this.GetDescription(this.TabStrip, "DisplayFormIcon")));
					this._actionItems.Add(new DesignerActionPropertyItem("MdiNewTabVisible", "Display the new tab", "Behavior", this.GetDescription(this.TabStrip, "MdiNewTabVisible")));
					this._actionItems.Add(new DesignerActionHeaderItem("Layout"));
					this._actionItems.Add(new DesignerActionPropertyItem("MinTabWidth", "Minimum tab width", this.GetCategory(this.TabStrip, "MinTabWidth"), this.GetDescription(this.TabStrip, "MinTabWidth")));
					this._actionItems.Add(new DesignerActionPropertyItem("MaxTabWidth", "Maximum tab width", this.GetCategory(this.TabStrip, "MaxTabWidth"), this.GetDescription(this.TabStrip, "MaxTabWidth")));
					this._actionItems.Add(new DesignerActionPropertyItem("MdiWindowState",
						"Mdi form window state",
						this.GetCategory(this.TabStrip, "MdiWindowState"),
						this.GetDescription(this.TabStrip, "MdiWindowState")));
				}
			}
			return this._actionItems;
		}
		public MdiTabStrip TabStrip { get { return (MdiTabStrip) base.Component; } }
		private void SetProperty(string propertyName, object value) {
			PropertyDescriptor prop = TypeDescriptor.GetProperties(this.TabStrip)[propertyName];
			prop.SetValue(this.TabStrip, value);
		}
		private string GetCategory(object source, string propertyName) {
			PropertyInfo prop = source.GetType().GetProperty(propertyName);
			object[] attributes = prop.GetCustomAttributes(typeof(CategoryAttribute), false);
			if(attributes.Length == 0) {
				return null;
			}
			CategoryAttribute attr = (CategoryAttribute) attributes[0];
			if(attr == null) {
				return null;
			}
			return attr.Category;
		}
		private string GetDescription(object source, string propertyName) {
			PropertyInfo prop = source.GetType().GetProperty(propertyName);
			object[] attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if(attributes.Length == 0) {
				return null;
			}
			DescriptionAttribute attr = (DescriptionAttribute) attributes[0];
			if(attr == null) {
				return null;
			}
			return attr.Description;
		}

		#region MdiTabStrip Properties

		public Color ActiveTabColor { get { return this.TabStrip.ActiveTabColor; } set { this.SetProperty("ActiveTabColor", value); } }
		public Color ActiveTabForeColor { get { return this.TabStrip.ActiveTabForeColor; } set { this.SetProperty("ActiveTabForeColor", value); } }
		public Font ActiveTabFont { get { return this.TabStrip.ActiveTabFont; } set { this.SetProperty("ActiveTabFont", value); } }
		public Color CloseButtonBackColor { get { return this.TabStrip.CloseButtonBackColor; } set { this.SetProperty("CloseButtonBackColor", value); } }
		public Color CloseButtonForeColor { get { return this.TabStrip.CloseButtonForeColor; } set { this.SetProperty("CloseButtonForeColor", value); } }
		public Color CloseButtonHotForeColor { get { return this.TabStrip.CloseButtonHotForeColor; } set { this.SetProperty("CloseButtonHotForeColor", value); } }
		public Color CloseButtonBorderColor { get { return this.TabStrip.CloseButtonBorderColor; } set { this.SetProperty("CloseButtonBorderColor", value); } }
		public Color InactiveTabColor { get { return this.TabStrip.InactiveTabColor; } set { this.SetProperty("InactiveTabColor", value); } }
		public Color InactiveTabForeColor { get { return this.TabStrip.InactiveTabForeColor; } set { this.SetProperty("InactiveTabForeColor", value); } }
		public Font InactiveTabFont { get { return this.TabStrip.InactiveTabFont; } set { this.SetProperty("InactiveTabFont", value); } }
		public Color MouseOverTabColor { get { return this.TabStrip.MouseOverTabColor; } set { this.SetProperty("MouseOverTabColor", value); } }
		public Color MouseOverTabForeColor { get { return this.TabStrip.MouseOverTabForeColor; } set { this.SetProperty("MouseOverTabForeColor", value); } }
		public Font MouseOverTabFont { get { return this.TabStrip.MouseOverTabFont; } set { this.SetProperty("MouseOverTabFont", value); } }
		public Color ActiveTabBorderColor { get { return this.TabStrip.ActiveTabBorderColor; } set { this.SetProperty("ActiveTabBorderColor", value); } }
		public Color InactiveTabBorderColor { get { return this.TabStrip.InactiveTabBorderColor; } set { this.SetProperty("InactiveTabBorderColor", value); } }
		public bool Animate { get { return this.TabStrip.Animate; } set { this.SetProperty("Animate", value); } }
		public MdiTabPermanence TabPermanence { get { return this.TabStrip.TabPermanence; } set { this.SetProperty("TabPermanence", value); } }
		public int MaxTabWidth { get { return this.TabStrip.MaxTabWidth; } set { this.SetProperty("MaxTabWidth", value); } }
		public int MinTabWidth { get { return this.TabStrip.MinTabWidth; } set { this.SetProperty("MinTabWidth", value); } }
		public bool DisplayFormIcon { get { return this.TabStrip.DisplayFormIcon; } set { this.SetProperty("DisplayFormIcon", value); } }
		public MdiChildWindowState MdiWindowState { get { return this.TabStrip.MdiWindowState; } set { this.SetProperty("MdiWindowState", value); } }
		public RightToLeft RightToLeft { get { return this.TabStrip.RightToLeft; } }
		public bool MdiNewTabVisible { get { return this.TabStrip.MdiNewTabVisible; } set { this.SetProperty("MdiNewTabVisible", value); } }

		#endregion

		private void OpenInactiveTabEditor() {
			MdiTabStripDesignerForm editor = new MdiTabStripDesignerForm();
			MdiTabTemplateControl template = new MdiTabTemplateControl();
			template.InactiveTabTemplate.BackColor = this.InactiveTabColor;
			template.InactiveTabTemplate.ForeColor = this.InactiveTabForeColor;
			template.InactiveTabTemplate.Font = this.InactiveTabFont;
			template.InactiveTabTemplate.BorderColor = this.InactiveTabBorderColor;
			template.ActiveTabTemplate.BackColor = this.ActiveTabColor;
			template.ActiveTabTemplate.ForeColor = this.ActiveTabForeColor;
			template.ActiveTabTemplate.Font = this.ActiveTabFont;
			template.ActiveTabTemplate.BorderColor = this.ActiveTabBorderColor;
			template.ActiveTabTemplate.CloseButtonBackColor = this.CloseButtonBackColor;
			template.ActiveTabTemplate.CloseButtonBorderColor = this.CloseButtonBorderColor;
			template.ActiveTabTemplate.CloseButtonForeColor = this.CloseButtonForeColor;
			template.ActiveTabTemplate.CloseButtonHotForeColor = this.CloseButtonHotForeColor;
			template.MouseOverTabTemplate.BackColor = this.MouseOverTabColor;
			template.MouseOverTabTemplate.ForeColor = this.MouseOverTabForeColor;
			template.MouseOverTabTemplate.Font = this.MouseOverTabFont;
			template.RightToLeft = this.RightToLeft;
			editor.TabTemplate = template;
			editor.ShowDialog();
			if(editor.DialogResult == DialogResult.OK) {
				this.InactiveTabColor = editor.TabTemplate.InactiveTabTemplate.BackColor;
				this.InactiveTabForeColor = editor.TabTemplate.InactiveTabTemplate.ForeColor;
				this.InactiveTabFont = editor.TabTemplate.InactiveTabTemplate.Font;
				this.InactiveTabBorderColor = editor.TabTemplate.InactiveTabTemplate.BorderColor;
				this.ActiveTabColor = editor.TabTemplate.ActiveTabTemplate.BackColor;
				this.ActiveTabForeColor = editor.TabTemplate.ActiveTabTemplate.ForeColor;
				this.ActiveTabBorderColor = editor.TabTemplate.ActiveTabTemplate.BorderColor;
				this.ActiveTabFont = editor.TabTemplate.ActiveTabTemplate.Font;
				this.CloseButtonBackColor = editor.TabTemplate.ActiveTabTemplate.CloseButtonBackColor;
				this.CloseButtonForeColor = editor.TabTemplate.ActiveTabTemplate.CloseButtonForeColor;
				this.CloseButtonHotForeColor = editor.TabTemplate.ActiveTabTemplate.CloseButtonHotForeColor;
				this.CloseButtonBorderColor = editor.TabTemplate.ActiveTabTemplate.CloseButtonBorderColor;
				this.MouseOverTabColor = editor.TabTemplate.MouseOverTabTemplate.BackColor;
				this.MouseOverTabForeColor = editor.TabTemplate.MouseOverTabTemplate.ForeColor;
				this.MouseOverTabFont = editor.TabTemplate.MouseOverTabTemplate.Font;
			}
		}
	}
}