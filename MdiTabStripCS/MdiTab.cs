#region Using

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#endregion

namespace MdiTabStripCS {
	/// <summary>
	/// Represents a selectable tab that corresponds to exactly one open <see cref="Form"/> 
	/// whose <see cref="Form.MdiParent"/> property has been 
	/// set to an instance of another form in an MDI application.
	/// </summary>
	[ToolboxItem(false)]
	public class MdiTab : MdiTabStripItemBase {
		#region "Fields"

		private MdiTabStrip m_owner;
		private Form m_form;
		private bool m_isMouseOver;
		private bool m_isMouseOverCloseButton;
		private bool m_isSwitching;
		private Rectangle m_dragBox = Rectangle.Empty;
		private Point[] m_activeBounds;
		private Point[] m_activeInnerBounds;
		private Point[] m_inactiveBounds;
		private Point[] m_inactiveInnerBounds;
		private Point[] m_closeButtonBounds;
		private Point[] m_closeButtonGlyphBounds;
		private Cursor m_dragCursor;
		private bool m_isAnimating;
		private AnimationType m_animationType;

		#endregion

		private int m_currentFrame;

		#region "Constructor/Destructor"

		/// <summary>
		/// Initializes a new instance of the <see cref="MdiTab"/> class.
		/// </summary>
		public MdiTab(MdiTabStrip owner) {
			this.ParentInternal = owner;
		}
		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="MdiTab"/> and optionally releases the managed resources. 
		/// </summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(this.m_dragCursor != null) {
					this.m_dragCursor.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region "Properties"

		/// <summary>
		/// Gets or sets the instance of a <see cref="Form"/> the <see cref="MdiTab"/> represents.
		/// </summary>
		/// <returns>The <see cref="Form"/> object the tab represents.</returns>
		public Form Form {
			get {
				return this.m_form;
			}
			set {
				this.m_form = value;
			}
		}
		internal MdiTabStrip ParentInternal {
			get {
				return this.m_owner;
			}
			set {
				this.m_owner = value;
			}
		}
		internal bool IsMouseOver {
			get {
				return this.m_isMouseOver;
			}
			set {
				this.m_isMouseOver = value;
			}
		}
		internal bool IsActive {
			get {
				return ReferenceEquals(this.ParentInternal.ActiveTab, this);
			}
		}
		private bool IsAnimating {
			get {
				return this.m_isAnimating;
			}
			set {
				this.m_isAnimating = value;
				if(value) {
					this.ParentInternal.AddAnimatingTab(this);
				} else {
					this.ParentInternal.RemoveAnimatingTab(this);
					this.m_animationType = AnimationType.None;
				}
			}
		}
		internal int CurrentFrame {
			get {
				return this.m_currentFrame;
			}
			set {
				this.m_currentFrame = value;
			}
		}
		internal AnimationType AnimationType {
			get {
				return this.m_animationType;
			}
		}
		/// <summary>
		/// Gets the rectangle that represents the display area of the control.
		/// </summary>
		/// <returns>A <see cref="Rectangle"/> that represents the display area of the control.</returns>
		public override Rectangle DisplayRectangle {
			get {
				Rectangle rect = base.DisplayRectangle;
				rect.Offset(Location.X, Location.Y);
				return rect;
			}
		}
		internal bool IsMouseOverCloseButton {
			get {
				return this.m_isMouseOverCloseButton;
			}
			set {
				if(this.m_isMouseOverCloseButton != value) {
					string txt = this.Form.Text;
					this.m_isMouseOverCloseButton = value;
					if(value) {
						txt = "이 탭 닫기";
					}
					if(this.ParentInternal.ShowTabToolTip) {
						this.ParentInternal.UpdateToolTip(txt);
					}
					this.ParentInternal.Invalidate();
				}
			}
		}
		internal bool CanDrag {
			get {
				if(this.ParentInternal.Tabs.Count == 1) {
					return false;
				}
				return !(this.ParentInternal.TabPermanence == MdiTabPermanence.First & (this.ParentInternal.Tabs.IndexOf(this) == 0));
			}
		}
		internal bool CanClose {
			get {
				if(this.ParentInternal.TabPermanence == MdiTabPermanence.First && (this.ParentInternal.Tabs.IndexOf(this) == 0)) {
					return false;
				} else if(this.ParentInternal.TabPermanence == MdiTabPermanence.LastOpen && this.ParentInternal.Tabs.Count == 1) {
					return false;
				}
				return true;
			}
		}
		private bool CanAnimate {
			get {
				return this.ParentInternal.Animate;
			}
		}
		private Color TabBackColor {
			get {
				Color tabcolor = this.ParentInternal.InactiveTabColor;
				if(this.IsActive) {
					tabcolor = this.ParentInternal.ActiveTabColor;
				} else if(!Enabled) {
					tabcolor = this.ParentInternal.InactiveTabColor;
				} else if(this.IsAnimating) {
					tabcolor = this.ParentInternal.BackColorFadeSteps[this.m_currentFrame];
				} else if(this.IsMouseOver) {
					tabcolor = this.ParentInternal.MouseOverTabColor;
				}
				return tabcolor;
			}
		}
		protected Color TabForeColor {
			get {
				Color foreColor = this.ParentInternal.InactiveTabForeColor;
				if(this.IsActive) {
					foreColor = this.ParentInternal.ActiveTabForeColor;
				} else if(this.IsAnimating) {
					foreColor = this.ParentInternal.ForeColorFadeSteps[this.m_currentFrame];
				} else if(this.IsMouseOver) {
					foreColor = this.ParentInternal.MouseOverTabForeColor;
				}
				return foreColor;
			}
		}
		private Font TabFont {
			get {
				//We default to the font for the inactive tab because it is more ofter used. If animating we switch
				//to the font for the moused over tab when the current frame is in the latter half of the animation.
				Font font = this.ParentInternal.InactiveTabFont;
				if(this.IsActive) {
					font = this.ParentInternal.ActiveTabFont;
				} else if(this.IsAnimating) {
					if(this.CurrentFrame > (this.ParentInternal.Duration / 2)) {
						font = this.ParentInternal.MouseOverTabFont;
					}
				} else if(this.IsMouseOver) {
					font = this.ParentInternal.MouseOverTabFont;
				}
				return font;
			}
		}

