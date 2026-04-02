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
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using SimPe.PackedFiles.Wrapper;
using pjse.BhavNameWizards;

namespace pjse.BhavOperandWizards.Wiz0x0001
{
	/// <summary>
	/// Summary description for BhavInstruction.
	/// </summary>
	internal class UI : Window, iBhavOperandWizForm
	{
		#region Form variables

		internal StackPanel pnWiz0x0001;
		private ComboBoxCompat cbGenericSimsCall;
		private LabelCompat lbGenericSimsCallparms;
		/// <summary>
		/// Required designer variable.
		/// </summary>
				#endregion

		private string genericSimsCallparamText(int i)
		{
            return BhavWiz.readStr(GS.BhavStr.GenericsDesc, (ushort)i);
		}


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
		public void Dispose() { }


        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x0001; } }

        public void Execute(Instruction inst)
		{
			byte operand0 = inst.Operands[0];

			this.cbGenericSimsCall.Items.Clear();
			for (byte i = 0; i < BhavWiz.readStr(GS.BhavStr.Generics).Count; i++)
				this.cbGenericSimsCall.Items.Add("0x" + SimPe.Helper.HexString(i) + ": " + BhavWiz.readStr(GS.BhavStr.Generics, i));
			this.lbGenericSimsCallparms.Content = "Should never see this";

			lbGenericSimsCallparms.Content = genericSimsCallparamText(operand0);
			cbGenericSimsCall.SelectedIndex = (operand0 < cbGenericSimsCall.Items.Count) ? operand0 : -1;
		}

		public Instruction Write(Instruction inst)
		{
			if (this.cbGenericSimsCall.SelectedIndex >= 0)
				inst.Operands[0] = (byte)this.cbGenericSimsCall.SelectedIndex;
			return inst;
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{            this.pnWiz0x0001 = new StackPanel();
            this.lbGenericSimsCallparms = new LabelCompat();
            this.cbGenericSimsCall = new ComboBoxCompat();            // pnWiz0x0001
            // 
            this.pnWiz0x0001.Children.Add(this.lbGenericSimsCallparms);
            this.pnWiz0x0001.Children.Add(this.cbGenericSimsCall);            this.pnWiz0x0001.Name = "pnWiz0x0001";
            // lbGenericSimsCallparms
            //            this.lbGenericSimsCallparms.Name = "lbGenericSimsCallparms";
            // cbGenericSimsCall
            // 
            this.cbGenericSimsCall.Name = "cbGenericSimsCall";
            this.cbGenericSimsCall.SelectionChanged += (s, e) => this.cbGenericSimsCall_Changed(s, e);
            // UI
            //            this.Controls.Add(this.pnWiz0x0001);
            this.Name = "UI";
		}
		#endregion

		private void cbGenericSimsCall_Changed(object sender, System.EventArgs e)
		{
			lbGenericSimsCallparms.Content = (cbGenericSimsCall.SelectedIndex >= 0)
				? genericSimsCallparamText(cbGenericSimsCall.SelectedIndex)
				: "";
		}

    }

}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x0001 : pjse.ABhavOperandWiz
	{
        public BhavOperandWiz0x0001(Instruction i) : base(i) { myForm = new Wiz0x0001.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
