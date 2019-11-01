#region Using

using System;
using System.Windows.Forms;

#endregion

namespace MdiTabStripCS {
	public class MdiTabStripTabClickedEventArgs : EventArgs {
		public MdiTab ClickedTab { get; private set; }
		public MouseEventArgs MouseEvent { get; private set; }
		public MdiTabStripTabClickedEventArgs(MdiTab tabItem, MouseEventArgs e) {
			ClickedTab = tabItem;
			MouseEvent = e;
		}
	}
}