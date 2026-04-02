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
using Avalonia.Markup.Xaml;
using SimPe.Events;

namespace SimPe
{
    partial class MainForm : Avalonia.Controls.Window
    {
        // All field declarations are in SimPE.Main.Stubs.cs — do NOT re-declare them here.

        /// <summary>
        /// Public constructor — called by App.axaml.cs.
        /// AvaloniaXamlLoader.Load(this) explicitly loads MainForm.axaml from the embedded
        /// resource — bypassing the source generator (which generates an empty stub when no
        /// MainForm.axaml.cs code-behind exists).
        /// </summary>
        public MainForm()
        {
            throw new System.InvalidOperationException("CONSTRUCTOR_BODY_REACHED");
            System.IO.File.AppendAllText("/tmp/simpe.log", "1: constructor body started\n");
            try { AvaloniaXamlLoader.Load(this); }
            catch (Exception ex) { System.IO.File.AppendAllText("/tmp/simpe.log", "AXAML LOAD FAILED:\n" + ex + "\n"); throw; }
            System.IO.File.AppendAllText("/tmp/simpe.log", "2: AXAML loaded. Title=" + Title + "\n");

            SetupMainForm();
            System.IO.File.AppendAllText("/tmp/simpe.log", "3: SetupMainForm done\n");
            this.Opened += LoadForm;
        }
    }
}
