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

// Ported from WinForms/Windows implementation.
// All Windows GDI P/Invoke methods removed — not available on macOS/Avalonia.
// Only portable constants and WM_APP are kept; subclasses use Avalonia drawing instead.

using System;

namespace Ambertation.Windows.Forms;

public static class APIHelp
{
    // Windows application message base — kept because SplashForm uses WM_APP + offset.
    public const uint WM_APP = 0x8000;

    // No-op on macOS — kept for API compatibility with callers.
    public static IntPtr SendMessage(IntPtr hWnd, uint msg, long wParam, long lParam)
        => IntPtr.Zero;
}
