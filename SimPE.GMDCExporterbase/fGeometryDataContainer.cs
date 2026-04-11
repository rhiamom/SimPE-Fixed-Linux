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

// Ported from WinForms Form to Avalonia UserControl stub (Mac port).
// All business logic lives in cGeometryDataContainer.cs — this file only
// provides the UI scaffold and exposes the named fields that cGeometryDataContainer
// accesses.  WinForms layout code (InitializeComponent / designer code) is removed;
// controls are created directly.

using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Layout;
using SimPe.Plugin.Gmdc;
using SimPe.Geometry;

namespace SimPe.Plugin
{
    // ── Visual CheckedListBox for GMDC form ──────────────────────────────────────
    internal class CheckedListBox : Avalonia.Controls.UserControl
    {
        private readonly List<(object Item, bool Checked)> _items = new();
        private readonly Avalonia.Controls.StackPanel _panel = new();
        public CheckedListBoxItemCollection Items { get; }

        public CheckedListBox()
        {
            Items = new CheckedListBoxItemCollection(_items, _panel);
            Content = new Avalonia.Controls.ScrollViewer { Content = _panel };
        }

        public bool GetItemChecked(int index) => _items[index].Checked;

        public CheckedItemsCollection CheckedItems => new CheckedItemsCollection(_items);
    }

    internal class CheckedItemsCollection
    {
        private readonly List<(object Item, bool Checked)> _items;
        internal CheckedItemsCollection(List<(object, bool)> items) { _items = items; }
        public int Count { get { int c = 0; foreach (var i in _items) if (i.Checked) c++; return c; } }
        public object this[int index]
        {
            get { int c = 0; foreach (var i in _items) { if (i.Checked) { if (c == index) return i.Item; c++; } } throw new IndexOutOfRangeException(); }
        }
    }

    internal class CheckedListBoxItemCollection
    {
        private readonly List<(object Item, bool Checked)> _items;
        private readonly Avalonia.Controls.StackPanel _panel;
        internal CheckedListBoxItemCollection(List<(object, bool)> items, Avalonia.Controls.StackPanel panel)
        { _items = items; _panel = panel; }

        public int Count => _items.Count;
        public object this[int i] => _items[i].Item;
        public bool GetItemChecked(int i) => _items[i].Checked;
        public void Clear() { _items.Clear(); _panel.Children.Clear(); }
        public void Add(object item) => Add(item, false);
        public void Add(object item, bool isChecked)
        {
            _items.Add((item, isChecked));
            var cb = new Avalonia.Controls.CheckBox
            {
                Content = item?.ToString() ?? "",
                IsChecked = isChecked,
                Margin = new Avalonia.Thickness(2)
            };
            cb.IsCheckedChanged += (s, e) =>
            {
                int idx = _panel.Children.IndexOf(cb);
                if (idx >= 0 && idx < _items.Count)
                    _items[idx] = (_items[idx].Item, cb.IsChecked == true);
            };
            _panel.Children.Add(cb);
        }
    }

    // ── fGeometryDataContainer ──────────────────────────────────────────────────
    /// <summary>
    /// Avalonia UserControl replacement for the WinForms fGeometryDataContainer Form.
    /// Exposes only the fields accessed by cGeometryDataContainer (all internal).
    /// </summary>
    internal class fGeometryDataContainer : Avalonia.Controls.UserControl, IDisposable
    {
        // ── TabItems (were WinForms TabPages) ───────────────────────────────────
        internal Avalonia.Controls.TabItem tMesh;
        internal Avalonia.Controls.TabItem tGeometryDataContainer;
        internal Avalonia.Controls.TabItem tGeometryDataContainer2;
        internal Avalonia.Controls.TabItem tGeometryDataContainer3;
        internal Avalonia.Controls.TabItem tModel;
        internal Avalonia.Controls.TabItem tSubset;
        internal Avalonia.Controls.TabItem tAdvncd;

        // ── Settings ────────────────────────────────────────────────────────────
        internal Avalonia.Controls.TextBox tb_ver;

        // ── Elements tab ────────────────────────────────────────────────────────
        internal Avalonia.Controls.TextBlock label_elements;
        internal Avalonia.Controls.ListBox   list_elements;
        internal Avalonia.Controls.TextBlock label_links;
        internal Avalonia.Controls.ListBox   list_links;
        internal Avalonia.Controls.TextBlock label_groups;
        internal Avalonia.Controls.ListBox   list_groups;
        internal Avalonia.Controls.TextBlock label_subsets;
        internal Avalonia.Controls.ListBox   list_subsets;

