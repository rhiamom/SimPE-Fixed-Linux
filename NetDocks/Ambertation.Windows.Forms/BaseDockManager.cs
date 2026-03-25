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

// Ported from WinForms BaseDockManager.
// Original: abstract class extending DockContainer (WinForms), with a BaseRenderer,
//   floating panel list, and abstract drag-dock methods (StartDockMode, StopDockMode,
//   MouseMoved) implemented via WinForms message pump.
// On Avalonia: DockContainer base is now our Avalonia port.
//   Drag-dock abstract methods are kept as no-ops.
//   BaseRenderer is kept as a property for rendering pipeline compatibility.

using System;
using System.Collections.Generic;

namespace Ambertation.Windows.Forms;

/// <summary>
/// Abstract base for the dock manager hierarchy.
/// Ported from WinForms BaseDockManager : DockContainer.
/// On Avalonia, drag-dock operations are no-op.
/// </summary>
public abstract class BaseDockManager : DockContainer
{
    private BaseRenderer _renderer;

    protected bool dockmode;

    protected List<DockPanel> floatingpanels;

    // ── Properties ────────────────────────────────────────────────────────

    public BaseRenderer Renderer
    {
        get => _renderer;
        set => _renderer = value;
    }

    public bool DockMode => dockmode;

    protected abstract bool MeAsCenterDock { get; }

    // ── Constructor ───────────────────────────────────────────────────────

    protected BaseDockManager(BaseRenderer renderer)
        : base(null)
    {
        _renderer      = renderer;
        floatingpanels = new List<DockPanel>();
    }

    // ── Floating panel tracking ───────────────────────────────────────────

    internal void NotifyFloating(DockPanel dp)
    {
        if (dp.Floating && !floatingpanels.Contains(dp))
            floatingpanels.Add(dp);
        else if (!dp.Floating && floatingpanels.Contains(dp))
            floatingpanels.Remove(dp);
    }

    // ── Drag-dock stubs (no-op on Mac) ────────────────────────────────────

    internal virtual void StartDockMode(DockPanel dock) { }
    internal virtual void StopDockMode(DockPanel dock)  { }
    internal virtual void MouseMoved(System.Drawing.Point scrpt) { }

    /// <summary>
    /// Dock a panel at the given style position.
    /// On Mac, docking interactions are no-op — panel is placed in the first
    /// matching container or this manager if MeAsCenterDock.
    /// </summary>
    internal virtual void DockPanelInt(DockPanel dp, object style)
    {
        // No physical layout on Mac; DockPanel.DockContainer already handles
        // the container assignment for logical tracking.
    }

    // ── Container removal (no-op on Mac) ─────────────────────────────────

    internal void Remove(DockContainer dc) { }

    // ── Hint hover (no-op on Mac) ─────────────────────────────────────────

    protected void MouseOverHint(object sender, EventArgs e) { }
}
