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
using System.Drawing;
using System.Collections;
using SimPe;
using SimPe.Interfaces.Plugin.Scanner;
using SimPe.Plugin.Scanner;

namespace SimPe.Plugin
{
    /// <summary>
    /// Summary description for ScannerForm.
    /// </summary>
    internal class ScannerForm : Avalonia.Controls.Window
    {
        private Avalonia.Controls.Button btclear;
        private Avalonia.Controls.TabItem tbcache;
        private Avalonia.Controls.Button button2;
        private Avalonia.Controls.Button button3;
        private Avalonia.Controls.Button btscan;
        private Avalonia.Controls.TabItem tbidentify;
        private Avalonia.Controls.TextBlock label5;
        private Avalonia.Controls.ListBox lbid;
        private Avalonia.Controls.ListBox lbscandebug;
        private Avalonia.Controls.TextBlock label6;
        private Avalonia.Controls.TextBox tbflname;
        private Avalonia.Controls.ComboBox lbprop;
        private Avalonia.Controls.Button llSave;
        private SaveFileDialogStub sfd;
        private Avalonia.Controls.CheckBox cbrec;
        private Avalonia.Controls.Button linkLabel1;
        private Avalonia.Controls.ComboBox cbfolder;
        private FolderBrowserDialogStub fbd;
        private Avalonia.Controls.ProgressBar pb;
        private SimPe.Scenegraph.Compat.ListView lv;
        private SimPe.Scenegraph.Compat.ColumnHeader columnHeader1;
        private SimPe.Scenegraph.Compat.ColumnHeader columnHeader2;
        private SimPe.Scenegraph.Compat.ColumnHeader columnHeader3;
        private Avalonia.Controls.TabControl tabControl1;
        private Avalonia.Controls.TabItem tbscanners;
        private SimPe.Scenegraph.Compat.CheckedListBox lbscanners;
        private Avalonia.Controls.TextBlock label1;
        private SimPe.Scenegraph.Compat.ImageList ilist;
        private SimPe.Scenegraph.Compat.PictureBoxCompat thumb;
        private SimPe.Scenegraph.Compat.GroupBox gbinfo;
        private Avalonia.Controls.Button llopen;
        private Avalonia.Controls.CheckBox cbenable;
        private Avalonia.Controls.TextBlock lbname;
        private Avalonia.Controls.TextBlock lbtype;
        private Avalonia.Controls.TabItem tboperations;
        private Avalonia.Controls.StackPanel pnop;

        /// <summary>
        /// Create a new Instance
        /// </summary>
        public ScannerForm()
        {
            BuildLayout();
            scanClicked = Scan;

            //hide the Identifier Tab in non Creator Mode
            if (!UserVerification.HaveUserId)
            {
                this.tabControl1.Items.Remove(this.tbidentify);
            }

            //load the Group Cache
            SimPe.Plugin.ScenegraphWrapperFactory.LoadGroupCache();

            this.cbfolder.SelectedIndex = 0;

            cachefile = new SimPe.Cache.PackageCacheFile();
            try
            {
                cachefile.Load(SimPe.Cache.PackageCacheFile.CacheFileName);
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage("Unable to reload the Cache File.", ex);
            }

            //display the list of identifiers
            foreach (IIdentifier id in ScannerRegistry.Global.Identifiers)
            {
                lbid.Items.Add(id.GetType().Name + " (version=" + id.Version.ToString() + ", index=" + id.Index.ToString() + ")");
            }

            //add the scanners to the Selection and show the Scanner Controls (if available)
            SimPe.Plugin.Scanner.AbstractScanner.UpdateList finishcallback = new SimPe.Plugin.Scanner.AbstractScanner.UpdateList(this.UpdateList);
            ArrayList uids = new ArrayList();
            foreach (IScanner i in ScannerRegistry.Global.Scanners)
            {
                string name = i.GetType().Name + " (version=" + i.Version.ToString() + ", uid=0x" + Helper.HexString(i.Uid) + ", index=" + i.Index.ToString() + ")";
                if (!uids.Contains(i.Uid))
                {
                    if (!i.OnTop) lbscanners.Items.Add(i, i.IsActiveByDefault);
                    else
                    {
                        lbscanners.Items.Insert(0, i);
                        lbscanners.SetItemChecked(0, i.IsActiveByDefault);
                    }
                    ShowControls(i);
                    i.SetFinishCallback(finishcallback);

                    uids.Add(i.Uid);
                }
                else
                {
                    name = "--- " + name;
                }

                this.lbscandebug.Items.Add(name);
            }

            pnop.IsEnabled = false;
            sorter = new ColumnSorter();
            lv.ListViewItemSorter = sorter;
        }

