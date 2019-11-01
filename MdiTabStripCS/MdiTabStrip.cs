#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

using MdiTabStripCS.Designer;

#endregion

namespace MdiTabStripCS {
	/// <summary>
	/// Provides a container for tabs representing forms opened in an MDI application.
	/// </summary>
	[Designer(typeof(MdiTabStripDesigner)), ToolboxBitmap(typeof(MdiTabStrip), "ToolBoxImg1.bmp")]
	public class MdiTabStrip : ScrollableControl, ISupportInitialize {
		#region "Fields"

		private TabStripLayoutEngine m_layout;
		private MdiTab m_activeTab;
		private MdiTabCollection m_tabs = new MdiTabCollection();
		private int m_indexOfTabForDrop;
		private bool m_isDragging;
		private int m_maxTabWidth = 200;
		private int m_minTabWidth = 80;
		private MdiTabStripItemBase m_mouseOverControl;
		private ScrollDirection m_dragDirection = ScrollDirection.Left;
		/*private MdiScrollTab withEventsField_m_leftScrollTab = new MdiScrollTab(this, ScrollTabType.ScrollTabLeft);
		private MdiScrollTab m_dropDownScrollTab = new MdiScrollTab(this, ScrollTabType.ScrollTabDropDown);
		private MdiScrollTab withEventsField_m_rightScrollTab = new MdiScrollTab(this, ScrollTabType.ScrollTabRight);*/
		private MdiScrollTab withEventsField_m_leftScrollTab;
		private MdiScrollTab m_dropDownScrollTab;
		private MdiScrollTab withEventsField_m_rightScrollTab;
		private MdiScrollTab m_leftScrollTab {
			get {
				return this.withEventsField_m_leftScrollTab;
			}
			set {
				this.withEventsField_m_leftScrollTab = value;
				/*if(this.withEventsField_m_leftScrollTab != null) {
					this.withEventsField_m_leftScrollTab.ScrollTab -= this.leftTabScroll_ScrollTab;
				}
				this.withEventsField_m_leftScrollTab = value;
				if(this.withEventsField_m_leftScrollTab != null) {
					this.withEventsField_m_leftScrollTab.ScrollTab += this.leftTabScroll_ScrollTab;
				}*/
			}
		}
		private MdiScrollTab m_rightScrollTab {
			get {
				return this.withEventsField_m_rightScrollTab;
			}
			set {
				this.withEventsField_m_rightScrollTab = value;
				/*if(this.withEventsField_m_rightScrollTab != null) {
					this.withEventsField_m_rightScrollTab.ScrollTab -= this.rightTabScroll_ScrollTab;
				}
				this.withEventsField_m_rightScrollTab = value;
				if(this.withEventsField_m_rightScrollTab != null) {
					this.withEventsField_m_rightScrollTab.ScrollTab += this.rightTabScroll_ScrollTab;
				}*/
			}
		}
		//	private MdiNewTab m_newTab = new MdiNewTab(this);
		private MdiNewTab m_newTab;
		private bool m_mdiNewTabVisible;
		private int m_mdiNewTabWidth = 25;
		private Image m_mdiNewTabImage;
		private MdiTabPermanence m_tabPermanence = MdiTabPermanence.None;
		private bool m_displayFormIcon = true;
		private MdiChildWindowState m_mdiWindowState = MdiChildWindowState.Normal;
		private ToolTip withEventsField_m_toolTip = new ToolTip();
		private ToolTip m_toolTip {
			get {
				return this.withEventsField_m_toolTip;
			}
			set {
				this.withEventsField_m_toolTip = value;
				/*if(this.withEventsField_m_toolTip != null) {
					this.withEventsField_m_toolTip.Popup -= this.m_toolTip_Popup;
					this.withEventsField_m_toolTip.Draw -= this.m_toolTip_Draw;
				}
				this.withEventsField_m_toolTip = value;
				if(this.withEventsField_m_toolTip != null) {
					this.withEventsField_m_toolTip.Popup += this.m_toolTip_Popup;
					this.withEventsField_m_toolTip.Draw += this.m_toolTip_Draw;
				}*/
			}
		}
		private bool _showTabToolTip = true;
		private string _newTabToolTipText = "New Tab";
		//Active tab fields
		private Color m_activeTabColor = Color.LightSteelBlue;
		private Color m_activeTabBorderColor = Color.Gray;
		private Color m_activeTabForeColor = SystemColors.ControlText;
		private Font m_activeTabFont = SystemFonts.DefaultFont;
		private Color m_closeButtonBackColor = Color.Gainsboro;
		private Color m_closeButtonBorderColor = Color.Gray;
		private Color m_closeButtonForeColor = SystemColors.ControlText;
		private Color m_closeButtonHotForeColor = Color.Firebrick;
		//Inactive tab fields
		private Color m_inactiveTabColor = Color.Gainsboro;
		private Color m_inactiveTabBorderColor = Color.Silver;
		private Color m_inactiveTabForeColor = SystemColors.ControlText;
		private Font m_inactiveTabFont = SystemFonts.DefaultFont;
		//Moused over tab fields
		private Color m_mouseOverTabColor = Color.LightSteelBlue;
		private Color m_mouseOverTabForeColor = SystemColors.ControlText;
		private Font m_mouseOverTabFont = SystemFonts.DefaultFont;
		//Fade animation fields
		private bool m_animate = true;
		private int m_duration = 20;
		private Timer withEventsField_m_timer = new Timer();
		private Timer m_timer {
			get {
				return this.withEventsField_m_timer;
			}
			set {
				this.withEventsField_m_timer = value;
				/*if(this.withEventsField_m_timer != null) {
					this.withEventsField_m_timer.Tick -= this.m_timer_Tick;
				}
				this.withEventsField_m_timer = value;
				if(this.withEventsField_m_timer != null) {
					this.withEventsField_m_timer.Tick += this.m_timer_Tick;
				}*/
			}
		}
		private List<Color> m_backColorFadeArray = new List<Color>();
		private List<Color> m_foreColorFadeArray;
		private ArrayList m_animatingTabs = new ArrayList();

		#endregion


		#region "Events"

		/// <summary>
		/// Occurs when the <see cref="MdiTab"/> has been made active.
		/// </summary>
		public event CurrentMdiTabChangedEventHandler CurrentMdiTabChanged;

		public delegate void CurrentMdiTabChangedEventHandler(object sender, EventArgs e);

		/// <summary>
		/// Occurs when a new <see cref="MdiTab"/> is added to the <see cref="MdiTabCollection"/>.
		/// </summary>
		public event MdiTabAddedEventHandler MdiTabAdded;

		public delegate void MdiTabAddedEventHandler(object sender, MdiTabStripTabEventArgs e);

		/// <summary>
		/// Occurs when the <see cref="MdiTab"/> is clicked.
		/// </summary>
		public event MdiTabClickedEventHandler MdiTabClicked;

		public delegate void MdiTabClickedEventHandler(object sender, MdiTabStripTabClickedEventArgs e);

		/// <summary>
		/// Occurs when the <see cref="MdiTab"/> has been moved to a new position.
		/// </summary>
		public event MdiTabIndexChangedEventHandler MdiTabIndexChanged;

		public delegate void MdiTabIndexChangedEventHandler(object sender, EventArgs e);

		/// <summary>
		/// Occurs when an <see cref="MdiTab"/> is removed from the <see cref="MdiTabCollection"/>.
		/// </summary>
		public event MdiTabRemovedEventHandler MdiTabRemoved;

		public delegate void MdiTabRemovedEventHandler(object sender, MdiTabStripTabEventArgs e);

		/// <summary>
		/// Occurs when the <see cref="MdiNewTab"/> is clicked.
		/// </summary>
		public event MdiNewTabClickedEventHandler MdiNewTabClicked;

		public delegate void MdiNewTabClickedEventHandler(object sender, EventArgs e);

        #endregion


        // 2010-12-10 16:45:09 NOTE By Jaeyong Park : Add context menu when right mouse click
        #region 
        ContextMenuStrip CTXRightMenu = new ContextMenuStrip();
		ToolStripMenuItem CTXRightMenuCloseThis = new ToolStripMenuItem();
		ToolStripMenuItem CTXRightMenuCloseAnother = new ToolStripMenuItem();
		ToolStripMenuItem CTXRightMenuCloseAll = new ToolStripMenuItem();
		#endregion 

		#region "Constructor/Destructor"

