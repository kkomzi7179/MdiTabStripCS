#region Using

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

using MdiTabStripCS.Properties;

#endregion

namespace MdiTabStripCS.Designer {
	[ToolboxItem(false)]
	public class MdiTabTemplateControl : Control {
		private ActiveMdiTabProperties withEventsField__activeTemplate = new ActiveMdiTabProperties();
		private ActiveMdiTabProperties _activeTemplate {
			get { return this.withEventsField__activeTemplate; }
			set {
				this.withEventsField__activeTemplate = value;
				/*if(this.withEventsField__activeTemplate != null) {
					this.withEventsField__activeTemplate.PropertyChanged -= this._activeTemplate_PropertyChanged;
				}
				this.withEventsField__activeTemplate = value;
				if(this.withEventsField__activeTemplate != null) {
					this.withEventsField__activeTemplate.PropertyChanged += this._activeTemplate_PropertyChanged;
				}*/
			}
		}
		private InactiveMdiTabProperties withEventsField__inactiveTemplate = new InactiveMdiTabProperties();
		private InactiveMdiTabProperties _inactiveTemplate {
			get { return this.withEventsField__inactiveTemplate; }
			set {
				this.withEventsField__inactiveTemplate = value;
				/*if(this.withEventsField__inactiveTemplate != null) {
					this.withEventsField__inactiveTemplate.PropertyChanged -= this._inactiveTemplate_PropertyChanged;
				}
				this.withEventsField__inactiveTemplate = value;
				if(this.withEventsField__inactiveTemplate != null) {
					this.withEventsField__inactiveTemplate.PropertyChanged += this._inactiveTemplate_PropertyChanged;
				}*/
			}
		}
		private MdiTabProperties withEventsField__mouseOverTemplate = new MdiTabProperties();
		private MdiTabProperties _mouseOverTemplate {
			get { return this.withEventsField__mouseOverTemplate; }
			set {
				this.withEventsField__mouseOverTemplate = value;
				/*if(this.withEventsField__mouseOverTemplate != null) {
					this.withEventsField__mouseOverTemplate.PropertyChanged -= this._mouseOverTemplate_PropertyChanged;
				}
				this.withEventsField__mouseOverTemplate = value;
				if(this.withEventsField__mouseOverTemplate != null) {
					this.withEventsField__mouseOverTemplate.PropertyChanged += this._mouseOverTemplate_PropertyChanged;
				}*/
			}
		}
		private Point[] m_activeBounds;
		private Point[] m_activeInnerBounds;
		private Point[] m_inactiveBounds;
		private Point[] m_inactiveInnerBounds;
		private Point[] m_mouseOverBounds;
		private Point[] m_mouseOverInnerBounds;
		private Point[] m_closeButtonBounds;
		private Point[] m_closeButtonGlyphBounds;
		private bool _isMouseOverCloseButton;
		internal event TabSelectedEventHandler TabSelected;

		internal delegate void TabSelectedEventHandler(TabSelectedEventArgs e);

		public MdiTabTemplateControl() {
			DoubleBuffered = true;
			this.withEventsField__activeTemplate.PropertyChanged += this._activeTemplate_PropertyChanged;
			this.withEventsField__inactiveTemplate.PropertyChanged += this._inactiveTemplate_PropertyChanged;
			this.withEventsField__mouseOverTemplate.PropertyChanged += this._mouseOverTemplate_PropertyChanged;
			this.GetTabBounds();
			Dock = DockStyle.Top;
		}

		#region Properties

		public ActiveMdiTabProperties ActiveTabTemplate { get { return this._activeTemplate; } }
		public InactiveMdiTabProperties InactiveTabTemplate { get { return this._inactiveTemplate; } }
		public MdiTabProperties MouseOverTabTemplate { get { return this._mouseOverTemplate; } }
		public bool IsMouseOverCloseButton {
			get { return this._isMouseOverCloseButton; }
			set {
				if(this._isMouseOverCloseButton != value) {
					this._isMouseOverCloseButton = value;
					Invalidate();
				}
			}
		}
		protected override Size DefaultSize { get { return new Size(50, 40); } }