        SimPe.Cache.PackageCacheFile cachefile;

        string flname;
        string folder;
        string errorlog;
        bool cachechg;
        ColumnSorter sorter;
        int controltop = 0;
        ScannerItem lastitem;
        bool _settingCbenable = false;

        public string FileName
        {
            get { return flname; }
        }

        private void BuildLayout()
        {
            // Controls
            cbfolder = new Avalonia.Controls.ComboBox();
            cbfolder.Items.Add("Download Folder");
            cbfolder.Items.Add("Teleport Folder");
            cbfolder.Items.Add("Neighbourhoods Folder");
            cbfolder.Items.Add("Bodyshop Sim Templates Folder");
            cbfolder.Items.Add("Browse for Folder...");
            cbfolder.SelectionChanged += (s, e) => SelectFolder(s, EventArgs.Empty);

            fbd = new FolderBrowserDialogStub { ShowNewFolderButton = false };
            sfd = new SaveFileDialogStub { Filter = "Comma Seperated Values (*.csv)|*.csv|All Files (*.*)|*.*" };

            pb = new Avalonia.Controls.ProgressBar { Minimum = 0, Maximum = 1000, Value = 0 };

            lv = new SimPe.Scenegraph.Compat.ListView { FullRowSelect = true, HideSelection = false };
            columnHeader1 = new SimPe.Scenegraph.Compat.ColumnHeader { Text = "Filename", Width = 281 };
            columnHeader2 = new SimPe.Scenegraph.Compat.ColumnHeader { Text = "Enabled", Width = 57 };
            columnHeader3 = new SimPe.Scenegraph.Compat.ColumnHeader { Text = "Type", Width = 104 };
            lv.Columns.Add(columnHeader1);
            lv.Columns.Add(columnHeader2);
            lv.Columns.Add(columnHeader3);
            ilist = new SimPe.Scenegraph.Compat.ImageList { ImageSize = new System.Drawing.Size(48, 48) };
            lv.LargeImageList = ilist;
            lv.ColumnClick += (s, e) => SortList(s, EventArgs.Empty);
            lv.SelectedIndexChanged += (s, e) => SelectItem(s, EventArgs.Empty);

            lbscanners = new SimPe.Scenegraph.Compat.CheckedListBox { CheckOnClick = true, HorizontalScrollbar = true };
            label1 = new Avalonia.Controls.TextBlock { Text = "active Scanners:" };
            pnop = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Vertical };

            btclear = new Avalonia.Controls.Button { Content = "Clear Cache" };
            btclear.Click += (s, e) => ClearCache(s, EventArgs.Empty);
            button2 = new Avalonia.Controls.Button { Content = "Reload Cache" };
            button2.Click += (s, e) => ReloadCache(s, EventArgs.Empty);
            button3 = new Avalonia.Controls.Button { Content = "Reload FileTable" };
            button3.Click += (s, e) => ReloadFileTable(s, EventArgs.Empty);

            lbid = new Avalonia.Controls.ListBox();
            label5 = new Avalonia.Controls.TextBlock { Text = "loaded Identifiers:" };
            lbscandebug = new Avalonia.Controls.ListBox();
            label6 = new Avalonia.Controls.TextBlock { Text = "loaded Scanners:" };

            btscan = new Avalonia.Controls.Button { Content = "Scan" };
            btscan.Click += (s, e) => ScanButtonClicked(s, EventArgs.Empty);

            cbrec = new Avalonia.Controls.CheckBox { Content = "Recursive" };

