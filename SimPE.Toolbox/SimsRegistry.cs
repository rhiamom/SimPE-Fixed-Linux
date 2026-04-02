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
using CheckBox = Avalonia.Controls.CheckBox;

namespace SimPe.Plugin
{
	/// <summary>
	/// Registry Keys for the Object Workshop
	/// </summary>
	class SimsRegistry : System.IDisposable
	{
		XmlRegistryKey xrk;		
		Sims form;
		public SimsRegistry(Sims form)
		{
			this.form = form;
			xrk = Helper.XmlRegistry.PluginRegistryKey;

            form.ckbPlayable.IsChecked = this.ShowPlayable;
            form.ckbPlayable.IsCheckedChanged += (s,e) => ckbPlayable_CheckedChanged(s, EventArgs.Empty);

            form.cbTownie.IsChecked = this.ShowTownies;
            form.cbTownie.IsCheckedChanged += (s,e) => cbTownie_CheckedChanged(s, EventArgs.Empty);

            form.cbNpc.IsChecked = this.ShowNPCs;
			form.cbNpc.IsCheckedChanged += (s,e) => cbNpc_CheckedChanged(s, EventArgs.Empty);

            form.ckbUnEditable.IsChecked = this.ShowUnEditable;
            form.ckbUnEditable.IsCheckedChanged += (s,e) => ckbUnEditable_CheckedChanged(s, EventArgs.Empty);

            form.cbdetail.IsChecked = this.ShowDetails;
            form.cbdetail.IsCheckedChanged += (s,e) => cbdetail_CheckedChanged(s, EventArgs.Empty);

            form.cbgals.IsChecked = this.JustGals;
            form.cbgals.IsCheckedChanged += (s,e) => cbgals_CheckedChanged(s, EventArgs.Empty);
            form.cbmens.IsEnabled = form.cbgals.IsChecked != true;

            form.cbadults.IsChecked = this.AdultsOnly;
            form.cbadults.IsCheckedChanged += (s,e) => cbadults_CheckedChanged(s, EventArgs.Empty);

			form.sorter.CurrentColumn = this.SortedColumn;
			form.sorter.Sorting = this.SortOrder;
			form.sorter.Changed += new EventHandler(sorter_Changed);
		}

		#region Properties
        public bool ShowPlayable
        {
            get
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                object o = rkf.GetValue("ShowPlayable", true);
                return Convert.ToBoolean(o);
            }
            set
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                rkf.SetValue("ShowPlayable", value);
            }
        }

        public bool ShowTownies
        {
            get
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                object o = rkf.GetValue("ShowTownies", false);
                return Convert.ToBoolean(o);
            }
            set
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                rkf.SetValue("ShowTownies", value);
            }
        }

        public bool ShowNPCs
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				object o = rkf.GetValue("ShowNPCs", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				rkf.SetValue("ShowNPCs", value);
			}
		}

        public bool ShowUnEditable
        {
            get
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                object o = rkf.GetValue("ShowUnEditable", false);
                return Convert.ToBoolean(o);
            }
            set
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                rkf.SetValue("ShowUnEditable", value);
            }
        }

        public bool ShowDetails
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				object o = rkf.GetValue("ShowDetails", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				rkf.SetValue("ShowDetails", value);
			}
        }

        public bool JustGals
        {
            get
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                object o = rkf.GetValue("JustGals", false);
                return Convert.ToBoolean(o);
            }
            set
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                rkf.SetValue("JustGals", value);
            }
        }

        public bool AdultsOnly
        {
            get
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                object o = rkf.GetValue("AdultsOnly", false);
                return Convert.ToBoolean(o);
            }
            set
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                rkf.SetValue("AdultsOnly", value);
            }
        }

		public int SortedColumn
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				object o = rkf.GetValue("SortedColumn", 3);
				return Convert.ToInt32(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				rkf.SetValue("SortedColumn", value);
			}
		}

		public SimPe.SortOrder SortOrder
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				object o = rkf.GetValue("SortOrder", (int)SimPe.SortOrder.Ascending);
				return (SimPe.SortOrder)Convert.ToInt32(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				rkf.SetValue("SortOrder", (int)value);
			}
		}

		#endregion
		

		#region IDisposable Member

		public void Dispose()
		{
			

			form = null;
			xrk = null;
		}

		#endregion

        private void ckbPlayable_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            this.ShowPlayable = cb?.IsChecked == true;
        }

        private void cbTownie_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            this.ShowTownies = cb?.IsChecked == true;
        }

        private void cbNpc_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            this.ShowNPCs = cb?.IsChecked == true;
        }

        private void ckbUnEditable_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox cb = sender as CheckBox;
            this.ShowUnEditable = cb?.IsChecked == true;
		}

		private void cbdetail_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			this.ShowDetails = cb?.IsChecked == true;
        }

        private void cbgals_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            this.JustGals = cb?.IsChecked == true;
        }

        private void cbadults_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            this.AdultsOnly = cb?.IsChecked == true;
        }

		private void sorter_Changed(object sender, EventArgs e)
		{
			SimPe.ColumnSorter cs = sender as SimPe.ColumnSorter;
			this.SortedColumn = cs.CurrentColumn;
			this.SortOrder = cs.Sorting;
		}
	}
}
