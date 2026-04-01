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
	/// Summary description for TtabMotiveGroupUI.
	/// </summary>
	public class TtabMotiveGroupUI : Avalonia.Controls.UserControl
	{
        // WinForms layout compat
        public System.Drawing.Point Location { get; set; }
        public new System.Drawing.Size Size { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

		#region Form variables
		private GroupBox gbMotiveGroup;
		private LabelCompat lbMin;
		private LabelCompat lbDelta;
        private LabelCompat lbType;
        private ButtonCompat btnClear;
		#endregion

		public TtabMotiveGroupUI()
		{
			InitializeComponent();
        }

		public void Dispose()
		{
        }


        #region Extra attributes
        private TtabItemMotiveGroup item = null;
        public String MGName
        {
            get { return this.gbMotiveGroup.Text; }
            set { this.gbMotiveGroup.Text = value; }
        }


        private ArrayList tops = new ArrayList();
        public int[] Tops { get { return (int[])tops.ToArray(typeof(Int32)); } }
        #endregion

        #region TtabMotiveGroupUI

        public TtabItemMotiveGroup MotiveGroup
		{
            get { return item; }
			set
			{
                if (this.item != value)
                {
                    if (item != null)
                        item.Wrapper.WrapperChanged -= new System.EventHandler(this.WrapperChanged);
                    this.item = value;
                    setData();
                    if (item != null)
                        item.Wrapper.WrapperChanged += (s, e) => this.WrapperChanged(s, e);
                }
            }
		}

        private void WrapperChanged(object sender, System.EventArgs e)
        {
            if (sender != item) return;
            setData();
        }


        private void setData()
        {
            this.gbMotiveGroup.Controls.Clear();
            tops = new ArrayList();

            if (item != null)
            {
                if (item.Parent.Type == TtabItemMotiveTableType.Human)
                {
                    this.gbMotiveGroup.Controls.Add(this.lbMin);
                    this.gbMotiveGroup.Controls.Add(this.lbDelta);
                    this.gbMotiveGroup.Controls.Add(this.lbType);

                    for (int i = 0; i < item.Count; i++)
                    {
                        TtabSingleMotiveUI c = new TtabSingleMotiveUI();
                        c.Motive = (TtabItemSingleMotiveItem)item[i];
                        this.gbMotiveGroup.Controls.Add(c);
                        tops.Add(i * 32);
                    }
                }
                else
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        TtabAnimalMotiveUI c = new TtabAnimalMotiveUI();
                        c.Motive = (TtabItemAnimalMotiveItem)item[i];
                        this.gbMotiveGroup.Controls.Add(c);
                        tops.Add(i * 32);
                    }
                }
            }

            this.gbMotiveGroup.Controls.Add(this.btnClear);
        }

		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.gbMotiveGroup = new GroupBox();
            this.btnClear = new ButtonCompat { Content = "Clear" };
            this.lbMin = new LabelCompat { Content = "Min" };
            this.lbDelta = new LabelCompat { Content = "Delta" };
            this.lbType = new LabelCompat { Content = "Type" };
            this.gbMotiveGroup.Name = "gbMotiveGroup";
            this.btnClear.Name = "btnClear";
            this.btnClear.Click += (s, e) => btnClear_Click(s, e);
		}
		#endregion

		private void btnClear_Click(object sender, System.EventArgs e)
		{
            item.Clear();
        }

	}

	#region MotiveClickEvent
	public class MotiveClickEventArgs : System.EventArgs
	{
		private int motive;
		public MotiveClickEventArgs(int m) : base()  { motive = m; }
		public int Motive { get { return motive; } }
	}
	public delegate void MotiveClickEventHandler(object sender, MotiveClickEventArgs e);
	#endregion
}