		#endregion

		#region Paint

		protected override void OnPaint(PaintEventArgs e) {
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			this.DrawTab(e.Graphics);
		}
		protected virtual void DrawTab(Graphics g) {
			this.DrawActiveTab(g);
			this.DrawInactiveTab(g);
			this.DrawMouseOverTab(g);
		}
		private void DrawActiveTab(Graphics g) {
			Rectangle iconRectangle = default(Rectangle);
			Rectangle textRectangle = new Rectangle(30, 16, 98, Height - 21);
			if(RightToLeft == RightToLeft.Yes) {
				iconRectangle = new Rectangle(180, 13, 17, 17);
				textRectangle.Offset(52, 0);
			} else {
				iconRectangle = new Rectangle(13, 13, 17, 17);
			}
			this.DrawFormIcon(g, iconRectangle);
			this.DrawTabText(g, textRectangle, "Active Tab", this.ActiveTabTemplate.ForeColor, this.ActiveTabTemplate.Font);
			this.DrawCloseButton(g);
		}
		private void DrawInactiveTab(Graphics g) {
			Rectangle iconRectangle = default(Rectangle);
			Rectangle textRectangle = new Rectangle(172, 18, 123, Height - 23);
			if(RightToLeft == RightToLeft.Yes) {
				iconRectangle = new Rectangle(330, 15, 17, 17);
				textRectangle.Offset(37, 0);
			} else {
				iconRectangle = new Rectangle(155, 15, 17, 17);
			}
			this.DrawFormIcon(g, iconRectangle);
			this.DrawTabText(g, textRectangle, "Inactive Tab", this.InactiveTabTemplate.ForeColor, this.InactiveTabTemplate.Font);
		}
		private void DrawMouseOverTab(Graphics g) {
			Rectangle iconRectangle = default(Rectangle);
			Rectangle textRectangle = new Rectangle(322, 18, 123, Height - 23);
			if(RightToLeft == RightToLeft.Yes) {
				iconRectangle = new Rectangle(480, 15, 17, 17);
				textRectangle.Offset(37, 0);
			} else {
				iconRectangle = new Rectangle(305, 15, 17, 17);
			}
			this.DrawFormIcon(g, iconRectangle);
			this.DrawTabText(g, textRectangle, "MouseOver Tab", this.MouseOverTabTemplate.ForeColor, this.MouseOverTabTemplate.Font);
		}
		private void DrawFormIcon(Graphics g, Rectangle rect) {
			Icon icon = Resources.document;
			using(Bitmap bmp = new Bitmap(icon.Width, icon.Height, PixelFormat.Format32bppArgb)) {
				using(Graphics bg = Graphics.FromImage(bmp)) {
					bg.DrawIcon(icon, 0, 0);
				}
				g.DrawImage(bmp, rect);
			}
		}
		private void DrawTabText(Graphics g, Rectangle rect, string tabText, Color textColor, Font tabFont) {
			TextFormatFlags textFlags = TextFormatFlags.WordEllipsis | TextFormatFlags.EndEllipsis;
			if(RightToLeft == RightToLeft.Yes) {
				textFlags = textFlags | TextFormatFlags.Right;
			} else {
				textFlags = textFlags | TextFormatFlags.Left;
			}
			TextRenderer.DrawText(g, tabText, tabFont, rect, textColor, textFlags);
		}
		private void DrawCloseButton(Graphics g) {
			if(this.IsMouseOverCloseButton) {
				this.DrawActiveCloseButton(g);
			} else {
				this.DrawInactiveCloseButton(g);
			}
		}
		private void DrawActiveCloseButton(Graphics g) {
			using(GraphicsPath gp = new GraphicsPath()) {
				gp.AddLines(this.m_closeButtonBounds);
				using(SolidBrush backBrush = new SolidBrush(this.ActiveTabTemplate.CloseButtonBackColor)) {
					g.FillPath(backBrush, gp);
				}
				using(Pen borderPen = new Pen(this.ActiveTabTemplate.CloseButtonBorderColor)) {
					g.DrawPath(borderPen, gp);
				}
			}
			this.DrawCloseButtonGlyph(g, this.ActiveTabTemplate.CloseButtonHotForeColor);
		}
		private void DrawInactiveCloseButton(Graphics g) { this.DrawCloseButtonGlyph(g, this.ActiveTabTemplate.CloseButtonForeColor); }
		private void DrawCloseButtonGlyph(Graphics g, Color glyphColor) {
			g.SmoothingMode = SmoothingMode.None;
			using(GraphicsPath shadow = new GraphicsPath()) {
				Matrix translateMatrix = new Matrix();
				Color shadowColor = Color.FromArgb(120, 130, 130, 130);
				shadow.AddLines(this.m_closeButtonGlyphBounds);
				translateMatrix.Translate(1, 1);
				shadow.Transform(translateMatrix);
				using(SolidBrush shadowBrush = new SolidBrush(shadowColor)) {
					g.FillPath(shadowBrush, shadow);
				}
				using(Pen shadowPen = new Pen(shadowColor)) {
					g.DrawPath(shadowPen, shadow);
				}
			}
			using(GraphicsPath gp = new GraphicsPath()) {
				gp.AddLines(this.m_closeButtonGlyphBounds);
				using(SolidBrush glyphBrush = new SolidBrush(glyphColor)) {
					g.FillPath(glyphBrush, gp);
				}
				using(Pen glyphPen = new Pen(glyphColor)) {
					g.DrawPath(glyphPen, gp);
				}
			}
			g.SmoothingMode = SmoothingMode.AntiAlias;
		}

