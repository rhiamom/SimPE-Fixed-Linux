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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;

namespace SimPe.Plugin.Tool
{
	/// <summary>
	/// Summary description for Report.
	/// </summary>
	internal class Report : Avalonia.Controls.Window
	{
        private Avalonia.Controls.StackPanel xpGradientPanel1;
        private Avalonia.Controls.TextBox rtb;
        private object sfd; // stub: was SaveFileDialog
        private Avalonia.Controls.Button button1;
		private System.ComponentModel.Container components = null;

		public Report()
		{
            InitializeComponent();

            ThemeManager tm = ThemeManager.Global.CreateChild();
            tm.AddControl(this.xpGradientPanel1);
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
            this.xpGradientPanel1 = new Avalonia.Controls.StackPanel();
            this.button1 = new Avalonia.Controls.Button { Content = "Save" };
            this.rtb = new Avalonia.Controls.TextBox { IsReadOnly = true };
            button1.Click += (s, e) => button1_Click(s, null);
		}
		#endregion

		System.IO.StreamWriter csv;
		public void Execute(System.IO.StreamWriter csv)
		{
			csv.Flush();
			csv.BaseStream.Seek(0, SeekOrigin.Begin);
			StreamReader sr = new StreamReader(csv.BaseStream);
			sr.BaseStream.Seek(0, SeekOrigin.Begin);

			this.csv = csv;
			this.rtb.Text = sr.ReadToEnd();
			this.Show(); // was ShowDialog()
		}


        private void button1_Click(object sender, EventArgs e)
        {
            // TODO: implement save file dialog via Avalonia StorageProvider
        }
	}
}
