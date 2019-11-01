#region Using

using System.ComponentModel;
using System.Drawing;

#endregion

namespace MdiTabStripCS.Designer {
	public class InactiveMdiTabProperties : MdiTabProperties {
		private Color _borderColor;
		[Category("Tab Appearance"), Description("The border color of the tab.")]
		public Color BorderColor {
			get { return this._borderColor; }
			set {
				if(this._borderColor != value) {
					this._borderColor = value;
					InvokePropertyChanged();
				}
			}
		}
	}
}