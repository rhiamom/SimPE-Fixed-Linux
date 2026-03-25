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

// Ported from WinForms TabControl.
// Original: extends BaseDockManager, used ManagerSingelton.Global.TabRenderer
//   and WinForms ToolboxItem/ToolboxBitmap designer attributes.
// On Avalonia: WinForms designer attributes removed.
//   ManagerSingelton.Global.TabRenderer is now a property on ManagerSingelton.
//   TabRenderer defaults to null until rendering is wired in a future pass.

using System;

namespace Ambertation.Windows.Forms;

/// <summary>
/// Tab container — holds TabPages and renders them as a tab strip.
/// Ported from WinForms TabControl : BaseDockManager.
/// On Avalonia, tab rendering will be implemented in a future pass.
/// </summary>
public class TabControl : BaseDockManager
{
    public override bool OneChild => false;

    protected override bool MeAsCenterDock => true;

    public TabControl()
        : base(ManagerSingelton.Global.TabRenderer)
    {
    }

    protected override void CleanUp() { }

    internal override void StartDockMode(DockPanel dock)  { }

    internal override void StopDockMode(DockPanel dock)
    {
        AddPage(dock as TabPage);
    }

    internal override void MouseMoved(System.Drawing.Point scrpt) { }

    public void AddPage(TabPage tp)
    {
        if (tp != null) DockPanelInt(tp, DockStyle.Fill);
    }

    public override void Collapse(bool animated = true) { }
    public override void Expand(bool animated = true)   { }
}
