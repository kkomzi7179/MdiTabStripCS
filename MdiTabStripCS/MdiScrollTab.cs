#region Using

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

namespace MdiTabStripCS {
	[ToolboxItem(false)]
	public class MdiScrollTab : MdiTab {
		#region "Fields"

		private bool m_mouseDown;
		private ScrollTabType m_scrollTabType = ScrollTabType.ScrollTabLeft;
		internal MdiTabStripDropDown m_mdiMenu { get; set; }
		private bool m_isDroppedDown;

		#endregion

		private bool m_dropDownByTab;

		#region "Events"

		internal event ScrollTabEventHandler ScrollTab;

		internal delegate void ScrollTabEventHandler(ScrollDirection direction);

		#endregion

		#region "Constructor/Destructor"

		public MdiScrollTab(MdiTabStrip owner, ScrollTabType scrollType)
			: base(owner) {
			this.m_mdiMenu = new MdiTabStripDropDown();
			this.m_scrollTabType = scrollType;
			this.m_mdiMenu.Closed += this.m_mdiMenu_Closed;
			this.m_mdiMenu.Opened += this.m_mdiMenu_Opened;
			owner.RightToLeftChanged += this.OnOwnerRightToLeftChanged;
			//We initially hide the scroll tab
			Visible = false;
		}
		protected override void Dispose(bool disposing) {
			if(disposing) {
				this.m_mdiMenu.Dispose();
				if(ParentInternal != null) {
					ParentInternal.RightToLeftChanged -= this.OnOwnerRightToLeftChanged;
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region "Properties"

		internal MdiTabStripDropDown MdiMenu {
			get {
				return this.m_mdiMenu;
			}
		}
		internal ScrollTabType ScrollTabType {
			get {
				return this.m_scrollTabType;
			}
		}

		#endregion

		#region "Methods"

		private void m_mdiMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e) {
			if(!IsMouseOver) {
				this.m_isDroppedDown = false;
			}
		}
		private void m_mdiMenu_Opened(object sender, EventArgs e) {
			this.m_isDroppedDown = true;
		}

		#region "Paint methods"

		internal override void DrawControl(Graphics g) {
			this.DrawTab(g);
		}
		private void DrawTab(Graphics g) {
			if(this.m_scrollTabType == ScrollTabType.ScrollTabLeft) {
				this.DrawLeftGlyph(g);
			} else if(this.m_scrollTabType == ScrollTabType.ScrollTabRight) {
				this.DrawRightGlyph(g);
			} else {
				this.DrawDropDownGlyph(g);
			}
		}
		private void DrawLeftGlyph(Graphics g) {
			Rectangle rect = new Rectangle(Left + ((Width / 2) - 6), Top + 13, 11, 5);
			Point[] lines1 = {
			new Point(rect.X + 4, rect.Y), new Point(rect.X + 5, rect.Y), new Point(rect.X + 3, rect.Y + 2), new Point(rect.X + 5, rect.Y + 4), new Point(rect.X + 4, rect.Y + 4),
			new Point(rect.X + 2, rect.Y + 2), new Point(rect.X + 4, rect.Y)
		};
			Point[] lines2 = {
			new Point(rect.X + 8, rect.Y), new Point(rect.X + 9, rect.Y), new Point(rect.X + 7, rect.Y + 2), new Point(rect.X + 9, rect.Y + 4), new Point(rect.X + 8, rect.Y + 4),
			new Point(rect.X + 6, rect.Y + 2), new Point(rect.X + 8, rect.Y)
		};
			this.DrawChevron(g, lines1, lines2);
		}
		private void DrawRightGlyph(Graphics g) {
			Rectangle rect = new Rectangle(Left + ((Width / 2) - 5), Top + 13, 11, 5);
			Point[] lines1 = {
			new Point(rect.X + 1, rect.Y), new Point(rect.X + 2, rect.Y), new Point(rect.X + 4, rect.Y + 2), new Point(rect.X + 2, rect.Y + 4), new Point(rect.X + 1, rect.Y + 4),
			new Point(rect.X + 3, rect.Y + 2), new Point(rect.X + 1, rect.Y)
		};
			Point[] lines2 = {
			new Point(rect.X + 5, rect.Y), new Point(rect.X + 6, rect.Y), new Point(rect.X + 8, rect.Y + 2), new Point(rect.X + 6, rect.Y + 4), new Point(rect.X + 5, rect.Y + 4),
			new Point(rect.X + 7, rect.Y + 2), new Point(rect.X + 5, rect.Y)
		};
			this.DrawChevron(g, lines1, lines2);
		}
		private void DrawChevron(Graphics g, Point[] chevron1, Point[] chevron2) {
			g.SmoothingMode = SmoothingMode.None;
			using(Pen glyphPen = new Pen(TabForeColor, 1)) {
				if(!Enabled) {
					Color c = ParentInternal.InactiveTabForeColor;
					int luminosity = 0;
					int num1 = c.R;
					int num2 = c.G;
					int num3 = c.B;
					int num4 = Math.Max(Math.Max(num1, num2), num3);
					int num5 = Math.Min(Math.Min(num1, num2), num3);
					int num6 = (num4 + num5);
					luminosity = (((num6 * 240) + 255) / 510);
					if(luminosity == 0) {
						glyphPen.Color = ControlPaint.LightLight(c);
					} else if(luminosity < 120) {
						glyphPen.Color = ControlPaint.Light(c, 0.5f);
					} else {
						glyphPen.Color = ControlPaint.Light(c, 0.5f);
					}
				}
				glyphPen.StartCap = LineCap.Square;
				glyphPen.EndCap = LineCap.Square;
				g.DrawLines(glyphPen, chevron1);
				g.DrawLines(glyphPen, chevron2);
			}
			g.SmoothingMode = SmoothingMode.AntiAlias;
		}
		private void DrawDropDownGlyph(Graphics g) {
			Rectangle rect = new Rectangle(Left + ((Width / 2) - 3), Top + 12, 4, 6);
			Point[] dropDown = { new Point(rect.X, rect.Y + 1), new Point(rect.X + 3, rect.Y + 5), new Point(rect.X + 6, rect.Y + 1) };
			using(SolidBrush glyphBrush = new SolidBrush(TabForeColor)) {
				g.FillPolygon(glyphBrush, dropDown);
			}
		}

		#endregion

		#region "Mouse methods"

		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);
			this.m_isDroppedDown = false;
		}
		protected override void OnMouseDown(MouseEventArgs e) {
			this.m_mouseDown = true;
		}
		protected override void OnMouseUp(MouseEventArgs e) {
			if(!Enabled) {
				return;
			}
			if(this.m_mouseDown) {
				this.m_mouseDown = false;
				if(this.m_scrollTabType == ScrollTabType.ScrollTabLeft) {
					ScrollDirection direction = ScrollDirection.Left;
					if(ParentInternal.RightToLeft == RightToLeft.Yes) {
						direction = ScrollDirection.Right;
					}
					if(this.ScrollTab != null) {
						this.ScrollTab(direction);
					}
				} else if(this.m_scrollTabType == ScrollTabType.ScrollTabRight) {
					ScrollDirection direction = ScrollDirection.Right;
					if(ParentInternal.RightToLeft == RightToLeft.Yes) {
						direction = ScrollDirection.Left;
					}
					if(this.ScrollTab != null) {
						this.ScrollTab(direction);
					}
				} else {
					if(this.m_isDroppedDown) {
						this.m_isDroppedDown = false;
					} else {
						Point dropPoint = default(Point);
						if(ParentInternal.RightToLeft == RightToLeft.Yes) {
							dropPoint = ParentInternal.PointToScreen(new Point(Right - this.m_mdiMenu.Width, ParentInternal.Height - 5));
						} else {
							dropPoint = ParentInternal.PointToScreen(new Point(Left, ParentInternal.Height - 5));
						}
						this.m_mdiMenu.Show(dropPoint, ToolStripDropDownDirection.Default);
					}
				}
			}
		}
		protected override void OnMouseMove(MouseEventArgs e) {
			//Not implemeted, but overridden to bypass inherited functionality
		}

		#endregion

		#endregion

		#region "Event Handlers"

		private void OnOwnerRightToLeftChanged(object sender, EventArgs e) {
			//Need to know when the MdiTabStrip RightToLeft property has changed so that the drop down menu's
			//RightToLeft property is set to match it.
			this.m_mdiMenu.RightToLeft = ParentInternal.RightToLeft;
		}

		#endregion
	}
}