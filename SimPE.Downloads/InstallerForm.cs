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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using Avalonia.Controls;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for InstallerForm.
	/// </summary>
	public class InstallerForm : Avalonia.Controls.Window
	{
		private SimPe.Plugin.InstallerControl installerControl1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public InstallerForm()
		{
			InitializeComponent();
		}

		public void Dispose()
		{
			if (components != null)
			{
				components.Dispose();
			}
		}

		private void InitializeComponent()
		{
            this.installerControl1 = new SimPe.Plugin.InstallerControl();
            this.Title = "Content Preview";
            this.Width = 624;
            this.Height = 334;
            this.Content = this.installerControl1;
		}

    }
}
