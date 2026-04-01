/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
 *   pljones@users.sf.net                                                  *
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
 *   59 Temple Place - Suite 330, Boston, MA  32111-1307, USA.             *
 ***************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using SimPe.PackedFiles.Wrapper;

namespace pjse.BhavOperandWizards.Wiz0x0032
{
    /// <summary>
    /// Summary description for StrBig.
    /// </summary>
    internal class UI : Window, iBhavOperandWizForm
	{
		#region Form variables

        internal StackPanel pnWiz0x0032;
        private RadioButton rbModeIcon;
        private RadioButton rbModeAction;
        private Panel pnAction;
        private Panel pnIcon;
        private ComboBoxCompat cbScope;
        private LabelCompat label1;
        private LabelCompat lbDisabled;
        private ComboBoxCompat cbDisabled;
        private LabelCompat label3;
        private LabelCompat label4;
        private Panel pnStrIndex;
        private LabelCompat label5;
        private ButtonCompat btnActionString;
        private TextBoxCompat tbStrIndex;
        private LabelCompat lbActionString;
        private CheckBoxCompat2 tfActionTemp;
        private CheckBoxCompat2 tfIconTemp;
        private Panel pnIconIndex;
        private LabelCompat label6;
        private TextBoxCompat tbIconIndex;
        private Panel pnThumbnail;
        private CheckBoxCompat2 tfGUIDTemp;
        private Panel pnGUID;
        private LabelCompat label8;
        private TextBoxCompat tbGUID;
        private LabelCompat label7;
        private RadioButton rbIconSourceObj;
        private RadioButton rbIconSourceTN;
        private LabelCompat label10;
        private Panel pnObject;
        private LabelCompat label9;
        private ComboBoxCompat cbPicker1;
        private TextBoxCompat tbVal1;
        private ComboBoxCompat cbDataOwner1;
        private CheckBoxCompat2 cbAttrPicker;
        private CheckBoxCompat2 cbDecimal;
        private CheckBoxCompat2 tfSubQ;
		/// <summary>
		/// Required designer variable.
		/// </summary>
				#endregion

		public UI()
		{
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected void Dispose(bool disposing)
		{
			if( disposing )
			{
			}
			// base.Dispose(disposing) not available on Avalonia Window

			inst = null;
        }


        private Instruction inst = null;
		private DataOwnerControl doid1 = null;
        private bool internalchg = false;

        private Scope Scope
        {
            get
            {
                Scope scope = Scope.Private;
                switch (this.cbScope.SelectedIndex)
                {
                    case 1: scope = Scope.SemiGlobal; break;
                    case 2: scope = Scope.Global; break;
                }
                return scope;
            }
        }

        private void doStrChooser()
        {
            pjse.FileTable.Entry[] items = pjse.FileTable.GFT[(uint)SimPe.Data.MetaData.STRING_FILE, inst.Parent.GroupForScope(this.Scope), (uint)GS.GlobalStr.MakeAction];

            if (items == null || items.Length == 0)
            {
                MessageBox.Show(pjse.Localization.GetString("bow_noStrings")
                    + " (" + pjse.Localization.GetString(this.Scope.ToString()) + ")");
                return; // eek!
            }

            SimPe.PackedFiles.Wrapper.StrWrapper str = new StrWrapper();
            str.ProcessData(items[0].PFD, items[0].Package);

            int i = (new StrChooser(true)).Strnum(str);
            if (i >= 0)
            {
                this.tbStrIndex.Text = "0x" + SimPe.Helper.HexString((byte)(i+1));
                this.lbActionString.Content = ((BhavWiz)inst).readStr(this.Scope, GS.GlobalStr.MakeAction, (ushort)i, -1, pjse.Detail.ErrorNames);
            }
        }

        private bool hex8_IsValid(object sender)
        {
            try { Convert.ToByte(((TextBoxCompat)sender).Text, 16); }
            catch (Exception) { return false; }
            return true;
        }

        private bool hex16_IsValid(object sender)
        {
            try { Convert.ToUInt16(((TextBoxCompat)sender).Text, 16); }
            catch (Exception) { return false; }
            return true;
        }

        private bool hex32_IsValid(object sender)
        {
            try { Convert.ToUInt32(((TextBoxCompat)sender).Text, 16); }
            catch (Exception) { return false; }
            return true;
        }



        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x0032; } }