            lbprop = new Avalonia.Controls.ComboBox();
            llSave = new Avalonia.Controls.Button { Content = "save...", IsEnabled = false };
            llSave.Click += (s, e) => llSave_LinkClicked(s, EventArgs.Empty);

            tbflname = new Avalonia.Controls.TextBox { IsReadOnly = true };

            cbenable = new Avalonia.Controls.CheckBox { Content = "Enabled" };
            cbenable.IsCheckedChanged += (s, e) => SetEnabledState(s, EventArgs.Empty);

            lbtype = new Avalonia.Controls.TextBlock { Text = "Type" };
            lbname = new Avalonia.Controls.TextBlock { Text = "Caption" };

            llopen = new Avalonia.Controls.Button { Content = "open" };
            llopen.Click += (s, e) => OpenPackage(s, EventArgs.Empty);

            thumb = new SimPe.Scenegraph.Compat.PictureBoxCompat();

            gbinfo = new SimPe.Scenegraph.Compat.GroupBox { Text = "Information" };

            linkLabel1 = new Avalonia.Controls.Button { Content = "scan" };
            linkLabel1.Click += (s, e) => Scan(s, EventArgs.Empty);

            // Tab items
            var scannerTabContent = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Vertical };
            scannerTabContent.Children.Add(label1);
            scannerTabContent.Children.Add(lbscanners);
            tbscanners = new Avalonia.Controls.TabItem { Header = "Scanner Settings", Content = scannerTabContent };

            tboperations = new Avalonia.Controls.TabItem { Header = "Operations", Content = pnop };

