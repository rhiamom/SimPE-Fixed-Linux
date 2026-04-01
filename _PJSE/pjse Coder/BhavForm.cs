/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
 *   pljones@users.sf.net                                                  *
 *                                                                         *
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                *
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
using System.ComponentModel;
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using DialogResult = SimPe.Scenegraph.Compat.DialogResult;
using MessageBoxButtons = SimPe.Scenegraph.Compat.MessageBoxButtons;
using MessageBoxIcon = SimPe.Scenegraph.Compat.MessageBoxIcon;
using SimPe.Interfaces;
using SimPe.Interfaces.Files;
using SimPe.Interfaces.Plugin;
using SimPe.Interfaces.Scenegraph;
using SimPe.PackedFiles.Wrapper;
using pjse;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// Summary description for BhavForm.
	/// </summary>
	public class BhavForm : Window, IPackedFileUI
	{
		#region Form variables

        private LabelCompat lbFilename;
		private LabelCompat lbFormat;
		private LabelCompat lbType;
		private LabelCompat lbLocalC;
		private TextBoxCompat tbFilename;
		private TextBoxCompat tbType;
		private TextBoxCompat tbArgC;
		private TextBoxCompat tbLocalC;
		private ComboBoxCompat tba1;
		private ComboBoxCompat tba2;
		private LinkLabel llopenbhav;
		private LabelCompat label9;
		private LabelCompat label10;
		private LabelCompat label11;
		private LabelCompat label12;
		private LabelCompat label13;
		private TextBoxCompat tbInst_OpCode;
		private TextBoxCompat tbInst_Op7;
		private TextBoxCompat tbInst_Op6;
		private TextBoxCompat tbInst_Op5;
		private TextBoxCompat tbInst_Op4;
		private TextBoxCompat tbInst_Op3;
		private TextBoxCompat tbInst_Op2;
		private TextBoxCompat tbInst_Op1;
		private TextBoxCompat tbInst_Op0;
		private TextBoxCompat tbInst_Unk7;
		private TextBoxCompat tbInst_Unk6;
		private TextBoxCompat tbInst_Unk5;
		private TextBoxCompat tbInst_Unk4;
		private TextBoxCompat tbInst_Unk3;
		private TextBoxCompat tbInst_Unk2;
		private TextBoxCompat tbInst_Unk1;
		private TextBoxCompat tbInst_Unk0;
		private GroupBox gbInstruction;
		private StackPanel bhavPanel;
		private ButtonCompat btnCommit;
		private ButtonCompat btnOpCode;
		private ButtonCompat btnOperandWiz;
		private ButtonCompat btnSort;
		private LabelCompat lbUpDown;
		private TextBoxCompat tbLines;
		private ButtonCompat btnUp;
		private ButtonCompat btnDown;
		private ButtonCompat btnDel;
		private ButtonCompat btnAdd;
		private ButtonCompat btnCancel;
        private SimPe.PackedFiles.UserInterface.BhavInstListControl pnflowcontainer;
		private GroupBox gbMove;
		private LabelCompat lbArgC;
		private GroupBox gbSpecial;
		private ButtonCompat btnInsTrue;
		private ButtonCompat btnInsFalse;
		private ButtonCompat btnLinkInge;
		private ButtonCompat btnDelPescado;
		private ButtonCompat btnAppend;
		private ComboBoxCompat cbFormat;
		private ButtonCompat btnDelMerola;
		private LabelCompat lbCacheFlags;
		private TextBoxCompat tbCacheFlags;
		private LabelCompat lbTreeVersion;
		private TextBoxCompat tbTreeVersion;
		private TextBoxCompat tbHeaderFlag;
		private LabelCompat lbHeaderFlag;
		private ButtonCompat btnOperandRaw;
		private TextBoxCompat tbInst_NodeVersion;
		private ButtonCompat btnClose;
		private CheckBoxCompat2 cbSpecial;
		private TextBoxCompat tbInst_Longname;
        private ButtonCompat btnCopyListing;
        private ButtonCompat btnTPRPMaker;
        private ButtonCompat btnGUIDIndex;
        private ContextMenuStrip cmenuGUIDIndex;
        private ToolStripMenuItem createAllPackagesToolStripMenuItem;
        private ToolStripMenuItem createCurrentPackageToolStripMenuItem;
        private ToolStripMenuItem loadIndexToolStripMenuItem;
        private ToolStripMenuItem defaultFileToolStripMenuItem;
        private ToolStripMenuItem fromFileToolStripMenuItem;
        private ToolStripMenuItem saveIndexToolStripMenuItem;
        private ToolStripMenuItem defaultFileToolStripMenuItem1;
        private ToolStripMenuItem toFileToolStripMenuItem;
        private ButtonCompat btnCopyBHAV;
        private TextBoxCompat tbHidesOP;
        private LinkLabel llHidesOP;
        private LabelCompat lbHidesOP;
        private ButtonCompat btnPasteListing;
        private ButtonCompat btnZero;
        private SimPe.Scenegraph.Compat.ToolTip ttBhavForm;
        private pjse_banner pjse_banner1;
        private CompareButton cmpBHAV;
        private ButtonCompat btnInsUnlinked;
        private ButtonCompat btnImportBHAV;
        private ButtonCompat button1;
        private IContainer components;
        #endregion
       
		public BhavForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            hidesFmt = llHidesOP.Text;
			this.Tag = "Normal"; // Used by SetReadOnly
            TextBoxCompat[] iob = {
								 tbInst_Op0  ,tbInst_Op1  ,tbInst_Op2  ,tbInst_Op3
								,tbInst_Op4  ,tbInst_Op5  ,tbInst_Op6  ,tbInst_Op7
								,tbInst_Unk0 ,tbInst_Unk1 ,tbInst_Unk2 ,tbInst_Unk3
								,tbInst_Unk4 ,tbInst_Unk5 ,tbInst_Unk6 ,tbInst_Unk7
								,tbInst_NodeVersion
								,tbHeaderFlag
								,tbType
								,tbCacheFlags
								,tbArgC
								,tbLocalC
							};
			alHex8 = new ArrayList(iob);

            TextBoxCompat[] w = { tbInst_OpCode ,tbLines ,};
			alHex16 = new ArrayList(w);

			TextBoxCompat[] dw = { tbTreeVersion ,};
			alHex32 = new ArrayList(dw);

			ComboBoxCompat[] cb = { tba1 ,tba2 ,cbFormat ,};
			alHex16cb = new ArrayList(cb);

            this.button1.IsVisible = (UserVerification.HaveValidUserId && !Helper.XmlRegistry.HiddenMode);

			this.cbSpecial.IsChecked = pjse.Settings.PJSE.ShowSpecialButtons;
			this.gbSpecial.IsVisible = pjse.Settings.PJSE.ShowSpecialButtons;

			pjse.FileTable.GFT.FiletableRefresh += (s, e) => this.FiletableRefresh(s, e);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose()
		{
			if (setHandler && wrapper != null)
			{
                wrapper.FileDescriptor.DescriptionChanged -= new EventHandler(FileDescriptor_DescriptionChanged);
                wrapper.WrapperChanged -= new System.EventHandler(this.WrapperChanged);
				setHandler = false;
			}
			wrapper = null;
			currentInst = null;
			origInst = null;
            alHex8 = alHex16 = alHex32 = alDec8 = alHex16cb = null;
		}

		
		#region BhavForm
		private Bhav wrapper;
		private bool setHandler = false;
		private BhavWiz currentInst;
		private Instruction origInst;
		private bool internalchg;
        private ArrayList alHex8;
		private ArrayList alHex16;
		private ArrayList alHex32;
		private ArrayList alDec8;
		private ArrayList alHex16cb;
        private String hidesFmt = "{0}";

        // These should be on the ExtendedWrapper class or BhavWiz or, indeed, PackedFileDescriptor
        private static IPackedFileDescriptor newPFD(IPackedFileDescriptor oldPFD) { return newPFD(oldPFD.Type, oldPFD.Group, oldPFD.SubType, oldPFD.Instance); }
        private static IPackedFileDescriptor newPFD(uint type, uint group, uint instance) { return newPFD(type, group, 0x00000000, instance); }
        private static IPackedFileDescriptor newPFD(uint type, uint group, uint subtype, uint instance)
        {
            IPackedFileDescriptor npfd = new SimPe.Packages.PackedFileDescriptor();
            npfd.Type = type;
            npfd.Group = group;
            npfd.SubType = subtype;
            npfd.Instance = instance;
            return npfd;
        }

        private IPackageFile currentPackage = null;
        private void TakeACopy()
        {
            IPackedFileDescriptor npfd = newPFD(wrapper.FileDescriptor);
            npfd.UserData = wrapper.Package.Read(wrapper.FileDescriptor).UncompressedData;
            currentPackage.Add(npfd, true);
        }

        private delegate bool ignoreEntry(pjse.FileTable.Entry i, IPackedFileDescriptor npfd);
        private delegate bool matchItem(object o, uint inst);
        private delegate void setter(object o, ushort inst);

        private void doUpdate(string typeName
            , uint oldInst
            , IPackedFileDescriptor npfd
            , pjse.FileTable.Entry[] entries
            , ignoreEntry ieDelegate
            , matchItem[] matchDelegates
            , setter[] setDelegates
            )
        {
            if (npfd == null) return;
            if (entries == null || entries.Length == 0) return;
            if (matchDelegates == null || matchDelegates.Length == 0) return;
            if (setDelegates == null || setDelegates.Length != matchDelegates.Length) return;

            WaitingScreen.Message = "Updating current package - " + typeName + "s...";
            foreach (pjse.FileTable.Entry i in entries)
            {
                ResourceLoader.Refresh(i); // make sure it's been saved before we search it
                // Application.DoEvents() not needed in Avalonia

                AbstractWrapper wrapper = i.Wrapper;
                if (wrapper as IEnumerable == null) break;

                if (ieDelegate != null && ieDelegate(i, npfd)) continue;

                foreach (object o in (IEnumerable)wrapper)
                {
                    for (int j = 0; j < matchDelegates.Length; j++)
                    {
                        matchItem md = matchDelegates[j];
                        setter sd = setDelegates[j];
                        if (md != null && sd != null && md(o, oldInst))
                        {
                            sd(o, (ushort)npfd.Instance);
                        }
                    }
                }
                if (wrapper.Changed)
                {
                    wrapper.SynchronizeUserData();
                    ResourceLoader.Refresh(i);
                }
            }
        }
        private void ImportBHAV()
        {
            WaitingScreen.Wait();

            #region Finding available BHAV number
            WaitingScreen.Message = "Finding available BHAV number...";
            pjse.FileTable.Entry[] ai = pjse.FileTable.GFT[Bhav.Bhavtype, pjse.FileTable.Source.Local];
            ushort newInst = 0x0fff;
            foreach (pjse.FileTable.Entry i in ai) if (i.Instance >= 0x1000 && i.Instance < 0x2000 && i.Instance > newInst) newInst = (ushort)i.Instance;
            newInst++;
            #endregion

            currentPackage.BeginUpdate();

            #region Cloning BHAV
            WaitingScreen.Message = "Cloning BHAV...";
            IPackedFileDescriptor npfd = newPFD(Bhav.Bhavtype, 0xffffffff, newInst);
            npfd.UserData = wrapper.Package.Read(wrapper.FileDescriptor).UncompressedData;
            currentPackage.Add(npfd, true);
            #endregion

            #region Updating current package - BHAVs
            doUpdate("BHAV"
                , wrapper.FileDescriptor.Instance
                , npfd
                , ai
                , delegate(pjse.FileTable.Entry i, IPackedFileDescriptor pfd) { return (i.Group != pfd.Group || i.Instance < 0x1000 || i.Instance >= 0x2000); }
                , new matchItem[] { delegate(object o, uint value) {
                    return ((Instruction)o).OpCode == value; } }
                , new setter[] { delegate(object o, ushort value) { ((Instruction)o).OpCode = value; } }
                );
            #endregion

            #region Updating current package - OBJFs
            doUpdate("OBJF"
                , wrapper.FileDescriptor.Instance
                , npfd
                , pjse.FileTable.GFT[Objf.Objftype, pjse.FileTable.Source.Local]
                , null
                , new matchItem[] {
                    delegate(object o, uint value) { return ((ObjfItem)o).Action == value; },
                    delegate(object o, uint value) { return ((ObjfItem)o).Guardian == value; },
                }
                , new setter[] {
                    delegate(object o, ushort value) { ((ObjfItem)o).Action = value; },
                    delegate(object o, ushort value) { ((ObjfItem)o).Guardian = value; },
                }
                );
            #endregion

            #region Updating current package - TTABs
            doUpdate("TTAB"
                , wrapper.FileDescriptor.Instance
                , npfd
                , pjse.FileTable.GFT[Ttab.Ttabtype, pjse.FileTable.Source.Local]
                , null
                , new matchItem[] {
                    delegate(object o, uint value) { return ((TtabItem)o).Action == value; },
                    delegate(object o, uint value) { return ((TtabItem)o).Guardian == value; },
                }
                , new setter[] {
                    delegate(object o, ushort value) { ((TtabItem)o).Action = value; },
                    delegate(object o, ushort value) { ((TtabItem)o).Guardian = value; },
                }
                );
            #endregion

            currentPackage.EndUpdate();

            WaitingScreen.Message = "";
            WaitingScreen.Stop();
            SimPe.Scenegraph.Compat.MessageBox.ShowAsync(
                pjse.Localization.GetString("ml_done")
                , btnImportBHAV.Content?.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information).GetAwaiter().GetResult();
        }

        private void cmpBHAV_CompareWith(object sender, CompareButton.CompareWithEventArgs e) { common_LinkClicked(e.Item, e.ExpansionItem, true); }
        private void common_LinkClicked(pjse.FileTable.Entry item) { common_LinkClicked(item, null, false); }
        private void common_LinkClicked(pjse.FileTable.Entry item, SimPe.ExpansionItem exp, bool noOverride)
        {
            if (item == null) return; // this should never happen
            Bhav bhav = new Bhav();
            bhav.ProcessData(item.PFD, item.Package);

            BhavForm ui = (BhavForm)bhav.UIHandler;
            string tag = "Popup"; // tells the SetReadOnly function it's in a popup - so everything locked down
            if (noOverride) tag += ";noOverride"; // prevents handleOverride displaying anything
            tag += ";callerID=+" + wrapper.FileDescriptor.ExportFileName +"+";
            if (exp != null) tag += ";expName=+" + exp.NameShort + "+";
            ui.Tag = tag;

            bhav.RefreshUI();
            ui.Show();
        }

        private string getValueFromTag(string key)
        {
            string s = this.Tag as string;
            if (s == null) return null;

            key = ";" + key + "=+";
            int i = s.IndexOf(key);
            if (i < 0) return null;

            s = s.Substring(i + key.Length);
            i = s.IndexOf("+");
            return (i >= 0) ? s.Substring(0, i) : null;
        }
        private bool isPopup { get { return (this.Tag == null || this.Tag as string == null) ? false : ((string)(this.Tag)).StartsWith("Popup"); } }
        private bool isNoOverride { get { return (this.Tag == null || this.Tag as string == null) ? false : ((string)(this.Tag)).Contains(";noOverride"); } }
        private string callerID { get { return getValueFromTag("callerID"); } }
        private string expName
        {
            get
            {
                string s = getValueFromTag("expName");
                if (s != null) return s;

                foreach(pjse.FileTable.Entry item in pjse.FileTable.GFT[wrapper.Package, wrapper.FileDescriptor])
                    if (item.PFD == wrapper.FileDescriptor)
                    {
                        if (item.IsMaxis) return pjse.Localization.GetString("expCurrent");
                        else break;
                    }
                return pjse.Localization.GetString("expCustom");
            }
        }

        private String formTitle
        {
            get
            {
                return pjse.Localization.GetString("pjseWindowTitle"
                    , expName // EP Name or Custom
                    , System.IO.Path.GetFileName(wrapper.Package.SaveFileName) // package Filename without path
                    , wrapper.FileDescriptor.TypeName.shortname // Type (short name)
                    , "0x" + SimPe.Helper.HexString(wrapper.FileDescriptor.Group) // Group Number
                    , "0x" + SimPe.Helper.HexString((ushort)wrapper.FileDescriptor.Instance) // Instance Number
                    , wrapper.FileName
                    ,  pjse.Localization.GetString(isPopup ? "pjseWindowTitleView" : "pjseWindowTitleEdit") // View or Edit
                    );
            }
        }

        private void handleOverride()
        {
            lbHidesOP.IsVisible = tbHidesOP.IsVisible = llHidesOP.IsVisible = false;
            llHidesOP.Tag = null;
            if (this.isNoOverride) return;

            pjse.FileTable.Entry[] items = pjse.FileTable.GFT[wrapper.Package, wrapper.FileDescriptor];
            
            if (items.Length > 1) // currentpkg, other, fixed, maxis
            {
                pjse.FileTable.Entry item = items[items.Length - 1];
                if (item.PFD == wrapper.FileDescriptor) return;
                if (!item.IsMaxis && !item.IsFixed) return;

                this.lbHidesOP.IsVisible = this.tbHidesOP.IsVisible = this.llHidesOP.IsVisible = true;
                llHidesOP.Links[0].Start -= llHidesOP.Text.Length;
                llHidesOP.Content = hidesFmt.Replace("{0}", System.IO.Path.GetFileName(item.Package.SaveFileName));
                llHidesOP.Links[0].Start += llHidesOP.Text.Length;
                this.tbHidesOP.Text = wrapper.Package.FileName;
                llHidesOP.Tag = item.IsMaxis ? pjse.FileTable.Source.Maxis : pjse.FileTable.Source.Fixed;
            }
        }

        private void SetReadOnly(bool state) 
		{
            //if (this.isPopup) state = true;

            this.tbInst_OpCode.IsReadOnly = state;
			this.btnOpCode.IsEnabled = !state;
			this.tbInst_NodeVersion.IsReadOnly = state || wrapper.Header.Format < 0x8005;
			this.tba1.IsEnabled = !state;
			this.tba2.IsEnabled = !state;

			/*this.tbInst_Op01_dec.IsReadOnly = state;
			this.tbInst_Op23_dec.IsReadOnly = state;*/

			this.tbInst_Op0.IsReadOnly = state;
			this.tbInst_Op1.IsReadOnly = state;
			this.tbInst_Op2.IsReadOnly = state;
			this.tbInst_Op3.IsReadOnly = state;
			this.tbInst_Op4.IsReadOnly = state;
			this.tbInst_Op5.IsReadOnly = state;
			this.tbInst_Op6.IsReadOnly = state;
			this.tbInst_Op7.IsReadOnly = state;

			this.btnOperandWiz.IsEnabled = !state;
			/*this.btnOperandRaw.IsEnabled = !state;*/
            this.btnZero.IsEnabled = !state;
			
			this.tbInst_Unk0.IsReadOnly = state || wrapper.Header.Format < 0x8003;
			this.tbInst_Unk1.IsReadOnly = state || wrapper.Header.Format < 0x8003;
			this.tbInst_Unk2.IsReadOnly = state || wrapper.Header.Format < 0x8003;
			this.tbInst_Unk3.IsReadOnly = state || wrapper.Header.Format < 0x8003;
			this.tbInst_Unk4.IsReadOnly = state || wrapper.Header.Format < 0x8003;
			this.tbInst_Unk5.IsReadOnly = state || wrapper.Header.Format < 0x8003;
			this.tbInst_Unk6.IsReadOnly = state || wrapper.Header.Format < 0x8003;
			this.tbInst_Unk7.IsReadOnly = state || wrapper.Header.Format < 0x8003;

			this.btnUp.IsEnabled = !state;
			this.btnDown.IsEnabled = !state;
			this.tbLines.IsReadOnly = state;
			this.btnDelPescado.IsEnabled = this.btnDel.IsEnabled = !state;
			this.btnInsTrue.IsEnabled = this.btnInsFalse.IsEnabled = this.btnAdd.IsEnabled = !state;
		}

        private bool instIsBhav()
        {
            return wrapper.ResourceByInstance(SimPe.Data.MetaData.BHAV_FILE, currentInst.Instruction.OpCode) != null;
        }

        private void OperandWiz(int type)
        {
            internalchg = true;
            bool changed = false;
            Instruction inst = currentInst.Instruction;
            currentInst = null;
            try
            {
                changed = ((new BhavOperandWiz()).Execute(btnCommit.IsVisible ? inst : inst.Clone(), type) != null);
            }
            finally
            {
                currentInst = inst;
                if (btnCommit.IsVisible)
                {
                    if (changed) UpdateInstPanel();
                    this.btnCancel.IsEnabled = true;
                }
                internalchg = false;
            }
        }

        private void UpdateInstPanel()
		{
			internalchg = true;
			if (currentInst == null || wrapper.IndexOf(currentInst.Instruction) < 0)
			{
				SetReadOnly(true);
				this.llopenbhav.IsEnabled = false;
				this.btnInsTrue.IsEnabled = this.btnInsFalse.IsEnabled = this.btnAdd.IsEnabled = true;

				this.tbInst_OpCode.Text = "";
				this.tbInst_NodeVersion.Text = "";
				this.tba1.SelectedIndex = 0;
				this.tba2.SelectedIndex = 0;
				this.tbInst_Op0.Text = "";
				this.tbInst_Op1.Text = "";
				this.tbInst_Op2.Text = "";
				this.tbInst_Op3.Text = "";
				this.tbInst_Op4.Text = "";
				this.tbInst_Op5.Text = "";
				this.tbInst_Op6.Text = "";
				this.tbInst_Op7.Text = "";
				this.tbInst_Unk0.Text = "";
				this.tbInst_Unk1.Text = "";
				this.tbInst_Unk2.Text = "";
				this.tbInst_Unk3.Text = "";
				this.tbInst_Unk4.Text = "";
				this.tbInst_Unk5.Text = "";
				this.tbInst_Unk6.Text = "";
				this.tbInst_Unk7.Text = "";
			}
			else
			{
				Instruction inst = currentInst.Instruction; // saves typing

				SetReadOnly(false);

				this.tbInst_OpCode.Text = "0x"+Helper.HexString(inst.OpCode);
				this.tbInst_NodeVersion.Text = "0x"+Helper.HexString(inst.NodeVersion);
				if (inst.Target1 >= 0xFFFC && inst.Target1 < 0xFFFF)
				{
					this.tba1.SelectedIndex = inst.Target1 - 0xFFFC;
				}
				else
				{
					this.tba1.SelectedIndex = -1;
					this.tba1.Text = "0x"+Helper.HexString(inst.Target1);
				}
				if (inst.Target2 >= 0xFFFC && inst.Target2 < 0xFFFF)
				{
					this.tba2.SelectedIndex = inst.Target2 - 0xFFFC;
				}
				else
				{
					this.tba2.SelectedIndex = -1;
					this.tba2.Text = "0x"+Helper.HexString(inst.Target2);
				}

				this.tbInst_Op0.Text = Helper.HexString(inst.Operands[0]);
				this.tbInst_Op1.Text = Helper.HexString(inst.Operands[1]);
				this.tbInst_Op2.Text = Helper.HexString(inst.Operands[2]);
				this.tbInst_Op3.Text = Helper.HexString(inst.Operands[3]);
				this.tbInst_Op4.Text = Helper.HexString(inst.Operands[4]);
				this.tbInst_Op5.Text = Helper.HexString(inst.Operands[5]);
				this.tbInst_Op6.Text = Helper.HexString(inst.Operands[6]);
				this.tbInst_Op7.Text = Helper.HexString(inst.Operands[7]);
				this.tbInst_Unk0.Text = Helper.HexString(inst.Reserved1[0]);
				this.tbInst_Unk1.Text = Helper.HexString(inst.Reserved1[1]);
				this.tbInst_Unk2.Text = Helper.HexString(inst.Reserved1[2]);
				this.tbInst_Unk3.Text = Helper.HexString(inst.Reserved1[3]);
				this.tbInst_Unk4.Text = Helper.HexString(inst.Reserved1[4]);
				this.tbInst_Unk5.Text = Helper.HexString(inst.Reserved1[5]);
				this.tbInst_Unk6.Text = Helper.HexString(inst.Reserved1[6]);
				this.tbInst_Unk7.Text = Helper.HexString(inst.Reserved1[7]);
				this.btnUp.IsEnabled = pnflowcontainer.SelectedIndex > 0;
				this.btnDown.IsEnabled = pnflowcontainer.SelectedIndex < wrapper.Count - 1;

				this.btnDelPescado.IsEnabled = this.btnDel.IsEnabled = wrapper.Count > 1;

                this.llopenbhav.IsEnabled = instIsBhav();
				this.btnOperandWiz.IsEnabled = currentInst.Wizard() != null;
			}
            setLongname();
            internalchg = false;
		}

        private void OpcodeChanged(ushort value)
        {
            currentInst.Instruction.OpCode = value; 
            this.currentInst = currentInst.Instruction;
            this.llopenbhav.IsEnabled = instIsBhav();
            this.btnOperandWiz.IsEnabled = currentInst.Wizard() != null;
            setLongname();
        }

        private void ChangeLongname(byte oldval, byte newval) { if (oldval != newval) setLongname(); }

        private static string onearg = pjse.Localization.GetString("oneArg");
        private static string manyargs = pjse.Localization.GetString("manyArgs");
        private void setLongname()
        {
            if (currentInst == null || wrapper.IndexOf(currentInst.Instruction) < 0)
                this.tbInst_Longname.Text = "";
            else
            {
                try
                {
                    this.tbInst_Longname.Text = currentInst.LongName.Replace(", ", ",\r\n  ")
                    .Replace(onearg + ": ", onearg + ":\r\n  ")
                    .Replace(manyargs + ": ", manyargs + ":\r\n  ")
                    ;
                }
                finally { }
            }
        }

		private void CopyListing()
		{
			string listing = "";

			int lines = wrapper.Count;
			for (short i = 0; i < lines; i++)
			{
				Instruction inst = wrapper[i];
				BhavWiz w = inst;

				string operands = "";
				for(int j = 0; j < 8; j++) operands += SimPe.Helper.HexString(inst.Operands[j]);
				for(int j = 0; j < 8; j++) operands += SimPe.Helper.HexString(inst.Reserved1[j]);

				listing += ("     "
					+ SimPe.Helper.HexString(i)
					+ " : " + SimPe.Helper.HexString(inst.OpCode)
                    + " : " + SimPe.Helper.HexString(inst.NodeVersion)
                    + " : " + SimPe.Helper.HexString(inst.Target1)
                    + " : " + SimPe.Helper.HexString(inst.Target2)
                    + " : " + operands
					+ "\r\n" + w.LongName + "\r\n\r\n");
			}

			_ = Clipboard?.SetTextAsync(listing);
		}

        private void PasteListing()
        {
            int i = 0;
            int origlen = wrapper.Count;

            string listing = Clipboard?.GetTextAsync().GetAwaiter().GetResult() ?? "";
            foreach (string line in listing.Split('\r', '\n'))
            {
                if (line.Length == 0) continue;
                string[] args = line.Split(':');
                if (args.Length != 6) continue;

                try
                {
                    if (Convert.ToUInt32(args[0].Trim(), 16) != i)
                        throw new Exception("Foo");

                    Instruction inst = new Instruction(wrapper);

                    inst.OpCode = Convert.ToUInt16(args[1].Trim(), 16);
                    inst.NodeVersion = Convert.ToByte(args[2].Trim(), 16);
                    inst.Target1 = Convert.ToUInt16(args[3].Trim(), 16);
                    inst.Target2 = Convert.ToUInt16(args[4].Trim(), 16);
                    for (int j = 0; j < 8; j++)
                        inst.Operands[j] = Convert.ToByte(args[5].Trim().Substring(j * 2, 2), 16);
                    for (int j = 0; j < 8; j++)
                        inst.Reserved1[j] = Convert.ToByte(args[5].Trim().Substring(16 + j * 2, 2), 16);

                    if (inst.Target1 < 0xfffc) inst.Target1 = (ushort)(inst.Target1 + origlen);
                    if (inst.Target2 < 0xfffc) inst.Target2 = (ushort)(inst.Target2 + origlen);

                    wrapper.Add(inst);
                }
                finally
                {
                    i++;
                }
            }
        }

        private void TPRPMaker()
        {
            try
            {
                int minArgc = 0;
                int minLocalC = 0;
                TPRP tprp = (TPRP)wrapper.SiblingResource(TPRP.TPRPtype); // find TPRP for this BHAV

                wrapper.Package.BeginUpdate();

                if (tprp != null && tprp.TextOnly)
                {
                    // if it exists but is unreadable, as if user wants to overwrite
                    SimPe.Scenegraph.Compat.DialogResult dr = SimPe.Scenegraph.Compat.MessageBox.ShowAsync(
                        pjse.Localization.GetString("ml_overwriteduff")
                        , btnTPRPMaker.Content?.ToString()
                        , MessageBoxButtons.OKCancel
                        , MessageBoxIcon.Warning).GetAwaiter().GetResult();
                    if (dr != SimPe.Scenegraph.Compat.DialogResult.OK)
                        return;
                    wrapper.Package.Remove(tprp.FileDescriptor);
                    tprp = null;
                }
                if (tprp != null)
                {
                    // if it exists ask if user wants to preserve content
                    SimPe.Scenegraph.Compat.DialogResult dr = SimPe.Scenegraph.Compat.MessageBox.ShowAsync(
                        pjse.Localization.GetString("ml_keeplabels")
                        , btnTPRPMaker.Content?.ToString()
                        , MessageBoxButtons.YesNoCancel
                        , MessageBoxIcon.Warning).GetAwaiter().GetResult();
                    if (dr == SimPe.Scenegraph.Compat.DialogResult.Cancel)
                        return;

                    if (!tprp.Package.Equals(wrapper.Package))
                    {
                        // Clone the original into this package
                        if (dr == SimPe.Scenegraph.Compat.DialogResult.Yes) Wait.MaxProgress = tprp.Count;
                        SimPe.Interfaces.Files.IPackedFileDescriptor npfd = newPFD(tprp.FileDescriptor);
                        TPRP ntprp = new TPRP();
                        ntprp.FileDescriptor = npfd;
                        wrapper.Package.Add(npfd, true);
                        if (dr == SimPe.Scenegraph.Compat.DialogResult.Yes) foreach (TPRPItem item in tprp) { ntprp.Add(item.Clone()); Wait.Progress++; }
                        tprp = ntprp;
                        tprp.SynchronizeUserData();
                        Wait.MaxProgress = 0;
                    }

                    if (dr == SimPe.Scenegraph.Compat.DialogResult.Yes)
                    {
                        minArgc = tprp.ParamCount;
                        minLocalC = tprp.LocalCount;
                    }
                    else
                        tprp.Clear();
                }
                else
                {
                    // create a new TPRP file
                    tprp = new TPRP();
                    tprp.FileDescriptor =
                        newPFD(TPRP.TPRPtype, wrapper.FileDescriptor.Group, wrapper.FileDescriptor.SubType, wrapper.FileDescriptor.Instance);
                    wrapper.Package.Add(tprp.FileDescriptor, true);
                    tprp.SynchronizeUserData();
                }

                Wait.MaxProgress = wrapper.Header.ArgumentCount - minArgc + wrapper.Header.LocalVarCount - minLocalC;
                tprp.FileName = wrapper.FileName;

                for (int arg = minArgc; arg < wrapper.Header.ArgumentCount; arg++)
                {
                    tprp.Add(new TPRPParamLabel(tprp));
                    tprp[false, tprp.ParamCount - 1].LabelCompat = BhavWiz.dnParam() + " " + arg.ToString();
                    Wait.Progress++;
                }
                for (int local = minLocalC; local < wrapper.Header.LocalVarCount; local++)
                {
                    tprp.Add(new TPRPLocalLabel(tprp));
                    tprp[true, tprp.LocalCount - 1].LabelCompat = BhavWiz.dnLocal() + " " + local.ToString();
                    Wait.Progress++;
                }
                tprp.SynchronizeUserData();
                wrapper.Package.EndUpdate();
            }
            finally
            {
                Wait.SubStop();
            }
            SimPe.Scenegraph.Compat.MessageBox.ShowAsync( pjse.Localization.GetString("ml_done"), btnTPRPMaker.Content?.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information).GetAwaiter().GetResult();
        }

        private void TPFWMaker() // Fuck
        {
            try
            {
                SimPe.Plugin.TreesPackedFileWrapper tpfw = (SimPe.Plugin.TreesPackedFileWrapper)wrapper.SiblingResource(0x54524545); // find TPFW for this BHAV
                if (tpfw != null) return;
                tpfw = new SimPe.Plugin.TreesPackedFileWrapper();
                tpfw.FileDescriptor = newPFD(0x54524545, wrapper.FileDescriptor.Group, wrapper.FileDescriptor.SubType, wrapper.FileDescriptor.Instance);
                tpfw.Count = 0;
                tpfw.FileNam = wrapper.FileName;
                for (int i = 0; i < wrapper.Count; i++)
                {
                    tpfw.AddBlock();
                }
                wrapper.Package.Add(tpfw.FileDescriptor, true);
                tpfw.SynchronizeUserData();
                pjse_banner1.TreeVisible = true;
                button1.IsEnabled = false;
                SimPe.Scenegraph.Compat.MessageBox.ShowAsync(pjse.Localization.GetString("ml_done"), "comments", MessageBoxButtons.OK, MessageBoxIcon.Information).GetAwaiter().GetResult();
            }
            catch { }
        }

        private void SetComments()
        {
            if (!UserVerification.HaveValidUserId || Helper.XmlRegistry.HiddenMode) return;
            SimPe.Plugin.TreesPackedFileWrapper tpfw = (SimPe.Plugin.TreesPackedFileWrapper)wrapper.SiblingResource(0x54524545);
            if (tpfw == null) return;
            int indx = 0;
            BhavInstListItemUI cc;
            foreach (Control LI in this.pnflowcontainer.Controls)
            {
                if (LI.GetType() == typeof(BhavInstListItemUI))
                {
                    cc = LI as BhavInstListItemUI;
                    cc.SetComment(tpfw.ReadComment(indx));
                    indx++;
                }
            }
        }

		private short OpsToShort(byte lo, byte hi)
		{
			ushort uval = (ushort)(lo + (hi << 8));
			if (uval > 32767) return (short)(uval - 65536);
			else return (short)uval;
		}

		private byte[] ShortToOps(short val)
		{
			byte[] ops = new byte[2];
			ushort uval;
			if (val < 0)
				uval = (ushort)(65536 + val);
			else
				uval = (ushort)val;
			ops[0] = (byte)(uval & 0xFF);
			ops[1] = (byte)((uval >> 8) & 0xFF);
			return ops;
		}

		private bool cbHex16_IsValid(object sender)
		{
			if (alHex16cb.IndexOf(sender) < 0)
				throw new Exception("cbHex16_IsValid not applicable to control " + sender.ToString());
			if (((ComboBoxCompat)sender).Items.IndexOf(((ComboBoxCompat)sender).Text) != -1) return true;

			try { Convert.ToUInt16(((ComboBoxCompat)sender).Text, 16); }
			catch (Exception) { return false; }
			return true;
		}

		private bool dec8_IsValid(object sender)
		{
			if (alDec8.IndexOf(sender) < 0)
				throw new Exception("dec8_IsValid not applicable to control " + sender.ToString());
			try { Convert.ToByte(((TextBoxCompat)sender).Text); }
			catch (Exception) { return false; }
			return true;
		}

		private bool hex8_IsValid(object sender)
		{
			if (alHex8.IndexOf(sender) < 0)
				throw new Exception("hex8_IsValid not applicable to control " + sender.ToString());
			try { Convert.ToByte(((TextBoxCompat)sender).Text, 16); }
			catch (Exception) { return false; }
			return true;
		}

		private bool hex16_IsValid(object sender)
		{
			if (alHex16.IndexOf(sender) < 0)
				throw new Exception("hex16_IsValid not applicable to control " + sender.ToString());
			try { Convert.ToUInt16(((TextBoxCompat)sender).Text, 16); }
			catch (Exception) { return false; }
			return true;
		}

		private bool hex32_IsValid(object sender)
		{
			if (alHex32.IndexOf(sender) < 0)
				throw new Exception("hex32_IsValid not applicable to control " + sender.ToString());
			try { Convert.ToUInt32(((TextBoxCompat)sender).Text, 16); }
			catch (Exception) { return false; }
			return true;
		}

		private void FiletableRefresh(object sender, System.EventArgs e)
		{
            pjse_banner1.SiblingEnabled = wrapper != null && wrapper.SiblingResource(TPRP.TPRPtype) != null;
            UpdateInstPanel();
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
				return bhavPanel;
			}
		}

		/// <summary>
		/// Called by the AbstractWrapper when the file should be displayed to the user.
		/// </summary>
		/// <param name="wrp">Reference to the wrapper to be displayed.</param>
		public void UpdateGUI(IFileWrapper wrp) // Fuck
		{
			wrapper = (Bhav) wrp;

			internalchg = true;
            this.tbLines.Text = "0x0001";
			internalchg = false;

			this.WrapperChanged(wrapper, null);
            pjse_banner1.SiblingEnabled = wrapper.SiblingResource(TPRP.TPRPtype) != null;

			currentInst = null;
			origInst = null;
			UpdateInstPanel();
			this.pnflowcontainer.UpdateGUI(wrapper);
			// pnflowcontainer to install its handler before us.
			if (!setHandler)
			{
				wrapper.WrapperChanged += (s, e) => this.WrapperChanged(s, e);
                wrapper.FileDescriptor.DescriptionChanged += new EventHandler(FileDescriptor_DescriptionChanged);
				setHandler = true;
			}

            if (this.isPopup)
            {
                currentPackage = pjse.FileTable.GFT.CurrentPackage;
                pjse_banner1.TreeVisible = pjse_banner1.ViewVisible = pjse_banner1.FloatVisible = false;
                btnClose.IsVisible = gbSpecial.IsVisible = true;
                button1.IsEnabled = cbSpecial.IsEnabled = false;
                btnCopyBHAV.IsVisible = (currentPackage != wrapper.Package);
                btnImportBHAV.IsVisible = (currentPackage != wrapper.Package)
                    && (callerID != null && callerID.IndexOf("-FFFFFFFF-") == 17); //42484156-00000000-FFFFFFFF-00001003
                btnCopyBHAV.IsEnabled = currentPackage != null;
                btnImportBHAV.IsEnabled = (currentPackage != null) &&
                    ((wrapper.FileDescriptor.Instance >= 0x100 && wrapper.FileDescriptor.Instance < 0x1000)
                    || (wrapper.FileDescriptor.Instance >= 0x2000 && wrapper.FileDescriptor.Instance < 0x3000));

                handleOverride();

                this.Content = formTitle;
                ttBhavForm.SetToolTip(tbFilename, null);
            }
            else
            {
                this.lbHidesOP.IsVisible = this.tbHidesOP.IsVisible = this.llHidesOP.IsVisible = false;
                this.llHidesOP.Tag = null;
                if (wrapper.SiblingResource(0x54524545) != null && UserVerification.HaveValidUserId && !Helper.XmlRegistry.HiddenMode)
                {
                    pjse_banner1.TreeVisible = true;
                    pjse_banner1.TreeEnabled = wrapper.SiblingResource(0x54524545).Package == wrapper.Package;
                    button1.IsEnabled = false;
                }
                else
                {
                    pjse_banner1.TreeVisible = false;
                    button1.IsEnabled = true;
                }
                currentPackage = wrapper.Package;
                ttBhavForm.SetToolTip(tbFilename, expName + ": 0x" + SimPe.Helper.HexString((ushort)wrapper.FileDescriptor.Instance));
            }
            SetComments();
        }

        void FileDescriptor_DescriptionChanged(object sender, EventArgs e)
        {
            pjse_banner1.SiblingEnabled = wrapper.SiblingResource(TPRP.TPRPtype) != null;
            if (isPopup)
                this.Content = formTitle;
            else
            {
                ttBhavForm.SetToolTip(tbFilename, expName + ": 0x" + SimPe.Helper.HexString((ushort)wrapper.FileDescriptor.Instance));
                pjse_banner1.TreeVisible = (wrapper.SiblingResource(0x54524545) != null && UserVerification.HaveValidUserId && !Helper.XmlRegistry.HiddenMode);
            }
            SetComments();
        }

        private void WrapperChanged(object sender, System.EventArgs e)
        {
            if (isPopup) wrapper.Changed = false;

            this.btnCommit.IsEnabled = wrapper.Changed;

            // Handler for header
            if (sender == wrapper && !internalchg)
            {
                internalchg = true;
                /*this.Text = */
                tbFilename.Text = wrapper.FileName;
                // cbFormat.Content = "0x" + Helper.HexString(wrapper.Header.Format); // TODO: set selected item
                tbType.Text = "0x" + Helper.HexString(wrapper.Header.Type);
                tbArgC.Text = "0x" + Helper.HexString(wrapper.Header.ArgumentCount);
                tbLocalC.Text = "0x" + Helper.HexString(wrapper.Header.LocalVarCount);
                tbHeaderFlag.Text = "0x" + Helper.HexString(wrapper.Header.HeaderFlag);
                tbTreeVersion.Text = "0x" + Helper.HexString(wrapper.Header.TreeVersion);
                tbCacheFlags.Text = "0x" + Helper.HexString(wrapper.Header.CacheFlags);
                tbCacheFlags.IsEnabled = (wrapper.Header.Format > 0x8008);
                cmpBHAV.Wrapper = wrapper;
                cmpBHAV.WrapperName = wrapper.FileName;
                internalchg = false;
            }

            // Handler for current instruction
            if (currentInst != null && sender == currentInst.Instruction)
            {
                if (internalchg)
                    this.btnCancel.IsEnabled = true;
                else
                    pnflowcontainer_SelectedInstChanged(null, null);
            }
            SetComments();
        }

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BhavForm));
            this.gbInstruction = new GroupBox();
            this.btnZero = new ButtonCompat();
            this.tbInst_Longname = new TextBoxCompat();
            this.btnOperandRaw = new ButtonCompat();
            this.btnCancel = new ButtonCompat();
            this.btnOperandWiz = new ButtonCompat();
            this.llopenbhav = new LinkLabel();
            this.tba2 = new ComboBoxCompat();
            this.tba1 = new ComboBoxCompat();
            this.label13 = new LabelCompat();
            this.tbInst_Unk7 = new TextBoxCompat();
            this.tbInst_Unk6 = new TextBoxCompat();
            this.tbInst_Unk5 = new TextBoxCompat();
            this.tbInst_Unk4 = new TextBoxCompat();
            this.tbInst_Unk3 = new TextBoxCompat();
            this.tbInst_Unk2 = new TextBoxCompat();
            this.tbInst_Unk1 = new TextBoxCompat();
            this.tbInst_Unk0 = new TextBoxCompat();
            this.tbInst_Op7 = new TextBoxCompat();
            this.tbInst_Op6 = new TextBoxCompat();
            this.tbInst_Op5 = new TextBoxCompat();
            this.tbInst_Op4 = new TextBoxCompat();
            this.tbInst_Op3 = new TextBoxCompat();
            this.tbInst_Op2 = new TextBoxCompat();
            this.tbInst_Op1 = new TextBoxCompat();
            this.tbInst_Op0 = new TextBoxCompat();
            this.tbInst_NodeVersion = new TextBoxCompat();
            this.tbInst_OpCode = new TextBoxCompat();
            this.label10 = new LabelCompat();
            this.label9 = new LabelCompat();
            this.label12 = new LabelCompat();
            this.label11 = new LabelCompat();
            this.btnOpCode = new ButtonCompat();
            this.tbFilename = new TextBoxCompat();
            this.lbFilename = new LabelCompat();
            this.tbLocalC = new TextBoxCompat();
            this.tbArgC = new TextBoxCompat();
            this.tbType = new TextBoxCompat();
            this.lbTreeVersion = new LabelCompat();
            this.lbType = new LabelCompat();
            this.lbLocalC = new LabelCompat();
            this.lbArgC = new LabelCompat();
            this.lbFormat = new LabelCompat();
            this.bhavPanel = new StackPanel();
            this.pjse_banner1 = new pjse.pjse_banner();
            this.lbHidesOP = new LabelCompat();
            this.gbSpecial = new GroupBox();
            this.button1 = new ButtonCompat();
            this.cmpBHAV = new pjse.CompareButton();
            this.btnPasteListing = new ButtonCompat();
            this.btnAppend = new ButtonCompat();
            this.btnInsTrue = new ButtonCompat();
            this.btnInsFalse = new ButtonCompat();
            this.btnDelPescado = new ButtonCompat();
            this.btnLinkInge = new ButtonCompat();
            this.btnGUIDIndex = new ButtonCompat();
            this.btnInsUnlinked = new ButtonCompat();
            this.btnDelMerola = new ButtonCompat();
            this.btnCopyListing = new ButtonCompat();
            this.btnTPRPMaker = new ButtonCompat();
            this.llHidesOP = new LinkLabel();
            this.tbHidesOP = new TextBoxCompat();
            this.cbSpecial = new CheckBoxCompat2();
            this.btnImportBHAV = new ButtonCompat();
            this.btnCopyBHAV = new ButtonCompat();
            this.btnClose = new ButtonCompat();
            this.tbHeaderFlag = new TextBoxCompat();
            this.lbHeaderFlag = new LabelCompat();
            this.tbCacheFlags = new TextBoxCompat();
            this.cbFormat = new ComboBoxCompat();
            this.pnflowcontainer = new SimPe.PackedFiles.UserInterface.BhavInstListControl();
            this.btnDel = new ButtonCompat();
            this.gbMove = new GroupBox();
            this.btnUp = new ButtonCompat();
            this.btnDown = new ButtonCompat();
            this.lbUpDown = new LabelCompat();
            this.tbLines = new TextBoxCompat();
            this.btnSort = new ButtonCompat();
            this.btnCommit = new ButtonCompat();
            this.tbTreeVersion = new TextBoxCompat();
            this.btnAdd = new ButtonCompat();
            this.lbCacheFlags = new LabelCompat();
            this.cmenuGUIDIndex = new ContextMenuStrip();
            this.createAllPackagesToolStripMenuItem = new ToolStripMenuItem();
            this.createCurrentPackageToolStripMenuItem = new ToolStripMenuItem();
            this.loadIndexToolStripMenuItem = new ToolStripMenuItem();
            this.defaultFileToolStripMenuItem = new ToolStripMenuItem();
            this.fromFileToolStripMenuItem = new ToolStripMenuItem();
            this.saveIndexToolStripMenuItem = new ToolStripMenuItem();
            this.defaultFileToolStripMenuItem1 = new ToolStripMenuItem();
            this.toFileToolStripMenuItem = new ToolStripMenuItem();
            this.ttBhavForm = new SimPe.Scenegraph.Compat.ToolTip();
            //
            // gbInstruction
            //            this.gbInstruction.Children.Add(this.btnZero);
            this.gbInstruction.Children.Add(this.tbInst_Longname);
            this.gbInstruction.Children.Add(this.btnOperandRaw);
            this.gbInstruction.Children.Add(this.btnCancel);
            this.gbInstruction.Children.Add(this.btnOperandWiz);
            this.gbInstruction.Children.Add(this.llopenbhav);
            this.gbInstruction.Children.Add(this.tba2);
            this.gbInstruction.Children.Add(this.tba1);
            this.gbInstruction.Children.Add(this.label13);
            this.gbInstruction.Children.Add(this.tbInst_Unk7);
            this.gbInstruction.Children.Add(this.tbInst_Unk6);
            this.gbInstruction.Children.Add(this.tbInst_Unk5);
            this.gbInstruction.Children.Add(this.tbInst_Unk4);
            this.gbInstruction.Children.Add(this.tbInst_Unk3);
            this.gbInstruction.Children.Add(this.tbInst_Unk2);
            this.gbInstruction.Children.Add(this.tbInst_Unk1);
            this.gbInstruction.Children.Add(this.tbInst_Unk0);
            this.gbInstruction.Children.Add(this.tbInst_Op7);
            this.gbInstruction.Children.Add(this.tbInst_Op6);
            this.gbInstruction.Children.Add(this.tbInst_Op5);
            this.gbInstruction.Children.Add(this.tbInst_Op4);
            this.gbInstruction.Children.Add(this.tbInst_Op3);
            this.gbInstruction.Children.Add(this.tbInst_Op2);
            this.gbInstruction.Children.Add(this.tbInst_Op1);
            this.gbInstruction.Children.Add(this.tbInst_Op0);
            this.gbInstruction.Children.Add(this.tbInst_NodeVersion);
            this.gbInstruction.Children.Add(this.tbInst_OpCode);
            this.gbInstruction.Children.Add(this.label10);
            this.gbInstruction.Children.Add(this.label9);
            this.gbInstruction.Children.Add(this.label12);
            this.gbInstruction.Children.Add(this.label11);
            this.gbInstruction.Children.Add(this.btnOpCode);
            this.gbInstruction.Name = "gbInstruction";
            // btnZero
            // 
            this.btnZero.Click += (s, e) => this.btnZero_Click(s, e);
            // 
            // tbInst_Longname
            //            this.tbInst_Longname.BorderStyle = 
            this.tbInst_Longname.Name = "tbInst_Longname";
            this.tbInst_Longname.IsReadOnly = true;
            // 
            // btnOperandRaw
            // 
            this.btnOperandRaw.Click += (s, e) => this.btnOperandRaw_Click(s, e);
            // 
            // btnCancel
            //            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Click += (s, e) => this.btnCancel_Clicked(s, e);
            // 
            // btnOperandWiz
            // 
            this.btnOperandWiz.Click += (s, e) => this.btnOperandWiz_Clicked(s, e);
            // 
            // llopenbhav
            //            this.llopenbhav.Name = "llopenbhav";
            this.llopenbhav.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llopenbhav_LinkClicked);
            // 
            // tba2

            this.tba2.Name = "tba2";
            this.tba2.QueryContinueDrag += (s, e) => this.ItemQueryContinueDragTarget(s, e as SimPe.Scenegraph.Compat.QueryContinueDragEventArgs ?? new SimPe.Scenegraph.Compat.QueryContinueDragEventArgs());
            this.tba2.DragOver += (s, e) => this.ItemDragEnter(s, e as SimPe.Scenegraph.Compat.DragEventArgs ?? new SimPe.Scenegraph.Compat.DragEventArgs());
            this.tba2.SelectionChanged += (s, e) => this.cbHex16_SelectedIndexChanged(s, e);
            this.tba2.GotFocus += (s, e) => this.cbHex16_Enter(s, e);
            this.tba2.DragDrop += (s, e) => this.ItemDrop(s, e as SimPe.Scenegraph.Compat.DragEventArgs ?? new SimPe.Scenegraph.Compat.DragEventArgs());
            this.tba2.DragEnter += (s, e) => this.ItemDragEnter(s, e as SimPe.Scenegraph.Compat.DragEventArgs ?? new SimPe.Scenegraph.Compat.DragEventArgs());
            this.tba2.LostFocus += (s, e) => this.cbHex16_Validated(s, e);
            this.tba2.TextChanged += (s, e) => this.cbHex16_TextChanged(s, e);
            // 
            // tba1

            this.tba1.Name = "tba1";
            this.tba1.QueryContinueDrag += (s, e) => this.ItemQueryContinueDragTarget(s, e as SimPe.Scenegraph.Compat.QueryContinueDragEventArgs ?? new SimPe.Scenegraph.Compat.QueryContinueDragEventArgs());
            this.tba1.DragOver += (s, e) => this.ItemDragEnter(s, e as SimPe.Scenegraph.Compat.DragEventArgs ?? new SimPe.Scenegraph.Compat.DragEventArgs());
            this.tba1.SelectionChanged += (s, e) => this.cbHex16_SelectedIndexChanged(s, e);
            this.tba1.GotFocus += (s, e) => this.cbHex16_Enter(s, e);
            this.tba1.DragDrop += (s, e) => this.ItemDrop(s, e as SimPe.Scenegraph.Compat.DragEventArgs ?? new SimPe.Scenegraph.Compat.DragEventArgs());
            this.tba1.DragEnter += (s, e) => this.ItemDragEnter(s, e as SimPe.Scenegraph.Compat.DragEventArgs ?? new SimPe.Scenegraph.Compat.DragEventArgs());
            this.tba1.LostFocus += (s, e) => this.cbHex16_Validated(s, e);
            this.tba1.TextChanged += (s, e) => this.cbHex16_TextChanged(s, e);
            // 
            // label13
            //            this.label13.Name = "label13";
            // 
            // tbInst_Unk7
            //            this.tbInst_Unk7.Name = "tbInst_Unk7";
            this.tbInst_Unk7.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Unk7.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Unk6
            //            this.tbInst_Unk6.Name = "tbInst_Unk6";
            this.tbInst_Unk6.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Unk6.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Unk5
            //            this.tbInst_Unk5.Name = "tbInst_Unk5";
            this.tbInst_Unk5.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Unk5.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Unk4
            //            this.tbInst_Unk4.Name = "tbInst_Unk4";
            this.tbInst_Unk4.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Unk4.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Unk3
            //            this.tbInst_Unk3.Name = "tbInst_Unk3";
            this.tbInst_Unk3.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Unk3.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Unk2
            //            this.tbInst_Unk2.Name = "tbInst_Unk2";
            this.tbInst_Unk2.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Unk2.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Unk1
            //            this.tbInst_Unk1.Name = "tbInst_Unk1";
            this.tbInst_Unk1.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Unk1.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Unk0
            //            this.tbInst_Unk0.Name = "tbInst_Unk0";
            this.tbInst_Unk0.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Unk0.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Op7
            //            this.tbInst_Op7.Name = "tbInst_Op7";
            this.tbInst_Op7.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Op7.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Op6
            //            this.tbInst_Op6.Name = "tbInst_Op6";
            this.tbInst_Op6.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Op6.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Op5
            //            this.tbInst_Op5.Name = "tbInst_Op5";
            this.tbInst_Op5.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Op5.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Op4
            //            this.tbInst_Op4.Name = "tbInst_Op4";
            this.tbInst_Op4.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Op4.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Op3
            //            this.tbInst_Op3.Name = "tbInst_Op3";
            this.tbInst_Op3.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Op3.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Op2
            //            this.tbInst_Op2.Name = "tbInst_Op2";
            this.tbInst_Op2.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Op2.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Op1
            //            this.tbInst_Op1.Name = "tbInst_Op1";
            this.tbInst_Op1.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Op1.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_Op0
            //            this.tbInst_Op0.Name = "tbInst_Op0";
            this.tbInst_Op0.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_Op0.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_NodeVersion
            //            this.tbInst_NodeVersion.Name = "tbInst_NodeVersion";
            this.tbInst_NodeVersion.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbInst_NodeVersion.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbInst_OpCode
            //            this.tbInst_OpCode.Name = "tbInst_OpCode";
            this.tbInst_OpCode.TextChanged += (s, e) => this.hex16_TextChanged(s, e);
            this.tbInst_OpCode.LostFocus += (s, e) => this.hex16_Validated(s, e);
            // label10
            //            this.label10.Name = "label10";
            // 
            // label9
            //            this.label9.Name = "label9";
            // 
            // label12
            //            this.label12.Name = "label12";
            // 
            // label11
            //            this.label11.Name = "label11";
            // 
            // btnOpCode
            //            this.btnOpCode.Name = "btnOpCode";
            this.btnOpCode.Click += (s, e) => this.btnOpCode_Clicked(s, e);
            // 
            // tbFilename
            //            this.tbFilename.Name = "tbFilename";
            this.tbFilename.TextChanged += (s, e) => this.tbFilename_TextChanged(s, e);
            this.tbFilename.LostFocus += (s, e) => this.tbFilename_Validated(s, e);
            // 
            // lbFilename
            //            this.lbFilename.Name = "lbFilename";
            // 
            // tbLocalC
            //            this.tbLocalC.Name = "tbLocalC";
            this.tbLocalC.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbLocalC.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbArgC
            //            this.tbArgC.Name = "tbArgC";
            this.tbArgC.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbArgC.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // tbType
            //            this.tbType.Name = "tbType";
            this.tbType.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbType.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // lbTreeVersion
            //            this.lbTreeVersion.Name = "lbTreeVersion";
            // 
            // lbType
            //            this.lbType.Name = "lbType";
            // 
            // lbLocalC
            //            this.lbLocalC.Name = "lbLocalC";
            // 
            // lbArgC
            //            this.lbArgC.Name = "lbArgC";
            // 
            // lbFormat
            //            this.lbFormat.Name = "lbFormat";
            // 
            // bhavPanel
            //            this.bhavPanel.Children.Add(this.pjse_banner1);
            this.bhavPanel.Children.Add(this.lbHidesOP);
            this.bhavPanel.Children.Add(this.gbSpecial);
            this.bhavPanel.Children.Add(this.llHidesOP);
            this.bhavPanel.Children.Add(this.tbHidesOP);
            this.bhavPanel.Children.Add(this.cbSpecial);
            this.bhavPanel.Children.Add(this.btnImportBHAV);
            this.bhavPanel.Children.Add(this.btnCopyBHAV);
            this.bhavPanel.Children.Add(this.btnClose);
            this.bhavPanel.Children.Add(this.tbHeaderFlag);
            this.bhavPanel.Children.Add(this.lbHeaderFlag);
            this.bhavPanel.Children.Add(this.tbCacheFlags);
            this.bhavPanel.Children.Add(this.cbFormat);
            this.bhavPanel.Children.Add(this.pnflowcontainer);
            this.bhavPanel.Children.Add(this.btnDel);
            this.bhavPanel.Children.Add(this.gbMove);
            this.bhavPanel.Children.Add(this.btnSort);
            this.bhavPanel.Children.Add(this.btnCommit);
            this.bhavPanel.Children.Add(this.lbFilename);
            this.bhavPanel.Children.Add(this.tbFilename);
            this.bhavPanel.Children.Add(this.gbInstruction);
            this.bhavPanel.Children.Add(this.tbLocalC);
            this.bhavPanel.Children.Add(this.tbTreeVersion);
            this.bhavPanel.Children.Add(this.tbArgC);
            this.bhavPanel.Children.Add(this.tbType);
            this.bhavPanel.Children.Add(this.lbTreeVersion);
            this.bhavPanel.Children.Add(this.lbType);
            this.bhavPanel.Children.Add(this.lbLocalC);
            this.bhavPanel.Children.Add(this.lbArgC);
            this.bhavPanel.Children.Add(this.lbFormat);
            this.bhavPanel.Children.Add(this.btnAdd);
            this.bhavPanel.Children.Add(this.lbCacheFlags);
            this.bhavPanel.Name = "bhavPanel";
            // 
            // pjse_banner1
            //            this.pjse_banner1.ExtractVisible = true;
            this.pjse_banner1.FloatVisible = true;
            this.pjse_banner1.Name = "pjse_banner1";
            this.pjse_banner1.SiblingVisible = true;
            this.pjse_banner1.ViewVisible = true;
            this.pjse_banner1.ExtractClick += (s, e) => this.pjse_banner1_ExtractClick(s, e);
            this.pjse_banner1.TreeClick += (s, e) => this.pjse_banner1_TreeClick(s, e);
            this.pjse_banner1.SiblingClick += (s, e) => this.pjse_banner1_SiblingClick(s, e);
            this.pjse_banner1.ViewClick += (s, e) => this.pjse_banner1_ViewClick(s, e);
            this.pjse_banner1.FloatClick += (s, e) => this.btnFloat_Click(s, e);
            // 
            // lbHidesOP
            //            this.lbHidesOP.Name = "lbHidesOP";
            // 
            // gbSpecial
            //            this.gbSpecial.Children.Add(this.button1);
            this.gbSpecial.Children.Add(this.cmpBHAV);
            this.gbSpecial.Children.Add(this.btnPasteListing);
            this.gbSpecial.Children.Add(this.btnAppend);
            this.gbSpecial.Children.Add(this.btnInsTrue);
            this.gbSpecial.Children.Add(this.btnInsFalse);
            this.gbSpecial.Children.Add(this.btnDelPescado);
            this.gbSpecial.Children.Add(this.btnLinkInge);
            this.gbSpecial.Children.Add(this.btnGUIDIndex);
            this.gbSpecial.Children.Add(this.btnInsUnlinked);
            this.gbSpecial.Children.Add(this.btnDelMerola);
            this.gbSpecial.Children.Add(this.btnCopyListing);
            this.gbSpecial.Children.Add(this.btnTPRPMaker);
            this.gbSpecial.Name = "gbSpecial";
            // button1
            //            this.button1.Name = "button1";
            this.button1.Click += (s, e) => this.button1_Click(s, e);
            // 
            // cmpBHAV
            //            this.cmpBHAV.Name = "cmpBHAV";
            this.cmpBHAV.Wrapper = null;
            this.cmpBHAV.WrapperName = null;
            this.cmpBHAV.CompareWith += new pjse.CompareButton.CompareWithEventHandler(this.cmpBHAV_CompareWith);
            // 
            // btnPasteListing
            //            this.btnPasteListing.Name = "btnPasteListing";
            this.btnPasteListing.Click += (s, e) => this.btnPasteListing_Click(s, e);
            // 
            // btnAppend
            //            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Click += (s, e) => this.btnAppend_Click(s, e);
            // 
            // btnInsTrue
            //            this.btnInsTrue.Name = "btnInsTrue";
            this.btnInsTrue.Click += (s, e) => this.btnInsVia_Click(s, e);
            // 
            // btnInsFalse
            //            this.btnInsFalse.Name = "btnInsFalse";
            this.btnInsFalse.Click += (s, e) => this.btnInsVia_Click(s, e);
            // 
            // btnDelPescado
            //            this.btnDelPescado.Name = "btnDelPescado";
            this.btnDelPescado.Click += (s, e) => this.btnDelPescado_Click(s, e);
            // 
            // btnLinkInge
            //            this.btnLinkInge.Name = "btnLinkInge";
            this.btnLinkInge.Click += (s, e) => this.btnLinkInge_Click(s, e);
            // 
            // btnGUIDIndex
            //            this.btnGUIDIndex.Name = "btnGUIDIndex";
            this.btnGUIDIndex.Click += (s, e) => this.btnGUIDIndex_Click(s, e);
            // 
            // btnInsUnlinked
            //            this.btnInsUnlinked.Name = "btnInsUnlinked";
            this.btnInsUnlinked.Click += (s, e) => this.btnInsUnlinked_Click(s, e);
            // 
            // btnDelMerola
            //            this.btnDelMerola.Name = "btnDelMerola";
            this.btnDelMerola.Click += (s, e) => this.btnDelMerola_Click(s, e);
            // 
            // btnCopyListing
            //            this.btnCopyListing.Name = "btnCopyListing";
            this.btnCopyListing.Click += (s, e) => this.btnCopyListing_Click(s, e);
            // 
            // btnTPRPMaker
            //            this.btnTPRPMaker.Name = "btnTPRPMaker";
            this.btnTPRPMaker.Click += (s, e) => this.btnTPRPMaker_Click(s, e);
            // 
            // llHidesOP
            //            this.llHidesOP.Name = "llHidesOP";
            this.llHidesOP.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llHidesOP_LinkClicked);
            // 
            // tbHidesOP
            //            this.tbHidesOP.BorderStyle = 
            this.tbHidesOP.Name = "tbHidesOP";
            this.tbHidesOP.IsReadOnly = true;
            // 
            // cbSpecial
            //            this.cbSpecial.Name = "cbSpecial";
            this.cbSpecial.CheckedChanged += (s, e) => this.cbSpecial_CheckStateChanged(s, e);
            // 
            // btnImportBHAV
            //            this.btnImportBHAV.Name = "btnImportBHAV";
            this.btnImportBHAV.Click += (s, e) => this.btnImportBHAV_Click(s, e);
            // 
            // btnCopyBHAV
            //            this.btnCopyBHAV.Name = "btnCopyBHAV";
            this.btnCopyBHAV.Click += (s, e) => this.btnCopyBHAV_Click(s, e);
            // 
            // btnClose
            //            this.btnClose
            this.btnClose.Name = "btnClose";
            this.btnClose.Click += (s, e) => this.btnClose_Click(s, e);
            // 
            // tbHeaderFlag
            //            this.tbHeaderFlag.Name = "tbHeaderFlag";
            this.tbHeaderFlag.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbHeaderFlag.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // lbHeaderFlag
            //            this.lbHeaderFlag.Name = "lbHeaderFlag";
            // 
            // tbCacheFlags
            //            this.tbCacheFlags.Name = "tbCacheFlags";
            this.tbCacheFlags.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbCacheFlags.LostFocus += (s, e) => this.hex8_Validated(s, e);
            // cbFormat

            this.cbFormat.Name = "cbFormat";
            this.cbFormat.SelectionChanged += (s, e) => this.cbHex16_SelectedIndexChanged(s, e);
            this.cbFormat.GotFocus += (s, e) => this.cbHex16_Enter(s, e);
            this.cbFormat.LostFocus += (s, e) => this.cbHex16_Validated(s, e);
            this.cbFormat.TextChanged += (s, e) => this.cbHex16_TextChanged(s, e);
            // 
            // pnflowcontainer
            //            this.pnflowcontainer.Name = "pnflowcontainer";
            this.pnflowcontainer.SelectedIndex = -1;
            this.pnflowcontainer.SelectedInstChanged += (s, e) => this.pnflowcontainer_SelectedInstChanged(s, e);
            // 
            // btnDel
            //            this.btnDel.Name = "btnDel";
            this.btnDel.Click += (s, e) => this.btnDel_Clicked(s, e);
            // 
            // gbMove
            //            this.gbMove.Children.Add(this.btnUp);
            this.gbMove.Children.Add(this.btnDown);
            this.gbMove.Children.Add(this.lbUpDown);
            this.gbMove.Children.Add(this.tbLines);
            this.gbMove.Name = "gbMove";
            // btnUp
            // 
            this.btnUp.Click += (s, e) => this.btnMove_Clicked(s, e);
            // 
            // btnDown
            // 
            this.btnDown.Click += (s, e) => this.btnMove_Clicked(s, e);
            // 
            // lbUpDown
            //            this.lbUpDown.Name = "lbUpDown";
            // 
            // tbLines
            //            this.tbLines.Name = "tbLines";
            this.tbLines.TextChanged += (s, e) => this.hex16_TextChanged(s, e);
            this.tbLines.LostFocus += (s, e) => this.hex16_Validated(s, e);
            // btnSort
            //            this.btnSort.Name = "btnSort";
            this.btnSort.Click += (s, e) => this.btnSort_Clicked(s, e);
            // 
            // btnCommit
            //            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Click += (s, e) => this.btnCommit_Clicked(s, e);
            // 
            // tbTreeVersion
            //            this.tbTreeVersion.Name = "tbTreeVersion";
            this.tbTreeVersion.TextChanged += (s, e) => this.hex32_TextChanged(s, e);
            this.tbTreeVersion.LostFocus += (s, e) => this.hex32_Validated(s, e);
            // btnAdd
            //            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += (s, e) => this.btnAdd_Clicked(s, e);
            // 
            // lbCacheFlags
            //            this.lbCacheFlags.Name = "lbCacheFlags";
            // 
            // cmenuGUIDIndex
            // 
            this.cmenuGUIDIndex.Name = "cmenuGUIDIndex";            this.cmenuGUIDIndex.Opening += new System.EventHandler(this.cmenuGUIDIndex_Opening);
            // 
            // createAllPackagesToolStripMenuItem
            // 
            this.createAllPackagesToolStripMenuItem.Name = "createAllPackagesToolStripMenuItem";            this.createAllPackagesToolStripMenuItem.Click += (s, e) => this.createToolStripMenuItem_Click(s, e);
            // 
            // createCurrentPackageToolStripMenuItem
            // 
            this.createCurrentPackageToolStripMenuItem.Name = "createCurrentPackageToolStripMenuItem";            this.createCurrentPackageToolStripMenuItem.Click += (s, e) => this.createToolStripMenuItem_Click(s, e);
            // 
            // loadIndexToolStripMenuItem
            // 
            this.loadIndexToolStripMenuItem.DropDownItems.AddRange(new object[] {
            this.defaultFileToolStripMenuItem,
            this.fromFileToolStripMenuItem});
            this.loadIndexToolStripMenuItem.Name = "loadIndexToolStripMenuItem";            // 
            // defaultFileToolStripMenuItem
            // 
            this.defaultFileToolStripMenuItem.Name = "defaultFileToolStripMenuItem";            this.defaultFileToolStripMenuItem.Click += (s, e) => this.defaultFileToolStripMenuItem_Click(s, e);
            // 
            // fromFileToolStripMenuItem
            // 
            this.fromFileToolStripMenuItem.Name = "fromFileToolStripMenuItem";            this.fromFileToolStripMenuItem.Click += (s, e) => this.fileToolStripMenuItem_Click(s, e);
            // 
            // saveIndexToolStripMenuItem
            // 
            this.saveIndexToolStripMenuItem.DropDownItems.AddRange(new object[] {
            this.defaultFileToolStripMenuItem1,
            this.toFileToolStripMenuItem});
            this.saveIndexToolStripMenuItem.Name = "saveIndexToolStripMenuItem";            // 
            // defaultFileToolStripMenuItem1
            // 
            this.defaultFileToolStripMenuItem1.Name = "defaultFileToolStripMenuItem1";            this.defaultFileToolStripMenuItem1.Click += (s, e) => this.defaultFileToolStripMenuItem_Click(s, e);
            // 
            // toFileToolStripMenuItem
            // 
            this.toFileToolStripMenuItem.Name = "toFileToolStripMenuItem";            this.toFileToolStripMenuItem.Click += (s, e) => this.fileToolStripMenuItem_Click(s, e);
            // 
            // ttBhavForm
            // 
            this.ttBhavForm.ShowAlways = true;
            // 
            // BhavForm
            // 
            this.Name = "BhavForm";
        }

		#endregion

		private void pnflowcontainer_SelectedInstChanged(object sender, System.EventArgs e)
		{
			int index = pnflowcontainer.SelectedIndex;
			if (index < 0 || index >= wrapper.Count)
			{
				currentInst = null;
				origInst = null;
			}
			else
			{
				currentInst = wrapper[index];
				origInst = wrapper[index].Clone();
			}
			UpdateInstPanel();
			this.btnCancel.IsEnabled = false;
		}

		private void ItemQueryContinueDragTarget(object sender, QueryContinueDragEventArgs e)
		{
			if (e.KeyState==0) e.Action = DragAction.Drop;
			else e.Action = DragAction.Continue;
		}

		private void ItemDragEnter(object sender, DragEventArgs e)
		{
			// Drag-and-drop: Avalonia IDataObject has different API
			// if (e.Data.Contains(DataFormats.Text)) e.Effect = DragDropEffects.Link;
		}

		private void ItemDrop(object sender, System.EventArgs e)
		{
			int sel = 0;
			// Drag-and-drop data retrieval not available in this Avalonia port
			// sel = (int)e.Data.GetData(sel.GetType());
			ComboBoxCompat cb = ((ComboBoxCompat)sender);
			cb.SelectedIndex = -1;
			cb.Content = "0x"+Helper.HexString((ushort)sel);
		}

		private void btnCommit_Clicked(object sender, System.EventArgs e)
		{
			try 
			{
				wrapper.SynchronizeUserData();
				btnCommit.IsEnabled = wrapper.Changed;
				pnflowcontainer_SelectedInstChanged(null, null);
			} 
			catch (Exception ex) 
			{
				Helper.ExceptionMessage(pjse.Localization.GetString("errwritingfile"), ex);
			}			
		}

		private void btnCancel_Clicked(object sender, System.EventArgs e)
		{
			wrapper[pnflowcontainer.SelectedIndex] = origInst.Clone();
			pnflowcontainer_SelectedInstChanged(null, null);
		}

        private void pjse_banner1_SiblingClick(object sender, EventArgs e)
        {
            TPRP tprp = (TPRP)wrapper.SiblingResource(TPRP.TPRPtype);
            if (tprp == null) return;
            if (tprp.Package != wrapper.Package)
            {
                if (SimPe.Scenegraph.Compat.MessageBox.ShowAsync(Localization.GetString("OpenOtherPkg"), pjse_banner1.TitleText, MessageBoxButtons.YesNo).GetAwaiter().GetResult() != SimPe.Scenegraph.Compat.DialogResult.Yes) return;
            }
            SimPe.RemoteControl.OpenPackedFile(tprp.FileDescriptor, tprp.Package);
        }

        private void pjse_banner1_TreeClick(object sender, EventArgs e) // Fuck
        {
            SimPe.Plugin.TreesPackedFileWrapper tpfw = (SimPe.Plugin.TreesPackedFileWrapper)wrapper.SiblingResource(0x54524545);
            if (tpfw == null) return;

            if (tpfw.Package != wrapper.Package)
            {
                if (SimPe.Scenegraph.Compat.MessageBox.ShowAsync(Localization.GetString("OpenOtherPkg"), pjse_banner1.TitleText, MessageBoxButtons.YesNo).GetAwaiter().GetResult() != SimPe.Scenegraph.Compat.DialogResult.Yes) return;
            }
            SimPe.RemoteControl.OpenPackedFile(tpfw.FileDescriptor, tpfw.Package);
        }

        private void btnFloat_Click(object sender, EventArgs e)
        {
            Avalonia.Controls.Control old = this.bhavPanel.Parent as Avalonia.Controls.Control;
            string oldFloatText = this.pjse_banner1.FloatText;

            Window f = new Window();
            f.Title = formTitle;
            

            f.Content = this.bhavPanel;
            this.pjse_banner1.FloatText = pjse.Localization.GetString("bhavForm.Unfloat");
            this.pjse_banner1.FloatClick -= new System.EventHandler(this.btnFloat_Click);
            this.pjse_banner1.SetFormCancelButton(f);

            this.gbSpecial.IsVisible = true;
            this.cbSpecial.IsEnabled = false;
            this.btnCopyBHAV.IsVisible = false;

            handleOverride();

            f.ShowDialog(null).GetAwaiter().GetResult();

            // restore bhavPanel to old parent - not directly supported in Avalonia;
            this.pjse_banner1.FloatText = oldFloatText;
            this.pjse_banner1.FloatClick += (s, e) => this.btnFloat_Click(s, e);

            this.gbSpecial.IsVisible = this.cbSpecial.IsChecked == true;
            this.cbSpecial.IsEnabled = true;

            this.lbHidesOP.IsVisible = this.tbHidesOP.IsVisible = this.llHidesOP.IsVisible = false;
            this.llHidesOP.Tag = null;

            // f.Dispose() - no-op in Avalonia

            wrapper.RefreshUI();
        }

        private void pjse_banner1_ViewClick(object sender, EventArgs e)
        {
            common_LinkClicked(pjse.FileTable.GFT[wrapper.Package, wrapper.FileDescriptor][0]);
        }

		private void llopenbhav_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
            common_LinkClicked(wrapper.ResourceByInstance(SimPe.Data.MetaData.BHAV_FILE, currentInst.Instruction.OpCode));
		}

        private void llHidesOP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            common_LinkClicked(wrapper.ResourceByInstance(SimPe.Data.MetaData.BHAV_FILE, wrapper.FileDescriptor.Instance, (pjse.FileTable.Source)llHidesOP.Tag));
        }

		private void btnClose_Click(object sender, System.EventArgs e)
		{
            if (this.isPopup)
                Close();
		}

        private void btnCopyBHAV_Click(object sender, EventArgs e)
        {
            btnCopyBHAV.IsEnabled = false;
            TakeACopy();
            btnCopyBHAV.Content = pjse.Localization.GetString("ml_done");
        }

        private void btnImportBHAV_Click(object sender, EventArgs e)
        {
            btnImportBHAV.IsEnabled = false;
            ImportBHAV();
            btnImportBHAV.Content = pjse.Localization.GetString("ml_done");
        }

        private void pjse_banner1_ExtractClick(object sender, EventArgs e) { pjse.ExtractCurrent.Execute(wrapper, pjse_banner1.TitleText); }

		private void btnOpCode_Clicked(object sender, System.EventArgs e)
		{
            pjse.FileTable.Entry item = new ResourceChooser().Execute(SimPe.Data.MetaData.BHAV_FILE, wrapper.FileDescriptor.Group, bhavPanel.Parent as Avalonia.Controls.Control, false);

			if (item != null && item.Instance != currentInst.Instruction.OpCode)
				this.tbInst_OpCode.Text = "0x" + SimPe.Helper.HexString((ushort)item.Instance);
		}

        private void btnOperandWiz_Clicked(object sender, System.EventArgs e) { OperandWiz(1); }
		
		private void btnOperandRaw_Click(object sender, System.EventArgs e) { OperandWiz(0); }

        private void btnZero_Click(object sender, EventArgs e)
        {
            internalchg = true;
            Instruction inst = currentInst.Instruction;
            currentInst = null;
            try
            {
                for (int i = 0; i < 8; i++) inst.Operands[i] = 0;
                for (int i = 0; i < 8; i++) inst.Reserved1[i] = 0;
            }
            finally
            {
                currentInst = inst;
                UpdateInstPanel();
                this.btnCancel.IsEnabled = true;
                internalchg = false;
            }
        }

        private void tbFilename_TextChanged(object sender, System.EventArgs e)
		{
			wrapper.FileName = tbFilename.Text;
		}

		private void tbFilename_Validated(object sender, System.EventArgs e)
		{
			tbFilename.SelectAll();
		}

		private void cbHex16_Enter(object sender, System.EventArgs e)
		{
			((ComboBoxCompat)sender).SelectAll();
		}

		private void cbHex16_TextChanged(object sender, System.EventArgs ev)
		{
			if (internalchg) return;
			if (!cbHex16_IsValid(sender)) return;
			if (((ComboBoxCompat)sender).Items.IndexOf(((ComboBoxCompat)sender).Text) != -1) return;

			ushort val = Convert.ToUInt16(((ComboBoxCompat)sender).Text, 16);
			internalchg = true;
			switch (alHex16cb.IndexOf(sender))
			{
				case 0: currentInst.Instruction.Target1 = val; break;
				case 1: currentInst.Instruction.Target2 = val; break;
				case 2: wrapper.Header.Format = val; break;
			}
			internalchg = false;
		}

		private void cbHex16_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (cbHex16_IsValid(sender)) return;

			int i = alHex16cb.IndexOf(sender);
			if (i < 0)
				throw new Exception("cbHex16_Validating not applicable to control " + sender.ToString());

			e.Cancel = true;

			ushort val = 0;
			switch (i)
			{
				case 0: val = origInst.Target1; break;
				case 1: val = origInst.Target2; break;
				case 2: val = wrapper.Header.Format; break;
			}

			if (i < 2 && val >= 0xfffc && val <= 0xfffe)
			{
				((ComboBoxCompat)sender).SelectedIndex = val - 0xfffc;
			}
			else if (i == 2 && val >= 0x8000 && val <= 0x8007)
			{
				((ComboBoxCompat)sender).SelectedIndex = val - 0x8000;
			}
			else
			{
				((ComboBoxCompat)sender).SelectedIndex = -1;
				((ComboBoxCompat)sender).Content = "0x" + Helper.HexString(val);
			}
			((ComboBoxCompat)sender).SelectAll();
		}

		private void cbHex16_Validated(object sender, System.EventArgs e)
		{
			int i = alHex16cb.IndexOf(sender);
			if (i < 0)
				throw new Exception("cbHex16_Validated not applicable to control " + sender.ToString());
			if (((ComboBoxCompat)sender).Items.IndexOf(((ComboBoxCompat)sender).Text) != -1) return;

			ushort val = Convert.ToUInt16(((ComboBoxCompat)sender).Text, 16);

			bool origstate = internalchg;
			internalchg = true;
			if (i < 2 && val >= 0xfffc && val <= 0xfffe)
			{
				((ComboBoxCompat)sender).SelectedIndex = val - 0xfffc;
			}
			else if (i == 2 && val >= 0x8000 && val <= 0x8007)
			{
				((ComboBoxCompat)sender).SelectedIndex = val - 0x8000;
			}
			else
			{
				((ComboBoxCompat)sender).SelectedIndex = -1;
				((ComboBoxCompat)sender).Content = "0x" + Helper.HexString(val);
			}
			internalchg = origstate;
			((ComboBoxCompat)sender).Select(0, 0);
		}

		private void cbHex16_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (internalchg) return;

			int i = alHex16cb.IndexOf(sender);
			if (i < 0)
				throw new Exception("cbHex16_SelectedIndexChanged not applicable to control " + sender.ToString());
			if (((ComboBoxCompat)sender).SelectedIndex == -1) return;

			ushort val = (ushort)((ComboBoxCompat)alHex16cb[i]).SelectedIndex;
			((ComboBoxCompat)sender).SelectAll();

			internalchg = true;
			if (i < 2)
			{
				val += 0xFFFC;
				if (i == 0) currentInst.Instruction.Target1 = val;
				else        currentInst.Instruction.Target2 = val;
			}
			else
			{
				val += 0x8000;
				wrapper.Header.Format = val;
			}
			internalchg = false;
		}

		private void dec8_TextChanged(object sender, System.EventArgs ev)
		{
			if (internalchg) return;
			if (!dec8_IsValid(sender)) return;

			byte val = Convert.ToByte(((TextBoxCompat)sender).Text);
			internalchg = true;
			switch (alDec8.IndexOf(sender))
			{
				default: break;
			}
			internalchg = false;
		}

		private void dec8_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (dec8_IsValid(sender)) return;

			e.Cancel = true;

			byte val = 0;
			switch (alDec8.IndexOf(sender))
			{
				default: break;
			}

			((TextBoxCompat)sender).Text = val.ToString();
			((TextBoxCompat)sender).SelectAll();
        }

        private void hex8_TextChanged(object sender, System.EventArgs ev)
		{
			if (internalchg) return;
			if (!hex8_IsValid(sender)) return;

			byte val = Convert.ToByte(((TextBoxCompat)sender).Text, 16);
			int i = alHex8.IndexOf(sender);

			internalchg = true;

            byte oldval = val;
            if (i < 8) { oldval = currentInst.Instruction.Operands[i]; currentInst.Instruction.Operands[i] = val; ChangeLongname(oldval, val); }
            else if (i < 16) { oldval = currentInst.Instruction.Reserved1[i - 8]; currentInst.Instruction.Reserved1[i - 8] = val; ChangeLongname(oldval, val); }
            else
                switch (i)
                {
                    case 16: oldval = currentInst.Instruction.NodeVersion; currentInst.Instruction.NodeVersion = val; ChangeLongname(oldval, val); break;
                    case 17: wrapper.Header.HeaderFlag = val; break;
                    case 18: wrapper.Header.Type = val; break;
                    case 19: wrapper.Header.CacheFlags = val; break;
                    case 20: oldval = wrapper.Header.ArgumentCount; wrapper.Header.ArgumentCount = val; ChangeLongname(oldval, val); break;
                    case 21: wrapper.Header.LocalVarCount = val; break;
                }

			internalchg = false;
		}

		private void hex8_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (hex8_IsValid(sender)) return;

			e.Cancel = true;

			byte val = 0;
			int i = alHex8.IndexOf(sender);

			if (i < 8) val = origInst.Operands[i];
			else if (i < 16) val = origInst.Reserved1[i-8];
			else switch(i)
				 {
					 case 16: val = origInst.NodeVersion; break;
					 case 17: val = wrapper.Header.HeaderFlag; break;
					 case 18: val = wrapper.Header.Type; break;
					 case 19: val = wrapper.Header.CacheFlags; break;
					 case 20: val = wrapper.Header.ArgumentCount; break;
					 case 21: val = wrapper.Header.LocalVarCount; break;
				 }

			((TextBoxCompat)sender).Text = ((i >= 16) ? "0x" : "") + Helper.HexString(val);
			((TextBoxCompat)sender).SelectAll();
		}

		private void hex8_Validated(object sender, System.EventArgs e)
		{
			bool origstate = internalchg;
			internalchg = true;
			((TextBoxCompat)sender).Text = ((alHex8.IndexOf(sender) >= 16) ? "0x" : "") + Helper.HexString(Convert.ToByte(((TextBoxCompat)sender).Text, 16));
			((TextBoxCompat)sender).SelectAll();
			internalchg = origstate;
		}

		private void hex16_TextChanged(object sender, System.EventArgs ev)
		{
			if (internalchg) return;
			if (!hex16_IsValid(sender)) return;

			ushort val = Convert.ToUInt16(((TextBoxCompat)sender).Text, 16);
			internalchg = true;
			switch (alHex16.IndexOf(sender))
			{
                case 0: OpcodeChanged(val); break;
			}
			internalchg = false;
		}

		private void hex16_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (hex16_IsValid(sender)) return;

			e.Cancel = true;

			ushort val = 0;
			switch (alHex16.IndexOf(sender))
			{
				case 0: val = origInst.OpCode; break;
                case 1: val = 1; break;
			}

			((TextBoxCompat)sender).Text = "0x" + Helper.HexString(val);
			((TextBoxCompat)sender).SelectAll();
		}

		private void hex16_Validated(object sender, System.EventArgs e)
		{
			bool origstate = internalchg;
			internalchg = true;
			((TextBoxCompat)sender).Text = "0x" + Helper.HexString(Convert.ToUInt16(((TextBoxCompat)sender).Text, 16));
			((TextBoxCompat)sender).SelectAll();
			internalchg = origstate;
		}

		private void hex32_TextChanged(object sender, System.EventArgs ev)
		{
			if (internalchg) return;
			if (!hex32_IsValid(sender)) return;

			uint val = Convert.ToUInt32(((TextBoxCompat)sender).Text, 16);
			internalchg = true;
			switch (alHex32.IndexOf(sender))
			{
				case 0: wrapper.Header.TreeVersion = val; break;
			}
			internalchg = false;
		}

		private void hex32_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (hex32_IsValid(sender)) return;

			e.Cancel = true;

			uint val = 0;
			switch (alHex32.IndexOf(sender))
			{
				case 0: val = wrapper.Header.TreeVersion; break;
			}

			((TextBoxCompat)sender).Text = "0x" + Helper.HexString(val);
			((TextBoxCompat)sender).SelectAll();
		}

		private void hex32_Validated(object sender, System.EventArgs e)
		{
			bool origstate = internalchg;
			internalchg = true;
			((TextBoxCompat)sender).Text = "0x" + Helper.HexString(Convert.ToUInt32(((TextBoxCompat)sender).Text, 16));
			((TextBoxCompat)sender).SelectAll();
			internalchg = origstate;
		}

		private void btnSort_Clicked(object sender, System.EventArgs e)
		{
            this.pnflowcontainer.Sort();
            SetComments();
		}

		private void btnMove_Clicked(object sender, System.EventArgs e)
		{
			int mv;
			try { mv = Convert.ToInt32(tbLines.Text, 16); }
			catch (Exception) { return; }
            try
            {
                this.gbMove.IsEnabled = false;
                if (sender == this.btnUp)
                    this.pnflowcontainer.MoveInst(mv * -1);
                else
                    this.pnflowcontainer.MoveInst(mv);
                SetComments();
            }
            finally
            {
                this.gbMove.IsEnabled = true;
            }
        }

		private void btnAdd_Clicked(object sender, EventArgs e)
		{
            this.pnflowcontainer.Add(BhavUIAddType.Default);
            SetComments();
		}

		private void btnDel_Clicked(object sender, EventArgs e)
		{
            this.pnflowcontainer.Delete(BhavUIDeleteType.Default);
            SetComments();
		}

		private void cbSpecial_CheckStateChanged(object sender, EventArgs e)
		{
			gbSpecial.IsVisible =
                pjse.Settings.PJSE.ShowSpecialButtons = ((CheckBoxCompat2)sender).IsChecked == true;
		}

		private void btnInsVia_Click(object sender, EventArgs e)
		{
			this.pnflowcontainer.Add( (sender == this.btnInsTrue) ? BhavUIAddType.ViaTrue : BhavUIAddType.ViaFalse );
		}

		private void btnDelPescado_Click(object sender, EventArgs e)
		{
			this.pnflowcontainer.Delete(BhavUIDeleteType.Pescado);
		}

		private void btnLinkInge_Click(object sender, EventArgs e)
		{
			this.pnflowcontainer.Relink();
		}

		private void btnAppend_Click(object sender, EventArgs e)
		{
            this.pnflowcontainer.Append(new ResourceChooser().Execute(SimPe.Data.MetaData.BHAV_FILE, wrapper.FileDescriptor.Group, bhavPanel.Parent as Avalonia.Controls.Control, true, 0x10));
		}

		private void btnDelMerola_Click(object sender, EventArgs e)
		{
			this.pnflowcontainer.DeleteUnlinked();
		}

		private void btnCopyListing_Click(object sender, EventArgs e)
		{
			this.CopyListing();
		}

        private void btnPasteListing_Click(object sender, EventArgs e)
        {
            this.PasteListing();
        }

		private void btnTPRPMaker_Click(object sender, EventArgs e)
		{
			this.TPRPMaker();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.TPFWMaker();
        }

        private void btnInsUnlinked_Click(object sender, EventArgs e)
        {
            this.pnflowcontainer.Add(BhavUIAddType.Unlinked);
        }

        private void btnGUIDIndex_Click(object sender, EventArgs e)
        {
            this.cmenuGUIDIndex.Show((Control)sender, new Point(3 ,3));
        }

        private void cmenuGUIDIndex_Opening(object sender, EventArgs e)
        {
            createCurrentPackageToolStripMenuItem.IsEnabled =
                (pjse.FileTable.GFT.CurrentPackage != null
                && pjse.FileTable.GFT.CurrentPackage.FileName != null
                && !pjse.FileTable.GFT.CurrentPackage.FileName.ToLower().EndsWith("objects.package"));
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Application.DoEvents() not needed in Avalonia
            pjse.GUIDIndex.TheGUIDIndex.Create(sender.Equals(this.createCurrentPackageToolStripMenuItem));
            // Application.DoEvents() not needed in Avalonia
            
            // DialogResult dr = pjseMsgBox.Show(RemoteControl.ApplicationForm, pjse.Localization.GetString("guidAskMessage"), pjse.Localization.GetString("guidAskTitle"),
            SimPe.DialogResult dr = pjseMsgBox.ShowAsync(RemoteControl.ApplicationForm, "Do you want to save the GUID Index now? \r\n [Default] - save in the default location \r\n [Specify...] - let me specify where to save \r\n [No] - don't save, just let me get back to SimPe", pjse.Localization.GetString("guidAskTitle"),
                new Boolset("111"), new Boolset("111"), new string[] {
                    pjse.Localization.GetString("guidAskDefault"),
                    pjse.Localization.GetString("guidAskSpecify"),
                    pjse.Localization.GetString("guidAskNoSave"),
                },
                new SimPe.DialogResult[] { SimPe.DialogResult.OK, SimPe.DialogResult.Retry, SimPe.DialogResult.Cancel, }).GetAwaiter().GetResult();
            //SimPe.Scenegraph.Compat.DialogResult dr = SimPe.Scenegraph.Compat.MessageBox.ShowAsync(pjse.Localization.GetString("guidAskMessage"), pjse.Localization.GetString("guidAskTitle"), "\r\n" + 
            //    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr == SimPe.DialogResult.OK) defaultFileToolStripMenuItem_Click(this.defaultFileToolStripMenuItem1, null);
            else if (dr == SimPe.DialogResult.Retry) fileToolStripMenuItem_Click(this.toFileToolStripMenuItem, null);
        }

        private void defaultFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender.Equals(this.defaultFileToolStripMenuItem))
                pjse.GUIDIndex.TheGUIDIndex.Load();
            else
                pjse.GUIDIndex.TheGUIDIndex.Save();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool load = sender.Equals(this.fromFileToolStripMenuItem);
            FileDialogCompat fd;
            if (load)
                fd = new OpenFileDialogCompat();
            else
                fd = new SaveFileDialogCompat();
            fd.AddExtension = true;
            fd.CheckFileExists = load;
            fd.CheckPathExists = true;
            fd.DefaultExt = "txt";
            fd.DereferenceLinks = true;
            //fd.FileName = pjse.GUIDIndex.DefaultGUIDFile;
            fd.FileName = "guidindex.txt";
            fd.Filter = pjse.Localization.GetString("guidFilter");
            fd.FilterIndex = 1;
            fd.RestoreDirectory = false;
            fd.ShowHelp = false;
            // fd.SupportMultiDottedExtensions = false; // Methods missing from Mono
            fd.Title = load
                ? pjse.Localization.GetString("guidLoadIndex")
                : pjse.Localization.GetString("guidSaveIndex");
            fd.ValidateNames = true;
            SimPe.Scenegraph.Compat.DialogResult dr = fd.ShowDialog();
            if (dr == SimPe.Scenegraph.Compat.DialogResult.OK)
            {
                if (load)
                    pjse.GUIDIndex.TheGUIDIndex.Load(fd.FileName);
                else
                    pjse.GUIDIndex.TheGUIDIndex.Save(fd.FileName);
            }
        }
	}
}