		/// <summary>
		/// Initializes a new instance of the <see cref="MdiTabStrip"/> class.
		/// </summary>
		public MdiTabStrip() {
			ResizeRedraw = true;
			DoubleBuffered = true;
			MinimumSize = new Size(50, 33);
			Dock = DockStyle.Top;
			AllowDrop = true;

			this.withEventsField_m_leftScrollTab = new MdiScrollTab(this, ScrollTabType.ScrollTabLeft);
			this.m_dropDownScrollTab = new MdiScrollTab(this, ScrollTabType.ScrollTabDropDown);
			this.withEventsField_m_rightScrollTab = new MdiScrollTab(this, ScrollTabType.ScrollTabRight);
			this.m_newTab = new MdiNewTab(this);

			this.withEventsField_m_toolTip.Popup += this.m_toolTip_Popup;
			this.withEventsField_m_toolTip.Draw += this.m_toolTip_Draw;
			this.withEventsField_m_leftScrollTab.ScrollTab += this.leftTabScroll_ScrollTab;
			this.withEventsField_m_rightScrollTab.ScrollTab += this.rightTabScroll_ScrollTab;
			this.withEventsField_m_timer.Tick += this.m_timer_Tick;

			//Padding values directly affect the tab's placement, change these in the designer to see
			//how the tab's size and placement change.
			Padding = new Padding(5, 3, 20, 5);
			this.m_timer.Interval = 2;
			this.m_backColorFadeArray = this.GetFadeSteps(this.m_inactiveTabColor, this.m_mouseOverTabColor);
			this.m_foreColorFadeArray = this.GetFadeSteps(this.m_inactiveTabForeColor, this.m_mouseOverTabForeColor);
			this.m_toolTip.AutoPopDelay = 2000;
			this.m_toolTip.OwnerDraw = true;
			//Setup scrolltab sizes
			this.m_leftScrollTab.Size = new Size(20, this.DisplayRectangle.Height);
			this.m_dropDownScrollTab.Size = new Size(14, this.DisplayRectangle.Height);
			this.m_rightScrollTab.Size = new Size(20, this.DisplayRectangle.Height);
			this.m_newTab.Size = new Size(25, this.DisplayRectangle.Height);

			// 2010-12-10 16:45:21 NOTE By Jaeyong Park : context menu setting
			#region 
			CTXRightMenuCloseThis.Text = "Close tab(&C)";
			CTXRightMenuCloseThis.Click += this.CTXRightMenuCloseThis_Click;
			CTXRightMenuCloseAnother.Text = "Close other tab(&O)";
			CTXRightMenuCloseAnother.Click += this.CTXRightMenuCloseAnother_Click;
			CTXRightMenuCloseAll.Text = "Close all tab(&L)";
			CTXRightMenuCloseAll.Click += this.CTXRightMenuCloseAll_Click;
			CTXRightMenu.Items.Add(CTXRightMenuCloseThis);
			CTXRightMenu.Items.Add(CTXRightMenuCloseAnother);
			CTXRightMenu.Items.Add(CTXRightMenuCloseAll);
			#endregion 
		}

		// NOTE By Jaeyong Park : context menu event
		#region 

		void CTXRightMenuCloseThis_Click(object sender, EventArgs e) {
			try {
				this.ActiveTab.Form.Close();
			} catch {
			}
		}

		void CTXRightMenuCloseAnother_Click(object sender, EventArgs e) {
			try {
				if(this.Tabs.Count > 1) {
					string _activeFormName = this.ActiveTab.Form.Name;
					while(this.Tabs.Count > 1) {
						//this.RemoveMdiItem(this.Tabs[k].Form);
						if(this.Tabs[0].Form.Name == _activeFormName) {
							this.Tabs[1].Form.Close();
						} else {
							this.Tabs[0].Form.Close();
						}
					}
				}
			} catch{
			}
		}

		void CTXRightMenuCloseAll_Click(object sender, EventArgs e) {
			try {
				while(this.Tabs.Count > 0) {
					this.Tabs[0].Form.Close();
				}
			} catch{
			}
		}

		#endregion

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="MdiTabStrip"/> and optionally releases the managed resources. 
		/// </summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				this.LeftScrollTab.Dispose();
				this.DropDownTab.Dispose();
				this.RightScrollTab.Dispose();
				this.MdiNewTab.Dispose();
				this.m_toolTip.Dispose();
				this.m_activeTabFont.Dispose();
				this.m_inactiveTabFont.Dispose();
				this.m_mouseOverTabFont.Dispose();
				this.m_timer.Dispose();

