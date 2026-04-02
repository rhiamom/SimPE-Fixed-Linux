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
using System.Windows.Forms;
using Avalonia.Controls;
using Avalonia.Layout;
using SimPe.Scenegraph.Compat;
using Button      = Avalonia.Controls.Button;
using Label       = SimPe.Scenegraph.Compat.LabelCompat;
using TextBox     = SimPe.Scenegraph.Compat.TextBoxCompat;
using CheckBox    = Avalonia.Controls.CheckBox;
using RadioButton = Avalonia.Controls.RadioButton;
using Panel       = SimPe.Scenegraph.Compat.PanelCompat;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for Hash.
	/// </summary>
	public class Hash : Avalonia.Controls.Window
	{
        private Label label1;
		private Label label4;
        private TextBox tbtext;
		private TextBox tbhash;
		private RadioButton rb24;
		private RadioButton rb32;
		private RadioButton radioButton1;
        private Panel panel1;
        private Button btcopy;
        private CheckBox cbTrim;
        private Label lbnamer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Hash()
		{
			//
			// Required designer variable.
			//
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
            this.Title  = "Hash Generator";
            this.Width  = 500;
            this.Height = 170;

            // ── Labels ────────────────────────────────────────────────────────────
            this.label1  = new Label { Text = "String:" };
            this.label4  = new Label { Text = "Hash Value:" };
            this.lbnamer = new Label { Text = "Available", IsVisible = false };

            // ── Text boxes ────────────────────────────────────────────────────────
            this.tbtext = new TextBox();
            this.tbtext.TextChanged += this.tbtext_TextChanged;

            this.tbhash = new TextBox { Text = "0xB00B0069", IsReadOnly = true };

            // ── Radio buttons ─────────────────────────────────────────────────────
            this.rb24 = new RadioButton { Content = "CRC 24", IsChecked = true, GroupName = "hashtype" };
            this.rb32 = new RadioButton { Content = "CRC 32", GroupName = "hashtype" };
            this.radioButton1 = new RadioButton { Content = "GUID", GroupName = "hashtype" };
            this.rb24.IsCheckedChanged         += (s, e) => rb14_CheckedChanged(s, EventArgs.Empty);
            this.rb24.IsCheckedChanged         += (s, e) => tbtext_TextChanged(s, EventArgs.Empty);
            this.rb32.IsCheckedChanged         += (s, e) => rb32_CheckedChanged(s, EventArgs.Empty);
            this.rb32.IsCheckedChanged         += (s, e) => tbtext_TextChanged(s, EventArgs.Empty);
            this.radioButton1.IsCheckedChanged += (s, e) => guid_CheckedChanged(s, EventArgs.Empty);

            // ── CheckBox + Copy button ────────────────────────────────────────────
            this.cbTrim = new CheckBox { Content = "Use Lower Case Only", IsChecked = true };
            this.cbTrim.IsCheckedChanged += (s, e) => cbTrim_CheckedChanged(s, EventArgs.Empty);

            this.btcopy = new Button { Content = "Copy" };
            this.btcopy.Click += (s, e) => btcopy_Click(s, e);

            // ── Row 0: String label + tbtext ─────────────────────────────────────
            var row0 = new Avalonia.Controls.StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
            row0.Children.Add(this.label1);
            row0.Children.Add(this.tbtext);

            // ── Row 1: cbTrim + radio buttons ─────────────────────────────────────
            var row1 = new Avalonia.Controls.StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            row1.Children.Add(this.cbTrim);
            row1.Children.Add(this.rb24);
            row1.Children.Add(this.rb32);
            row1.Children.Add(this.radioButton1);

            // ── Row 2: Hash label + tbhash + btcopy + lbnamer ────────────────────
            var row2 = new Avalonia.Controls.StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
            row2.Children.Add(this.label4);
            row2.Children.Add(this.tbhash);
            row2.Children.Add(this.btcopy);
            row2.Children.Add(this.lbnamer);

            // ── Root ──────────────────────────────────────────────────────────────
            this.panel1 = new Panel();
            var root = new Avalonia.Controls.StackPanel { Orientation = Orientation.Vertical, Spacing = 6, Margin = new Avalonia.Thickness(8) };
            root.Children.Add(row0);
            root.Children.Add(row1);
            root.Children.Add(row2);
            this.panel1.Children.Add(root);

            this.Content = this.panel1;
		}
        #endregion

        public void Execute(Interfaces.Files.IPackageFile package)
        {
            if (package != null)
            {
                if (package.FileName != null)
                    this.tbtext.Text = System.IO.Path.GetFileNameWithoutExtension(package.FileName).ToLower();
                else
                    this.tbtext.Text = "Generate Hashes";
            }
            else
            {
                this.tbtext.Text = "Generate Hashes";
            }

            this.Show();
        }


        private void tbtext_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				ulong hash = 0;
                if (cbTrim.IsChecked == true)
                {
                    if (rb24.IsChecked == true) hash = Hashes.ToLong(Hashes.Crc24.ComputeHash(Helper.ToBytes(tbtext.Text.ToLower())));
                    else hash = Hashes.ToLong(Hashes.Crc32.ComputeHash(Helper.ToBytes(tbtext.Text.ToLower())));
                }
                else
                {
                    if (rb24.IsChecked == true) hash = Hashes.ToLong(Hashes.Crc24.ComputeHash(Helper.ToBytes(tbtext.Text)));
                    else hash = Hashes.ToLong(Hashes.Crc32.ComputeHash(Helper.ToBytes(tbtext.Text)));
                }
				tbhash.Text = "0x"+Helper.HexString((uint)hash);
                setupinuse(hash);
			}
			catch (Exception)
			{
			}
		}

		private void rb32_CheckedChanged(object sender, System.EventArgs e)
		{
            tbtext.IsEnabled = cbTrim.IsEnabled = true;
		}

		private void guid_CheckedChanged(object sender, System.EventArgs e)
		{
            tbtext.IsEnabled = cbTrim.IsEnabled = false;
            lbnamer.IsVisible = false;
			tbhash.Text = System.Guid.NewGuid().ToString();
		}

		private void rb14_CheckedChanged(object sender, System.EventArgs e)
		{
            tbtext.IsEnabled = cbTrim.IsEnabled = true;
		}

        private void btcopy_Click(object sender, EventArgs e)
        {
            _ = this.Clipboard?.SetTextAsync(tbhash.Text);
        }

        private void cbTrim_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.IsChecked != true) tbtext_TextChanged(sender, e);
        }

        private void setupinuse(ulong vid)
        {
            if (rb32.IsChecked == true)
            {
                lbnamer.IsVisible = true;
                string objName = pjse.GUIDIndex.TheGUIDIndex[Convert.ToUInt32(vid)];
                if (objName != null && objName.Length > 0)
                    lbnamer.Text = objName;
                else
                    lbnamer.Text = "Available";
            }
            else
                lbnamer.IsVisible = false;
        }
	}
}