        public void Execute(Instruction inst)
        {
            this.inst = inst;

            wrappedByteArray ops1 = inst.Operands;
            wrappedByteArray ops2 = inst.Reserved1;

            internalchg = true;

            this.lbDisabled.IsEnabled = this.cbDisabled.IsEnabled = inst.NodeVersion != 0;
            this.tfSubQ.IsEnabled = inst.NodeVersion > 2;

            this.cbScope.SelectedIndex = -1;
            switch (ops1[0x02] & 0x0c)
            {
                case 0x00: this.cbScope.SelectedIndex = 0; break; // Private
                case 0x04: this.cbScope.SelectedIndex = 2; break; // Global
                case 0x08: this.cbScope.SelectedIndex = 1; break; // SemiGlobal
            }

            this.tfActionTemp.Checked = (ops1[0x02] & 0x10) != 0;
            this.pnStrIndex.IsEnabled = !this.tfActionTemp.Checked;

            this.rbIconSourceTN.IsChecked = ((ops1[0x02] & 0x20) != 0);
            this.pnThumbnail.IsEnabled = ((ops1[0x02] & 0x20) != 0);
            this.rbIconSourceObj.IsChecked = !this.rbIconSourceTN.IsChecked == true;
            this.pnObject.IsEnabled = this.rbIconSourceObj.IsChecked == true;

            this.tfGUIDTemp.Checked = ((ops1[0x02] & 0x40) != 0);
            this.pnGUID.IsEnabled = !this.tfGUIDTemp.Checked;

            this.tfIconTemp.Checked = (ops1[0x02] & 0x80) != 0;
            this.pnIconIndex.IsEnabled = !this.tfIconTemp.Checked;

            this.cbDisabled.SelectedIndex = -1;
            switch (ops1[0x03] & 0x03)
            {
                case 0x00: this.cbDisabled.SelectedIndex = 2; break;
                case 0x01: this.cbDisabled.SelectedIndex = 0; break;
                case 0x02: this.cbDisabled.SelectedIndex = 1; break;
            }
            this.tfSubQ.Checked = (ops1[0x03] & 0x10) != 0;

            int val = inst.NodeVersion < 2 ? ops1[0x04] : BhavWiz.ToShort(ops2[0x06], ops2[0x07]);
            this.tbStrIndex.Text = "0x" + SimPe.Helper.HexString((ushort)val);
            this.lbActionString.Content = ((BhavWiz)inst).readStr(this.Scope, GS.GlobalStr.MakeAction, (ushort)(val - 1), -1, pjse.Detail.ErrorNames);

            this.tbGUID.Text
                = "0x" + SimPe.Helper.HexString(ops1[0x05] | (ops1[0x06] << 8) | (ops1[0x07] << 16) | (ops2[0x00] << 24));

            this.rbModeAction.IsChecked = ops2[0x01] == 0;
            this.pnAction.IsEnabled = ops2[0x01] == 0;
            this.rbModeIcon.IsChecked = !this.rbModeAction.IsChecked == true;
            this.pnIcon.IsEnabled = this.rbModeIcon.IsChecked == true;

            this.tbIconIndex.Text = "0x" + SimPe.Helper.HexString(ops2[0x03]);

            doid1 = new DataOwnerControl(inst, this.cbDataOwner1, this.cbPicker1, this.tbVal1, this.cbDecimal, this.cbAttrPicker, null,
                ops2[0x03], BhavWiz.ToShort(ops2[0x04], ops2[0x05]));

            internalchg = false;
        }

