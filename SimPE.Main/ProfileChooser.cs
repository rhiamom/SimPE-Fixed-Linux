/***************************************************************************
 *   Copyright (C) 2008 by Peter L Jones                                   *
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SimPe
{
    public partial class ProfileChooser : Avalonia.Controls.Window, IDisposable
    {
        public ProfileChooser()
        {
            InitializeComponent();
        }

        public string Value
        {
            get
            {
                return cbProfiles.SelectedItem?.ToString() ?? "";
            }
        }

        private void ProfileChooser_Activated(object sender, EventArgs e)
        {
            // cbProfiles.BeginUpdate(); // not available on Avalonia ComboBox
            cbProfiles.Items.Clear();
            foreach (string s in Directory.GetDirectories(SimPe.Helper.DataFolder.Profiles))
                cbProfiles.Items.Add(Path.GetFileName(s));
            // cbProfiles.EndUpdate(); // not available on Avalonia ComboBox

            btnOK.IsEnabled = false;
        }

        private void ProfileChooser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing && e.CloseReason != CloseReason.None) return;
            // this.DialogResult check not applicable on Avalonia Window

            string text = cbProfiles.SelectedItem?.ToString()?.Trim() ?? "";
            if (text.Length == 0) { e.Cancel = true; return; }

            string path = Path.Combine(Helper.DataFolder.Profiles, text);
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    // TODO: show error message — MessageBox not available without owner
                    System.Diagnostics.Debug.WriteLine("ProfileChooser: " + ex.Message);
                    e.Cancel = true;
                }
            }
            // else: path exists, proceed
        }

        private void cbProfiles_TextChanged(object sender, EventArgs e)
        {
            btnOK.IsEnabled = (cbProfiles.SelectedItem?.ToString()?.Trim().Length ?? 0) != 0;
        }

        public void Dispose() { }

        public new System.Windows.Forms.DialogResult ShowDialog()
        {
            this.Show();
            return System.Windows.Forms.DialogResult.Cancel;
        }
    }
}