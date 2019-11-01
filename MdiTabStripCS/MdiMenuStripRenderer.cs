#region Using

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

namespace MdiTabStripCS {

internal class MdiMenuStripRenderer : ToolStripRenderer {
	protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
		base.OnRenderToolStripBorder(e);
		ControlPaint.DrawFocusRectangle(e.Graphics, e.AffectedBounds, SystemColors.ControlDarkDark, SystemColors.ControlDarkDark);
	}
	protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e) {
		base.OnRenderToolStripBackground(e);
		ToolStrip strip = e.ToolStrip;
		double h = strip.Height / strip.Items.Count;
		using(Bitmap scratchImage = new Bitmap(strip.Width, strip.Height)) {
			using(Graphics g = Graphics.FromImage(scratchImage)) {
				RectangleF rect = new RectangleF(0, 0, scratchImage.Width, (float) h);
				g.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), scratchImage.Size));
				foreach(MdiMenuItem item in strip.Items) {
					if(item.IsTabVisible) {
						g.FillRectangle(Brushes.White, rect);
					} else {
						g.FillRectangle(new SolidBrush(Color.FromArgb(255, 225, 225, 225)), rect);
					}
					rect.Offset(0, (float) h);
				}
			}
			e.Graphics.DrawImage(scratchImage, e.AffectedBounds);
		}
	}
	protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {
		/*MdiMenuItem mdiItem = (MdiMenuItem)e.Item;
		if(mdiItem.IsMouseOver) {
			e.TextColor = Color.Black;
			if(mdiItem.IsTabActive) {
				e.TextColor = Color.Black;
			} else {
				e.TextColor = Color.White;
			}
		}
		if(mdiItem.IsTabActive) {
			e.TextFont = new Font(e.TextFont, FontStyle.Bold);
		}*/
		e.TextColor = Color.Black;
		base.OnRenderItemText(e);
	}
	/*protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e) {
		base.OnRenderItemBackground(e);
		MdiMenuItem mdiItem = (MdiMenuItem) e.Item;
		if(mdiItem.IsMouseOver) {
			if(mdiItem.IsTabActive) {
				e.Graphics.DrawRectangle(Pens.Black, e.Item.ContentRectangle);
			} else {
				e.Graphics.FillRectangle(Brushes.Black, e.Item.ContentRectangle);
			}
		}
	}*/
	protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {
		base.OnRenderMenuItemBackground(e);
		MdiMenuItem mdiItem = (MdiMenuItem) e.Item;
		/*if(mdiItem.IsMouseOver) {
			if(mdiItem.IsTabActive) {
				e.Graphics.DrawRectangle(Pens.Black, e.Item.ContentRectangle);
			} else {
				e.Graphics.FillRectangle(Brushes.Black, e.Item.ContentRectangle);
			}
		}*/
		/*if(mdiItem.IsTabActive) {
			Blend shadowBlend = new Blend {
				Factors = new[] { 0f, 0.1f, 0.3f, 0.4f },
				Positions = new[] { 0f, 0.5f, 0.8f, 1f }
			};
			using(LinearGradientBrush shadowBrush = new LinearGradientBrush(e.Item.ContentRectangle, Color.White, Color.AliceBlue, LinearGradientMode.Horizontal)) {
				shadowBrush.Blend = shadowBlend;
				e.Graphics.FillRectangle(shadowBrush, e.Item.ContentRectangle);
			}
		}*/
		if(mdiItem.IsMouseOver) {
			e.Graphics.DrawRectangle(mdiItem.IsMouseOver ? Pens.Black : Pens.White, e.Item.ContentRectangle);
		}
	}
	protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e) {
		MdiMenuItem mdiItem = (MdiMenuItem) e.Item;
		if(!mdiItem.IsTabActive) {
			base.OnRenderItemImage(e);
		}
	}
	protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e) {
		MdiMenuItem mdiItem = (MdiMenuItem) e.Item;
		ToolStripItemImageRenderEventArgs tsi = new ToolStripItemImageRenderEventArgs(e.Graphics, e.Item, mdiItem.CheckedImage, e.ImageRectangle);
		base.OnRenderItemImage(tsi);
	}
}


}