		#endregion

		#region "Methods"

		#region "Hit Testing"

		internal bool HitTest(int x, int y) {
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

		#region "Paint Background"

		internal void DrawControlBackground(Graphics g) {
			if(this.IsActive) {
				this.DrawActiveTabBackground(g);
			} else {
				this.DrawInactiveTabBackground(g);
			}
		}
		private void DrawActiveTabBackground(Graphics g) {
			//The shadowRectangle fills the divider that is a part of the active tab and spans the width
			//of the parent MdiTabStrip.
			Rectangle shadowRectangle = new Rectangle(this.ParentInternal.ClientRectangle.X, this.DisplayRectangle.Bottom, this.ParentInternal.ClientRectangle.Width, this.ParentInternal.Padding.Bottom);
			Blend shadowBlend = new Blend();
			g.SmoothingMode = SmoothingMode.None;
			shadowBlend.Factors = new[] { 0f, 0.1f, 0.3f, 0.4f };
			shadowBlend.Positions = new[] { 0f, 0.5f, 0.8f, 1f };
			using(GraphicsPath outerPath = new GraphicsPath()) {
				outerPath.AddLines(this.m_activeBounds);
				using(LinearGradientBrush gradientBrush = this.GetGradientBackBrush()) {
					g.FillPath(gradientBrush, outerPath);
				}
				using(LinearGradientBrush shadowBrush = new LinearGradientBrush(shadowRectangle, this.TabBackColor, Color.Black, LinearGradientMode.Vertical)) {
					shadowBrush.Blend = shadowBlend;
					g.FillRectangle(shadowBrush, shadowRectangle);
				}
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.DrawPath(new Pen(this.ParentInternal.ActiveTabBorderColor), outerPath);
			}
			//Draw the inner border
			using(GraphicsPath innerPath = new GraphicsPath()) {
				innerPath.AddLines(this.m_activeInnerBounds);
				Color lineColor = Color.FromArgb(120, 255, 255, 255);
				g.DrawPath(new Pen(lineColor), innerPath);
			}
			if(this.CanClose) {
				this.DrawCloseButton(g);
			}
		}
		private void DrawInactiveTabBackground(Graphics g) {
			g.SmoothingMode = SmoothingMode.None;
			using(GraphicsPath outerPath = new GraphicsPath()) {
				outerPath.AddLines(this.m_inactiveBounds);
				using(LinearGradientBrush gradientBrush = this.GetGradientBackBrush()) {
					g.FillPath(gradientBrush, outerPath);
				}
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.DrawPath(new Pen(this.ParentInternal.InactiveTabBorderColor), outerPath);
			}
			//Draw the inner border
			using(GraphicsPath innerPath = new GraphicsPath()) {
				innerPath.AddLines(this.m_inactiveInnerBounds);
				Color lineColor = Color.FromArgb(120, 255, 255, 255);
				g.DrawPath(new Pen(lineColor), innerPath);
			}
		}
		protected LinearGradientBrush GetGradientBackBrush() {
			LinearGradientBrush b = new LinearGradientBrush(this.DisplayRectangle, Color.White, this.TabBackColor, LinearGradientMode.Vertical);
			Blend bl = new Blend();
			if(this.IsActive) {
				bl.Factors = new[] { 0.3f, 0.4f, 0.5f, 1f, 1f };
				bl.Positions = new[] { 0f, 0.2f, 0.35f, 0.35f, 1f };
			} else {
				bl.Factors = new[] { 0.3f, 0.4f, 0.5f, 1f, 0.8f, 0.7f };
				bl.Positions = new[] { 0f, 0.2f, 0.4f, 0.4f, 0.8f, 1f };
			}
			b.Blend = bl;
			return b;
		}

		#endregion

		#region "Paint"

		internal virtual void DrawControl(Graphics g) {
			if(this.IsActive) {
				this.DrawActiveTab(g);
			} else {
				this.DrawInactiveTab(g);
			}
		}
		private void DrawActiveTab(Graphics g) {
			//The proposedSize variable determines the size available to draw the text of the tab.
			Size proposedSize = new Size(Width - 5, Height);
			if(this.CanClose) {
				//If the tab can close then subtract the button's width
				proposedSize.Width -= 22;
				this.DrawCloseButton(g);
			}
			if(this.ParentInternal.DisplayFormIcon) {
				//If the tab will display an icon the subtract the icon's width
				proposedSize.Width -= 22;
				this.DrawFormIcon(g);
			}
			this.DrawTabText(g, proposedSize);
		}
		private void DrawFormIcon(Graphics g) {
			Rectangle iconRectangle = default(Rectangle);
			if(this.ParentInternal.RightToLeft == RightToLeft.Yes) {
				iconRectangle = new Rectangle(Right - 20, Top + 5, 17, 17);
			} else {
				iconRectangle = new Rectangle(Left + 5, Top + 5, 17, 17);
			}
			if(!this.IsActive) {
				iconRectangle.Offset(0, 2);
			}
			// 2019-11-01 TODO By Jaeyong Park : 활성화된 폼은 활성화 폼 아이콘으로 표시 - 원복
//			if(IsActive) {
//				Icon _Icon = global::MdiTabStripCS.Properties.Resources.CurrentTab;
//				using(Bitmap bmp = new Bitmap(_Icon.Width
//					, _Icon.Height
//					, PixelFormat.Format32bppArgb)) {
//					using(Graphics bg = Graphics.FromImage(bmp)) {
//						bg.DrawIcon(_Icon, 0, 0);
//					}
//					g.DrawImage(bmp, iconRectangle);
//				}
//			} else {
				using(Bitmap bmp = new Bitmap(this.Form.Icon.Width, this.Form.Icon.Height, PixelFormat.Format32bppArgb)) {
					using(Graphics bg = Graphics.FromImage(bmp)) {
						bg.DrawIcon(this.Form.Icon, 0, 0);
					}
					g.DrawImage(bmp, iconRectangle);
				}
//			}
		}
		private void DrawTabText(Graphics g, Size proposedSize) {
			Size s = default(Size);
			Rectangle textRectangle = default(Rectangle);
			TextFormatFlags textFlags = TextFormatFlags.WordEllipsis | TextFormatFlags.EndEllipsis;
			bool isRightToLeft = this.ParentInternal.RightToLeft == RightToLeft.Yes;
			if(isRightToLeft) {
				textFlags = textFlags | TextFormatFlags.Right;
			}
			s = TextRenderer.MeasureText(g, this.Form.Text, this.TabFont, proposedSize, textFlags);
			textRectangle = new Rectangle(Left + 5, Top + 8, proposedSize.Width, s.Height);
			if(isRightToLeft) {
				if(this.IsActive && this.CanClose) {
					textRectangle.Offset(22, 0);
				}
			} else {
				if(this.ParentInternal.DisplayFormIcon) {
					textRectangle.Offset(17, 0);
				}
			}
			if(!this.IsActive) {
				textRectangle.Offset(0, 2);
			}
			TextRenderer.DrawText(g, this.Form.Text, this.TabFont, textRectangle, this.TabForeColor, textFlags);
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
				using(SolidBrush backBrush = new SolidBrush(this.ParentInternal.CloseButtonBackColor)) {
					g.FillPath(backBrush, gp);
				}
				using(Pen borderPen = new Pen(this.ParentInternal.CloseButtonBorderColor)) {
					g.DrawPath(borderPen, gp);
				}
			}
			this.DrawCloseButtonGlyph(g, this.ParentInternal.CloseButtonHotForeColor);
		}
		private void DrawInactiveCloseButton(Graphics g) {
			this.DrawCloseButtonGlyph(g, this.ParentInternal.CloseButtonForeColor);
		}
		private void DrawCloseButtonGlyph(Graphics g, Color glyphColor) {
			g.SmoothingMode = SmoothingMode.None;
			using(GraphicsPath shadow = new GraphicsPath()) {
				Matrix translateMatrix = new Matrix();
				Color shadowColor = Color.FromArgb(30, 0, 0, 0);
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
		private void DrawInactiveTab(Graphics g) {
			//The proposedSize variable determines the size available to draw the text of the tab.
			Size proposedSize = new Size(Width - 5, Height);
			if(this.ParentInternal.DisplayFormIcon) {
				//If the tab will display an icon the subtract the icon's width
				proposedSize.Width -= 22;
				this.DrawFormIcon(g);
			}
			this.DrawTabText(g, proposedSize);
		}

		#endregion

		#region "Fade Animation"

		internal void StartAnimation(AnimationType animation) {
			//When the cursor is moved over the control very quick it causes some odd behavior with the animation
			//These two checks are done to make sure that the tab isn't needlessly added to the animation arraylist.
			if(animation == AnimationType.FadeIn && this.CurrentFrame == this.ParentInternal.Duration - 1) {
				return;
			}
			if(animation == AnimationType.FadeOut && this.CurrentFrame == 0) {
				return;
			}
			this.m_animationType = animation;
			if(((this.ParentInternal != null))) {
				this.IsAnimating = true;
			}
		}
		internal void OnAnimationTick(int newFrame) {
			this.m_currentFrame = newFrame;
			this.ParentInternal.Invalidate(this.DisplayRectangle, false);
		}
		internal void StopAnimation() {
			this.IsAnimating = false;
			this.ParentInternal.Invalidate(this.DisplayRectangle, false);
		}
		[DllImport("User32.dll", EntryPoint = "LoadCursorFromFileW", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern IntPtr LoadCursorFromFile(string filename);

		#endregion

		#region "CustomCursor"

		private Cursor GetCustomCursor(string fileName) {
			IntPtr hCursor = default(IntPtr);
			Cursor result = null;
			try {
				hCursor = LoadCursorFromFile(fileName);
				if(!IntPtr.Zero.Equals(hCursor)) {
					result = new Cursor(hCursor);
				}
			} catch(Exception ex) {
				//Catch but don't process the exception. If this method returns nothing then
				//the default Windows drag cursor will be used.
				return null;
			}
			return result;
		}

		#endregion

		#endregion

		#region "Events"

		#region "Layout"

		protected override void OnLayout(LayoutEventArgs levent) {
			this.m_activeBounds = new[] {
			new Point(-2, this.ParentInternal.Bottom), new Point(-2, Bottom), new Point(Left - 3, Bottom), new Point(Left, Bottom - 1), new Point(Left, Top + 3), new Point(Left + 2, Top),
			new Point(Right - 2, Top), new Point(Right, Top + 3), new Point(Right, Bottom - 1), new Point(Right + 2, Bottom), new Point(this.ParentInternal.Width, Bottom),
			new Point(this.ParentInternal.Width, this.ParentInternal.Bottom)
		};
			this.m_activeInnerBounds = new[] {
			new Point(-1, this.ParentInternal.Bottom), new Point(-1, Bottom + 1), new Point(Left - 4, Bottom + 1), new Point(Left + 1, Bottom), new Point(Left + 1, Top + 4),
			new Point(Left + 3, Top + 1), new Point(Right - 3, Top + 1), new Point(Right - 1, Top + 4), new Point(Right - 1, Bottom), new Point(Right + 3, Bottom + 1),
			new Point(this.ParentInternal.Width - 1, Bottom + 1), new Point(this.ParentInternal.Width - 1, this.ParentInternal.Bottom)
		};
			this.m_inactiveBounds = new[] { new Point(Left, Bottom), new Point(Left, Top + 5), new Point(Left + 2, Top + 2), new Point(Right - 2, Top + 2), new Point(Right, Top + 5), new Point(Right, Bottom) };
			this.m_inactiveInnerBounds = new[] {
			new Point(Left + 1, Bottom), new Point(Left + 1, Top + 6), new Point(Left + 3, Top + 3), new Point(Right - 3, Top + 3), new Point(Right - 1, Top + 6),
			new Point(Right - 1, Bottom)
		};
			if(this.ParentInternal.RightToLeft == RightToLeft.Yes) {
				this.m_closeButtonBounds = new[] {
				new Point(Left + 18, Top + 7), new Point(Left + 6, Top + 7), new Point(Left + 4, Top + 9), new Point(Left + 4, Top + 20), new Point(Left + 6, Top + 22),
				new Point(Left + 18, Top + 22), new Point(Left + 20, Top + 20), new Point(Left + 20, Top + 9), new Point(Left + 18, Top + 7)
			};
				Point startPoint = new Point(Left + 8, Top + 11);
				this.m_closeButtonGlyphBounds = new[] {
				new Point(startPoint.X, startPoint.Y), new Point(startPoint.X + 2, startPoint.Y), new Point(startPoint.X + 4, startPoint.Y + 2),
				new Point(startPoint.X + 6, startPoint.Y), new Point(startPoint.X + 8, startPoint.Y), new Point(startPoint.X + 5, startPoint.Y + 3),
				new Point(startPoint.X + 5, startPoint.Y + 4), new Point(startPoint.X + 8, startPoint.Y + 7), new Point(startPoint.X + 6, startPoint.Y + 7),
				new Point(startPoint.X + 4, startPoint.Y + 5), new Point(startPoint.X + 2, startPoint.Y + 7), new Point(startPoint.X, startPoint.Y + 7),
				new Point(startPoint.X + 3, startPoint.Y + 4), new Point(startPoint.X + 3, startPoint.Y + 3), new Point(startPoint.X, startPoint.Y)
			};
			} else {
				this.m_closeButtonBounds = new[] {
				new Point(Right - 18, Top + 7), new Point(Right - 6, Top + 7), new Point(Right - 4, Top + 9), new Point(Right - 4, Top + 20), new Point(Right - 6, Top + 22),
				new Point(Right - 18, Top + 22), new Point(Right - 20, Top + 20), new Point(Right - 20, Top + 9), new Point(Right - 18, Top + 7)
			};
				Point startPoint = new Point(Right - 16, Top + 11);
				this.m_closeButtonGlyphBounds = new[] {
				new Point(startPoint.X, startPoint.Y), new Point(startPoint.X + 2, startPoint.Y), new Point(startPoint.X + 4, startPoint.Y + 2),
				new Point(startPoint.X + 6, startPoint.Y), new Point(startPoint.X + 8, startPoint.Y), new Point(startPoint.X + 5, startPoint.Y + 3),
				new Point(startPoint.X + 5, startPoint.Y + 4), new Point(startPoint.X + 8, startPoint.Y + 7), new Point(startPoint.X + 6, startPoint.Y + 7),
				new Point(startPoint.X + 4, startPoint.Y + 5), new Point(startPoint.X + 2, startPoint.Y + 7), new Point(startPoint.X, startPoint.Y + 7),
				new Point(startPoint.X + 3, startPoint.Y + 4), new Point(startPoint.X + 3, startPoint.Y + 3), new Point(startPoint.X, startPoint.Y)
			};
			}
		}

		#endregion

		#region "DragDrop Events"

		protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent) {
			gfbevent.UseDefaultCursors = this.m_dragCursor == null;
			if((gfbevent.Effect & DragDropEffects.Move) == DragDropEffects.Move) {
				Cursor.Current = this.m_dragCursor;
			} else {
				Cursor.Current = Cursors.No;
			}
		}

		#endregion

		#region "Mouse Events"

		protected override void OnMouseEnter(EventArgs e) {
			this.IsMouseOver = true;
			if(this.CanAnimate && !this.IsActive) {
				this.StartAnimation(AnimationType.FadeIn);
			}
			if(this.Form != null) {
				this.ParentInternal.UpdateToolTip(this.Form.Text);
			}
		}
		protected override void OnMouseLeave(EventArgs e) {
			//Reset the mouse over fields to False
			this.IsMouseOver = false;
			this.IsMouseOverCloseButton = false;
			if(this.CanAnimate) {
				if(!this.IsActive) {
					//If not the currently active tab
					if(this.IsAnimating) {
						//If the tab is currently animating then change it's animation type to properly fade
						//back to the inactive color.
						this.m_animationType = AnimationType.FadeOut;
					} else {
						//The cursor was still over the tab and animation had finished so we need to fade
						//from the mouseover color to the inactive color
						this.StartAnimation(AnimationType.FadeOut);
					}
				} else {
					//If it is the active tab then reset the current frame to 0 because the tab
					//might have been selected while animation was in process
					this.m_currentFrame = 0;
				}
			} else {
				//If the tab cannot animate then invalidate the tab to repaint with the inactive color
				this.ParentInternal.Invalidate(this.DisplayRectangle, false);
			}
		}
		protected override void OnMouseDown(MouseEventArgs e) {
			Size dragsize = SystemInformation.DragSize;
			this.m_owner.OnMdiTabClicked(new MdiTabStripTabClickedEventArgs(this, e));
			if(this.CanDrag) {
				//If the tab can be dragged, which is determined by the TabPermenance property, then set the
				//drag box and load the custom cursor.
				this.m_dragBox = new Rectangle(new Point(e.X - (dragsize.Width / 2), e.Y - (dragsize.Height / 2)), dragsize);
				if(this.m_dragCursor == null) {
					string filePath = Path.Combine(Application.StartupPath, "MyDragTab.cur");
					this.m_dragCursor = this.GetCustomCursor(filePath);
				}
			}
			if(!this.IsActive) {
				//Set the isSwitching field. This prevents the tab from being closed in the MouseUp event
				//if the cursor is over the area in which the close button will be displayed.
				this.m_isSwitching = true;
				this.Form.Activate();
			}
		}
		protected override void OnMouseUp(MouseEventArgs e) {
			this.m_dragBox = Rectangle.Empty;
			//If the tab is closable and the user is not switching tabs and the mouse was clicked over the
			//close button then close the form. The tab is removed via the FormClose event handler in MdiTabStrip class.
			if(this.CanClose && !this.m_isSwitching && this.closeButtonHitTest(e.X, e.Y)) {
				this.Form.Close();
			}
			this.m_isSwitching = false;
		}
		protected override void OnMouseMove(MouseEventArgs e) {
			if((e.Button & MouseButtons.Left) == MouseButtons.Left) {
				if(this.CanDrag) {
					//If the tab can be dragged, which is determined by the TabPermenace property, then.
					//				if(Rectangle.op_Inequality(this.m_dragBox, Rectangle.Empty) & !this.m_dragBox.Contains(e.X, e.Y)) {
					if((this.m_dragBox != Rectangle.Empty) && (!this.m_dragBox.Contains(e.X, e.Y))) {
						//If the cursor has been moved out of the bounds of the drag box while the left
						//mouse button is down then initiate dragging by calling the DoDragDrop method.
						this.m_isSwitching = false;
						DragDropEffects dropEffects = DoDragDrop(this, DragDropEffects.Move);
						this.m_dragBox = Rectangle.Empty;
					}
				}
			} else {
				//if active then test if mouse cursor is over the close button to display the tool tip.
				if(this.IsActive) {
					this.IsMouseOverCloseButton = this.closeButtonHitTest(e.X, e.Y);
				}
			}
		}

		#endregion

		#endregion
	}
}