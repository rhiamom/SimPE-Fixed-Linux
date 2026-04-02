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
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using SimPe.Interfaces.Plugin;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Summary description for StrForm.
    /// </summary>
    public class StrForm : Window, IPackedFileUI
    {
        #region Form variables
        private StackPanelCompat strPanel;
        private ButtonCompat btnCommit;
        private LabelCompat lbFilename;
        private TextBoxCompat tbFilename;
        private LabelCompat lbFormat;
        private TextBoxCompat tbFormat;
        private LabelCompat lbStringNum;
        private ButtonCompat btnStrDelete;
        private ButtonCompat btnStrAdd;
        private ButtonCompat btnClearAll;
        private LabelCompat lbLngSelect;
        private ComboBoxCompat cbLngSelect;
        private ButtonCompat btnLngNext;
        private ButtonCompat btnLngPrev;
        private ButtonCompat btnLngClear;
        private TextBoxCompat rtbTitle;
        private TextBoxCompat rtbDescription;
        private LabelCompat label1;
        private ButtonCompat btnBigString;
        private ButtonCompat btnBigDesc;
        private ButtonCompat btnAppend;
        private ColumnHeader chString;
        private ColumnHeader chDefault;
        private ColumnHeader chLang;
        private ListView lvStrItems;
        private ButtonCompat btnStrClear;
        private LabelCompat lbDesc;
        private CheckBoxCompat2 ckbDefault;
        private ButtonCompat btnStrPrev;
        private ButtonCompat btnStrNext;
        private ButtonCompat btnReplace;
        private ButtonCompat btnLngFirst;
        private ButtonCompat btnStrDefault;
        private ColumnHeader chLangDesc;
        private ColumnHeader chDefaultDesc;
        private CheckBoxCompat2 ckbDescription;
        private ButtonCompat btnImport;
        private ButtonCompat btnExport;
        private ButtonCompat btnStrCopy;
        private pjse.pjse_banner pjse_banner1;
        private ButtonCompat BtnClean;
        /// <summary>
        /// Required designer variable.
        /// </summary>
                #endregion

        public StrForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            Control[] af = { tbFormat };
            alHex16 = new ArrayList(af);

            Control[] at = { tbFilename, rtbTitle, rtbDescription };
            alTextBoxBase = new ArrayList(at);

            Control[] ab = { btnBigString, btnBigDesc };
            alBigBtn = new ArrayList(ab);

            pjse.FileTable.GFT.FiletableRefresh += new EventHandler(GFT_FiletableRefresh);
        }

        void GFT_FiletableRefresh(object sender, EventArgs e)
        {
            if (wrapper.FileDescriptor == null) return;

            byte oldLid = lid;
            int oldIndex = index;
            bool savedchg = internalchg;
            internalchg = true;

            updateLists();

            setLid(oldLid); // sets internalchg to false
            setIndex(oldIndex);

            internalchg = savedchg;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose() { }

        #region Controller
        private StrWrapper wrapper = null;
        private bool setHandler = false;
        private bool internalchg = false;

        private ArrayList alHex16 = null;
        private ArrayList alTextBoxBase = null;
        private ArrayList alBigBtn = null;

        private byte lid = 1;
        private int index = -1;
        private int count = 0;
        private bool[] isEmpty = new bool[45];
        private String langName = pjse.BhavWiz.readStr(pjse.GS.BhavStr.Languages, 1);

        private bool hex16_IsValid(object sender)
        {
            if (alHex16.IndexOf(sender) < 0)
                throw new Exception("hex16_IsValid not applicable to control " + sender.ToString());
            try { Convert.ToUInt16(((TextBoxCompat)sender).Text, 16); }
            catch (Exception) { return false; }
            return true;
        }

        private void updateSelectedItem()
        {
            if (lid == 1)
            {
                this.lvStrItems.Items[index].SubItems[3].Text = wrapper[1, index].Title;
                this.lvStrItems.Items[index].SubItems[4].Text = wrapper[1, index].Description;
            }
            this.lvStrItems.Items[index].SubItems[1].Text = wrapper[lid, index].Title;
            this.lvStrItems.Items[index].SubItems[2].Text = wrapper[lid, index].Description;
            isEmpty[lid] = true;
            List<StrItem> sa = wrapper[lid];
            for (int j = count - 1; j >= 0 && isEmpty[lid]; j--)
                if (sa[j] != null && (sa[j].Title.Trim().Length + sa[j].Description.Trim().Length > 0))
                    isEmpty[lid] = false;
            this.cbLngSelect.Items[lid - 1] = langName + (isEmpty[lid] ? " (" + pjse.Localization.GetString("empty") + ")" : "");

            doButtons();
        }

        private void doButtons()
        {
            // (index >= 0) means row selected
            // isEmpty[lid] means rows exist
            // empty means only default language has strings

            bool empty = true;
            foreach (StrItem s in wrapper)
                if ((s.LanguageID != 1) && (s.Title.Trim().Length + s.Description.Trim().Length > 0))
                    empty = false;

            this.btnStrPrev.IsEnabled = (index > 0);
            this.btnStrNext.IsEnabled = (index < count - 1);

            this.btnClearAll.IsEnabled = !empty; // "Default lang only"
            this.btnLngClear.IsEnabled = (lid != 1) && !isEmpty[lid]; // "Clear this lang"

            this.btnStrAdd.IsEnabled = (lid == 1);
            this.btnStrDelete.IsEnabled = (lid == 1) && (index >= 0);
            this.btnStrDefault.IsEnabled = (lid != 1) && !isEmpty[lid] && (index >= 0); // "Make default"
            this.btnStrClear.IsEnabled = (wrapper.Format != 0x0000) && !empty && (index >= 0); // "Default string only"
            this.btnStrCopy.IsEnabled = (wrapper.Format != 0x0000) && !isEmpty[lid] && (index >= 0);
            this.btnReplace.IsEnabled = (lid == 1);
            this.BtnClean.IsEnabled = (wrapper.Format != 0x0000 && wrapper.Format != 0xFFFE);
        }

        private void updateLists()
        {
            wrapper.CleanUp();

            lid = 0;
            index = -1;
            count = 0;

            bool onlyDefault = true;

            this.cbLngSelect.Items.Clear();
            this.cbLngSelect.Items.AddRange(pjse.BhavWiz.readStr(pjse.GS.BhavStr.Languages).ToArray());

            // I really wish there were a nicer way...
            for (byte i = 0; i < 44; i++)
            {
                isEmpty[i] = !wrapper.HasLanguage(i);
                if (!isEmpty[i] && i > 1) onlyDefault = false;

                while (i >= this.cbLngSelect.Items.Count)
                    this.cbLngSelect.Items.Add("0x" + SimPe.Helper.HexString((byte)this.cbLngSelect.Items.Count) + " (" + pjse.Localization.GetString("unk") + ")");
                this.cbLngSelect.Items[i] += isEmpty[i] ? " (" + pjse.Localization.GetString("empty") + ")" : "";

                if (i > 0) count = Math.Max(count, wrapper.CountOf(i));
            }

            this.btnClearAll.IsEnabled = !onlyDefault;
            this.cbLngSelect.Items.RemoveAt(0);
            while (wrapper.CountOf(1) < count) wrapper.Add(1, "", "");

            this.lvStrItems.Columns.Clear();
            this.lvStrItems.Columns.AddRange(new ColumnHeader[] { this.chString, this.chLang, this.chLangDesc, this.chDefault, this.chDefaultDesc});
            this.lvStrItems.Columns[1].Text = "";
            this.lvStrItems.Items.Clear();
            for (int i = 0; i < count; i++)
            {
                StrItem si = wrapper[1, i];
                this.lvStrItems.Items.Add(new ListViewItem(
                    new string[] {
                        "0x" + Helper.HexString((ushort)i) + " (" + i + ")",
                        "",
                        "",
                        ((si == null) ? "" : si.Title),
                        ((si == null) ? "" : si.Description)
                    }));
                this.lvStrItems.Items[i].UseItemStyleForSubItems = false;
            }
        }

        private void setLid(byte l)
        {
            if (lid == l) return;
            lid = l;
            langName = pjse.BhavWiz.readStr(pjse.GS.BhavStr.Languages, lid);

            internalchg = true;
            if (lid > 0) this.cbLngSelect.SelectedIndex = l - 1;
            internalchg = false;
            this.btnLngFirst.IsEnabled = this.btnLngPrev.IsEnabled = (this.cbLngSelect.SelectedIndex != null && cbLngSelect.SelectedIndex > 0);
            this.btnLngNext.IsEnabled = (wrapper.Format != 0x0000) && (this.cbLngSelect.Items.Count > 0) && (this.cbLngSelect.SelectedIndex < this.cbLngSelect.Items.Count - 1);

            this.btnLngClear.Text = pjse.Localization.GetString("Clear") + " " + langName;
            while (wrapper.CountOf(lid) < count) wrapper.Add(lid, "", "");
            this.lvStrItems.Columns[1].Text = this.cbLngSelect.SelectedItem.ToString();
            for (int i = 0; i < count; i++)
            {
                this.lvStrItems.Items[i].SubItems[1].Text = wrapper[lid, i].Title;
                this.lvStrItems.Items[i].SubItems[2].Text = wrapper[lid, i].Description;
            }

            displayStrItem();
        }

        private void setIndex(int i)
        {
            internalchg = true;
            if (i >= 0) this.lvStrItems.Items[i].Selected = true;
            else if (index >= 0) this.lvStrItems.Items[index].Selected = false;
            internalchg = false;

            if (this.lvStrItems.SelectedItems.Count > 0)
            {
                if (this.lvStrItems.Focused) this.lvStrItems.SelectedItems[0].Focused = true;
                this.lvStrItems.SelectedItems[0].EnsureVisible();
            }

            if (index == i) return;
            index = i;
            displayStrItem();
        }

        private void displayStrItem()
        {
            StrItem s = (index < 0) ? null : wrapper[lid, index];

            internalchg = true;
            if (s != null)
            {
                this.lbStringNum.Text = pjse.Localization.GetString("String") + " 0x"
                    + Helper.HexString((ushort)index) + " (" + langName + ")";
                this.rtbTitle.Text = s.Title;
                this.rtbTitle.SelectAll();
                this.btnBigString.IsEnabled = this.rtbTitle.IsEnabled = true;
                this.rtbDescription.Text = s.Description;
                this.rtbDescription.SelectAll();
                this.btnBigDesc.IsEnabled = this.rtbDescription.IsEnabled = (wrapper.Format != 0x0000 && wrapper.Format != 0xFFFE);
            }
            else
            {
                this.lbStringNum.Text = "";
                this.rtbDescription.Text = this.rtbTitle.Text = "";
                this.btnBigDesc.IsEnabled = this.rtbDescription.IsEnabled = this.btnBigString.IsEnabled = this.rtbTitle.IsEnabled = false;
            }
            internalchg = false;

            doButtons();
        }

        private void LngClear()
        {
            bool savedstate = internalchg;
            internalchg = true;

            wrapper.Remove(lid);

            byte l = lid;
            int i = index;
            updateLists();

            internalchg = savedstate;

            setLid(l);
            setIndex((i >= count) ? count - 1 : i);
        }

        private void LngClearAll()
        {
            bool savedstate = internalchg;
            internalchg = true;

            wrapper.DefaultOnly();

            byte l = lid;
            int i = index;
            updateLists();

            internalchg = savedstate;

            setLid(l);
            setIndex((i >= count) ? count - 1 : i);
        }

        private void CleanAll()
        {
            bool savedstate = internalchg;
            internalchg = true;

            wrapper.CleanHim();

            byte l = lid;
            int i = index;
            updateLists();

            internalchg = savedstate;

            setLid(l);
            setIndex((i >= count) ? count - 1 : i);
        }

        private void StrAdd()
        {
            bool savedstate = internalchg;
            internalchg = true;

            string title, desc;
            if (index >= 0)
            {
                StrItem si = (StrItem)wrapper[1, index];
                if (si != null)
                {
                    title = si.Title;
                    desc = si.Description;
                }
                else
                    title = desc = "";
            }
            else
                title = desc = "";

            try
            {
                wrapper.Add(1, title, desc);
                count++;
                this.lvStrItems.Items.Add(new ListViewItem(new string[] { "0x" + Helper.HexString((ushort)(count - 1)) + " (" + ((ushort)(count - 1)) + ")", title, desc, title, desc }));
            }
            catch { }

            internalchg = savedstate;

            //setLid(1);
            setIndex(count - 1);
        }

        private void StrDelete()
        {
            bool savedstate = internalchg;
            internalchg = true;

            for (byte j = 1; j < 44; j++)
            {
                for (int ix = index; ix < count - 1; ix++)
                {
                    StrItem s1 = wrapper[j, ix];
                    if (s1 != null)
                    {
                        StrItem s2 = wrapper[j, ix + 1];
                        if (s2 != null)
                        {
                            s1.Title = s2.Title;
                            s1.Description = s2.Description;
                        }
                        else
                            s1.Title = s1.Description = "";
                    }
                }
                wrapper.Remove(wrapper[j, count - 1]);
            }

            byte l = lid;
            int i = index;
            updateLists();

            internalchg = savedstate;

            setLid(l);
            setIndex((i >= count) ? count - 1 : i);
        }

        private void StrCopy()
        {
            bool savedstate = internalchg;
            internalchg = true;

            for (byte m = 1; m < 44; m++)
            {
                if (m == lid) continue;

                while (wrapper[m, index] == null) wrapper.Add(m, "", "");
                wrapper[m, index].Title = wrapper[lid, index].Title;
                wrapper[m, index].Description = wrapper[lid, index].Description;
            }

            byte l = lid;
            int i = index;
            updateLists();

            internalchg = savedstate;

            setLid(l);
            setIndex((i >= count) ? count - 1 : i);
        }

        private void StrReplace()
        {
            pjse.FileTable.Entry e = (new pjse.ResourceChooser()).Execute(wrapper.FileDescriptor.Type, wrapper.FileDescriptor.Group, strPanel, true);
            if (e == null || !(e.Wrapper is StrWrapper)) return;

            StrWrapper b = (StrWrapper)e.Wrapper;
            int strnum = (new pjse.StrChooser()).Strnum(b);
            if (strnum < 0) return;

            bool savedstate = internalchg;
            internalchg = true;

            if (wrapper.Format == 0x0000)
            {
                wrapper[1, index].Title = b[1, strnum].Title;
                wrapper[1, index].Description = b[1, strnum].Description;
            }
            else
                for (byte m = 1; m < 44; m++)
                {
                    while (wrapper[m, index] == null) wrapper.Add(m, "", "");
                    if (b[m, strnum] == null)
                    {
                        wrapper[m, index].Title = "";
                        wrapper[m, index].Description = "";
                    }
                    else
                    {
                        wrapper[m, index].Title = b[m, strnum].Title;
                        wrapper[m, index].Description = b[m, strnum].Description;
                    }
                }

            byte l = lid;
            int i = index;
            updateLists();

            internalchg = savedstate;

            setLid(l);
            setIndex((i >= count) ? count - 1 : i);
        }

        private void StrClear()
        {
            bool savedstate = internalchg;
            internalchg = true;

            for (byte m = 2; m < 44; m++)
            {
                StrItem s = wrapper[m, index];
                if (s != null) s.Description = s.Title = "";
            }

            byte l = lid;
            int i = index;
            updateLists();

            internalchg = savedstate;

            setLid(l);
            setIndex((i >= count) ? count - 1 : i);
        }

        private void StrDefault()
        {
            StrItem di = wrapper[1, index];
            StrItem si = wrapper[lid, index];

            di.Title = si.Title;
            di.Description = si.Description;

            this.lvStrItems.Items[index].SubItems[3].Text = wrapper[1, index].Title;
            this.lvStrItems.Items[index].SubItems[4].Text = wrapper[1, index].Description;
            isEmpty[1] = true;
            List<StrItem> sa = wrapper[(byte)1];
            for (int j = count - 1; j >= 0 && isEmpty[1]; j--)
                if (sa[j] != null && (sa[j].Title.Trim().Length + sa[j].Description.Trim().Length > 0))
                    isEmpty[1] = false;
            this.cbLngSelect.Items[0] = pjse.BhavWiz.readStr(pjse.GS.BhavStr.Languages, 1)
                + (isEmpty[1] ? " (" + pjse.Localization.GetString("empty") + ")" : "");
        }

        private void Append(pjse.FileTable.Entry e)
        {
            if (e == null) return;

            bool savedstate = internalchg;
            internalchg = true;


            using (StrWrapper b = (StrWrapper)e.Wrapper)
            {
                if (wrapper.Format != 0x0000)
                    for (byte m = 1; m < 44; m++)
                        while (wrapper[m, count - 1] == null) wrapper.Add(m, "", "");
                for (int bi = 0; bi < b.Count; bi++)
                {
                    if (wrapper.Format == 0x0000 && b[bi].LanguageID != 1) continue;
                    try { wrapper.Add(b[bi]); }
                    catch { break; }
                }
            }


            byte l = lid;
            int i = index;
            updateLists();

            internalchg = savedstate;

            setLid(l);
            setIndex((i >= count) ? count - 1 : i);
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

            byte l = lid;
            int i = index;
            updateLists();

            internalchg = savedstate;

            setLid(l);
            setIndex((i >= count) ? count - 1 : i);
        }

        private void StringFile(bool load)
        {
            FileDialogCompat fd = load ? (FileDialogCompat)new OpenFileDialogCompat() : (FileDialogCompat)new SaveFileDialogCompat();
            fd.AddExtension = true;
            fd.CheckFileExists = load;
            fd.CheckPathExists = true;
            fd.DefaultExt = "txt";
            fd.DereferenceLinks = true;
            fd.FileName = langName + ".txt";
            fd.Filter = pjse.Localization.GetString("strLangFilter");
            fd.FilterIndex = 1;
            fd.RestoreDirectory = false;
            fd.ShowHelp = false;
            //fd.SupportMultiDottedExtensions = false; // Methods missing from Mono
            fd.Title = load
                ? pjse.Localization.GetString("strLangLoad")
                : pjse.Localization.GetString("strLangSave");
            fd.ValidateNames = true;
            SimPe.DialogResult dr = fd.ShowDialog();

            if (dr == SimPe.DialogResult.OK)
            {
                if (load)
                {
                    bool savedstate = internalchg;
                    internalchg = true;

                    wrapper.ImportLanguage(lid, fd.FileName);

                    byte l = lid;
                    int i = index;
                    updateLists();

                    internalchg = savedstate;

                    setLid(l);
                    setIndex((i >= count) ? count - 1 : i);
                }
                else
                    wrapper.ExportLanguage(lid, fd.FileName);
            }
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
                return strPanel;
            }
        }

        /// <summary>
        /// Called by the AbstractWrapper when the file should be displayed to the user.
        /// </summary>
        /// <param name="wrp">Reference to the wrapper to be displayed.</param>
        public void UpdateGUI(IFileWrapper wrp)
        {
            wrapper = (StrWrapper)wrp;
            this.WrapperChanged(wrapper, null);

            internalchg = true;
            updateLists();
            this.ckbDefault.IsChecked = pjse.Settings.PJSE.StrShowDefault;
            this.ckbDescription.IsChecked = pjse.Settings.PJSE.StrShowDesc;
            internalchg = false;

            setLid(1);
            setIndex(count > 0 ? 0 : -1);
            ckb_CheckedChanged(null, null);

            if (!setHandler)
            {
                wrapper.WrapperChanged += (s, e) => this.WrapperChanged(s, e);
                setHandler = true;
            }
        }

        private void WrapperChanged(object sender, System.EventArgs e)
        {
            this.btnCommit.IsEnabled = wrapper.Changed;

            if (internalchg) return;
            internalchg = true;
            this.Title = this.tbFilename.Text = wrapper.FileName;
            this.tbFormat.Text = "0x" + Helper.HexString(wrapper.Format);
            if (wrapper.Format == 0x0000)
            {
                this.btnBigDesc.IsEnabled = this.rtbDescription.IsEnabled = this.ckbDefault.IsEnabled = this.cbLngSelect.IsEnabled = false;
            }
            else if (wrapper.Format == 0xFFFE)
            {
                this.btnBigDesc.IsEnabled = this.rtbDescription.IsEnabled = false;
                this.ckbDefault.IsEnabled = this.cbLngSelect.IsEnabled = true;
            }
            else
            {
                this.btnBigDesc.IsEnabled = this.rtbDescription.IsEnabled = this.ckbDefault.IsEnabled = this.cbLngSelect.IsEnabled = true;
            }
            internalchg = false;

            this.ckbDefault.IsEnabled = this.cbLngSelect.IsEnabled = (wrapper.Format != 0x0000);
        }

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrForm));
            this.strPanel = new StackPanelCompat();
            this.BtnClean = new ButtonCompat();
            this.pjse_banner1 = new pjse.pjse_banner();
            this.ckbDescription = new CheckBoxCompat2();
            this.btnLngFirst = new ButtonCompat();
            this.btnStrPrev = new ButtonCompat();
            this.btnStrNext = new ButtonCompat();
            this.ckbDefault = new CheckBoxCompat2();
            this.btnStrClear = new ButtonCompat();
            this.lvStrItems = new ListView();
            this.chString = new ColumnHeader();
            this.chLang = new ColumnHeader();
            this.chLangDesc = new ColumnHeader();
            this.chDefault = new ColumnHeader();
            this.chDefaultDesc = new ColumnHeader();
            this.btnBigDesc = new ButtonCompat();
            this.btnBigString = new ButtonCompat();
            this.lbDesc = new LabelCompat();
            this.label1 = new LabelCompat();
            this.rtbDescription = new TextBoxCompat();
            this.rtbTitle = new TextBoxCompat();
            this.btnLngNext = new ButtonCompat();
            this.btnLngPrev = new ButtonCompat();
            this.btnLngClear = new ButtonCompat();
            this.cbLngSelect = new ComboBoxCompat();
            this.lbLngSelect = new LabelCompat();
            this.btnClearAll = new ButtonCompat();
            this.lbStringNum = new LabelCompat();
            this.tbFilename = new TextBoxCompat();
            this.lbFilename = new LabelCompat();
            this.btnCommit = new ButtonCompat();
            this.lbFormat = new LabelCompat();
            this.tbFormat = new TextBoxCompat();
            this.btnImport = new ButtonCompat();
            this.btnExport = new ButtonCompat();
            this.btnAppend = new ButtonCompat();
            this.btnStrDelete = new ButtonCompat();
            this.btnStrAdd = new ButtonCompat();
            this.btnReplace = new ButtonCompat();
            this.btnStrCopy = new ButtonCompat();
            this.btnStrDefault = new ButtonCompat();
            // strPanel
            //            this.strPanel.Children.Add(this.BtnClean);
            this.strPanel.Children.Add(this.pjse_banner1);
            this.strPanel.Children.Add(this.ckbDescription);
            this.strPanel.Children.Add(this.btnLngFirst);
            this.strPanel.Children.Add(this.btnStrPrev);
            this.strPanel.Children.Add(this.btnStrNext);
            this.strPanel.Children.Add(this.ckbDefault);
            this.strPanel.Children.Add(this.btnStrClear);
            this.strPanel.Children.Add(this.lvStrItems);
            this.strPanel.Children.Add(this.btnBigDesc);
            this.strPanel.Children.Add(this.btnBigString);
            this.strPanel.Children.Add(this.lbDesc);
            this.strPanel.Children.Add(this.label1);
            this.strPanel.Children.Add(this.rtbDescription);
            this.strPanel.Children.Add(this.rtbTitle);
            this.strPanel.Children.Add(this.btnLngNext);
            this.strPanel.Children.Add(this.btnLngPrev);
            this.strPanel.Children.Add(this.btnLngClear);
            this.strPanel.Children.Add(this.cbLngSelect);
            this.strPanel.Children.Add(this.lbLngSelect);
            this.strPanel.Children.Add(this.btnClearAll);
            this.strPanel.Children.Add(this.lbStringNum);
            this.strPanel.Children.Add(this.tbFilename);
            this.strPanel.Children.Add(this.lbFilename);
            this.strPanel.Children.Add(this.btnCommit);
            this.strPanel.Children.Add(this.lbFormat);
            this.strPanel.Children.Add(this.tbFormat);
            this.strPanel.Children.Add(this.btnImport);
            this.strPanel.Children.Add(this.btnExport);
            this.strPanel.Children.Add(this.btnAppend);
            this.strPanel.Children.Add(this.btnStrDelete);
            this.strPanel.Children.Add(this.btnStrAdd);
            this.strPanel.Children.Add(this.btnReplace);
            this.strPanel.Children.Add(this.btnStrCopy);
            this.strPanel.Children.Add(this.btnStrDefault);
            this.strPanel.Name = "strPanel";
            this.strPanel.Paint += (s, e) => this.strPanel_Paint(s, e as SimPe.Scenegraph.Compat.PaintEventArgs ?? new SimPe.Scenegraph.Compat.PaintEventArgs());
            this.strPanel.Resize += (s, e) => this.strPanel_Resize(s, e);
            // 
            // BtnClean
            //            this.BtnClean.Name = "BtnClean";
            this.BtnClean.Click += (s, e) => this.btnClean_Click(s, e);
            // 
            // pjse_banner1
            //            this.pjse_banner1.Name = "pjse_banner1";
            // 
            // ckbDescription
            //            this.ckbDescription.Name = "ckbDescription";
            this.ckbDescription.IsCheckedChanged += (s, e) => this.ckb_CheckedChanged(s, e);
            // 
            // btnLngFirst
            //            this.btnLngFirst.Name = "btnLngFirst";
            this.btnLngFirst.Click += (s, e) => this.btnLngFirst_Click(s, e);
            // 
            // btnStrPrev
            //            this.btnStrPrev.Name = "btnStrPrev";
            this.btnStrPrev.Click += (s, e) => this.btnStrPrev_Click(s, e);
            // 
            // btnStrNext
            //            this.btnStrNext.Name = "btnStrNext";
            this.btnStrNext.Click += (s, e) => this.btnStrNext_Click(s, e);
            // 
            // ckbDefault
            //            this.ckbDefault.Name = "ckbDefault";
            this.ckbDefault.IsCheckedChanged += (s, e) => this.ckb_CheckedChanged(s, e);
            // 
            // btnStrClear
            //            this.btnStrClear.Name = "btnStrClear";
            this.btnStrClear.Click += (s, e) => this.btnStrClear_Click(s, e);
            // 
            // lvStrItems
            // 
            this.lvStrItems.Columns.AddRange(new ColumnHeader[] {
            this.chString,
            this.chLang,
            this.chLangDesc,
            this.chDefault,
            this.chDefaultDesc});
            this.lvStrItems.Name = "lvStrItems";

            this.lvStrItems.ItemActivate += (s, e) => this.lvStrItems_ItemActivate(s, e);
            this.lvStrItems.SelectionChanged += (s, e) => this.lvStrItems_SelectedIndexChanged(s, e);
            // 
            // chString
            //            // 
            // chLang
            //            // 
            // chLangDesc
            //            // 
            // chDefault
            //            // 
            // chDefaultDesc
            //            // 
            // btnBigDesc
            //            this.btnBigDesc.Name = "btnBigDesc";
            this.btnBigDesc.Click += (s, e) => this.btnBigString_Click(s, e);
            // 
            // btnBigString
            //            this.btnBigString.Name = "btnBigString";
            this.btnBigString.Click += (s, e) => this.btnBigString_Click(s, e);
            // 
            // lbDesc
            //            this.lbDesc.Name = "lbDesc";
            // 
            // label1
            //            this.label1.Name = "label1";
            // 
            // rtbDescription
            //            this.rtbDescription.Name = "rtbDescription";
            this.rtbDescription.GotFocus += (s, e) => this.textBoxBase_Enter(s, e);
            this.rtbDescription.TextChanged += (s, e) => this.textBoxBase_TextChanged(s, e);
            // 
            // rtbTitle
            //            this.rtbTitle.Name = "rtbTitle";
            this.rtbTitle.GotFocus += (s, e) => this.textBoxBase_Enter(s, e);
            this.rtbTitle.TextChanged += (s, e) => this.textBoxBase_TextChanged(s, e);
            // 
            // btnLngNext
            //            this.btnLngNext.Name = "btnLngNext";
            this.btnLngNext.Click += (s, e) => this.btnLngNext_Click(s, e);
            // 
            // btnLngPrev
            //            this.btnLngPrev.Name = "btnLngPrev";
            this.btnLngPrev.Click += (s, e) => this.btnLngPrev_Click(s, e);
            // 
            // btnLngClear
            //            this.btnLngClear.Name = "btnLngClear";
            this.btnLngClear.Click += (s, e) => this.btnLngClear_Click(s, e);
            // 
            // cbLngSelect
            // 
            this.cbLngSelect.SelectionChanged += (s, e) => this.cbLngSelect_SelectedIndexChanged(s, e);
            // 
            // lbLngSelect
            //            this.lbLngSelect.Name = "lbLngSelect";
            // 
            // btnClearAll
            //            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Click += (s, e) => this.btnClearAll_Click(s, e);
            // 
            // lbStringNum
            //            this.lbStringNum.Name = "lbStringNum";
            // 
            // tbFilename
            //            this.tbFilename.Name = "tbFilename";
            this.tbFilename.TextChanged += (s, e) => this.textBoxBase_TextChanged(s, e);
            this.tbFilename.GotFocus += (s, e) => this.textBoxBase_Enter(s, e);
            // 
            // lbFilename
            //            this.lbFilename.Name = "lbFilename";
            // 
            // btnCommit
            //            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Click += (s, e) => this.btnCommit_Click(s, e);
            // 
            // lbFormat
            //            this.lbFormat.Name = "lbFormat";
            // 
            // tbFormat
            //            this.tbFormat.Name = "tbFormat";
            this.tbFormat.TextChanged += (s, e) => this.hex16_TextChanged(s, e);
            this.tbFormat.LostFocus += (s, e) => this.hex16_Validated(s, e);
            // btnImport
            //            this.btnImport.Name = "btnImport";
            this.btnImport.Click += (s, e) => this.btnStringFile_Click(s, e);
            // 
            // btnExport
            //            this.btnExport.Name = "btnExport";
            this.btnExport.Click += (s, e) => this.btnStringFile_Click(s, e);
            // 
            // btnAppend
            //            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Click += (s, e) => this.btnAppend_Click(s, e);
            // 
            // btnStrDelete
            //            this.btnStrDelete.Name = "btnStrDelete";
            this.btnStrDelete.Click += (s, e) => this.btnStrDelete_Click(s, e);
            // 
            // btnStrAdd
            //            this.btnStrAdd.Name = "btnStrAdd";
            this.btnStrAdd.Click += (s, e) => this.btnStrAdd_Click(s, e);
            // 
            // btnReplace
            //            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Click += (s, e) => this.btnImport_Click(s, e);
            // 
            // btnStrCopy
            //            this.btnStrCopy.Name = "btnStrCopy";
            this.btnStrCopy.Click += (s, e) => this.btnStrCopy_Click(s, e);
            // 
            // btnStrDefault
            //            this.btnStrDefault.Name = "btnStrDefault";
            this.btnStrDefault.Click += (s, e) => this.btnStrDefault_Click(s, e);
            // 
            // StrForm
            // 
