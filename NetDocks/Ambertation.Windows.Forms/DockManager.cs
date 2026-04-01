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

// Ported from WinForms DockManager.
// Original: concrete DockManager : BaseDockManager with drag-dock hints (DockHint,
//   DockOverlay, ManagedLayeredForm), serialization, DockButtonBar containers
//   for each dock side, and WinForms Control management.
// On Avalonia: base class hierarchy preserved (DockManager : BaseDockManager : DockContainer).
//   Drag-dock hints, overlays, and WinForms Controls wiring are removed.
//   Serialization kept for future implementation.
//   GetPanels() and DockPanel() public surface preserved for callers.

using System;
using System.Collections.Generic;
using System.IO;

namespace Ambertation.Windows.Forms;

/// <summary>
/// Top-level dock manager.
/// Ported from WinForms DockManager : BaseDockManager.
/// On Avalonia, drag-dock interactions are no-op; the manager tracks panels logically.
/// </summary>
public class DockManager : BaseDockManager
{
    /// <summary>Descriptor for a serialized DockContainer.</summary>
    public class DockContainerDescriptor
    {
        public DockContainer Container { get; }
        public int           Index     { get; }
        public bool          Collapsed { get; }
        public string        HighlightName { get; }

        internal DockContainerDescriptor(DockContainer dc, int index, bool collapsed, string highlightname)
        {
            Container     = dc;
            Index         = index;
            Collapsed     = collapsed;
            HighlightName = highlightname;
        }
    }

    // ── State ─────────────────────────────────────────────────────────────

    private System.Drawing.Size _defaultSize = new System.Drawing.Size(100, 100);

    protected override bool MeAsCenterDock => true;

    public bool Visible { get; set; } = true;

    public new System.Drawing.Size DefaultSize
    {
        get => _defaultSize;
        set => _defaultSize = value;
    }

    // ── Constructor ───────────────────────────────────────────────────────

    public DockManager()
        : base(null)    // null renderer — rendering is no-op on Mac for now
    {
        ManagerSingelton.Global.SetMainManager(this);
        manager = this;
    }

    // ── GetPanels ─────────────────────────────────────────────────────────

    public new List<DockPanel> GetPanels()
    {
        var dict = new Dictionary<string, DockPanel>();
        GetPanels(dict);
        return new List<DockPanel>(dict.Values);
    }

    protected override void GetPanels(Dictionary<string, DockPanel> list)
    {
        foreach (DockPanel dp in floatingpanels)
        {
            if (dp.Name == "") dp.Name = "dp_" + list.Count;
            list[dp.Name] = dp;
        }
        base.GetPanels(list);
    }

    // ── Docking ───────────────────────────────────────────────────────────

    /// <summary>
    /// Dock a panel at the given style.
    /// On Mac, physical layout is fixed; this is a logical no-op.
    /// </summary>
    public void DockPanel(DockPanel dp, object dockStyle)
    {
        DockPanelInt(dp, dockStyle);
    }

    // ── Cleanup ───────────────────────────────────────────────────────────

    public void ForceCleanUp() { }

    // ── Serialization (stubs — will be wired when layout persistence is needed) ──

    public void Serialize(BinaryWriter writer)   { }
    public void Deserialize(BinaryReader reader) { }
}
