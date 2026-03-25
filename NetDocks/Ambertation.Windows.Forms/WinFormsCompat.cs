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

// Local definitions for WinForms types that are not available on macOS.
// System.Windows.Forms is Windows-only; these are the equivalents used throughout
// the NetDocks rendering and layout pipeline.

using System;

namespace Ambertation.Windows.Forms;

/// <summary>
/// Specifies how a control anchors to the edges of its container.
/// Mirrors System.Windows.Forms.DockStyle, replacing it on non-Windows platforms.
/// </summary>
public enum DockStyle
{
    None   = 0,
    Top    = 1,
    Bottom = 2,
    Left   = 3,
    Right  = 4,
    Fill   = 5,
}

/// <summary>
/// Represents padding or margin around a rectangle.
/// Mirrors System.Windows.Forms.Padding, replacing it on non-Windows platforms.
/// </summary>
public struct Padding
{
    public int Left   { get; set; }
    public int Top    { get; set; }
    public int Right  { get; set; }
    public int Bottom { get; set; }

    public static readonly Padding Empty = new Padding(0);

    public Padding(int all)
    {
        Left = Top = Right = Bottom = all;
    }

    public Padding(int left, int top, int right, int bottom)
    {
        Left   = left;
        Top    = top;
        Right  = right;
        Bottom = bottom;
    }

    /// <summary>
    /// Gets or sets all four sides to the same value.
    /// On get, returns Left if all sides are equal, otherwise -1.
    /// </summary>
    public int All
    {
        get => (Left == Top && Top == Right && Right == Bottom) ? Left : -1;
        set { Left = Top = Right = Bottom = value; }
    }

    public int Horizontal => Left + Right;
    public int Vertical   => Top + Bottom;

    public static bool operator ==(Padding a, Padding b)
        => a.Left == b.Left && a.Top == b.Top && a.Right == b.Right && a.Bottom == b.Bottom;

    public static bool operator !=(Padding a, Padding b) => !(a == b);

    public override bool Equals(object obj) => obj is Padding p && this == p;
    public override int  GetHashCode()      => HashCode.Combine(Left, Top, Right, Bottom);
    public override string ToString()       => $"{{Left={Left},Top={Top},Right={Right},Bottom={Bottom}}}";
}

/// <summary>
/// Specifies which mouse button was pressed.
/// Mirrors System.Windows.Forms.MouseButtons, replacing it on non-Windows platforms.
/// </summary>
[Flags]
public enum MouseButtons
{
    None    = 0,
    Left    = 1,
    Right   = 2,
    Middle  = 4,
    XButton1 = 8,
    XButton2 = 16,
}
