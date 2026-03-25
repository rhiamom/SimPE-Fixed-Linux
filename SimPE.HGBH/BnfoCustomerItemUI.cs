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
using System.Drawing;
using Avalonia.Controls;
using Ambertation.Windows.Forms;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for BnfoCustomerItemUI.
	/// </summary>
	public class BnfoCustomerItemUI : Avalonia.Controls.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		public BnfoCustomerItemUI()
		{
			InitializeComponent();

			try
			{
				tb.IsVisible = Helper.XmlRegistry.HiddenMode;
				SetContent();
			}
			catch {}
		}

		#region Windows Form Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tb = new Avalonia.Controls.TextBox();
            this.pb = new LabeledProgressBar();

            // tb
            this.tb.IsReadOnly = true;
            this.tb.TextChanged += (s, e) => tb_TextChanged(s, EventArgs.Empty);

            // pb
            this.pb.DisplayOffset = 0;
            this.pb.Maximum = 2000;
            this.pb.NumberFormat = "N0";
            this.pb.NumberOffset = -1000;
            this.pb.NumberScale = 0.005;
            this.pb.SelectedColor = System.Drawing.Color.Gold;
            this.pb.TokenCount = 11;
            this.pb.UnselectedColor = System.Drawing.Color.Black;
            this.pb.Value = 1000;
            this.pb.ChangedValue += new System.EventHandler(this.pb_Changed);

            var panel = new Avalonia.Controls.StackPanel();
            panel.Children.Add(this.tb);
            panel.Children.Add(this.pb);
            Content = panel;
		}
		#endregion

		BnfoCustomerItem item;
		private Avalonia.Controls.TextBox tb;
	
		[System.ComponentModel.Browsable(false)]
		public BnfoCustomerItem Item
		{
			get {return item;}
			set 
			{
				/*if (item!=null) 
				{
					item.LoyaltyScore = pb.Value;
				}*/
				item = value;
				SetContent();
			}
		}

		BnfoCustomerItemsUI ui;
		private LabeledProgressBar pb;
	
		public BnfoCustomerItemsUI BnfoCustomerItemsUI
		{
			get {return ui;}
			set 
			{
				if (ui!=null) ui.SelectedItemChanged -= new EventHandler(ui_SelectedItemChanged);
				ui = value;
				if (ui!=null) 
				{
					ui.SelectedItemChanged += new EventHandler(ui_SelectedItemChanged);
					ui_SelectedItemChanged(ui, null);
				}
			}
		}

		bool intern;
		void SetContent()
		{
			if (intern) return;
			intern = true;
			if (item!=null) 
			{
				tb.Text = Helper.BytesToHexList(item.Data);
				pb.Value = item.LoyaltyScore;				
				pb.IsEnabled = true;
			} 
			else 
			{
				tb.Text = "";
				pb.Value = 0;
				pb.IsEnabled = false;
			}
			intern = false;
		}

		private void ui_SelectedItemChanged(object sender, EventArgs e)
		{
			Item = ui.SelectedItem;
		}

		private void tb_TextChanged(object sender, System.EventArgs e)
		{
		
		}		

		private void pb_Changed(object sender, System.EventArgs e)
		{
			if (intern) return;
			if (item==null) return;
			if (pb.Value<0 && pb.SelectedColor!=Color.Coral) 
			{
				pb.SelectedColor = Color.Coral;
				pb.InvalidateVisual();
			}
			else if (pb.Value>=0 && pb.SelectedColor!=Color.Gold) 
			{
				pb.SelectedColor = Color.Gold;
				pb.InvalidateVisual();
			}

			item.LoyaltyScore = pb.Value;
		}
	}
}
