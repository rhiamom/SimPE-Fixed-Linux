/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
 *   peter@users.sf.net                                                    *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   Rhiamom@mac.com                                                       *
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using SimPe.PackedFiles.Wrapper;
using SimPe.Interfaces.Plugin;

namespace pjse
{
	/// <summary>
	/// Summary description for StrBig.
	/// </summary>
	public class StrChooser : Window
	{
		#region Form variables
		private StackPanel panel1;
		private ButtonCompat OK;
		private ButtonCompat Cancel;
		private Avalonia.Controls.ListBox lbItemList;
		#endregion

		private bool _dialogResult = false;

		public StrChooser()
		{
            InitializeComponent();
		}

        public StrChooser(bool sortflag) : this() { this.sortflag = sortflag; }

		public void Dispose()
		{
		}


		#region StrChooser
        private bool sortflag = false;

		public int Strnum(StrWrapper wrapper)
		{
			fill(wrapper);

			this.ShowDialog(null).GetAwaiter().GetResult();

			if (_dialogResult)
			{
				if (lbItemList.SelectedIndex >= 0) return (int)((SimPe.Data.Alias)lbItemList.SelectedItem).Id;
				return -1;
			}
			return -1;
		}

		private void fill(StrWrapper wrapper)
		{
			this.lbItemList.Items.Clear();

			for (int i = 0; wrapper[1, i] != null; i++)
				lbItemList.Items.Add(new SimPe.Data.Alias((uint)i, wrapper[1, i].Title));
		}

		#endregion

		private void InitializeComponent()
		{
			this.panel1 = new StackPanel();
			this.OK = new ButtonCompat { Content = "OK" };
			this.Cancel = new ButtonCompat { Content = "Cancel" };
			this.lbItemList = new Avalonia.Controls.ListBox();

			this.OK.Click += (s, e) => { _dialogResult = true; Close(); };
			this.Cancel.Click += (s, e) => { _dialogResult = false; Close(); };
			this.lbItemList.DoubleTapped += (s, e) => lbItemList_DoubleClick(s, e);

			this.panel1.Children.Add(this.OK);
			this.panel1.Children.Add(this.Cancel);

			var mainPanel = new StackPanel();
			mainPanel.Children.Add(this.lbItemList);
			mainPanel.Children.Add(this.panel1);
			this.Content = mainPanel;
		}

		private void lbItemList_DoubleClick(object sender, System.EventArgs e)
		{
			if (lbItemList.SelectedIndex >= 0)
			{
				_dialogResult = true;
				this.Close();
			}
		}

	}
}
