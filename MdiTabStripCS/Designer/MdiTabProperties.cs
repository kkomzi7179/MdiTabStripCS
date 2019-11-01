#region Using

using System.ComponentModel;
using System.Drawing;

#endregion

namespace MdiTabStripCS.Designer {
	public class MdiTabProperties {
		private Color _backColor;
		private Color _foreColor;
		private Font _font;
		internal event PropertyChangedEventHandler PropertyChanged;

		internal delegate void PropertyChangedEventHandler();

		[Category("Tab Appearance"), Description("The background color of the tab.")]
		public Color BackColor {
			get { return this._backColor; }
			set {
				if(this._backColor != value) {
					this._backColor = value;
					if(this.PropertyChanged != null) {
						this.PropertyChanged();
					}
				}
			}
		}
		[Category("Tab Appearance"), Description("The text color of the tab.")]
		public Color ForeColor {
			get { return this._foreColor; }
			set {
				if(this._foreColor != value) {
					this._foreColor = value;
					if(this.PropertyChanged != null) {
						this.PropertyChanged();
					}
				}
			}
		}
		[Category("Tab Appearance"), Description("The font used to display the text of the tab.")]
		public Font Font {
			get { return this._font; }
			set {
				if((!ReferenceEquals(this._font, value))) {
					this._font = value;
					if(this.PropertyChanged != null) {
						this.PropertyChanged();
					}
				}
			}
		}
		protected void InvokePropertyChanged() {
			if(this.PropertyChanged != null) {
				this.PropertyChanged();
			}
		}
	}
}