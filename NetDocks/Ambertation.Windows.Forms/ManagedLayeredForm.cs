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

// Ported from WinForms ManagedLayeredForm.
// Original: extends WinForms Form via LayeredForm, with WndProc and mouse event overrides.
// On Avalonia: extends LayeredForm (Avalonia Window). Mouse events use Avalonia pointer events.
// The InitializeComponent designer-generated code is removed; layout is handled by Avalonia.

using System.Drawing;
using Avalonia.Input;

namespace Ambertation.Windows.Forms;

/// <summary>
/// A managed layered overlay window (dock hints, overlays) owned by a DockManager.
/// Ported from WinForms ManagedLayeredForm : LayeredForm.
/// On Avalonia, this extends LayeredForm (Avalonia Window).
/// </summary>
public class ManagedLayeredForm : LayeredForm
{
    private DockManager manager;

    internal DockManager Manager => manager;

    protected ManagedLayeredForm(DockManager manager)
    {
        this.manager = manager;
    }

    internal ManagedLayeredForm(DockManager manager, Bitmap bitmap)
        : base(bitmap)
    {
        this.manager = manager;
    }

    internal ManagedLayeredForm(DockManager manager, Color cl, Size sz)
        : base(cl, sz)
    {
        this.manager = manager;
    }

    /// <summary>
    /// Called when a pointer enters or leaves the hit area of this overlay.
    /// On Avalonia, wired via Avalonia pointer events instead of WndProc.
    /// </summary>
    internal virtual void MouseOver(System.Drawing.Point pt, bool hit) { }
}
