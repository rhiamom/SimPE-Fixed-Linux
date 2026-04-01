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
using System.Windows.Forms;

namespace SimPe
{
	/// <summary>
	/// Zusammenfassung f�r Hidden.
	/// </summary>
	public class Hidden : Avalonia.Controls.Window
	{
		private Avalonia.Controls.TextBlock label1;
		private Avalonia.Controls.TextBox tbComp;
		private Avalonia.Controls.TextBox tbBig;
		private Avalonia.Controls.TextBlock label2;
		private Avalonia.Controls.TextBlock label3;
		private Avalonia.Controls.TextBlock lbMem;
		private Avalonia.Controls.Button button1;
		private Avalonia.Controls.Button button2;
		private Avalonia.Controls.Button button3;
        private Avalonia.Controls.Button button4;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Hidden()
		{
			//
			// Erforderlich f�r die Windows Form-Designerunterst�tzung
			//
			InitializeComponent();

			button3.IsVisible = Helper.XmlRegistry.HiddenMode;
		}


		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode f�r die Designerunterst�tzung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor ge�ndert werden.
		/// </summary>
		private void InitializeComponent() { }
		#endregion

		private void Hidden_Load(object sender, System.EventArgs e)
		{
            UpdateDialog();
		}

        private void UpdateDialog()
        {
            this.tbComp.Text = SimPe.Packages.PackedFile.CompressionStrength.ToString();
            tbBig.Text = Helper.XmlRegistry.BigPackageResourceCount.ToString();

            this.lbMem.Text = GC.GetTotalMemory(false).ToString("N0") + " Byte";
        }

		private void Hidden_Closed(object sender, System.EventArgs e)
		{
			try 
			{
				SimPe.Packages.PackedFile.CompressionStrength = Convert.ToInt32(this.tbComp.Text);
				Helper.XmlRegistry.BigPackageResourceCount = Convert.ToInt32(tbBig.Text);
			} 
			catch {}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			this.lbMem.Text = GC.GetTotalMemory(false).ToString("N0") + " Byte";
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			SimPe.Packages.StreamFactory.WriteToConsole();
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			SimPe.FileTable.FileIndex.WriteContentToConsole();
		}

        private void button4_Click(object sender, EventArgs e)
        {
            // TODO: settings PropertyGrid dialog not ported to Avalonia
            UpdateDialog();
        }
	}
}
