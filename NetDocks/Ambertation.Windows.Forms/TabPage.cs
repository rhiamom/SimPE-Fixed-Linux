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

// Ported from WinForms TabPage.
// Original used System.Windows.Forms attributes (DesignerSerializationVisibility,
//   ReadOnly, Browsable) and System.Windows.Forms.DockStyle.
// On Avalonia: DockStyle is the local enum defined in WinFormsCompat.cs.
//   WinForms designer attributes removed; Browsable/ReadOnly kept via ComponentModel.

using System;
using System.ComponentModel;

namespace Ambertation.Windows.Forms;

/// <summary>
/// A tab page inside a TabControl — a DockPanel constrained to Dock=Fill.
/// Ported from WinForms TabPage : DockPanel.
/// </summary>
public class TabPage : DockPanel
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [ReadOnly(true)]
    [Browsable(false)]
    public new DockStyle Dock
    {
        get => base.Dock;
        set { }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [ReadOnly(true)]
    public override bool ShowCollapseButton
    {
        get => base.ShowCollapseButton;
        set { }
    }

    public TabControl TabControl => Manager as TabControl;

    public override bool OnlyChild => false;

    [ReadOnly(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override string ButtonText
    {
        get => base.CaptionText;
        set => base.CaptionText = value;
    }

    public TabPage()
        : this(null)
    {
    }

    public TabPage(TabControl tc)
        : base(tc)
    {
        base.Dock              = DockStyle.Fill;
        base.ShowCollapseButton = false;
    }

    protected override void OnDockChanged(EventArgs e)
    {
        base.Dock = DockStyle.Fill;
        base.OnDockChanged(e);
    }
}
