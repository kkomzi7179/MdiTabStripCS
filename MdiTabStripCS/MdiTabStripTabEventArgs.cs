#region Using

using System;

#endregion

namespace MdiTabStripCS {
	public class MdiTabStripTabEventArgs : EventArgs {
		private MdiTab _tab;
		public MdiTabStripTabEventArgs(MdiTab tabItem) {
			this._tab = tabItem;
		}
		public MdiTab MdiTab {
			get {
				return this._tab;
			}
		}
	}
}