            var cacheTabContent = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Vertical };
            cacheTabContent.Children.Add(btclear);
            cacheTabContent.Children.Add(button2);
            cacheTabContent.Children.Add(button3);
            tbcache = new Avalonia.Controls.TabItem { Header = "Cache", Content = cacheTabContent };

            var identifyTabContent = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Vertical };
            identifyTabContent.Children.Add(label5);
            identifyTabContent.Children.Add(lbid);
            identifyTabContent.Children.Add(label6);
            identifyTabContent.Children.Add(lbscandebug);
            tbidentify = new Avalonia.Controls.TabItem { Header = "Scanners", Content = identifyTabContent };

            tabControl1 = new Avalonia.Controls.TabControl();
            tabControl1.Items.Add(tbscanners);
            tabControl1.Items.Add(tboperations);
            tabControl1.Items.Add(tbcache);
            tabControl1.Items.Add(tbidentify);

            // gbinfo layout
            var gbinfoStack = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Vertical };
            gbinfoStack.Children.Add(thumb);
            gbinfoStack.Children.Add(cbenable);
            gbinfoStack.Children.Add(lbname);
            gbinfoStack.Children.Add(lbtype);
            gbinfoStack.Children.Add(tbflname);
            gbinfoStack.Children.Add(lbprop);
            gbinfoStack.Children.Add(llSave);
            gbinfoStack.Children.Add(llopen);
            gbinfo.Content = gbinfoStack;

            // Root layout
            var topBar = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            topBar.Children.Add(cbfolder);
            topBar.Children.Add(linkLabel1);

            var bottomBar = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            bottomBar.Children.Add(btscan);
            bottomBar.Children.Add(cbrec);

            var root = new Avalonia.Controls.DockPanel { LastChildFill = true };
            Avalonia.Controls.DockPanel.SetDock(topBar, Avalonia.Controls.Dock.Top);
            Avalonia.Controls.DockPanel.SetDock(bottomBar, Avalonia.Controls.Dock.Bottom);
            Avalonia.Controls.DockPanel.SetDock(pb, Avalonia.Controls.Dock.Bottom);
            Avalonia.Controls.DockPanel.SetDock(tabControl1, Avalonia.Controls.Dock.Left);
            Avalonia.Controls.DockPanel.SetDock(gbinfo, Avalonia.Controls.Dock.Right);
            root.Children.Add(topBar);
            root.Children.Add(bottomBar);
            root.Children.Add(pb);
            root.Children.Add(tabControl1);
            root.Children.Add(gbinfo);
            root.Children.Add(lv);

            Title = "Folder Scanner";
            Width = 964;
            Height = 602;
            WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterOwner;
            Content = root;
        }

        /// <summary>
        /// Display a control on the Panel
        /// </summary>
        void ShowControl(Avalonia.Controls.Control ctrl, bool indent, bool space)
        {
            pnop.Children.Add(ctrl);
            if (indent)
                ctrl.Margin = new Avalonia.Thickness(16, 0, 0, space ? 8 : 0);
            else
                ctrl.Margin = new Avalonia.Thickness(0, 0, 0, space ? 8 : 0);
        }

        /// <summary>
        /// Display the Controls of a Scanner
        /// </summary>
        void ShowControls(IScanner scanner)
        {
            Avalonia.Controls.Control ctrl = scanner.OperationControl;
            if (ctrl == null) return;

            var lb = new Avalonia.Controls.TextBlock { Text = scanner.ToString() + ":" };

            ShowControl(lb, false, false);
            ShowControl(ctrl, true, true);
        }

        /// <summary>
        /// Returns the last selected Scanner Item (can be null)
        /// </summary>
        internal ScannerItem SelectedScannerItem
        {
            get { return lastitem; }
        }

        /// <summary>
        /// Displays the Information about this Scanner Item
        /// </summary>
        void ShowInfo(ScannerItem si, SimPe.Scenegraph.Compat.ListViewItem lvi)
        {
            if (si == null) return;

            _settingCbenable = true;
            try
            {
                this.thumb.Image = null; // si.PackageCacheItem.Thumbnail is System.Drawing.Image; thumb.Image is SKBitmap — skip
                this.cbenable.IsChecked = si.PackageCacheItem.Enabled;
                this.lbname.Text = si.PackageCacheItem.Name;
                this.lbtype.Text = si.PackageCacheItem.Type.ToString();

                tbflname.Text = si.FileName;

                lbprop.Items.Clear();
                if (System.IO.File.Exists(si.FileName))
                {
                    string mod = " Modification Date: ";
                    mod += System.IO.File.GetLastWriteTime(si.FileName).ToShortDateString() + " ";
                    mod += System.IO.File.GetLastWriteTime(si.FileName).ToLongTimeString();
                    lbprop.Items.Add(mod);
                }
                for (int i = 3; i < lv.Columns.Count; i++)
                {
                    if (lvi.SubItems[i].Text.Trim() != "")
                        lbprop.Items.Add(lv.Columns[i].Text + ": " + lvi.SubItems[i].Text);
                }
            }
            finally
            {
                _settingCbenable = false;
            }
        }

        private void Scan(string folder, bool rec, ScannerCollection usedscanners)
        {
            //scan all Files
            pb.Value = 0;
            string[] files = System.IO.Directory.GetFiles(folder, "*.package");
            string[] dfiles = System.IO.Directory.GetFiles(folder, "*.simpedis");
            string[] dofiles = System.IO.Directory.GetFiles(folder, "*.packagedisabled");
            string[] tfiles = System.IO.Directory.GetFiles(folder, "*.Sims2Tmp");

            int ct = files.Length + dfiles.Length + dofiles.Length + tfiles.Length;
            Scan(files, true, 0, ct, usedscanners);
            if (!stopClicked) Scan(dfiles, false, files.Length, ct, usedscanners);
            if (!stopClicked) Scan(dofiles, false, files.Length + dfiles.Length, ct, usedscanners);
            if (!stopClicked) Scan(tfiles, false, files.Length + dfiles.Length + dofiles.Length, ct, usedscanners);
            pb.Value = 0;

            //issue a recursive Scan
            if (!stopClicked && rec)
            {
                string[] dirs = System.IO.Directory.GetDirectories(folder, "*");
                foreach (string dir in dirs) { Scan(dir, true, usedscanners); if (stopClicked) break; }
            }
        }

        /// <summary>
        /// Scan for all Files and display the Result
        /// </summary>
        void Scan(string[] files, bool enabled, int pboffset, int count, ScannerCollection usedscanners)
        {
            int ct = pboffset;
            foreach (string file in files)
            {
                pb.Value = Math.Max(Math.Min(((ct++) * pb.Maximum) / count, pb.Maximum), pb.Minimum);
                try
                {
                    //Load the Item from the cache (if possible)
                    ScannerItem si = cachefile.LoadItem(file);
                    si.PackageCacheItem.Enabled = enabled;
                    if (WaitingScreen.Running) WaitingScreen.UpdateMessage(si.PackageCacheItem.Name);

                    //determine Type
                    SimPe.Cache.PackageType pt = si.PackageCacheItem.Type;
                    foreach (IIdentifier id in ScannerRegistry.Global.Identifiers)
                    {
                        if ((si.PackageCacheItem.Type != SimPe.Cache.PackageType.Unknown) && (si.PackageCacheItem.Type != SimPe.Cache.PackageType.Undefined))
                            break;

                        if ((si.PackageCacheItem.Type == SimPe.Cache.PackageType.Unknown) || (si.PackageCacheItem.Type == SimPe.Cache.PackageType.Undefined))
                            si.PackageCacheItem.Type = id.GetType(si.Package);
                    }

                    if (pt != si.PackageCacheItem.Type) cachechg = true;

                    //setup the ListView Item
                    SimPe.Scenegraph.Compat.ListViewItem lvi = new SimPe.Scenegraph.Compat.ListViewItem();
                    si.ListViewItem = lvi;
                    lvi.Text = System.IO.Path.GetFileNameWithoutExtension(si.FileName);
                    lvi.SubItems.Add(si.PackageCacheItem.Enabled.ToString());
                    lvi.SubItems.Add(si.PackageCacheItem.Type.ToString());
                    lvi.Tag = si;
                    if (!si.PackageCacheItem.Enabled) lvi.ForeColor = Color.Gray;

                    //run file through available scanners
                    foreach (IScanner s in usedscanners)
                    {
                        SimPe.Cache.PackageState ps = si.PackageCacheItem.FindState(s.Uid, true);
                        if (ps.State == SimPe.Cache.TriState.Null)
                        {
                            s.ScanPackage(si, ps, lvi);
                            if (ps.State != SimPe.Cache.TriState.Null) cachechg = true;
                        }
                        else s.UpdateState(si, ps, lvi);
                        if (stopClicked) break;
                    }

                    lv.Items.Add(lvi);

                    if (stopClicked) break;
                }
                catch (Exception ex)
                {
                    errorlog += file + ": " + ex.Message + Helper.lbr + "----------------------------------------" + Helper.lbr;
                }
            } //foreach
        }

        void UpdateList(bool savecache, bool rescan)
        {
            if (Helper.XmlRegistry.UseCache && savecache) cachefile.Save();
            if (rescan) Scan(null, EventArgs.Empty);
            else SelectItem(lv, null);
        }

        private void SelectFolder(object sender, EventArgs e)
        {
            if (cbfolder.SelectedIndex == 0)
            {
                folder = System.IO.Path.Combine(PathProvider.SimSavegameFolder, "Downloads");
            }
            else if (cbfolder.SelectedIndex == 1)
            {
                folder = System.IO.Path.Combine(PathProvider.SimSavegameFolder, "Teleport");
            }
            else if (cbfolder.SelectedIndex == 2)
            {
                folder = System.IO.Path.Combine(PathProvider.SimSavegameFolder, "Neighborhoods");
                cbrec.IsChecked = true;
            }
            else if (cbfolder.SelectedIndex == 3)
            {
                folder = System.IO.Path.Combine(PathProvider.SimSavegameFolder, "SavedSims");
            }
            else
            {
                if (fbd.SelectedPath == "") fbd.SelectedPath = PathProvider.SimSavegameFolder;
                if (fbd.ShowDialog() == DialogResult.OK) folder = fbd.SelectedPath;
            }
        }

        bool stopClicked = false;
        private void Scan(object sender, EventArgs e)
        {
            errorlog = "";
            cachechg = false;
            lv.Items.Clear();
            lv.Columns.Clear();
            ilist.Images.Clear();

            lv.BeginUpdate();
            WaitingScreen.Wait();
            WaitingScreen.Message = "";
            stopClicked = false;
            try
            {
                btscan.IsEnabled = false;
                if (Helper.XmlRegistry.UseCache) cachefile.LoadFiles();

                //Setup ListView
                lv.SmallImageList = null;
                lv.Refresh();
                SimPe.Plugin.Scanner.AbstractScanner.AddColumn(lv, "Filename", 180);
                SimPe.Plugin.Scanner.AbstractScanner.AddColumn(lv, "Enabled", 60);
                SimPe.Plugin.Scanner.AbstractScanner.AddColumn(lv, "Type", 80);

                //Select only checked Scanners
                ScannerCollection scanners = new ScannerCollection();
                for (int i = 0; i < lbscanners.Items.Count; i++)
                {
                    IScanner scanner = (IScanner)lbscanners.Items[i];
                    if (lbscanners.GetItemChecked(i))
                    {
                        scanners.Add(scanner);
                        scanner.EnableControl(true);
                    }
                    else scanner.EnableControl(false);
                }

                SimPe.Plugin.Scanner.AbstractScanner.AssignFileTable();
                //setup Scanners
                foreach (IScanner s in scanners) { WaitingScreen.Message = s.GetType().Name; s.InitScan(this.lv); }

                btscan.Content = "Stop";
                scanClicked = StopScan;
                btscan.IsEnabled = true;
                WaitingScreen.Stop();
                //scan all Files
                Scan(folder, cbrec.IsChecked.GetValueOrDefault(), scanners);
                WaitingScreen.Wait();
                WaitingScreen.Message = "Finishing scan";

                //finish Scanners
                foreach (IScanner s in scanners) s.FinishScan();
                SimPe.Plugin.Scanner.AbstractScanner.DeAssignFileTable();

                try
                {
                    if (Helper.XmlRegistry.UseCache && cachechg) cachefile.Save();
                }
                catch (Exception ex)
                {
                    Helper.ExceptionMessage("", ex);
                }
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(ex);
            }
            finally
            {
                btscan.Content = "Scan";
                scanClicked = Scan;
                btscan.IsEnabled = true;
                llSave.IsEnabled = true;
                WaitingScreen.UpdateImage(null);
                WaitingScreen.Stop();
                WaitingScreen.Message = "";
                lv.EndUpdate();
            }

            if (errorlog.Trim() != "") Helper.ExceptionMessage(new Warning("Unreadable Files were found", errorlog));
        }

        private void StopScan(object sender, EventArgs e)
        {
            btscan.IsEnabled = false;
            stopClicked = true;
        }

        private void ScanButtonClicked(object sender, EventArgs e)
        {
            scanClicked(null, null);
        }

        EventHandler scanClicked;

        private void SelectItem(object sender, EventArgs e)
        {
            try
            {
                lastitem = null;
                gbinfo.IsEnabled = (lv.SelectedItems.Count != 0);
                pnop.IsEnabled = (lv.SelectedItems.Count != 0);

                if (lv.SelectedItems.Count == 0) return;

                ScannerItem si = (ScannerItem)lv.SelectedItems[0].Tag;
                ShowInfo(si, lv.SelectedItems[0]);
                lastitem = si;

                int encount = 0;

                //do something for all selected Items
                ScannerItem[] items = new ScannerItem[lv.SelectedItems.Count];
                int ct = 0;
                foreach (SimPe.Scenegraph.Compat.ListViewItem lvi in lv.SelectedItems)
                {
                    si = (ScannerItem)lvi.Tag;
                    items[ct++] = si;
                    if (si.PackageCacheItem.Enabled) encount++;
                }

                if (encount == lv.SelectedItems.Count) this.cbenable.IsChecked = true;
                else if (encount == 0) this.cbenable.IsChecked = false;
                else this.cbenable.IsChecked = null;

                //Enable the Scanner Controls
                foreach (IScanner scanner in this.lbscanners.Items)
                {
                    scanner.EnableControl(items, ScannerRegistry.Global.Scanners.Contains(scanner));
                }
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage("", ex);
            }
        }

        private void SortList(object sender, EventArgs e)
        {
            // Column click sorting not available without ColumnClickEventArgs in Avalonia port
        }

        private void ReloadFileTable(object sender, EventArgs e)
        {
            FileTable.FileIndex.ForceReload();
        }

        private void ReloadCache(object sender, EventArgs e)
        {
            if (Helper.XmlRegistry.UseCache) cachefile.Load(SimPe.Cache.PackageCacheFile.CacheFileName);
        }

        private void SetEnabledState(object sender, EventArgs e)
        {
            if (_settingCbenable) return;
            if (this.cbenable.IsChecked == null) return;

            WaitingScreen.Wait();
            try
            {
                string ext = ".package";
                if (!this.cbenable.IsChecked.GetValueOrDefault()) ext = ".packagedisabled";

                WaitingScreen.UpdateMessage("Disable/Enable Packages");
                int ct = 0;
                foreach (SimPe.Scenegraph.Compat.ListViewItem lvi in lv.SelectedItems)
                {
                    pb.Value = ((ct++) * pb.Maximum) / lv.SelectedItems.Count;
                    ScannerItem si = (ScannerItem)lvi.Tag;

                    string newname = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(si.FileName), System.IO.Path.GetFileNameWithoutExtension(si.FileName) + ext);
                    string orgname = si.FileName;

                    if (!System.IO.File.Exists(newname))
                    {
                        SimPe.Packages.StreamItem stri = SimPe.Packages.StreamFactory.UseStream(orgname, System.IO.FileAccess.Read);
                        stri.Close();
                        SimPe.Packages.StreamItem strit = SimPe.Packages.StreamFactory.UseStream(newname, System.IO.FileAccess.Read);
                        strit.Close();
                        System.IO.File.Move(orgname, newname);

                        si.FileName = newname;
                        si.PackageCacheItem.Enabled = cbenable.IsChecked.GetValueOrDefault();
                        si.ParentContainer.FileName = newname;
                        si.ParentContainer.Added = DateTime.Now;
                    }
                }

                try
                {
                    WaitingScreen.UpdateMessage("Writing Cache");
                    if (Helper.XmlRegistry.UseCache) cachefile.Save();
                }
                catch (Exception ex)
                {
                    Helper.ExceptionMessage("", ex);
                }
            }
            finally
            {
                WaitingScreen.Stop();
                pb.Value = 0;
            }
        }

        private void ClearCache(object sender, EventArgs e)
        {
            DialogResult dr = DialogResult.Yes;

            if (!Helper.XmlRegistry.Silent) dr = MessageBoxStub.Show("Do you really want to clear the Cache?", "Confirm", null);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    System.IO.File.Delete(SimPe.Cache.PackageCacheFile.CacheFileName);
                    cachefile.Load(SimPe.Cache.PackageCacheFile.CacheFileName);
                }
                catch (Exception ex)
                {
                    Helper.ExceptionMessage("", ex);
                }
            }
        }

        private void OpenPackage(object sender, EventArgs e)
        {
            if (lastitem == null) return;

            this.flname = lastitem.FileName;
            Close();
        }

        private void llSave_LinkClicked(object sender, EventArgs e)
        {
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.StreamWriter sw = System.IO.File.CreateText(sfd.FileName);
                    try
                    {
                        foreach (SimPe.Scenegraph.Compat.ColumnHeader ch in lv.Columns)
                            sw.Write(ch.Text.Replace(",", ";") + ",");
                        sw.WriteLine();

                        foreach (SimPe.Scenegraph.Compat.ListViewItem lvi in lv.Items)
                        {
                            foreach (SimPe.Scenegraph.Compat.ListViewItem.SubItem lvsi in lvi.SubItems)
                                sw.Write(lvsi.Text.Replace(",", ";") + ",");
                            sw.WriteLine();
                        }
                    }
                    finally
                    {
                        sw.Close();
                        sw.Dispose();
                        sw = null;
                    }
                }
                catch (Exception ex)
                {
                    Helper.ExceptionMessage(ex);
                }
            }
        }
    }
}
