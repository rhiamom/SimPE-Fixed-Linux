// WorkSpaceHelper.PortStubs.cs
// Stub implementations for WinForms/docking-dependent classes that cannot yet be
// ported to Avalonia. These provide the API shapes callers in SimPE.Main need so
// the project compiles. Actual functionality is no-op or minimal.

using System;
using System.Collections;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using SimPe.Interfaces;
using SimPe.Interfaces.Plugin;
using SimPe.Events;

namespace SimPe
{
    // ── PackageArg ─────────────────────────────────────────────────────────────

    public class PackageArg : System.EventArgs
    {
        public SimPe.Interfaces.Files.IPackageFile Package { get; set; }
        public SimPe.Interfaces.Files.IPackedFileDescriptor FileDescriptor { get; set; }
        public SimPe.Interfaces.Plugin.IToolResult Result { get; set; }
    }

    // ── ResourceLoader ─────────────────────────────────────────────────────────

    /// <summary>
    /// Stub ResourceLoader — Avalonia docking port pending.
    /// Preserves the API shape so callers compile; actual resource display
    /// is handled by the MainWindow Avalonia UI.
    /// </summary>
    public class ResourceLoader
    {
        public ResourceLoader(DockPanel dc, LoadedPackage lp) { }

        public static void Refresh() { }
        public static void Refresh(SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem fii) { }

        public bool AddResource(SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem fii, bool overload) => false;
        public bool AddResource(SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem fii, bool reload, bool overload) => false;

        public bool SelectResource(SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem fii) => false;
        public DockContent GetDocument(SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem fii) => null;
        public DockContent GetDocument(SimPe.Interfaces.Files.IPackedFileDescriptor pfd) => null;

        public bool CloseDocument(DockContent doc) => true;
        public bool Clear() => true;
        public bool Flush() => true;
    }

    // ── ToolMenuItemExt ────────────────────────────────────────────────────────

    /// <summary>
    /// Stub ToolMenuItemExt — Avalonia menu port pending.
    /// </summary>
    public class ToolMenuItemExt : ToolStripMenuItem
    {
        public delegate void ExternalToolNotify(object sender, PackageArg pk);

        public IToolExt ToolExt => tool is IToolExt t ? t : null;
        public ITool Tool => tool is ITool t ? t : null;
        public IToolPlus ToolPlus => tool is IToolPlus t ? t : null;

        IToolPlugin tool;
        ExternalToolNotify chghandler;
        SimPe.Interfaces.Files.IPackedFileDescriptor pfd;
        SimPe.Interfaces.Files.IPackageFile package;

        public ToolMenuItemExt(IToolPlus tool, ExternalToolNotify chghnd)
            : this(tool.ToString(), tool, chghnd) { }

        public ToolMenuItemExt(string text, IToolPlugin tool, ExternalToolNotify chghnd)
        {
            this.tool = tool;
            this.Text = text;
            this.chghandler = chghnd;
        }

        public SimPe.Interfaces.Files.IPackedFileDescriptor FileDescriptor
        {
            get { return pfd; }
            set { pfd = value; }
        }

        public SimPe.Interfaces.Files.IPackageFile Package
        {
            get { return package; }
            set { package = value; }
        }

        internal void ChangeEnabledStateEventHandler(object sender, SimPe.Events.ResourceEventArgs e)
        {
            // no-op stub
        }

        public override string ToString()
        {
            try { return tool.ToString(); }
            catch { return "Plugin Error"; }
        }
    }

    // LoadFileWrappers is defined in SimPE.GMDCExporterbase (LoadFileWrappersStub.cs).
    // LoadFileWrappersExt is a stub here — does not extend LoadFileWrappers directly
    // to avoid a circular dependency (WorkSpaceHelper cannot reference GMDCExporterbase).

    /// <summary>
    /// Stub LoadFileWrappersExt — dynamic plugin loading port pending.
    /// </summary>
    public class LoadFileWrappersExt
    {
        public LoadFileWrappersExt() { }

        public void AddListeners(ref SimPe.Events.ChangedResourceEvent ev) { }
    }

    // ── LoadHelpTopics ─────────────────────────────────────────────────────────

    /// <summary>
    /// Stub LoadHelpTopics — help menu port pending.
    /// </summary>
    public class LoadHelpTopics
    {
        public LoadHelpTopics(System.Windows.Forms.ToolStripMenuItem parentmenu) { }
    }
}
