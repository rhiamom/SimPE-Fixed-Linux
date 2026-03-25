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
	/// Summary description for NgbhSlotListView.
	/// </summary>
	[System.ComponentModel.DefaultEvent("SelectedSlotChanged")]
	public class NgbhSlotListView : Avalonia.Controls.UserControl
	{
		private Avalonia.Controls.ListBox lv;

		public NgbhSlotListView()
		{
			InitializeComponent();
			SetContent();
		}

		private void InitializeComponent()
		{
			lv = new Avalonia.Controls.ListBox();
			lv.SelectionChanged += (s, e) => lv_SelectedIndexChanged(s, EventArgs.Empty);
			Content = lv;
		}

		NgbhSlot slot;
		[System.ComponentModel.Browsable(false)]
		public NgbhSlot Slot
		{
			get {return slot;}
			set
			{
				slot = value;
				SetContent();
			}
		}

		Collections.NgbhSlots slots;
		[System.ComponentModel.Browsable(false)]
		public Collections.NgbhSlots Slots
		{
			get {return slots;}
			set
			{
				slots = value;
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
				SetContent();
			}
		}

		Data.NeighborhoodSlots st;
		public Data.NeighborhoodSlots SlotType
		{
			get {return st;}
			set
			{
				if (st!=value)
				{
					st = value;
					if (ngbh!=null)
						Slots = ngbh.GetSlots(st);
				}
			}
		}


		void SetContent()
		{
			if (lv == null) return;
			lv.Items.Clear();
			if (slots!=null)
			{
				foreach (NgbhSlot s in slots)
				{
					lv.Items.Add(s.ToString());
				}
			}
		}

		private void lv_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (SelectedSlotChanged!=null) SelectedSlotChanged(this, e);
		}

		public NgbhSlot SelectedSlot
		{
			get
			{
				if (lv.SelectedIndex < 0 || slots == null) return null;
				int idx = lv.SelectedIndex;
				int i = 0;
				foreach (NgbhSlot s in slots)
				{
					if (i == idx) return s;
					i++;
				}
				return null;
			}
		}

		public event EventHandler SelectedSlotChanged;
	}
}
