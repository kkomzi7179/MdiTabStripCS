using System;
using System.Windows.Forms;

namespace RuntimeProperty
{
	public class PropertyItem
	{
		public object Ctl { get; set; }

		public string? Display { get; set; }

		public PropertyItem(object ctl, string? display)
		{
			this.Ctl = ctl;
			Display = display;
		}
		public int CompareTo(PropertyItem other)
		{
			return Convert.ToInt32(Ctl.Equals(other.Ctl));
		}

		public override string ToString()
		{
			return Display ?? "Null";
		}
	}
	public static class Extensions
	{
		#region Show Property
		public static void ShowProperty(this object obj, params object[] objOthers)
		{
			Form frm = new Form();
			frm.Text = frm.Name = "Runtime Property";
			ComboBox cmb = new ComboBox();
			if (obj is Control)
			{
				((Control)obj).RecursiveControlsAddToCombo(cmb);
			}
			else
			{
				cmb.Items.Add(new PropertyItem(obj, $"[{obj.GetType().Name}] - {obj.ToString()}"));
			}
			if (objOthers != null)
			{
				foreach (object objOther in objOthers)
				{
					if (objOther is Control)
					{
						((Control)objOther).RecursiveControlsAddToCombo(cmb);
					}
					else
					{
						cmb.Items.Add(new PropertyItem(obj, $"[{obj.GetType().Name}] - {obj.ToString()}"));
					}
				}
			}
			frm.Controls.Add(cmb);
			cmb.Dock = DockStyle.Top;

			PropertyGrid pg = new PropertyGrid();
			frm.Controls.Add(pg);
			pg.Dock = DockStyle.Fill;
			pg.BringToFront();

			cmb.SelectedIndexChanged += (sender, args) => { pg.SelectedObject = ((PropertyItem)cmb.SelectedItem).Ctl; };
			if (cmb.Items.Count > 0)
			{
				cmb.SelectedIndex = 0;
			}

			//frm.TopMost = true;
			frm.Show();
			frm.BringToFront();
		}

