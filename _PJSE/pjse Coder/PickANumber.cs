/***************************************************************************
 *   Copyright (C) 2007 by Peter L Jones                                   *
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;

namespace pjse
{
    public partial class PickANumber : Window
    {
        private bool _dialogAccepted = false;
        public bool DialogAccepted { get { return _dialogAccepted; } }

        public PickANumber()
        {
            InitializeComponent();
        }

        private List<TextBoxCompat> ltb = new List<TextBoxCompat>();
        private List<Avalonia.Controls.RadioButton> lrb = new List<Avalonia.Controls.RadioButton>();
        private List<pjse.BhavOperandWizards.DataOwnerControl> ldoc = new List<pjse.BhavOperandWizards.DataOwnerControl>();
        private int selectedRB = -1;

        public PickANumber(ushort[] values, string[] labels) : this()
        {
            for (int i = 0; i < values.Length; i++)
            {
                TextBoxCompat t = new TextBoxCompat();
                t.Name = "textBox" + (i + 2).ToString();
                ltb.Add(t);
                t.IsEnabled = false;
                pjse.BhavOperandWizards.DataOwnerControl d = new pjse.BhavOperandWizards.DataOwnerControl(null, null, null,
                    t, null, null, null, 0x07, values[i]);
                ldoc.Add(d);

                Avalonia.Controls.RadioButton r = new Avalonia.Controls.RadioButton();
                r.Content = labels[i];
                r.IsChecked = false;
                r.IsCheckedChanged += (s, e) => radioButton1_CheckedChanged(s, e);
                lrb.Add(r);
            }

            ltb[ltb.Count - 1].IsEnabled = true;
            ltb[ltb.Count - 1].GotFocus += (s, e) => ltbLast_Enter(s, e);
            lrb[0].IsChecked = true;
        }

        public uint Value
        {
            get
            {
                return (selectedRB >= 0) ? ldoc[selectedRB].Value : (ushort)0xffff;
            }
        }

        public String FormTitle
        {
            get { return this.Title; }
            set { this.Title = value; }
        }

        public String Prompt
        {
            get { return this.label1?.Content?.ToString() ?? ""; }
            set { if (this.label1 != null) this.label1.Content = value; }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            selectedRB = lrb.IndexOf((Avalonia.Controls.RadioButton)sender);
        }

        private void ltbLast_Enter(object sender, EventArgs e)
        {
            lrb[ltb.Count - 1].IsChecked = true;
        }
    }
}
