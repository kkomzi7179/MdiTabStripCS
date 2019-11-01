#region Using

using System;
using System.Collections;
using System.ComponentModel;

#endregion

namespace MdiTabStripCS {
	//[EditorBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public class MdiTabCollection : CollectionBase {
		public MdiTab this[int index] {
			get {
				return (MdiTab)List[index];
			}
			set {
				List[index] = value;
			}
		}
		public int VisibleCount {
			get {
				int c = 0;
				foreach(MdiTab tab in List) {
					if(tab.Visible) {
						c += 1;
					}
				}
				return c;
			}
		}
		public int FirstVisibleTabIndex {
			get {
				int c = 0;
				foreach(MdiTab tab in List) {
					if(tab.Visible) {
						c = List.IndexOf(tab);
					}
				}
				return c;
			}
		}
		public int LastVisibleTabIndex {
			get {
				int c = 0;
				foreach(MdiTab tab in List) {
					if(tab.Visible) {
						c = List.IndexOf(tab);
					}
				}
				return c;
			}
		}
		public int Add(MdiTab tab) {
			return List.Add(tab);
		}
		public bool Contains(MdiTab tab) {
			return List.Contains(tab);
		}
		public void Insert(int index, MdiTab value) {
			List.Insert(index, value);
		}
		public int IndexOf(MdiTab value) {
			return List.IndexOf(value);
		}
		public void Remove(MdiTab value) {
			List.Remove(value);
		}
		protected override void OnValidate(object value) {
			if(!typeof(MdiTab).IsAssignableFrom(value.GetType())) {
				throw new Exception("Value must be MdiTab");
			}
		}
	}
}