		#endregion

		#region Paint Background

		protected override void OnPaintBackground(PaintEventArgs e) {
			base.OnPaintBackground(e);
			e.Graphics.FillRectangle(Brushes.White, e.ClipRectangle);
			this.DrawTabBackground(e.Graphics);
		}
		private void DrawTabBackground(Graphics g) {
			this.DrawInactiveTabBackground(g);
			this.DrawMouseOverTabBackground(g);
			this.DrawActiveTabBackground(g);
		}
		private void DrawActiveTabBackground(Graphics g) {
			Rectangle rect = DisplayRectangle;
			Rectangle shadowRectangle = new Rectangle(0, Height - 5, Width, 5);
			Blend shadowBlend = new Blend();
			rect.Offset(0, 8);
			rect.Height -= 8;
			g.SmoothingMode = SmoothingMode.None;
			shadowBlend.Factors = new[] {0f, 0.1f, 0.3f, 0.4f};
			shadowBlend.Positions = new[] {0f, 0.5f, 0.8f, 1f};
			using(GraphicsPath outerPath = new GraphicsPath()) {
				outerPath.AddLines(this.m_activeBounds);
				using(LinearGradientBrush gradientbrush = new LinearGradientBrush(rect, Color.White, this.ActiveTabTemplate.BackColor, LinearGradientMode.Vertical)) {
					Blend bl = new Blend();
					bl.Factors = new[] {0.3f, 0.4f, 0.5f, 1f, 1f};
					bl.Positions = new[] {0f, 0.2f, 0.35f, 0.35f, 1f};
					gradientbrush.Blend = bl;
					g.FillPath(gradientbrush, outerPath);
				}
				using(LinearGradientBrush shadowBrush = new LinearGradientBrush(shadowRectangle, this.ActiveTabTemplate.BackColor, Color.Black, LinearGradientMode.Vertical)) {
					shadowBrush.Blend = shadowBlend;
					g.FillRectangle(shadowBrush, shadowRectangle);
				}
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.DrawPath(new Pen(this.ActiveTabTemplate.BorderColor), outerPath);
			}
			using(GraphicsPath innerPath = new GraphicsPath()) {
				innerPath.AddLines(this.m_activeInnerBounds);
				Color lineColor = Color.FromArgb(120, 255, 255, 255);
				g.DrawPath(new Pen(lineColor), innerPath);
			}
		}
		private void DrawInactiveTabBackground(Graphics g) {
			Rectangle rect = DisplayRectangle;
			rect.Offset(0, 8);
			rect.Height -= 8;
			g.SmoothingMode = SmoothingMode.None;
			using(GraphicsPath outerPath = new GraphicsPath()) {
				outerPath.AddLines(this.m_inactiveBounds);
				using(LinearGradientBrush gradientbrush = new LinearGradientBrush(rect, Color.White, this.InactiveTabTemplate.BackColor, LinearGradientMode.Vertical)) {
					Blend bl = new Blend();
					bl.Factors = new[] {0.3f, 0.4f, 0.5f, 1f, 0.8f, 0.7f};
					bl.Positions = new[] {0f, 0.2f, 0.4f, 0.4f, 0.8f, 1f};
					gradientbrush.Blend = bl;
					g.FillPath(gradientbrush, outerPath);
				}
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.DrawPath(new Pen(this.InactiveTabTemplate.BorderColor), outerPath);
			}
			using(GraphicsPath innerPath = new GraphicsPath()) {
				innerPath.AddLines(this.m_inactiveInnerBounds);
				Color lineColor = Color.FromArgb(120, 255, 255, 255);
				g.DrawPath(new Pen(lineColor), innerPath);
			}
		}
		private void DrawMouseOverTabBackground(Graphics g) {
			Rectangle rect = DisplayRectangle;
			rect.Offset(0, 8);
			rect.Height -= 8;
			g.SmoothingMode = SmoothingMode.None;
			using(GraphicsPath outerPath = new GraphicsPath()) {
				outerPath.AddLines(this.m_mouseOverBounds);
				using(LinearGradientBrush gradientbrush = new LinearGradientBrush(rect, Color.White, this.MouseOverTabTemplate.BackColor, LinearGradientMode.Vertical)) {
					Blend bl = new Blend();
					bl.Factors = new[] {0.3f, 0.4f, 0.5f, 1f, 0.8f, 0.7f};
					bl.Positions = new[] {0f, 0.2f, 0.4f, 0.4f, 0.8f, 1f};
					gradientbrush.Blend = bl;
					g.FillPath(gradientbrush, outerPath);
				}
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.DrawPath(new Pen(this.InactiveTabTemplate.BorderColor), outerPath);
			}
			using(GraphicsPath innerPath = new GraphicsPath()) {
				innerPath.AddLines(this.m_mouseOverInnerBounds);
				Color lineColor = Color.FromArgb(120, 255, 255, 255);
				g.DrawPath(new Pen(lineColor), innerPath);
			}
		}

