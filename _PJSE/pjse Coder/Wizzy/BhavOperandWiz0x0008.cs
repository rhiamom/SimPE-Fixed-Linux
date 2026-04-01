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

namespace pjse.BhavOperandWizards.Wiz0x0008
{
	/// <summary>
	/// Summary description for BhavInstruction.
	/// </summary>
    internal class UI : Window, iBhavOperandWizForm
    {
        #region Form variables

        private TextBoxCompat tbval1;
        private TextBoxCompat tbval2;
        internal StackPanel pnWiz0x0008;
        private ComboBoxCompat cbPicker1;
        private ComboBoxCompat cbPicker2;
        private ComboBoxCompat cbDataOwner1;
        private ComboBoxCompat cbDataOwner2;
        private CheckBoxCompat2 cbDecimal;
        private CheckBoxCompat2 cbAttrPicker;
        private LabelCompat lbConst2;
        private LabelCompat lbConst1;
        private LabelCompat label2;
        private LabelCompat label1;
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
        private DataOwnerControl doid1 = null;
        private DataOwnerControl doid2 = null;

        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x0008; } }
        public void Execute(Instruction inst)
        {
            this.inst = inst;

            wrappedByteArray ops = inst.Operands;

            doid1 = new DataOwnerControl(inst, this.cbDataOwner1, this.cbPicker1, this.tbval1, this.cbDecimal, this.cbAttrPicker, this.lbConst1,
                ops[0x02], (ushort)((ops[0x01] << 8) | ops[0x00]));
            doid2 = new DataOwnerControl(inst, this.cbDataOwner2, this.cbPicker2, this.tbval2, this.cbDecimal, this.cbAttrPicker, this.lbConst2,
                ops[0x06], (ushort)((ops[0x05] << 8) | ops[0x04]));
        }

        public Instruction Write(Instruction inst)
        {
            if (inst != null)
            {
                wrappedByteArray ops = inst.Operands;
                ops[0x02] = doid1.DataOwner;
                ops[0x00] = (byte)(doid1.Value & 0xff);
                ops[0x01] = (byte)((doid1.Value >> 8) & 0xff);
                ops[0x06] = doid2.DataOwner;
                ops[0x04] = (byte)(doid2.Value & 0xff);
                ops[0x05] = (byte)((doid2.Value >> 8) & 0xff);
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
        {            this.pnWiz0x0008 = new StackPanel();
            this.label2 = new LabelCompat();
            this.label1 = new LabelCompat();
            this.lbConst2 = new LabelCompat();
            this.lbConst1 = new LabelCompat();
            this.cbAttrPicker = new CheckBoxCompat2();
            this.cbDecimal = new CheckBoxCompat2();
            this.cbPicker2 = new ComboBoxCompat();
            this.cbPicker1 = new ComboBoxCompat();
            this.tbval2 = new TextBoxCompat();
            this.cbDataOwner2 = new ComboBoxCompat();
            this.tbval1 = new TextBoxCompat();
            this.cbDataOwner1 = new ComboBoxCompat();            //
            // pnWiz0x0008
            //
            this.pnWiz0x0008.Children.Add(this.label2);
            this.pnWiz0x0008.Children.Add(this.label1);
            this.pnWiz0x0008.Children.Add(this.lbConst2);
            this.pnWiz0x0008.Children.Add(this.lbConst1);
            this.pnWiz0x0008.Children.Add(this.cbAttrPicker);
            this.pnWiz0x0008.Children.Add(this.cbDecimal);
            this.pnWiz0x0008.Children.Add(this.cbPicker2);
            this.pnWiz0x0008.Children.Add(this.cbPicker1);
            this.pnWiz0x0008.Children.Add(this.tbval2);
            this.pnWiz0x0008.Children.Add(this.cbDataOwner2);
            this.pnWiz0x0008.Children.Add(this.tbval1);
            this.pnWiz0x0008.Children.Add(this.cbDataOwner1);            this.pnWiz0x0008.Name = "pnWiz0x0008";
            //
            // label2
            //            this.label2.Name = "label2";
            //
            // label1
            //            this.label1.Name = "label1";
            //
            // lbConst2
            //            this.lbConst2.Name = "lbConst2";
            //
            // lbConst1
            //            this.lbConst1.Name = "lbConst1";
            //
            // cbAttrPicker
            //            this.cbAttrPicker.Name = "cbAttrPicker";
            //
            // cbDecimal
            //            this.cbDecimal.Name = "cbDecimal";
            //
            // cbPicker2
            //            this.cbPicker2.Name = "cbPicker2";
            //
            // cbPicker1
            //            this.cbPicker1.Name = "cbPicker1";
            //
            // tbval2
            //            this.tbval2.Name = "tbval2";
            //
            // cbDataOwner2
            //            this.cbDataOwner2.Name = "cbDataOwner2";
            //
            // tbval1
            //            this.tbval1.Name = "tbval1";
            //
            // cbDataOwner1
            //            this.cbDataOwner1.Name = "cbDataOwner1";
            //
            // UI
            //            this.Controls.Add(this.pnWiz0x0008);
            this.Name = "UI";
        }
        #endregion

    }
}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x0008 : pjse.ABhavOperandWiz
	{
		public BhavOperandWiz0x0008(Instruction i) : base(i) { myForm = new Wiz0x0008.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
