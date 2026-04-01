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
	/// Zusammenfassung f�r AddExtTool.
	/// </summary>
	public class AddExtTool : Avalonia.Controls.Window
	{
		private Avalonia.Controls.TextBlock label1;
		private Avalonia.Controls.TextBlock label2;
		private Avalonia.Controls.TextBlock label3;
		private Avalonia.Controls.TextBlock label4;
		private Avalonia.Controls.Button button1;
		private Avalonia.Controls.TextBox tbname;
		private Avalonia.Controls.TextBox tbfile;
		private Avalonia.Controls.TextBox tbattr;
		private Avalonia.Controls.Button button2;
		private System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
		private Avalonia.Controls.TextBox tbtype;
		private Avalonia.Controls.ComboBox cbtypes;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddExtTool()
		{
			//
			// Erforderlich f�r die Windows Form-Designerunterst�tzung
			//
			InitializeComponent();

			foreach (SimPe.Data.TypeAlias a in SimPe.Helper.TGILoader.FileTypes) 
			{
				if (a.Id==0xffffffff) 
				{
					SimPe.Data.TypeAlias an = new SimPe.Data.TypeAlias(false, "ALL", 0xffffffff, "---  All Types ---", true, true);
					cbtypes.Items.Add(an);
				} else  cbtypes.Items.Add(a);
			}
		}


		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode f�r die Designerunterst�tzung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor ge�ndert werden.
		/// </summary>
		private void InitializeComponent() { }
		#endregion

		ToolLoaderItemExt tli;
		public ToolLoaderItemExt Execute() 
		{
			tli = null;

			this.tbname.Text = Localization.Manager.GetString("Unknown");
			this.tbtype.Text = "0xffffffff";
			this.tbattr.Text = "{tempfile}";
			this.tbfile.Text = "";
			
			this.Show();
			return tli;
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			ofd.FileName = tbfile.Text;
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) tbfile.Text = ofd.FileName;
		}

		private void TypeSelectClick(object sender, System.EventArgs e)
		{
			if (cbtypes.Tag != null) return;
			tbtype.Text = "0x"+Helper.HexString(((SimPe.Data.TypeAlias)cbtypes.Items[cbtypes.SelectedIndex]).Id);
		}

		private void SelectTypeByNameClick(object sender, System.EventArgs e)
		{
			cbtypes.Tag = true;
			Data.TypeAlias a = Data.MetaData.FindTypeAlias(Helper.HexStringToUInt(tbtype.Text));

			int ct=0;
			foreach(Data.TypeAlias i in cbtypes.Items) 
			{								
				if (i==a) 
				{
					cbtypes.SelectedIndex = ct;
					cbtypes.Tag = null;
					return;
				}
				ct++;
			}

			cbtypes.SelectedIndex = -1;
			cbtypes.Tag = null;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			tli = new ToolLoaderItemExt(tbname.Text);
			tli.Attributes = tbattr.Text;
			tli.FileName = tbfile.Text;
			try 
			{
				tli.Type = Convert.ToUInt32(tbtype.Text);
			}
			catch (Exception) 
			{
				tli.Type = 0xffffffff;
			}

			Close();
		}
	}
}
