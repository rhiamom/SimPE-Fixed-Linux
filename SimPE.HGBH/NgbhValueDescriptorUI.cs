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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Ambertation.Windows.Forms;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for NgbhValueDescriptorUI.
	/// </summary>
	[System.ComponentModel.DefaultEvent("AddedNewItem")]
	public class NgbhValueDescriptorUI : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NgbhValueDescriptorUI()
		{
			SetStyle(
				ControlStyles.SupportsTransparentBackColor |
				ControlStyles.AllPaintingInWmPaint |
				//ControlStyles.Opaque |
				ControlStyles.UserPaint |
				ControlStyles.ResizeRedraw 
				| ControlStyles.DoubleBuffer
				,true);
			// Required designer variable.
			InitializeComponent();

			SetContent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
	private void InitializeComponent()
	{
		// Avalonia port: instantiate controls and wire events only
		this.panel1 = new System.Windows.Forms.Panel();
		this.pb = new LabeledProgressBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.cb = new System.Windows.Forms.CheckBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.lb = new System.Windows.Forms.Label();
		this.ll = new System.Windows.Forms.LinkLabel();

		// pb setup (LabeledProgressBar - Avalonia UserControl)
		this.pb.DisplayOffset = 0;
		this.pb.LabelText = "";
		this.pb.Maximum = 100;
		this.pb.Name = "pb";
		this.pb.NumberFormat = "N0";
		this.pb.NumberOffset = 0;
		this.pb.NumberScale = 1;
		this.pb.SelectedColor = System.Drawing.Color.YellowGreen;
		this.pb.TokenCount = 10;
		this.pb.UnselectedColor = System.Drawing.Color.Black;
		this.pb.Value = 0;
		this.pb.Changed += new System.EventHandler(this.pb_Changed);
		// pb.Resize and pb.HandleCreated not available on Avalonia UserControl

		// cb setup (CheckBox - WinForms stub)
		this.cb.Name = "cb";
		this.cb.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);

		// lb setup (Label - WinForms stub)
		this.lb.Name = "lb";

		// ll setup (LinkLabel - WinForms stub)
		this.ll.Name = "ll";
		this.ll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll_LinkClicked);

		// panel1 contains pb
		this.panel1.Controls.Add(this.pb);
		this.panel1.Name = "panel1";

		// panel2 contains cb
		this.panel2.Controls.Add(this.cb);
		this.panel2.Name = "panel2";

		// panel3 contains lb and ll
		this.panel3.Controls.Add(this.lb);
		this.panel3.Controls.Add(this.ll);
		this.panel3.Name = "panel3";

		// this (NgbhValueDescriptorUI) setup
		this.Controls.Add(this.panel3);
		this.Controls.Add(this.panel2);
		this.Controls.Add(this.panel1);
		this.Name = "NgbhValueDescriptorUI";
	}
		#endregion

		NgbhSlot slot;
		[System.ComponentModel.Browsable(false)]
		public NgbhSlot Slot
		{
			get {return slot;}
			set 
			{
				slot = value;
				SetContent();				
			}
		}

		NgbhValueDescriptor des;
		private System.Windows.Forms.Panel panel1;
        private LabeledProgressBar pb;
		private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox cb;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.LinkLabel ll;
		private System.Windows.Forms.Label lb;
		
				
		[System.ComponentModel.Browsable(false)]
		public NgbhValueDescriptor NgbhValueDescriptor
		{
			get {return des;}
			set 
			{
				des = value;
				SetContent();				
			}
		}

		NgbhValueDescriptorSelection vds;
		public NgbhValueDescriptorSelection NgbhValueDescriptorSelection
		{
			get {return vds;}
			set 
			{
				if (vds!=null) vds.SelectedDescriptorChanged -= new EventHandler(vds_SelectedDescriptorChanged);
				vds = value;
				if (vds!=null) vds.SelectedDescriptorChanged += new EventHandler(vds_SelectedDescriptorChanged);
			}
		}

		void SetVisible()
		{
			panel1.Visible = item!=null;
			if (des!=null)
				panel2.Visible = des.HasComplededFlag && item!=null;
			else
				panel2.Visible = false;
			
			panel3.Visible = des!=null && item==null;
		}

		NgbhItem item;
		bool inter;
		void SetContent()
		{
			if (inter) return;
			inter = true;
			if (des!=null && slot!=null)
			{
				item = slot.FindItem(des.Guid);
				pb.NumberOffset = des.Minimum;
				pb.Maximum = des.Maximum;				
				
				if (item!=null) 			
				{	
					pb.Value = item.GetValue(des.DataNumber);
					if (des.HasComplededFlag)
						cb.Checked = item.GetValue(des.CompletedDataNumber)!=0;
				}
				else
					lb.Text = des.ToString();

				this.Enabled = true;
			} 	
			else 
			{
				this.Enabled = false;
			}

			SetVisible();
			inter = false;
		}

		private void vds_SelectedDescriptorChanged(object sender, EventArgs e)
		{
			this.NgbhValueDescriptor = vds.SelectedDescriptor;
		}

		private void pb_Resize(object sender, System.EventArgs e)
		{
			
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			pb.TokenCount = 10;			
		}

		public event EventHandler AddedNewItem;
		public event EventHandler ChangedItem;

		private void ll_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			if (item!=null) return;
			if (slot==null) return;
			if (des==null) return;
			
			if (des.Intern) item = slot.ItemsA.AddNew(SimMemoryType.Skill);
			else item = slot.ItemsB.AddNew(SimMemoryType.Skill);

			item.Guid = des.Guid;
			item.PutValue(des.DataNumber, 0);
			if (des.HasComplededFlag) item.PutValue(des.CompletedDataNumber, 0);
								
			SetContent();

			if (AddedNewItem!=null) AddedNewItem(this, new EventArgs());
		}

		private void cb_CheckedChanged(object sender, System.EventArgs e)
		{
			if (inter) return;
			if (item==null) return;
			if (des==null) return;
			if (!des.HasComplededFlag) return;

			if (cb.Checked) item.PutValue(des.CompletedDataNumber, 1);
			else item.PutValue(des.CompletedDataNumber, 0);

			if (ChangedItem!=null) ChangedItem(this, new EventArgs());
		}

		private void pb_Changed(object sender, System.EventArgs e)
		{
			if (inter) return;
			if (item==null) return;
			if (des==null) return;			

			item.PutValue(des.DataNumber, (ushort)pb.Value);
			
			if (ChangedItem!=null) ChangedItem(this, new EventArgs());
		}

		private void pb_Load(object sender, System.EventArgs e)
		{		
		}
	}
}
