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
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using TextBox  = SimPe.Scenegraph.Compat.TextBoxCompat;
using Label    = SimPe.Scenegraph.Compat.LabelCompat;
using MessageBox = SimPe.Scenegraph.Compat.MessageBox;

namespace SimPe.Packages
{
	/// <summary>
	/// Summary description for SaveSims2CommunityPack.
	/// </summary>
	internal class SaveSims2Pack : Avalonia.Controls.Window
	{
		private ListBoxCompat lblist;
		private GroupBox gbsettings;
		private Label label5;
		private TextBox tbdesc;
		private OpenFileDialogCompat ofd;
		private Label label6;
		internal TextBox tbflname;
		private SaveFileDialogCompat sfd;
		private Button btdelete;
		private Button button4;
		private Button btadd;
		private Button btbrowse;
		private Button btsave;
		private Label label9;
		private TextBox tbname;
        private PanelCompat panel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public SaveSims2Pack()
		{
            InitializeComponent();
            ThemeManager tm = ThemeManager.Global.CreateChild();
				tm.AddControl(this.panel1);
                tm.AddControl(this.lblist);
                tm.AddControl(this.tbdesc);
                tm.AddControl(this.btdelete);
                tm.AddControl(this.button4);
                tm.AddControl(this.btadd);
                tm.AddControl(this.btbrowse);
                tm.AddControl(this.btsave);
		}

		public void Dispose()
		{
			if (components != null)
			{
				components.Dispose();
			}
		}

		private void InitializeComponent()
		{
            this.lblist    = new ListBoxCompat();
            this.gbsettings = new GroupBox { Text = "Settings" };
            this.tbname    = new TextBox { IsReadOnly = true };
            this.tbdesc    = new TextBox();
            this.label5    = new Label { Text = "Description:" };
            this.label9    = new Label { Text = "File Name" };
            this.btadd     = new Button { Content = "Add..." };
            this.ofd       = new OpenFileDialogCompat { Filter = "Sims 2 Package (*.package)|*.package|All Files (*.*)|*.*" };
            this.label6    = new Label { Text = "FileName:" };
            this.tbflname  = new TextBox();
            this.btbrowse  = new Button { Content = "Browse..." };
            this.sfd       = new SaveFileDialogCompat();
            this.btdelete  = new Button { Content = "Remove..." };
            this.btsave    = new Button { Content = "Save" };
            this.button4   = new Button { Content = "Cancel" };
            this.panel1    = new PanelCompat();

            this.lblist.SelectedIndexChanged += new EventHandler(this.Select);
            this.tbdesc.TextChanged          += (s, e) => this.ChangeText(s, EventArgs.Empty);
            this.btadd.Click    += (s, e) => this.AddPackage(s, EventArgs.Empty);
            this.btbrowse.Click += (s, e) => this.S2CPFilename(s, EventArgs.Empty);
            this.btdelete.Click += (s, e) => this.DeletePackage(s, EventArgs.Empty);
            this.btsave.Click   += (s, e) => this.button3_Click(s, EventArgs.Empty);
            this.button4.Click  += (s, e) => this.button4_Click(s, EventArgs.Empty);
            this.Closing        += (s, e) => this.AllowClose(s, e);
            this.Opened         += (s, e) => this.SaveSims2CommunityPack_Load(s, e);

            var settingsStack = new StackPanel();
            settingsStack.Children.Add(label5);
            settingsStack.Children.Add(tbdesc);
            settingsStack.Children.Add(label9);
            settingsStack.Children.Add(tbname);
            gbsettings.Content = settingsStack;

            var filenameRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            filenameRow.Children.Add(label6);
            filenameRow.Children.Add(tbflname);
            filenameRow.Children.Add(btbrowse);

            var buttonRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            buttonRow.Children.Add(btadd);
            buttonRow.Children.Add(btdelete);
            buttonRow.Children.Add(button4);
            buttonRow.Children.Add(btsave);

            var mainStack = new StackPanel();
            mainStack.Children.Add(filenameRow);
            mainStack.Children.Add(lblist);
            mainStack.Children.Add(gbsettings);
            mainStack.Children.Add(buttonRow);

            panel1.Children.Add(mainStack);
            this.Content = panel1;
            this.Title = "Sims 2 Pack File Browser";
            this.Width = 578;
            this.Height = 460;
		}

		/// <summary>
		/// true if the communit Extensions should be used
		/// </summary>
		bool extension;

		/// <summary>
		/// true if the File should be saved
		/// </summary>
		bool ok;

		/// <summary>
		/// Shows the Save Dialog
		/// </summary>
		public S2CPDescriptor[] Execute(SimPe.Packages.GeneratableFile[] files, ref bool extension)
		{
			this.extension = extension;
			ok = false;

			S2CPDescriptor[] s2cps = new S2CPDescriptor[files.Length];
			for (int i=0; i<files.Length; i++)
			{
				s2cps[i] = new S2CPDescriptor(files[i], "", "", "", "", Sims2CommunityPack.DEFAULT_COMPRESSION_STRENGTH, new S2CPDescriptorBase[0], extension);
				lblist.Items.Add(s2cps[i]);
			}

			btadd.IsVisible    = true;
			btdelete.IsVisible = true;
			btbrowse.IsEnabled = true;
			btsave.Content     = "Save";

			this.lblist.SelectionMode = Avalonia.Controls.SelectionMode.Single;

			if (lblist.Items.Count>0) lblist.SelectedIndex=0;
			btdelete.IsEnabled = (lblist.SelectedIndex>=0);

			this.ShowDialog<object>(null).GetAwaiter().GetResult();

			extension = false;
			if (ok)
			{
				s2cps = new S2CPDescriptor[lblist.Items.Count];
				for (int i=0; i<s2cps.Length; i++)
				{
					s2cps[i] = (S2CPDescriptor)lblist.Items[i];
				}

				return s2cps;
			}

			return null;
		}

