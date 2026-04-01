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

namespace pjse.BhavOperandWizards.Wiz0x001b
{
    /// <summary>
    /// Summary description for StrBig.
    /// </summary>
    internal class UI : Window, iBhavOperandWizForm
	{
		#region Form variables

        internal StackPanel pnWiz0x001b;
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox gbLocation;
        private ComboBoxCompat cbLocation;
        private GroupBox gbDirection;
        private ComboBoxCompat cbDirection;
        private CheckBoxCompat2 ckbNoFailureTrees;
        private CheckBoxCompat2 ckbDifferentAltitudes;
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

            cbLocation.Items.AddRange(BhavWiz.readStr(GS.BhavStr.RelativeLocations).ToArray());
            cbDirection.Items.AddRange(BhavWiz.readStr(GS.BhavStr.RelativeDirections).ToArray());
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
        //private bool internalchg = false;

        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x001b; } }

        public void Execute(Instruction inst)
        {
            this.inst = inst;

            wrappedByteArray ops1 = inst.Operands;
            wrappedByteArray ops2 = inst.Reserved1;
            Boolset ops16 = ops1[6];

            //internalchg = true;

            cbLocation.SelectedIndex = ((byte)(ops1[2] + 2) < cbLocation.Items.Count) ? (byte)(ops1[2] + 2) : -1;
            cbDirection.SelectedIndex = ((byte)(ops1[3] + 2) < cbDirection.Items.Count) ? (byte)(ops1[3] + 2) : -1;

            ckbNoFailureTrees.IsChecked = ops16[1];
            ckbDifferentAltitudes.IsChecked = ops16[2];

            //internalchg = false;
        }

		public Instruction Write(Instruction inst)
		{
			if (inst != null)
			{
                wrappedByteArray ops1 = inst.Operands;
                wrappedByteArray ops2 = inst.Reserved1;
                Boolset ops16 = ops1[6];

                if (cbLocation.SelectedIndex != null) ops1[2] = ((byte)(cbLocation.SelectedIndex - 2));
                if (cbDirection.SelectedIndex != null) ops1[3] = ((byte)(cbDirection.SelectedIndex - 2));

                ops16[1] = ckbNoFailureTrees.IsChecked == true;
                ops16[2] = ckbDifferentAltitudes.IsChecked == true;
                ops1[6] = ops16;

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
		{            this.pnWiz0x001b = new StackPanel();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.gbLocation = new SimPe.Scenegraph.Compat.GroupBox();
            this.cbLocation = new ComboBoxCompat();
            this.gbDirection = new SimPe.Scenegraph.Compat.GroupBox();
            this.cbDirection = new ComboBoxCompat();
            this.ckbNoFailureTrees = new CheckBoxCompat2();
            this.ckbDifferentAltitudes = new CheckBoxCompat2();            //
            // pnWiz0x001b
            //            this.pnWiz0x001b.Children.Add(this.flowLayoutPanel1);
            this.pnWiz0x001b.Name = "pnWiz0x001b";
            //
            // flowLayoutPanel1
            //            this.flowLayoutPanel1.Children.Add(this.gbLocation);
            this.flowLayoutPanel1.Children.Add(this.gbDirection);
            this.flowLayoutPanel1.Children.Add(this.ckbNoFailureTrees);
            this.flowLayoutPanel1.Children.Add(this.ckbDifferentAltitudes);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            //
            // gbLocation
            //            this.gbLocation.Children.Add(this.cbLocation);
            this.gbLocation.Name = "gbLocation";
            // cbLocation
            //
            //
            // gbDirection
            //            this.gbDirection.Children.Add(this.cbDirection);
            this.gbDirection.Name = "gbDirection";
            // cbDirection
            //
            //
            // ckbNoFailureTrees
            //            this.ckbNoFailureTrees.Name = "ckbNoFailureTrees";
            // ckbDifferentAltitudes
            //            this.ckbDifferentAltitudes.Name = "ckbDifferentAltitudes";
            // UI
            //            this.Controls.Add(this.pnWiz0x001b);

		}
		#endregion


    }

}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x001b : pjse.ABhavOperandWiz
	{
        public BhavOperandWiz0x001b(Instruction i) : base(i) { myForm = new Wiz0x001b.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
