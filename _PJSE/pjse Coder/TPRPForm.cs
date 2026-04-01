/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
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
using System.Collections;
using System.ComponentModel;
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using DialogResult = SimPe.Scenegraph.Compat.DialogResult;
using MessageBoxButtons = SimPe.Scenegraph.Compat.MessageBoxButtons;
using MessageBoxIcon = SimPe.Scenegraph.Compat.MessageBoxIcon;
using SimPe.Interfaces.Plugin;
using SimPe.Plugin;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Summary description for TPRPForm.
    /// </summary>
    public class TPRPForm : Window, IPackedFileUI
    {
        #region Form variables

        private LabelCompat lbFilename;
        private TextBoxCompat tbFilename;
        private ButtonCompat btnStrDelete;
        private ButtonCompat btnStrAdd;
        private LabelCompat lbLabel;
        private TextBoxCompat tbLabel;
        private ButtonCompat btnCancel;
        private ButtonCompat btnCommit;
        private LabelCompat lbVersion;
        private TabControlCompat tabControl1;
        private Avalonia.Controls.TabItem tpParams;
        private Avalonia.Controls.TabItem tpLocals;
        private StackPanel tprpPanel;
        private TextBoxCompat tbVersion;
        private ListView lvParams;
        private ListView lvLocals;
        private ButtonCompat btnStrPrev;
        private ButtonCompat btnStrNext;
        private ButtonCompat btnTabNext;
        private ButtonCompat btnTabPrev;
        private pjse.pjse_banner pjse_banner1;
        #endregion

        public TPRPForm()
        {
            InitializeComponent();
            TextBoxCompat[] t = { tbFilename, tbLabel, };
            alText = new ArrayList(t);

            TextBoxCompat[] dw = { tbVersion ,};
            alHex32 = new ArrayList(dw);

            pjse.FileTable.GFT.FiletableRefresh += new EventHandler(GFT_FiletableRefresh);
            if (SimPe.Helper.XmlRegistry.UseBigIcons)
            {
                // Font sizing handled at application level
            }
        }

        void GFT_FiletableRefresh(object sender, EventArgs e)
        {
            pjse_banner1.SiblingEnabled = wrapper != null && wrapper.SiblingResource(Bhav.Bhavtype) != null;
        }

        public void Dispose() { }


        #region Controller
        private TPRP wrapper = null;
        private bool setHandler = false;
        private bool internalchg = false;

        private ArrayList alText = null;
        private ArrayList alHex32 = null;

        private int index = -1;
        private int tab = 0;
        private TPRPItem origItem = null;
        private TPRPItem currentItem = null;

        private int InitialTab
        {
            get
            {
                XmlRegistryKey  rkf = Helper.XmlRegistry.PluginRegistryKey.CreateSubKey("PJSE\\TPRP");
                object o = rkf.GetValue("initialTab", 1);
                return Convert.ToInt16(o);
            }

            set
            {
                XmlRegistryKey rkf = Helper.XmlRegistry.PluginRegistryKey.CreateSubKey("PJSE\\TPRP");
                rkf.SetValue("initialTab", value);
            }

        }

        private bool hex32_IsValid(object sender)
        {
            if (alHex32.IndexOf(sender) < 0)
                throw new Exception("hex32_IsValid not applicable to control " + sender.ToString());
            try { Convert.ToUInt32(((TextBoxCompat)sender).Text, 16); }
            catch (Exception) { return false; }
            return true;
        }


        private void doTextOnly()
        {
            tprpPanel.Children.Clear();
            tprpPanel.Children.Add(this.pjse_banner1);
            tprpPanel.Children.Add(this.lbFilename);
            tbFilename.IsReadOnly = true;
            tbFilename.Text = wrapper.FileName;
            tprpPanel.Children.Add(this.tbFilename);

            var lb = new LabelCompat { Content = pjse.Localization.GetString("tprpTextOnly") };

            var tb = new TextBoxCompat
            {
                IsReadOnly = true,
                AcceptsReturn = true,
                Text = getText(wrapper.StoredData)
            };

            tprpPanel.Children.Add(lb);
            tprpPanel.Children.Add(tb);
        }

        private string getText(System.IO.BinaryReader br)
        {
            br.BaseStream.Seek(0x50, System.IO.SeekOrigin.Begin);
            string s = "";
            bool hadNL = true;
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                byte b = br.ReadByte();
                if (b < 0x20 || b > 0x7e)
                {
                    if (!hadNL)
                    {
                        s += "\r\n";
                        hadNL = true;
                    }
                }
                else
                {
                    s += Convert.ToChar(b);
                    hadNL = false;
                }
            }
            return s;
        }


        private ListView lvCurrent
        {
            get { return (ListView)((tabControl1.SelectedIndex != 0) ? lvLocals : lvParams); }
        }

        private void LVAdd(ListView lv, TPRPItem item)
        {
            string[] s = {
                             "0x" + lv.Items.Count.ToString("X") + " (" + lv.Items.Count + ")"
                             ,item.LabelCompat
                         };
            lv.Items.Add(new ListViewItem(s));
        }

        private void updateLists()
        {
            wrapper.CleanUp();

            index = -1;

            lvParams.Items.Clear();
            lvLocals.Items.Clear();
            foreach (TPRPItem item in wrapper)
                LVAdd((item is TPRPLocalLabel) ? lvLocals : lvParams, item);
        }


        private void setTab(int l)
        {
            internalchg = true;
            InitialTab = tab = l;
            tabControl1.SelectedIndex = l;
            internalchg = false;

            if (this.lvCurrent.SelectedIndices.Count == 0)
            {
                index = -1;
                setIndex(lvCurrent.Items.Count > 0 ? 0 : -1);
            }
            else
                index = this.lvCurrent.SelectedIndices[0];

            displayTPRPItem();
        }

        private void setIndex(int i)
        {
            internalchg = true;
            if (i >= 0) this.lvCurrent.Items[i].Selected = true;
            else if (index >= 0) this.lvCurrent.Items[index].Selected = false;
            internalchg = false;

            if (this.lvCurrent.SelectedItems.Count > 0)
            {
                this.lvCurrent.SelectedItems[0].EnsureVisible();
            }

            if (index == i) return;
            index = i;
            displayTPRPItem();
        }


        private void displayTPRPItem()
        {
            currentItem = (index < 0) ? null : wrapper[tabControl1.SelectedIndex.Equals(1), index];

            internalchg = true;
            if (currentItem != null)
            {
                origItem = currentItem.Clone();
                this.tbLabel.Text = currentItem.LabelCompat;
                this.btnStrDelete.IsEnabled = this.tbLabel.IsEnabled = true;
            }
            else
            {
                origItem = null;
                this.tbLabel.Text = "";
                this.btnStrDelete.IsEnabled = this.tbLabel.IsEnabled = false;
            }
            this.btnStrPrev.IsEnabled = (index > 0);
            this.btnStrNext.IsEnabled = (index < lvCurrent.Items.Count - 1);
            this.btnTabPrev.IsEnabled = tab > 0;
            this.btnTabNext.IsEnabled = tab < this.tabControl1.Items.Count - 1;

            internalchg = false;

            this.btnCancel.IsEnabled = false;
        }


        private void TPRPItemAdd()
        {
            bool savedstate = internalchg;
            internalchg = true;

            TPRPItem newItem = tabControl1.SelectedIndex.Equals(1)
                ? (TPRPItem)new TPRPLocalLabel(wrapper)
                : (TPRPItem)new TPRPParamLabel(wrapper)
                ;

            try
            {
                wrapper.Add(newItem);
                LVAdd(lvCurrent, newItem);
            }
            catch { }

            internalchg = savedstate;

            setIndex(lvCurrent.Items.Count - 1);
        }

        private void TPRPItemDelete()
        {
            bool savedstate = internalchg;
            internalchg = true;

            wrapper.Remove(currentItem);
            int i = index;
            updateLists();

            internalchg = savedstate;

            setIndex((i >= lvCurrent.Items.Count) ? lvCurrent.Items.Count - 1 : i);
        }

        private void Commit()
        {
            bool savedstate = internalchg;
            internalchg = true;

            try
            {
                wrapper.SynchronizeUserData();
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(pjse.Localization.GetString("errwritingfile"), ex);
            }

            btnCommit.IsEnabled = wrapper.Changed;

            int i = index;
            updateLists();

            internalchg = savedstate;

            setIndex((i >= lvCurrent.Items.Count) ? lvCurrent.Items.Count - 1 : i);
        }

        private void Cancel()
        {
            bool savedstate = internalchg;
            internalchg = true;

            lvCurrent.SelectedItems[0].SubItems[1].Text = currentItem.LabelCompat = origItem.LabelCompat;

            internalchg = savedstate;

            displayTPRPItem();
        }

        #endregion

        #region IPackedFileUI Member
        /// <summary>
        /// Returns the Control that will be displayed within SimPe
        /// </summary>
        public Avalonia.Controls.Control GUIHandle
        {
            get
            {
                return tprpPanel;
            }
        }

        /// <summary>
        /// Called by the AbstractWrapper when the file should be displayed to the user.
        /// </summary>
        public void UpdateGUI(IFileWrapper wrp)
        {
            wrapper = (TPRP)wrp;
            WrapperChanged(wrapper, null);
            pjse_banner1.SiblingEnabled = wrapper.SiblingResource(Bhav.Bhavtype) != null;

            if (!wrapper.TextOnly)
            {
                internalchg = true;
                updateLists();
                internalchg = false;

                setTab(InitialTab);
            }

            if (!setHandler)
            {
                wrapper.WrapperChanged += (s, e) => this.WrapperChanged(s, e);
                setHandler = true;
            }
        }

        private void WrapperChanged(object sender, System.EventArgs e)
        {
            if (wrapper.TextOnly)
            {
                doTextOnly();
                return;
            }
            this.btnCommit.IsEnabled = wrapper.Changed;
            if (sender.Equals(currentItem))
                this.btnCancel.IsEnabled = true;

            if (internalchg) return;

            if (sender.Equals(wrapper))
            {
                internalchg = true;
                this.Title = tbFilename.Text = wrapper.FileName;
                this.tbVersion.Text = "0x" + SimPe.Helper.HexString(wrapper.Version);
                internalchg = false;
            }
            else if (!sender.Equals(currentItem))
                updateLists();
        }

        #endregion

        #region InitializeComponent
        private void InitializeComponent()
        {
            this.btnCommit = new ButtonCompat { Content = "Commit" };
            this.tprpPanel = new StackPanel();
            this.pjse_banner1 = new pjse.pjse_banner();
            this.btnTabNext = new ButtonCompat { Content = ">>" };
            this.btnTabPrev = new ButtonCompat { Content = "<<" };
            this.btnStrPrev = new ButtonCompat { Content = "<" };
            this.btnStrNext = new ButtonCompat { Content = ">" };
            this.tabControl1 = new TabControlCompat();
            this.tpParams = new Avalonia.Controls.TabItem { Header = "Params" };
            this.tpLocals = new Avalonia.Controls.TabItem { Header = "Locals" };
            this.lvParams = new ListView();
            this.lvLocals = new ListView();
            this.btnCancel = new ButtonCompat { Content = "Cancel" };
            this.tbLabel = new TextBoxCompat();
            this.btnStrDelete = new ButtonCompat { Content = "Delete" };
            this.btnStrAdd = new ButtonCompat { Content = "Add" };
            this.lbVersion = new LabelCompat { Content = "Version" };
            this.tbVersion = new TextBoxCompat { IsReadOnly = true };
            this.tbFilename = new TextBoxCompat();
            this.lbFilename = new LabelCompat { Content = "Filename" };
            this.lbLabel = new LabelCompat { Content = "LabelCompat" };

            var chPID = new ColumnHeader { Text = "ID" };
            var chPLabel = new ColumnHeader { Text = "LabelCompat" };
            var chLID = new ColumnHeader { Text = "ID" };
            var chLLabel = new ColumnHeader { Text = "LabelCompat" };
            this.lvParams.Columns.Add(chPID);
            this.lvParams.Columns.Add(chPLabel);
            this.lvLocals.Columns.Add(chLID);
            this.lvLocals.Columns.Add(chLLabel);

            this.lvParams.ItemActivate += (s, e) => this.ListView_ItemActivate(s, e);
            this.lvParams.SelectedIndexChanged += (s, e) => this.ListView_SelectedIndexChanged(s, e);
            this.lvLocals.ItemActivate += (s, e) => this.ListView_ItemActivate(s, e);
            this.lvLocals.SelectedIndexChanged += (s, e) => this.ListView_SelectedIndexChanged(s, e);

            this.tpParams.Content = this.lvParams;
            this.tpLocals.Content = this.lvLocals;
            this.tabControl1.Items.Add(this.tpParams);
            this.tabControl1.Items.Add(this.tpLocals);
            this.tabControl1.SelectionChanged += (s, e) => this.tabControl1_SelectedIndexChanged(s, e);

            this.pjse_banner1.SiblingVisible = true;
            this.pjse_banner1.SiblingClick += (s, e) => this.pjse_banner1_SiblingClick(s, e);

            this.btnCommit.Click += (s, e) => this.btnCommit_Click(s, e);
            this.btnCancel.Click += (s, e) => this.btnCancel_Click(s, e);
            this.btnTabNext.Click += (s, e) => this.btnTabNext_Click(s, e);
            this.btnTabPrev.Click += (s, e) => this.btnTabPrev_Click(s, e);
            this.btnStrPrev.Click += (s, e) => this.btnStrPrev_Click(s, e);
            this.btnStrNext.Click += (s, e) => this.btnStrNext_Click(s, e);
            this.btnStrAdd.Click += (s, e) => this.btnStrAdd_Click(s, e);
            this.btnStrDelete.Click += (s, e) => this.btnStrDelete_Click(s, e);

            this.tbLabel.TextChanged += (s, e) => this.tbText_TextChanged(s, e);
            this.tbLabel.GotFocus += (s, e) => this.tbText_Enter(s, e);
            this.tbVersion.TextChanged += (s, e) => this.hex32_TextChanged(s, e);
            this.tbVersion.LostFocus += (s, e) => this.hex32_Validated(s, e);
            this.tbVersion.GotFocus += (s, e) => this.tbText_Enter(s, e);
            this.tbFilename.TextChanged += (s, e) => this.tbText_TextChanged(s, e);
            this.tbFilename.GotFocus += (s, e) => this.tbText_Enter(s, e);

            var filenameRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            filenameRow.Children.Add(this.lbFilename);
            filenameRow.Children.Add(this.tbFilename);
            var versionRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            versionRow.Children.Add(this.lbVersion);
            versionRow.Children.Add(this.tbVersion);
            var labelRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            labelRow.Children.Add(this.lbLabel);
            labelRow.Children.Add(this.tbLabel);
            var navRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            navRow.Children.Add(this.btnStrPrev);
            navRow.Children.Add(this.btnStrNext);
            navRow.Children.Add(this.btnTabPrev);
            navRow.Children.Add(this.btnTabNext);
            var actionRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            actionRow.Children.Add(this.btnStrAdd);
            actionRow.Children.Add(this.btnStrDelete);
            actionRow.Children.Add(this.btnCancel);
            actionRow.Children.Add(this.btnCommit);

            this.tprpPanel.Children.Add(this.pjse_banner1);
            this.tprpPanel.Children.Add(filenameRow);
            this.tprpPanel.Children.Add(versionRow);
            this.tprpPanel.Children.Add(navRow);
            this.tprpPanel.Children.Add(this.tabControl1);
            this.tprpPanel.Children.Add(labelRow);
            this.tprpPanel.Children.Add(actionRow);
            this.tprpPanel.Name = "tprpPanel";

            this.Content = this.tprpPanel;
            this.Name = "TPRPForm";
        }
        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (internalchg) return;
            setTab(tabControl1.SelectedIndex);
        }

        private void ListView_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (internalchg) return;
            setIndex((this.lvCurrent.SelectedIndices.Count > 0) ? this.lvCurrent.SelectedIndices[0] : -1);
        }

        private void ListView_ItemActivate(object sender, System.EventArgs e)
        {
            this.tbLabel.Focus();
        }


        private void btnCommit_Click(object sender, System.EventArgs e)
        {
            this.Commit();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Cancel();
            this.tbLabel.Focus();
        }


        private void pjse_banner1_SiblingClick(object sender, EventArgs e)
        {
            Bhav bhav = (Bhav)wrapper.SiblingResource(Bhav.Bhavtype);
            if (bhav == null) return;
            if (bhav.Package != wrapper.Package)
            {
                var dr = MessageBox.ShowAsync(Localization.GetString("OpenOtherPkg"), pjse_banner1.TitleText, MessageBoxButtons.YesNo).GetAwaiter().GetResult();
                if (dr != SimPe.Scenegraph.Compat.DialogResult.Yes) return;
            }
            SimPe.RemoteControl.OpenPackedFile(bhav.FileDescriptor, bhav.Package);
        }


        private void btnStrPrev_Click(object sender, System.EventArgs e)
        {
            setIndex(index - 1);
        }

        private void btnStrNext_Click(object sender, System.EventArgs e)
        {
            setIndex(index + 1);
        }

        private void btnTabPrev_Click(object sender, System.EventArgs e)
        {
            this.setTab(tab - 1);
        }

        private void btnTabNext_Click(object sender, System.EventArgs e)
        {
            this.setTab(tab + 1);
        }


        private void btnStrAdd_Click(object sender, System.EventArgs e)
        {
            this.TPRPItemAdd();
            this.tbLabel.Focus();
        }

        private void btnStrDelete_Click(object sender, System.EventArgs e)
        {
            this.TPRPItemDelete();
        }


        private void tbText_Enter(object sender, System.EventArgs e)
        {
            ((TextBoxCompat)sender).SelectAll();
        }

        private void tbText_TextChanged(object sender, System.EventArgs e)
        {
            if (internalchg) return;

            internalchg = true;
            switch(alText.IndexOf(sender))
            {
                case 0: wrapper.FileName = ((TextBoxCompat)sender).Text; break;
                case 1: lvCurrent.SelectedItems[0].SubItems[1].Text = currentItem.LabelCompat = ((TextBoxCompat)sender).Text; break;
            }
            internalchg = false;
        }


        private void hex32_TextChanged(object sender, System.EventArgs ev)
        {
            if (internalchg) return;
            if (!hex32_IsValid(sender)) return;

            internalchg = true;
            uint val = Convert.ToUInt32(((TextBoxCompat)sender).Text, 16);
            switch (alHex32.IndexOf(sender))
            {
                case 0: wrapper.Version = val; break;
            }
            internalchg = false;
        }

        private void hex32_Validating(object sender, EventArgs e)
        {
            if (hex32_IsValid(sender)) return;
            hex32_Validated(sender, null);
        }

        private void hex32_Validated(object sender, System.EventArgs e)
        {
            uint val = 0;
            switch (alHex32.IndexOf(sender))
            {
                case 0: val = wrapper.Version; break;
            }

            bool origstate = internalchg;
            internalchg = true;
            ((TextBoxCompat)sender).Text = "0x" + Helper.HexString(val);
            internalchg = origstate;
        }
    }
}
