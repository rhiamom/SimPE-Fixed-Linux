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

// Ported from WinForms DockPanelFloatingForm.
// Original: extends WinForms Form, implements IMessageFilter.
//   Used WndProc to intercept WM_MOVING, WM_ENTERSIZEMOVE/EXITSIZEMOVE,
//   WM_NCHITTEST, WM_ACTIVATE for drag-dock and topmost management.
//   Called Application.AddMessageFilter / Application.RemoveMessageFilter.
// On Avalonia: extends Avalonia Window.
//   WndProc / IMessageFilter removed — drag-dock is not yet implemented on Mac.
//   Floating windows are a future-pass feature; all dock/undock methods are no-op.

using System;
using Avalonia.Controls;

namespace Ambertation.Windows.Forms;

/// <summary>
/// Floating window that hosts a DockPanel when it is undocked.
/// Ported from WinForms DockPanelFloatingForm : Form, IMessageFilter.
/// On Avalonia, floating is not yet implemented; all methods are no-op.
/// </summary>
internal class DockPanelFloatingForm : Window
{
    private DockPanel dock;
    private DockContainer cnt;

    public DockPanel DockControl => dock;

    public BaseDockManager Manager => dock?.Manager;

    public bool HasContainer => cnt != null;

    public DockPanelFloatingForm(DockPanel dock)
    {
        ManagerSingelton.Global.AddFloatForm(this);
        Topmost = ManagerSingelton.Global.TopmostFloats;
        this.dock = dock;
    }

    ~DockPanelFloatingForm()
    {
        ManagerSingelton.Global.RemoveFloatForm(this);
    }

    public void DragContainerAlong(DockContainer cnt)
    {
        this.cnt = cnt;
        foreach (DockPanel dp in cnt.GetDockedPanels())
            dp.RefreshMargin();
    }

    // ── Floating lifecycle (no-op on Mac) ────────────────────────────────

    protected virtual void OnActivateApplication(bool active)
    {
        Topmost = active && ManagerSingelton.Global.TopmostFloats;
    }

    internal void StartFloatingBlocked(DockPanel p)
    {
        // On Mac, no WinForms message pump — floating is a future-pass feature.
        Title = p.CaptionText;
    }

    protected virtual void OnStartFloating()  { }
    protected virtual void OnStopFloating()   { }

    protected void StartFloating() { OnStartFloating(); }

    protected void StopFloating()
    {
        if (dock == null) return;
        if (HasContainer)
        {
            dock.UnFloat(this);
            if (cnt.GetDockedPanels().Count == 0)
            {
                cnt.Manager?.Remove(cnt);
                dock = null;
            }
        }
        else
        {
            dock.UnFloat(this);
        }
        OnStopFloating();
        if (dock == null)
        {
            cnt = null;
            Close();
        }
    }

    internal void SendeActivateEvent(bool active) => OnActivateApplication(active);
}