		/// <summary>
		/// Shows the Load Dialog
		/// </summary>
		public S2CPDescriptor[] Execute(S2CPDescriptor[] files, Avalonia.Controls.SelectionMode selmode)
		{
			this.extension = false;
			ok = false;

			for (int i=0; i<files.Length; i++) lblist.Items.Add(files[i]);

			this.tbflname.IsReadOnly = true;
			this.tbdesc.IsReadOnly   = true;
			btadd.IsVisible    = false;
			btdelete.IsVisible = false;
			btbrowse.IsEnabled = false;
			btsave.Content     = "Open";

			this.lblist.SelectionMode = selmode;

			if (lblist.Items.Count>0) lblist.SelectedIndex=0;
			btdelete.IsEnabled = (lblist.SelectedIndex>=0);

			this.ShowDialog<object>(null).GetAwaiter().GetResult();

			if (ok)
			{
				S2CPDescriptor[] fls = new S2CPDescriptor[lblist.SelectedItems.Count];
				for (int i=0; i<fls.Length; i++)
				{
					fls[i] = (S2CPDescriptor)lblist.SelectedItems[i];
				}

				return fls;
			}

			return null;
		}

		/// <summary>
		/// Select a List Item
		/// </summary>
		private void Select(object sender, System.EventArgs e)
		{
			if (lblist.Tag!=null) return;
			gbsettings.IsEnabled = false;
			btdelete.IsEnabled = false;
			if (lblist.SelectedIndex<0) return;
			gbsettings.IsEnabled = true;
			btdelete.IsEnabled = true;

			lblist.Tag = true;
			try
			{
				SimPe.Packages.S2CPDescriptor s2cp = (SimPe.Packages.S2CPDescriptor)lblist.Items[lblist.SelectedIndex];

				tbdesc.Text = s2cp.Description;
                tbname.Text = s2cp.Name;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lblist.Tag = null;
			}
		}

		private void ChangeText(object sender, System.EventArgs e)
		{
			if (lblist.Tag!=null) return;
			if (lblist.SelectedIndex<0) return;

			lblist.Tag = true;
			try
			{
				SimPe.Packages.S2CPDescriptor s2cp = (SimPe.Packages.S2CPDescriptor)lblist.Items[lblist.SelectedIndex];

				s2cp.Description = tbdesc.Text;

				lblist.Items[lblist.SelectedIndex] = s2cp;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lblist.Tag = null;
			}
		}

		private void AddPackage(object sender, System.EventArgs e)
		{
			ofd.Filter = "Sims 2 Package (*.package)|*.package|All Files (*.*)|*.*";
			if (ofd.ShowDialog() == SimPe.DialogResult.OK)
			{
				SimPe.Packages.GeneratableFile package = GeneratableFile.LoadFromFile(ofd.FileName);
				S2CPDescriptor s2cp = new S2CPDescriptor(package, "", "", "", "", Sims2CommunityPack.DEFAULT_COMPRESSION_STRENGTH, new S2CPDescriptorBase[0], extension);
				lblist.Items.Add(s2cp);
				lblist.SelectedIndex = lblist.Items.Count-1;
			}
		}

		private void DeletePackage(object sender, System.EventArgs e)
		{
			if (lblist.SelectedIndex<0) return;

			lblist.Items.RemoveAt(lblist.SelectedIndex);
		}

		private void S2CPFilename(object sender, System.EventArgs e)
		{
			sfd.Filter = "Sims 2 Package (*.sims2pack)|*.sims2pack|All Files (*.*)|*.*";
			if (sfd.ShowDialog() == SimPe.DialogResult.OK)
			{
				tbflname.Text = sfd.FileName;
			}
		}

		private void AllowClose(object sender, Avalonia.Controls.WindowClosingEventArgs e)
		{
			if (tbflname.IsReadOnly)
			{
				if ((lblist.SelectedItems.Count==0) && (ok))
				{
					MessageBox.Show("You have to select at Least one Package");
					e.Cancel = true;
				}
			}
			else
			{
				if ((tbflname.Text.Trim()=="") && (ok))
				{
					MessageBox.Show("You have to specify a Filename for the Sims2Community Pack File.");
					e.Cancel = true;
				}

				if ((lblist.Items.Count==0) && (ok))
				{
					MessageBox.Show("You have to add at least one Package.");
					e.Cancel = true;
				}
			}
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			ok = false;
			Close();
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			ok = true;
			Close();
		}

		private void SaveSims2CommunityPack_Load(object sender, System.EventArgs e)
		{
		}
	}
}