		public Instruction Write(Instruction inst)
		{
			if (inst != null)
			{
                wrappedByteArray ops1 = inst.Operands;
                wrappedByteArray ops2 = inst.Reserved1;

                if (this.rbModeAction.IsChecked == true)
                {
                    ops2[0x01] = 0;

                    if (this.cbScope.SelectedIndex != null)
                    {
                        ops1[0x02] &= 0xf3;
                        if (this.cbScope.SelectedIndex == 2) ops1[0x02] |= 0x04;
                        if (this.cbScope.SelectedIndex == 1) ops1[0x02] |= 0x08;
                    }

                    ops1[0x02] &= 0xef;
                    if (this.tfActionTemp.Checked)
                        ops1[0x02] |= 0x10;
                    else
                    {
                        ushort val = Convert.ToUInt16(this.tbStrIndex.Text, 16);
                        if (inst.NodeVersion < 2)
                            ops1[0x04] = (byte)(val & 0xff);
                        else
                            BhavWiz.FromShort(ref ops2, 6, val);
                    }

                    if (inst.NodeVersion != 0 && this.cbDisabled.SelectedIndex != -1)
                    {
                        ops1[0x03] &= 0xfc;
                        if (this.cbDisabled.SelectedIndex == 0) ops1[0x03] |= 0x01;
                        else if (this.cbDisabled.SelectedIndex == 1) ops1[0x03] |= 0x02;
                    }
                    if (inst.NodeVersion > 2)
                    {
                        ops1[0x03] &= 0xef;
                        if (this.tfSubQ.Checked)
                            ops1[0x03] |= 0x10;
                    }

                }
                else
                {
                    if (ops2[0x01] == 0) ops2[0x01] = 1;

                    ops1[0x02] &= 0x7f;
                    if (this.tfIconTemp.Checked)
                        ops1[0x02] |= 0x80;
                    else
                        ops2[0x03] = Convert.ToByte(this.tbIconIndex.Text, 16);

                    ops1[0x02] &= 0xdf;
                    if (this.pnThumbnail.IsEnabled)
                    {
                        ops1[0x02] |= 0x20;

                        ops1[0x02] &= 0xbf;
                        if (this.tfGUIDTemp.Checked)
                            ops1[0x02] |= 0x40;
                        else
                        {
                            uint val = Convert.ToUInt32(this.tbGUID.Text, 16);
                            ops1[0x05] = (byte)(val & 0xff);
                            ops1[0x06] = (byte)((val >> 8) & 0xff);
                            ops1[0x07] = (byte)((val >> 16) & 0xff);
                            ops2[0x00] = (byte)((val >> 24) & 0xff);
                        }
                    }
                    else
                    {
                        ops2[0x03] = doid1.DataOwner;
                        BhavWiz.FromShort(ref ops2, 4, doid1.Value);
                    }
                }
            }
			return inst;
		}

