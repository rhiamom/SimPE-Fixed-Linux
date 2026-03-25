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

// Ported from WinForms DockButtonBar.
// Original: extends WinForms UserControl, implements IButtonContainer.
//   Drew dock tab buttons for collapsed panels via OnPaint(PaintEventArgs).
//   Responded to OnMouseDown(MouseEventArgs) to expand a collapsed panel.
// On Avalonia: extends Avalonia UserControl.
//   Rendering will use Render(DrawingContext) in a future pass.
//   Pointer events replace WinForms mouse events.

using System.Collections.Generic;
using System.Drawing;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace Ambertation.Windows.Forms;

/// <summary>
/// Bar showing buttons for collapsed dock panels.
/// Ported from WinForms DockButtonBar : UserControl, IButtonContainer.
/// On Avalonia, rendering and pointer interaction will be wired in a future pass.
/// </summary>
public class DockButtonBar : UserControl, IButtonContainer
{
    /// <summary>Typed list of DockPanels — used throughout the docking pipeline.</summary>
    public class DockPanelList : List<DockPanel> { }

    // ── State ─────────────────────────────────────────────────────────────

    private DockPanelList panels;
    private Dictionary<DockContainer, DockPanelList> containers;
    private DockManager manager;
    private DockPanelButtonManager buttonData;

    // ── Properties ────────────────────────────────────────────────────────

    public DockManager Manager => manager;

    protected DockPanelButtonManager ButtonData => buttonData;

    public DockPanel Highlight { get => null; set { } }

    public DockStyle Dock { get; set; }

    public ButtonOrientation BestOrientation
    {
        get
        {
            if (Dock == DockStyle.Bottom) return ButtonOrientation.Top;
            if (Dock == DockStyle.Left)   return ButtonOrientation.Right;
            if (Dock == DockStyle.Top)    return ButtonOrientation.Bottom;
            return ButtonOrientation.Left;
        }
    }

    // ── Constructor ───────────────────────────────────────────────────────

    public DockButtonBar(DockManager manager)
    {
        this.manager = manager;
        panels     = new DockPanelList();
        containers = new Dictionary<DockContainer, DockPanelList>();
    }

    // ── Visibility ────────────────────────────────────────────────────────

    protected void SetVisibleState()
    {
        bool visible = panels.Count > 0;
        if (visible == IsVisible)
            InvalidateVisual();
        else if (manager?.Renderer?.DockPanelRenderer != null)
        {
            Width  = manager.Renderer.DockPanelRenderer.Dimension.Buttons;
            Height = Width;
        }
        IsVisible = visible;
    }

    // ── Container management ──────────────────────────────────────────────

    public bool Contains(DockContainer dc) => containers.ContainsKey(dc);

    public void Add(DockContainer c)
    {
        var raw    = c.GetDockedPanels();
        var docked = new DockPanelList();
        docked.AddRange(raw);
        containers[c] = docked;
        foreach (DockPanel p in docked)
        {
            p.SeperateInDockBar = false;
            if (!panels.Contains(p)) panels.Add(p);
        }
        if (panels.Count > 0)
            panels[panels.Count - 1].SeperateInDockBar = true;
        SetVisibleState();
    }

    public void Remove(DockContainer c)
    {
        if (containers.ContainsKey(c)) { DoRemove(c); SetVisibleState(); }
    }

    internal void SilentRemove(DockContainer c)
    {
        if (containers.ContainsKey(c)) DoRemove(c);
    }

    private void DoRemove(DockContainer c)
    {
        if (!containers.TryGetValue(c, out DockPanelList list) || list == null) return;
        containers.Remove(c);
        foreach (DockPanel p in list)
        {
            p.SeperateInDockBar = false;
            panels.Remove(p);
        }
    }

    public void Clear()
    {
        foreach (DockPanel p in panels) p.SeperateInDockBar = false;
        panels.Clear();
        containers.Clear();
        SetVisibleState();
    }

    private DockContainer FindDock(DockPanel p)
    {
        foreach (var kv in containers)
            foreach (DockPanel item in kv.Value)
                if (item == p) return kv.Key;
        return null;
    }

    // ── IButtonContainer ─────────────────────────────────────────────────

    public DockPanelList GetButtons() => panels;

    public Padding GetBorderSize(ButtonOrientation orient)
        => manager?.Renderer?.DockPanelRenderer?.GetBarBorderSize(orient) ?? Ambertation.Windows.Forms.Padding.Empty;

    // ── Rendering ─────────────────────────────────────────────────────────

    public override void Render(DrawingContext context)
    {
        // Dock button bar rendering will be implemented here in a future pass.
        // Original called: Manager.Renderer.DockPanelRenderer.RenderButtonBarBackground(...)
        // and ConstructButtonData(...).Render(renderbackgroundbar: false).
        base.Render(context);
    }
}