        // ── Element detail fields (tGeometryDataContainer) ──────────────────────
        internal Avalonia.Controls.ListBox lb_itemsa;
        internal Avalonia.Controls.ListBox lb_itemsa2;
        internal Avalonia.Controls.TextBox tb_itemsa2;
        internal Avalonia.Controls.TextBox tb_uk1;
        internal Avalonia.Controls.TextBox tb_uk5;
        internal Avalonia.Controls.TextBox tb_id;
        internal Avalonia.Controls.TextBox tb_mod1;
        internal Avalonia.Controls.TextBox tb_mod2;
        internal Avalonia.Controls.ListBox lb_itemsa1;
        internal Avalonia.Controls.ComboBox cbblock;
        internal Avalonia.Controls.ComboBox cbset;
        internal Avalonia.Controls.ComboBox cbid;

        // ── Group tab (tGeometryDataContainer3) ─────────────────────────────────
        internal Avalonia.Controls.ListBox lb_itemsc;
        internal Avalonia.Controls.TextBox tb_itemsc_name;
        internal Avalonia.Controls.TextBox tb_opacity;
        internal Avalonia.Controls.TextBox tb_uk2;
        internal Avalonia.Controls.TextBox tb_uk3;
        internal Avalonia.Controls.ListBox lb_itemsc2;
        internal Avalonia.Controls.TextBox tb_itemsc2;
        internal Avalonia.Controls.ListBox lb_itemsc3;
        internal Avalonia.Controls.TextBox tb_itemsc3;
        internal Avalonia.Controls.ComboBox cbGroupJoint;
        internal CheckedListBox lbmodel;
        internal Avalonia.Controls.TextBlock lb_models;

        // ── Link tab (tGeometryDataContainer2) ──────────────────────────────────
        internal Avalonia.Controls.ListBox lb_itemsb;
        internal Avalonia.Controls.ListBox lb_itemsb2;
        internal Avalonia.Controls.ListBox lb_itemsb3;
        internal Avalonia.Controls.ListBox lb_itemsb4;
        internal Avalonia.Controls.ListBox lb_itemsb5;
        internal Avalonia.Controls.TextBox tb_itemsb2;
        internal Avalonia.Controls.TextBox tb_itemsb3;
        internal Avalonia.Controls.TextBox tb_itemsb4;
        internal Avalonia.Controls.TextBox tb_itemsb5;
        internal Avalonia.Controls.TextBox tb_uk4;
        internal Avalonia.Controls.TextBox tb_uk6;

        // ── Subset tab ──────────────────────────────────────────────────────────
        internal Avalonia.Controls.ListBox lb_subsets;
        internal Avalonia.Controls.ListBox lb_sub_items;
        internal Avalonia.Controls.ListBox lb_sub_faces;

        // ── Model tab ───────────────────────────────────────────────────────────
        internal Avalonia.Controls.ListBox lb_model_trans;
        internal Avalonia.Controls.ListBox lb_model_names;
        internal Avalonia.Controls.ListBox lb_model_faces;
        internal Avalonia.Controls.ListBox lb_model_items;

        // ── Advanced tab ────────────────────────────────────────────────────────
        internal Avalonia.Controls.CheckBox cbCorrect;
        internal Avalonia.Controls.ComboBox cbaxis;

        // ── Mesh tab buttons ────────────────────────────────────────────────────
        private Button btnPreview;
        private Button btnExport;
        private Button btnImport;
        private Button btnBG;

        // ── 3D Preview ──────────────────────────────────────────────────────────
        private Ambertation.Graphics.DirectXPanel dxprev;
        private Ambertation.Graphics.RenderSelection scenesel;

        // ── static default axis index ────────────────────────────────────────────
        internal static int DefaultSelectedAxisIndex = -1;

        internal fGeometryDataContainer()
        {
            BuildControls();
            BuildLayout();
            PopulateDropdowns();
        }

