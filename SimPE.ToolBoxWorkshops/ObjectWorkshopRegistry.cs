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

namespace SimPe.Plugin.Tool.Dockable
{
	/// <summary>
	/// Registry Keys for the Object Workshop
	/// </summary>
	class ObjectWorkshopRegistry : System.IDisposable
	{
		XmlRegistryKey xrk;		
		dcObjectWorkshop dock;
		public ObjectWorkshopRegistry(dcObjectWorkshop dock)
		{
			this.dock = dock;
			xrk = Helper.XmlRegistry.PluginRegistryKey;
			
			try { dock.cbTask.SelectedIndex = LastOWAction;	} 
			catch {	dock.cbTask.SelectedIndex = 0;}
			dock.cbTask.SelectionChanged += (s, e) => cbTask_SelectedIndexChanged(s, EventArgs.Empty);

			dock.cbDesc.IsChecked = ChangeDescription;
			dock.cbDesc.IsCheckedChanged += (s, e) => cbDesc_CheckedChanged(s, EventArgs.Empty);

			dock.cbgid.IsChecked = SetCustomGroup;
			dock.cbgid.IsCheckedChanged += (s, e) => cbgid_CheckedChanged(s, EventArgs.Empty);

            dock.cbfix.IsChecked = FixCloned;
			dock.cbfix.IsCheckedChanged += (s, e) => cbfix_CheckedChanged(s, EventArgs.Empty);

            dock.cbclean.IsChecked = FixCloned;
			dock.cbclean.IsCheckedChanged += (s, e) => cbclean_CheckedChanged(s, EventArgs.Empty);

            dock.cbRemTxt.IsChecked = RemoveNoneDefaultLangaugeStrings;
			dock.cbRemTxt.IsCheckedChanged += (s, e) => cbRemTxt_CheckedChanged(s, EventArgs.Empty);

            dock.cbparent.IsChecked = CreateStandAlone;
			dock.cbparent.IsCheckedChanged += (s, e) => cbparent_CheckedChanged(s, EventArgs.Empty);

            dock.cbdefault.IsChecked = PullDefaultColorOnly;
			dock.cbdefault.IsCheckedChanged += (s, e) => cbdefault_CheckedChanged(s, EventArgs.Empty);

            dock.cbwallmask.IsChecked = PullWallmasks;
			dock.cbwallmask.IsCheckedChanged += (s, e) => cbwallmask_CheckedChanged(s, EventArgs.Empty);

            dock.cbanim.IsChecked = PullAnimations;
			dock.cbanim.IsCheckedChanged += (s, e) => cbanim_CheckedChanged(s, EventArgs.Empty);

            dock.cbstrlink.IsChecked = PullStrLinkedResources;
			dock.cbstrlink.IsCheckedChanged += (s, e) => cbstrlink_CheckedChanged(s, EventArgs.Empty);

            dock.cbOrgGmdc.IsChecked = ReferenceOriginalMesh;
			dock.cbOrgGmdc.IsCheckedChanged += (s, e) => cbOrgGmdc_CheckedChanged(s, EventArgs.Empty);
		}

        public void SetDefaults()
        {
            dock.cbDesc.IsChecked = true;
            dock.cbgid.IsChecked = true;
            dock.cbfix.IsChecked = true;
            dock.cbclean.IsChecked = true;
            dock.cbRemTxt.IsChecked = true;
            dock.cbparent.IsChecked = false;
            dock.cbdefault.IsChecked = true;
            dock.cbwallmask.IsChecked = true;
            dock.cbanim.IsChecked = false;
            dock.cbstrlink.IsChecked = true;
            dock.cbOrgGmdc.IsChecked = false;			
        }

		#region Properties
		/// <summary>
		/// true, if user wants to show the OBJD Filenames in OW
		/// </summary>
		public  int LastOWAction
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("LastOWAction", 0);
				return Convert.ToInt32(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("LastOWAction", value);
			}
		}

