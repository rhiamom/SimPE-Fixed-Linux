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

using SimPe.Interfaces.Plugin;
using System;
using Avalonia.Controls;
using Avalonia.Layout;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for TattUI.
	/// </summary>
	public class TattUI :
		SimPe.Windows.Forms.WrapperBaseControl
	{
		private Avalonia.Controls.TextBox tbVer;
		private Avalonia.Controls.TextBox tbRes;
		private Avalonia.Controls.TextBox tbFlname;
		private Avalonia.Controls.ListBox lb;

		public TattUI()
		{
			InitializeComponent();

            if (Helper.XmlRegistry.UseBigIcons) this.lb.FontSize = 16;
		}

		private void InitializeComponent()
		{
			this.tbFlname = new Avalonia.Controls.TextBox();
			this.tbVer    = new Avalonia.Controls.TextBox { IsReadOnly = true };
			this.tbRes    = new Avalonia.Controls.TextBox { IsReadOnly = true };
			this.lb       = new Avalonia.Controls.ListBox();

			this.tbFlname.TextChanged += (s, e) => tbFlname_TextChanged(s, EventArgs.Empty);

			// Layout: 3-row form at top (label+textbox pairs), then listbox filling the rest
			var grid = new Avalonia.Controls.Grid();
			grid.RowDefinitions = new Avalonia.Controls.RowDefinitions("Auto,Auto,Auto,Auto,*");
			grid.ColumnDefinitions = new Avalonia.Controls.ColumnDefinitions("Auto,*");

			var lbl1 = new Avalonia.Controls.TextBlock { Text = "FileName:", VerticalAlignment = VerticalAlignment.Center, Margin = new Avalonia.Thickness(4, 2) };
			var lbl2 = new Avalonia.Controls.TextBlock { Text = "Version:",  VerticalAlignment = VerticalAlignment.Center, Margin = new Avalonia.Thickness(4, 2) };
			var lbl3 = new Avalonia.Controls.TextBlock { Text = "Reserved:", VerticalAlignment = VerticalAlignment.Center, Margin = new Avalonia.Thickness(4, 2) };
			var lbl4 = new Avalonia.Controls.TextBlock { Text = "Items:",    VerticalAlignment = VerticalAlignment.Center, Margin = new Avalonia.Thickness(4, 2) };

			Avalonia.Controls.Grid.SetRow(lbl1, 0); Avalonia.Controls.Grid.SetColumn(lbl1, 0);
			Avalonia.Controls.Grid.SetRow(this.tbFlname, 0); Avalonia.Controls.Grid.SetColumn(this.tbFlname, 1);
			Avalonia.Controls.Grid.SetRow(lbl2, 1); Avalonia.Controls.Grid.SetColumn(lbl2, 0);
			Avalonia.Controls.Grid.SetRow(this.tbVer, 1); Avalonia.Controls.Grid.SetColumn(this.tbVer, 1);
			Avalonia.Controls.Grid.SetRow(lbl3, 2); Avalonia.Controls.Grid.SetColumn(lbl3, 0);
			Avalonia.Controls.Grid.SetRow(this.tbRes, 2); Avalonia.Controls.Grid.SetColumn(this.tbRes, 1);
			Avalonia.Controls.Grid.SetRow(lbl4, 3); Avalonia.Controls.Grid.SetColumn(lbl4, 0);
			Avalonia.Controls.Grid.SetRow(this.lb, 4); Avalonia.Controls.Grid.SetColumnSpan(this.lb, 2);

			grid.Children.Add(lbl1);
			grid.Children.Add(this.tbFlname);
			grid.Children.Add(lbl2);
			grid.Children.Add(this.tbVer);
			grid.Children.Add(lbl3);
			grid.Children.Add(this.tbRes);
			grid.Children.Add(lbl4);
			grid.Children.Add(this.lb);

			Content = grid;

			this.HeaderText = "TATT Wrapper";
			this.Commited += new System.EventHandler(this.TattUI_Commited);
		}

		public Tatt Tatt
		{
			get { return (Tatt)Wrapper ; }
		}

		public override void RefreshGUI()
		{
			base.RefreshGUI ();

			this.tbFlname.Text = Tatt.FileName;
			this.tbRes.Text = "0x"+Helper.HexString(Tatt.Reserved);
			this.tbVer.Text = "0x"+Helper.HexString(Tatt.Version);

			this.lb.Items.Clear();
			foreach (TattItem ti in Tatt)
				lb.Items.Add(ti);
		}

		private void TattUI_Commited(object sender, System.EventArgs e)
		{
			Tatt.SynchronizeUserData();
		}

		private void tbFlname_TextChanged(object sender, System.EventArgs e)
		{
			Tatt.FileName = tbFlname.Text;
			Tatt.Reserved = Helper.StringToUInt32(tbRes.Text, Tatt.Reserved, 16);
			Tatt.Version = Helper.StringToUInt32(tbVer.Text, Tatt.Version, 16);

			Tatt.Changed = true;
		}

	}
}
