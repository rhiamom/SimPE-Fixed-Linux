/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/

using System;
using System.Collections;
using System.ComponentModel;
using Avalonia.Controls;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for NgbhSlotSelection.
	/// </summary>
	[System.ComponentModel.DefaultEvent("SelectedSlotChanged")]
	public class NgbhSlotSelection : Avalonia.Controls.UserControl
	{
		private SimPe.Plugin.NgbhSlotListView lv;
		private Ambertation.Windows.Forms.EnumComboBox cb;

		public NgbhSlotSelection()
		{
			InitializeComponent();

			cb.Enum = typeof(Data.NeighborhoodSlots);
			cb.ResourceManager = SimPe.Localization.Manager;
			cb.SelectedIndex = 0;
		}

		private void InitializeComponent()
		{
			lv = new SimPe.Plugin.NgbhSlotListView();
			lv.NgbhResource = null;
			lv.Slot = null;
			lv.Slots = null;
			lv.SlotType = SimPe.Data.NeighborhoodSlots.LotsIntern;
			lv.SelectedSlotChanged += new System.EventHandler(this.lv_SelectedSlotChanged);

			cb = new Ambertation.Windows.Forms.EnumComboBox();
			cb.Enum = null;
			cb.Name = "cb";
			cb.ResourceManager = null;
			cb.SelectionChanged += (s, e) => cb_SelectedIndexChanged(s, EventArgs.Empty);

			var panel = new Avalonia.Controls.DockPanel();
			DockPanel.SetDock(cb, Avalonia.Controls.Dock.Top);
			panel.Children.Add(cb);
			panel.Children.Add(lv);
			Content = panel;
		}

		private void cb_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cb.SelectedIndex>=0)
			{
				lv.SlotType = SlotType;
				SetContent();
			}
		}

		Ngbh ngbh;
		[System.ComponentModel.Browsable(false)]
		public Ngbh NgbhResource
		{
			get {return ngbh;}
			set
			{
				ngbh = value;
				lv.NgbhResource = ngbh;
			}
		}

		void SetContent()
		{
			lv.SlotType = SlotType;
		}

		private void lv_SelectedSlotChanged(object sender, System.EventArgs e)
		{
			if (SelectedSlotChanged!=null) SelectedSlotChanged(this, e);
		}



		public NgbhSlot SelectedSlot
		{
			get
			{
				return lv.SelectedSlot;
			}
		}

		public Data.NeighborhoodSlots SlotType
		{
			get {
				if (cb.SelectedIndex<0) return Data.NeighborhoodSlots.Lots;
				return (Data.NeighborhoodSlots)cb.SelectedValue;
			}
		}

		public event EventHandler SelectedSlotChanged;


	}
}
