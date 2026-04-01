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
	/// Zusammenfassung f�r FileTableItemForm.
	/// </summary>
	public class FileTableItemForm : Avalonia.Controls.Window
	{
		private Avalonia.Controls.StackPanel xpGradientPanel1;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Avalonia.Controls.TextBlock label1;
		private Avalonia.Controls.TextBlock label2;
		private Avalonia.Controls.TextBlock label3;
		private Ambertation.Windows.Forms.TransparentCheckBox cbRec;
		private Avalonia.Controls.TextBox tbName;
		private Avalonia.Controls.TextBox tbRoot;
		private Avalonia.Controls.ComboBox cbEpVer;
		private Avalonia.Controls.Button button1;
		private Avalonia.Controls.Button button2;
		private Avalonia.Controls.Button button3;
		private System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
		private System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();

		SimPe.ThemeManager tm;
		public FileTableItemForm()
		{
			//
			// Erforderlich f�r die Windows Form-Designerunterst�tzung
			//
			InitializeComponent();

			tm = SimPe.ThemeManager.Global.CreateChild();
			tm.AddControl(this.xpGradientPanel1);

			ofd.Filter = SimPe.ExtensionProvider.BuildFilterString(new SimPe.ExtensionType[] { SimPe.ExtensionType.Package, SimPe.ExtensionType.AllFiles });

            this.cbEpVer.Items.Clear();
            cbEpVer.Items.Add(SimPe.Localization.GetString("All"));
            foreach (ExpansionItem ei in PathProvider.Global.Expansions)
            {
                cbEpVer.Items.Add(ei.Name);
            }
		}


		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode f�r die Designerunterst�tzung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor ge�ndert werden.
		/// </summary>
		private void InitializeComponent() { }
		#endregion

		bool ok;
		bool file;
		public static FileTableItem Execute()
		{
			FileTableItem fti = new FileTableItem("", false, false);
			
			if (Execute(fti)) return fti;
			else return null;
		}

		public static bool Execute(FileTableItem fti)
		{
			FileTableItemForm f = new FileTableItemForm();
			f.tbName.Text = fti.Name;
			f.tbRoot.Text = fti.Type.ToString();
            if (fti.EpVersion + 1 < f.cbEpVer.Items.Count)
                f.cbEpVer.SelectedIndex = fti.EpVersion + 1;
            else
            {
                ExpansionItem ei = PathProvider.Global[fti.EpVersion];
                for (int i = 0; i < f.cbEpVer.Items.Count; i++)
                {
                    if (f.cbEpVer.Items[i].ToString() == ei.Name)
                    {
                        f.cbEpVer.SelectedIndex = i;
                        break;
                    }

                }
            }
			f.cbRec.Checked = fti.IsRecursive;
			f.ok = false;
			f.file = fti.IsFile;
			f.UpdateRec();

			f.ShowDialog();

			if (f.ok) 
			{
                fti.Type = FileTablePaths.Absolute;
				fti.Name = f.tbName.Text.Trim();
				fti.IsRecursive = f.cbRec.Checked;
                string epname = f.cbEpVer.SelectedItem?.ToString() ?? "";
                foreach (ExpansionItem ei in PathProvider.Global.Expansions)
                    if (ei.Name == epname) fti.EpVersion = ei.Version;
				
				fti.IsFile = f.file;

				return true;
			}

			return false;
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			ok = true;
			Close();
		}

		void UpdateType()
		{
			FileTableItem fti = new FileTableItem(tbName.Text, false, file);
			fti.Name = tbName.Text;

			this.tbRoot.Text = fti.Type.ToString();			
		}

		void UpdateRec()
		{
			this.cbRec.Enabled = !file;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
			{
				file = false;
				tbName.Text = fbd.SelectedPath;		
		
				UpdateType();
				UpdateRec();
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
			{
				file = true;
				tbName.Text = ofd.FileName;			
		
				UpdateType();	
				UpdateRec();
			}
		}

		private void tbName_TextChanged(object sender, System.EventArgs e)
		{
		
		}
	}
}
