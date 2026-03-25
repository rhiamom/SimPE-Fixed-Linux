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

// Ported from WinForms StructureTreeView.
// Original: WinForms UserControl with TreeView for debugging the dock panel hierarchy.
//   Used TreeNode subclasses, WinForms Form, DockStyle.Fill, PointToScreen,
//   LayeredForm for hit-test overlay visualization.
// On Avalonia: base changed to Avalonia UserControl.
//   TreeView uses TreeViewItem; Form replaced with Avalonia Window.
//   LayeredForm overlay retained (Avalonia Window); OnCreateBitmap rendering
//   will be wired to Avalonia DrawingContext in a future pass.

using System;
using System.Drawing;
using Avalonia.Controls;
using Ambertation.Windows.Forms;

namespace Ambertation.Windows.Forms.Debug;

/// <summary>
/// Debug inspector for the dock panel/container hierarchy.
/// Ported from WinForms StructureTreeView : UserControl.
/// </summary>
public class StructureTreeView : UserControl
{
    // ── Overlay window ─────────────────────────────────────────────────────

    private class MyLayeredForm : LayeredForm
    {
        public MyLayeredForm(Color cl)
            : base(cl, new Size(2048, 2048))
        {
        }

        protected override void OnCreateBitmap(Graphics g, Bitmap bmp)
        {
            // Rendering will use Avalonia DrawingContext in a future pass.
            // Original drew a white crosshatch + border using System.Drawing.
        }
    }

    // ── State ──────────────────────────────────────────────────────────────

    private TreeView tv;
    private MyLayeredForm lf;
    private Window hostWindow;

    // ── Constructor ────────────────────────────────────────────────────────

    public StructureTreeView(DockManager manager)
    {
        tv = new TreeView();
        Content = tv;

        lf = new MyLayeredForm(Color.FromArgb(144, Color.Red));
        lf.Hide();

        var root = MakeItem(manager.Name ?? "DockManager");
        AddNodes(root, manager);
        tv.Items.Add(root);
    }

    // ── Tree helpers ───────────────────────────────────────────────────────

    private static TreeViewItem MakeItem(string text)
        => new TreeViewItem { Header = text };

    private void AddNodes(TreeViewItem parent, DockContainer main)
    {
        foreach (object control in main.Controls)
        {
            if (control is DockButtonBar bar)
            {
                parent.Items.Add(MakeItem(
                    $"{bar.Name} ({bar.GetType().Name}) - {bar.Dock}"));
            }
            else if (control is DockPanel dp)
            {
                parent.Items.Add(MakeItem(
                    $"{dp.TabText}, {dp.CaptionText}, {dp.Name} ({dp.GetType().Name}) - {dp.Dock}"));
            }
            else if (control is DockContainer dc)
            {
                var node = MakeItem(
                    $"{dc.Name} ({dc.GetType().Name}) - {dc.Dock}");
                parent.Items.Add(node);
                AddNodes(node, dc);
            }
        }
    }

    // ── Overlay ────────────────────────────────────────────────────────────

    public void HideOverlay() { lf.Hide(); }

    // ── Static launcher ───────────────────────────────────────────────────

    public static void Execute(DockManager manager)
    {
        var win = new Window
        {
            Title   = (manager.Name ?? "DockManager") + " Structure",
            Topmost = true,
            Width   = 400,
            Height  = 600,
        };
        var stv = new StructureTreeView(manager);
        win.Content = stv;
        win.Closed += (_, _) => stv.HideOverlay();
        win.Show();
    }
}