				CTXRightMenu.Dispose();
				Form parent = FindForm();
				if(parent != null) {
					//Unhook event handler registered with the top form.
					parent.MdiChildActivate -= this.MdiChildActivated;
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region "ISupportInitialize implementation"

		//Initialization is used so that the top form can be found. This is needed in case the control
		//is added to a container control such as a panel.
		/// <summary>
		/// Signals the object that initialization is starting.
		/// </summary>
		public void BeginInit() {
			SuspendLayout();
		}
		/// <summary>
		/// Signals the object that initialization is complete.
		/// </summary>
		public void EndInit() {
			ResumeLayout();
			Form parent = FindForm();
			if(parent != null) {
				//Register event handler with top form.
				parent.MdiChildActivate += this.MdiChildActivated;
			}
		}

		#endregion

		#region "Properties"

		#region "Active Tab properties"

		/// <summary>
		/// Gets or sets the background color of the active tab.
		/// </summary>
		/// <returns>The background <see cref="Color"/> of the active <see cref="MdiTab"/>. The default value is LightSteelBlue.</returns>
		[DefaultValue(typeof(Color), "LightSteelBlue"), Category("Active Tab"), Description("The background color of the currently active tab.")]
		public Color ActiveTabColor {
			get {
				return this.m_activeTabColor;
			}
			set {
				this.m_activeTabColor = value;
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the foreground color of the active tab.
		/// </summary>
		/// <returns>The foreground <see cref="Color"/> of the active <see cref="MdiTab"/>. The default value is ControlText.</returns>
		[DefaultValue(typeof(Color), "ControlText"), Category("Active Tab"), Description("The foreground color of the currently active tab, which is used to display text.")]
		public Color ActiveTabForeColor {
			get {
				return this.m_activeTabForeColor;
			}
			set {
				this.m_activeTabForeColor = value;
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the border color of the active tab.
		/// </summary>
		/// <returns>The border <see cref="Color"/> of the active <see cref="MdiTab"/>. The default value is Gray.</returns>
		[DefaultValue(typeof(Color), "Gray"), Category("Active Tab"), Description("The border color of the currently active tab.")]
		public Color ActiveTabBorderColor {
			get {
				return this.m_activeTabBorderColor;
			}
			set {
				this.m_activeTabBorderColor = value;
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the font of the active tab.
		/// </summary>
		/// <returns>The <see cref="Font"/> to apply to the text displayed by the active <see cref="MdiTab"/>. The value returned will vary depending on the user's operating system the local culture setting of their system.</returns>
		[DefaultValue(typeof(Font), "SystemFonts.DefaultFont"), Category("Active Tab"), Description("The font used to display text in the currently active tab.")]
		public Font ActiveTabFont {
			get {
				return this.m_activeTabFont;
			}
			set {
				this.m_activeTabFont = value;
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the background color of the close button.
		/// </summary>
		/// <returns>The background <see cref="Color"/> of the close button. The default value is Gainsboro.</returns>
		[DefaultValue(typeof(Color), "Gainsboro"), Category("Active Tab"), Description("The background color of the close button when moused over.")]
		public Color CloseButtonBackColor {
			get {
				return this.m_closeButtonBackColor;
			}
			set {
				this.m_closeButtonBackColor = value;
			}
		}
		/// <summary>
		/// Gets or sets the foreground color of the close button.
		/// </summary>
		/// <returns>The foreground <see cref="Color"/> of the close button. The default value is ControlText.</returns>
		[DefaultValue(typeof(Color), "ControlText"), Category("Active Tab"), Description("The foreground color of the close button, used to display the glyph.")]
		public Color CloseButtonForeColor {
			get {
				return this.m_closeButtonForeColor;
			}
			set {
				this.m_closeButtonForeColor = value;
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the foreground color of the close button when the mouse cursor is hovered over it.
		/// </summary>
		/// <returns>The foreground <see cref="Color"/> of the close button when hovered over. The default value is Firebrick.</returns>
		[DefaultValue(typeof(Color), "Firebrick"), Category("Active Tab"), Description("The foreground color of the close button when moused over, used to display the glyph.")]
		public Color CloseButtonHotForeColor {
			get {
				return this.m_closeButtonHotForeColor;
			}
			set {
				this.m_closeButtonHotForeColor = value;
			}
		}
		/// <summary>
		/// Gets or sets the border color of the close button.
		/// </summary>
		/// <returns>The border <see cref="Color"/> of the close button. The default value is Gray.</returns>
		[DefaultValue(typeof(Color), "Gray"), Category("Active Tab"), Description("The border color of the close button when moused over.")]
		public Color CloseButtonBorderColor {
			get {
				return this.m_closeButtonBorderColor;
			}
			set {
				this.m_closeButtonBorderColor = value;
			}
		}

		#endregion

		#region "Inactive Tab properties"

		/// <summary>
		/// Gets or sets the background color of the inactive tab.
		/// </summary>
		/// <returns>The background <see cref="Color"/> of the inactive <see cref="MdiTab"/>. The default value is Gainsboro.</returns>
		[DefaultValue(typeof(Color), "Gainsboro"), Category("Inactive Tab"), Description("The background color of all inactive tabs.")]
		public Color InactiveTabColor {
			get {
				return this.m_inactiveTabColor;
			}
			set {
				this.m_inactiveTabColor = value;
				this.m_backColorFadeArray = this.GetFadeSteps(this.InactiveTabColor, this.MouseOverTabColor);
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the foreground color of the inactive tab.
		/// </summary>
		/// <returns>The foreground <see cref="Color"/> of the inactive <see cref="MdiTab"/>. The default value is ControlText.</returns>
		[DefaultValue(typeof(Color), "ControlText"), Category("Inactive Tab"), Description("The foreground color of all inactive tabs, which is used to display text.")]
		public Color InactiveTabForeColor {
			get {
				return this.m_inactiveTabForeColor;
			}
			set {
				this.m_inactiveTabForeColor = value;
				this.m_foreColorFadeArray = this.GetFadeSteps(this.InactiveTabForeColor, this.MouseOverTabForeColor);
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the border color of the inactive tab.
		/// </summary>
		/// <returns>The border <see cref="Color"/> of the inactive <see cref="MdiTab"/>. The default value is Silver.</returns>
		[DefaultValue(typeof(Color), "Silver"), Category("Inactive Tab"), Description("The border color of all inactive tabs.")]
		public Color InactiveTabBorderColor {
			get {
				return this.m_inactiveTabBorderColor;
			}
			set {
				this.m_inactiveTabBorderColor = value;
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the font of the inactive tab.
		/// </summary>
		/// <returns>The <see cref="Font"/> to apply to the text displayed by the inactive <see cref="MdiTab"/>. The value returned will vary depending on the user's operating system the local culture setting of their system.</returns>
		[DefaultValue(typeof(Font), "SytemFonts.DefaultFont"), Category("Inactive Tab"), Description("The font used to display text in all inactive tabs.")]
		public Font InactiveTabFont {
			get {
				return this.m_inactiveTabFont;
			}
			set {
				this.m_inactiveTabFont = value;
				Invalidate();
			}
		}

		#endregion

		#region "Mouseover Tab properties"

		/// <summary>
		/// Gets or sets the background color of the moused over tab.
		/// </summary>
		/// <returns>The background <see cref="Color"/> of the moused over <see cref="MdiTab"/>. The default value is LightSteelBlue.</returns>
		[DefaultValue(typeof(Color), "LightSteelBlue"), Category("Mouse Over Tab"), Description("The background color for the tab the mouse cursor is currently over.")]
		public Color MouseOverTabColor {
			get {
				return this.m_mouseOverTabColor;
			}
			set {
				this.m_mouseOverTabColor = value;
				this.m_backColorFadeArray = this.GetFadeSteps(this.InactiveTabColor, this.MouseOverTabColor);
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the foreground color of the moused over tab.
		/// </summary>
		/// <returns>The foreground <see cref="Color"/> of the moused over <see cref="MdiTab"/>. The default value is ControlText.</returns>
		[DefaultValue(typeof(Color), "ControlText"), Category("Mouse Over Tab"), Description("The foreground color of the tab the mouse cursor is currently over, which is used to display text and glyphs.")]
		public Color MouseOverTabForeColor {
			get {
				return this.m_mouseOverTabForeColor;
			}
			set {
				this.m_mouseOverTabForeColor = value;
				this.m_foreColorFadeArray = this.GetFadeSteps(this.InactiveTabForeColor, this.MouseOverTabForeColor);
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the font of the moused over tab.
		/// </summary>
		/// <returns>The <see cref="Font"/> to apply to the text displayed by the moused over <see cref="MdiTab"/>. The value returned will vary depending on the user's operating system the local culture setting of their system.</returns>
		[DefaultValue(typeof(Font), "SystemFonts.DefaultFont"), Category("Mouse Over Tab"), Description("The font used to display text in the tab the mouse cursor is currently over.")]
		public Font MouseOverTabFont {
			get {
				return this.m_mouseOverTabFont;
			}
			set {
				this.m_mouseOverTabFont = value;
				Invalidate();
			}
		}

		#endregion

		#region "ScrollTab properties"

		[Browsable(false)]
		public MdiScrollTab LeftScrollTab {
			get {
				return this.m_leftScrollTab;
			}
		}
		[Browsable(false)]
		public MdiScrollTab RightScrollTab {
			get {
				return this.m_rightScrollTab;
			}
		}
		[Browsable(false)]
		public MdiScrollTab DropDownTab {
			get {
				return this.m_dropDownScrollTab;
			}
		}

		#endregion

		[Browsable(false)]
		public MdiNewTab MdiNewTab {
			get {
				return this.m_newTab;
			}
		}
		internal int Duration {
			get {
				return this.m_duration;
			}
		}
		/// <summary>
		/// Gets the rectangle that represents the display area of the control.
		/// </summary>
		/// <returns>A <see cref="Rectangle"/> that represents the display area of the control.</returns>
		public override Rectangle DisplayRectangle {
			get {
				Rectangle rect = new Rectangle(0, 0, Width, Height);
				rect.X += Padding.Left;
				rect.Y += Padding.Top;
				rect.Width -= Padding.Left + Padding.Right;
				rect.Height -= Padding.Top + Padding.Bottom;
				return rect;
			}
		}
		/// <summary>
		/// Gets the default size of the control.
		/// </summary>
		/// <returns>The default <see cref="Size"/> of the control.</returns>
		protected override Size DefaultSize {
			get {
				return new Size(50, 35);
			}
		}
		/// <summary>
		/// Gets or sets if the control should animate between the inactive and moused over background colors.
		/// </summary>
		/// <returns>true if the should animate; otherwise, false. The default is true.</returns>
		[DefaultValue(true), Category("Behavior")]
		public bool Animate {
			get {
				return this.m_animate;
			}
			set {
				this.m_animate = value;
			}
		}
		/// <summary>
		/// Gets or sets a value indicating whether an icon is displayed on the tab for the form.
		/// </summary>
		/// <returns>true if the control displays an icon in the tab; otherwise, false. The default is true.</returns>
		[DefaultValue(true), Category("Appearance")]
		public bool DisplayFormIcon {
			get {
				return this.m_displayFormIcon;
			}
			set {
				this.m_displayFormIcon = value;
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets a <see cref="ToolStripRenderer"/> used to customize the look and feel of the <see cref="MdiTabStrip"/>'s drop down.
		/// </summary>
		/// <returns>A <see cref="ToolStripRenderer"/> used to customize the look and feel of a <see cref="MdiTabStrip"/>'s drop down.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public ToolStripRenderer DropDownRenderer {
			get {
				return this.m_dropDownScrollTab.MdiMenu.Renderer;
			}
			set {
				this.m_dropDownScrollTab.MdiMenu.Renderer = value;
			}
		}
		internal bool IsFirstTabActive {
			get {
				return this.Tabs.IndexOf(this.ActiveTab) == 0;
			}
		}
		internal bool IsLastTabActive {
			get {
				return this.Tabs.IndexOf(this.ActiveTab) == this.Tabs.Count - 1;
			}
		}
		/// <summary>
		/// Gets or sets the desired window state of all child <see cref="Form"/>s.
		/// </summary>
		/// <returns>normal if each form's WindowState and ControlBox property settings should be obeyed; otherwise, 
		/// maximized, to force all forms to be maximized in the MDI window. The default is normal.</returns>
		[DefaultValue(typeof(MdiChildWindowState), "Normal"), Category("Layout"), Description("Gets or sets the desired window state of all child forms")]
		public MdiChildWindowState MdiWindowState {
			get {
				return this.m_mdiWindowState;
			}
			set {
				this.m_mdiWindowState = value;
			}
		}
		/// <summary>
		/// Gets or sets the permanance of the tab.
		/// </summary>
		/// <returns>first if the first tab open should not be closeable, last open if the last remaining tab should not be closeable; otherwise none. The default is all tabs are closeable.</returns>
		[DefaultValue(typeof(MdiTabPermanence), "None"), Category("Behavior"),
		 Description("Defines how the control will handle the closing of tabs. The first tab or the last remaining tab can be restricted from closing or a setting of 'None' will allow all tabs to be closed."
		 	)]
		public MdiTabPermanence TabPermanence {
			get {
				return this.m_tabPermanence;
			}
			set {
				this.m_tabPermanence = value;
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the visibility of the <see cref="MdiNewTab"/>.
		/// </summary>
		[DefaultValue(false), Category("Appearance"), Description("Gets or sets whether or not the control will display the MdiNewTab.")]
		public bool MdiNewTabVisible {
			get {
				return this.m_mdiNewTabVisible;
			}
			set {
				if(this.m_mdiNewTabVisible != value) {
					this.m_mdiNewTabVisible = value;
					PerformLayout();
					Invalidate();
				}
			}
		}
		/// <summary>
		/// Gets or sets the width of the <see cref="MdiNewTab"/>.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		[DefaultValue(25), Category("Layout"), Description("Gets or sets the width of the MdiNewTab.")]
		public int MdiNewTabWidth {
			//Return Me.m_mdiNewTabWidth
			get {
				return this.MdiNewTab.Width;
			}
			set {
				//If Me.m_mdiNewTabWidth <> value Then
				//Me.m_mdiNewTabWidth = value
				this.MdiNewTab.Width = value;
				PerformLayout();
				Invalidate();
				//End If
			}
		}
		/// <summary>
		/// Gets or sets the image for the <see cref="MdiNewTab"/>.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		[Category("Appearance"), Description("Gets or sets the image for the MdiNewTab.")]
		public Image MdiNewTabImage {
			get {
				return this.m_mdiNewTabImage;
			}
			set {
				this.m_mdiNewTabImage = value;
			}
		}
		/// <summary>
		/// Gets all the tabs that belong to the <see cref="MdiTabStrip"/>.
		/// </summary>
		/// <returns>An object of type <see cref="MdiTabCollection"/>, representing all the tabs contained by an <see cref="MdiTabStrip"/>.</returns>
		[Browsable(false)]
		public MdiTabCollection Tabs {
			get {
				return this.m_tabs;
			}
		}
		/// <summary>
		/// Passes a reference to the cached <see cref="LayoutEngine"/> returned by the layout engine interface.
		/// </summary>
		/// <returns>A <see cref="LayoutEngine"/> that represents the cached layout engine returned by the layout engine interface.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override LayoutEngine LayoutEngine {
			get {
				if(this.m_layout == null) {
					this.m_layout = new TabStripLayoutEngine();
				}
				return this.m_layout;
			}
		}
		/// <summary>
		/// Gets or sets the maximum width for the tab.
		/// </summary>
		/// <returns>The maximum width a tab can be sized to. The default value is 200.</returns>
		/// <remarks>This property affects the tab's size when resizing the control and is used in conjunction with the <seealso cref="MinTabWidth"/> property.</remarks>
		[DefaultValue(200), Category("Layout"), Description("The maximum width for each tab.")]
		public int MaxTabWidth {
			get {
				return this.m_maxTabWidth;
			}
			set {
				this.m_maxTabWidth = value;
			}
		}
		/// <summary>
		/// Gets or sets the minimum width for the tab.
		/// </summary>
		/// <returns>The minimum width a tab can be sized to. The default is 80.</returns>
		/// <remarks>This property affects the tab's size when resizing the control and is used in conjunction with the <seealso cref="MaxTabWidth"/> property.</remarks>
		[DefaultValue(80), Category("Layout"), Description("The minimum width for each tab.")]
		public int MinTabWidth {
			get {
				return this.m_minTabWidth;
			}
			set {
				this.m_minTabWidth = value;
			}
		}
		/// <summary>
		/// Gets or sets the active tab.
		/// </summary>
		/// <returns>The <see cref="MdiTab"/> that is currenly active.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public MdiTab ActiveTab {
			get {
				return this.m_activeTab;
			}
			set {
				if(!ReferenceEquals(this.m_activeTab, value)) {
					this.m_activeTab = value;
					this.OnCurrentMdiTabChanged(new EventArgs());
				}
			}
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		internal bool IsDragging {
			get {
				return this.m_isDragging;
			}
			set {
				this.m_isDragging = value;
			}
		}
		internal List<Color> BackColorFadeSteps {
			get {
				return this.m_backColorFadeArray;
			}
		}
		internal List<Color> ForeColorFadeSteps {
			get {
				return this.m_foreColorFadeArray;
			}
		}
		private MdiTabStripItemBase MouseOverControl {
			get {
				return this.m_mouseOverControl;
			}
			set {
				this.m_mouseOverControl = value;
				Invalidate();
			}
		}
		/// <summary>
		/// Specifies whether to display ToolTips on tabs.
		/// </summary>
		[DefaultValue(true), Category("Behavior"), Description("Specifies whether to display ToolTips on tabs.")]
		public bool ShowTabToolTip {
			get {
				return this._showTabToolTip;
			}
			set {
				this._showTabToolTip = value;
			}
		}
		/// <summary>
		/// Gets or sets the ToolTip text of the <see cref="MdiNewTab"/>.
		/// </summary>
		[Category("Behavior"), Description("Gets or sets the ToolTip text of the MdiNewTab.")]
		public string NewTabToolTipText {
			get {
				return this._newTabToolTipText;
			}
			set {
				this._newTabToolTipText = value;
			}
		}

		#endregion

		#region "Methods"

		#region "Form Event Handlers"

		protected void MdiChildActivated(object sender, EventArgs e) {
			Form f = ((Form)sender).ActiveMdiChild;
			//If the ActiveMDIChild is nothing then exit routine.
			if(f == null) {
				return;
			}
			//If a tab has already been created for the form then activate it,
			//otherwise create a new one.
			if(this.TabExists(f)) {
				this.ActivateTab(f);
			} else {
				this.CreateTab(f);
			}
			//If the first tab has been made active then disable the left scroll tab
			//If the last tab has been made active then disable the right scroll tab
			this.LeftScrollTab.Enabled = (RightToLeft == RightToLeft.Yes ? !this.IsLastTabActive : !this.IsFirstTabActive);
			this.RightScrollTab.Enabled = (RightToLeft == RightToLeft.Yes ? !this.IsFirstTabActive : !this.IsLastTabActive);
			Invalidate();
		}
		protected void OnFormTextChanged(object sender, EventArgs e) {
			Form f = (Form)sender;
			//Find the menu item that cooresponds to this form and update it's Text property.
			//Can't override the menuitem's Text property to return the Form.Text property because when
			//the form's text property is changed the drop down menu does not resize itself accordingly.
			foreach(MdiMenuItem mi in this.m_dropDownScrollTab.m_mdiMenu.Items) {
				if(ReferenceEquals(mi.Form, f)) {
					mi.Text = f.Text;
				}
			}
			Invalidate();
		}
		private bool TabExists(Form mdiForm) {
			foreach(MdiTab tab in this.Tabs) {
				if(ReferenceEquals(tab.Form, mdiForm)) {
					return true;
				}
			}
			return false;
		}
		private void ActivateTab(Form mdiForm) {
			foreach(MdiTab t in this.Tabs) {
				if(ReferenceEquals(t.Form, mdiForm)) {
					this.ActiveTab = t;
					//Find the menu item of the drop down menu and set it's Checked property
					foreach(MdiMenuItem mi in this.m_dropDownScrollTab.m_mdiMenu.Items) {
						if(ReferenceEquals(mi.Form, mdiForm)) {
							this.m_dropDownScrollTab.m_mdiMenu.SetItemChecked(mi);
							break; // TODO: might not be correct. Was : Exit For
						}
					}
					return;
				}
			}
		}
		private void CreateTab(Form mdiForm) {
			MdiTab tab = new MdiTab(this);
			//Set up the tab
			if(this.m_mdiWindowState == MdiChildWindowState.Maximized) {
				mdiForm.SuspendLayout();
				mdiForm.FormBorderStyle = FormBorderStyle.None;
				mdiForm.ControlBox = false;
				mdiForm.HelpButton = false;
				mdiForm.MaximizeBox = false;
				mdiForm.MinimizeBox = false;
				mdiForm.SizeGripStyle = SizeGripStyle.Hide;
				mdiForm.ShowIcon = false;
				mdiForm.Dock = DockStyle.Fill;
				mdiForm.ResumeLayout(true);
			}
			tab.Form = mdiForm;
			//Register event handler with the MdiChild form's FormClosed event.
			mdiForm.FormClosed += this.OnFormClosed;
			mdiForm.TextChanged += this.OnFormTextChanged;
			//Add the new tab to the Tabs collection and set it as the active tab
			this.Tabs.Add(tab);
			this.OnMdiTabAdded(new MdiTabStripTabEventArgs(tab));
			this.ActiveTab = tab;
			//Create a cooresponding menu item in the drop down menu
			this.AddMdiItem(mdiForm, tab);
			this.UpdateTabVisibility(ScrollDirection.Right);
		}
		private void RemoveTab(Form mdiForm) {
			foreach(MdiTab tab in this.Tabs) {
				if(ReferenceEquals(tab.Form, mdiForm)) {
					//This algorithm will get the index of the tab that is higher than the tab
					//that is to be removed. This has the affect of making the tab occuring after
					//the tab just closed the active tab.
					int tabIndex = this.Tabs.IndexOf(tab);
					//Remove tab from the Tabs collection
					this.Tabs.Remove(tab);
					this.OnMdiTabRemoved(new MdiTabStripTabEventArgs(tab));
					//Remove the cooresponding menu item from the drop down menu.
					foreach(MdiMenuItem mi in this.m_dropDownScrollTab.m_mdiMenu.Items) {
						if(ReferenceEquals(mi.Form, tab.Form)) {
							this.m_dropDownScrollTab.m_mdiMenu.Items.Remove(mi);
							break; // TODO: might not be correct. Was : Exit For
						}
					}
					//If the tab removed was the last tab in the collection then
					//set the index to the last tab.
					if(tabIndex > this.Tabs.Count - 1) {
						tabIndex = this.Tabs.Count - 1;
					}
					if(tabIndex > -1) {
						//Call the Form's Activate method to allow the event handlers
						//to perform their neccessary calculations.
						this.Tabs[tabIndex].Form.Activate();
					} else {
						this.ActiveTab = null;
					}
					this.UpdateTabVisibility(ScrollDirection.Right);
					Invalidate();
					break; // TODO: might not be correct. Was : Exit For
				}
			}
		}
		protected void OnFormClosed(object sender, FormClosedEventArgs e) {
			//Only remove the tab when the form was closed by the user. All other close reason look like they
			//will also be closing the Mdi parent and so will dispose the MdiTabStrip.
			if(e.CloseReason == CloseReason.UserClosing) {
				this.RemoveTab((Form)sender);
			}
		}

		#endregion

		#region "Paint Methods"

		#region "ToolTip painting"

		private void m_toolTip_Popup(object sender, PopupEventArgs e) {
			Size s = TextRenderer.MeasureText(this.m_toolTip.GetToolTip(e.AssociatedControl), SystemFonts.SmallCaptionFont);
			s.Width += 4;
			s.Height += 6;
			e.ToolTipSize = s;
		}
		private void m_toolTip_Draw(object sender, DrawToolTipEventArgs e) {
			Rectangle rect = e.Bounds;
			using(LinearGradientBrush lgb = new LinearGradientBrush(e.Bounds, Color.WhiteSmoke, Color.Silver, LinearGradientMode.Vertical)) {
				e.Graphics.FillRectangle(lgb, e.Bounds);
			}
			rect.Width -= 1;
			rect.Height -= 1;
			e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, rect);
			e.DrawText();
		}

		#endregion

		protected override void OnPaint(PaintEventArgs e) {
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			foreach(MdiTab tab in this.Tabs) {
				if(tab.Visible) {
					tab.DrawControl(e.Graphics);
				}
			}
			if(this.RightScrollTab.Visible) {
				this.RightScrollTab.DrawControl(e.Graphics);
			}
			if(this.LeftScrollTab.Visible) {
				this.LeftScrollTab.DrawControl(e.Graphics);
			}
			if(this.DropDownTab.Visible) {
				this.DropDownTab.DrawControl(e.Graphics);
			}
			if(this.MdiNewTabVisible) {
				this.m_newTab.DrawControl(e.Graphics);
			}
			//Draw DragDrop glyphs
			if(this.IsDragging) {
				MdiTab mditab = this.Tabs[this.m_indexOfTabForDrop];
				Point[] topTriangle = null;
				Point[] bottomTriangle = null;
				if(this.m_dragDirection == ScrollDirection.Left) {
					//Glyphs need to be located on the left side of the tab
					topTriangle = new[] { new Point(mditab.Left - 3, 0), new Point(mditab.Left + 3, 0), new Point(mditab.Left, 5) };
					bottomTriangle = new[] { new Point(mditab.Left - 3, Height - 1), new Point(mditab.Left + 3, Height - 1), new Point(mditab.Left, Height - 6) };
				} else {
					//Glyphs need to be located on the right side of the tab
					topTriangle = new[] { new Point(mditab.Right - 3, 0), new Point(mditab.Right + 3, 0), new Point(mditab.Right, 5) };
					bottomTriangle = new[] { new Point(mditab.Right - 3, Height - 1), new Point(mditab.Right + 3, Height - 1), new Point(mditab.Right, Height - 6) };
				}
				e.Graphics.FillPolygon(Brushes.Black, topTriangle);
				e.Graphics.FillPolygon(Brushes.Black, bottomTriangle);
			}
		}
		protected override void OnPaintBackground(PaintEventArgs e) {
			base.OnPaintBackground(e);
			foreach(MdiTab tab in this.Tabs) {
				//Draw the active tab last to make sure nothing paints over it.
				if(!tab.IsActive && tab.Visible) {
					tab.DrawControlBackground(e.Graphics);
				}
			}
			if(this.RightScrollTab != null && this.m_rightScrollTab.Visible) {
				this.RightScrollTab.DrawControlBackground(e.Graphics);
			}
			if(this.LeftScrollTab != null && this.m_leftScrollTab.Visible) {
				this.LeftScrollTab.DrawControlBackground(e.Graphics);
			}
			if(this.DropDownTab != null && this.m_dropDownScrollTab.Visible) {
				this.DropDownTab.DrawControlBackground(e.Graphics);
			}
			if(this.m_newTab != null && this.MdiNewTabVisible) {
				this.m_newTab.DrawControlBackground(e.Graphics);
			}
			if(this.ActiveTab != null) {
				this.ActiveTab.DrawControlBackground(e.Graphics);
			}
		}

		#endregion

		#region "Fade Animation"

		/// <summary>
		/// This method creates a Bitmap using the duration field as the width and creates a LinearGradientBrush
		/// using the colors passed in as parameters. It then fills the bitmap using
		/// the brush and reads the color values of each pixel along the width into a List for use in the
		/// fade animations. This method is called in the constructor and the Set procedures of the
		/// InactiveTabColor, InactiveTabForeColor, MouseOverTabColor and MouseOverTabForeColor properties.
		/// </summary>
		/// <remarks></remarks>
		private List<Color> GetFadeSteps(Color color1, Color color2) {
			List<Color> colorArray = new List<Color>();
			using(Bitmap bmp = new Bitmap(this.m_duration, 1)) {
				Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
				using(Graphics g = Graphics.FromImage(bmp)) {
					using(LinearGradientBrush lgb = new LinearGradientBrush(rect, color1, color2, LinearGradientMode.Horizontal)) {
						g.FillRectangle(lgb, rect);
					}
				}
				for(int x = 0 ; x <= bmp.Width - 1 ; x++) {
					colorArray.Add(bmp.GetPixel(x, 0));
				}
			}
			return colorArray;
		}
		/// <summary>
		/// For each tick of the Timer this event handler will iterate through the ArrayList of tabs that
		/// are currently needing to animate. For each tab it's current frame is incremented and sent as a
		/// parameter in the OnAnimationTick method. Depending on the animation type if the tab's current
		/// frame is 0 or equal to the Duration - 1 then the tab's animation will be stopped.
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Not used</param>
		/// <remarks></remarks>
		private void m_timer_Tick(object sender, EventArgs e) {
			int index = (this.m_animatingTabs.Count - 1);
			while((index >= 0)) {
				MdiTab tab = (MdiTab)this.m_animatingTabs[index];
				int frame = tab.CurrentFrame;
				if(tab.AnimationType == AnimationType.FadeIn) {
					if(frame == this.m_duration - 1) {
						tab.StopAnimation();
						return;
					}
					frame += 1;
				} else if(tab.AnimationType == AnimationType.FadeOut) {
					if(frame == 0) {
						tab.StopAnimation();
						return;
					}
					frame -= 1;
				}
				tab.OnAnimationTick(frame);
				index -= 1;
			}
		}
		internal void AddAnimatingTab(MdiTab tab) {
			if(this.m_animatingTabs.IndexOf(tab) < 0) {
				//Add the tab to the arraylist only if it is not already in here.
				this.m_animatingTabs.Add(tab);
				if(this.m_animatingTabs.Count == 1) {
					this.m_timer.Enabled = true;
				}
			}
		}
		internal void RemoveAnimatingTab(MdiTab tab) {
			this.m_animatingTabs.Remove(tab);
			if(this.m_animatingTabs.Count == 0) {
				this.m_timer.Enabled = false;
			}
		}

		#endregion

		#region "Mouse Events"

		/// <summary>
		/// Determines which tab the cursor is over, sends the appropriate MouseEvent to it and caches the tab.
		/// When the cached tab doesn't match the one the cursor is over then MouseLeave is invoked for this tab
		/// and MouseEnter is invoked for the new tab. If the tab has not changed then the MouseMove event is invoked.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks></remarks>
		private void CheckMousePosition(MouseEventArgs e) {
			//This test is done to handle a user attempting to drag a tab to a new location.
			//Without this test in place DragDrop would not be initiated when a user clicks and starts
			//to drag at a point close to a tabs edge.
			if((e.Button & MouseButtons.Left) == MouseButtons.Left && this.m_mouseOverControl != null) {
				this.m_mouseOverControl.InvokeMouseMove(e);
				return;
			}
			foreach(MdiTab tab in this.Tabs) {
				if(tab.Visible && tab.HitTest(e.X, e.Y)) {
					if(!ReferenceEquals(tab, this.m_mouseOverControl)) {
						if(this.m_mouseOverControl != null) {
							this.m_mouseOverControl.InvokeMouseLeave(new EventArgs());
						}
						this.MouseOverControl = tab;
						tab.InvokeMouseEnter(new EventArgs());
					} else {
						tab.InvokeMouseMove(e);
					}
					return;
				}
			}
			if(this.LeftScrollTab.Visible && this.LeftScrollTab.HitTest(e.X, e.Y)) {
				if(!ReferenceEquals(this.LeftScrollTab, this.m_mouseOverControl)) {
					if(this.m_mouseOverControl != null) {
						this.m_mouseOverControl.InvokeMouseLeave(new EventArgs());
					}
					this.MouseOverControl = this.LeftScrollTab;
					this.LeftScrollTab.InvokeMouseEnter(new EventArgs());
				} else {
					this.LeftScrollTab.InvokeMouseMove(e);
				}
				return;
			} else if(this.DropDownTab.Visible && this.DropDownTab.HitTest(e.X, e.Y)) {
				if(!ReferenceEquals(this.DropDownTab, this.m_mouseOverControl)) {
					if(this.m_mouseOverControl != null) {
						this.m_mouseOverControl.InvokeMouseLeave(new EventArgs());
					}
					this.MouseOverControl = this.DropDownTab;
					this.DropDownTab.InvokeMouseEnter(new EventArgs());
					if(this.ShowTabToolTip) {
						this.UpdateToolTip("Tab List");
					}
				} else {
					this.DropDownTab.InvokeMouseMove(e);
				}
				return;
			} else if(this.RightScrollTab.Visible && this.RightScrollTab.HitTest(e.X, e.Y)) {
				if(!ReferenceEquals(this.RightScrollTab, this.m_mouseOverControl)) {
					if(this.m_mouseOverControl != null) {
						this.m_mouseOverControl.InvokeMouseLeave(new EventArgs());
					}
					this.MouseOverControl = this.RightScrollTab;
					this.RightScrollTab.InvokeMouseEnter(new EventArgs());
				} else {
					this.RightScrollTab.InvokeMouseMove(e);
				}
				return;
			} else if(this.MdiNewTabVisible && this.MdiNewTab.HitTest(e.X, e.Y)) {
				if(!ReferenceEquals(this.MdiNewTab, this.m_mouseOverControl)) {
					if(this.m_mouseOverControl != null) {
						this.m_mouseOverControl.InvokeMouseLeave(new EventArgs());
					}
					this.MouseOverControl = this.MdiNewTab;
					this.MdiNewTab.InvokeMouseEnter(new EventArgs());
					if(this.ShowTabToolTip) {
						this.UpdateToolTip(this.NewTabToolTipText);
					}
				} else {
					this.MdiNewTab.InvokeMouseMove(e);
				}
				return;
			}
			if(this.m_mouseOverControl != null) {
				this.m_mouseOverControl.InvokeMouseLeave(new EventArgs());
			}
			this.m_mouseOverControl = null;
			this.m_toolTip.Hide(this);
			CTXRightMenu.Hide();
		}
		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			this.CheckMousePosition(e);
		}
		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			if(this.m_mouseOverControl != null) {
				this.m_mouseOverControl.InvokeMouseDown(e);
			}
		}
		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			if(this.MouseOverControl != null) {
				this.MouseOverControl.InvokeMouseUp(e);
			}
		}
		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);
			//Call each tab's MouseLeave method so that it can properly animate a fade out and
			//reset it's current frame to zero.
			foreach(MdiTab tab in this.Tabs) {
				tab.InvokeMouseLeave(e);
			}
			this.LeftScrollTab.InvokeMouseLeave(e);
			this.DropDownTab.InvokeMouseLeave(e);
			this.RightScrollTab.InvokeMouseLeave(e);
			this.MdiNewTab.InvokeMouseLeave(e);
			this.MouseOverControl = null;
			Invalidate();
		}
		internal void UpdateToolTip(string tipText) {
			this.m_toolTip.Hide(this);
			this.m_toolTip.Active = false;
			this.m_toolTip.Active = true;
			Point location = Cursor.Position;
			location.Y = (location.Y + (Cursor.Size.Height - Cursor.Current.HotSpot.Y));
			this.m_toolTip.Show(tipText, this, base.PointToClient(location), this.m_toolTip.AutoPopDelay);
		}

		#endregion

		#region "Drag Drop Methods"

		protected override void OnDragOver(DragEventArgs drgevent) {
			if(!drgevent.Data.GetDataPresent(typeof(MdiTab))) {
				drgevent.Effect = DragDropEffects.None;
				return;
			}
			this.IsDragging = true;
			drgevent.Effect = DragDropEffects.Move;
			Point pt = PointToClient(new Point(drgevent.X, drgevent.Y));
			this.DragDropHitTest(pt.X, pt.Y);
			Invalidate();
		}
		private void DragDropHitTest(int mouseX, int mouseY) {
			//		MdiTab tab = null;
			foreach(MdiTab tab in this.Tabs) {
				if(tab.CanDrag && tab.Visible) {
					//Only test mouse position if the tab is visible and can be dragged (which signifies
					//whether or not the tab can be reordered)
					if(tab.HitTest(mouseX, mouseY)) {
						int activeIndex = this.Tabs.IndexOf(this.ActiveTab);
						if(tab == null) {
							//This should never happen but check just in case.
							this.m_indexOfTabForDrop = activeIndex;
						} else if(ReferenceEquals(tab, this.ActiveTab)) {
							//When starting a drag operation this should be the first test hit. We set the index
							//to the active tab and setup the direction so that the indicator is displayed one
							//the correct side of the tab.
							this.m_indexOfTabForDrop = activeIndex;
							if(RightToLeft == RightToLeft.Yes) {
								this.m_dragDirection = ScrollDirection.Right;
							} else {
								this.m_dragDirection = ScrollDirection.Left;
							}
						} else {
							//The code below determines the index at which the tab being currently dragged
							//should be dropped at based on the direction the tab is being dragged (determined
							//by the active tab's current index) as well as splitting the tabs 80/20.
							//It is easier to understand seeing it in action than it is to explain it.
							int currentIndex = this.Tabs.IndexOf(tab);
							if(RightToLeft == RightToLeft.Yes) {
								if(currentIndex <= activeIndex) {
									int a = (int)(tab.Location.X + (tab.Width * 0.2));
									this.m_dragDirection = ScrollDirection.Right;
									if(mouseX < a) {
										if(currentIndex + 1 < this.Tabs.Count) {
											this.m_indexOfTabForDrop = currentIndex + 1;
										}
									} else {
										this.m_indexOfTabForDrop = currentIndex;
									}
								} else {
									int b = (int)(tab.Location.X + (tab.Width * 0.8));
									this.m_dragDirection = ScrollDirection.Left;
									if(mouseX < b) {
										if(currentIndex < this.Tabs.Count) {
											this.m_indexOfTabForDrop = currentIndex;
										}
									} else {
										if(activeIndex + 1 != currentIndex) {
											this.m_indexOfTabForDrop = currentIndex - 1;
										} else {
											this.m_indexOfTabForDrop = activeIndex;
											this.m_dragDirection = ScrollDirection.Right;
										}
									}
								}
							} else {
								if(currentIndex <= activeIndex) {
									int a = (int)(tab.Location.X + (tab.Width * 0.8));
									this.m_dragDirection = ScrollDirection.Left;
									if(mouseX < a) {
										this.m_indexOfTabForDrop = currentIndex;
									} else {
										if(currentIndex + 1 < this.Tabs.Count) {
											this.m_indexOfTabForDrop = currentIndex + 1;
										}
									}
								} else {
									int b = (int)(tab.Location.X + (tab.Width * 0.2));
									this.m_dragDirection = ScrollDirection.Right;
									if(mouseX < b) {
										if(activeIndex + 1 != currentIndex) {
											this.m_indexOfTabForDrop = currentIndex - 1;
										} else {
											this.m_indexOfTabForDrop = activeIndex;
											this.m_dragDirection = ScrollDirection.Left;
										}
									} else {
										if(currentIndex < this.Tabs.Count) {
											this.m_indexOfTabForDrop = currentIndex;
										}
									}
								}
							}
						}
						break; // TODO: might not be correct. Was : Exit For
					}
					// tab.HitTest
				}
				//tab.Visible
			}
			//tab
		}
		protected override void OnDragDrop(DragEventArgs drgevent) {
			if(drgevent.Data.GetDataPresent(typeof(MdiTab))) {
				MdiTab tab = (MdiTab)drgevent.Data.GetData(typeof(MdiTab));
				if(drgevent.Effect == DragDropEffects.Move) {
					//When the tab is dropped it is removed from the collection and then inserted back in at the
					//designated index. The cooresponding menu item for the drop down is also moved to the same position
					//in the menu's item collection.
					if(this.m_tabs.IndexOf(tab) != this.m_indexOfTabForDrop) {
						this.Tabs.Remove(tab);
						this.Tabs.Insert(this.m_indexOfTabForDrop, tab);
						this.OnMdiTabIndexChanged(new EventArgs());
						PerformLayout();
						Form f = tab.Form;
						foreach(MdiMenuItem mi in this.DropDownTab.m_mdiMenu.Items) {
							if(ReferenceEquals(mi.Form, f)) {
								this.DropDownTab.m_mdiMenu.Items.Remove(mi);
								this.DropDownTab.m_mdiMenu.Items.Insert(this.m_indexOfTabForDrop, mi);
								break; // TODO: might not be correct. Was : Exit For
							}
						}
						//After this operation need to determine if the left or right scroll tab should be enabled or not.
						this.LeftScrollTab.Enabled = !this.IsFirstTabActive;
						this.RightScrollTab.Enabled = !this.IsLastTabActive;
					}
				}
			}
			this.IsDragging = false;
			Invalidate();
		}

		#endregion

		#region "ContextMenu methods"

		private void AddMdiItem(Form f, MdiTab tab) {
			MdiMenuItem item = new MdiMenuItem(tab, this.MenuItemClick);
			Bitmap bmp = new Bitmap(f.Icon.Width, f.Icon.Height, PixelFormat.Format32bppArgb);
			using(Graphics g = Graphics.FromImage(bmp)) {
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.DrawIcon(f.Icon, 0, 0);
			}
			item.Image = bmp;
			item.Text = f.Text;
			this.m_dropDownScrollTab.m_mdiMenu.Items.Add(item);
		}
		private void RemoveMdiItem(Form f) {
			foreach(MdiMenuItem mi in this.m_dropDownScrollTab.m_mdiMenu.Items) {
				if(ReferenceEquals(mi.Form, f)) {
					this.m_dropDownScrollTab.m_mdiMenu.Items.Remove(mi);
					break; // TODO: might not be correct. Was : Exit For
				}
			}
		}
		private void MenuItemClick(object sender, EventArgs e) {
			MdiMenuItem mItem = sender as MdiMenuItem;
			if(mItem != null) {
				ScrollDirection direction = default(ScrollDirection);
				int activeIndex = this.Tabs.IndexOf(this.ActiveTab);
				int clickedTabIndex = 0;
				mItem.Form.Activate();
				clickedTabIndex = this.Tabs.IndexOf(this.ActiveTab);
				if(activeIndex > clickedTabIndex) {
					direction = ScrollDirection.Left;
				} else {
					direction = ScrollDirection.Right;
				}
				this.UpdateTabVisibility(direction);
			}
		}

		#endregion

		#region "Navigation button Events"

		private void leftTabScroll_ScrollTab(ScrollDirection direction) {
			this.ScrollTabHandler(direction);
		}
		private void rightTabScroll_ScrollTab(ScrollDirection direction) {
			this.ScrollTabHandler(direction);
		}
		/// <summary>
		/// The scroll handler determines the index of the next tab in the direction the user is
		/// intending to scroll. It then calls that tab's Form's Activate method.
		/// </summary>
		/// <param name="direction"></param>
		/// <remarks></remarks>
		private void ScrollTabHandler(ScrollDirection direction) {
			int nextIndex = 0;
			if(direction == ScrollDirection.Left) {
				nextIndex = this.Tabs.FirstVisibleTabIndex;
				nextIndex -= 1;
			} else {
				nextIndex = this.Tabs.LastVisibleTabIndex;
				nextIndex += 1;
			}
			if(nextIndex > this.Tabs.Count - 1) {
				nextIndex = this.Tabs.Count - 1;
			} else if(nextIndex < 0) {
				nextIndex = 0;
			}
			this.Tabs[nextIndex].Form.Activate();
			this.UpdateTabVisibility(direction);
		}
		private void UpdateTabVisibility(ScrollDirection direction) {
			int tabsToShow = 0;
			int leftTabIndex = 0;
			int rightTabIndex = 0;
			int activeTabIndex = 0;
			int tabAreaWidth = this.AdjustAvailableWidth();
			//tabAreaWidth = Me.DisplayRectangle.Width
			//'Must subtract the area occupied by the visible scroll tabs to get the
			//'true area the form tabs can occupy.
			//If Me.LeftScrollTab.Visible Then
			//    tabAreaWidth -= Me.LeftScrollTab.Width
			//End If
			//If Me.DropDownTab.Visible Then
			//    tabAreaWidth -= Me.DropDownTab.Width
			//End If
			//If Me.RightScrollTab.Visible Then
			//    tabAreaWidth -= Me.RightScrollTab.Width
			//End If
			//If Me.MdiNewTabVisible Then
			//    tabAreaWidth -= Me.m_newTab.Width
			//End If
			//Based on the minimum width each tab can be determine the number of tabs
			//that can be shown in the calculated area.
			tabsToShow = tabAreaWidth / this.MinTabWidth;
			activeTabIndex = this.Tabs.IndexOf(this.ActiveTab);
			if(tabsToShow == 1) {
				//If only one can be visible then set this tab's index as the right and left
				leftTabIndex = activeTabIndex;
				rightTabIndex = activeTabIndex;
			} else if(tabsToShow >= this.Tabs.Count) {
				//If all of the tabs can be visible then set the left index to 0 and 
				//the right to the last tab's index
				leftTabIndex = 0;
				rightTabIndex = this.Tabs.Count - 1;
			} else if(direction == ScrollDirection.Left) {
				//Tries to make the active tab the last tab visible. If this calculation puts the left
				//index past zero (negative) then it resets itself so that it shows the number of tabsToShow
				//starting at index zero.
				leftTabIndex = activeTabIndex - (tabsToShow - 1);
				if(leftTabIndex >= 0) {
					rightTabIndex = activeTabIndex;
				} else {
					rightTabIndex = activeTabIndex - leftTabIndex;
					leftTabIndex = 0;
				}
			} else if(direction == ScrollDirection.Right) {
				//Tries to make the active tab the first tab visible. If this calculation puts the right
				//index past the number of tabs in the collection then it resets itself so that it shows
				//the number of tabsToShow ending at the last index in the collection.
				rightTabIndex = activeTabIndex + (tabsToShow - 1);
				if(rightTabIndex < this.Tabs.Count) {
					leftTabIndex = activeTabIndex;
				} else {
					rightTabIndex = this.Tabs.Count - 1;
					leftTabIndex = rightTabIndex - (tabsToShow - 1);
				}
			} else {
				//The resize event is handled by this section of code. It tries to evenly distribute the hiding
				//and showing of tabs between each side of the active tab. If you have 5 tabs open and the third
				//one is active and you resize the window smaller and smaller you will notice that the last tab
				//on the right disappears first. Then as you continue to resize the first tab on the left 
				//disappears, then the last one on the right and then the first one on the left. At this point
				//only one tab is left visible. If you now resize the window larger a tab on the left will become
				//visible and then one on the right, then the left and then the right.
				int l = tabsToShow / 2;
				int r = 0;
				if(tabsToShow == this.Tabs.VisibleCount) {
					return;
				}
				if(tabsToShow < this.Tabs.VisibleCount) {
					this.SetScrollTabVisibility();
					this.AdjustAvailableWidth();
				}
				if(tabsToShow % 2 == 0) {
					r = l - 1;
				} else {
					r = l;
				}
				if(activeTabIndex - this.Tabs.FirstVisibleTabIndex <= this.Tabs.LastVisibleTabIndex - activeTabIndex) {
					leftTabIndex = activeTabIndex - l;
					if(leftTabIndex >= 0) {
						rightTabIndex = activeTabIndex + r;
					} else {
						rightTabIndex = tabsToShow - 1;
						leftTabIndex = 0;
					}
					if(rightTabIndex >= this.Tabs.Count) {
						rightTabIndex = this.Tabs.Count - 1;
						leftTabIndex = rightTabIndex - (tabsToShow - 1);
					}
				} else {
					rightTabIndex = activeTabIndex + r;
					if(rightTabIndex < this.Tabs.Count) {
						leftTabIndex = activeTabIndex - l;
					} else {
						rightTabIndex = this.Tabs.Count - 1;
						leftTabIndex = rightTabIndex - (tabsToShow - 1);
					}
					if(leftTabIndex < 0) {
						leftTabIndex = 0;
						rightTabIndex = tabsToShow - 1;
					}
				}
			}
			//Using the left and right indeces determined above iterate through the tab collection
			//and hide the tab if is not within the range of indeces and show it if it is.
			foreach(MdiTab tab in this.Tabs) {
				int tabPos = this.Tabs.IndexOf(tab);
				if(tabPos <= rightTabIndex & tabPos >= leftTabIndex) {
					tab.Visible = true;
				} else {
					tab.Visible = false;
				}
			}
			//The active tab needs to always be visible. This code ensures that even when the main form
			//is resized to a very small width that this tab remains visible and the control draws correctly.
			if(this.ActiveTab != null) {
				this.ActiveTab.Visible = true;
			}
			//Figure each scroll tab's visiblity and perform a layout to set each tab's size and location.
			this.SetScrollTabVisibility();
			PerformLayout();
		}
		private void SetScrollTabVisibility() {
			//If tabs are hidden then the left and right scroll tabs need to be displayed.
			//If there are more than one tab open then the drop down tab needs to be displayed.
			//DesignMode is checked so that these tabs will be visible in the design window.
			if(!DesignMode) {
				bool multipleTabs = this.Tabs.Count > 1;

                // 2019-11-01 TODO By Jaeyong Park : Temporarily invisible due to tab shift bug - Revert
                #region Before
                bool hiddenTabs = this.Tabs.VisibleCount < this.Tabs.Count;
				this.LeftScrollTab.Visible = hiddenTabs;
				this.RightScrollTab.Visible = hiddenTabs;
				#endregion
				#region After
//				this.LeftScrollTab.Visible = false;
//				this.RightScrollTab.Visible = false;
				#endregion

				this.DropDownTab.Visible = multipleTabs;
			}
		}
		private int AdjustAvailableWidth() {
			int w = this.DisplayRectangle.Width;
			//Must subtract the area occupied by the visible scroll tabs to get the
			//true area the form tabs can occupy.
			if(this.LeftScrollTab.Visible) {
				w -= this.LeftScrollTab.Width;
			}
			if(this.DropDownTab.Visible) {
				w -= this.DropDownTab.Width;
			}
			if(this.RightScrollTab.Visible) {
				w -= this.RightScrollTab.Width;
			}
			if(this.MdiNewTabVisible) {
				w -= this.m_newTab.Width;
			}
			return w;
		}

		#endregion

		#region "Resize"

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			this.UpdateTabVisibility(ScrollDirection.None);
		}

		#endregion

		#region "Control Events"

		protected internal virtual void OnMdiTabAdded(MdiTabStripTabEventArgs e) {
			if(this.MdiTabAdded != null) {
				this.MdiTabAdded(this, e);
			}
		}
		protected internal virtual void OnMdiTabRemoved(MdiTabStripTabEventArgs e) {
			if(this.MdiTabRemoved != null) {
				this.MdiTabRemoved(this, e);
			}
		}
		protected internal virtual void OnMdiTabClicked(MdiTabStripTabClickedEventArgs e) {
			if(this.MdiTabClicked != null) {
				this.MdiTabClicked(this, e);
			}
            
            // 2010-12-10 16:42:52 NOTE By Jaeyong Park : add mouse event : wheel-click to close current tab & right-click to display the context menu
            #region
            switch (e.MouseEvent.Button) {
				case MouseButtons.Middle:
					e.ClickedTab.Form.Close();
					break;
				case MouseButtons.Right:
					//CTXRightMenu.Show(this, PointToClient(e.MouseEvent.Location));
					this.ActiveTab = e.ClickedTab;
					CTXRightMenu.Show(this, e.MouseEvent.Location);
					break;
			}
			#endregion
		}
		protected internal virtual void HideCTXMenu() {
			CTXRightMenu.Hide();
		}
		protected internal virtual void OnMdiTabIndexChanged(EventArgs e) {
			if(this.MdiTabIndexChanged != null) {
				this.MdiTabIndexChanged(this, e);
			}
		}
		protected internal virtual void OnCurrentMdiTabChanged(EventArgs e) {
			if(this.CurrentMdiTabChanged != null) {
				this.CurrentMdiTabChanged(this, e);
			}
		}
		protected internal void OnMdiNewTabClick() {
			if(this.MdiNewTabClicked != null) {
				this.MdiNewTabClicked(this.m_newTab, new EventArgs());
			}
		}

		#endregion

		#endregion

		#region "LayoutEngine Class"

		private class TabStripLayoutEngine : LayoutEngine {
			public override bool Layout(object container, LayoutEventArgs layoutEventArgs) {
				MdiTabStrip strip = (MdiTabStrip)container;
				int proposedWidth = strip.MaxTabWidth;
				int visibleCount = strip.Tabs.VisibleCount;
				Rectangle stripRectangle = strip.DisplayRectangle;
				int tabAreaWidth = stripRectangle.Width;
				Point nextLocation = stripRectangle.Location;
				int leftOver = 0;
				int visibleIndex = 0;
				//If the MdiTabStrip's DisplayRectangle width is less than 1 or there are no tabs
				//to display then don't try to layout the control.
				if(tabAreaWidth < 1 | visibleCount < 1) {
					//If the MdiNewTab is visible then we need to layout it's position.
					this.LayoutMdiNewTab(strip, nextLocation, stripRectangle.Height + strip.Margin.Bottom);
					return false;
				}
				//For each of the scroll tabs need to determine their location and height (the width
				//is set in the MdiTabStrip constructor and is fixed). The width of the scroll tab
				//also needs to be subtracted from the tabAreaWidth so that the true tab area can be
				//properly calculated.
				if(strip.RightToLeft == RightToLeft.Yes) {
					nextLocation.X = stripRectangle.Right;
					if(strip.RightScrollTab.Visible) {
						nextLocation = this.MirrorScrollTab(strip.RightScrollTab, nextLocation, stripRectangle.Height);
						tabAreaWidth -= strip.RightScrollTab.Width;
					}
					if(strip.DropDownTab.Visible) {
						nextLocation = this.MirrorScrollTab(strip.DropDownTab, nextLocation, stripRectangle.Height);
						tabAreaWidth -= strip.DropDownTab.Width;
					}
					if(strip.LeftScrollTab.Visible) {
						nextLocation = this.MirrorScrollTab(strip.LeftScrollTab, nextLocation, stripRectangle.Height);
						tabAreaWidth -= strip.LeftScrollTab.Width;
					}
				} else {
					if(strip.LeftScrollTab.Visible) {
						nextLocation = this.SetScrollTab(strip.LeftScrollTab, nextLocation, stripRectangle.Height);
						tabAreaWidth -= strip.LeftScrollTab.Width;
					}
					if(strip.DropDownTab.Visible) {
						nextLocation = this.SetScrollTab(strip.DropDownTab, nextLocation, stripRectangle.Height);
						tabAreaWidth -= strip.DropDownTab.Width;
					}
					if(strip.RightScrollTab.Visible) {
						nextLocation = this.SetScrollTab(strip.RightScrollTab, nextLocation, stripRectangle.Height);
						tabAreaWidth -= strip.RightScrollTab.Width;
					}
				}
				if(strip.MdiNewTabVisible) {
					tabAreaWidth -= strip.MdiNewTab.Width;
				}
				//If the total width of all visible tabs is greater than the total area available for the
				//tabs then need to set the proposed width of each tab. We also retreive the remainder for use below.
				if(visibleCount * proposedWidth > tabAreaWidth) {
					//The \ operator returns an Integer value and disgards the remainder.
					proposedWidth = tabAreaWidth / visibleCount;
					leftOver = tabAreaWidth % visibleCount;
				}
				//Set the tabWidth to the larger of the two variables; proposed width and minimum width.
				proposedWidth = Math.Max(proposedWidth, strip.MinTabWidth);
				//Set each visible tab's width and location and perform layout on each tab.
				foreach(MdiTab tab in strip.Tabs) {
					if(tab.Visible) {
						Size tabSize = new Size(proposedWidth, stripRectangle.Height);
						//Suspend the tab's layout so that we can set it's properties without triggering
						//extraneous layouts. Once all changes are made then we can PerformLayout.
						tab.SuspendLayout();
						//To allow the tabs to completely fill the total available width we adjust the width
						//of the tabs (starting with the first tab) by one. The number of tabs that need to be
						//adjusted is determined by the leftOver variable that was calculated above.
						if(proposedWidth < strip.MaxTabWidth && visibleIndex < (leftOver - 1)) {
							tabSize.Width = proposedWidth + 1;
						}
						if(strip.RightToLeft == RightToLeft.Yes) {
							nextLocation.X -= tabSize.Width;
							tab.Size = tabSize;
							tab.Location = nextLocation;
						} else {
							tab.Size = tabSize;
							tab.Location = nextLocation;
							nextLocation.X += tabSize.Width;
						}
						visibleIndex += 1;
						tab.ResumeLayout();
						tab.PerformLayout();
					}
				}
				this.LayoutMdiNewTab(strip, nextLocation, stripRectangle.Height);
				//Return False because we don't want layout to be performed again by the parent of the container
				return false;
			}
			private void LayoutMdiNewTab(MdiTabStrip strip, Point position, int height) {
				if(strip.MdiNewTabVisible) {
					if(strip.RightToLeft == RightToLeft.Yes) {
						this.MirrorNewTab(strip.MdiNewTab, position, height);
					} else {
						this.SetNewTab(strip.MdiNewTab, position, height);
					}
				}
			}
			private Point SetScrollTab(MdiScrollTab tab, Point position, int height) {
				if(tab.Visible) {
					tab.Location = position;
					tab.Height = height;
					tab.PerformLayout();
				}
				return new Point(position.X + tab.Width, position.Y);
			}
			private Point SetNewTab(MdiNewTab tab, Point position, int height) {
				tab.Location = position;
				tab.Height = height;
				tab.PerformLayout();
				return new Point(position.X + tab.Width, position.Y);
			}
			private Point MirrorScrollTab(MdiScrollTab tab, Point position, int height) {
				if(tab.Visible) {
					tab.Location = new Point(position.X - tab.Width, position.Y);
					tab.Height = height;
					tab.PerformLayout();
				}
				return tab.Location;
			}
			private Point MirrorNewTab(MdiNewTab tab, Point position, int height) {
				tab.Location = new Point(position.X - tab.Width, position.Y);
				tab.Height = height;
				tab.PerformLayout();
				return tab.Location;
			}
		}

		#endregion
	}

	/// <summary>
	/// Specifies the direction in which a scroll event initiated.
	/// </summary>
	public enum ScrollDirection {
		None = 0,
		Left = 1,
		Right = 2
	}

	/// <summary>
	/// Specifies the type of tab the <see cref="MdiScrollTab"/> object has been initialized as.
	/// </summary>
	public enum ScrollTabType {
		ScrollTabLeft = 1,
		ScrollTabRight = 2,
		ScrollTabDropDown = 3
	}

	/// <summary>
	/// Specifies the desired permanance for the tabs of a <see cref="MdiTabStrip"/>.
	/// </summary>
	public enum MdiTabPermanence {
		None = 0,
		First = 1,
		LastOpen = 2
	}

	/// <summary>
	/// Specifies whether or not to obey each form's individual property settings or force each to form
	/// to always be maximized.
	/// </summary>
	public enum MdiChildWindowState {
		Normal = 1,
		Maximized = 2
	}

	internal enum AnimationType {
		None = 0,
		FadeIn = 1,
		FadeOut = 2
	}
}