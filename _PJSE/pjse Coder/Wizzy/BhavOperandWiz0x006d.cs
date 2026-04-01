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

namespace pjse.BhavOperandWizards.Wiz0x006d
{
    /// <summary>
    /// Summary description for StrBig.
    /// </summary>
    internal class UI : Window, iBhavOperandWizForm
    {
        #region Form variables

        internal StackPanel pnWiz0x006d;
        private LabelCompat label1;
        private Panel pnMaterial;
        private LabelCompat label3;
        private ComboBoxCompat cbPicker1;
        private CheckBoxCompat2 cbDecimal;
        private TextBoxCompat tbVal1;
        private CheckBoxCompat2 cbAttrPicker;
        private ComboBoxCompat cbDataOwner1;
        private RadioButton rb1Object;
        private RadioButton rb1Me;
        private RadioButton rb1ScrShot;
        private Panel pnNotScrShot;
        private CheckBoxCompat2 ckbMaterialTemp;
        private RadioButton rb2MovingTexture;
        private RadioButton rb2Material;
        private LabelCompat label5;
        private TextBoxCompat tbVal3;
        private ComboBoxCompat cbMatScope;
        private LabelCompat label6;
        private ButtonCompat btnMaterial;
        private TextBoxCompat tbMaterial;
        private Panel panel1;
        private LabelCompat label2;
        private RadioButton rb3Object;
        private RadioButton rb3Me;
        private Panel pnNotAllOver;
        private CheckBoxCompat2 ckbAllOver;
        private ComboBoxCompat cbMeshScope;
        private LabelCompat label4;
        private LabelCompat label7;
        private TextBoxCompat tbMesh;
        private ButtonCompat btnMesh;
        private TextBoxCompat tbVal5;
        private LabelCompat label8;
        private CheckBoxCompat2 ckbMeshTemp;
        private LabelCompat label9;
        private ComboBoxCompat cbPicker2;
        private TextBoxCompat tbVal2;
        private ComboBoxCompat cbDataOwner2;
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

            rb1group = new ArrayList(new Control[] { this.rb1ScrShot, this.rb1Me, this.rb1Object });
            rb3group = new ArrayList(new Control[] { this.rb3Me, this.rb3Object });
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
        private DataOwnerControl doid3 = null;
        private DataOwnerControl doid5 = null;

        ArrayList rb1group = null;
        ArrayList rb3group = null;
        private bool internalchg = false;

        void doid3_DataOwnerControlChanged(object sender, EventArgs e)
        {
            doStrValue(cbMatScope, GS.GlobalStr.MaterialName, doid3.Value, tbMaterial);
        }

        void doid5_DataOwnerControlChanged(object sender, EventArgs e)
        {
            doStrValue(cbMeshScope, GS.GlobalStr.MeshGroup, doid5.Value, tbMesh);
        }

        private void doStrChooser(ComboBoxCompat scope, pjse.GS.GlobalStr instance, TextBoxCompat tbVal, TextBoxCompat strText)
        {
            Scope[] s = { Scope.Private, Scope.SemiGlobal, Scope.Global };
            pjse.FileTable.Entry[] items = ((scope.SelectedIndex) < 0) ? null :
                pjse.FileTable.GFT[(uint)SimPe.Data.MetaData.STRING_FILE, inst.Parent.GroupForScope(s[scope.SelectedIndex]), (uint)instance];

            if (items == null || items.Length == 0)
            {
                MessageBox.Show(pjse.Localization.GetString("bow_noStrings")
                    + " (" + pjse.Localization.GetString(s[scope.SelectedIndex].ToString()) + ")");
                return; // eek!
            }

            SimPe.PackedFiles.Wrapper.StrWrapper str = new StrWrapper();
            str.ProcessData(items[0].PFD, items[0].Package);

            int i = (new StrChooser(true)).Strnum(str);
            if (i >= 0)
            {
                bool savedState = internalchg;
                internalchg = true;
                tbVal.Text = "0x" + SimPe.Helper.HexString((ushort)i);
                doStrValue(scope, instance, (ushort)i, strText);
                internalchg = savedState;
            }
        }

        private void doStrValue(ComboBoxCompat scope, pjse.GS.GlobalStr instance, ushort strno, TextBoxCompat strText)
        {
            Scope[] s = { Scope.Private, Scope.Global, Scope.SemiGlobal };
            strText.Text = ((scope.SelectedIndex) < 0) ? "" :
                ((BhavWiz)inst).readStr(s[scope.SelectedIndex], instance, strno, -1, pjse.Detail.ErrorNames);
        }

