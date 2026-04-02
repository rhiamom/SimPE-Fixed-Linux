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

namespace pjse.BhavOperandWizards.Wiz0x0076
{
	/// <summary>
	/// Summary description for BhavInstruction.
	/// </summary>
    internal class UI : Window, iBhavOperandWizForm
    {
        #region Form variables

        internal StackPanel pnWiz0x0076;
        private RadioButton rb1StackObj;
        private RadioButton rb1My;
        private TableLayoutPanel tableLayoutPanel1;
        private LabelCompat lbOp2;
        private Panel pnOp1;
        private LabelCompat lbConst1;
        private ComboBoxCompat cbPicker1;
        private TextBoxCompat tbval1;
        private ComboBoxCompat cbDataOwner1;
        private LabelCompat lbOp1;
        private Panel pnOp2;
        private LabelCompat lbConst2;
        private ComboBoxCompat cbPicker2;
        private TextBoxCompat tbval2;
        private ComboBoxCompat cbDataOwner2;
        private Panel panel1;
        private CheckBoxCompat2 ckbAttrPicker;
        private CheckBoxCompat2 ckbDecimal;
        private Panel pnArray;
        private Panel panel2;
        private LabelCompat label1;
        private LabelCompat label3;
        private ComboBoxCompat cbOperation;
        private ComboBoxCompat cbObjectArray;
        private TextBoxCompat tbObjectArray;
        private Panel panel3;
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
        private DataOwnerControl doidArray = null;
        private DataOwnerControl doidValue = null;
        private DataOwnerControl doidIndex = null;

        static string sIndex = Localization.GetString("Index");
        static string sValue = Localization.GetString("Value");

        private bool[] d1enable = { false, true, true, true, true, true, true, false, false, false, true, true, false, false, };
        private bool[] d1IndexValue = { false, false, false, false, false, false, false, false, false, false, false, true, false, false, };
        private bool[] d2enable = { false, false, false, false, false, false, true, false, false, true, true, true, false, false, };
        private bool[] d2IndexValue = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, };

