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

namespace pjse.BhavOperandWizards.Wiz0x007c
{

    internal class UI : Window, iBhavOperandWizForm
    {
        #region Form variables

        internal StackPanel pnWiz0x007c;
        private LabelCompat lblType;
        private LabelCompat lblWantName;
        private LabelCompat lblTargetSim;
        private LabelCompat lblSubjectSim;
        private LabelCompat lblLevel;
        private LabelCompat lblWant;
        private TextBoxCompat textDataValue1;
        private TextBoxCompat textDataValue2;
        private TextBoxCompat textDataValue3;
        private ComboBoxCompat comboDataPicker1;
        private ComboBoxCompat comboDataPicker2;
        private ComboBoxCompat comboType;
        private ComboBoxCompat comboDataPicker3;
        private TextBoxCompat textGUID;
        private ComboBoxCompat comboDataOwner1;
        private ComboBoxCompat comboDataOwner2;
        private ComboBoxCompat comboDataOwner3;
        private PictureBox WantIcon;
        private CheckBoxCompat2 checkDecimal;
        private CheckBoxCompat2 checkAttrPicker;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
                #endregion

        public UI()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
            }


            inst = null;
        }

        private Instruction inst = null;
        private DataOwnerControl doTargetSim = null;
        private DataOwnerControl doSubjectSim = null;
        private DataOwnerControl doLevel = null;

        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x007c; } }
        public void Execute(Instruction inst)
        {
            this.inst = inst;

            wrappedByteArray reserved1 = inst.Reserved1;
            wrappedByteArray ops = inst.Operands;

            textGUID.Text = "0x" + SimPe.Helper.HexString(ops[3] | (ops[4] << 8) | (ops[5] << 16) | (ops[6] << 24));
            doTargetSim = new DataOwnerControl(inst, this.comboDataOwner1, this.comboDataPicker1, this.textDataValue1, this.checkDecimal, this.checkAttrPicker, null, ops[0], BhavWiz.ToShort(ops[1], ops[2]));
            doSubjectSim = new DataOwnerControl(inst, this.comboDataOwner2, this.comboDataPicker2, this.textDataValue2, this.checkDecimal, this.checkAttrPicker, null, ops[7], BhavWiz.ToShort(reserved1[0], reserved1[1]));
            doLevel = new DataOwnerControl(inst, this.comboDataOwner3, this.comboDataPicker3, this.textDataValue3, this.checkDecimal, this.checkAttrPicker, null, reserved1[2], BhavWiz.ToShort(reserved1[3], reserved1[4]));
            if ( (int)reserved1[3] < comboType.Items.Count) comboType.SelectedIndex = (int)reserved1[3];

            UpdateWantName();
        }

        public Instruction Write(Instruction inst)
        {
            if (inst != null)
            {
                wrappedByteArray ops = inst.Operands;
                wrappedByteArray reserved1 = inst.Reserved1;;
                uint uint32 = Convert.ToUInt32(textGUID.Text, 16);

                ops[0] = doTargetSim.DataOwner;
                ops[1] = (byte)doTargetSim.Value;
                ops[2] = (byte)(doTargetSim.Value >> 8);
                ops[3] = (byte)(uint32 & (uint)byte.MaxValue);
                ops[4] = (byte)(uint32 >> 8 & (uint)byte.MaxValue);
                ops[5] = (byte)(uint32 >> 16 & (uint)byte.MaxValue);
                ops[6] = (byte)(uint32 >> 24 & (uint)byte.MaxValue);
                ops[7] = doSubjectSim.DataOwner;

                reserved1[0] = (byte)doSubjectSim.Value;
                reserved1[1] = (byte)(doSubjectSim.Value >> 8);
                reserved1[2] = doLevel.DataOwner;
                reserved1[3] = (byte)doLevel.Value;
                reserved1[4] = (byte)(doLevel.Value >> 8);
                reserved1[5] = (byte)(comboType.SelectedIndex);
            }
            return inst;
        }

        #endregion


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnWiz0x007c = new StackPanel();
            this.lblType = new LabelCompat();
            this.lblWantName = new LabelCompat();
            this.lblTargetSim = new LabelCompat();
            this.lblSubjectSim = new LabelCompat();
            this.lblLevel = new LabelCompat();
            this.lblWant = new LabelCompat();
            this.textDataValue1 = new TextBoxCompat();
            this.textDataValue2 = new TextBoxCompat();
            this.textDataValue3 = new TextBoxCompat();
            this.comboDataPicker1 = new ComboBoxCompat();
            this.comboDataPicker2 = new ComboBoxCompat();
            this.comboType = new ComboBoxCompat();
            this.comboDataPicker3 = new ComboBoxCompat();
            this.textGUID = new TextBoxCompat();
            this.comboDataOwner1 = new ComboBoxCompat();
            this.comboDataOwner2 = new ComboBoxCompat();
            this.comboDataOwner3 = new ComboBoxCompat();
            this.WantIcon = new SimPe.Scenegraph.Compat.PictureBox();
            this.checkDecimal = new CheckBoxCompat2();
            // 
            this.pnWiz0x007c.Children.Add(this.lblType);
            this.pnWiz0x007c.Children.Add(this.lblWantName);
            this.pnWiz0x007c.Children.Add(this.lblTargetSim);
            this.pnWiz0x007c.Children.Add(this.lblSubjectSim);
            this.pnWiz0x007c.Children.Add(this.lblLevel);
            this.pnWiz0x007c.Children.Add(this.lblWant);
            this.pnWiz0x007c.Children.Add(this.textDataValue1);
            this.pnWiz0x007c.Children.Add(this.textDataValue2);
            this.pnWiz0x007c.Children.Add(this.textDataValue3);
            this.pnWiz0x007c.Children.Add(this.comboDataPicker1);
            this.pnWiz0x007c.Children.Add(this.comboDataPicker2);
            this.pnWiz0x007c.Children.Add(this.comboType);
            this.pnWiz0x007c.Children.Add(this.comboDataPicker3);
            this.pnWiz0x007c.Children.Add(this.textGUID);
            this.pnWiz0x007c.Children.Add(this.comboDataOwner1);
            this.pnWiz0x007c.Children.Add(this.comboDataOwner2);
            this.pnWiz0x007c.Children.Add(this.comboDataOwner3);
            this.pnWiz0x007c.Children.Add(this.WantIcon);
            this.pnWiz0x007c.Children.Add(this.checkDecimal);
            this.pnWiz0x007c.Children.Add(this.checkAttrPicker);
            this.pnWiz0x007c.Name = "pnWiz0x007c";
            // lblType
            // 
            this.lblType.Name = "lblType";
            this.lblType.Content = "Type:";
            // lblWantName
            // 
            this.lblWantName.Name = "lblWantName";
            this.lblWantName.Content = "want name";
            // lblTargetSim
            // 
            this.lblTargetSim.Name = "lblTargetSim";
            this.lblTargetSim.Content = "Target Sim:";
            // lblSubjectSim
            // 
            this.lblSubjectSim.Name = "lblSubjectSim";
            this.lblSubjectSim.Content = "Target:";
            // lblLevel
            // 
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Content = "(optional) level:";
            // lblWant
            // 
            this.lblWant.Name = "lblWant";
            this.lblWant.Content = "Want:";
            // textDataValue1
            // 
            this.textDataValue1.Name = "textDataValue1";
            // textDataValue2
            // 
            this.textDataValue2.Name = "textDataValue2";
            // textDataValue3
            // 
            this.textDataValue3.Name = "textDataValue3";
            // comboDataPicker1
            // 
            this.comboDataPicker1.Name = "comboDataPicker1";
            // comboDataPicker2
            // 
            this.comboDataPicker2.Name = "comboDataPicker2";
            // comboType
            // 
            this.comboType.Items.Add("Generic");
            this.comboType.Items.Add("Sim");
            this.comboType.Items.Add("Object");
            this.comboType.Items.Add("{unused}");
            this.comboType.Items.Add("Skill");
            this.comboType.Items.Add("Career");
            this.comboType.Name = "comboType";
            this.comboType.SelectionChanged += (s, e) => this.comboType_SelectedIndexChanged(s, e);
            // comboDataPicker3
            // 
            this.comboDataPicker3.Name = "comboDataPicker3";
            // textGUID
            // 
            this.textGUID.Name = "textGUID";
            this.textGUID.TextChanged += (s, e) => this.textGUID_TextChanged(s, e);
            // comboDataOwner1
            // 
            this.comboDataOwner1.Name = "comboDataOwner1";
            // comboDataOwner2
            // 
            this.comboDataOwner2.Name = "comboDataOwner2";
            // comboDataOwner3
            // 
            this.comboDataOwner3.Name = "comboDataOwner3";
            // WantIcon
            // 
            this.WantIcon.Name = "WantIcon";
            this.WantIcon.SizeMode = SimPe.Scenegraph.Compat.PictureBoxSizeMode.Zoom;
            // 
            this.checkDecimal.Name = "checkDecimal";
            this.checkDecimal.Text = "Decimal (except Consts)";
            // 
            this.checkAttrPicker.Name = "checkAttrPicker";
            this.checkAttrPicker.Text = "use Attribute picker";
            // 
            this.Content = this.pnWiz0x007c;            this.Name = "UI";
        }
        #endregion

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWantName();
        }

        private void textGUID_TextChanged(object sender, EventArgs e)
        {
            if (!hex32_IsValid(sender)) return;
            UpdateWantName();
        }

        private bool hex32_IsValid(object sender)
        {
            try { Convert.ToUInt32(((TextBoxCompat)sender).Text, 16); }
            catch (Exception) { return false; }
            return true;
        }

        private void UpdateWantName()
        {
            try
            {
                SimPe.Plugin.WantInformation wantim = SimPe.Plugin.WantInformation.LoadWant(Convert.ToUInt32(textGUID.Text, 16));
                if (wantim != null) { lblWantName.Content = wantim.Name; WantIcon.Image = wantim.Icon; }
                else { lblWantName.Content = textGUID.Text; WantIcon.Image = null; }
            }
            catch { lblWantName.Content = textGUID.Text; WantIcon.Image = null; }
        }
    }
}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x007c : pjse.ABhavOperandWiz
	{
		public BhavOperandWiz0x007c(Instruction i) : base(i) { myForm = new Wiz0x007c.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion
	}
}