		#endregion

		#region Hit Testing

		private bool ActiveHitTest(int x, int y) {
			bool hit = false;
			using(GraphicsPath gp = new GraphicsPath()) {
				gp.StartFigure();
				gp.AddLines(this.m_activeBounds);
				gp.CloseFigure();
				using(Pen borderpen = new Pen(Color.Black, 1)) {
					if(gp.IsOutlineVisible(x, y, borderpen) || gp.IsVisible(x, y)) {
						hit = true;
					}
				}
			}
			return hit;
		}
		private bool InactiveHitTest(int x, int y) {
			bool hit = false;
			using(GraphicsPath gp = new GraphicsPath()) {
				gp.StartFigure();
				gp.AddLines(this.m_inactiveBounds);
				gp.CloseFigure();
				using(Pen borderpen = new Pen(Color.Black, 1)) {
					if(gp.IsOutlineVisible(x, y, borderpen) || gp.IsVisible(x, y)) {
						hit = true;
					}
				}
			}
			return hit;
		}
		private bool MouseOverHitTest(int x, int y) {
			bool hit = false;
			using(GraphicsPath gp = new GraphicsPath()) {
				gp.StartFigure();
				gp.AddLines(this.m_mouseOverBounds);
				gp.CloseFigure();
				using(Pen borderpen = new Pen(Color.Black, 1)) {
					if(gp.IsOutlineVisible(x, y, borderpen) || gp.IsVisible(x, y)) {
						hit = true;
					}
				}
			}
			return hit;
		}
		private bool closeButtonHitTest(int x, int y) {
			bool hit = false;
			using(GraphicsPath gp = new GraphicsPath()) {
				gp.StartFigure();
				gp.AddLines(this.m_closeButtonBounds);
				gp.CloseFigure();
				using(Pen borderpen = new Pen(Color.Black, 1)) {
					if(gp.IsOutlineVisible(x, y, borderpen) || gp.IsVisible(x, y)) {
						hit = true;
					}
				}
			}
			return hit;
		}

