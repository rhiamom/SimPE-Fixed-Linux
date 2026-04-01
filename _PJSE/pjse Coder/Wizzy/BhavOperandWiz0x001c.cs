/***************************************************************************
 *   Copyright (C) 2005-2008 by Peter L Jones                              *
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
 *   59 Temple Place - Suite 330, Boston, MA  1c111-1307, USA.             *
 ***************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using SimPe.PackedFiles.Wrapper;

namespace pjse.BhavOperandWizards.Wiz0x001c
{
    /// <summary>
    /// Summary description for StrBig.
    /// </summary>
    internal class UI : Window, iBhavOperandWizForm
	{
		#region Form variables

        internal StackPanel pnWiz0x001c;
        private LabelCompat label1;
        private ComboBoxCompat cbScope;
        private LabelCompat label2;
        private CheckBoxCompat2 tfPrivate;
        private CheckBoxCompat2 tfGlobal;
        private CheckBoxCompat2 tfSemiGlobal;
        private LabelCompat label3;
        private ComboBoxCompat cbRTBNType;
        private LabelCompat label4;
        private CheckBoxCompat2 tfParams;
        private CheckBoxCompat2 tfArgs;
        private LabelCompat label8;
        private TextBoxCompat tbTree;
        private LabelCompat lbTreeName;
        private ButtonCompat btnTreeName;
        private LabelledDataOwner ldocArg1;
        private LabelledDataOwner ldocArg2;
        private LabelledDataOwner ldocArg3;
        private FlowLayoutPanel flpArgs;
        private FlowLayoutPanel flowLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel4;
        private FlowLayoutPanel flowLayoutPanel3;
        private FlowLayoutPanel flowLayoutPanel6;
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

            this.cbRTBNType.Items.Clear();
            this.cbRTBNType.Items.AddRange(BhavWiz.readStr(GS.BhavStr.RTBNType).ToArray());

            ldocArg3.Decimal = ldocArg2.Decimal = ldocArg1.Decimal = pjse.Settings.PJSE.DecimalDOValue;
            ldocArg3.UseInstancePicker = ldocArg2.UseInstancePicker = ldocArg1.UseInstancePicker = pjse.Settings.PJSE.InstancePickerAsText;

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
        private bool internalchg = false;
        private DataOwnerControl doidTree = null;

        void doidTree_DataOwnerControlChanged(object sender, EventArgs e)
        {
            this.lbTreeName.Content = ((BhavWiz)inst).readStr(this.Scope, GS.GlobalStr.NamedTree, (ushort)(doidTree.Value - 1), -1, pjse.Detail.ErrorNames);
        }

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
            pjse.FileTable.Entry[] items = pjse.FileTable.GFT[(uint)SimPe.Data.MetaData.STRING_FILE, inst.Parent.GroupForScope(this.Scope), (uint)GS.GlobalStr.NamedTree];

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
                this.tbTree.Text = "0x" + SimPe.Helper.HexString((ushort)(i+1));
                this.lbTreeName.Content = ((BhavWiz)inst).readStr(this.Scope, GS.GlobalStr.NamedTree, (ushort)i, -1, pjse.Detail.ErrorNames);
            }
        }


        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x001c; } }

		public void Execute(Instruction inst)
		{
            this.inst = ldocArg1.Instruction = ldocArg2.Instruction = ldocArg3.Instruction = inst;

            wrappedByteArray ops1 = inst.Operands;
            wrappedByteArray ops2 = inst.Reserved1;

            internalchg = true;

            Boolset options = (byte)(ops1[0x02] & 0x3f);

            this.cbScope.SelectedIndex = 0; // Private
            if      (options[0]) this.cbScope.SelectedIndex = 2; // Global
            else if (options[1]) this.cbScope.SelectedIndex = 1; // SemiGlobal

            this.tfSemiGlobal.Checked = !options[3];
            this.tfGlobal.Checked     = !options[2];

            this.cbRTBNType.SelectedIndex = ops1[0x05] < this.cbRTBNType.Items.Count ? ops1[0x05] : -1;

            this.flpArgs.IsEnabled = this.tfArgs.Checked = options[4];
            this.tfParams.Checked = options[5];

            doidTree = new DataOwnerControl(null, null, null, this.tbTree, null, null, null, 0x07, BhavWiz.ToShort(ops1[0x04], (byte)((ops1[0x02] >> 6) & 0x01)));
            doidTree.DataOwnerControlChanged += new EventHandler(doidTree_DataOwnerControlChanged);
            doidTree_DataOwnerControlChanged(null, null);

            ldocArg1.Value = BhavWiz.ToShort(ops1[0x07], ops2[0x00]); ldocArg1.DataOwner = ops1[0x06];
            ldocArg2.Value = BhavWiz.ToShort(ops2[0x02], ops2[0x03]); ldocArg2.DataOwner = ops2[0x01];
            ldocArg3.Value = BhavWiz.ToShort(ops2[0x05], ops2[0x06]); ldocArg3.DataOwner = ops2[0x04];

            internalchg = false;
        }

		public Instruction Write(Instruction inst)
		{
			if (inst != null)
			{
                wrappedByteArray ops1 = inst.Operands;
                wrappedByteArray ops2 = inst.Reserved1;

                Boolset options = (Boolset)(ops1[0x02] & 0xbf);
                int scope = this.cbScope.SelectedIndex;
                options[0] = (scope == 2);
                options[1] = (scope == 1);
                options[2] = !this.tfGlobal.Checked;
                options[3] = !this.tfSemiGlobal.Checked;
                options[4] = this.tfArgs.Checked;
                options[5] = this.tfParams.Checked;
                ops1[0x02] = options;
                ops1[0x02] |= (byte)((doidTree.Value >> 2) & 0x40);

                ops1[0x04] = (byte)(doidTree.Value & 0xff);

                if (this.cbRTBNType.SelectedIndex != null)
                    ops1[0x05] = (byte)this.cbRTBNType.SelectedIndex;

                byte[] lohi = { 0, 0 };
                ops1[0x06] = ldocArg1.DataOwner; BhavWiz.FromShort(ref lohi, 0, ldocArg1.Value); ops1[0x07] = lohi[0]; ops2[0x00] = lohi[1];
                ops2[0x01] = ldocArg2.DataOwner; BhavWiz.FromShort(ref ops2, 2, ldocArg2.Value);
                ops2[0x04] = ldocArg3.DataOwner; BhavWiz.FromShort(ref ops2, 5, ldocArg3.Value);
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
		{            this.pnWiz0x001c = new StackPanel();
            this.flpArgs = new FlowLayoutPanel();
            this.ldocArg1 = new pjse.LabelledDataOwner();
            this.ldocArg2 = new pjse.LabelledDataOwner();
            this.ldocArg3 = new pjse.LabelledDataOwner();
            this.btnTreeName = new ButtonCompat();
            this.tbTree = new TextBoxCompat();
            this.tfGlobal = new CheckBoxCompat2();
            this.tfParams = new CheckBoxCompat2();
            this.tfArgs = new CheckBoxCompat2();
            this.tfSemiGlobal = new CheckBoxCompat2();
            this.tfPrivate = new CheckBoxCompat2();
            this.cbRTBNType = new ComboBoxCompat();
            this.cbScope = new ComboBoxCompat();
            this.label3 = new LabelCompat();
            this.lbTreeName = new LabelCompat();
            this.label2 = new LabelCompat();
            this.label4 = new LabelCompat();
            this.label8 = new LabelCompat();
            this.label1 = new LabelCompat();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.flowLayoutPanel2 = new FlowLayoutPanel();
            this.flowLayoutPanel3 = new FlowLayoutPanel();
            this.flowLayoutPanel4 = new FlowLayoutPanel();
            this.flowLayoutPanel6 = new FlowLayoutPanel();            // pnWiz0x001c
            //            this.pnWiz0x001c.Children.Add(this.tableLayoutPanel1);
            this.pnWiz0x001c.Name = "pnWiz0x001c";
            // flpArgs
            //            this.tableLayoutPanel1.SetColumnSpan(this.flpArgs, 2);
            this.flpArgs.Children.Add(this.ldocArg1);
            this.flpArgs.Children.Add(this.ldocArg2);
            this.flpArgs.Children.Add(this.ldocArg3);
            this.flpArgs.Name = "flpArgs";
            // ldocArg1
            //            this.ldocArg1.DataOwner = ((byte)(255));
            this.ldocArg1.DataOwnerEnabled = true;
            this.ldocArg1.DecimalVisible = false;
            this.ldocArg1.Instruction = null;
            this.ldocArg1.LabelSize = new System.Drawing.Size(61, 13);
            this.ldocArg1.Name = "ldocArg1";
            this.ldocArg1.UseFlagNames = false;
            this.ldocArg1.UseInstancePickerVisible = false;
            this.ldocArg1.Value = ((ushort)(0));
            // ldocArg2
            //            this.ldocArg2.DataOwner = ((byte)(255));
            this.ldocArg2.DataOwnerEnabled = true;
            this.ldocArg2.DecimalVisible = false;
            this.ldocArg2.Instruction = null;
            this.ldocArg2.LabelSize = new System.Drawing.Size(61, 13);
            this.ldocArg2.Name = "ldocArg2";
            this.ldocArg2.UseFlagNames = false;
            this.ldocArg2.UseInstancePickerVisible = false;
            this.ldocArg2.Value = ((ushort)(0));
            // ldocArg3
            //            this.ldocArg3.DataOwner = ((byte)(255));
            this.ldocArg3.DataOwnerEnabled = true;
            this.ldocArg3.Instruction = null;
            this.ldocArg3.LabelSize = new System.Drawing.Size(61, 13);
            this.ldocArg3.Name = "ldocArg3";
            this.ldocArg3.UseFlagNames = false;
            this.ldocArg3.Value = ((ushort)(0));
            // btnTreeName
            //            this.btnTreeName.Name = "btnTreeName";
            this.btnTreeName.Click += (s, e) => this.btnTreeName_Click(s, e);
            // tbTree
            //            this.tbTree.Name = "tbTree";
            // tfGlobal
            //            this.tfGlobal.Name = "tfGlobal";
            //            this.tfParams.Name = "tfParams";
            //            this.tfArgs.Name = "tfArgs";
            this.tfArgs.IsCheckedChanged += (s, e) => this.tfArgs_CheckedChanged(s, e);
            // tfSemiGlobal
            //            this.tfSemiGlobal.Name = "tfSemiGlobal";
            //            this.tfPrivate.Checked = true;
            this.tfPrivate.Name = "tfPrivate";
            // 
            // cbScope
            this.cbScope.Name = "cbScope";
            this.cbScope.SelectionChanged += (s, e) => this.cbScope_SelectedIndexChanged(s, e);
            // label3
            //            this.label3.Name = "label3";
            // lbTreeName
            //            this.lbTreeName.Name = "lbTreeName";
            // label2
            //            this.label2.Name = "label2";
            // label4
            //            this.label4.Name = "label4";
            // label8
            //            this.label8.Name = "label8";
            // label1
            //            this.label1.Name = "label1";
            // tableLayoutPanel1
            //            this.tableLayoutPanel1.Children.Add(this.flpArgs);
            this.tableLayoutPanel1.Children.Add(this.label1);
            this.tableLayoutPanel1.Children.Add(this.flowLayoutPanel2);
            this.tableLayoutPanel1.Children.Add(this.label2);
            this.tableLayoutPanel1.Children.Add(this.cbRTBNType);
            this.tableLayoutPanel1.Children.Add(this.flowLayoutPanel4);
            this.tableLayoutPanel1.Children.Add(this.label4);
            this.tableLayoutPanel1.Children.Add(this.flowLayoutPanel3);
            this.tableLayoutPanel1.Children.Add(this.flowLayoutPanel6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // flowLayoutPanel2
            //            this.flowLayoutPanel2.Children.Add(this.cbScope);
            this.flowLayoutPanel2.Children.Add(this.label8);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            // flowLayoutPanel3
            //            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel3, 2);
            this.flowLayoutPanel3.Children.Add(this.label3);
            this.flowLayoutPanel3.Children.Add(this.tfPrivate);
            this.flowLayoutPanel3.Children.Add(this.tfSemiGlobal);
            this.flowLayoutPanel3.Children.Add(this.tfGlobal);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            // flowLayoutPanel4
            //            this.flowLayoutPanel4.Children.Add(this.tbTree);
            this.flowLayoutPanel4.Children.Add(this.btnTreeName);
            this.flowLayoutPanel4.Children.Add(this.lbTreeName);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            // flowLayoutPanel6
            //            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel6, 2);
            this.flowLayoutPanel6.Children.Add(this.tfArgs);
            this.flowLayoutPanel6.Children.Add(this.tfParams);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            // UI
            //            this.Controls.Add(this.pnWiz0x001c);

		}
		#endregion

        private void btnTreeName_Click(object sender, EventArgs e)
        {
            doStrChooser();
        }

        private void tfArgs_CheckedChanged(object sender, EventArgs e)
        {
            this.flpArgs.IsEnabled = this.tfArgs.Checked;
        }

        private void cbScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (internalchg) return;
            this.lbTreeName.Content = ((BhavWiz)inst).readStr(this.Scope, GS.GlobalStr.NamedTree, (ushort)(doidTree.Value - 1), -1, pjse.Detail.ErrorNames);
        }

	}

}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x001c : pjse.ABhavOperandWiz
	{
		public BhavOperandWiz0x001c(Instruction i) : base(i) { myForm = new Wiz0x001c.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