this.Name = "StrForm";
        }
        #endregion

        private void strPanel_Resize(object sender, System.EventArgs e)
        {
            this.btnBigDesc.Left = this.btnCommit.Right - this.btnBigDesc.Width;

            int width = this.btnBigDesc.Left - this.rtbTitle.Left - this.lbDesc.Width - 8;

            this.rtbDescription.Width = this.rtbTitle.Width = width / 2;
            this.btnBigString.Left = this.rtbTitle.Right;
            this.lbDesc.Left = this.rtbTitle.Right + 4;
            this.rtbDescription.Left = this.lbDesc.Right + 4;
        }

        private void textBoxBase_Enter(object sender, System.EventArgs e)
        {
            ((TextBoxCompat)sender).SelectAll();
        }

        private void textBoxBase_TextChanged(object sender, System.EventArgs e)
        {
            if (internalchg) return;

            internalchg = true;
            switch (alTextBoxBase.IndexOf(sender))
            {
                case 0: wrapper.FileName = ((TextBoxCompat)sender).Text; break;
                case 1: wrapper[lid, index].Title = ((TextBoxCompat)sender).Text; updateSelectedItem(); break;
                case 2: wrapper[lid, index].Description = ((TextBoxCompat)sender).Text; updateSelectedItem(); break;
            }
            internalchg = false;
        }

        private void hex16_TextChanged(object sender, System.EventArgs ev)
        {
            if (internalchg) return;
            if (!hex16_IsValid(sender)) return;

            ushort val = Convert.ToUInt16(((TextBoxCompat)sender).Text, 16);
            internalchg = true;
            switch (alHex16.IndexOf(sender))
            {
                case 0: wrapper.Format = val; break;
            }
            internalchg = false;
        }

        private void hex16_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (hex16_IsValid(sender)) return;
            e.Cancel = true;
            hex16_Validated(sender, null);
        }

        private void hex16_Validated(object sender, System.EventArgs e)
        {
            bool origstate = internalchg;
            internalchg = true;
            ushort val = 0;
            switch (alHex16.IndexOf(sender))
            {
                case 0: val = wrapper.Format; break;
            }

            ((TextBoxCompat)sender).Text = "0x" + Helper.HexString(val);
            ((TextBoxCompat)sender).SelectAll();
            internalchg = origstate;
        }

        private void cbLngSelect_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (internalchg) return;
            if (this.cbLngSelect.SelectedIndex != null)
                setLid((byte)((this.cbLngSelect.SelectedIndex) + 1));
        }

        private void lvStrItems_ItemActivate(object sender, System.EventArgs e)
        {
            this.rtbTitle.Focus();
        }

        private void lvStrItems_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (internalchg) return;
            setIndex((this.lvStrItems.SelectedIndices.Count > 0) ? this.lvStrItems.SelectedIndices[0] : -1);
        }

        private void ckb_CheckedChanged(object sender, System.EventArgs e)
        {
            if (internalchg) return;
            pjse.Settings.PJSE.StrShowDefault = this.ckbDefault.IsChecked == true;
            pjse.Settings.PJSE.StrShowDesc = this.ckbDescription.IsChecked == true;

            int w1 = this.chLang.Width + this.chDefault.Width;
            int w2 = this.chLangDesc.Width + this.chDefaultDesc.Width;
            if (w1 <= 0) w1 = 100;
            if (w2 <= 0) w2 = 50;

            if (this.ckbDefault.IsChecked == true) w1 /= 2;
            w1 -= w2;

            this.chLangDesc.Width = this.chDefault.Width = this.chDefaultDesc.Width = 0;
            this.chLang.Width = w1;
            this.chLangDesc.Width = w2;
            if (this.ckbDefault.IsChecked == true)
            {
                this.chDefault.Width = w1;
                this.chDefaultDesc.Width = w2;
            }
        }

        private void btnBigString_Click(object sender, System.EventArgs e)
        {
            int index = alBigBtn.IndexOf(sender);
            if (index < 0)
                throw new Exception("btnBigString_Click not applicable to control " + sender.ToString());

            TextBoxCompat[] rtb = { rtbTitle, rtbDescription };
            string result = (new pjse.StrBig()).doBig(rtb[index].Text);
            if (result != null) rtb[index].Text = result;
        }

        private void btnStrPrev_Click(object sender, System.EventArgs e)
        {
            setIndex(index - 1);
        }

        private void btnStrNext_Click(object sender, System.EventArgs e)
        {
            setIndex(index + 1);
        }

        private void btnLngFirst_Click(object sender, System.EventArgs e)
        {
            setLid(1);
        }

        private void btnLngPrev_Click(object sender, System.EventArgs e)
        {
            setLid((byte)(lid - 1));
        }

        private void btnLngNext_Click(object sender, System.EventArgs e)
        {
            setLid((byte)(lid + 1));
        }

        private void btnLngClear_Click(object sender, System.EventArgs e)
        {
            this.LngClear();
        }

        private void btnClearAll_Click(object sender, System.EventArgs e)
        {
            this.LngClearAll();
        }

        private void btnStrAdd_Click(object sender, System.EventArgs e)
        {
            this.StrAdd();
            this.rtbTitle.Focus();
        }

        private void btnStrDelete_Click(object sender, System.EventArgs e)
        {
            this.StrDelete();
        }

        private void btnStrDefault_Click(object sender, System.EventArgs e)
        {
            StrDefault();
        }

        private void btnClean_Click(object sender, System.EventArgs e)
        {
            CleanAll();
        }

        private void btnStrClear_Click(object sender, System.EventArgs e)
        {
            this.StrClear();
        }

        private void btnAppend_Click(object sender, System.EventArgs e)
        {
            this.Append((new pjse.ResourceChooser()).Execute(wrapper.FileDescriptor.Type, wrapper.FileDescriptor.Group, strPanel, true));
        }

        private void btnStrCopy_Click(object sender, EventArgs e)
        {
            this.StrCopy();
        }

        private void btnImport_Click(object sender, System.EventArgs e)
        {
            this.StrReplace();
        }

        private void btnCommit_Click(object sender, System.EventArgs e)
        {
            this.Commit();
        }

        private void btnStringFile_Click(object sender, EventArgs e)
        {
            this.StringFile(sender.Equals(this.btnImport));
        }

        private void strPanel_Paint(object sender, PaintEventArgs e)
        {

        }

    }

}