		private static void RecursiveControlsAddToCombo(this Control ctl, ComboBox cmb)
		{
			#region DevExpress
			/*if(ctl is GridControl)
			{
				GridControl gc = ((GridControl)ctl);
				cmb.Properties.Items.Add(new PropertyItem(gc,
				                                          "GridControl : " + (string.IsNullOrEmpty(gc.Name) ? ("[" + gc.GetType()
				                                                                                                       .Name + "]") : gc.Name)));
				foreach(BaseView view in gc.ViewCollection)
				{
					if(view is AdvBandedGridView)
					{
						cmb.Properties.Items.Add(new PropertyItem(view,
						                                          "AdvBandedGridView : " + (string.IsNullOrEmpty(view.Name) ? ("[" + view.GetType()
						                                                                                                                 .Name + "]") : view.Name)));
						foreach(GridBand band in ((AdvBandedGridView)view).Bands)
						{
							cmb.Properties.Items.Add(new PropertyItem(band, "GridBand : " + (string.IsNullOrEmpty(band.Name) ? ("[" + band.Caption + "]") : band.Name)));
						}
						foreach(BandedGridColumn col in ((AdvBandedGridView)view).Columns)
						{
							cmb.Properties.Items.Add(new PropertyItem(col, "BandedGridColumn : " + (string.IsNullOrEmpty(col.Name) ? ("[" + col.FieldName + "]") : col.Name)));
						}
					} else if(view is BandedGridView)
					{
						cmb.Properties.Items.Add(new PropertyItem(view,
						                                          "BandedGridView : " + (string.IsNullOrEmpty(view.Name) ? ("[" + view.GetType()
						                                                                                                              .Name + "]") : view.Name)));
						foreach(GridBand band in ((BandedGridView)view).Bands)
						{
							cmb.Properties.Items.Add(new PropertyItem(band, "GridBand : " + (string.IsNullOrEmpty(band.Name) ? ("[" + band.Caption + "]") : band.Name)));
						}
						foreach(BandedGridColumn col in ((BandedGridView)view).Columns)
						{
							cmb.Properties.Items.Add(new PropertyItem(col, "BandedGridColumn : " + (string.IsNullOrEmpty(col.Name) ? ("[" + col.FieldName + "]") : col.Name)));
						}
					} else
					{
						cmb.Properties.Items.Add(new PropertyItem(view,
						                                          "GridView : " + (string.IsNullOrEmpty(view.Name) ? ("[" + view.GetType()
						                                                                                                        .Name + "]") : view.Name)));
						foreach(GridColumn col in ((GridView)view).Columns)
						{
							cmb.Properties.Items.Add(new PropertyItem(col, "GridColumn : " + (string.IsNullOrEmpty(col.Name) ? ("[" + col.FieldName + "]") : col.Name)));
						}
					}
				}
				foreach(RepositoryItem item in gc.RepositoryItems)
				{
					if(item is RepositoryItemDateEdit)
					{
						cmb.Properties.Items.Add(new PropertyItem((RepositoryItemDateEdit)item,
																  "RepositoryItemDateEdit : " + (string.IsNullOrEmpty(item.Name) ? ("[" + item.GetType()
																																  .Name + "]") : item.Name)));
					} else if(item is RepositoryItemMemoEdit)
					{
						cmb.Properties.Items.Add(new PropertyItem((RepositoryItemMemoEdit)item,
																  "RepositoryItemMemoEdit : " + (string.IsNullOrEmpty(item.Name) ? ("[" + item.GetType()
																																  .Name + "]") : item.Name)));
					} else
					{
						cmb.Properties.Items.Add(new PropertyItem(item,
																  "RepositoryItem : " + (string.IsNullOrEmpty(item.Name) ? ("[" + item.GetType()
						                                                                                                              .Name + "]") : item.Name)));
					}
				}
			} else if(ctl is TreeList)
			{
				TreeList gc = ((TreeList)ctl);
				cmb.Properties.Items.Add(new PropertyItem(gc,
				                                          "TreeList : " + (string.IsNullOrEmpty(gc.Name) ? ("[" + gc.GetType()
				                                                                                                    .Name + "]") : gc.Name)));
				foreach(TreeListColumn col in gc.Columns)
				{
					cmb.Properties.Items.Add(new PropertyItem(col, "TreeListColumn : " + (string.IsNullOrEmpty(col.Name) ? ("[" + col.FieldName + "]") : col.Name)));
				}
				foreach(RepositoryItem item in gc.RepositoryItems)
				{
					cmb.Properties.Items.Add(new PropertyItem(item,
					                                          "TreeRepository : " + (string.IsNullOrEmpty(item.Name) ? ("[" + item.GetType()
					                                                                                                          .Name + "]") : item.Name)));
				}
			} else if(ctl is RibbonControl)
			{
				RibbonControl gc = ((RibbonControl)ctl);
				cmb.Properties.Items.Add(new PropertyItem(gc,
														  "RibbonControl : " + (string.IsNullOrEmpty(gc.Name) ? ("[" + gc.GetType()
				                                                                                                    .Name + "]") : gc.Name)));
				foreach(RibbonPage rp in gc.Pages)
				{
					cmb.Properties.Items.Add(new PropertyItem(rp, "RibbonPage : " + (string.IsNullOrEmpty(rp.Name) ? ("[" + rp.Text + "]") : rp.Name)));
					foreach(RibbonPageGroup rpg in rp.Groups)
					{
						cmb.Properties.Items.Add(new PropertyItem(rpg, "RibbonPageGroup : " + (string.IsNullOrEmpty(rpg.Name) ? ("[" + rpg.Text + "]") : rpg.Name)));
						
						foreach(BarItemLink barItem in rpg.ItemLinks)
						{
							if(barItem.Item is BarSubItem)
							{
								BarSubItem barSubItem = (BarSubItem)barItem.Item;
								cmb.Properties.Items.Add(new PropertyItem(barSubItem, "BarSubItem : " + (string.IsNullOrEmpty(barSubItem.Name) ? ("[" + barSubItem.Caption + "]") : barSubItem.Name)));
								foreach(LinkPersistInfo linkPersistInfo in barSubItem.LinksPersistInfo)
								{
									BarButtonItem barButtonItem = (BarButtonItem)(linkPersistInfo.Item);
									cmb.Properties.Items.Add(new PropertyItem(barButtonItem, "BarButtonItem : " + (string.IsNullOrEmpty(barButtonItem.Name) ? ("[" + barButtonItem.Caption + "]") : barButtonItem.Name)));
								}
							}
							else if(barItem.Item is BarButtonItem)
							{
								BarButtonItem barButtonItem = (BarButtonItem)barItem.Item;
								cmb.Properties.Items.Add(new PropertyItem(barButtonItem, "BarButtonItem : " + (string.IsNullOrEmpty(barButtonItem.Name) ? ("[" + barButtonItem.Caption + "]") : barButtonItem.Name)));
							}
						}
					}
				}
			} */
			#endregion
			cmb.Items.Add(new PropertyItem(ctl, $"[{ctl.GetType().Name}] - {ctl.Name}"));
			foreach (Control ctlInner in ctl.Controls)
			{
				ctlInner.RecursiveControlsAddToCombo(cmb);
			}
		}

		#endregion // Show Property
	}
}