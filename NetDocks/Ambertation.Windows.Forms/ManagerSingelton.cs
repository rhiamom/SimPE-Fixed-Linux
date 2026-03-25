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

// Ported from WinForms ManagerSingelton.
// Original implemented IMessageFilter (Windows message pump hook) for drag-dock.
// On Avalonia: IMessageFilter removed; panel registry and GetPanelWithName() kept.
// Drag-dock will be re-implemented using Avalonia pointer events in a future pass.

using System;
using System.Collections.Generic;

namespace Ambertation.Windows.Forms;

/// <summary>
/// Singleton registry of all DockPanels known to the docking system.
/// On macOS/Avalonia, used to look up panels by name; drag-docking is not yet implemented.
/// </summary>
public class ManagerSingelton
{
    private static ManagerSingelton glob;

    private readonly List<DockPanel>            known     = new();
    private readonly List<DockPanelFloatingForm> floats   = new();
    private DockManager dm;
    private BaseRenderer _tabRenderer;

    private int pnid;

    public static ManagerSingelton Global
    {
        get
        {
            if (glob == null)
                glob = new ManagerSingelton();
            return glob;
        }
    }

    public DockManager MainDockManager => dm;

    /// <summary>Whether floating windows should always stay on top.</summary>
    public bool TopmostFloats { get; set; } = true;

    /// <summary>
    /// Renderer used by TabControl. Returns null until a TabControl is constructed;
    /// rendering will be wired in a future pass.
    /// </summary>
    public BaseRenderer TabRenderer
    {
        get => _tabRenderer;
        internal set => _tabRenderer = value;
    }

    // ── Dock renderer (used by DockManager constructor in the original) ────

    /// <summary>Global dock renderer. Returns null until a DockManager sets one.</summary>
    public BaseRenderer DockRenderer { get; internal set; }

    private ManagerSingelton()
    {
        pnid = 0;
        dm   = null;
    }

    internal void SetMainManager(DockManager m)
    {
        if (dm == null)
            dm = m;
    }

    internal void AddPanel(DockPanel dp)
    {
        if (!known.Contains(dp))
        {
            known.Add(dp);
            if (dp.Name == "")
            {
                dp.Name = "ManagedDockPanel" + pnid;
                pnid++;
            }
        }
    }

    internal void RemovePanel(DockPanel dp)
    {
        known.Remove(dp);
    }

    internal void AddFloatForm(DockPanelFloatingForm form)
    {
        if (!floats.Contains(form)) floats.Add(form);
    }

    internal void RemoveFloatForm(DockPanelFloatingForm form)
    {
        floats.Remove(form);
    }

    /// <summary>Returns the first panel with the given Name, or null.</summary>
    public DockPanel GetPanelWithName(string name)
    {
        foreach (DockPanel item in known)
        {
            if (item.Name == name)
                return item;
        }
        return null;
    }
}
