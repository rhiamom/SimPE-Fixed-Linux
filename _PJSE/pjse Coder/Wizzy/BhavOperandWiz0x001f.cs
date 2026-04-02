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

namespace pjse.BhavOperandWizards.Wiz0x001f
{
    /// <summary>
    /// Summary description for StrBig.
    /// </summary>
    internal class UI : Window, iBhavOperandWizForm
	{
		#region Form variables

        internal StackPanel pnWiz0x001f;
        private CheckBoxCompat2 ckbStackObj;
        private Panel pnObject;
        private CheckBoxCompat2 cbAttrPicker;
        private CheckBoxCompat2 cbDecimal;
        private ComboBoxCompat cbPicker1;
        private TextBoxCompat tbVal1;
        private ComboBoxCompat cbDataOwner1;
        private LabelCompat label1;
        private Panel pnNodeVersion;
        private CheckBoxCompat2 ckbDisabled;
        private Panel pnWhere;
        private ComboBoxCompat cbWhere;
        private TextBoxCompat tbWhereVal;
        private LabelCompat label4;
        private CheckBoxCompat2 ckbWhere;
        private ComboBoxCompat cbToNext;
        private TextBoxCompat tbLocalVar;
        private TextBoxCompat tbGUID;
        private LabelCompat label2;
        private LabelCompat lbGUIDText;
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

            this.tbGUID.IsVisible = false;
            this.tbGUID.Left = this.cbToNext.Left + this.cbToNext.Width + 3;
            this.tbLocalVar.IsVisible = false;
            this.tbLocalVar.Left = this.cbToNext.Left + this.cbToNext.Width + 3;

            this.cbToNext.Items.AddRange(BhavWiz.readStr(GS.BhavStr.NextObject).ToArray());
            this.cbWhere.Items.AddRange(BhavWiz.readStr(GS.BhavStr.DataLabels).ToArray());
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

        private void setToNext(byte val)
        {
            bool guid = false;
            bool local = false;
            switch (val)
            {
                case 0x04: case 0x07: guid = true; break;
                case 0x09: case 0x22: local = true; break;
            }
            this.lbGUIDText.IsVisible = this.tbGUID.IsVisible = guid;
            this.tbLocalVar.IsVisible = local;
            if (val == cbToNext.SelectedIndex) return;
            cbToNext.SelectedIndex = (val >= cbToNext.Items.Count) ? -1 : val;
        }

