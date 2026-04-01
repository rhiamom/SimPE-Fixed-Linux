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

namespace pjse.BhavOperandWizards.Wiz0x002d
{
    /// <summary>
    /// Summary description for StrBig.
    /// </summary>
    internal class UI : Window, iBhavOperandWizForm
	{
		#region Form variables

        internal StackPanel pnWiz0x002d;
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox gbRoutingSlot;
        private Panel pnObject;
        private ComboBoxCompat cbSlotType;
        private CheckBoxCompat2 ckbDecimal;
        private TextBoxCompat tbVal1;
        private CheckBoxCompat2 ckbNFailTrees;
        private CheckBoxCompat2 ckbIgnDstFootprint;
        private CheckBoxCompat2 ckbDiffAlts;
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
        //private bool internalchg = false;

        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x002d; } }

        public void Execute(Instruction inst)
        {
            this.inst = inst;

            wrappedByteArray ops1 = inst.Operands;
            wrappedByteArray ops2 = inst.Reserved1;
            Boolset ops14 = ops1[4];

            //internalchg = true;

            doid1 = new DataOwnerControl(inst, null, null, this.tbVal1, this.ckbDecimal, null, null,
                0x07, BhavWiz.ToShort(ops1[0x00], ops1[0x01])); // Literal

            int i = 0;
            if (!ops14[1]) i = BhavWiz.ToShort(ops1[2], ops1[3]);
            if (i < cbSlotType.Items.Count) cbSlotType.SelectedIndex = i;

            ckbNFailTrees.IsChecked = ops14[0];
            ckbIgnDstFootprint.IsChecked = ops14[2];
            ckbDiffAlts.IsChecked = ops14[3];

            //internalchg = false;
        }

		public Instruction Write(Instruction inst)
		{
			if (inst != null)
			{
                wrappedByteArray ops1 = inst.Operands;
                wrappedByteArray ops2 = inst.Reserved1;
                Boolset ops14 = ops1[4];

                ops1[0] = (byte)doid1.Value;
                ops1[1] = (byte)(doid1.Value >> 8);

                if ((cbSlotType.SelectedIndex) >= 1)
                {
                    ops1[2] = (byte)(cbSlotType.SelectedIndex - 1);
                    ops1[3] = (byte)((cbSlotType.SelectedIndex - 1) >> 8);
                }

                ops14[0] = ckbNFailTrees.IsChecked == true;
                ops14[1] = (cbSlotType.SelectedIndex == 0);
                ops14[2] = ckbIgnDstFootprint.IsChecked == true;
                ops14[3] = ckbDiffAlts.IsChecked == true;
                ops1[4] = ops14;

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
		{            this.pnWiz0x002d = new StackPanel();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.gbRoutingSlot = new SimPe.Scenegraph.Compat.GroupBox();
            this.pnObject = new StackPanel();
            this.cbSlotType = new ComboBoxCompat();
            this.ckbDecimal = new CheckBoxCompat2();
            this.tbVal1 = new TextBoxCompat();
            this.ckbNFailTrees = new CheckBoxCompat2();
            this.ckbIgnDstFootprint = new CheckBoxCompat2();
            this.ckbDiffAlts = new CheckBoxCompat2();            //
            // pnWiz0x002d
            //            this.pnWiz0x002d.Children.Add(this.flowLayoutPanel1);
            this.pnWiz0x002d.Name = "pnWiz0x002d";
            //
            // flowLayoutPanel1
            //            this.flowLayoutPanel1.Children.Add(this.gbRoutingSlot);
            this.flowLayoutPanel1.Children.Add(this.ckbNFailTrees);
            this.flowLayoutPanel1.Children.Add(this.ckbIgnDstFootprint);
            this.flowLayoutPanel1.Children.Add(this.ckbDiffAlts);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            //
            // gbRoutingSlot
            //            this.gbRoutingSlot.Children.Add(this.pnObject);
            this.gbRoutingSlot.Name = "gbRoutingSlot";
            // pnObject
            //            this.pnObject.Children.Add(this.cbSlotType);
            this.pnObject.Children.Add(this.ckbDecimal);
            this.pnObject.Children.Add(this.tbVal1);
            this.pnObject.Name = "pnObject";
            //
            // cbSlotType
            //
            //
            // ckbDecimal
            //            this.ckbDecimal.Name = "ckbDecimal";
            //
            // tbVal1
            //            this.tbVal1.Name = "tbVal1";
            //
            // ckbNFailTrees
            //            this.ckbNFailTrees.Name = "ckbNFailTrees";
            // ckbIgnDstFootprint
            //            this.ckbIgnDstFootprint.Name = "ckbIgnDstFootprint";
            // ckbDiffAlts
            //            this.ckbDiffAlts.Name = "ckbDiffAlts";
            // UI
            //            this.Controls.Add(this.pnWiz0x002d);

		}
		#endregion


    }

}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x002d : pjse.ABhavOperandWiz
	{
        public BhavOperandWiz0x002d(Instruction i) : base(i) { myForm = new Wiz0x002d.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
