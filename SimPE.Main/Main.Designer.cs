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

// Ported from WinForms to Avalonia.
// All field declarations live in SimPE.Main.Stubs.cs (partial class fragment).
// This file provides:
//   - The base class declaration (: Avalonia.Controls.Window)
//   - A minimal InitializeComponent() that sets window properties
//   - A public constructor that calls SetupMainForm() (business logic in Main.Setup.cs)

using System;
using Avalonia;
using Avalonia.Controls;
using SimPe.Events;

namespace SimPe
{
    partial class MainForm : Avalonia.Controls.Window
    {
        // All field declarations are in SimPE.Main.Stubs.cs — do NOT re-declare them here.

        /// <summary>
        /// Minimal Avalonia InitializeComponent substitute.
        /// Sets the window title and minimum size; actual layout is built at runtime
        /// by SetupMainForm() / LoadForm() in Main.Setup.cs.
        /// </summary>
        private void InitializeComponent()
        {
            Title   = "SimPE - Sims Package Editor";
            Width   = 1200;
            Height  = 800;
            MinWidth  = 800;
            MinHeight = 600;
        }

        /// <summary>
        /// Public constructor — called by App.axaml.cs.
        /// Runs InitializeComponent then calls SetupMainForm (business logic).
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            SetupMainForm();
        }
    }
}
