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

namespace Ambertation.Graphics;

// TODO: Implement PropertyGrid-based viewport settings UI for Avalonia.
// The double-click trigger in DirectXPanel is currently commented out pending this work.
public class ViewPortSetup : Window
{
	private ViewportSetting vp;

	private DirectXPanel panel;

	private static bool visible;

	public new static bool Visible => visible;

	private ViewPortSetup()
	{
		Title = "ViewPort Setup";
		Width = 248;
		Height = 429;
	}

	public static ViewPortSetup Execute(ViewportSetting vp, DirectXPanel panel)
	{
		visible = true;
		ViewPortSetup viewPortSetup = new ViewPortSetup();
		viewPortSetup.vp = vp;
		viewPortSetup.panel = panel;
		viewPortSetup.Show();
		return viewPortSetup;
	}

	public static void Hide(ViewPortSetup f)
	{
		try
		{
			f.Close();
			visible = false;
		}
		catch
		{
		}
	}
}
