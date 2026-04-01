/***************************************************************************
 *   Copyright (C) 2007 by Peter L Jones                                   *
 *   pljones@users.sf.net                                                  *
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
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
using SimPe.PackedFiles.Wrapper;

namespace pjse.BhavOperandWizards.Wiz0x0079
{
	/// <summary>
	/// Summary description for BhavInstruction.
	/// </summary>
    internal class UI : Window, iBhavOperandWizForm
    {
        #region Form variables

        internal StackPanel pnWiz0x0079;
        private CheckBoxCompat2 checkClear;
        private CheckBoxCompat2 checkRebuild;
        private CheckBoxCompat2 checkSave;
        private CheckBoxCompat2 checkAttrPicker;
        private TextBoxCompat textDataValue1;
        private LabelCompat lblTarget;
        private ComboBoxCompat comboDataPicker1;
        private ComboBoxCompat comboDataOwner1;
        private ComboBoxCompat comboSource;
        private CheckBoxCompat2 checkDecimal;
        private CheckBoxCompat2 checkOutfitVariable;
        private ComboBoxCompat comboOutfitType;
        private LabelCompat lblOutfitType;
        private LabelCompat lblGuid;
        private TextBoxCompat textGUID;
        private LabelCompat lblSource;
        private ComboBoxCompat comboDataOwner2;
        private Panel panelVariable;
        private LabelCompat lblVariable;
        private ComboBoxCompat comboDataPicker2;
        private TextBoxCompat textDataValue2;
        /// <summary>
        /// Required designer variable.
        /// </summary>
                #endregion

        public UI()
        {
            //
            // Required designer variable.
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
            }


            inst = null;
        }

        private Instruction inst = null;
        private DataOwnerControl doVariable = null;
        private DataOwnerControl doTarget = null;

        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x0079; } }

        public void Execute(Instruction inst)
        {
            this.inst = inst;
            this.comboOutfitType.Items.Clear();
            this.comboOutfitType.Items.AddRange(BhavWiz.readStr(GS.BhavStr.PersonOutfits).ToArray());

            wrappedByteArray ops1 = inst.Operands;
            wrappedByteArray ops2 = inst.Reserved1;
            Boolset boolset0 = (Boolset)ops1[0];

            comboSource.SelectedIndex = (boolset0[0] ? 3 : (boolset0[1] ? 1 : (boolset0[6] ? 2 : 0)));
            checkOutfitVariable.IsChecked = boolset0[2];
            checkSave.IsChecked = boolset0[3];
            checkRebuild.IsChecked = boolset0[4];
            checkClear.IsChecked = boolset0[5];

            doVariable = new DataOwnerControl(inst, this.comboDataOwner2, this.comboDataPicker2, this.textDataValue2, this.checkDecimal, this.checkAttrPicker, null, ops1[1], BhavWiz.ToShort(ops1[2], ops1[3]));
            doTarget = new DataOwnerControl(inst, this.comboDataOwner1, this.comboDataPicker1, this.textDataValue1, this.checkDecimal, this.checkAttrPicker, null, ops2[1], BhavWiz.ToShort(ops2[2], ops2[3]));
            textGUID.Text = "0x" + SimPe.Helper.HexString(ops1[4] | (ops1[5] << 8) | (ops1[6] << 16) | (ops1[7] << 24));
            if ((int)ops2[0] < comboOutfitType.Items.Count) comboOutfitType.SelectedIndex = (int)ops2[0];

            UpdatePanelState();
        }

        public Instruction Write(Instruction inst)
        {
            if (inst != null)
            {
                wrappedByteArray ops1 = inst.Operands;
                wrappedByteArray ops2 = inst.Reserved1;

                uint uint32 = Convert.ToUInt32(textGUID.Text, 16);
                Boolset boolset0 = (Boolset)ops1[0];
                boolset0[0] = (comboSource.SelectedIndex == 3);
                boolset0[1] = (comboSource.SelectedIndex == 1);
                boolset0[2] = checkOutfitVariable.IsChecked == true;
                boolset0[3] = checkSave.IsChecked == true;
                boolset0[4] = checkRebuild.IsChecked == true;
                boolset0[5] = checkClear.IsChecked == true;
                boolset0[6] = (comboSource.SelectedIndex == 2);
                ops1[0] = (byte)boolset0;
                ops1[1] = doVariable.DataOwner;
                ops1[2] = (byte)doVariable.Value;
                ops1[3] = (byte)(doVariable.Value >> 8);
                ops1[4] = (byte)(uint32 & (uint)byte.MaxValue);
                ops1[5] = (byte)(uint32 >> 8 & (uint)byte.MaxValue);
                ops1[6] = (byte)(uint32 >> 16 & (uint)byte.MaxValue);
                ops1[7] = (byte)(uint32 >> 24 & (uint)byte.MaxValue);

                ops2[0] = (byte)(comboOutfitType.SelectedIndex);
                ops2[1] = doTarget.DataOwner;
                ops2[2] = (byte)doTarget.Value;
                ops2[3] = (byte)(doTarget.Value >> 8);
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
        {
            this.pnWiz0x0079 = new StackPanel();
            this.checkClear = new CheckBoxCompat2();
            this.checkRebuild = new CheckBoxCompat2();
            this.checkSave = new CheckBoxCompat2();
            this.checkAttrPicker = new CheckBoxCompat2();
            this.textDataValue1 = new TextBoxCompat();
            this.lblTarget = new LabelCompat();
            this.comboDataPicker1 = new ComboBoxCompat();
            this.comboDataOwner1 = new ComboBoxCompat();
            this.comboSource = new ComboBoxCompat();
            this.checkDecimal = new CheckBoxCompat2();
            this.checkOutfitVariable = new CheckBoxCompat2();
            this.comboOutfitType = new ComboBoxCompat();
            this.panelVariable = new StackPanel();
            this.comboDataOwner2 = new ComboBoxCompat();
            this.lblVariable = new LabelCompat();
            this.comboDataPicker2 = new ComboBoxCompat();
            this.textDataValue2 = new TextBoxCompat();
            this.lblOutfitType = new LabelCompat();
            this.lblGuid = new LabelCompat();
            this.textGUID = new TextBoxCompat();
            this.lblSource = new LabelCompat();            // pnWiz0x0079
            // 
            this.pnWiz0x0079.Children.Add(this.checkClear);
            this.pnWiz0x0079.Children.Add(this.checkRebuild);
            this.pnWiz0x0079.Children.Add(this.checkSave);
            this.pnWiz0x0079.Children.Add(this.checkAttrPicker);
            this.pnWiz0x0079.Children.Add(this.textDataValue1);
            this.pnWiz0x0079.Children.Add(this.lblTarget);
            this.pnWiz0x0079.Children.Add(this.comboDataPicker1);
            this.pnWiz0x0079.Children.Add(this.comboDataOwner1);
            this.pnWiz0x0079.Children.Add(this.comboSource);
            this.pnWiz0x0079.Children.Add(this.checkDecimal);
            this.pnWiz0x0079.Children.Add(this.checkOutfitVariable);
            this.pnWiz0x0079.Children.Add(this.comboOutfitType);
            this.pnWiz0x0079.Children.Add(this.panelVariable);
            this.pnWiz0x0079.Children.Add(this.lblOutfitType);
            this.pnWiz0x0079.Children.Add(this.lblGuid);
            this.pnWiz0x0079.Children.Add(this.textGUID);
            this.pnWiz0x0079.Children.Add(this.lblSource);
            this.pnWiz0x0079.Name = "pnWiz0x0079";
            // checkClear
            // 
            this.checkClear.Name = "checkClear";
            this.checkClear.Text = "Clear GUID pointers in person data fields:";
            // 
            this.checkRebuild.Name = "checkRebuild";
            this.checkRebuild.Text = "Rebuild:";
            // 
            this.checkSave.Name = "checkSave";
            this.checkSave.Text = "Save change:";
            // 
            this.checkAttrPicker.Name = "checkAttrPicker";
            this.checkAttrPicker.Text = "use Attribute picker";
            // 
            this.textDataValue1.Name = "textDataValue1";
            // lblTarget
            // 
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Content = "Target:";
            // comboDataPicker1
            // 
            this.comboDataPicker1.Name = "comboDataPicker1";
            // comboDataOwner1
            // 
            this.comboDataOwner1.Name = "comboDataOwner1";
            // comboSource
            // 
            this.comboSource.Items.Add("the sim's outfits");
            this.comboSource.Items.Add("specific GUID");
            this.comboSource.Items.Add("GUID [Temp 0x0000,1]");
            this.comboSource.Items.Add("Stack Object");
            this.comboSource.Name = "comboSource";
            this.comboSource.SelectionChanged += (s, e) => this.comboSource_SelectedIndexChanged(s, e);
            // checkDecimal
            // 
            this.checkDecimal.Name = "checkDecimal";
            this.checkDecimal.Text = "Decimal (except Consts)";
            // 
            this.checkOutfitVariable.Name = "checkOutfitVariable";
            this.checkOutfitVariable.Text = "Use Variable";
            this.checkOutfitVariable.IsCheckedChanged += (s, e) => this.checkOutfitVariable_CheckedChanged(s, e);
            // comboOutfitType
            // 
            this.comboOutfitType.Name = "comboOutfitType";
            // panelVariable
            // 
            this.panelVariable.Children.Add(this.comboDataOwner2);
            this.panelVariable.Children.Add(this.lblVariable);
            this.panelVariable.Children.Add(this.comboDataPicker2);
            this.panelVariable.Children.Add(this.textDataValue2);
            this.panelVariable.Name = "panelVariable";
            // comboDataOwner2
            // 
            this.comboDataOwner2.Name = "comboDataOwner2";
            // lblVariable
            // 
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Content = "Variable:";
            // comboDataPicker2
            // 
            this.comboDataPicker2.Name = "comboDataPicker2";
            // textDataValue2
            // 
            this.textDataValue2.Name = "textDataValue2";
            this.textDataValue2.IsVisible = false;
            // lblOutfitType
            // 
            this.lblOutfitType.Name = "lblOutfitType";
            this.lblOutfitType.Content = "Outfit index:";
            // lblGuid
            // 
            this.lblGuid.Name = "lblGuid";
            this.lblGuid.Content = "GUID";
            // textGUID
            // 
            this.textGUID.Name = "textGUID";
            this.textGUID.TextChanged += (s, e) => this.textGUID_TextChanged(s, e);
            // lblSource
            // 
            this.lblSource.Name = "lblSource";
            this.lblSource.Content = "Source:";
            // UI
            // 
            this.Content = this.pnWiz0x0079;            this.Name = "UI";

        }
        #endregion

        private void textGUID_TextChanged(object sender, EventArgs e)
        {
            uint guid = 0;
            try { guid = Convert.ToUInt32(((TextBoxCompat)sender).Text, 16); }
            catch { return; }
            lblGuid.Content = pjse.BhavWiz.FormatGUID(true, guid);
        }

        private void checkOutfitVariable_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePanelState();
        }

        private void comboSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePanelState();
        }

        private void UpdatePanelState()
        {
            textGUID.IsVisible = lblGuid.IsVisible = (comboSource.SelectedIndex == 1);
            comboOutfitType.IsVisible = !checkOutfitVariable.IsChecked == true;
            panelVariable.IsVisible = checkOutfitVariable.IsChecked == true;
        }
    }
}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x0079 : pjse.ABhavOperandWiz
	{
		public BhavOperandWiz0x0079(Instruction i) : base(i) { myForm = new Wiz0x0079.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