		#endregion

		#region Mouse Events

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if(this.closeButtonHitTest(e.X, e.Y)) {
				this.IsMouseOverCloseButton = true;
			} else {
				this.IsMouseOverCloseButton = false;
			}
		}
		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			TabSelectedEventArgs ev = null;
			if(this.ActiveHitTest(e.X, e.Y)) {
				ev = new TabSelectedEventArgs(TabType.Active);
			} else if(this.InactiveHitTest(e.X, e.Y)) {
				ev = new TabSelectedEventArgs(TabType.Inactive);
			} else if(this.MouseOverHitTest(e.X, e.Y)) {
				ev = new TabSelectedEventArgs(TabType.MouseOver);
			}
			if(ev != null) {
				if(this.TabSelected != null) {
					this.TabSelected(ev);
				}
			}
		}

		#endregion

		#region Resizing

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			this.GetTabBounds();
			Invalidate();
		}
		private void GetTabBounds() {
			Point startPoint = default(Point);
			if(RightToLeft == RightToLeft.Yes) {
				this.m_activeBounds = new[] {
					new Point(-2, Height), new Point(-2, Height - 5), new Point(55, Height - 5), new Point(58, Height - 6), new Point(58, 11), new Point(60, 8), new Point(198, 8),
					new Point(200, 11), new Point(200, Height - 6), new Point(202, Height - 5), new Point(Width, Height - 5), new Point(Width, Height)
				};
				this.m_activeInnerBounds = new[] {
					new Point(-1, Height), new Point(-1, Height - 4), new Point(56, Height - 4), new Point(59, Height - 6), new Point(59, 12), new Point(61, 9), new Point(197, 9),
					new Point(199, 12), new Point(199, Height - 6), new Point(201, Height - 4), new Point(Width - 1, Height - 4), new Point(Width - 1, Height)
				};
				this.m_inactiveBounds = new[] {new Point(200, Height - 5), new Point(200, 13), new Point(202, 10), new Point(348, 10), new Point(350, 13), new Point(350, Height - 5)};
				this.m_inactiveInnerBounds = new[] {new Point(201, Height - 5), new Point(201, 14), new Point(203, 11), new Point(347, 11), new Point(349, 14), new Point(349, Height - 5)};
				this.m_mouseOverBounds = new[] {new Point(350, Height - 5), new Point(350, 13), new Point(352, 10), new Point(498, 10), new Point(500, 13), new Point(500, Height - 5)};
				this.m_mouseOverInnerBounds = new[] {new Point(351, Height - 5), new Point(351, 14), new Point(353, 11), new Point(497, 11), new Point(499, 14), new Point(499, Height - 5)};
				this.m_closeButtonBounds = new[] {
					new Point(75, 15), new Point(63, 15), new Point(61, 17), new Point(61, 28), new Point(63, 30), new Point(75, 30), new Point(77, 28), new Point(77, 17),
					new Point(75, 15)
				};
				startPoint = new Point(65, 19);
			} else {
				this.m_activeBounds = new[] {
					new Point(-2, Height), new Point(-2, Height - 5), new Point(5, Height - 5), new Point(8, Height - 6), new Point(8, 11), new Point(10, 8), new Point(148, 8),
					new Point(150, 11), new Point(150, Height - 6), new Point(152, Height - 5), new Point(Width, Height - 5), new Point(Width, Height)
				};
				this.m_activeInnerBounds = new[] {
					new Point(-1, Height), new Point(-1, Height - 4), new Point(6, Height - 4), new Point(9, Height - 6), new Point(9, 12), new Point(11, 9), new Point(147, 9),
					new Point(149, 12), new Point(149, Height - 6), new Point(151, Height - 4), new Point(Width - 1, Height - 4), new Point(Width - 1, Height)
				};
				this.m_inactiveBounds = new[] {new Point(150, Height - 5), new Point(150, 13), new Point(152, 10), new Point(298, 10), new Point(300, 13), new Point(300, Height - 5)};
				this.m_inactiveInnerBounds = new[] {new Point(151, Height - 5), new Point(151, 14), new Point(153, 11), new Point(297, 11), new Point(299, 14), new Point(299, Height - 5)};
				this.m_mouseOverBounds = new[] {new Point(300, Height - 5), new Point(300, 13), new Point(302, 10), new Point(448, 10), new Point(450, 13), new Point(450, Height - 5)};
				this.m_mouseOverInnerBounds = new[] {new Point(301, Height - 5), new Point(301, 14), new Point(303, 11), new Point(447, 11), new Point(449, 14), new Point(449, Height - 5)};
				this.m_closeButtonBounds = new[] {
					new Point(132, 15), new Point(144, 15), new Point(146, 17), new Point(146, 28), new Point(144, 30), new Point(132, 30), new Point(130, 28), new Point(130, 17),
					new Point(132, 15)
				};
				startPoint = new Point(134, 19);
			}
			this.m_closeButtonGlyphBounds = new[] {
				new Point(startPoint.X, startPoint.Y), new Point(startPoint.X + 2, startPoint.Y), new Point(startPoint.X + 4, startPoint.Y + 2),
				new Point(startPoint.X + 6, startPoint.Y), new Point(startPoint.X + 8, startPoint.Y), new Point(startPoint.X + 5, startPoint.Y + 3),
				new Point(startPoint.X + 5, startPoint.Y + 4), new Point(startPoint.X + 8, startPoint.Y + 7), new Point(startPoint.X + 6, startPoint.Y + 7),
				new Point(startPoint.X + 4, startPoint.Y + 5), new Point(startPoint.X + 2, startPoint.Y + 7), new Point(startPoint.X, startPoint.Y + 7),
				new Point(startPoint.X + 3, startPoint.Y + 4), new Point(startPoint.X + 3, startPoint.Y + 3), new Point(startPoint.X, startPoint.Y)
			};
		}

		#endregion

		#region Property Changed Handlers

		private void _activeTemplate_PropertyChanged() { Invalidate(); }
		private void _inactiveTemplate_PropertyChanged() { Invalidate(); }
		private void _mouseOverTemplate_PropertyChanged() { Invalidate(); }

		#endregion
	}

	internal class TabSelectedEventArgs : EventArgs {
		private TabType _tabType;
		public TabSelectedEventArgs(TabType tabType) { this._tabType = tabType; }
		public TabType TabType { get { return this._tabType; } }
	}

	internal enum TabType {
		Active = 1,
		Inactive = 2,
		MouseOver = 3
	}
}