/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
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
using Avalonia.Controls;
using Avalonia.Layout;
using SimPe.Scenegraph.Compat;
using Button   = Avalonia.Controls.Button;
using ListBox  = SimPe.Scenegraph.Compat.ListBoxCompat;
using Panel    = SimPe.Scenegraph.Compat.PanelCompat;
using MessageBox    = SimPe.Scenegraph.Compat.MessageBox;
using MessageBoxButtons = SimPe.Scenegraph.Compat.MessageBoxButtons;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for NgbBackup.
	/// </summary>
	public class NgbBackup : Avalonia.Controls.Window
	{
		private ListBox lbdirs;
		private Button button1;
		private Button button2;
        private Panel pnNice;
        ThemeManager tm;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NgbBackup()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected virtual void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
		}

		#region Avalonia layout
		private void InitializeComponent()
		{
			this.Title  = "Backup Browser";
			this.Width  = 500;
			this.Height = 300;

			this.lbdirs  = new ListBox();
			this.lbdirs.SelectedIndexChanged += (s, e) => SelectBackup(s, e);

			this.button1 = new Button { Content = "Restore", IsEnabled = false };
			this.button2 = new Button { Content = "Delete",  IsEnabled = false };
			this.button1.Click += (s, e) => Restore(s, e);
			this.button2.Click += (s, e) => Delete(s, e);

			var btnRow = new Avalonia.Controls.StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
			btnRow.Children.Add(this.button1);
			btnRow.Children.Add(this.button2);

			var root = new Avalonia.Controls.Grid { Margin = new Avalonia.Thickness(8) };
			root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			Avalonia.Controls.Grid.SetRow(this.lbdirs, 0);
			Avalonia.Controls.Grid.SetRow(btnRow, 1);
			root.Children.Add(this.lbdirs);
			root.Children.Add(btnRow);

			this.pnNice = new Panel();
			this.pnNice.Children.Add(root);

			this.Content = this.pnNice;
		}
		#endregion

		string path;
		string backuppath;

		protected void UpdateList()
		{
			lbdirs.Items.Clear();
			if (System.IO.Directory.Exists(backuppath))
			{
				string[] dirs = System.IO.Directory.GetDirectories(backuppath, "*");
				foreach (string dir in dirs)
				{
					lbdirs.Items.Add(System.IO.Path.GetFileName(dir));
				}
			}
		}

		SimPe.Interfaces.Files.IPackageFile package;
		Interfaces.IProviderRegistry prov;
		public void Execute(string path, SimPe.Interfaces.Files.IPackageFile package, Interfaces.IProviderRegistry prov, string lable)
		{
			this.path = path;
			this.package = package;
			this.prov = prov;

			string name = System.IO.Path.GetFileName(path);
            if (lable != "") name = lable + "_" + name;
            long grp = PathProvider.Global.SaveGamePathProvidedByGroup(path);
            if (grp > 1) name = grp.ToString() + "_" + name;
            backuppath = System.IO.Path.Combine(PathProvider.Global.BackupFolder, name);

			UpdateList();

			this.Show();
		}

		private void SelectBackup(object sender, System.EventArgs e)
		{
			button1.IsEnabled = (lbdirs.SelectedIndex>=0);
			button2.IsEnabled = button1.IsEnabled;
		}

		private async void Restore(object sender, System.EventArgs e)
		{
			if (lbdirs.SelectedIndex < 0) return;

			prov.SimDescriptionProvider.BasePackage = null;
			prov.SimFamilynameProvider.BasePackage = null;
			prov.SimNameProvider.BaseFolder = null;
			DialogResult dr = await MessageBox.ShowAsync(Localization.Manager.GetString("backuprestore"), Localization.Manager.GetString("backup?"), MessageBoxButtons.YesNoCancel);
			if (dr!=DialogResult.Cancel)
			{
				SimPe.Packages.StreamFactory.CloseAll();
                this.Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Wait);
				WaitingScreen.Wait();

				try
				{
					string source = System.IO.Path.Combine(backuppath, lbdirs.Items[lbdirs.SelectedIndex].ToString());

					if (dr==DialogResult.Yes)
					{
						//create backup of current
						string newback= System.IO.Path.Combine(backuppath, "(automatic) "+DateTime.Now.ToString().Replace("\\", "-").Replace(":", "-").Replace(".", "-"));
						if (!System.IO.Directory.Exists(newback)) System.IO.Directory.CreateDirectory(newback);
						Helper.CopyDirectory(path, newback, true);
					}

					//remove the Neighborhood
					try
					{
						SimPe.Packages.PackageMaintainer.Maintainer.RemovePackagesInPath(path);
						System.IO.Directory.Delete(path, true);
					}
					catch (Exception) {}

					//copy the backup
					System.IO.Directory.CreateDirectory(path);
					Helper.CopyDirectory(source, path, true);

					UpdateList();
					WaitingScreen.Stop(this);
					MessageBox.Show("The backup was restored succesfully!");
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage("", ex);
				}
				finally
				{
					WaitingScreen.Stop();
					this.Cursor = Avalonia.Input.Cursor.Default;
				}
			}
		}

		private async void Delete(object sender, System.EventArgs e)
		{
			if (lbdirs.SelectedIndex < 0) return;
			string source = System.IO.Path.Combine(backuppath, lbdirs.Items[lbdirs.SelectedIndex].ToString());
			if (await MessageBox.ShowAsync(Localization.Manager.GetString("backupdelete").Replace("{0}", source), Localization.Manager.GetString("delete?"), MessageBoxButtons.YesNo)==DialogResult.Yes)
			{
                this.Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Wait);

				if (System.IO.Directory.Exists(source)) System.IO.Directory.Delete(source, true);
				UpdateList();
				this.Cursor = Avalonia.Input.Cursor.Default;
			}
		}
	}
}