        private void PopulateDropdowns()
        {
            // Populate axis order dropdown (XYZ, XZY — skip Preview)
            var sortings = (ElementSorting[])Enum.GetValues(typeof(ElementSorting));
            foreach (var es in sortings)
            {
                if (es == ElementSorting.Preview) continue;
                cbaxis.Items.Add(es);
                if (es == ElementSorting.XYZ) cbaxis.SelectedIndex = cbaxis.Items.Count - 1;
            }
            if (DefaultSelectedAxisIndex >= 0 && DefaultSelectedAxisIndex < cbaxis.Items.Count)
                cbaxis.SelectedIndex = DefaultSelectedAxisIndex;

            // Populate element identity dropdown
            var ids = (Gmdc.ElementIdentity[])Enum.GetValues(typeof(Gmdc.ElementIdentity));
            foreach (var e in ids) cbid.Items.Add(e);

            // Set correct joint checkbox from registry
            cbCorrect.IsChecked = SimPe.Helper.XmlRegistry.CorrectJointDefinitionOnExport;
        }

        private void BuildControls()
        {
            // TabItems
            tMesh                   = new TabItem { Header = "Mesh" };
            tGeometryDataContainer  = new TabItem { Header = "Elements" };
            tGeometryDataContainer2 = new TabItem { Header = "Links" };
            tGeometryDataContainer3 = new TabItem { Header = "Groups" };
            tModel                  = new TabItem { Header = "Model" };
            tSubset                 = new TabItem { Header = "Subsets" };
            tAdvncd                 = new TabItem { Header = "Advanced" };

            // Settings
            tb_ver = new TextBox { Text = "0x00000000" };
            tb_ver.TextChanged += SettingsChange;

            // Elements tab
            label_elements = new TextBlock { Text = "Elements:" };
            list_elements  = new ListBox();
            label_links    = new TextBlock { Text = "Links:" };
            list_links     = new ListBox();
            label_groups   = new TextBlock { Text = "Groups:" };
            list_groups    = new ListBox();
            label_subsets  = new TextBlock { Text = "Joints:" };
            list_subsets   = new ListBox();

            // Element detail
            lb_itemsa  = new ListBox(); lb_itemsa.SelectionChanged  += SelectItemsA;
            lb_itemsa2 = new ListBox(); lb_itemsa2.SelectionChanged += SelectItemsA2;
            tb_itemsa2 = new TextBox { Text = "0x00000000", IsReadOnly = true };
            tb_uk1     = new TextBox { IsReadOnly = true };
            tb_uk5     = new TextBox { IsReadOnly = true };
            tb_id      = new TextBox { IsReadOnly = true };
            tb_mod1    = new TextBox { IsReadOnly = true };
            tb_mod2    = new TextBox { IsReadOnly = true };
            lb_itemsa1 = new ListBox();
            cbblock    = new ComboBox();
            cbset      = new ComboBox();
            cbid       = new ComboBox();

            // Group tab
            lb_itemsc      = new ListBox(); lb_itemsc.SelectionChanged += SelectItemsC;
            tb_itemsc_name = new TextBox();
            tb_opacity     = new TextBox();
            tb_uk2         = new TextBox();
            tb_uk3         = new TextBox();
            lb_itemsc2     = new ListBox(); lb_itemsc2.SelectionChanged += SelectItemsC2;
            tb_itemsc2     = new TextBox { IsReadOnly = true };
            lb_itemsc3     = new ListBox(); lb_itemsc3.SelectionChanged += SelectItemsC3;
            tb_itemsc3     = new TextBox { IsReadOnly = true };
            cbGroupJoint   = new ComboBox();
            lbmodel        = new CheckedListBox();
            lb_models      = new TextBlock { Text = "Models:" };

            // Link tab
            lb_itemsb  = new ListBox(); lb_itemsb.SelectionChanged  += SelectItemsB;
            lb_itemsb2 = new ListBox(); lb_itemsb2.SelectionChanged += SelectItemsB2;
            lb_itemsb3 = new ListBox(); lb_itemsb3.SelectionChanged += SelectItemsB3;
            lb_itemsb4 = new ListBox(); lb_itemsb4.SelectionChanged += SelectItemsB4;
            lb_itemsb5 = new ListBox(); lb_itemsb5.SelectionChanged += SelectItemsB5;
            tb_itemsb2 = new TextBox { IsReadOnly = true };
            tb_itemsb3 = new TextBox { IsReadOnly = true };
            tb_itemsb4 = new TextBox { IsReadOnly = true };
            tb_itemsb5 = new TextBox { IsReadOnly = true };
            tb_uk4     = new TextBox();
            tb_uk6     = new TextBox();

            // Subset tab
            lb_subsets    = new ListBox(); lb_subsets.SelectionChanged    += SelectSubset;
            lb_sub_items  = new ListBox();
            lb_sub_faces  = new ListBox();

            // Model tab
            lb_model_trans = new ListBox();
            lb_model_names = new ListBox();
            lb_model_faces = new ListBox();
            lb_model_items = new ListBox();

            // Advanced tab
            cbCorrect = new CheckBox { Content = "Correct Joint Definition on Export" };
            cbCorrect.IsCheckedChanged += cbCorrect_CheckedChanged;
            cbaxis = new ComboBox();
            cbaxis.SelectionChanged += cbaxis_SelectionChanged;

            // 3D Preview
            dxprev = new Ambertation.Graphics.DirectXPanel();
            dxprev.Settings.RenderJoints = false;
            scenesel = new Ambertation.Graphics.RenderSelection();
            scenesel.DirectXPanel = dxprev;
        }

