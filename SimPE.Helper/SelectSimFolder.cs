/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace SimPe
{
    /// <summary>
    /// Dialog that lets the user pick a Sims 2 install folder — either by
    /// choosing a known expansion from the drop-down or by browsing freely.
    /// Ported from WinForms Form to Avalonia Window.
    /// </summary>
    internal class SelectSimFolder : Avalonia.Controls.Window
    {
        // ── inner type ────────────────────────────────────────────────────────
        sealed class FolderWrapper
        {
            readonly string _name;
            readonly string _folder;

            public FolderWrapper(string name, string folder)
            {
                _name   = Localization.GetString(name);
                _folder = folder;
            }

            public string Folder => _folder;

            public override string ToString() => _folder;
        }

        // ── controls ──────────────────────────────────────────────────────────
        readonly TextBox  _tbPath;
        readonly ComboBox _cbPresets;

        // ── result ────────────────────────────────────────────────────────────
        bool _confirmed;

        // ── constructor ───────────────────────────────────────────────────────
        SelectSimFolder()
        {
            Title           = "Select Sim Folder";
            Width           = 680;
            SizeToContent   = SizeToContent.Height;
            CanResize       = false;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // ── preset combo ──────────────────────────────────────────────────
            _cbPresets = new ComboBox { HorizontalAlignment = HorizontalAlignment.Stretch };
            var presets = new List<FolderWrapper>();
            foreach (ExpansionItem ei in PathProvider.Global.Expansions)
                presets.Add(new FolderWrapper(ei.Name, ei.RealInstallFolder));
            _cbPresets.ItemsSource = presets;
            _cbPresets.SelectionChanged += (_, _) =>
            {
                if (_cbPresets.SelectedItem is FolderWrapper fw)
                    _tbPath.Text = fw.Folder;
            };

            // ── path text box ─────────────────────────────────────────────────
            _tbPath = new TextBox { HorizontalAlignment = HorizontalAlignment.Stretch };

            // ── browse button ─────────────────────────────────────────────────
            var btnBrowse = new Button { Content = "Browse…", MinWidth = 80 };
            btnBrowse.Click += async (_, _) => await BrowseAsync();

            // ── OK / Cancel ───────────────────────────────────────────────────
            var btnOK = new Button { Content = "OK", MinWidth = 80,
                                     HorizontalAlignment = HorizontalAlignment.Right };
            btnOK.Click += (_, _) => { _confirmed = true; Close(); };

            var btnCancel = new Button { Content = "Cancel", MinWidth = 80,
                                         HorizontalAlignment = HorizontalAlignment.Right };
            btnCancel.Click += (_, _) => { _confirmed = false; Close(); };

            // ── layout ────────────────────────────────────────────────────────
            // Row 0: [Presets combo] [Browse]
            // Row 1: [Folder: label] [path textbox]
            // Row 2: [OK] [Cancel]  (right-aligned)

            var grid = new Grid
            {
                Margin = new Thickness(12),
                RowDefinitions = new RowDefinitions("Auto,8,Auto,12,Auto"),
                ColumnDefinitions = new ColumnDefinitions("Auto,8,*,8,Auto"),
            };

            // Row 0: preset selector
            var lbPreset = new TextBlock
            {
                Text = "Preset:",
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeight.Bold,
            };
            Grid.SetRow(lbPreset,  0); Grid.SetColumn(lbPreset,  0);
            Grid.SetRow(_cbPresets, 0); Grid.SetColumn(_cbPresets, 2); Grid.SetColumnSpan(_cbPresets, 3);

            // Row 2: path entry + browse
            var lbFolder = new TextBlock
            {
                Text = "Folder:",
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeight.Bold,
            };
            Grid.SetRow(lbFolder, 2); Grid.SetColumn(lbFolder, 0);
            Grid.SetRow(_tbPath,  2); Grid.SetColumn(_tbPath,  2);
            Grid.SetRow(btnBrowse, 2); Grid.SetColumn(btnBrowse, 4);

            // Row 4: OK / Cancel
            var btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Spacing = 8,
            };
            btnPanel.Children.Add(btnOK);
            btnPanel.Children.Add(btnCancel);
            Grid.SetRow(btnPanel, 4); Grid.SetColumn(btnPanel, 0); Grid.SetColumnSpan(btnPanel, 5);

            grid.Children.Add(lbPreset);
            grid.Children.Add(_cbPresets);
            grid.Children.Add(lbFolder);
            grid.Children.Add(_tbPath);
            grid.Children.Add(btnBrowse);
            grid.Children.Add(btnPanel);

            Content = grid;

            // Keyboard: Enter = OK, Escape = Cancel
            KeyDown += (_, e) =>
            {
                if (e.Key == Avalonia.Input.Key.Return) { _confirmed = true;  Close(); }
                if (e.Key == Avalonia.Input.Key.Escape) { _confirmed = false; Close(); }
            };
        }

        // ── browse ────────────────────────────────────────────────────────────
        async Task BrowseAsync()
        {
            var opts = new Avalonia.Platform.Storage.FolderPickerOpenOptions
            {
                Title           = "Select Folder",
                AllowMultiple   = false,
            };

            if (System.IO.Directory.Exists(_tbPath.Text))
            {
                var start = await StorageProvider.TryGetFolderFromPathAsync(new Uri(_tbPath.Text));
                if (start != null)
                    opts.SuggestedStartLocation = start;
            }

            var results = await StorageProvider.OpenFolderPickerAsync(opts);
            if (results.Count > 0)
                _tbPath.Text = results[0].Path.LocalPath;
        }

        // ── public API ────────────────────────────────────────────────────────

        /// <summary>
        /// Shows the dialog and returns the chosen folder path, or
        /// <paramref name="path"/> unchanged if the user cancels.
        /// Must be called from the UI thread (awaited).
        /// </summary>
        public static async Task<string> ShowDialogAsync(
            Window owner, string path)
        {
            var dlg = new SelectSimFolder();
            dlg._tbPath.Text = path;
            await dlg.ShowDialog(owner);
            return dlg._confirmed ? dlg._tbPath.Text : path;
        }
    }

    // SelectSimFolderUITypeEditor (WinForms UITypeEditor for property grids) removed:
    // UITypeEditor is a WinForms design-time concept with no Avalonia equivalent,
    // and no callers exist in the current codebase.
}
