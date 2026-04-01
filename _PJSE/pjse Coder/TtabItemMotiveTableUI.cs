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
	/// Summary description for TtabItemMotiveTableUI.
	/// </summary>
	public class TtabItemMotiveTableUI : Avalonia.Controls.UserControl
	{
		#region Form variables

        private LabelCompat lbMotive0;
		private LabelCompat lbMotive1;
		private LabelCompat lbMotive2;
		private LabelCompat lbMotive3;
		private LabelCompat lbMotive4;
		private LabelCompat lbMotive5;
		private LabelCompat lbMotive6;
		private LabelCompat lbMotive7;
		private LabelCompat lbMotive9;
		private LabelCompat lbMotive11;
		private LabelCompat lbMotive8;
		private LabelCompat lbMotive10;
		private LabelCompat lbMotive14;
		private LabelCompat lbMotive15;
		private LabelCompat lbMotive13;
		private LabelCompat lbMotive12;
        private StackPanelCompat pnAllGroups;
		private CheckBoxCompat2 cbShowAll;
		private StackPanelCompat pnCopyButtons;
		private ButtonCompat btnCpyM0;
		private ButtonCompat btnCpyM1;
		private ButtonCompat btnCpyM2;
		private ButtonCompat btnCpyM3;
		private ButtonCompat btnCpyM4;
		private ButtonCompat btnCpyM5;
		private ButtonCompat btnCpyM7;
		private ButtonCompat btnCpyM6;
		private ButtonCompat btnCpyM9;
		private ButtonCompat btnCpyM12;
		private ButtonCompat btnCpyM11;
		private ButtonCompat btnCpyM10;
		private ButtonCompat btnCpyM15;
		private ButtonCompat btnCpyM14;
		private ButtonCompat btnCpyM13;
		private ButtonCompat btnCpyM8;
		private LabelCompat lbCBM0;
		private LabelCompat lbCBM1;
		private LabelCompat lbCBM2;
		private LabelCompat lbCBM3;
		private LabelCompat lbCBM4;
		private LabelCompat lbCBM5;
		private LabelCompat lbCBM6;
		private LabelCompat lbCBM7;
		private LabelCompat lbCBM15;
		private LabelCompat lbCBM11;
		private LabelCompat lbCBM14;
		private LabelCompat lbCBM8;
		private LabelCompat lbCBM9;
		private LabelCompat lbCBM13;
		private LabelCompat lbCBM10;
		private LabelCompat lbCBM12;
		private ButtonCompat btnCopyAll;
        private LabelCompat lbNrGroups;
		/// <summary>
		/// Required designer variable.
		/// </summary>
				#endregion

		public TtabItemMotiveTableUI()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
            pnCopyButtons.IsVisible = pnAllGroups.IsVisible = false;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose() { }

		#region TtabItemMotiveTableUI
        private TtabItemMotiveTable item = null;

		private ButtonCompat[] aButtons;
        private int maxWidth = 0;

        public TtabItemMotiveTable MotiveTable
        {
            get { return item; }
            set
            {
                if (this.item != value)
                {
                    if (item != null && item.Wrapper != null)
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
            cbShowAll.IsEnabled = false;
            this.pnAllGroups.Children.Clear();

            if (item != null && item.Count > 0)
            {
                this.lbNrGroups.Content = (this.lbNrGroups.Text.Split(new char[]{':'})[0]) + ": " + item.Count.ToString();
                TtabMotiveGroupUI c = new TtabMotiveGroupUI();
                c.MotiveGroup = item[0];
                if (item.Type == TtabItemMotiveTableType.Human)
                    c.MGName = pjse.BhavWiz.readStr(pjse.GS.BhavStr.TTABAges, 0);
                else
                    c.MGName = "[0]";
                setLocations(c);

                if (item.Count > 1)
                {
                    cbShowAll.IsEnabled = true;
                    int nextLeft = 0;
                    for (int i = 1; i < item.Count; i++)
                    {
                        c = new TtabMotiveGroupUI();
                        this.pnAllGroups.Children.Add(c);
                        c.MotiveGroup = item[i];
                        if (item.Type == TtabItemMotiveTableType.Human)
                            c.MGName = pjse.BhavWiz.readStr(pjse.GS.BhavStr.TTABAges, (ushort)i);
                        else
                            c.MGName = "[" + i.ToString() + "]";
                        c.Location = new Point(nextLeft, 0);
                        nextLeft += (int)c.Width + 2;
                    }
                }
            }
            else
                this.lbNrGroups.Content = (this.lbNrGroups.Text.Split(new char[]{':'})[0]) + ": 0";
            cbShowAll_CheckedChanged(null, null);
        }

        private void setLocations(TtabMotiveGroupUI c)
        {
            ButtonCompat[] b = {
							 btnCpyM0  ,btnCpyM1  ,btnCpyM2  ,btnCpyM3  ,btnCpyM4  ,btnCpyM5  ,btnCpyM6  ,btnCpyM7
							,btnCpyM8  ,btnCpyM9  ,btnCpyM10 ,btnCpyM11 ,btnCpyM12 ,btnCpyM13 ,btnCpyM14 ,btnCpyM15
							};
            aButtons = b;

            LabelCompat[] lbCBM = {
                lbCBM0, lbCBM1, lbCBM2, lbCBM3, lbCBM4, lbCBM5, lbCBM6, lbCBM7
                ,lbCBM8, lbCBM9, lbCBM10, lbCBM11, lbCBM12, lbCBM13, lbCBM14, lbCBM15
            };

            LabelCompat[] aMotiveLabels = {
				lbMotive0 ,lbMotive1 ,lbMotive2  ,lbMotive3  ,lbMotive4  ,lbMotive5  ,lbMotive6  ,lbMotive7
				,lbMotive8 ,lbMotive9 ,lbMotive10 ,lbMotive11 ,lbMotive12 ,lbMotive13 ,lbMotive14 ,lbMotive15
			};

            this.pnAllGroups.Children.Add(c);

            maxWidth = this.lbNrGroups.Width;

            int cbW = 0;
            for (ushort m = 0; m < aMotiveLabels.Length; m++)
            {
                aMotiveLabels[m].Content = pjse.BhavWiz.readStr(pjse.GS.BhavStr.Motives, m);
                if (aMotiveLabels[m].Width > maxWidth) maxWidth = aMotiveLabels[m].Width;
                cbW = b[m].Width;
            }

            for (ushort m = 0; m < aMotiveLabels.Length; m++)
            {
                aMotiveLabels[m].Location = new Point(maxWidth - aMotiveLabels[m].Width, c.Tops[m] + 2);
                aButtons[m].Location = new Point(0, c.Tops[m]);
                lbCBM[m].Location = new Point(cbW + 2, c.Tops[m] + 2);
            }

            this.cbShowAll.Location = new Point(maxWidth - this.cbShowAll.Width, 2);

            c.Location = new Point(maxWidth + 2, 0);
            this.Height = c.Height + 24;

            this.btnCopyAll.Location = new Point(0, c.Tops[15] + c.Tops[1] - c.Tops[0]);
            this.lbNrGroups.Location = new Point(4, this.btnCopyAll.Top + 2);

            this.pnCopyButtons.Location = new Point(c.Right + 2, 0);
            this.pnCopyButtons.Size = new Size(lbCBM0.Right + 4, (int)this.Height);
            this.pnAllGroups.Location = new Point(c.Right + 2, 0);
            this.pnAllGroups.Size = new Size((int)this.Width - this.pnAllGroups.Left, c.Bottom + 24);
        }

        private void doCopyMotive(int m)
        {
            for (int i = 1; i < item.Count; i++)
                item[0][m].CopyTo(item[i][m]);
        }

		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TtabItemMotiveTableUI));
            this.lbMotive0 = new LabelCompat();
            this.lbMotive1 = new LabelCompat();
            this.lbMotive2 = new LabelCompat();
            this.lbMotive3 = new LabelCompat();
            this.lbMotive4 = new LabelCompat();
            this.lbMotive5 = new LabelCompat();
            this.lbMotive6 = new LabelCompat();
            this.lbMotive7 = new LabelCompat();
            this.lbMotive9 = new LabelCompat();
            this.lbMotive11 = new LabelCompat();
            this.lbMotive8 = new LabelCompat();
            this.lbMotive10 = new LabelCompat();
            this.lbMotive14 = new LabelCompat();
            this.lbMotive15 = new LabelCompat();
            this.lbMotive13 = new LabelCompat();
            this.lbMotive12 = new LabelCompat();
            this.pnAllGroups = new StackPanelCompat();
            this.cbShowAll = new CheckBoxCompat2();
            this.pnCopyButtons = new StackPanelCompat();
            this.btnCopyAll = new ButtonCompat();
            this.lbCBM0 = new LabelCompat();
            this.btnCpyM0 = new ButtonCompat();
            this.btnCpyM1 = new ButtonCompat();
            this.btnCpyM2 = new ButtonCompat();
            this.btnCpyM3 = new ButtonCompat();
            this.btnCpyM4 = new ButtonCompat();
            this.btnCpyM5 = new ButtonCompat();
            this.btnCpyM7 = new ButtonCompat();
            this.btnCpyM6 = new ButtonCompat();
            this.btnCpyM9 = new ButtonCompat();
            this.btnCpyM12 = new ButtonCompat();
            this.btnCpyM11 = new ButtonCompat();
            this.btnCpyM10 = new ButtonCompat();
            this.btnCpyM15 = new ButtonCompat();
            this.btnCpyM14 = new ButtonCompat();
            this.btnCpyM13 = new ButtonCompat();
            this.btnCpyM8 = new ButtonCompat();
            this.lbCBM1 = new LabelCompat();
            this.lbCBM2 = new LabelCompat();
            this.lbCBM3 = new LabelCompat();
            this.lbCBM4 = new LabelCompat();
            this.lbCBM5 = new LabelCompat();
            this.lbCBM6 = new LabelCompat();
            this.lbCBM7 = new LabelCompat();
            this.lbCBM15 = new LabelCompat();
            this.lbCBM11 = new LabelCompat();
            this.lbCBM14 = new LabelCompat();
            this.lbCBM8 = new LabelCompat();
            this.lbCBM9 = new LabelCompat();
            this.lbCBM13 = new LabelCompat();
            this.lbCBM10 = new LabelCompat();
            this.lbCBM12 = new LabelCompat();
            this.lbNrGroups = new LabelCompat();
            // lbMotive0
            //            this.lbMotive0.Name = "lbMotive0";
            // 
            // lbMotive1
            //            this.lbMotive1.Name = "lbMotive1";
            // 
            // lbMotive2
            //            this.lbMotive2.Name = "lbMotive2";
            // 
            // lbMotive3
            //            this.lbMotive3.Name = "lbMotive3";
            // 
            // lbMotive4
            //            this.lbMotive4.Name = "lbMotive4";
            // 
            // lbMotive5
            //            this.lbMotive5.Name = "lbMotive5";
            // 
            // lbMotive6
            //            this.lbMotive6.Name = "lbMotive6";
            // 
            // lbMotive7
            //            this.lbMotive7.Name = "lbMotive7";
            // 
            // lbMotive9
            //            this.lbMotive9.Name = "lbMotive9";
            // 
            // lbMotive11
            //            this.lbMotive11.Name = "lbMotive11";
            // 
            // lbMotive8
            //            this.lbMotive8.Name = "lbMotive8";
            // 
            // lbMotive10
            //            this.lbMotive10.Name = "lbMotive10";
            // 
            // lbMotive14
            //            this.lbMotive14.Name = "lbMotive14";
            // 
            // lbMotive15
            //            this.lbMotive15.Name = "lbMotive15";
            // 
            // lbMotive13
            //            this.lbMotive13.Name = "lbMotive13";
            // 
            // lbMotive12
            //            this.lbMotive12.Name = "lbMotive12";
            // 
            // pnAllGroups
            //            this.pnAllGroups.Name = "pnAllGroups";
            // 
            // cbShowAll
            //            this.cbShowAll.Name = "cbShowAll";
            this.cbShowAll.IsCheckedChanged += (s, e) => this.cbShowAll_CheckedChanged(s, e);
            // 
            // pnCopyButtons
            // 
            this.pnCopyButtons.Children.Add(this.btnCopyAll);
            this.pnCopyButtons.Children.Add(this.lbCBM0);
            this.pnCopyButtons.Children.Add(this.btnCpyM0);
            this.pnCopyButtons.Children.Add(this.btnCpyM1);
            this.pnCopyButtons.Children.Add(this.btnCpyM2);
            this.pnCopyButtons.Children.Add(this.btnCpyM3);
            this.pnCopyButtons.Children.Add(this.btnCpyM4);
            this.pnCopyButtons.Children.Add(this.btnCpyM5);
            this.pnCopyButtons.Children.Add(this.btnCpyM7);
            this.pnCopyButtons.Children.Add(this.btnCpyM6);
            this.pnCopyButtons.Children.Add(this.btnCpyM9);
            this.pnCopyButtons.Children.Add(this.btnCpyM12);
            this.pnCopyButtons.Children.Add(this.btnCpyM11);
            this.pnCopyButtons.Children.Add(this.btnCpyM10);
            this.pnCopyButtons.Children.Add(this.btnCpyM15);
            this.pnCopyButtons.Children.Add(this.btnCpyM14);
            this.pnCopyButtons.Children.Add(this.btnCpyM13);
            this.pnCopyButtons.Children.Add(this.btnCpyM8);
            this.pnCopyButtons.Children.Add(this.lbCBM1);
            this.pnCopyButtons.Children.Add(this.lbCBM2);
            this.pnCopyButtons.Children.Add(this.lbCBM3);
            this.pnCopyButtons.Children.Add(this.lbCBM4);
            this.pnCopyButtons.Children.Add(this.lbCBM5);
            this.pnCopyButtons.Children.Add(this.lbCBM6);
            this.pnCopyButtons.Children.Add(this.lbCBM7);
            this.pnCopyButtons.Children.Add(this.lbCBM15);
            this.pnCopyButtons.Children.Add(this.lbCBM11);
            this.pnCopyButtons.Children.Add(this.lbCBM14);
            this.pnCopyButtons.Children.Add(this.lbCBM8);
            this.pnCopyButtons.Children.Add(this.lbCBM9);
            this.pnCopyButtons.Children.Add(this.lbCBM13);
            this.pnCopyButtons.Children.Add(this.lbCBM10);
            this.pnCopyButtons.Children.Add(this.lbCBM12);            this.pnCopyButtons.Name = "pnCopyButtons";
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.Click += (s, e) => this.copy_Click(s, e);
            // 
            // lbCBM0
            //            this.lbCBM0.Name = "lbCBM0";
            // 
            // btnCpyM0
            // 
            this.btnCpyM0.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM1
            // 
            this.btnCpyM1.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM2
            // 
            this.btnCpyM2.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM3
            // 
            this.btnCpyM3.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM4
            // 
            this.btnCpyM4.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM5
            // 
            this.btnCpyM5.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM7
            // 
            this.btnCpyM7.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM6
            // 
            this.btnCpyM6.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM9
            // 
            this.btnCpyM9.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM12
            // 
            this.btnCpyM12.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM11
            // 
            this.btnCpyM11.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM10
            // 
            this.btnCpyM10.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM15
            // 
            this.btnCpyM15.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM14
            // 
            this.btnCpyM14.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM13
            // 
            this.btnCpyM13.Click += (s, e) => this.copy_Click(s, e);
            // 
            // btnCpyM8
            // 
            this.btnCpyM8.Click += (s, e) => this.copy_Click(s, e);
            // 
            // lbCBM1
            //            this.lbCBM1.Name = "lbCBM1";
            // 
            // lbCBM2
            //            this.lbCBM2.Name = "lbCBM2";
            // 
            // lbCBM3
            //            this.lbCBM3.Name = "lbCBM3";
            // 
            // lbCBM4
            //            this.lbCBM4.Name = "lbCBM4";
            // 
            // lbCBM5
            //            this.lbCBM5.Name = "lbCBM5";
            // 
            // lbCBM6
            //            this.lbCBM6.Name = "lbCBM6";
            // 
            // lbCBM7
            //            this.lbCBM7.Name = "lbCBM7";
            // 
            // lbCBM15
            //            this.lbCBM15.Name = "lbCBM15";
            // 
            // lbCBM11
            //            this.lbCBM11.Name = "lbCBM11";
            // 
            // lbCBM14
            //            this.lbCBM14.Name = "lbCBM14";
            // 
            // lbCBM8
            //            this.lbCBM8.Name = "lbCBM8";
            // 
            // lbCBM9
            //            this.lbCBM9.Name = "lbCBM9";
            // 
            // lbCBM13
            //            this.lbCBM13.Name = "lbCBM13";
            // 
            // lbCBM10
            //            this.lbCBM10.Name = "lbCBM10";
            // 
            // lbCBM12
            //            this.lbCBM12.Name = "lbCBM12";
            // 
            // lbNrGroups
            //            this.lbNrGroups.Name = "lbNrGroups";
            // 
            // TtabItemMotiveTableUI
            // 
            var _rootCanvas = new Canvas();
            _rootCanvas.Children.Add(this.lbNrGroups);
            _rootCanvas.Children.Add(this.pnCopyButtons);
            _rootCanvas.Children.Add(this.cbShowAll);
            _rootCanvas.Children.Add(this.pnAllGroups);
            _rootCanvas.Children.Add(this.lbMotive0);
            _rootCanvas.Children.Add(this.lbMotive1);
            _rootCanvas.Children.Add(this.lbMotive2);
            _rootCanvas.Children.Add(this.lbMotive3);
            _rootCanvas.Children.Add(this.lbMotive4);
            _rootCanvas.Children.Add(this.lbMotive5);
            _rootCanvas.Children.Add(this.lbMotive6);
            _rootCanvas.Children.Add(this.lbMotive7);
            _rootCanvas.Children.Add(this.lbMotive9);
            _rootCanvas.Children.Add(this.lbMotive11);
            _rootCanvas.Children.Add(this.lbMotive8);
            _rootCanvas.Children.Add(this.lbMotive10);
            _rootCanvas.Children.Add(this.lbMotive14);
            _rootCanvas.Children.Add(this.lbMotive15);
            _rootCanvas.Children.Add(this.lbMotive13);
            _rootCanvas.Children.Add(this.lbMotive12);
            this.Name = "TtabItemMotiveTableUI";
            this.Content = _rootCanvas;
		}
		#endregion

		private void copy_Click(object sender, System.EventArgs e)
		{
			ArrayList alBtnCopy = new ArrayList(aButtons);
			int bn = alBtnCopy.IndexOf(sender);
			if (bn >= 0)
				doCopyMotive(bn);
			else
                for (int i = 0; i < aButtons.Length; i++)
					doCopyMotive(i);
        }

		private void cbShowAll_CheckedChanged(object sender, System.EventArgs e)
		{
            pnAllGroups.IsVisible = cbShowAll.IsEnabled && cbShowAll.IsChecked == true;
            pnCopyButtons.IsVisible = cbShowAll.IsEnabled && !cbShowAll.IsChecked == true;
        }
	}
}