		#endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
		{            this.pnWiz0x0032 = new StackPanel();
            this.rbModeIcon = new Avalonia.Controls.RadioButton();
            this.rbModeAction = new Avalonia.Controls.RadioButton();
            this.pnAction = new StackPanel();
            this.tfSubQ = new CheckBoxCompat2();
            this.pnStrIndex = new StackPanel();
            this.label5 = new LabelCompat();
            this.btnActionString = new ButtonCompat();
            this.tbStrIndex = new TextBoxCompat();
            this.lbActionString = new LabelCompat();
            this.tfActionTemp = new CheckBoxCompat2();
            this.cbDisabled = new ComboBoxCompat();
            this.cbScope = new ComboBoxCompat();
            this.label3 = new LabelCompat();
            this.lbDisabled = new LabelCompat();
            this.label1 = new LabelCompat();
            this.pnIcon = new StackPanel();
            this.pnObject = new StackPanel();
            this.cbAttrPicker = new CheckBoxCompat2();
            this.cbDecimal = new CheckBoxCompat2();
            this.cbPicker1 = new ComboBoxCompat();
            this.tbVal1 = new TextBoxCompat();
            this.cbDataOwner1 = new ComboBoxCompat();
            this.label9 = new LabelCompat();
            this.pnThumbnail = new StackPanel();
            this.tfGUIDTemp = new CheckBoxCompat2();
            this.pnGUID = new StackPanel();
            this.label8 = new LabelCompat();
            this.tbGUID = new TextBoxCompat();
            this.label7 = new LabelCompat();
            this.rbIconSourceObj = new Avalonia.Controls.RadioButton();
            this.rbIconSourceTN = new Avalonia.Controls.RadioButton();
            this.tfIconTemp = new CheckBoxCompat2();
            this.pnIconIndex = new StackPanel();
            this.label6 = new LabelCompat();
            this.tbIconIndex = new TextBoxCompat();
            this.label10 = new LabelCompat();
            this.label4 = new LabelCompat();            // pnWiz0x0032
            // 
            this.pnWiz0x0032.Children.Add(this.rbModeIcon);
            this.pnWiz0x0032.Children.Add(this.rbModeAction);
            this.pnWiz0x0032.Children.Add(this.pnAction);
            this.pnWiz0x0032.Children.Add(this.pnIcon);            this.pnWiz0x0032.Name = "pnWiz0x0032";
            // rbModeIcon
            //            this.rbModeIcon.Name = "rbModeIcon";
            this.rbModeIcon.IsCheckedChanged += (s, e) => this.rbModeIcon_CheckedChanged(s, e);
            // rbModeAction
            //            this.rbModeAction.Name = "rbModeAction";
            this.rbModeAction.IsCheckedChanged += (s, e) => this.rbModeAction_CheckedChanged(s, e);
            // pnAction
            // 
            this.pnAction.Children.Add(this.tfSubQ);
            this.pnAction.Children.Add(this.pnStrIndex);
            this.pnAction.Children.Add(this.tfActionTemp);
            this.pnAction.Children.Add(this.cbDisabled);
            this.pnAction.Children.Add(this.cbScope);
            this.pnAction.Children.Add(this.label3);
            this.pnAction.Children.Add(this.lbDisabled);
            this.pnAction.Children.Add(this.label1);            this.pnAction.Name = "pnAction";
            // tfSubQ
            //            this.tfSubQ.Name = "tfSubQ";
            //            this.pnStrIndex.Children.Add(this.label5);
            this.pnStrIndex.Children.Add(this.btnActionString);
            this.pnStrIndex.Children.Add(this.tbStrIndex);
            this.pnStrIndex.Children.Add(this.lbActionString);
            this.pnStrIndex.Name = "pnStrIndex";
            // label5
            //            this.label5.Name = "label5";
            // btnActionString
            //            this.btnActionString.Name = "btnActionString";
            this.btnActionString.Click += (s, e) => this.btnActionString_Click(s, e);
            // tbStrIndex
            //            this.tbStrIndex.Name = "tbStrIndex";
            this.tbStrIndex.TextChanged += (s, e) => this.hex16_TextChanged(s, e);
            this.tbStrIndex.LostFocus += (s, e) => this.hex16_Validated(s, e);
            //            this.lbActionString.Name = "lbActionString";
            // tfActionTemp
            //            this.tfActionTemp.Name = "tfActionTemp";
            this.tfActionTemp.IsCheckedChanged += (s, e) => this.tfActionTemp_CheckedChanged(s, e);
            // cbDisabled
            // 
            // cbScope
            // 
            this.cbScope.SelectionChanged += (s, e) => this.cbScope_SelectedIndexChanged(s, e);
            // label3
            //            this.label3.Name = "label3";
            // lbDisabled
            //            this.lbDisabled.Name = "lbDisabled";
            // label1
            //            this.label1.Name = "label1";
            // pnIcon
            // 
            this.pnIcon.Children.Add(this.pnObject);
            this.pnIcon.Children.Add(this.pnThumbnail);
            this.pnIcon.Children.Add(this.rbIconSourceObj);
            this.pnIcon.Children.Add(this.rbIconSourceTN);
            this.pnIcon.Children.Add(this.tfIconTemp);
            this.pnIcon.Children.Add(this.pnIconIndex);
            this.pnIcon.Children.Add(this.label10);
            this.pnIcon.Children.Add(this.label4);            this.pnIcon.Name = "pnIcon";
            // pnObject
            // 
            this.pnObject.Children.Add(this.cbAttrPicker);
            this.pnObject.Children.Add(this.cbDecimal);
            this.pnObject.Children.Add(this.cbPicker1);
            this.pnObject.Children.Add(this.tbVal1);
            this.pnObject.Children.Add(this.cbDataOwner1);
            this.pnObject.Children.Add(this.label9);            this.pnObject.Name = "pnObject";
            // cbAttrPicker
            //            this.cbAttrPicker.Name = "cbAttrPicker";
            // cbDecimal
            //            this.cbDecimal.Name = "cbDecimal";
            // cbPicker1
            // 
            this.cbPicker1.Name = "cbPicker1";
            // tbVal1
            //            this.tbVal1.Name = "tbVal1";
            // cbDataOwner1
            // 
            this.cbDataOwner1.Name = "cbDataOwner1";
            // label9
            //            this.label9.Name = "label9";
            // pnThumbnail
            // 
            this.pnThumbnail.Children.Add(this.tfGUIDTemp);
            this.pnThumbnail.Children.Add(this.pnGUID);
            this.pnThumbnail.Children.Add(this.label7);            this.pnThumbnail.Name = "pnThumbnail";
            // tfGUIDTemp
            //            this.tfGUIDTemp.Name = "tfGUIDTemp";
            this.tfGUIDTemp.IsCheckedChanged += (s, e) => this.tfGUIDTemp_CheckedChanged(s, e);
            // pnGUID
            // 
            this.pnGUID.Children.Add(this.label8);
            this.pnGUID.Children.Add(this.tbGUID);            this.pnGUID.Name = "pnGUID";
            // label8
            //            this.label8.Name = "label8";
            // tbGUID
            //            this.tbGUID.Name = "tbGUID";
            this.tbGUID.LostFocus += (s, e) => this.hex32_Validated(s, e);
            //            this.label7.Name = "label7";
            // rbIconSourceObj
            //            this.rbIconSourceObj.Name = "rbIconSourceObj";
            this.rbIconSourceObj.IsCheckedChanged += (s, e) => this.rbIconSourceObj_CheckedChanged(s, e);
            // rbIconSourceTN
            //            this.rbIconSourceTN.Name = "rbIconSourceTN";
            this.rbIconSourceTN.IsCheckedChanged += (s, e) => this.rbIconSourceTN_CheckedChanged(s, e);
            // tfIconTemp
            //            this.tfIconTemp.Name = "tfIconTemp";
            this.tfIconTemp.IsCheckedChanged += (s, e) => this.tfIconTemp_CheckedChanged(s, e);
            // pnIconIndex
            // 
            this.pnIconIndex.Children.Add(this.label6);
            this.pnIconIndex.Children.Add(this.tbIconIndex);            this.pnIconIndex.Name = "pnIconIndex";
            // label6
            //            this.label6.Name = "label6";
            // tbIconIndex
            //            this.tbIconIndex.Name = "tbIconIndex";
            this.tbIconIndex.LostFocus += (s, e) => this.hex8_Validated(s, e);
            //            this.label10.Name = "label10";
            // label4
            //            this.label4.Name = "label4";
            // UI
            //            this.Controls.Add(this.pnWiz0x0032);
		}
		#endregion