        // Helper: creates a labeled GroupBox with a border
        private static Border MakeGroupBox(string title, Avalonia.Controls.Control child)
        {
            var header = new TextBlock { Text = title, FontWeight = Avalonia.Media.FontWeight.Bold, Margin = new Avalonia.Thickness(4, 0) };
            var inner = new StackPanel { Spacing = 4 };
            inner.Children.Add(header);
            if (child != null) inner.Children.Add(child);
            return new Border
            {
                BorderBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(180, 180, 180)),
                BorderThickness = new Avalonia.Thickness(1),
                Padding = new Avalonia.Thickness(4),
                Margin = new Avalonia.Thickness(2),
                Child = inner
            };
        }

        // Helper: label + textbox row
        private static StackPanel LabeledValue(string label, TextBox tb)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
            row.Children.Add(new TextBlock { Text = label, VerticalAlignment = VerticalAlignment.Center });
            row.Children.Add(tb);
            return row;
        }

        private void BuildLayout()
        {
            // ══════════════════════════════════════════════════════════════════════
            // ── tMesh (3D Mesh) ──────────────────────────────────────────────────
            // ══════════════════════════════════════════════════════════════════════
            btnExport  = new Button { Content = "Export...",  MinWidth = 78 };
            btnImport  = new Button { Content = "Import...",  MinWidth = 78 };
            btnPreview = new Button { Content = "Preview",    MinWidth = 172 };
            btnBG      = new Button { Content = "BG",         MinWidth = 32 };
            btnPreview.Click += Preview;

            lb_models.FontWeight = Avalonia.Media.FontWeight.Bold;

            // Left: Models checklist + buttons
            var meshLeft = new DockPanel { MinWidth = 380 };
            DockPanel.SetDock(lb_models, Dock.Top);
            lbmodel.MinHeight = 150;
            DockPanel.SetDock(lbmodel, Dock.Top);

            cbaxis.MinWidth = 80;
            var exportImportRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4, Margin = new Avalonia.Thickness(0, 4) };
            exportImportRow.Children.Add(btnExport);
            exportImportRow.Children.Add(btnImport);
            exportImportRow.Children.Add(new TextBlock { Text = "Order:", FontWeight = Avalonia.Media.FontWeight.Bold, VerticalAlignment = VerticalAlignment.Center, Margin = new Avalonia.Thickness(8, 0, 0, 0) });
            exportImportRow.Children.Add(cbaxis);
            DockPanel.SetDock(exportImportRow, Dock.Top);

            var previewRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4, Margin = new Avalonia.Thickness(0, 4) };
            previewRow.Children.Add(btnPreview);
            previewRow.Children.Add(btnBG);
            DockPanel.SetDock(previewRow, Dock.Top);

            DockPanel.SetDock(cbCorrect, Dock.Top);
            cbCorrect.Margin = new Avalonia.Thickness(0, 4);

            meshLeft.Children.Add(lb_models);
            meshLeft.Children.Add(lbmodel);
            meshLeft.Children.Add(exportImportRow);
            meshLeft.Children.Add(previewRow);
            meshLeft.Children.Add(cbCorrect);

            // Center: 3D OpenGL preview wrapped in a border
            var dxBorder = new Border
            {
                BorderBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(180, 180, 180)),
                BorderThickness = new Avalonia.Thickness(1),
                Child = dxprev,
                ClipToBounds = true
            };

            // Right: Joint/bone selection list
            scenesel.MinWidth = 200;

            var meshGrid = new Grid { Margin = new Avalonia.Thickness(4) };
            meshGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(440, GridUnitType.Pixel)));
            meshGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(4, GridUnitType.Pixel)));
            meshGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            meshGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(4, GridUnitType.Pixel)));
            meshGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(230, GridUnitType.Pixel)));
            Grid.SetColumn(meshLeft, 0);
            Grid.SetColumn(dxBorder, 2);
            Grid.SetColumn(scenesel, 4);
            meshGrid.Children.Add(meshLeft);
            meshGrid.Children.Add(dxBorder);
            meshGrid.Children.Add(scenesel);
            tMesh.Content = meshGrid;

            // ══════════════════════════════════════════════════════════════════════
            // ── tGeometryDataContainer (Elements) ────────────────────────────────
            // ══════════════════════════════════════════════════════════════════════
            // Left: Element Section with main list + detail fields
            var elemLeftContent = new Grid();
            elemLeftContent.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(250, GridUnitType.Pixel)));
            elemLeftContent.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            Grid.SetColumn(lb_itemsa, 0); lb_itemsa.MinHeight = 150;
            var elemFields = new StackPanel { Spacing = 4 };
            elemFields.Children.Add(LabeledValue("Identity:", tb_id));
            elemFields.Children.Add(cbid);
            elemFields.Children.Add(LabeledValue("Block Format:", tb_mod1));
            elemFields.Children.Add(cbblock);
            elemFields.Children.Add(LabeledValue("Set Format:", tb_mod2));
            elemFields.Children.Add(cbset);
            elemFields.Children.Add(LabeledValue("Group UID:", tb_uk5));
            Grid.SetColumn(elemFields, 1);
            elemLeftContent.Children.Add(lb_itemsa);
            elemLeftContent.Children.Add(elemFields);

            // Right: Items + Values
            var elemItemsBox = MakeGroupBox("Element Section - Items", null);
            var elemItemsInner = (StackPanel)((Border)elemItemsBox).Child;
            lb_itemsa2.MinHeight = 70;
            elemItemsInner.Children.Add(lb_itemsa2);
            elemItemsInner.Children.Add(LabeledValue("Value:", tb_itemsa2));

            var elemValuesBox = MakeGroupBox("Element Section - Values", lb_itemsa1);
            lb_itemsa1.MinHeight = 100;

            var elemRight = new StackPanel { Spacing = 4, MinWidth = 250 };
            elemRight.Children.Add(elemItemsBox);
            elemRight.Children.Add(elemValuesBox);

            // Version box at top
            var verRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4, Margin = new Avalonia.Thickness(0, 0, 0, 4) };
            verRow.Children.Add(new TextBlock { Text = "Version:", VerticalAlignment = VerticalAlignment.Center });
            verRow.Children.Add(tb_ver);

            var elemGrid = new Grid();
            elemGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            elemGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            elemGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            elemGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(260, GridUnitType.Pixel)));
            Grid.SetRow(verRow, 0); Grid.SetColumnSpan(verRow, 2);
            Grid.SetRow(elemLeftContent, 1); Grid.SetColumn(elemLeftContent, 0);
            Grid.SetRow(elemRight, 1); Grid.SetColumn(elemRight, 1);
            elemGrid.Children.Add(verRow);
            elemGrid.Children.Add(elemLeftContent);
            elemGrid.Children.Add(elemRight);
            tGeometryDataContainer.Content = elemGrid;

            // ══════════════════════════════════════════════════════════════════════
            // ── tGeometryDataContainer2 (Links) ─────────────────────────────────
            // ══════════════════════════════════════════════════════════════════════
            var linkLeftContent = new StackPanel { Spacing = 4 };
            lb_itemsb.MinHeight = 200;
            linkLeftContent.Children.Add(lb_itemsb);
            linkLeftContent.Children.Add(LabeledValue("Referenced Size:", tb_uk4));
            linkLeftContent.Children.Add(LabeledValue("Active Elements:", tb_uk6));

            var linkElemRef = MakeGroupBox("Link Section - Elements Ref.", null);
            var linkElemRefInner = (StackPanel)((Border)linkElemRef).Child;
            lb_itemsb2.MinHeight = 70;
            linkElemRefInner.Children.Add(lb_itemsb2);
            linkElemRefInner.Children.Add(LabeledValue("Value:", tb_itemsb2));

            var linkVertAlias = MakeGroupBox("Link Section - Vertex Alias", null);
            var linkVertInner = (StackPanel)((Border)linkVertAlias).Child;
            lb_itemsb3.MinHeight = 70;
            linkVertInner.Children.Add(lb_itemsb3);
            linkVertInner.Children.Add(LabeledValue("Value:", tb_itemsb3));

            var linkNormAlias = MakeGroupBox("Link Section - Normal Alias", null);
            var linkNormInner = (StackPanel)((Border)linkNormAlias).Child;
            lb_itemsb4.MinHeight = 70;
            linkNormInner.Children.Add(lb_itemsb4);
            linkNormInner.Children.Add(LabeledValue("Value:", tb_itemsb4));

            var linkUVAlias = MakeGroupBox("Link Section - UVCoord. Alias", null);
            var linkUVInner = (StackPanel)((Border)linkUVAlias).Child;
            lb_itemsb5.MinHeight = 70;
            linkUVInner.Children.Add(lb_itemsb5);
            linkUVInner.Children.Add(LabeledValue("Value:", tb_itemsb5));

            var linkGrid = new Grid();
            linkGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            linkGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(240, GridUnitType.Pixel)));
            linkGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(240, GridUnitType.Pixel)));
            linkGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            linkGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            Grid.SetColumn(linkLeftContent, 0); Grid.SetRowSpan(linkLeftContent, 2);
            Grid.SetColumn(linkElemRef, 1); Grid.SetRow(linkElemRef, 0);
            Grid.SetColumn(linkVertAlias, 1); Grid.SetRow(linkVertAlias, 1);
            Grid.SetColumn(linkNormAlias, 2); Grid.SetRow(linkNormAlias, 0);
            Grid.SetColumn(linkUVAlias, 2); Grid.SetRow(linkUVAlias, 1);
            linkGrid.Children.Add(linkLeftContent);
            linkGrid.Children.Add(linkElemRef);
            linkGrid.Children.Add(linkVertAlias);
            linkGrid.Children.Add(linkNormAlias);
            linkGrid.Children.Add(linkUVAlias);
            tGeometryDataContainer2.Content = linkGrid;

            // ══════════════════════════════════════════════════════════════════════
            // ── tGeometryDataContainer3 (Groups) ────────────────────────────────
            // ══════════════════════════════════════════════════════════════════════
            var grpLeftContent = new Grid();
            grpLeftContent.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(250, GridUnitType.Pixel)));
            grpLeftContent.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            Grid.SetColumn(lb_itemsc, 0); lb_itemsc.MinHeight = 200;
            var grpFields = new StackPanel { Spacing = 4 };
            grpFields.Children.Add(LabeledValue("Prim. Type:", tb_uk2));
            grpFields.Children.Add(LabeledValue("Link Ref:", tb_uk3));
            grpFields.Children.Add(LabeledValue("Opacity:", tb_opacity));
            grpFields.Children.Add(LabeledValue("Name:", tb_itemsc_name));
            grpFields.Children.Add(new TextBlock { Text = "Joints:", FontWeight = Avalonia.Media.FontWeight.Bold });
            grpFields.Children.Add(cbGroupJoint);
            Grid.SetColumn(grpFields, 1);
            grpLeftContent.Children.Add(lb_itemsc);
            grpLeftContent.Children.Add(grpFields);

            var grpFacesBox = MakeGroupBox("Group Section - Faces", null);
            var grpFacesInner = (StackPanel)((Border)grpFacesBox).Child;
            lb_itemsc2.MinHeight = 70;
            grpFacesInner.Children.Add(lb_itemsc2);
            grpFacesInner.Children.Add(LabeledValue("Value:", tb_itemsc2));

            var grpJointsBox = MakeGroupBox("Group Section - Used Joints", null);
            var grpJointsInner = (StackPanel)((Border)grpJointsBox).Child;
            lb_itemsc3.MinHeight = 70;
            grpJointsInner.Children.Add(lb_itemsc3);
            grpJointsInner.Children.Add(LabeledValue("Value:", tb_itemsc3));

            var grpRight = new StackPanel { Spacing = 4, MinWidth = 250 };
            grpRight.Children.Add(grpFacesBox);
            grpRight.Children.Add(grpJointsBox);

            var grpGrid = new Grid();
            grpGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            grpGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(260, GridUnitType.Pixel)));
            Grid.SetColumn(grpLeftContent, 0);
            Grid.SetColumn(grpRight, 1);
            grpGrid.Children.Add(grpLeftContent);
            grpGrid.Children.Add(grpRight);
            tGeometryDataContainer3.Content = grpGrid;

            // ══════════════════════════════════════════════════════════════════════
            // ── tModel ──────────────────────────────────────────────────────────
            // ══════════════════════════════════════════════════════════════════════
            var modelTransBox = MakeGroupBox("Model Section - Transformations", lb_model_trans);
            lb_model_trans.MinHeight = 130;
            var modelVertBox = MakeGroupBox("Bounding Mesh - Vertices", lb_model_faces);
            lb_model_faces.MinHeight = 130;
            var modelNamesBox = MakeGroupBox("Model Section - Names", lb_model_names);
            lb_model_names.MinHeight = 50;
            var modelFacesBox = MakeGroupBox("Bounding Mesh - Faces", lb_model_items);
            lb_model_items.MinHeight = 50;

            var modelGrid = new Grid();
            modelGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));
            modelGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            modelGrid.RowDefinitions.Add(new RowDefinition(new GridLength(2, GridUnitType.Star)));
            modelGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            Grid.SetColumn(modelTransBox, 0); Grid.SetRow(modelTransBox, 0);
            Grid.SetColumn(modelVertBox, 1);  Grid.SetRow(modelVertBox, 0);
            Grid.SetColumn(modelNamesBox, 0); Grid.SetRow(modelNamesBox, 1);
            Grid.SetColumn(modelFacesBox, 1); Grid.SetRow(modelFacesBox, 1);
            modelGrid.Children.Add(modelTransBox);
            modelGrid.Children.Add(modelVertBox);
            modelGrid.Children.Add(modelNamesBox);
            modelGrid.Children.Add(modelFacesBox);
            tModel.Content = modelGrid;

            // ══════════════════════════════════════════════════════════════════════
            // ── tSubset (Joints) ────────────────────────────────────────────────
            // ══════════════════════════════════════════════════════════════════════
            var subLeftBox = MakeGroupBox("Joints Section", lb_subsets);
            lb_subsets.MinHeight = 200;
            var subVertBox = MakeGroupBox("Joints Section - Vertices", lb_sub_faces);
            lb_sub_faces.MinHeight = 100;
            var subItemsBox = MakeGroupBox("Joints Section - Items", lb_sub_items);
            lb_sub_items.MinHeight = 80;

            var subRight = new StackPanel { Spacing = 4, MinWidth = 260 };
            subRight.Children.Add(subVertBox);
            subRight.Children.Add(subItemsBox);

            var subGrid = new Grid();
            subGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            subGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(270, GridUnitType.Pixel)));
            Grid.SetColumn(subLeftBox, 0);
            Grid.SetColumn(subRight, 1);
            subGrid.Children.Add(subLeftBox);
            subGrid.Children.Add(subRight);
            tSubset.Content = subGrid;

            // ══════════════════════════════════════════════════════════════════════
            // ── tAdvncd (Advanced) ──────────────────────────────────────────────
            // ══════════════════════════════════════════════════════════════════════
            var advLeft = new StackPanel { Spacing = 4, MinWidth = 270 };
            advLeft.Children.Add(label_elements); advLeft.Children.Add(list_elements);
            list_elements.MinHeight = 80;
            advLeft.Children.Add(label_links);    advLeft.Children.Add(list_links);
            list_links.MinHeight = 60;
            advLeft.Children.Add(label_groups);   advLeft.Children.Add(list_groups);
            list_groups.MinHeight = 60;

            var advMid = new StackPanel { Spacing = 4, MinWidth = 270 };
            advMid.Children.Add(label_subsets);   advMid.Children.Add(list_subsets);
            list_subsets.MinHeight = 80;

            var advGrid = new Grid();
            advGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(280, GridUnitType.Pixel)));
            advGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(280, GridUnitType.Pixel)));
            advGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            Grid.SetColumn(advLeft, 0);
            Grid.SetColumn(advMid, 1);
            // Right column: placeholder for PropertyGrid (not available in Avalonia)
            var advRightPlaceholder = new Border
            {
                BorderBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(180, 180, 180)),
                BorderThickness = new Avalonia.Thickness(1),
                Margin = new Avalonia.Thickness(4),
                Child = new TextBlock { Text = "Properties", Margin = new Avalonia.Thickness(4) }
            };
            Grid.SetColumn(advRightPlaceholder, 2);
            advGrid.Children.Add(advLeft);
            advGrid.Children.Add(advMid);
            advGrid.Children.Add(advRightPlaceholder);
            tAdvncd.Content = advGrid;

            // ══════════════════════════════════════════════════════════════════════
            // ── Outer TabControl ─────────────────────────────────────────────────
            // ══════════════════════════════════════════════════════════════════════
            // Tab order matches Windows version
            var tc = new TabControl();
            tc.Items.Add(tGeometryDataContainer);  // Elements
            tc.Items.Add(tGeometryDataContainer2); // Links
            tc.Items.Add(tGeometryDataContainer3); // Groups
            tc.Items.Add(tMesh);                   // 3D Mesh
            tc.Items.Add(tModel);                  // Model
            tc.Items.Add(tSubset);                 // Joints
            tc.Items.Add(tAdvncd);                 // Advanced
            Content = tc;
        }

        // ── Public methods called by cGeometryDataContainer ─────────────────────

        internal void ResetPreview()
        {
            this.scenesel.Scene = null;
        }

        private void Preview(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("[Preview] tMesh.Tag = " + (tMesh.Tag?.GetType().Name ?? "null"));
            if (tMesh.Tag == null) return;
            var gmdc = (GeometryDataContainer)this.tMesh.Tag;
            try
            {
                var models = GetModelsExt();
                System.Diagnostics.Debug.WriteLine("[Preview] models.Count = " + models.Count);
                var gmdcext = new GeometryDataContainerExt(gmdc);
                if (this.scenesel.Scene != null) this.scenesel.Scene.Dispose();
                this.scenesel.Scene = gmdcext.GetScene(models, new ElementOrder(ElementSorting.Preview));
                // Re-set aspect and re-render now that layout has occurred
                if (dxprev.Bounds.Width > 0 && dxprev.Bounds.Height > 0)
                {
                    dxprev.Settings.Aspect = (float)dxprev.Bounds.Width / (float)dxprev.Bounds.Height;
                    dxprev.Render();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[Preview] ERROR: " + ex);
            }
        }

        private GmdcGroups GetModelsExt()
        {
            var list = new GmdcGroups();
            for (int i = 0; i < lbmodel.CheckedItems.Count; i++)
                list.Add((GmdcGroup)lbmodel.CheckedItems[i]);
            return list;
        }

        // ── Event handlers (no-op stubs — business logic is in cGeometryDataContainer) ──

        private void SettingsChange(object sender, Avalonia.Controls.TextChangedEventArgs e) { }
        private void SelectItemsA(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsA2(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsB(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsB2(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsB3(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsB4(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsB5(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsC(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsC2(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsC3(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectSubset(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void cbCorrect_CheckedChanged(object sender, Avalonia.Interactivity.RoutedEventArgs e) { }
        private void cbaxis_SelectionChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            if (cbaxis.SelectedIndex >= 0) DefaultSelectedAxisIndex = cbaxis.SelectedIndex;
        }

        // ── Static export/import stubs ───────────────────────────────────────────
        // Called from AnimMeshBlockControl. File dialogs are no-ops in Avalonia port.
        internal static void StartExport(GeometryDataContainer gdc, string ext,
            GmdcGroups groups, SimPe.Plugin.Gmdc.ElementSorting sorting, bool correct)
        {
            // TODO: implement async file dialog when Avalonia StorageProvider is wired up
            System.Diagnostics.Trace.TraceInformation("[fGeometryDataContainer] StartExport (stub)");
        }

        internal static void StartImport(GeometryDataContainer gdc, string ext,
            SimPe.Plugin.Gmdc.ElementSorting sorting, bool correct)
        {
            // TODO: implement async file dialog when Avalonia StorageProvider is wired up
            System.Diagnostics.Trace.TraceInformation("[fGeometryDataContainer] StartImport (stub)");
        }

        // ── IDisposable ──────────────────────────────────────────────────────────
        public void Dispose() { }
    }
}
