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
 ***************************************************************************/

// No-op replacement for the original Windows-only UxTheme P/Invoke wrapper.
// uxtheme.dll and comctl32.dll are Windows system DLLs that do not exist on
// Linux or macOS; the original DllImport calls throw DllNotFoundException on
// those platforms.  Avalonia provides its own cross-platform theming, so this
// class simply returns safe defaults and does nothing.

using System;

namespace Ambertation.Windows.Forms
{
    /// <summary>No-op wrapper — replaces Windows-only UxTheme P/Invoke calls.</summary>
    public class UxTheme
    {
        public static Version ControlVersion => new Version();
        public static bool Themed => false;
        public static bool VisualStylesEnabled => false;

        public static void Draw(System.Drawing.Graphics g, object ctl, string themeClass, int themePart, int themeState, System.Drawing.Rectangle bounds, System.Drawing.Rectangle clipRect) { }

        public static void DrawBackground(System.Drawing.Graphics g, object ctl, System.Drawing.Rectangle clipRect) { }

        public static void DrawBackground(System.Drawing.Graphics g, object ctl, System.Drawing.Rectangle clipRect, bool ignoreBackColor) { }
    }
}