        private void hex8_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (hex8_IsValid(sender)) return;

            e.Cancel = true;

            bool origstate = internalchg;
            internalchg = true;
            ((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(inst.Reserved1[0x03]);
            ((TextBoxCompat)sender).SelectAll();
            internalchg = origstate;
        }

        private void hex8_Validated(object sender, System.EventArgs e)
        {
            bool origstate = internalchg;
            internalchg = true;
            ((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(Convert.ToByte(((TextBoxCompat)sender).Text, 16));
            ((TextBoxCompat)sender).SelectAll();
            internalchg = origstate;
        }

        private void hex16_TextChanged(object sender, System.EventArgs ev)
        {
            if (internalchg) return;
            if (inst.NodeVersion < 2 && !hex8_IsValid(sender)) return;
            else if (!hex16_IsValid(sender)) return;

            ushort val = Convert.ToUInt16(((TextBoxCompat)sender).Text, 16);
            this.lbActionString.Content = ((BhavWiz)inst).readStr(this.Scope, GS.GlobalStr.MakeAction, (ushort)(val - 1), -1, pjse.Detail.ErrorNames);
        }

        private void hex16_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (inst.NodeVersion < 2 && hex8_IsValid(sender)) return;
            else if (hex16_IsValid(sender)) return;

            e.Cancel = true;

            bool origstate = internalchg;
            internalchg = true;
            ushort val = inst.NodeVersion < 2 ? inst.Operands[0x04] : BhavWiz.ToShort(inst.Reserved1[0x06], inst.Reserved1[0x07]);
            this.lbActionString.Content = ((BhavWiz)inst).readStr(this.Scope, GS.GlobalStr.MakeAction, (ushort)(val - 1), -1, pjse.Detail.ErrorNames);
            ((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(val);
            ((TextBoxCompat)sender).SelectAll();
            internalchg = origstate;
        }

        private void hex16_Validated(object sender, System.EventArgs e)
        {
            bool origstate = internalchg;
            internalchg = true;
            ((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(Convert.ToUInt16(((TextBoxCompat)sender).Text, 16));
            ((TextBoxCompat)sender).SelectAll();
            internalchg = origstate;
        }

        private void hex32_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (hex32_IsValid(sender)) return;

            e.Cancel = true;

            bool origstate = internalchg;
            internalchg = true;
            ((TextBoxCompat)sender).Text
               = "0x" + SimPe.Helper.HexString(inst.Operands[0x05] | (inst.Operands[0x06] << 8) | (inst.Operands[0x07] << 16) | (inst.Reserved1[0x00] << 24));
            ((TextBoxCompat)sender).SelectAll();
            internalchg = origstate;
        }

        private void hex32_Validated(object sender, System.EventArgs e)
        {
            bool origstate = internalchg;
            internalchg = true;
            ((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(Convert.ToUInt32(((TextBoxCompat)sender).Text, 16));
            ((TextBoxCompat)sender).SelectAll();
            internalchg = origstate;
        }

        private void cbScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (internalchg) return;
            this.lbActionString.Text = ((BhavWiz)inst).readStr(this.Scope, GS.GlobalStr.MakeAction, (ushort)(Convert.ToByte(this.tbStrIndex.Text, 16) - 1), -1, pjse.Detail.ErrorNames);
        }

        private void tfActionTemp_CheckedChanged(object sender, EventArgs e)
        {
            this.pnStrIndex.IsEnabled = !((CheckBoxCompat2)sender).Checked;
        }

        private void tfIconTemp_CheckedChanged(object sender, EventArgs e)
        {
            this.pnIconIndex.IsEnabled = !((CheckBoxCompat2)sender).Checked;
        }

        private void rbModeAction_CheckedChanged(object sender, EventArgs e)
        {
            this.pnAction.IsEnabled = ((RadioButton)sender).IsChecked == true;
        }

        private void rbModeIcon_CheckedChanged(object sender, EventArgs e)
        {
            this.pnIcon.IsEnabled = ((RadioButton)sender).IsChecked == true;
        }

        private void rbIconSourceTN_CheckedChanged(object sender, EventArgs e)
        {
            this.pnThumbnail.IsEnabled = ((RadioButton)sender).IsChecked == true;
        }

        private void rbIconSourceObj_CheckedChanged(object sender, EventArgs e)
        {
            this.pnObject.IsEnabled = ((RadioButton)sender).IsChecked == true;
        }

        private void tfGUIDTemp_CheckedChanged(object sender, EventArgs e)
        {
            this.pnGUID.IsEnabled = !((CheckBoxCompat2)sender).Checked;
        }

        private void btnActionString_Click(object sender, EventArgs e)
        {
            doStrChooser();
        }

	}

}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x0032 : pjse.ABhavOperandWiz
	{
		public BhavOperandWiz0x0032(Instruction i) : base(i) { myForm = new Wiz0x0032.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
