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

namespace pjse
{
	/// <summary>
	/// Summary description for StrBig.
	/// </summary>
	public class StrBig : Window
	{
		#region Form variables
		private StackPanel panel1;
		private TextBoxCompat richTextBox1;
		private ButtonCompat OK;
		private ButtonCompat Cancel;
		#endregion

		private bool _dialogResult = false;

		public StrBig()
		{
			InitializeComponent();
		}

		public void Dispose()
		{
		}


		#region StrBig
		public string doBig(string init)
		{
			richTextBox1.Text = init;

			this.ShowDialog(null).GetAwaiter().GetResult();

			if (!_dialogResult)
				return null;
			return richTextBox1.Text;
		}

		#endregion

		private void InitializeComponent()
		{
			this.panel1 = new StackPanel();
			this.OK = new ButtonCompat { Content = "OK" };
			this.Cancel = new ButtonCompat { Content = "Cancel" };
			this.richTextBox1 = new TextBoxCompat { AcceptsReturn = true };

			this.OK.Click += (s, e) => { _dialogResult = true; Close(); };
			this.Cancel.Click += (s, e) => { _dialogResult = false; Close(); };

			this.panel1.Children.Add(this.OK);
			this.panel1.Children.Add(this.Cancel);

			var mainPanel = new StackPanel();
			mainPanel.Children.Add(this.richTextBox1);
			mainPanel.Children.Add(this.panel1);
			this.Content = mainPanel;
		}
	}
}
