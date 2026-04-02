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
using MessageBoxButtons = SimPe.Scenegraph.Compat.MessageBoxButtons;
using MessageBoxIcon = SimPe.Scenegraph.Compat.MessageBoxIcon;
using SimPe.Interfaces.Plugin;
using SimPe.Plugin;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// Summary description for TrcnForm.
	/// </summary>
	public class TrcnForm : Window, IPackedFileUI
	{
		#region Form variables

        private StackPanelCompat trcnPanel;
		private LabelCompat lbFilename;
		private TextBoxCompat tbFilename;
		private ListView lvTrcnItem;
		private ColumnHeader chConstName;
		private ColumnHeader chUsed;
		private ColumnHeader chConstId;
		private ColumnHeader chDefValue;
		private ColumnHeader chMinValue;
		private ColumnHeader chMaxValue;
		private LabelCompat lbFormat;
		private TextBoxCompat tbFormat;
		private ButtonCompat btnStrDelete;
		private ButtonCompat btnStrAdd;
		private LabelCompat lbID;
		private LabelCompat lbDefValue;
		private LabelCompat lbMinValue;
		private LabelCompat lbMaxValue;
		private LabelCompat lbLabel;
		private TextBoxCompat tbDefValue;
		private TextBoxCompat tbMinValue;
		private TextBoxCompat tbMaxValue;
		private TextBoxCompat tbLabel;
		private CheckBoxCompat2 cbUsed;
		private ButtonCompat btnCancel;
		private ButtonCompat btnCommit;
		private ColumnHeader chValue;
		private TextBoxCompat tbID;
		private ColumnHeader chLine;
		private StackPanelCompat panel1;
		private LabelCompat label5;
		private ButtonCompat btnStrPrev;
        private ButtonCompat btnStrNext;
        private TextBoxCompat tbDesc;
        private LabelCompat lbDesc;
        private pjse.pjse_banner pjse_banner1;
        private TableLayoutPanel tlpUnused;
        private Panel panel2;
        private ButtonCompat btSetAll;
		/// <summary>
		/// Required designer variable.
		/// </summary>
				#endregion

		public TrcnForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            this.lvTrcnItem.Items.Clear();

            TextBoxCompat[] t = { tbFilename, tbLabel, };
			alText = new ArrayList(t);

			TextBoxCompat[] w = { tbDefValue ,tbMinValue ,tbMaxValue ,};
			alHex16 = new ArrayList(w);

			TextBoxCompat[] dw = { tbFormat ,tbID ,};
			alHex32 = new ArrayList(dw);

            pjse.FileTable.GFT.FiletableRefresh += (s, e) => this.FiletableRefresh(s, e);
            if (SimPe.Helper.XmlRegistry.UseBigIcons)
            {
                this.chUsed.Width = 48;
                this.chDefValue.Width = 72;
                this.chMinValue.Width = 72;
                this.chMaxValue.Width = 78;
                this.chLine.Width = 84;
            }
  		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose()
		{
            if (setHandler && wrapper != null)
            {
                wrapper.WrapperChanged -= new System.EventHandler(this.WrapperChanged);
                setHandler = false;
            }
            wrapper = null;
            bconres = null;
        }

		#region Controller
		private Trcn wrapper = null;
        private Bcon bconres = null;
		private bool setHandler = false;
		private bool internalchg = false;

		private ArrayList alText = null;
		private ArrayList alHex16 = null;
		private ArrayList alHex32 = null;

		private int index = -1;
		private TrcnItem origItem = null;
		private TrcnItem currentItem = null;

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

        private void doTextOnly()
        {            trcnPanel.Children.Clear();
            trcnPanel.Children.Add(this.pjse_banner1);
            trcnPanel.Children.Add(this.lbFilename);
            tbFilename.IsReadOnly = true;
            tbFilename.Text = wrapper.FileName;
            tbFormat.Text = SimPe.Helper.HexString(wrapper.Version);
            trcnPanel.Children.Add(this.tbFilename);
            trcnPanel.Children.Add(this.lbFormat);
            trcnPanel.Children.Add(this.tbFormat);

            LabelCompat lb = new LabelCompat();            lb.Location = new Point(0, tbFormat.Bottom + 6);
            lb.Text = pjse.Localization.GetString("trcnTextOnly");
            TextBoxCompat tb = new TextBoxCompat();            tb.Location = new Point(0, lb.Bottom + 6);
            tb.IsReadOnly = true;            tb.Size = trcnPanel.Size;
            tb.Height -= tb.Top;

            tb.Text = getText(wrapper.StoredData);
            trcnPanel.Children.Add(lb);
            trcnPanel.Children.Add(tb);        }

        private string getText(System.IO.BinaryReader br)
        {
            br.BaseStream.Seek(0x50, System.IO.SeekOrigin.Begin); // Skip filename, header and item count
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

		private void updateSelectedItem()
		{
			ListViewItem lv = this.lvTrcnItem.SelectedItems[0];
			if (lv == null) return;

			lv.SubItems[3].Text = "0x" + SimPe.Helper.HexString(currentItem.ConstId);
			lv.SubItems[4].Text = "0x" + currentItem.Used.ToString("X");
            if (wrapper.Version > 0x53) lv.SubItems[5].Text = "0x" + SimPe.Helper.HexString((byte)currentItem.DefValue);
			else lv.SubItems[5].Text = "0x" + SimPe.Helper.HexString(currentItem.DefValue);
			lv.SubItems[6].Text = "0x" + SimPe.Helper.HexString(currentItem.MinValue);
			lv.SubItems[7].Text = "0x" + SimPe.Helper.HexString(currentItem.MaxValue);
		}

		private string[] trcnItemToStringArray(int i)
		{
			if (i < 0 || i >= wrapper.Count) return new string[] { "", "", "", "", "", "", "", "" };

			TrcnItem ti = wrapper[i];
            string tiValue = (bconres != null && i < bconres.Count) ? "0x" + SimPe.Helper.HexString(bconres[i]) : "?";

			return new string[] {
									"0x" + i.ToString("X") + " (" + i + ")"
									, tiValue
									, ti.ConstName
									, "0x" + SimPe.Helper.HexString(ti.ConstId & (wrapper.Version == 0x3f ? 0x000f : 0xffffffff))
									, "0x" + ti.Used.ToString("X")
									, "0x" + (wrapper.Version > 0x53 ? SimPe.Helper.HexString((byte)ti.DefValue) : SimPe.Helper.HexString(ti.DefValue))
									, "0x" + SimPe.Helper.HexString(ti.MinValue)
									, "0x" + SimPe.Helper.HexString(ti.MaxValue)
								};

		}

        private void updateLists()
		{
            if (wrapper != null) wrapper.CleanUp();

			index = -1;
            bconres = (Bcon)(wrapper == null ? null : wrapper.SiblingResource(Bcon.Bcontype));

			this.lvTrcnItem.Items.Clear();
            int nItems = wrapper == null ? 0 : wrapper.Count;
			for(int i = 0; i < nItems; i++)
				this.lvTrcnItem.Items.Add(new ListViewItem(trcnItemToStringArray(i)));
		}

		private void setIndex(int i)
		{
			internalchg = true;
			if (i >= 0) this.lvTrcnItem.Items[i].Selected = true;
			else if (index >= 0) this.lvTrcnItem.Items[index].Selected = false;
			internalchg = false;

			if (this.lvTrcnItem.SelectedItems.Count > 0)
			{
				if (this.lvTrcnItem.Focused) this.lvTrcnItem.SelectedItems[0].Focused = true;
				this.lvTrcnItem.SelectedItems[0].EnsureVisible();
			}
			else
			{
				internalchg = true;
				this.tbLabel.Text = "";
				this.tbID.Text = "";
				this.cbUsed.IsChecked = null;
                this.tbDesc.Text = "";
				this.tbDefValue.Text = "";
				this.tbMinValue.Text = "";
				this.tbMaxValue.Text = "";
				this.btnCancel.IsEnabled = false;
				internalchg = false;
			}

			if (index == i) return;
			index = i;
			displayTrcnItem();
		}

		private void displayTrcnItem()
		{
			currentItem = (index < 0) ? null : wrapper[index];

			internalchg = true;
			if (currentItem != null)
			{
				origItem = currentItem.Clone();

				string[] s = trcnItemToStringArray(index);
				this.tbLabel.Text = s[2];
				this.tbID.Text = s[3];
				this.cbUsed.IsChecked = currentItem.Used != 0
					? true
					: false;
                this.tbDesc.Text = currentItem.ConstDesc;
				this.tbDefValue.Text = s[5];
				this.tbMinValue.Text = s[6];
				this.tbMaxValue.Text = s[7];
                this.tbID.IsEnabled = this.tbLabel.IsEnabled
                    = this.tbDefValue.IsEnabled = this.tbMinValue.IsEnabled = this.tbMaxValue.IsEnabled
                    = this.btnStrDelete.IsEnabled
                    = true;
                this.cbUsed.IsEnabled = (wrapper.Version > 0x3e);
                this.tbDefValue.IsEnabled = this.tbID.IsEnabled = this.tbMinValue.IsEnabled = this.tbMaxValue.IsEnabled = (wrapper.Version > 1);
            }
			else
			{
				origItem = null;

				this.tbID.Text = this.tbLabel.Text
					= this.tbDefValue.Text = this.tbMinValue.Text = this.tbMaxValue.Text
					= "";
				this.cbUsed.IsChecked = null;

				this.tbID.IsEnabled = this.tbLabel.IsEnabled = this.cbUsed.IsEnabled
					= this.tbDefValue.IsEnabled = this.tbMinValue.IsEnabled = this.tbMaxValue.IsEnabled
					= this.btnStrDelete.IsEnabled
					= false;
			}
			this.btnStrPrev.IsEnabled = (index > 0);
			this.btnStrNext.IsEnabled = (index < lvTrcnItem.Items.Count - 1);
			internalchg = false;

			this.btnCancel.IsEnabled = false;
		}

		private void TrcnItemAdd()
		{
			bool savedstate = internalchg;
			internalchg = true;

            try
            {
                wrapper.Add(new TrcnItem(wrapper));
                this.lvTrcnItem.Items.Add(new ListViewItem(trcnItemToStringArray(wrapper.Count - 1)));
            }
            catch { }

			internalchg = savedstate;

			setIndex(lvTrcnItem.Items.Count - 1);
		}

		private void TrcnItemDelete()
		{
			bool savedstate = internalchg;
			internalchg = true;

			wrapper.Remove(currentItem);
			int i = index;
			updateLists();

			internalchg = savedstate;

			setIndex((i >= lvTrcnItem.Items.Count) ? lvTrcnItem.Items.Count - 1 : i);
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
            if (tbFormat.Text == "0x00000001") tbFormat.Text = "0x" + SimPe.Helper.HexString(wrapper.Version);
			internalchg = savedstate;

			setIndex((i >= lvTrcnItem.Items.Count) ? lvTrcnItem.Items.Count - 1 : i);
		}

		private void Cancel()
		{
			bool savedstate = internalchg;
			internalchg = true;

			this.lvTrcnItem.SelectedItems[0].SubItems[2].Text = currentItem.ConstName = origItem.ConstName;
			currentItem.ConstId = origItem.ConstId;
			currentItem.Used = origItem.Used;
			currentItem.DefValue = origItem.DefValue;
			currentItem.MaxValue = origItem.MaxValue;
			currentItem.MinValue = origItem.MinValue;
			updateSelectedItem();

			internalchg = savedstate;

			displayTrcnItem();
		}

        void FiletableRefresh(object sender, EventArgs e)
        {
            pjse_banner1.SiblingEnabled = wrapper != null && wrapper.SiblingResource(Bcon.Bcontype) != null;
            updateLists();
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
				return trcnPanel;
			}
		}

		/// <summary>
		/// Called by the AbstractWrapper when the file should be displayed to the user.
		/// </summary>
		/// <param name="wrp">Reference to the wrapper to be displayed.</param>
		public void UpdateGUI(IFileWrapper wrp)
		{
			wrapper = (Trcn)wrp;
			WrapperChanged(wrapper, null);
            pjse_banner1.SiblingEnabled = wrapper.SiblingResource(Bcon.Bcontype) != null;

			internalchg = true;
			updateLists();
			internalchg = false;

			setIndex(lvTrcnItem.Items.Count > 0 ? 0 : -1);

			if (!setHandler)
			{
				wrapper.WrapperChanged += (s, e) => this.WrapperChanged(s, e);
				setHandler = true;
			}
            this.btSetAll.IsVisible = (wrapper.Version != 1);
        }

		private void WrapperChanged(object sender, System.EventArgs e)
		{
            if (wrapper.TextOnly)
            {
                doTextOnly();
                return;
            }

            this.tbDesc.IsReadOnly = (wrapper.Version <= 0x53);
            this.btnCommit.IsEnabled = (wrapper.Changed || wrapper.Version == 1); 
			if (sender.Equals(currentItem))
				this.btnCancel.IsEnabled = true;

			if (internalchg) return;

			if (sender.Equals(wrapper))
			{
				internalchg = true;
				this.Title = tbFilename.Text = wrapper.FileName;
				this.tbFormat.Text = "0x" + SimPe.Helper.HexString(wrapper.Version);
				internalchg = false;
			}
			else if (!sender.Equals(currentItem))
				updateLists();
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrcnForm));
            this.btnCommit = new ButtonCompat();
            this.trcnPanel = new StackPanelCompat();
            this.panel2 = new FlowLayoutPanel();
            this.btnStrAdd = new ButtonCompat();
            this.lbLabel = new LabelCompat();
            this.btnStrDelete = new ButtonCompat();
            this.tbLabel = new TextBoxCompat();
            this.btnCancel = new ButtonCompat();
            this.btnStrPrev = new ButtonCompat();
            this.btnStrNext = new ButtonCompat();
            this.tlpUnused = new TableLayoutPanel();
            this.label5 = new LabelCompat();
            this.lbID = new LabelCompat();
            this.tbID = new TextBoxCompat();
            this.lbDesc = new LabelCompat();
            this.tbDesc = new TextBoxCompat();
            this.lbDefValue = new LabelCompat();
            this.tbDefValue = new TextBoxCompat();
            this.panel1 = new StackPanelCompat();
            this.lbMinValue = new LabelCompat();
            this.tbMinValue = new TextBoxCompat();
            this.lbMaxValue = new LabelCompat();
            this.tbMaxValue = new TextBoxCompat();
            this.cbUsed = new CheckBoxCompat2();
            this.tbFormat = new TextBoxCompat();
            this.lbFormat = new LabelCompat();
            this.tbFilename = new TextBoxCompat();
            this.lbFilename = new LabelCompat();
            this.pjse_banner1 = new pjse.pjse_banner();
            this.lvTrcnItem = new ListView();
            this.chLine = new ColumnHeader();
            this.chValue = new ColumnHeader();
            this.chConstName = new ColumnHeader();
            this.chConstId = new ColumnHeader();
            this.chUsed = new ColumnHeader();
            this.chDefValue = new ColumnHeader();
            this.chMinValue = new ColumnHeader();
            this.chMaxValue = new ColumnHeader();
            this.btSetAll = new ButtonCompat();
            // btnCommit
            //            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Click += (s, e) => this.btnCommit_Click(s, e);
            // 
            // trcnPanel
            //            this.trcnPanel.Children.Add(this.btSetAll);
            this.trcnPanel.Children.Add(this.panel2);
            this.trcnPanel.Children.Add(this.tlpUnused);
            this.trcnPanel.Children.Add(this.btnCommit);
            this.trcnPanel.Children.Add(this.tbFormat);
            this.trcnPanel.Children.Add(this.lbFormat);
            this.trcnPanel.Children.Add(this.tbFilename);
            this.trcnPanel.Children.Add(this.lbFilename);
            this.trcnPanel.Children.Add(this.pjse_banner1);
            this.trcnPanel.Children.Add(this.lvTrcnItem);
            this.trcnPanel.Name = "trcnPanel";
            // 
            // panel2
            //            this.panel2.Children.Add(this.btnStrAdd);
            this.panel2.Children.Add(this.lbLabel);
            this.panel2.Children.Add(this.btnStrDelete);
            this.panel2.Children.Add(this.tbLabel);
            this.panel2.Children.Add(this.btnCancel);
            this.panel2.Children.Add(this.btnStrPrev);
            this.panel2.Children.Add(this.btnStrNext);
            this.panel2.Name = "panel2";
            // 
            // btnStrAdd
            //            this.btnStrAdd.Name = "btnStrAdd";
            this.btnStrAdd.Click += (s, e) => this.btnStrAdd_Click(s, e);
            // 
            // lbLabel
            //            this.lbLabel.Name = "lbLabel";
            // 
            // btnStrDelete
            //            this.btnStrDelete.Name = "btnStrDelete";
            this.btnStrDelete.Click += (s, e) => this.btnStrDelete_Click(s, e);
            // 
            // tbLabel
            //            this.tbLabel.Name = "tbLabel";
            this.tbLabel.TextChanged += (s, e) => this.tbText_TextChanged(s, e);
            this.tbLabel.GotFocus += (s, e) => this.tbText_Enter(s, e);
            // 
            // btnCancel
            //            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Click += (s, e) => this.btnCancel_Click(s, e);
            // 
            // btnStrPrev
            //            this.btnStrPrev.Name = "btnStrPrev";
            this.btnStrPrev.Click += (s, e) => this.btnStrPrev_Click(s, e);
            // 
            // btnStrNext
            //            this.btnStrNext.Name = "btnStrNext";
            this.btnStrNext.Click += (s, e) => this.btnStrNext_Click(s, e);
            // 
            // tlpUnused
            //            this.tlpUnused.Children.Add(this.label5);
            this.tlpUnused.Children.Add(this.lbID);
            this.tlpUnused.Children.Add(this.tbID);
            this.tlpUnused.Children.Add(this.lbDesc);
            this.tlpUnused.Children.Add(this.tbDesc);
            this.tlpUnused.Children.Add(this.lbDefValue);
            this.tlpUnused.Children.Add(this.tbDefValue);
            this.tlpUnused.Children.Add(this.panel1);
            this.tlpUnused.Children.Add(this.lbMinValue);
            this.tlpUnused.Children.Add(this.tbMinValue);
            this.tlpUnused.Children.Add(this.lbMaxValue);
            this.tlpUnused.Children.Add(this.tbMaxValue);
            this.tlpUnused.Children.Add(this.cbUsed);
            this.tlpUnused.Name = "tlpUnused";
            // 
            // label5
            //            this.tlpUnused
            this.label5.Name = "label5";
            // 
            // lbID
            //            this.lbID.Name = "lbID";
            // 
            // tbID
            //            this.tbID.Name = "tbID";
            this.tbID.TextChanged += (s, e) => this.hex32_TextChanged(s, e);
            this.tbID.LostFocus += (s, e) => this.hex32_Validated(s, e);
            this.tbID.GotFocus += (s, e) => this.tbText_Enter(s, e);
            // lbDesc
            //            this.lbDesc.Name = "lbDesc";
            // 
            // tbDesc
            //            this.tbDesc.Name = "tbDesc";
            this.tbDesc.IsReadOnly = true;
            this.tbDesc.TextChanged += (s, e) => this.tbDesc_TextChanged(s, e);
            // 
            // lbDefValue
            //            this.lbDefValue.Name = "lbDefValue";
            // 
            // tbDefValue
            //            this.tbDefValue.Name = "tbDefValue";
            this.tbDefValue.TextChanged += (s, e) => this.hex16_TextChanged(s, e);
            this.tbDefValue.LostFocus += (s, e) => this.hex16_Validated(s, e);
            this.tbDefValue.GotFocus += (s, e) => this.tbText_Enter(s, e);
            // panel1
            //            this.tlpUnused            this.panel1.Name = "panel1";
            // 
            // lbMinValue
            //            this.lbMinValue.Name = "lbMinValue";
            // 
            // tbMinValue
            //            this.tbMinValue.Name = "tbMinValue";
            this.tbMinValue.TextChanged += (s, e) => this.hex16_TextChanged(s, e);
            this.tbMinValue.LostFocus += (s, e) => this.hex16_Validated(s, e);
            this.tbMinValue.GotFocus += (s, e) => this.tbText_Enter(s, e);
            // lbMaxValue
            //            this.lbMaxValue.Name = "lbMaxValue";
            // 
            // tbMaxValue
            //            this.tbMaxValue.Name = "tbMaxValue";
            this.tbMaxValue.TextChanged += (s, e) => this.hex16_TextChanged(s, e);
            this.tbMaxValue.LostFocus += (s, e) => this.hex16_Validated(s, e);
            this.tbMaxValue.GotFocus += (s, e) => this.tbText_Enter(s, e);
            // cbUsed
            //            this.tlpUnused
            this.cbUsed.Name = "cbUsed";
            this.cbUsed.IsCheckedChanged += (s, e) => this.cbUsed_CheckedChanged(s, e);
            // 
            // tbFormat
            //            this.tbFormat.Name = "tbFormat";
            this.tbFormat.IsReadOnly = true;
            this.tbFormat.TextChanged += (s, e) => this.hex32_TextChanged(s, e);
            this.tbFormat.LostFocus += (s, e) => this.hex32_Validated(s, e);
            this.tbFormat.GotFocus += (s, e) => this.tbText_Enter(s, e);
            // lbFormat
            //            this.lbFormat.Name = "lbFormat";
            // 
            // tbFilename
            //            this.tbFilename.Name = "tbFilename";
            this.tbFilename.TextChanged += (s, e) => this.tbText_TextChanged(s, e);
            this.tbFilename.GotFocus += (s, e) => this.tbText_Enter(s, e);
            // 
            // lbFilename
            //            this.lbFilename.Name = "lbFilename";
            // 
            // pjse_banner1
            //            this.pjse_banner1.Name = "pjse_banner1";
            this.pjse_banner1.SiblingVisible = true;
            this.pjse_banner1.SiblingClick += (s, e) => this.pjse_banner1_SiblingClick(s, e);
            // 
            // lvTrcnItem
            this.lvTrcnItem.Name = "lvTrcnItem";
            this.lvTrcnItem.Resize += (s, e) => this.lvTrcnItem_Resize(s, e);
            this.lvTrcnItem.SelectionChanged += (s, e) => this.lvTrcnItem_SelectedIndexChanged(s, e);
            // 
            // chLine
            //            // 
            // chValue
            //            // 
            // chConstName
            //            // 
            // chConstId
            //            // 
            // chUsed
            //            // 
            // chDefValue
            //            // 
            // chMinValue
            //            // 
            // chMaxValue
            //            // 
            // btSetAll
            //            this.btSetAll.Name = "btSetAll";
            this.btSetAll.Click += (s, e) => this.btSetAll_Click(s, e);
            // 
            // TrcnForm
            // 
            this.Name = "TrcnForm";
        }

		#endregion

        private void lvTrcnItem_Resize(object sender, EventArgs e)
        {
            int before = lvTrcnItem.Columns[0].Width + lvTrcnItem.Columns[1].Width;
            int after = 0;
            for (int i = 3; i < lvTrcnItem.Columns.Count; i++) after += lvTrcnItem.Columns[i].Width;
            lvTrcnItem.Columns[2].Width = lvTrcnItem.Width - (before + after + 36);
        }

        private void lvTrcnItem_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (internalchg) return;
			setIndex((this.lvTrcnItem.SelectedIndices.Count > 0) ? this.lvTrcnItem.SelectedIndices[0] : -1);
		}

		private void btnCommit_Click(object sender, System.EventArgs e)
		{
			this.Commit();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Cancel();
			this.tbLabel.SelectAll();
			this.tbLabel.Focus();
		}

        private void pjse_banner1_SiblingClick(object sender, EventArgs e)
        {
            Bcon bcon = (Bcon)wrapper.SiblingResource(Bcon.Bcontype);
            if (bcon == null) return;
            if (bcon.Package != wrapper.Package)
            {
                if (SimPe.Scenegraph.Compat.MessageBox.ShowAsync(Localization.GetString("OpenOtherPkg"), pjse_banner1.TitleText, MessageBoxButtons.YesNo).GetAwaiter().GetResult() != SimPe.DialogResult.Yes) return;
            }
            SimPe.RemoteControl.OpenPackedFile(bcon.FileDescriptor, bcon.Package);
        }

		private void btnStrPrev_Click(object sender, System.EventArgs e)
		{
			this.setIndex(index - 1);
		}

		private void btnStrNext_Click(object sender, System.EventArgs e)
		{
			this.setIndex(index + 1);
		}

		private void btnStrAdd_Click(object sender, System.EventArgs e)
		{
			this.TrcnItemAdd();
			this.tbLabel.SelectAll();
			this.tbLabel.Focus();
		}

		private void btnStrDelete_Click(object sender, System.EventArgs e)
		{
			this.TrcnItemDelete();
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
				case 1: lvTrcnItem.SelectedItems[0].SubItems[2].Text = currentItem.ConstName = ((TextBoxCompat)sender).Text; break;
			}
			internalchg = false;
		}

		private void cbUsed_CheckedChanged(object sender, System.EventArgs e)
		{
			if (internalchg) return;
			currentItem.Used = (uint)(((CheckBoxCompat2)sender).IsChecked == true ? 1 : 0);
			updateSelectedItem();
		}

		private void hex16_TextChanged(object sender, System.EventArgs ev)
		{
			if (internalchg) return;
			if (!hex16_IsValid(sender)) return;

			internalchg = true;
			ushort val = Convert.ToUInt16(((TextBoxCompat)sender).Text, 16);
			switch(alHex16.IndexOf(sender))
			{
				case 0: currentItem.DefValue = val; updateSelectedItem(); break;
				case 1: currentItem.MinValue = val; updateSelectedItem(); break;
				case 2: currentItem.MaxValue = val; updateSelectedItem(); break;
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
			ushort val = 0;
			switch(alHex16.IndexOf(sender))
			{
				case 0: val = currentItem.DefValue; break;
				case 1: val = currentItem.MinValue; break;
				case 2: val = currentItem.MaxValue; break;
			}

			bool origstate = internalchg;
			internalchg = true;
			((TextBoxCompat)sender).Text = "0x" + Helper.HexString(val);
			internalchg = origstate;
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
				case 1: currentItem.ConstId = val; updateSelectedItem(); break;
			}
			internalchg = false;
		}

		private void hex32_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (hex32_IsValid(sender)) return;
			e.Cancel = true;
			hex32_Validated(sender, null);
		}

		private void hex32_Validated(object sender, System.EventArgs e)
		{
			uint val = 0;
			switch (alHex32.IndexOf(sender))
			{
				case 0: val = wrapper.Version; break;
				case 1: val = currentItem.ConstId;break;
			}

			bool origstate = internalchg;
			internalchg = true;
			((TextBoxCompat)sender).Text = "0x" + Helper.HexString(val);
			internalchg = origstate;
		}

        private void tbDesc_TextChanged(object sender, EventArgs e)
        {
            if (internalchg) return;
            if (wrapper.Version > 0x53) currentItem.ConstDesc = this.tbDesc.Text;
        }

        private void btSetAll_Click(object sender, EventArgs e)
        {
            if (internalchg) return;
            internalchg = true;
            uint fid = 0;
            foreach (TrcnItem fing in wrapper)
            {
                fid++;
                fing.Used = 1;
                if (fing.MaxValue == 0) fing.MaxValue = 100;
                if (fing.ConstId == 0) fing.ConstId = fid;
            }
            internalchg = false;
            updateLists();
        }
	}
}