        private void MaterialFrom()
        {
            this.pnNotScrShot.IsEnabled = !this.rb1ScrShot.IsChecked == true;
            this.tbVal3.IsEnabled = !this.ckbMaterialTemp.IsChecked == true;
            this.btnMaterial.IsEnabled = this.tbMaterial.IsVisible = this.rb1Me.IsChecked == true && !this.ckbMaterialTemp.IsChecked == true;
        }

        private void MeshFrom()
        {
            this.pnNotAllOver.IsEnabled = !this.ckbAllOver.IsChecked == true;
            this.tbVal5.IsEnabled = !this.ckbMeshTemp.IsChecked == true;
            this.btnMesh.IsEnabled = this.tbMesh.IsVisible = !this.ckbAllOver.IsChecked == true && this.rb3Me.IsChecked == true && !this.ckbMeshTemp.IsChecked == true;
        }

        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x006d; } }

        public void Execute(Instruction inst)
        {
            this.inst = inst;

            wrappedByteArray ops1 = inst.Operands;
            wrappedByteArray ops2 = inst.Reserved1;

            internalchg = true;

            doid3 = new DataOwnerControl(inst, null, null, this.tbVal3, this.cbDecimal, this.cbAttrPicker, null,
                0x07, BhavWiz.ToShort(ops1[0x00], ops1[0x01]));

            this.rb3Object.IsChecked = ((ops1[0x02] & 0x01) != 0);
            this.rb3Me.IsChecked = !this.rb3Object.IsChecked == true;
            this.btnMesh.IsVisible = this.rb3Me.IsChecked == true;
            this.tbMesh.IsVisible = this.rb3Me.IsChecked == true;

            this.cbMatScope.SelectedIndex = -1;
            switch (ops1[0x02] & 0x06)
            {
                case 0x00: this.cbMatScope.SelectedIndex = 0; break; // Private
                case 0x02: this.cbMatScope.SelectedIndex = 2; break; // Global
                case 0x04: this.cbMatScope.SelectedIndex = 1; break; // SemiGlobal
            }

            this.rb1ScrShot.IsChecked = ((ops2[0x05] & 0x02) != 0);
            this.rb1Me.IsChecked = !this.rb1ScrShot.IsChecked == true && ((ops1[0x02] & 0x08) == 0);
            this.rb1Object.IsChecked = !this.rb1ScrShot.IsChecked == true && !this.rb1Me.IsChecked == true;

            this.rb2MovingTexture.IsChecked = ((ops2[0x05] & 0x01) != 0);
            this.rb2Material.IsChecked = !this.rb2MovingTexture.IsChecked == true;

            this.ckbMaterialTemp.IsChecked = ((ops1[0x02] & 0x10) != 0);
            this.ckbMeshTemp.IsChecked     = ((ops1[0x02] & 0x20) != 0);

            this.cbMeshScope.SelectedIndex = -1;
            switch (ops1[0x02] & 0xc0)
            {
                case 0x00: this.cbMeshScope.SelectedIndex = 0; break; // Private
                case 0x40: this.cbMeshScope.SelectedIndex = 2; break; // Global
                case 0x80: this.cbMeshScope.SelectedIndex = 1; break; // SemiGlobal
            }

            doid5 = new DataOwnerControl(inst, null, null, this.tbVal5, this.cbDecimal, this.cbAttrPicker, null,
                0x07, (ushort)(BhavWiz.ToShort(ops1[0x03], ops1[0x04]) & 0x7fff));
            this.ckbAllOver.IsChecked = (ops1[0x04] & 0x80) != 0;

            doid1 = new DataOwnerControl(inst, this.cbDataOwner1, this.cbPicker1, this.tbVal1, this.cbDecimal, this.cbAttrPicker, null,
                ops1[0x05], BhavWiz.ToShort(ops1[0x06], ops1[0x07]));
            doid2 = new DataOwnerControl(inst, this.cbDataOwner2, this.cbPicker2, this.tbVal2, this.cbDecimal, this.cbAttrPicker, null,
                ops2[0x00], BhavWiz.ToShort(ops2[0x01], ops2[0x02]));

            doid3.DataOwnerControlChanged += new EventHandler(doid3_DataOwnerControlChanged);
            doid3_DataOwnerControlChanged(null, null);
            doid5.DataOwnerControlChanged += new EventHandler(doid5_DataOwnerControlChanged);
            doid5_DataOwnerControlChanged(null, null);

            internalchg = false;

            this.MaterialFrom();
            this.MeshFrom();
        }

        public Instruction Write(Instruction inst)
        {
            if (inst != null)
            {
                wrappedByteArray ops1 = inst.Operands;
                wrappedByteArray ops2 = inst.Reserved1;

                BhavWiz.FromShort(ref ops1, 0, doid3.Value);

                ops1[0x02] = 0x00;
                ops1[0x02] |= (byte)(this.rb3Object.IsChecked == true ? 0x01 : 0x00);
                switch (this.cbMatScope.SelectedIndex)
                {
                    case 2: ops1[0x02] |= 0x02; break; // Global
                    case 1: ops1[0x02] |= 0x04; break; // SemiGlobal
                }
                ops1[0x02] |= (byte)(this.rb1Object.IsChecked == true ? 0x08 : 0x00);
                ops1[0x02] |= (byte)(this.ckbMaterialTemp.IsChecked == true ? 0x10 : 0x00);
                ops1[0x02] |= (byte)(this.ckbMeshTemp.IsChecked == true ? 0x20 : 0x00);
                switch (this.cbMeshScope.SelectedIndex)
                {
                    case 2: ops1[0x02] |= 0x40; break; // Global
                    case 1: ops1[0x02] |= 0x80; break; // SemiGlobal
                }

                BhavWiz.FromShort(ref ops1, 3, (ushort)(doid5.Value & 0x7fff));
                ops1[0x04] |= (byte)(this.ckbAllOver.IsChecked == true ? 0x80 : 0x00);

                ops1[0x05] = doid1.DataOwner;
                BhavWiz.FromShort(ref ops1, 6, doid1.Value);

                ops2[0x00] = doid2.DataOwner;
                BhavWiz.FromShort(ref ops2, 1, doid2.Value);

                ops2[0x05] &= 0xfc;
                ops2[0x05] |= (byte)(this.rb2MovingTexture.IsChecked == true ? 0x01 : 0x00);
                ops2[0x05] |= (byte)(this.rb1ScrShot.IsChecked == true ? 0x02 : 0x00);
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
        {            this.pnWiz0x006d = new StackPanel();
            this.cbPicker2 = new ComboBoxCompat();
            this.cbAttrPicker = new CheckBoxCompat2();
            this.cbDecimal = new CheckBoxCompat2();
            this.tbVal2 = new TextBoxCompat();
            this.cbDataOwner2 = new ComboBoxCompat();
            this.panel1 = new StackPanel();
            this.pnNotAllOver = new StackPanel();
            this.tbMesh = new TextBoxCompat();
            this.btnMesh = new ButtonCompat();
            this.tbVal5 = new TextBoxCompat();
            this.label8 = new LabelCompat();
            this.ckbMeshTemp = new CheckBoxCompat2();
            this.cbMeshScope = new ComboBoxCompat();
            this.label4 = new LabelCompat();
            this.ckbAllOver = new CheckBoxCompat2();
            this.rb3Object = new Avalonia.Controls.RadioButton();
            this.rb3Me = new Avalonia.Controls.RadioButton();
            this.label2 = new LabelCompat();
            this.cbPicker1 = new ComboBoxCompat();
            this.tbVal1 = new TextBoxCompat();
            this.cbDataOwner1 = new ComboBoxCompat();
            this.pnMaterial = new StackPanel();
            this.pnNotScrShot = new StackPanel();
            this.tbMaterial = new TextBoxCompat();
            this.btnMaterial = new ButtonCompat();
            this.tbVal3 = new TextBoxCompat();
            this.cbMatScope = new ComboBoxCompat();
            this.label7 = new LabelCompat();
            this.label6 = new LabelCompat();
            this.label5 = new LabelCompat();
            this.ckbMaterialTemp = new CheckBoxCompat2();
            this.rb2MovingTexture = new Avalonia.Controls.RadioButton();
            this.rb2Material = new Avalonia.Controls.RadioButton();
            this.rb1Object = new Avalonia.Controls.RadioButton();
            this.rb1Me = new Avalonia.Controls.RadioButton();
            this.rb1ScrShot = new Avalonia.Controls.RadioButton();
            this.label3 = new LabelCompat();
            this.label9 = new LabelCompat();
            this.label1 = new LabelCompat();            //
            // pnWiz0x006d
            //
            this.pnWiz0x006d.Children.Add(this.cbPicker2);
            this.pnWiz0x006d.Children.Add(this.cbAttrPicker);
            this.pnWiz0x006d.Children.Add(this.cbDecimal);
            this.pnWiz0x006d.Children.Add(this.tbVal2);
            this.pnWiz0x006d.Children.Add(this.cbDataOwner2);
            this.pnWiz0x006d.Children.Add(this.panel1);
            this.pnWiz0x006d.Children.Add(this.cbPicker1);
            this.pnWiz0x006d.Children.Add(this.tbVal1);
            this.pnWiz0x006d.Children.Add(this.cbDataOwner1);
            this.pnWiz0x006d.Children.Add(this.pnMaterial);
            this.pnWiz0x006d.Children.Add(this.label9);
            this.pnWiz0x006d.Children.Add(this.label1);            this.pnWiz0x006d.Name = "pnWiz0x006d";
            //
            // cbPicker2
            //
            this.cbPicker2.Name = "cbPicker2";
            //
            // cbAttrPicker
            //            this.cbAttrPicker.Name = "cbAttrPicker";
            //
            // cbDecimal
            //            this.cbDecimal.Name = "cbDecimal";
            //
            // tbVal2
            //            this.tbVal2.Name = "tbVal2";
            //
            // cbDataOwner2
            //
            this.cbDataOwner2.Name = "cbDataOwner2";
            //
            // panel1
            //            this.panel1.Children.Add(this.pnNotAllOver);
            this.panel1.Children.Add(this.ckbAllOver);
            this.panel1.Children.Add(this.rb3Object);
            this.panel1.Children.Add(this.rb3Me);
            this.panel1.Children.Add(this.label2);
            this.panel1.Name = "panel1";
            //
            // pnNotAllOver
            //            this.pnNotAllOver.Children.Add(this.tbMesh);
            this.pnNotAllOver.Children.Add(this.btnMesh);
            this.pnNotAllOver.Children.Add(this.tbVal5);
            this.pnNotAllOver.Children.Add(this.label8);
            this.pnNotAllOver.Children.Add(this.ckbMeshTemp);
            this.pnNotAllOver.Children.Add(this.cbMeshScope);
            this.pnNotAllOver.Children.Add(this.label4);
            this.pnNotAllOver.Name = "pnNotAllOver";
            //
            // tbMesh
            //            this.tbMesh.Name = "tbMesh";
            this.tbMesh.IsReadOnly = true;
            // btnMesh
            //            this.btnMesh.Name = "btnMesh";
            this.btnMesh.Click += (s, e) => this.btnMesh_Click(s, e);
            //
            // tbVal5
            //            this.tbVal5.Name = "tbVal5";
            //
            // label8
            //            this.label8.Name = "label8";
            //
            // ckbMeshTemp
            //            this.ckbMeshTemp.Name = "ckbMeshTemp";
            this.ckbMeshTemp.IsCheckedChanged += (s, e) => this.ckbMeshTemp_CheckedChanged(s, e);
            //
            // cbMeshScope
            //
            this.cbMeshScope.SelectionChanged += (s, e) => this.cbMatMeshScope_SelectedIndexChanged(s, e);
            //
            // label4
            //            this.label4.Name = "label4";
            //
            // ckbAllOver
            //            this.ckbAllOver.Name = "ckbAllOver";
            this.ckbAllOver.IsCheckedChanged += (s, e) => this.ckbAllOver_CheckedChanged(s, e);
            //
            // rb3Object
            //            this.rb3Object.Name = "rb3Object";
            this.rb3Object.IsCheckedChanged += (s, e) => this.rb3group_CheckedChanged(s, e);
            //
            // rb3Me
            //            this.rb3Me.Name = "rb3Me";
            this.rb3Me.IsCheckedChanged += (s, e) => this.rb3group_CheckedChanged(s, e);
            //
            // label2
            //            this.label2.Name = "label2";
            //
            // cbPicker1
            //
            this.cbPicker1.Name = "cbPicker1";
            //
            // tbVal1
            //            this.tbVal1.Name = "tbVal1";
            //
            // cbDataOwner1
            //
            this.cbDataOwner1.Name = "cbDataOwner1";
            //
            // pnMaterial
            //            this.pnMaterial.Children.Add(this.pnNotScrShot);
            this.pnMaterial.Children.Add(this.rb1Object);
            this.pnMaterial.Children.Add(this.rb1Me);
            this.pnMaterial.Children.Add(this.rb1ScrShot);
            this.pnMaterial.Children.Add(this.label3);
            this.pnMaterial.Name = "pnMaterial";
            //
            // pnNotScrShot
            //            this.pnNotScrShot.Children.Add(this.tbMaterial);
            this.pnNotScrShot.Children.Add(this.btnMaterial);
            this.pnNotScrShot.Children.Add(this.tbVal3);
            this.pnNotScrShot.Children.Add(this.cbMatScope);
            this.pnNotScrShot.Children.Add(this.label7);
            this.pnNotScrShot.Children.Add(this.label6);
            this.pnNotScrShot.Children.Add(this.label5);
            this.pnNotScrShot.Children.Add(this.ckbMaterialTemp);
            this.pnNotScrShot.Children.Add(this.rb2MovingTexture);
            this.pnNotScrShot.Children.Add(this.rb2Material);
            this.pnNotScrShot.Name = "pnNotScrShot";
            //
            // tbMaterial
            //            this.tbMaterial.Name = "tbMaterial";
            this.tbMaterial.IsReadOnly = true;
            // btnMaterial
            //            this.btnMaterial.Name = "btnMaterial";
            this.btnMaterial.Click += (s, e) => this.btnMaterial_Click(s, e);
            //
            // tbVal3
            //            this.tbVal3.Name = "tbVal3";
            //
            // cbMatScope
            //
            this.cbMatScope.SelectionChanged += (s, e) => this.cbMatMeshScope_SelectedIndexChanged(s, e);
            //
            // label7
            //            this.label7.Name = "label7";
            //
            // label6
            //            this.label6.Name = "label6";
            //
            // label5
            //            this.label5.Name = "label5";
            //
            // ckbMaterialTemp
            //            this.ckbMaterialTemp.Name = "ckbMaterialTemp";
            this.ckbMaterialTemp.IsCheckedChanged += (s, e) => this.ckbMaterialTemp_CheckedChanged(s, e);
            //
            // rb2MovingTexture
            //            this.rb2MovingTexture.Name = "rb2MovingTexture";
            // rb2Material
            //            this.rb2Material.Name = "rb2Material";
            // rb1Object
            //            this.rb1Object.Name = "rb1Object";
            this.rb1Object.IsCheckedChanged += (s, e) => this.rb1group_CheckedChanged(s, e);
            //
            // rb1Me
            //            this.rb1Me.Name = "rb1Me";
            this.rb1Me.IsCheckedChanged += (s, e) => this.rb1group_CheckedChanged(s, e);
            //
            // rb1ScrShot
            //            this.rb1ScrShot.Name = "rb1ScrShot";
            this.rb1ScrShot.IsCheckedChanged += (s, e) => this.rb1group_CheckedChanged(s, e);
            //
            // label3
            //            this.label3.Name = "label3";
            //
            // label9
            //            this.label9.Name = "label9";
            //
            // label1
            //            this.label1.Name = "label1";
            //
            // UI
            //            this.Controls.Add(this.pnWiz0x006d);
        }
        #endregion

        private void rb1group_CheckedChanged(object sender, EventArgs e)
        {
            if (internalchg) return;
            this.MaterialFrom();
        }

        private void ckbMaterialTemp_CheckedChanged(object sender, EventArgs e)
        {
            if (internalchg) return;
            this.MaterialFrom();
        }

        private void btnMaterial_Click(object sender, EventArgs e)
        {
            this.doStrChooser(this.cbMatScope, GS.GlobalStr.MaterialName, this.tbVal3, this.tbMaterial);
        }

        private void rb3group_CheckedChanged(object sender, EventArgs e)
        {
            if (internalchg) return;
            this.MeshFrom();
        }

        private void ckbAllOver_CheckedChanged(object sender, EventArgs e)
        {
            if (internalchg) return;
            this.MeshFrom();
        }

        private void ckbMeshTemp_CheckedChanged(object sender, EventArgs e)
        {
            if (internalchg) return;
            this.MeshFrom();
        }

        private void btnMesh_Click(object sender, EventArgs e)
        {
            this.doStrChooser(this.cbMeshScope, GS.GlobalStr.MeshGroup, this.tbVal5, this.tbMesh);
        }

        private void cbMatMeshScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (internalchg) return;
            if (sender.Equals(this.cbMatScope))
                doStrValue(cbMatScope, GS.GlobalStr.MaterialName, doid3.Value, tbMaterial);
            else
                doStrValue(cbMeshScope, GS.GlobalStr.MeshGroup, doid5.Value, tbMesh);
        }

    }

}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x006d : pjse.ABhavOperandWiz
	{
        public BhavOperandWiz0x006d(Instruction i) : base(i) { myForm = new Wiz0x006d.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
