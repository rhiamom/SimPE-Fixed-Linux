/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
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
	public class TtabSingleMotiveUI : Avalonia.Controls.UserControl
	{
        #region Form variables
        private TextBoxCompat Min;
		private TextBoxCompat Delta;
		private TextBoxCompat Type;
        #endregion

        public TtabSingleMotiveUI()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			TextBoxCompat[] tb = { Min, Delta, Type };
			alHex16 = new ArrayList(tb);
            this.IsEnabled = false;
		}

		public void Dispose()
		{
		}


		#region TtabSingleMotiveUI
        private TtabItemSingleMotiveItem item = null;

		private ArrayList alHex16;
		private bool internalchg;

        public TtabItemSingleMotiveItem Motive
        {
            get { return item; }
            set
            {
                if (this.item != value)
                {
                    if (item != null && item.Wrapper != null)
                        item.Wrapper.WrapperChanged -= new System.EventHandler(this.WrapperChanged);
                    this.item = value;
                    setText();
                    if (item != null && item.Wrapper != null)
                        item.Wrapper.WrapperChanged += (s, e) => this.WrapperChanged(s, e);
                }
            }
        }

        private void WrapperChanged(object sender, System.EventArgs e)
        {
            if (internalchg || sender != item) return;
            setText();
        }

        private void setText()
        {
            bool prev = internalchg;
            internalchg = true;
            Min.Text = item == null ? "" : Helper.HexString(item.Min);
            Delta.Text = item == null ? "" : Helper.HexString(item.Delta);
            Type.Text = item == null ? "" : Helper.HexString(item.Type);
            internalchg = prev;
            this.IsEnabled = (item != null);
        }

		public void Clear()
		{
            bool prev = internalchg;
            internalchg = true;
			item.Min = item.Delta = item.Type = 0;
            setText();
            internalchg = prev;
        }

		private bool hex16_IsValid(object sender)
		{
			if (alHex16.IndexOf(sender) < 0)
				throw new Exception("hex16_IsValid not applicable to control " + sender.ToString());
			try { Convert.ToInt16(((TextBoxCompat)sender).Text, 16); }
			catch (Exception) { return false; }
			return true;
		}
		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.Min = new TextBoxCompat();
            this.Delta = new TextBoxCompat();
            this.Type = new TextBoxCompat();
            this.Min.Name = "Min";
            this.Min.LostFocus += (s, e) => hex16_Validated(s, e);
            this.Min.TextChanged += (s, e) => hex16_TextChanged(s, e);
            this.Delta.Name = "Delta";
            this.Delta.LostFocus += (s, e) => hex16_Validated(s, e);
            this.Delta.TextChanged += (s, e) => hex16_TextChanged(s, e);
            this.Type.Name = "Type";
            this.Type.LostFocus += (s, e) => hex16_Validated(s, e);
            this.Type.TextChanged += (s, e) => hex16_TextChanged(s, e);
		}
		#endregion

		private void hex16_TextChanged(object sender, System.EventArgs ev)
		{
			if (internalchg) return;
			if (!hex16_IsValid(sender)) return;

			internalchg = true;
            short val = Convert.ToInt16(((TextBoxCompat)sender).Text, 16);
            switch (alHex16.IndexOf(sender))
            {
                case 0: item.Min = val; break;
                case 1: item.Delta = val; break;
                case 2: item.Type = val; break;
            }
			internalchg = false;
		}

		private void hex16_Validating(object sender, System.EventArgs e)
		{
			if (hex16_IsValid(sender)) return;

            short val = 0;
            switch (alHex16.IndexOf(sender))
            {
                case 0: val = item.Min; break;
                case 1: val = item.Delta; break;
                case 2: val = item.Type; break;
            }
            internalchg = true;
            ((TextBoxCompat)sender).Text = Helper.HexString(val);
			((TextBoxCompat)sender).SelectAll();
			internalchg = false;
		}

		private void hex16_Validated(object sender, System.EventArgs ev)
		{
            internalchg = true;
            short val = Convert.ToInt16(((TextBoxCompat)sender).Text, 16);
            ((TextBoxCompat)sender).Text = Helper.HexString(val);
            ((TextBoxCompat)sender).SelectAll();
            internalchg = false;
        }

	}
}
