/***************************************************************************
 *   Copyright (C) 2008 by Peter L Jones                                   *
 *   peter@users.sf.net                                                    *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   Rhiamom@mac.com                                                       *
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
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using System.Threading;
using SimPe.Interfaces;
using SimPe.Interfaces.Plugin;
using SimPe.Interfaces.Scenegraph;
using SimPe.PackedFiles.Wrapper;

namespace pjse.guidtool
{
    /// <summary>
    /// Summary description for GUIDForm.
    /// </summary>
    public class GUIDForm : Window
    {
        #region Form variables

        private Avalonia.Controls.ProgressBar progressBar1;
        private LabelCompat lbStatus;
        private TextBoxCompat rtbReport;
        private TextBoxCompat tbNumber;
        private LabelCompat lbName;
        private TextBoxCompat tbName;
        private LabelCompat lbNumber;
        private ButtonCompat btnSearch;
        private ButtonCompat btnClose;
        private GroupBox groupBox1;
        private CheckBoxCompat2 ckbObjdGUID;
        private CheckBoxCompat2 ckbObjdName;
        private CheckBoxCompat2 ckbNrefName;
        private CheckBoxCompat2 ckbBhavName;
        private CheckBoxCompat2 ckbBconName;
        private GroupBox groupBox2;
        private Avalonia.Controls.RadioButton rb1default;
        private Avalonia.Controls.RadioButton rb1CPOnly;
        private ButtonCompat btnHelp;
        private CheckBoxCompat2 ckbCallsToBHAV;
        private SimPe.Plugin.GUIDChooser gcGroup;
        private ButtonCompat btnClearFilter;
        private CheckBoxCompat2 ckbSGSearch;
        private LabelCompat label1;
        private CheckBoxCompat2 ckbFromBHAV;
        private CheckBoxCompat2 ckbFromObjf;
        private CheckBoxCompat2 ckbFromTtab;
        private CheckBoxCompat2 ckbGLOB;
        private LabelCompat label2;
        private CheckBoxCompat2 ckbSTR;
        private CheckBoxCompat2 ckbCTSS;
        private CheckBoxCompat2 ckbTTAs;
        private CheckBoxCompat2 ckbDefLang;
        private StackPanel panel1;
        private StackPanel pnFixer;
        private ButtonCompat btclipb;

        #endregion

        public GUIDForm(bool packageloaded)
        {
            InitializeComponent();

            rb1CPOnly.IsEnabled = packageloaded;
            if (!rb1CPOnly.IsEnabled && rb1CPOnly.IsChecked == true)
                rb1default.IsChecked = true;

            if (pjse.FileTable.gft == null)
                pjse.FileTable.GFT.Refresh();

            SimPe.ThemeManager tm = SimPe.ThemeManager.Global.CreateChild();

            lHex32 = new List<TextBoxCompat>(new TextBoxCompat[] { tbNumber, });
            rbGroup = new List<Avalonia.Controls.RadioButton>(new Avalonia.Controls.RadioButton[] { rb1default, rb1CPOnly });

            this.oldText = this.btnSearch.Content?.ToString();

            SearchComplete += new EventHandler(Complete);

            #region Group filter
            sgNames = new List<string>();
            sgGroups = new List<uint>();
            sgNames.Add("Globals");
            sgGroups.Add(0x7FD46CD0);
            sgNames.Add("Behaviour");
            sgGroups.Add(0x7FE59FD0);
            foreach (SimPe.Data.SemiGlobalAlias sga in SimPe.Data.MetaData.SemiGlobals)
                if (sga.Known)
                {
                    sgNames.Add(sga.Name);
                    sgGroups.Add(sga.Id);
                }

            gcGroup.KnownObjects = new object[] { sgNames, sgGroups, };
            gcGroup.ComboBoxWidth = 420;
            #endregion
        }

        public void Dispose() { }


        private bool searching = false;
        private int matches = 0;
        private string oldText = null;
        private Thread searchThread = null;

        private List<String> sgNames = null;
        private List<UInt32> sgGroups = null;

        private List<Avalonia.Controls.RadioButton> rbGroup = null;
        private static bool Selected(Avalonia.Controls.RadioButton rb) { return rb.IsChecked == true; }

        private static int byPackageGroupTypeInstance(pjse.FileTable.Entry x, pjse.FileTable.Entry y)
        {
            int result = x.Package.FileName.CompareTo(y.Package.FileName);
            if (result == 0)
                result = x.Group.CompareTo(y.Group);
            if (result == 0)
                result = x.Type.CompareTo(y.Type);
            if (result == 0)
                result = x.Instance.CompareTo(y.Instance);
            return result;
        }

        private void Search(object o)
        {
            bool[] type = (bool[])((object[])o)[0];
            FileTable.Source where = (FileTable.Source)((object[])o)[1];
            uint searchNumber = (uint)((object[])o)[2];
            string searchText = (string)((object[])o)[3];
            uint group = (uint)((object[])o)[4];

            SetProgressCallback setProgress = new SetProgressCallback(SetProgress);
            AddResultCallback addResult = new AddResultCallback(AddResult);
            StopSearchCallback stopSearch = new StopSearchCallback(StopSearch);
            EventHandler onSearchComplete = new EventHandler(OnSearchComplete);

            try
            {
                List<pjse.FileTable.Entry> results = new List<FileTable.Entry>();
                if (group != 0)
                {
                    if (type[6])
                    #region Focus on SemiGlobal group
                    {
                        List<pjse.FileTable.Entry> globs = new List<FileTable.Entry>(pjse.FileTable.GFT[SimPe.Data.MetaData.GLOB_FILE, where]);
                        foreach (pjse.FileTable.Entry fte in globs)
                        {
                            SimPe.Plugin.Glob glob = ((SimPe.Plugin.Glob)fte.Wrapper);
                            if (glob == null) continue;
                            if (group != glob.SemiGlobalGroup) continue;

                            List<pjse.FileTable.Entry> temp = new List<FileTable.Entry>();
                            if (type[7]) temp.AddRange(pjse.FileTable.GFT[Bhav.Bhavtype, fte.Group, where]);
                            if (type[8]) temp.AddRange(pjse.FileTable.GFT[Objf.Objftype, fte.Group, where]);
                            if (type[9]) temp.AddRange(pjse.FileTable.GFT[Ttab.Ttabtype, fte.Group, where]);

                            if (fte.Group == 0xffffffff)
                            {
                                foreach (pjse.FileTable.Entry entry in temp)
                                    if (entry.Package == fte.Package) results.Add(entry);
                            }
                            else results.AddRange(temp);
                        }
                        if (type[7]) results.AddRange(pjse.FileTable.GFT[Bhav.Bhavtype, group, where]);
                        if (type[8]) results.AddRange(pjse.FileTable.GFT[Objf.Objftype, group, where]);
                        if (type[9]) results.AddRange(pjse.FileTable.GFT[Ttab.Ttabtype, group, where]);
                    }
                    #endregion
                    else if (type[10])
                    #region References to GLOB
                    {
                        List<pjse.FileTable.Entry> globs = new List<FileTable.Entry>(pjse.FileTable.GFT[SimPe.Data.MetaData.GLOB_FILE, where]);
                        foreach (pjse.FileTable.Entry fte in globs)
                        {
                            SimPe.Plugin.Glob glob = ((SimPe.Plugin.Glob)fte.Wrapper);
                            if (glob == null) continue;
                            if (group != glob.SemiGlobalGroup) continue;

                            pjse.FileTable.Entry[] objds = pjse.FileTable.GFT[SimPe.Data.MetaData.OBJD_FILE, fte.Group, where];

                            if (objds.Length == 0)
                                results.Add(fte);
                            else
                            {
                                if (fte.Group == 0xffffffff)
                                {
                                    foreach(pjse.FileTable.Entry entry in objds)
                                        if (entry.Package == fte.Package)
                                        {
                                            results.Add(entry);
                                            break;
                                        }
                                }
                                else
                                    results.Add(objds[0]);
                            }
                        }
                    }
                    #endregion
                    else
                    #region Search within group
                    {
                        if (type[0] || type[1])
                            results.AddRange(pjse.FileTable.GFT[SimPe.Data.MetaData.OBJD_FILE, group, where]);
                        if (type[2])
                            results.AddRange(pjse.FileTable.GFT[0x4E524546, group, where]); // NREF
                        if (type[3])
                            results.AddRange(pjse.FileTable.GFT[Bhav.Bhavtype, group, where]);
                        if (type[4])
                            results.AddRange(pjse.FileTable.GFT[Bcon.Bcontype, group, where]);
                        if (type[5])
                        {
                            if (type[7]) results.AddRange(pjse.FileTable.GFT[Bhav.Bhavtype, group, where]);
                            if (type[8]) results.AddRange(pjse.FileTable.GFT[Objf.Objftype, group, where]);
                            if (type[9]) results.AddRange(pjse.FileTable.GFT[Ttab.Ttabtype, group, where]);
                        }
                        if (type[11])
                            results.AddRange(pjse.FileTable.GFT[StrWrapper.Strtype, group, where]);
                        if (type[12])
                            results.AddRange(pjse.FileTable.GFT[StrWrapper.CTSStype, group, where]);
                        if (type[13])
                            results.AddRange(pjse.FileTable.GFT[StrWrapper.TTAstype, group, where]);
                    }
                    #endregion
                }
                else // group == 0
                {
                    if (type[6] || type[10]) { } // no results for group == 0
                    else
                    #region Search without group
                    {
                        if (type[0] || type[1])
                            results.AddRange(pjse.FileTable.GFT[SimPe.Data.MetaData.OBJD_FILE, where]);
                        if (type[2])
                            results.AddRange(pjse.FileTable.GFT[0x4E524546, where]); // NREF
                        if (type[3])
                            results.AddRange(pjse.FileTable.GFT[Bhav.Bhavtype, where]);
                        if (type[4])
                            results.AddRange(pjse.FileTable.GFT[Bcon.Bcontype, where]);
                        if (type[5])
                        {
                            if (type[7]) results.AddRange(pjse.FileTable.GFT[Bhav.Bhavtype, where]);
                            if (type[8]) results.AddRange(pjse.FileTable.GFT[Objf.Objftype, where]);
                            if (type[9]) results.AddRange(pjse.FileTable.GFT[Ttab.Ttabtype, where]);
                        }
                        if (type[11])
                            results.AddRange(pjse.FileTable.GFT[StrWrapper.Strtype, where]);
                        if (type[12])
                            results.AddRange(pjse.FileTable.GFT[StrWrapper.CTSStype, where]);
                        if (type[13])
                            results.AddRange(pjse.FileTable.GFT[StrWrapper.TTAstype, where]);
                    }
                    #endregion
                }

                results.Sort(byPackageGroupTypeInstance);

                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => setProgress( false, results.Count )).GetAwaiter().GetResult();

                int j = 0;
                pjse.FileTable.Entry previtem = null;
                foreach (pjse.FileTable.Entry item in results)
                {
                    if (item != previtem)
                    {
                        previtem = item;

                        uint itemguid = 0;

                        System.IO.BinaryReader reader = item.Wrapper.StoredData;
                        if (item.Type == SimPe.Data.MetaData.OBJD_FILE)
                            if (reader.BaseStream.Length > 0x5c + 4) // sizeof(uint)
                            {
                                reader.BaseStream.Seek(0x5c, System.IO.SeekOrigin.Begin);
                                itemguid = reader.ReadUInt32();
                            }

                        if ((type[0] && itemguid == searchNumber) ||
                            ((type[1] || type[2] || type[3]) && item.ToString().ToLower().Contains(searchText)) ||
                            type[10])
                            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => addResult( itemguid, item)).GetAwaiter().GetResult();

                        else if (type[5]) switch (item.Type)
                            {
                                case Bhav.Bhavtype:
                                    foreach (Instruction i in (Bhav)item.Wrapper)
                                        if (i.OpCode == searchNumber)
                                            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => addResult( itemguid, item)).GetAwaiter().GetResult();
                                    break;
                                case Objf.Objftype:
                                    foreach (ObjfItem i in (Objf)item.Wrapper)
                                        if (i.Action == searchNumber || i.Guardian == searchNumber)
                                            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => addResult( itemguid, item)).GetAwaiter().GetResult();
                                    break;
                                case Ttab.Ttabtype:
                                    foreach (TtabItem i in (Ttab)item.Wrapper)
                                        if (i.Action == searchNumber || i.Guardian == searchNumber)
                                            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => addResult( itemguid, item)).GetAwaiter().GetResult();
                                    break;
                            }

                        else if (((type[11] && item.Type == StrWrapper.Strtype) ||
                          (type[12] && item.Type == StrWrapper.CTSStype) ||
                          (type[13] && item.Type == StrWrapper.TTAstype)))
                        {
                            if (type[14])
                                foreach (StrItem si in ((StrWrapper)item.Wrapper)[(byte)1])
                                {
                                    if (si.Title.ToString().ToLower().Contains(searchText))
                                    {
                                        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => addResult( itemguid, item)).GetAwaiter().GetResult();
                                        break;
                                    }
                                }
                            else
                                foreach (StrItem si in (StrWrapper)item.Wrapper)
                                {
                                    if (si.Title.ToString().ToLower().Contains(searchText))
                                    {
                                        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => addResult( itemguid, item)).GetAwaiter().GetResult();
                                        break;
                                    }
                                }
                        }
                    }
                    Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => setProgress( true, ++j )).GetAwaiter().GetResult();
                    Thread.Sleep(0);
                    if (!searching)
                        break;
                }
            }
            catch (ThreadInterruptedException) { }
            finally
            {
                Thread.Sleep(0);
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => onSearchComplete( this, EventArgs.Empty ));
            }
        }

        private delegate void SetProgressCallback(bool maxOrValue, int progress);
        private void SetProgress(bool maxOrValue, int progress)
        {
            if (maxOrValue == false)
            {
                SimPe.WaitingScreen.Stop();
                this.progressBar1.Maximum = progress;
            }
            else
                this.progressBar1.Value = progress;
        }

        private delegate void AddResultCallback(uint itemguid, pjse.FileTable.Entry item);
        private void AddResult(uint itemguid, pjse.FileTable.Entry item)
        {
            if (item.Type == SimPe.Data.MetaData.OBJD_FILE)
            {
                this.rtbReport.Text += Localization.GetString("gt_reportOBJD",
                    SimPe.Helper.HexString(item.PFD.Group),
                    item.PFD.TypeName.Name,
                    "0x" + SimPe.Helper.HexString(itemguid),
                    item.ToString(),
                    item.Package.FileName) + "\r\n";
            }
            else
            {
                this.rtbReport.Text += Localization.GetString("gt_report",
                    SimPe.Helper.HexString(item.PFD.Group),
                    item.PFD.TypeName.Name,
                    item.ToString(),
                    item.Package.FileName)+"\r\n";
            }

            matches++;
        }

        private delegate bool StopSearchCallback();
        private bool StopSearch()
        {
            return !searching;
        }

        private event EventHandler SearchComplete;
        private void OnSearchComplete(object sender, EventArgs e)
        {
            if (SearchComplete != null) { SearchComplete(sender, e); }
        }

        private void Start()
        {
            bool[] type = new bool[] {
                /*0*/ckbObjdGUID.IsChecked == true, ckbObjdName.IsChecked == true, ckbNrefName.IsChecked == true, ckbBhavName.IsChecked == true, ckbBconName.IsChecked == true,
                /*5*/ckbCallsToBHAV.IsChecked == true, ckbSGSearch.IsChecked == true, ckbFromBHAV.IsChecked == true, ckbFromObjf.IsChecked == true, ckbFromTtab.IsChecked == true,
                /*10*/ckbGLOB.IsChecked == true, ckbSTR.IsChecked == true, ckbCTSS.IsChecked == true, ckbTTAs.IsChecked == true, ckbDefLang.IsChecked == true,
            };
            uint number = 0;
            try { number = Convert.ToUInt32(this.tbNumber.Text.Trim(), 16); }
            catch(System.FormatException) { number = 0; }
            this.tbNumber.Text = "0x" + SimPe.Helper.HexString(number);
            if (number == 0) { type[0] = type[5] = false; }
            if (number < 0x2000 || number > 0x2fff) { type[6] = false; }
            if (gcGroup.Value == 0) { type[6] = type[10] = false; }
            this.tbName.Text = this.tbName.Text.Trim().ToLower();
            if (this.tbName.Text.Length == 0) { type[1] = type[2] = type[3] = type[4] = type[11] = type[12] = type[13] = false; }
            SimPe.WaitingScreen.Wait();
            groupBox1.IsEnabled = false;
            ckbObjdGUID.IsEnabled = ckbCallsToBHAV.IsEnabled = ckbFromBHAV.IsEnabled = ckbFromObjf.IsEnabled = ckbFromTtab.IsEnabled = false;
            gcGroup.IsEnabled = ckbSGSearch.IsEnabled = btnClearFilter.IsEnabled = tbNumber.IsEnabled = tbName.IsEnabled = this.btnClose.IsEnabled = false;
            this.btnSearch.Content = pjse.Localization.GetString("gt_Stop");
            this.lbStatus.IsVisible = this.btclipb.IsVisible = false;
            this.progressBar1.Value = 0;
            this.progressBar1.IsVisible = true;
            this.rtbReport.Text = "";

            searching = true;
            matches = 0;

            FileTable.Source[] aS = new FileTable.Source[] { FileTable.Source.Any, FileTable.Source.Local };
            FileTable.Source s;
            int rbS = rbGroup.FindIndex(Selected);

            s = (rbS >= 0 && rbS < aS.Length) ? aS[rbS] : FileTable.Source.Any;

            searchThread = new Thread(new ParameterizedThreadStart(Search));
            searchThread.Start(new object[] { type, s, number, this.tbName.Text, gcGroup.Value });
        }

        private void Stop()
        {
            if (!searching) Complete(null, null);
            else
            {
                this.btnSearch.IsEnabled = false;
                searching = false;
            }
        }

        internal void Complete(object sender, EventArgs e)
        {
            searching = false;
            while (searchThread != null && searchThread.IsAlive)
                searchThread.Join(10);
            searchThread = null;
            ckbObjdGUID.IsEnabled = ckbCallsToBHAV.IsEnabled = ckbFromBHAV.IsEnabled = ckbFromObjf.IsEnabled = ckbFromTtab.IsEnabled = gcGroup.IsEnabled = true;
            ckbSGSearch.IsEnabled = btnClearFilter.IsEnabled = tbNumber.IsEnabled = tbName.IsEnabled = this.btnClose.IsEnabled = this.btnSearch.IsEnabled = true;
            groupBox1.IsEnabled = true;
            this.btnSearch.Content = oldText;
            this.progressBar1.Value = 0;
            this.progressBar1.IsVisible = false;
            this.lbStatus.IsVisible = true;
            if (matches > 0)
            {
                this.lbStatus.Content = pjse.Localization.GetString("gt_MatchesFound") + ": " + matches.ToString();
                this.btclipb.IsVisible = true;
            }
            else
            {
                this.lbStatus.Content = pjse.Localization.GetString("gt_NoMatchesFound");
                this.btclipb.IsVisible = false;
            }
        }


        List<TextBoxCompat> lHex32 = null;
        private bool hex32_IsValid(object sender)
        {
            if (!(sender is TextBoxCompat) || lHex32.IndexOf((TextBoxCompat)sender) < 0)
                throw new Exception("hex32_IsValid not applicable to control " + sender.ToString());
            try { Convert.ToUInt32(((TextBoxCompat)sender).Text, 16); }
            catch (Exception) { return false; }
            return true;
        }

        #region InitializeComponent
        private void InitializeComponent()
        {
            this.progressBar1 = new Avalonia.Controls.ProgressBar { Minimum = 0, Maximum = 100, Value = 0, IsVisible = false };
            this.lbStatus = new LabelCompat();
            this.rtbReport = new TextBoxCompat { IsReadOnly = true, AcceptsReturn = true };
            this.lbNumber = new LabelCompat();
            this.tbNumber = new TextBoxCompat();
            this.lbName = new LabelCompat();
            this.tbName = new TextBoxCompat();
            this.gcGroup = new SimPe.Plugin.GUIDChooser();
            this.ckbSGSearch = new CheckBoxCompat2();
            this.btnClearFilter = new ButtonCompat { Content = "Clear Filter" };
            this.btnSearch = new ButtonCompat { Content = "Search" };
            this.btnClose = new ButtonCompat { Content = "Close" };
            this.groupBox1 = new GroupBox();
            this.label2 = new LabelCompat();
            this.ckbObjdName = new CheckBoxCompat2();
            this.ckbGLOB = new CheckBoxCompat2();
            this.ckbSTR = new CheckBoxCompat2();
            this.ckbObjdGUID = new CheckBoxCompat2();
            this.ckbCallsToBHAV = new CheckBoxCompat2();
            this.ckbCTSS = new CheckBoxCompat2();
            this.ckbNrefName = new CheckBoxCompat2();
            this.label1 = new LabelCompat();
            this.ckbTTAs = new CheckBoxCompat2();
            this.ckbFromTtab = new CheckBoxCompat2();
            this.ckbBhavName = new CheckBoxCompat2();
            this.ckbDefLang = new CheckBoxCompat2();
            this.ckbFromObjf = new CheckBoxCompat2();
            this.ckbFromBHAV = new CheckBoxCompat2();
            this.ckbBconName = new CheckBoxCompat2();
            this.groupBox2 = new GroupBox();
            this.rb1default = new Avalonia.Controls.RadioButton { IsChecked = true };
            this.rb1CPOnly = new Avalonia.Controls.RadioButton();
            this.btnHelp = new ButtonCompat { Content = "Help" };
            this.panel1 = new StackPanel();
            this.pnFixer = new StackPanel();
            this.btclipb = new ButtonCompat { Content = "Copy to Clipboard" };

            this.ckbObjdName.IsCheckedChanged += (s, e) => this.ckbSomeName_CheckedChanged(s, e);
            this.ckbGLOB.IsCheckedChanged += (s, e) => this.ckbGLOB_CheckedChanged(s, e);
            this.ckbSTR.IsCheckedChanged += (s, e) => this.ckbSomeName_CheckedChanged(s, e);
            this.ckbObjdGUID.IsCheckedChanged += (s, e) => this.ckbObjdGUID_CheckedChanged(s, e);
            this.ckbCallsToBHAV.IsCheckedChanged += (s, e) => this.ckbCallsToBHAV_CheckedChanged(s, e);
            this.ckbCTSS.IsCheckedChanged += (s, e) => this.ckbSomeName_CheckedChanged(s, e);
            this.ckbNrefName.IsCheckedChanged += (s, e) => this.ckbSomeName_CheckedChanged(s, e);
            this.ckbTTAs.IsCheckedChanged += (s, e) => this.ckbSomeName_CheckedChanged(s, e);
            this.ckbBhavName.IsCheckedChanged += (s, e) => this.ckbSomeName_CheckedChanged(s, e);
            this.ckbBconName.IsCheckedChanged += (s, e) => this.ckbSomeName_CheckedChanged(s, e);
            this.btnClearFilter.Click += (s, e) => this.btnClearFilter_Click(s, e);
            this.btnSearch.Click += (s, e) => this.btnSearch_Click(s, e);
            this.btnClose.Click += (s, e) => this.btnClose_Click(s, e);
            this.btnHelp.Click += (s, e) => this.btnHelp_Click(s, e);
            this.btclipb.Click += (s, e) => this.btclipb_Click(s, e);

            this.gcGroup.ComboBoxWidth = 420;
            this.gcGroup.LabelCompat = "Group Filter:";
            this.gcGroup.Value = ((uint)(0u));

            this.pnFixer.Children.Add(this.gcGroup);

            var checkPanel = new WrapPanel();
            checkPanel.Children.Add(this.label2);
            checkPanel.Children.Add(this.ckbObjdName);
            checkPanel.Children.Add(this.ckbGLOB);
            checkPanel.Children.Add(this.ckbSTR);
            checkPanel.Children.Add(this.ckbObjdGUID);
            checkPanel.Children.Add(this.ckbCallsToBHAV);
            checkPanel.Children.Add(this.ckbCTSS);
            checkPanel.Children.Add(this.ckbNrefName);
            checkPanel.Children.Add(this.label1);
            checkPanel.Children.Add(this.ckbTTAs);
            checkPanel.Children.Add(this.ckbFromTtab);
            checkPanel.Children.Add(this.ckbBhavName);
            checkPanel.Children.Add(this.ckbDefLang);
            checkPanel.Children.Add(this.ckbFromObjf);
            checkPanel.Children.Add(this.ckbFromBHAV);
            checkPanel.Children.Add(this.ckbBconName);
            this.groupBox1.Content = checkPanel;

            var rbPanel = new StackPanel();
            rbPanel.Children.Add(this.rb1default);
            rbPanel.Children.Add(this.rb1CPOnly);
            this.groupBox2.Content = rbPanel;

            this.panel1.Children.Add(this.btclipb);
            this.panel1.Children.Add(this.pnFixer);
            this.panel1.Children.Add(this.tbName);
            this.panel1.Children.Add(this.lbNumber);
            this.panel1.Children.Add(this.lbName);
            this.panel1.Children.Add(this.btnClearFilter);
            this.panel1.Children.Add(this.ckbSGSearch);
            this.panel1.Children.Add(this.tbNumber);
            this.panel1.Children.Add(this.groupBox1);
            this.panel1.Children.Add(this.btnHelp);
            this.panel1.Children.Add(this.btnSearch);
            this.panel1.Children.Add(this.btnClose);
            this.panel1.Children.Add(this.lbStatus);
            this.panel1.Children.Add(this.progressBar1);
            this.panel1.Children.Add(this.rtbReport);
            this.panel1.Children.Add(this.groupBox2);

            this.Content = this.panel1;
            this.Name = "GUIDForm";
        }
        #endregion

        protected override void OnClosing(Avalonia.Controls.WindowClosingEventArgs e)
        {
            searching = false;
            if (searchThread != null && searchThread.IsAlive)
            {
                searchThread.Interrupt();
                searchThread.Join();
                searchThread = null;
            }
            e.Cancel = true;
            Hide();
        }

        private void hex32_Validating(object sender, EventArgs e)
        {
            if (hex32_IsValid(sender)) return;

            uint val = 0;
            switch (lHex32.IndexOf((TextBoxCompat)sender))
            {
                case 0: val = 0; break;
            }

            ((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(val);
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (searching)
                Stop();
            else
                Start();
        }

        private void btnHelp_Click(object sender, System.EventArgs e)
        {
            string protocol = "file://";
            string relativePathToHelp = "pjse.coder.plugin/PJSE_Help";

            SimPe.RemoteControl.ShowHelp(protocol + SimPe.Helper.SimPePluginPath + "/" + relativePathToHelp + "/Finder.htm");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool isCkbObjdGUIDEnabled { get { return !ckbCallsToBHAV.IsChecked == true && !ckbGLOB.IsChecked == true; } }
        private bool isCkbCallsToBHAVEnabled { get { return !ckbObjdGUID.IsChecked == true && !ckbGLOB.IsChecked == true && !isCkbSomeTextChecked; } }
        private bool isCkbGLOBEnabled { get { return !ckbObjdGUID.IsChecked == true && !ckbCallsToBHAV.IsChecked == true && !isCkbSomeTextChecked; } }
        private bool isFlpNamesEnabled { get { return !ckbCallsToBHAV.IsChecked == true && !ckbGLOB.IsChecked == true; } }
        private static bool isChecked(CheckBoxCompat2 cb) { return cb.IsChecked == true; }
        private bool isCkbSomeTextChecked { get { return isCkbSomeNameChecked || isCkbSomeStringChecked; } }
        private bool isCkbSomeNameChecked
        {
            get
            {
                List<CheckBoxCompat2> lcb = new List<CheckBoxCompat2>(new CheckBoxCompat2[] { ckbObjdName, ckbNrefName, ckbBhavName, ckbBconName, });
                return (lcb.Find(isChecked) != null);
            }
        }
        private bool isCkbSomeStringChecked
        {
            get
            {
                List<CheckBoxCompat2> lcb = new List<CheckBoxCompat2>(new CheckBoxCompat2[] { ckbSTR, ckbCTSS, ckbTTAs, });
                return (lcb.Find(isChecked) != null);
            }
        }

        private void ckbObjdGUID_CheckedChanged(object sender, EventArgs e)
        {
            ckbCallsToBHAV.IsEnabled = isCkbCallsToBHAVEnabled;
            ckbGLOB.IsEnabled = isCkbGLOBEnabled;
            ckbSTR.IsEnabled = ckbCTSS.IsEnabled = ckbTTAs.IsEnabled = ckbDefLang.IsEnabled = ckbObjdName.IsEnabled = ckbNrefName.IsEnabled = ckbBhavName.IsEnabled = ckbBconName.IsEnabled = isFlpNamesEnabled;

            if (ckbObjdGUID.IsChecked == true) ckbCallsToBHAV.IsChecked = ckbGLOB.IsChecked = false;

            tbNumber.IsEnabled = ckbObjdGUID.IsChecked == true;
            lbNumber.Content = ckbObjdGUID.IsChecked == true ? pjse.Localization.GetString("GUID") : "";
        }

        private void ckbCallsToBHAV_CheckedChanged(object sender, EventArgs e)
        {
            ckbGLOB.IsEnabled = isCkbGLOBEnabled;
            ckbObjdGUID.IsEnabled = isCkbObjdGUIDEnabled;
            ckbSTR.IsEnabled = ckbCTSS.IsEnabled = ckbTTAs.IsEnabled = ckbDefLang.IsEnabled = ckbObjdName.IsEnabled = ckbNrefName.IsEnabled = ckbBhavName.IsEnabled = ckbBconName.IsEnabled = isFlpNamesEnabled;

            if (ckbCallsToBHAV.IsChecked == true) ckbObjdGUID.IsChecked = ckbGLOB.IsChecked = false;

            tbNumber.IsEnabled = ckbSGSearch.IsEnabled = ckbFromBHAV.IsEnabled = ckbFromObjf.IsEnabled = ckbFromTtab.IsEnabled = ckbCallsToBHAV.IsChecked == true;
            lbNumber.Content = ckbCallsToBHAV.IsChecked == true ? pjse.Localization.GetString("OpCode") : "";
        }

        private void ckbGLOB_CheckedChanged(object sender, EventArgs e)
        {
            ckbCallsToBHAV.IsEnabled = isCkbObjdGUIDEnabled;
            ckbObjdGUID.IsEnabled = isCkbObjdGUIDEnabled;
            ckbSTR.IsEnabled = ckbCTSS.IsEnabled = ckbTTAs.IsEnabled = ckbDefLang.IsEnabled = ckbObjdName.IsEnabled = ckbNrefName.IsEnabled = ckbBhavName.IsEnabled = ckbBconName.IsEnabled = isFlpNamesEnabled;

            if (ckbGLOB.IsChecked == true) ckbObjdGUID.IsChecked = ckbCallsToBHAV.IsChecked = false;
        }

        private void ckbSomeName_CheckedChanged(object sender, EventArgs e)
        {
            ckbCallsToBHAV.IsEnabled = isCkbCallsToBHAVEnabled;
            ckbGLOB.IsEnabled = isCkbGLOBEnabled;
            ckbObjdGUID.IsEnabled = isCkbObjdGUIDEnabled;

            lbName.IsEnabled = tbName.IsEnabled = isCkbSomeTextChecked;
            ckbDefLang.IsEnabled = isCkbSomeStringChecked;
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            gcGroup.Value = 0;
        }

        private void btclipb_Click(object sender, EventArgs e)
        {
            string text = "";
            foreach (string clit in this.rtbReport.Text.Split('\n')) text += clit + "\r\n";
            _ = Clipboard?.SetTextAsync(text);
        }
    }
}
