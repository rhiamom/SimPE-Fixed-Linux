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
using Avalonia.Controls;
using SimPe.Interfaces.Plugin;
using SimPe.Interfaces;
using SimPe.PackedFiles.Wrapper.Supporting;
using SimPe.Data;
using Ambertation.Windows.Forms;
using SimPe.Windows.Forms;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.Plugin
{
	/// <summary>
	/// Zusammenfassung f�r ExtNgbhUI.
	/// </summary>
	public class BnfoUI : 
		//System.Windows.Forms.UserControl
		SimPe.Windows.Forms.WrapperBaseControl, SimPe.Interfaces.Plugin.IPackedFileUI
	{
		private SimPe.Plugin.BnfoCustomerItemsUI lv;
		private SimPe.Plugin.BnfoCustomerItemUI bnfoCustomerItemUI1;
		private Avalonia.Controls.TextBlock label1;
		private Avalonia.Controls.TextBlock lblot;
		private Avalonia.Controls.StackPanel toolBar1;
		private Avalonia.Controls.Panel panel1;
		private Avalonia.Controls.Button biMax;
		private Avalonia.Controls.Button biReward;
		private Avalonia.Controls.TextBlock label2;
		private Avalonia.Controls.TextBlock label3;
		private Avalonia.Controls.TextBox tbCur;
		private Avalonia.Controls.TextBox tbMax;


        public BnfoUI()
        {
            InitializeComponent();

            ThemeManager.Global.AddControl(this.toolBar1);
        }

		private void InitializeComponent()
		{
            this.lv = new SimPe.Plugin.BnfoCustomerItemsUI();
            this.bnfoCustomerItemUI1 = new SimPe.Plugin.BnfoCustomerItemUI();
            this.label1 = new Avalonia.Controls.TextBlock { Text = "Lot:" };
            this.lblot = new Avalonia.Controls.TextBlock();
            this.toolBar1 = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            this.biMax = new Avalonia.Controls.Button();
            this.biReward = new Avalonia.Controls.Button();
            this.panel1 = new Avalonia.Controls.Panel();
            this.tbMax = new Avalonia.Controls.TextBox();
            this.tbCur = new Avalonia.Controls.TextBox();
            this.label3 = new Avalonia.Controls.TextBlock { Text = "Rewarded Level:" };
            this.label2 = new Avalonia.Controls.TextBlock { Text = "Current Level:" };

            this.bnfoCustomerItemUI1.BnfoCustomerItemsUI = this.lv;
            this.bnfoCustomerItemUI1.Item = null;

            this.biMax.Content = "Maximize";
            this.biMax.Click += (s, e) => biMax_Activate(s, EventArgs.Empty);

            this.biReward.Content = "Reward again";
            this.biReward.Click += (s, e) => biReward_Activate(s, EventArgs.Empty);

            this.tbMax.TextChanged += (s, e) => tbMax_TextChanged(s, EventArgs.Empty);
            this.tbCur.TextChanged += (s, e) => tbCur_TextChanged(s, EventArgs.Empty);

            this.HeaderText = "Business Info";
		}


		public Bnfo Bnfo
		{
			get { return (Bnfo)Wrapper; }
		}

		bool intern;
		public override void RefreshGUI()
		{			
			if (intern) return;
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BnfoUI));
			this.HeaderText = resources.GetString("$this.HeaderText");
			intern = true;
			if (Bnfo!=null) 
			{
				lv.Items = Bnfo.CustomerItems;
				biMax.IsEnabled = true;
				biReward.IsEnabled = true;

				SimPe.Interfaces.Providers.ILotItem ili = FileTable.ProviderRegistry.LotProvider.FindLot(Bnfo.FileDescriptor.Instance);
				if (ili!=null)
					this.lblot.Text = ili.LotName;
				else
					this.lblot.Text = SimPe.Localization.GetString("Unknown");

				tbCur.Text = Bnfo.CurrentBusinessState.ToString();
				tbMax.Text = Bnfo.MaxSeenBusinessState.ToString();

				this.HeaderText += ": " + lblot.Text;
			} 
			else 
			{
				lv.Items = null;
				this.lblot.Text = "";
				
				biMax.IsEnabled = false;
				biReward.IsEnabled = false;				
			}

			tbMax.IsEnabled = biMax.IsEnabled;
			tbCur.IsEnabled = biMax.IsEnabled;
			intern=false;
		}

		public override void OnCommit()
		{
			Bnfo.SynchronizeUserData(true, false);
		}


		private void biMax_Activate(object sender, System.EventArgs e)
		{
			if (lv.Items==null) return;
			foreach (BnfoCustomerItem item in lv.Items)			
				item.LoyaltyScore = 1000;
						
			lv.Refresh();
		}

		private void biReward_Activate(object sender, System.EventArgs e)
		{
			if (Bnfo==null) return;
			Bnfo.CurrentBusinessState = 0;
			Bnfo.MaxSeenBusinessState = 0;
			RefreshGUI();
		}

		private void tbCur_TextChanged(object sender, System.EventArgs e)
		{
			if (intern) return;
			if (Bnfo==null) return;
			Bnfo.CurrentBusinessState = Helper.StringToUInt32(tbCur.Text, Bnfo.CurrentBusinessState, 10);
		}

		private void tbMax_TextChanged(object sender, System.EventArgs e)
		{
			if (intern) return;
			if (Bnfo==null) return;
			Bnfo.MaxSeenBusinessState = Helper.StringToUInt32(tbMax.Text, Bnfo.MaxSeenBusinessState, 10);
		}

								
	}
}