        private void setOperation(int val)
        {
            cbOperation.SelectedIndex = (val < cbOperation.Items.Count) ? val : -1;

            pnOp1.IsEnabled = (val < d1enable.Length && d1enable[val]);
            lbOp1.Content = pnOp1.IsEnabled ? (d1IndexValue[val] ? sIndex : sValue) : "";

            pnOp2.IsEnabled = (val < d2enable.Length && d2enable[val]);
            lbOp2.Content = pnOp2.IsEnabled ? (d2IndexValue[val] ? sIndex : sValue) : "";
        }

        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x0076; } }

        public void Execute(Instruction inst)
        {
            this.inst = inst;

			byte[] o = new byte[16];
            ((byte[])inst.Operands).CopyTo(o, 0);
            ((byte[])inst.Reserved1).CopyTo(o, 8);

            setOperation(o[0x01]);
            // See discussion around whether this is a bit vs boolean:
            // http://simlogical.com/SMF/index.php?topic=917.msg6641#msg6641
            rb1StackObj.IsChecked = !(rb1My.IsChecked = (o[0x2] == 0));

            doidArray = new DataOwnerControl(inst, null, this.cbObjectArray, this.tbObjectArray,
                this.ckbDecimal, this.ckbAttrPicker, null,
                0x29, BhavWiz.ToShort(o[0x03], o[0x04]));
            doidValue = new DataOwnerControl(inst, this.cbDataOwner1, this.cbPicker1, this.tbval1,
                this.ckbDecimal, this.ckbAttrPicker, this.lbConst1,
                o[0x05], BhavWiz.ToShort(o[0x06], o[0x07]));
            doidIndex = new DataOwnerControl(inst, this.cbDataOwner2, this.cbPicker2, this.tbval2,
                this.ckbDecimal, this.ckbAttrPicker, this.lbConst2,
                o[0x08], BhavWiz.ToShort(o[0x09], o[0x0a]));
        }

        public Instruction Write(Instruction inst)
        {
            if (inst != null)
            {
                wrappedByteArray ops1 = inst.Operands;
                wrappedByteArray ops2 = inst.Reserved1;

                if (cbOperation.SelectedIndex >= 0)
                    ops1[0x01] = (byte)(cbOperation.SelectedIndex);
                ops1[0x02] = (byte)(rb1My.IsChecked == true ? 0x00 : 0x02); // Not sure why "0x02" at the game treats as 0 / !0

                BhavWiz.FromShort(ref ops1, 3, doidArray.Value);

                ops1[0x05] = doidValue.DataOwner;
                BhavWiz.FromShort(ref ops1, 6, doidValue.Value);

                ops2[0x00] = doidIndex.DataOwner;
                BhavWiz.FromShort(ref ops2, 1, doidIndex.Value);
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
        {            this.pnWiz0x0076 = new StackPanel();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.pnOp2 = new StackPanel();
            this.lbConst2 = new LabelCompat();
            this.cbPicker2 = new ComboBoxCompat();
            this.tbval2 = new TextBoxCompat();
            this.cbDataOwner2 = new ComboBoxCompat();
            this.lbOp2 = new LabelCompat();
            this.pnOp1 = new StackPanel();
            this.lbConst1 = new LabelCompat();
            this.cbPicker1 = new ComboBoxCompat();
            this.tbval1 = new TextBoxCompat();
            this.cbDataOwner1 = new ComboBoxCompat();
            this.lbOp1 = new LabelCompat();
            this.panel1 = new StackPanel();
            this.ckbAttrPicker = new CheckBoxCompat2();
            this.ckbDecimal = new CheckBoxCompat2();
            this.rb1StackObj = new Avalonia.Controls.RadioButton();
            this.rb1My = new Avalonia.Controls.RadioButton();
            this.tbObjectArray = new TextBoxCompat();
            this.cbObjectArray = new ComboBoxCompat();
            this.cbOperation = new ComboBoxCompat();
            this.panel2 = new StackPanel();
            this.label1 = new LabelCompat();
            this.pnArray = new StackPanel();
            this.label3 = new LabelCompat();
            this.panel3 = new StackPanel();            //
            // pnWiz0x0076
            //            this.pnWiz0x0076.Children.Add(this.tableLayoutPanel1);
            this.pnWiz0x0076.Children.Add(this.rb1StackObj);
            this.pnWiz0x0076.Children.Add(this.rb1My);
            this.pnWiz0x0076.Name = "pnWiz0x0076";
            //
            // tableLayoutPanel1
            //            this.tableLayoutPanel1.Children.Add(this.pnArray);
            this.tableLayoutPanel1.Children.Add(this.pnOp2);
            this.tableLayoutPanel1.Children.Add(this.lbOp2);
            this.tableLayoutPanel1.Children.Add(this.pnOp1);
            this.tableLayoutPanel1.Children.Add(this.lbOp1);
            this.tableLayoutPanel1.Children.Add(this.panel1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            //
            // pnOp2
            //            this.pnOp2.Children.Add(this.lbConst2);
            this.pnOp2.Children.Add(this.cbPicker2);
            this.pnOp2.Children.Add(this.tbval2);
            this.pnOp2.Children.Add(this.cbDataOwner2);
            this.pnOp2.Name = "pnOp2";
            //
            // lbConst2
            //            this.lbConst2.Name = "lbConst2";
            //
            // cbPicker2
            //
            this.cbPicker2.Name = "cbPicker2";
            //
            // tbval2
            //            this.tbval2.Name = "tbval2";
            //
            // cbDataOwner2
            //
            this.cbDataOwner2.Name = "cbDataOwner2";
            //
            // lbOp2
            //            this.lbOp2.Name = "lbOp2";
            //
            // pnOp1
            //            this.pnOp1.Children.Add(this.lbConst1);
            this.pnOp1.Children.Add(this.cbPicker1);
            this.pnOp1.Children.Add(this.tbval1);
            this.pnOp1.Children.Add(this.cbDataOwner1);
            this.pnOp1.Name = "pnOp1";
            //
            // lbConst1
            //            this.lbConst1.Name = "lbConst1";
            //
            // cbPicker1
            //
            this.cbPicker1.Name = "cbPicker1";
            //
            // tbval1
            //            this.tbval1.Name = "tbval1";
            //
            // cbDataOwner1
            //
            this.cbDataOwner1.Name = "cbDataOwner1";
            //
            // lbOp1
            //            this.lbOp1.Name = "lbOp1";
            //
            // panel1
            //            this.panel1.Children.Add(this.ckbAttrPicker);
            this.panel1.Children.Add(this.ckbDecimal);
            this.panel1.Name = "panel1";
            //
            // ckbAttrPicker
            //            this.ckbAttrPicker.Name = "ckbAttrPicker";
            //
            // ckbDecimal
            //            this.ckbDecimal.Name = "ckbDecimal";
            //
            // rb1StackObj
            //            this.rb1StackObj.Name = "rb1StackObj";
            // rb1My
            //            this.rb1My.Name = "rb1My";
            // tbObjectArray
            //            this.tbObjectArray.Name = "tbObjectArray";
            //
            // cbObjectArray
            //
            this.cbObjectArray.Name = "cbObjectArray";
            //
            // cbOperation
            //
            this.cbOperation.SelectionChanged += (s, e) => this.cbOperation_SelectedIndexChanged(s, e);
            //
            // panel2
            //            this.panel2.Children.Add(this.label1);
            this.panel2.Children.Add(this.label3);
            this.panel2.Name = "panel2";
            //
            // label1
            //            this.label1.Name = "label1";
            //
            // pnArray
            //            this.pnArray.Children.Add(this.panel3);
            this.pnArray.Children.Add(this.panel2);
            this.pnArray.Children.Add(this.cbOperation);
            this.pnArray.Children.Add(this.cbObjectArray);
            this.pnArray.Children.Add(this.tbObjectArray);
            this.pnArray.Name = "pnArray";
            //
            // label3
            //            this.label3.Name = "label3";
            //
            // panel3
            //this.panel3.Name = "panel3";
            //
            // UI
            //            this.Controls.Add(this.pnWiz0x0076);

        }
        #endregion

        private void cbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            setOperation(cbOperation.SelectedIndex);
        }

    }
}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x0076 : pjse.ABhavOperandWiz
	{
		public BhavOperandWiz0x0076(Instruction i) : base(i) { myForm = new Wiz0x0076.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
