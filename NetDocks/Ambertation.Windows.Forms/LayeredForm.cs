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

// Ported from WinForms LayeredForm.
// Original used UpdateLayeredWindow / GDI alpha-blending (Windows-only).
// On Avalonia, the Window compositor handles alpha natively.

using System;
using Avalonia;
using Avalonia.Controls;

namespace Ambertation.Windows.Forms;

/// <summary>
/// Base class for SimPE layered/alpha-blended overlay windows.
/// Ported from WinForms to Avalonia Window.
/// </summary>
public class LayeredForm : Window
{
    protected LayeredForm()
        : this(System.Drawing.Color.Blue, new System.Drawing.Size(300, 400))
    {
    }

    public LayeredForm(System.Drawing.Bitmap bitmap)
    {
        Topmost           = true;
        SystemDecorations = SystemDecorations.None;
        ShowInTaskbar     = false;
        if (bitmap != null)
        {
            Width  = bitmap.Width;
            Height = bitmap.Height;
        }
    }

    public LayeredForm(System.Drawing.Color cl, System.Drawing.Size sz)
    {
        Topmost           = true;
        SystemDecorations = SystemDecorations.None;
        ShowInTaskbar     = false;
        Width  = sz.Width;
        Height = sz.Height;
    }

    /// <summary>
    /// Screen position of this window.
    /// On Avalonia, Position is in device pixels; callers use it for hit-testing.
    /// </summary>
    internal PixelPoint ScreenLocation => Position;

    // ── WinForms Form compatibility members ───────────────────────────────

    /// <summary>Integer width/height matching WinForms Form.Width/Height.</summary>
    public new int Width  { get => (int)base.Width;  set => base.Width  = value; }
    public new int Height { get => (int)base.Height; set => base.Height = value; }

    /// <summary>Window size as System.Drawing.Size (WinForms Form.Size compat).</summary>
    public System.Drawing.Size Size
    {
        get => new System.Drawing.Size((int)Width, (int)Height);
        set { Width = value.Width; Height = value.Height; }
    }

    /// <summary>Window title text (WinForms Form.Text compat).</summary>
    public string Text
    {
        get => Title;
        set => Title = value ?? "";
    }

    /// <summary>Visibility (WinForms Form.Visible compat).</summary>
    public bool Visible
    {
        get => IsVisible;
        set { if (value) Show(); else Hide(); }
    }

    /// <summary>Screen bounds rectangle (WinForms Form.DesktopBounds compat).</summary>
    public System.Drawing.Rectangle DesktopBounds
        => new System.Drawing.Rectangle(Position.X, Position.Y, (int)Width, (int)Height);

    /// <summary>Set initial bitmap — alias for SelectBitmap (WinForms compat).</summary>
    public void Init(System.Drawing.Bitmap bmp) => SelectBitmap(bmp);

    /// <summary>
    /// Update the window's visible bitmap.
    /// On Avalonia, painting is done through Render() overrides; this is a no-op.
    /// </summary>
    public void SelectBitmap(System.Drawing.Bitmap bitmap) { }

    /// <summary>Refresh the bitmap after a resize. No-op on Avalonia.</summary>
    public void RefreshBitmap() { }

    /// <summary>
    /// Called when a new backing bitmap is created.
    /// On Avalonia, rendering uses Render(DrawingContext); this hook is kept for
    /// subclass compatibility during the porting pass.
    /// </summary>
    protected virtual void OnCreateBitmap(System.Drawing.Graphics g, System.Drawing.Bitmap bmp) { }

    /// <summary>Hit-test this window against a screen point.</summary>
    internal bool Hit(PixelPoint scrpt)
    {
        if (!IsVisible) return false;
        var loc = ScreenLocation;
        return scrpt.X > loc.X && scrpt.X < loc.X + (int)Width &&
               scrpt.Y > loc.Y && scrpt.Y < loc.Y + (int)Height;
    }
}
