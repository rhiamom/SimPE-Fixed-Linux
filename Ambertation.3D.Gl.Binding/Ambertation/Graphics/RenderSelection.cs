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
using Avalonia.Controls;
using Ambertation.Scenes;
using Ambertation.Scenes.Collections;

namespace Ambertation.Graphics;

public class RenderSelection : UserControl
{
	private ListBox lb;

	private Scene scn;

	private SceneToMesh stm;

	private DirectXPanel dx;

	public Scene Scene
	{
		get
		{
			return scn;
		}
		set
		{
			scn = value;
			SetContent();
		}
	}

	public DirectXPanel DirectXPanel
	{
		get
		{
			return dx;
		}
		set
		{
			if (dx != null)
			{
				dx.ResetDevice -= dx_ResetDevice;
			}
			dx = value;
			if (dx != null)
			{
				dx.ResetDevice += dx_ResetDevice;
			}
			SetContent();
		}
	}

	private void InitializeComponent()
	{
		lb = new ListBox();
		lb.SelectionMode = SelectionMode.Multiple;
		Content = lb;
	}

	public RenderSelection()
	{
		InitializeComponent();
		lb.SelectionChanged += lb_SelectionChanged;
	}

	private void dx_ResetDevice(object sender, EventArgs e)
	{
		if (!(sender is DirectXPanel directXPanel))
		{
			return;
		}
		try
		{
			directXPanel.Meshes.Clear(dispose: true);
			if (lb.SelectedItem == null)
			{
				directXPanel.Meshes.AddRange(stm.ConvertToDx());
			}
			else if (!(lb.SelectedItem is Joint))
			{
				directXPanel.Meshes.AddRange(stm.ConvertToDx());
			}
			else if (lb.SelectedItems != null && lb.SelectedItems.Count == 1)
			{
				directXPanel.Meshes.AddRange(stm.ConvertToDx(lb.SelectedItem as Joint));
			}
			else
			{
				JointCollection jointCollection = new JointCollection();
				if (lb.SelectedItems != null)
				{
					foreach (object selectedItem in lb.SelectedItems)
					{
						if (selectedItem is Joint)
						{
							jointCollection.Add(selectedItem as Joint);
						}
					}
				}
				directXPanel.Meshes.AddRange(stm.ConvertToDx(jointCollection));
			}
		}
		catch
		{
		}
	}

	private void SetContent()
	{
		lb.Items.Clear();
		stm = null;
		if (scn == null || dx == null)
		{
			return;
		}
		stm = new SceneToMesh(scn, dx);
		dx.Reset();
		dx.ResetDefaultViewport();
		lb.Items.Add("--- [Display Mesh] ---");
		foreach (Joint item in scn.JointCollection)
		{
			lb.Items.Add(item);
		}
	}

	private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (dx != null)
		{
			dx.Reset();
		}
	}
}
