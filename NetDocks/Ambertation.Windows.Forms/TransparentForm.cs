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

// Ported from WinForms TransparentForm.
// Original used Windows GDI layered windows (UpdateLayeredWindow, WM_NCHITTEST).
// Avalonia handles compositing and transparency natively — no GDI needed.

using System;
using Avalonia;
using Avalonia.Controls;

namespace Ambertation.Windows.Forms;

/// <summary>
/// Base class for SimPE overlay/splash windows.
/// Ported from WinForms to Avalonia Window.
/// </summary>
public class TransparentForm : Window
{
    /// <summary>
    /// Rectangle in which a mouse-down starts a window drag.
    /// Not applicable on Avalonia (window dragging is handled by the OS).
    /// </summary>
    protected virtual Rect HeaderRect => default(Rect);

    public TransparentForm()
    {
        Topmost      = true;
        ShowInTaskbar = false;
    }

    /// <summary>
    /// Called when the backing bitmap is created or updated.
    /// On Avalonia, rendering is done in XAML or by overriding Render(); this hook
    /// is kept for subclass compatibility during the porting pass.
    /// </summary>
    protected virtual void OnCreateBitmap(System.Drawing.Graphics g, System.Drawing.Bitmap b) { }
}
