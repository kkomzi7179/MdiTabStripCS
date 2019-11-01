#region Using

using System.ComponentModel;
using System.Drawing;

#endregion

namespace MdiTabStripCS.Designer {
	public class ActiveMdiTabProperties : InactiveMdiTabProperties {
		private Color _closeButtonBackColor;
		private Color _closeButtonBorderColor;
		private Color _closeButtonForeColor;
		private Color _closeButtonHotForeColor;
		[Category("Close Button Appearance"), Description("The background color of the tab's close button when moused over.")]
		public Color CloseButtonBackColor {
			get { return this._closeButtonBackColor; }
			set {
				if(this._closeButtonBackColor != value) {
					this._closeButtonBackColor = value;
					InvokePropertyChanged();
				}
			}
		}
		[Category("Close Button Appearance"), Description("The border color of the tab's close button when moused over.")]
		public Color CloseButtonBorderColor {
			get { return this._closeButtonBorderColor; }
			set {
				if(this._closeButtonBorderColor != value) {
					this._closeButtonBorderColor = value;
					InvokePropertyChanged();
				}
			}
		}
		[Category("Close Button Appearance"), Description("The glyph color of the tab's close button.")]
		public Color CloseButtonForeColor {
			get { return this._closeButtonForeColor; }
			set {
				if(this._closeButtonForeColor != value) {
					this._closeButtonForeColor = value;
					InvokePropertyChanged();
				}
			}
		}
		[Category("Close Button Appearance"), Description("The glyph color of the tab's close button when moused over.")]
		public Color CloseButtonHotForeColor {
			get { return this._closeButtonHotForeColor; }
			set {
				if(this._closeButtonHotForeColor != value) {
					this._closeButtonHotForeColor = value;
					InvokePropertyChanged();
				}
			}
		}
	}
}