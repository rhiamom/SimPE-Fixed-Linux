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

// Ported from WinForms RubberBandHelper.
// Original: extends WinForms Control, overlays drag-resize rubber-band on a DockContainer.
//   Used SetStyle(ControlStyles.*) and OnPaint(PaintEventArgs) / OnMouseMove(MouseEventArgs).
// On Avalonia: extends Avalonia.Controls.Control.
//   SetStyle() calls removed (Avalonia handles double-buffering natively).
//   Rendering will use Render(DrawingContext) in a future pass.
//   Drag-resize rubber band is no-op on Mac (fixed layout).

using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace Ambertation.Windows.Forms;

/// <summary>
/// Visual overlay during drag-resize of a DockContainer.
/// Ported from WinForms RubberBandHelper : Control.
/// On Avalonia, drag-resize is no-op; the class is kept for API compatibility.
/// </summary>
public class RubberBandHelper : Avalonia.Controls.Control
{
    private DockContainer dc;
    private Dictionary<object, bool> map;
    private DockStyle dock;

    public DockStyle ContainerDock => dock;

    /// <summary>The bounding rectangle of this rubber-band overlay (zero-based).</summary>
    public System.Drawing.Rectangle ClientRectangle
        => new System.Drawing.Rectangle(0, 0, (int)Width, (int)Height);

    internal RubberBandHelper(DockContainer dc)
    {
        this.dc = dc;
        map = new Dictionary<object, bool>();
        // On Mac, no rubber-band overlay is drawn; dock style is captured for API callers.
        dock = dc.Dock is DockStyle ds ? ds : DockStyle.None;
    }

    internal void Close()
    {
        // On Mac, no rubber-band to remove.
    }

    public override void Render(DrawingContext context)
    {
        // Rubber-band resize rendering will be implemented here using Avalonia
        // DrawingContext in a future pass.
        if (dc?.Manager?.Renderer?.DockPanelRenderer != null)
            dc.Manager.Renderer.DockPanelRenderer.RenderResizePanel(dc, this, null);
        base.Render(context);
    }
}