		public bool ChangeDescription 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("ChangeDescription", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("ChangeDescription", value);
			}
		}		

		public bool SetCustomGroup 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("SetCustomGroup", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("SetCustomGroup", value);
			}
		}

		public bool FixCloned 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("FixCloned", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("FixCloned", value);
			}
		}

		public bool Cleanup 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("Cleanup", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("Cleanup", value);
			}
		}

		public bool RemoveNoneDefaultLangaugeStrings 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("RemoveNoneDefaultLangaugeStrings", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("RemoveNoneDefaultLangaugeStrings", value);
			}
		}

		public bool CreateStandAlone 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("CreateStandAlone", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("CreateStandAlone", value);
			}
		}

		public bool PullDefaultColorOnly 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("PullDefaultColorOnly", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("PullDefaultColorOnly", value);
			}
		}

		public bool PullWallmasks
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("PullWallmaks", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("PullWallmaks", value);
			}
		}

		public bool PullAnimations
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("PullAnimations", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("PullAnimations", value);
			}
		}

		public bool PullStrLinkedResources
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("PullStrLinkedResources", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("PullStrLinkedResources", value);
			}
		}

		public bool ReferenceOriginalMesh
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("ReferenceOriginalMesh", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("ReferenceOriginalMesh", value);
			}
		}

		#endregion
		

		#region IDisposable Member

		public void Dispose()
		{
			dock.cbTask.SelectionChanged -= (s, e) => cbTask_SelectedIndexChanged(s, EventArgs.Empty);
			dock.cbDesc.IsCheckedChanged -= (s, e) => cbDesc_CheckedChanged(s, EventArgs.Empty);
			dock.cbgid.IsCheckedChanged -= (s, e) => cbgid_CheckedChanged(s, EventArgs.Empty);
			dock.cbfix.IsCheckedChanged -= (s, e) => cbfix_CheckedChanged(s, EventArgs.Empty);
			dock.cbclean.IsCheckedChanged -= (s, e) => cbclean_CheckedChanged(s, EventArgs.Empty);
			dock.cbRemTxt.IsCheckedChanged -= (s, e) => cbRemTxt_CheckedChanged(s, EventArgs.Empty);
			dock.cbparent.IsCheckedChanged -= (s, e) => cbparent_CheckedChanged(s, EventArgs.Empty);
			dock.cbdefault.IsCheckedChanged -= (s, e) => cbdefault_CheckedChanged(s, EventArgs.Empty);
			dock.cbwallmask.IsCheckedChanged -= (s, e) => cbwallmask_CheckedChanged(s, EventArgs.Empty);
			dock.cbanim.IsCheckedChanged -= (s, e) => cbanim_CheckedChanged(s, EventArgs.Empty);
			dock.cbstrlink.IsCheckedChanged -= (s, e) => cbstrlink_CheckedChanged(s, EventArgs.Empty);
			dock.cbOrgGmdc.IsCheckedChanged -= (s, e) => cbOrgGmdc_CheckedChanged(s, EventArgs.Empty);

			dock = null;
			xrk = null;
		}

		#endregion

		
		#region Events
		private void cbDesc_CheckedChanged(object sender, EventArgs e)
		{
			Avalonia.Controls.CheckBox cb = sender as Avalonia.Controls.CheckBox;
			ChangeDescription = cb.IsChecked == true;
		}

		private void cbgid_CheckedChanged(object sender, EventArgs e)
		{
			Avalonia.Controls.CheckBox cb = sender as Avalonia.Controls.CheckBox;
			SetCustomGroup = cb.IsChecked == true;
		}

		private void cbTask_SelectedIndexChanged(object sender, EventArgs e)
		{
            Avalonia.Controls.ComboBox cb = sender as Avalonia.Controls.ComboBox;
			LastOWAction = cb.SelectedIndex;
		}

		private void cbfix_CheckedChanged(object sender, EventArgs e)
		{
			Avalonia.Controls.CheckBox cb = sender as Avalonia.Controls.CheckBox;
			FixCloned = cb.IsChecked == true;
		}

		private void cbclean_CheckedChanged(object sender, EventArgs e)
		{
			Avalonia.Controls.CheckBox cb = sender as Avalonia.Controls.CheckBox;
			Cleanup = cb.IsChecked == true;
		}

		private void cbRemTxt_CheckedChanged(object sender, EventArgs e)
		{
			Avalonia.Controls.CheckBox cb = sender as Avalonia.Controls.CheckBox;
			RemoveNoneDefaultLangaugeStrings = cb.IsChecked == true;
		}

		private void cbparent_CheckedChanged(object sender, EventArgs e)
		{
			Avalonia.Controls.CheckBox cb = sender as Avalonia.Controls.CheckBox;
			CreateStandAlone = cb.IsChecked == true;
		}

		private void cbdefault_CheckedChanged(object sender, EventArgs e)
		{
			Avalonia.Controls.CheckBox cb = sender as Avalonia.Controls.CheckBox;
			PullDefaultColorOnly = cb.IsChecked == true;
		}

		private void cbwallmask_CheckedChanged(object sender, EventArgs e)
		{
			Avalonia.Controls.CheckBox cb = sender as Avalonia.Controls.CheckBox;
			PullWallmasks = cb.IsChecked == true;
		}

		private void cbanim_CheckedChanged(object sender, EventArgs e)
		{
			Avalonia.Controls.CheckBox cb = sender as Avalonia.Controls.CheckBox;
			PullAnimations = cb.IsChecked == true;
		}

		private void cbstrlink_CheckedChanged(object sender, EventArgs e)
		{
			Avalonia.Controls.CheckBox cb = sender as Avalonia.Controls.CheckBox;
			PullStrLinkedResources = cb.IsChecked == true;
		}

		private void cbOrgGmdc_CheckedChanged(object sender, EventArgs e)
		{
			Avalonia.Controls.CheckBox cb = sender as Avalonia.Controls.CheckBox;
			ReferenceOriginalMesh = cb.IsChecked == true;
		}
		#endregion
	}
}
