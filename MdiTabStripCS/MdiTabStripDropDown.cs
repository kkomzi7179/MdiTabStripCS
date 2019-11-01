#region Using

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace MdiTabStripCS {

	#region 열린 창 목록

	[ToolboxItem(false)]
	internal class MdiTabStripDropDown : ContextMenuStrip {
		internal MdiTabStripDropDown() {
			Renderer = new MdiMenuStripRenderer();
		}
		internal void SetItemChecked(MdiMenuItem item) {
			foreach(MdiMenuItem mi in Items) {
				mi.Checked = false;
			}
			item.Checked = true;
		}
		protected override void OnItemAdded(ToolStripItemEventArgs e) {
			base.OnItemAdded(e);
			this.SetItemChecked((MdiMenuItem)e.Item);
		}
	}

	internal class MdiMenuItem : ToolStripMenuItem {
		private bool m_isMouseOver;
		private MdiTab m_ownerTab;
		public MdiMenuItem(MdiTab tab, EventHandler handler) {
			this.m_ownerTab = tab;
			Click += handler;
		}
		public Form Form {
			get {
				return this.m_ownerTab.Form;
			}
		}
		public bool IsMouseOver {
			get {
				return this.m_isMouseOver;
			}
		}
		public bool IsTabActive {
			get {
				return this.m_ownerTab.IsActive;
			}
		}
		public bool IsTabVisible {
			get {
				return this.m_ownerTab.Visible;
			}
		}
		internal Image CheckedImage {
			get {
				Bitmap bmp = new Bitmap(typeof(MdiTabStrip), "CurrentTab.png");
				bmp.MakeTransparent(bmp.GetPixel(1, 1));
				return bmp;
			}
		}
		//Public Overrides Property Text() As String
		//    Get
		//        Return Me.Form.Text
		//    End Get
		//    Set(ByVal value As String)
		//        MyBase.Text = value
		//    End Set
		//End Property
		protected override void OnMouseEnter(EventArgs e) {
			base.OnMouseEnter(e);
			this.m_isMouseOver = true;
			Invalidate();
		}
		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);
			this.m_isMouseOver = false;
			Invalidate();
		}
	}

	#endregion

}