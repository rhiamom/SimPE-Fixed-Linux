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

// Ported from WinForms DockPanelControlCollection.
// Original was a thin wrapper around WinForms Control.ControlCollection.
// On Avalonia: stores items as object so callers that pass WinForms Controls
// still compile; actual Avalonia children are wired in a future pass.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Ambertation.Windows.Forms;

/// <summary>
/// Collection of child controls inside a DockPanel or DockContainer.
/// Stores items as <c>object</c> so callers using WinForms controls compile.
/// Actual Avalonia children will be wired in a future rendering pass.
/// </summary>
public class DockPanelControlCollection : IEnumerable<object>
{
    private readonly List<object> _list = new();

    public int    Count       => _list.Count;
    public object this[int i] => _list[i];

    public void Add(object c)    { if (c != null && !_list.Contains(c)) _list.Add(c); }
    public void Remove(object c) { _list.Remove(c); }
    public void Clear()          { _list.Clear(); }

    public IEnumerator<object> GetEnumerator()
        => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _list.GetEnumerator();
}
