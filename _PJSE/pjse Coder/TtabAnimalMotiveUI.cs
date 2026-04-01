/***************************************************************************
 *   Copyright (C) 2006 by Peter L Jones                                   *
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// Summary description for TtabSingleMotive.
	/// </summary>
	public class TtabAnimalMotiveUI : Avalonia.Controls.UserControl
	{
        #region Form variables
        private TextBoxCompat tbValue;
        private ButtonCompat btnPopup;
        #endregion

        public TtabAnimalMotiveUI()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void Dispose()
		{
		}


		#region TtabSingleMotiveUI
        private TtabItemAnimalMotiveItem item = null;

        public TtabItemAnimalMotiveItem Motive
        {
            get { return item; }
            set
            {
                if (this.item != value)
                {
                    if (item != null)
                        item.Wrapper.WrapperChanged -= new System.EventHandler(this.WrapperChanged);
                    this.item = value;
                    setText();
                    if (item != null)
                        item.Wrapper.WrapperChanged += (s, e) => this.WrapperChanged(s, e);
                }
            }
        }

        private void WrapperChanged(object sender, System.EventArgs e)
        {
            if (sender != item) return;
            setText();
        }

        private void setText()
        {
            this.tbValue.Text = "0x" +
                ((item.Count<0x100) ? Helper.HexString((byte)item.Count)
                : (item.Count<0x10000) ? Helper.HexString((ushort)item.Count)
                : Helper.HexString(item.Count))
                ;
            for (int i = 0; i < item.Count; i++)
            {
                this.tbValue.Text += "; " + Helper.HexString(item[i].Min)
                + " " + Helper.HexString(item[i].Delta)
                + " " + Helper.HexString(item[i].Type)
                ;
            }
        }

        public void Clear()
		{
            TtabItemAnimalMotiveItem newItem = new TtabItemAnimalMotiveItem(item.Parent);
            newItem.CopyTo(item);
            setText();
        }
		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tbValue = new TextBoxCompat();
            this.btnPopup = new ButtonCompat();
            this.tbValue.Name = "tbValue";
            this.tbValue.IsReadOnly = true;
            this.btnPopup.Name = "btnPopup";
            this.btnPopup.Click += (s, e) => btnPopup_Click(s, e);
		}
		#endregion

        private void btnPopup_Click(object sender, EventArgs e)
        {
            pjse.TtabAnimalMotiveWiz amw = new pjse.TtabAnimalMotiveWiz();
            amw.MotiveSet = item;
            amw.ShowDialog(null);
            // Note: Window result handling simplified; always reload
            setText();
        }

	}
}
