// WeifenLuoStubs.cs — Avalonia TabControl replacement for WeifenLuo.WinFormsUI.Docking.
//
// Original SimPE bottom panel (Package / Resource / Wrapper / Converter / Hex / etc. tabs)
// was a WeifenLuo DockPanel in Document mode.  We replace it with a plain Avalonia
// TabControl.  The WeifenLuo type names are preserved so no callers need changing.

using System;
using Avalonia.Controls;

namespace WeifenLuo.WinFormsUI.Docking
{
    public enum DocumentStyle { DockingMdi, DockingWindow, DockingSdi, SystemMdi }
    public interface IDockContent { }
    public class ThemeBase { }
    public class VS2015LightTheme : ThemeBase { }
    public class VS2015BlueTheme : ThemeBase { }
    public class VS2015DarkTheme : ThemeBase { }

    // ── ControlCollection stub (used by ResourceLoader cleanup code) ─────────

    public class ControlCollection
    {
        public int Count => 0;
        public void Clear() { }
        public System.Collections.IEnumerator GetEnumerator() => Array.Empty<object>().GetEnumerator();
    }

    // ── DockContent — wraps an Avalonia TabItem ──────────────────────────────

    /// <summary>
    /// Replaces WeifenLuo DockContent.  Each instance is one tab in the
    /// bottom TabControl.  Callers (ResourceLoader etc.) use the same API.
    /// </summary>
    public class DockContent : IDockContent
    {
        // The real Avalonia tab item for this document.
        public TabItem TabItem { get; } = new TabItem();

        // Back-reference to the owning DockPanel (set by Show).
        public DockPanel DockPanel { get; private set; }

        // ── API used by ResourceLoader ──────────────────────────────────────

        public DockState  DockState  { get; set; } = DockState.Document;
        public DockAreas  DockAreas  { get; set; } = DockAreas.Document;
        public bool       CloseButton { get; set; } = true;

        public string Text
        {
            get => TabItem.Header?.ToString() ?? "";
            set => TabItem.Header = value;
        }

        public object Tag
        {
            get => TabItem.Tag;
            set => TabItem.Tag = value;
        }

        public System.Drawing.Rectangle ClientRectangle =>
            new System.Drawing.Rectangle(0, 0, 800, 600);

        // DockPaneHandler stub — Form is used to parent child controls.
        public DockPaneHandler DockHandler { get; } = new DockPaneHandler();

        // Controls stub (for cleanup code in ResourceLoader).
        public ControlCollection Controls { get; } = new ControlCollection();

        public event System.Windows.Forms.FormClosingEventHandler FormClosing;

        public void Show(DockPanel dp, DockState state)
        {
            DockPanel = dp;
            DockState = state;
            TabItem.Tag = this;   // allow DockPanel to walk back to DockContent
            if (!dp.TabControl.Items.Contains(TabItem))
                dp.TabControl.Items.Add(TabItem);
            dp.TabControl.SelectedItem = TabItem;
        }

        public void Show(DockPanel dp) => Show(dp, DockState.Document);

        public void Activate()
        {
            if (DockPanel != null)
                DockPanel.TabControl.SelectedItem = TabItem;
        }

        public void Close()
        {
            DockPanel?.TabControl.Items.Remove(TabItem);
            DockPanel = null;
            DockState = DockState.Hidden;
            FormClosing?.Invoke(this, new System.Windows.Forms.FormClosingEventArgs());
        }
    }

    // ── DockPanel — wraps an Avalonia TabControl ─────────────────────────────

    /// <summary>
    /// Replaces WeifenLuo DockPanel.  The inner <see cref="TabControl"/> is the
    /// real Avalonia control that must be placed in the visual tree by MainForm.
    /// Access it via <c>dc.TabControl</c> when building the Avalonia layout.
    /// </summary>
    public class DockPanel
    {
        /// <summary>The underlying Avalonia TabControl — wire this into the Window layout.</summary>
        public TabControl TabControl { get; } = new TabControl();

        public DocumentStyle DocumentStyle { get; set; }
        public ThemeBase Theme { get; set; }

        /// <summary>Returns the DockContent whose TabItem is currently selected.</summary>
        public IDockContent ActiveDocument
        {
            get
            {
                if (TabControl.SelectedItem is TabItem ti && ti.Tag is DockContent dc)
                    return dc;
                return null;
            }
        }

        public IDockContent[] DocumentsToArray()
        {
            var list = new System.Collections.Generic.List<IDockContent>();
            foreach (object item in TabControl.Items)
                if (item is TabItem ti && ti.Tag is DockContent dc)
                    list.Add(dc);
            return list.ToArray();
        }
    }

    // ── DockPaneHandler stub ─────────────────────────────────────────────────

    public class DockPaneHandler
    {
        public bool       CloseButton { get; set; } = true;
        public DockState  DockState   { get; set; } = DockState.Document;
        public DockPane   Pane        { get; }
        public DockPanel  DockPanel   { get; }
        /// <summary>Form is the DockContent itself (used to parent plugin controls).</summary>
        public DockContent Form       { get; }
    }

    public class DockPane { }

    public enum DockState
    {
        Unknown, Float, DockTopAutoHide, DockLeftAutoHide,
        DockBottomAutoHide, DockRightAutoHide,
        Document, DockTop, DockLeft, DockBottom, DockRight, Hidden
    }

    [Flags]
    public enum DockAreas
    {
        None = 0, Float = 1, DockLeft = 2, DockRight = 4,
        DockTop = 8, DockBottom = 16, Document = 32
    }
}
