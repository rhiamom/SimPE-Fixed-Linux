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
using Avalonia.Media;
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

	private Joint GetJointFromItem(object item)
	{
		if (item is Joint j) return j;
		if (item is Avalonia.Controls.Border b && b.Tag is Joint bj) return bj;
		return null;
	}

	private void dx_ResetDevice(object sender, EventArgs e)
	{
		if (!(sender is DirectXPanel directXPanel))
		{
			return;
		}
		if (stm == null) return;
		try
		{
			directXPanel.Meshes.Clear(dispose: true);
			var selectedJoint = GetJointFromItem(lb.SelectedItem);
			if (selectedJoint == null)
			{
				directXPanel.Meshes.AddRange(stm.ConvertToDx());
			}
			else if (lb.SelectedItems != null && lb.SelectedItems.Count == 1)
			{
				directXPanel.Meshes.AddRange(stm.ConvertToDx(selectedJoint));
			}
			else
			{
				JointCollection jointCollection = new JointCollection();
				if (lb.SelectedItems != null)
				{
					foreach (object selectedItem in lb.SelectedItems)
					{
						var joint = GetJointFromItem(selectedItem);
						if (joint != null) jointCollection.Add(joint);
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
		lb.Items.Add(new TextBlock { Text = "--- [Display Mesh] ---", FontWeight = FontWeight.Bold });
		foreach (Joint item in scn.JointCollection)
		{
			var color = stm.GetJointColor(item);
			var avColor = Color.FromRgb(color.R, color.G, color.B);
			var tb = new Avalonia.Controls.Border
			{
				Background = new SolidColorBrush(avColor),
				CornerRadius = new Avalonia.CornerRadius(4),
				Padding = new Avalonia.Thickness(6, 2),
				Margin = new Avalonia.Thickness(2),
				Child = new TextBlock
				{
					Text = item.ToString(),
					FontWeight = FontWeight.Bold,
					Foreground = Brushes.Black
				},
				Tag = item
			};
			lb.Items.Add(tb);
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