        private void setGUID(byte[] o, int sub) { setGUID(true, (UInt32)(o[sub] | o[sub + 1] << 8 | o[sub + 2] << 16 | o[sub + 3] << 24)); }
        private void setGUID(bool setTB, UInt32 guid)
        {
            if (setTB) this.tbGUID.Text = "0x" + SimPe.Helper.HexString(guid);
            this.lbGUIDText.Content = BhavWiz.FormatGUID(true, guid);
        }

        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x001f; } }

        public void Execute(Instruction inst)
        {
            this.inst = inst;

            wrappedByteArray ops1 = inst.Operands;
            wrappedByteArray ops2 = inst.Reserved1;

            internalchg = true;

            setGUID(ops1, 0);

            this.cbToNext.SelectedIndex = -1;
            setToNext((byte)(ops1[4] & 0x7f));

            this.ckbStackObj.IsChecked = (ops1[4] & 0x80) == 0;
            this.pnObject.IsEnabled = !this.ckbStackObj.IsChecked == true;

            doid1 = new DataOwnerControl(inst, this.cbDataOwner1, this.cbPicker1, this.tbVal1, this.cbDecimal, this.cbAttrPicker, null,
                ops1[0x05], ops1[0x07]);

            this.tbLocalVar.Text = "0x" + SimPe.Helper.HexString(ops1[0x06]);

            this.pnNodeVersion.IsEnabled = (inst.NodeVersion != 0);
            this.ckbDisabled.IsChecked = (ops2[0x00] & 0x01) != 0;
            this.ckbWhere.IsChecked = (ops2[0x00] & 0x02) != 0;
            this.pnWhere.IsEnabled = (ops2[0x00] & 0x02) != 0;

            ushort where = BhavWiz.ToShort(ops2[0x01], ops2[0x02]);
            this.cbWhere.SelectedIndex = -1;
            if (this.cbWhere.Items.Count > where)
                this.cbWhere.SelectedIndex = where;
            this.tbWhereVal.Text = "0x" + SimPe.Helper.HexString(BhavWiz.ToShort(ops2[0x03], ops2[0x04]));

            internalchg = false;
        }

		public Instruction Write(Instruction inst)
		{
			if (inst != null)
			{
                wrappedByteArray ops1 = inst.Operands;
                wrappedByteArray ops2 = inst.Reserved1;

                UInt32 val = Convert.ToUInt32(this.tbGUID.Text, 16);
                ops1[0x00] = (byte)(val & 0xff);
                ops1[0x01] = (byte)(val >> 8 & 0xff);
                ops1[0x02] = (byte)(val >> 16 & 0xff);
                ops1[0x03] = (byte)(val >> 24 & 0xff);
                if (this.cbToNext.SelectedIndex >= 0)
                    ops1[0x04] = (byte)(this.cbToNext.SelectedIndex & 0x7f);
                ops1[0x04] |= (byte)(!this.ckbStackObj.IsChecked == true ? 0x80 : 0x00);
                ops1[0x05] = doid1.DataOwner;
                ops1[0x06] = Convert.ToByte(this.tbLocalVar.Text, 16);
                ops1[0x07] = (byte)(doid1.Value & 0xff);

                ops2[0x00] &= 0xfc;
                ops2[0x00] |= (byte)(this.ckbDisabled.IsChecked == true ? 0x01 : 0x00);
                ops2[0x00] |= (byte)(this.ckbWhere.IsChecked == true ? 0x02 : 0x00);
                if (this.cbWhere.SelectedIndex >= 0)
                    BhavWiz.FromShort(ref ops2, 1, (ushort)this.cbWhere.SelectedIndex);
                BhavWiz.FromShort(ref ops2, 3, (ushort)Convert.ToUInt32(this.tbWhereVal.Text, 16));
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
		{            this.pnWiz0x001f = new StackPanel();
            this.cbToNext = new ComboBoxCompat();
            this.tbLocalVar = new TextBoxCompat();
            this.tbGUID = new TextBoxCompat();
            this.lbGUIDText = new LabelCompat();
            this.label2 = new LabelCompat();
            this.pnNodeVersion = new StackPanel();
            this.pnWhere = new StackPanel();
            this.cbWhere = new ComboBoxCompat();
            this.tbWhereVal = new TextBoxCompat();
            this.label4 = new LabelCompat();
            this.ckbWhere = new CheckBoxCompat2();
            this.ckbDisabled = new CheckBoxCompat2();
            this.label1 = new LabelCompat();
            this.pnObject = new StackPanel();
            this.cbAttrPicker = new CheckBoxCompat2();
            this.cbDecimal = new CheckBoxCompat2();
            this.cbPicker1 = new ComboBoxCompat();
            this.tbVal1 = new TextBoxCompat();
            this.cbDataOwner1 = new ComboBoxCompat();
            this.ckbStackObj = new CheckBoxCompat2();            // pnWiz0x001f
            // 
            this.pnWiz0x001f.Children.Add(this.cbToNext);
            this.pnWiz0x001f.Children.Add(this.tbLocalVar);
            this.pnWiz0x001f.Children.Add(this.tbGUID);
            this.pnWiz0x001f.Children.Add(this.lbGUIDText);
            this.pnWiz0x001f.Children.Add(this.label2);
            this.pnWiz0x001f.Children.Add(this.pnNodeVersion);
            this.pnWiz0x001f.Children.Add(this.label1);
            this.pnWiz0x001f.Children.Add(this.pnObject);
            this.pnWiz0x001f.Children.Add(this.ckbStackObj);            this.pnWiz0x001f.Name = "pnWiz0x001f";
            // cbToNext
            // 
            this.cbToNext.SelectionChanged += (s, e) => this.cbToNext_SelectedIndexChanged(s, e);
            // tbLocalVar
            //            this.tbLocalVar.Name = "tbLocalVar";
            this.tbLocalVar.LostFocus += (s, e) => this.hex8_Validated(s, e);
            //            this.tbGUID.Name = "tbGUID";
            this.tbGUID.TextChanged += (s, e) => this.tbGUID_TextChanged(s, e);
            this.tbGUID.LostFocus += (s, e) => this.hex32_Validated(s, e);
            //            this.lbGUIDText.Name = "lbGUIDText";
            // label2
            //            this.label2.Name = "label2";
            // pnNodeVersion
            // 
            this.pnNodeVersion.Children.Add(this.pnWhere);
            this.pnNodeVersion.Children.Add(this.ckbWhere);
            this.pnNodeVersion.Children.Add(this.ckbDisabled);            this.pnNodeVersion.Name = "pnNodeVersion";
            // pnWhere
            // 
            this.pnWhere.Children.Add(this.cbWhere);
            this.pnWhere.Children.Add(this.tbWhereVal);
            this.pnWhere.Children.Add(this.label4);            this.pnWhere.Name = "pnWhere";
            // cbWhere
            // 
            // tbWhereVal
            //            this.tbWhereVal.Name = "tbWhereVal";
            this.tbWhereVal.LostFocus += (s, e) => this.hex16_Validated(s, e);
            //            this.label4.Name = "label4";
            // ckbWhere
            //            this.ckbWhere.Name = "ckbWhere";
            this.ckbWhere.IsCheckedChanged += (s, e) => this.ckbWhere_CheckedChanged(s, e);
            // ckbDisabled
            //            this.ckbDisabled.Name = "ckbDisabled";
            //            this.label1.Name = "label1";
            // pnObject
            // 
            this.pnObject.Children.Add(this.cbAttrPicker);
            this.pnObject.Children.Add(this.cbDecimal);
            this.pnObject.Children.Add(this.cbPicker1);
            this.pnObject.Children.Add(this.tbVal1);
            this.pnObject.Children.Add(this.cbDataOwner1);            this.pnObject.Name = "pnObject";
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
            // ckbStackObj
            //            this.ckbStackObj.Name = "ckbStackObj";
            this.ckbStackObj.IsCheckedChanged += (s, e) => this.ckbStackObj_CheckedChanged(s, e);
            // UI
            //            this.Controls.Add(this.pnWiz0x001f);
		}
		#endregion

        private void hex8_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (hex8_IsValid(sender)) return;

            e.Cancel = true;

            bool origstate = internalchg;
            internalchg = true;
            ((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(inst.Operands[0x06]);
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

        private void hex16_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (inst.NodeVersion < 2 && hex8_IsValid(sender)) return;
            else if (hex16_IsValid(sender)) return;

            e.Cancel = true;

            bool origstate = internalchg;
            internalchg = true;
            ((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(BhavWiz.ToShort(inst.Reserved1[0x03], inst.Reserved1[0x04]));
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

        private void tbGUID_TextChanged(object sender, EventArgs e)
        {
            if (!hex32_IsValid(sender)) return;
            setGUID(false, Convert.ToUInt32(((TextBoxCompat)sender).Text, 16));
        }

        private void hex32_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (hex32_IsValid(sender)) return;

            e.Cancel = true;

            bool origstate = internalchg;
            internalchg = true;
            ((TextBoxCompat)sender).Text
               = "0x" + SimPe.Helper.HexString(inst.Operands[0x00] | (inst.Operands[0x01] << 8) | (inst.Operands[0x02] << 16) | (inst.Operands[0x03] << 24));
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

        private void cbToNext_SelectedIndexChanged(object sender, EventArgs e)
        {
            setToNext((byte)((ComboBoxCompat)sender).SelectedIndex);
        }

        private void ckbStackObj_CheckedChanged(object sender, EventArgs e)
        {
            this.pnObject.IsEnabled = !this.ckbStackObj.IsChecked == true;
        }

        private void ckbWhere_CheckedChanged(object sender, EventArgs e)
        {
            this.pnWhere.IsEnabled = this.ckbWhere.IsChecked == true;
        }

	}

}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x001f : pjse.ABhavOperandWiz
	{
        public BhavOperandWiz0x001f(Instruction i) : base(i) { myForm = new Wiz0x001f.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
