/***************************************************************************
 *   Copyright (C) 2007 by Peter L Jones                                   *
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
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using SimPe.PackedFiles.Wrapper;

namespace pjse.BhavOperandWizards.Wiz0x0033
{
    internal class UI : Window, iBhavOperandWizForm
    {
        #region Form variables

        internal StackPanel pnWiz0x0033;
        private TableLayoutPanel tlpnGetSetValue;
        private Panel pnDoid1;
        private ComboBoxCompat cbPicker1;
        private TextBoxCompat tbVal1;
        private ComboBoxCompat cbDataOwner1;
        private LabelCompat lbDoid2;
        private LabelCompat lbDoid1;
        private LabelCompat lbDoid3;
        private Panel pnDoid2;
        private ComboBoxCompat cbPicker2;
        private TextBoxCompat tbVal2;
        private ComboBoxCompat cbDataOwner2;
        private Panel pnDoid3;
        private ComboBoxCompat cbPicker3;
        private TextBoxCompat tbVal3;
        private ComboBoxCompat cbDataOwner3;
        private LabelCompat lbGUID;
        private ComboBoxCompat cbInventory;
        private LabelCompat lbInventory;
        private FlowLayoutPanel flpnGUID;
        private TextBoxCompat tbGUID;
        private TextBoxCompat tbObjName;
        private GroupBox gbTokenTypes;
        private TableLayoutPanel tableLayoutPanel1;
        private CheckBoxCompat2 ckbTTInvShopping;
        private CheckBoxCompat2 ckbTTShopping;
        private CheckBoxCompat2 ckbTTInvMemory;
        private CheckBoxCompat2 ckbTTMemory;
        private CheckBoxCompat2 ckbTTInvVisible;
        private CheckBoxCompat2 ckbTTVisible;
        private GroupBox gbInventoryType;
        private FlowLayoutPanel flpnInventoryType;
        private RadioButton rb1Counted;
        private RadioButton rb1Singular;
        private FlowLayoutPanel flpnDoid0;
        private LabelCompat lbDoid0;
        private Panel pnDoid0;
        private ComboBoxCompat cbPicker0;
        private TextBoxCompat tbVal0;
        private ComboBoxCompat cbDataOwner0;
        private LabelCompat lbOperation;
        private FlowLayoutPanel flpnOperation;
        private ComboBoxCompat cbOperation;
        private CheckBoxCompat2 ckbReversed;
        private ComboBoxCompat cbTargetInv;
        private CheckBoxCompat2 ckbTTAll;
        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBoxCompat2 ckbDecimal;
        private CheckBoxCompat2 ckbAttrPicker;
        /// <summary>
        /// Required designer variable.
        /// </summary>
                #endregion


        /// <summary>
        /// Initialise the Wizard user interface
        /// </summary>
        /// <param name="mode">Specify whether the wizard is for Animate Object, Sim or Overlay</param>
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
            if (disposing)
            {
            }


            inst = null;
        }

        #region static data
        static List<String> aInventoryType = BhavWiz.readStr(GS.BhavStr.InventoryType);
        static List<String> aTokenOpsCounted = BhavWiz.readStr(GS.BhavStr.TokenOpsCounted);
        static List<String> aTokenOpsSingular = BhavWiz.readStr(GS.BhavStr.TokenOpsSingular);
        static String[] names = { "", "Object", "bwp33_index", "bwp33_property", "bwp33_count", "Value" };
        static int[][] aNamesCounted = {
            new int[] { 0, 0, 0, 0,  0, 0, 0, 0,  0, 0, 0, 0, }, // Doid0
            new int[] { 0, 0, 0, 0,  0, 0, 0, 0,  0, 0, 0, 0, }, // Doid1
            new int[] { 0, 2, 0, 2,  0, 2, 2, 0,  2, 2, 0, 2, }, // Doid2
            new int[] { 4, 4, 4, 4,  0, 0, 4, 0,  0, 0, 4, 0, }, // Doid3 was new int[] { 4, 4, 4, 4,  0, 0, 0, 0,  0, 0, 4, 0, },
        };
        static int[][] aNamesSingular = {
            new int[] { 0, 0, 0, 0,  0, 0, 0, 0,  0, 0, 0, 0,  0, 0, 0, 0,  0, 0, 0, 0, }, // Doid0
            new int[] { 0, 0, 0, 0,  0, 0, 0, 0,  0, 0, 0, 0,  0, 0, 2, 2,  0, 0, 0, 0, }, // Doid1
            new int[] { 0, 2, 0, 2,  2, 2, 2, 3,  3, 0, 0, 0,  2, 0, 3, 3,  0, 2, 2, 0, }, // Doid2
            new int[] { 0, 0, 0, 0,  5, 5, 0, 5,  5, 0, 4, 0,  0, 4, 5, 5,  0, 0, 0, 0, }, // Doid3
        };
        static bool[] aByGUIDCounted =
            new bool[] { true , false, true , false,  true , false, true , true ,  false, false, true , false, };
        static bool[] aByGUIDSingular =
            new bool[] { true , false, true , true ,  false, false, false, false,  false, false, false, false,  false, false, false, false,  false, false, false, false, };
        static bool[] aCategoryCounted =
            new bool[] { true , false, true , false,  true , false, true , true ,  false, true , true , true , };
        static bool[] aCategorySingular =
            new bool[] { true , false, true , true ,  false, false, false, false,  false, false, false, true ,  true , true , false, false,  true , false, true , true , };
        #endregion

        private bool internalchg = false;

        private Instruction inst = null;

        private DataOwnerControl doid0 = null; // o[1], o[2], o[3]
        private DataOwnerControl doid1 = null; // o[6], o[7], o[8]
        private DataOwnerControl doid2 = null; // o[10], o[11], o[12]
        private DataOwnerControl doid3 = null; // o[13], o[14], o[15]
        private byte operation = 0;
        private byte[] o5678 = new byte[4];

        private bool hex32_IsValid(object sender)
        {
            try { Convert.ToUInt32(((TextBoxCompat)sender).Text, 16); }
            catch (Exception) { return false; }
            return true;
        }

        private void setGUID(byte[] o, int sub) { setGUID(true, (UInt32)(o[sub] | o[sub + 1] << 8 | o[sub + 2] << 16 | o[sub + 3] << 24)); }
        private void setGUID(bool setTB, UInt32 guid)
        {
            if (setTB) this.tbGUID.Text = "0x" + SimPe.Helper.HexString(guid);
            this.tbObjName.Text = (guid == 0) ? BhavWiz.dnStkOb() : BhavWiz.FormatGUID(true, guid);
        }

        private void doTokenOps(List<String> tokenops)
        {
            cbOperation.Items.Clear();
            cbOperation.Items.AddRange(tokenops.ToArray());
            cbOperation.SelectedIndex = (operation < cbOperation.Items.Count) ? operation : -1;
        }

        private void doTokenType()
        {
            gbTokenTypes.IsEnabled = true;
            ckbTTInvVisible.IsEnabled = !ckbTTVisible.IsEnabled || ckbTTVisible.IsChecked == true;
            ckbTTInvMemory.IsEnabled = !ckbTTMemory.IsEnabled || ckbTTMemory.IsChecked == true;
            ckbTTInvShopping.IsEnabled = ckbTTShopping.IsChecked == true;
            ckbTTAll.IsChecked = ckbTTVisible.IsChecked == true != true && ckbTTMemory.IsChecked == true != true && ckbTTShopping.IsChecked == true != true;
        }

        private void doFromInventory(bool enable)
        {
            if (enable)
                cbInventory.IsEnabled = true;
            int i = (o5678[1] & 0x07);
            cbInventory.SelectedIndex = (i < cbInventory.Items.Count) ? i : -1;
            lbDoid3.Content = (pnDoid3.IsEnabled = (i >= 1 && i <= 3)) ? cbInventory.SelectedItem.ToString() : "";
        }

        private void doByGUID()
        {
            flpnGUID.IsEnabled = true;
            setGUID(o5678, 0);
        }

        private void refreshDoid1()
        {
            tbVal1.Text = "0x" + SimPe.Helper.HexString(BhavWiz.ToShort(o5678[2], o5678[3]));
            cbDataOwner1.SelectedIndex = (cbDataOwner1.Items.Count > o5678[1]) ? o5678[1] : -1;
        }

        private void doBoth(List<String> aTokenOps, int[][] aNames, bool[] aByGUID, bool[] aCategory)
        {
            doTokenOps(aTokenOps);

            pnDoid1.IsEnabled = pnDoid2.IsEnabled = pnDoid3.IsEnabled = false;
            gbTokenTypes.IsEnabled = ckbReversed.IsEnabled = false;
            cbInventory.IsEnabled = false;
            flpnGUID.IsEnabled = false; tbObjName.Text = tbGUID.Text = "";
            gbInventoryType.IsEnabled = true;

            if (operation < aByGUID.Length && aByGUID[operation])
                doByGUID();

            if (operation < aCategory.Length && aCategory[operation])
                doTokenType();

            bool doid1Enabled = pnDoid1.IsEnabled;

            if (operation < aNames[0].Length)
            {
                lbDoid1.Content = (pnDoid1.IsEnabled = (aNames[1][operation] > 0)) ? pjse.Localization.GetString(names[aNames[1][operation]]) : "";
                lbDoid2.Content = (pnDoid2.IsEnabled = (aNames[2][operation] > 0)) ? pjse.Localization.GetString(names[aNames[2][operation]]) : "";
                lbDoid3.Content = (pnDoid3.IsEnabled = (aNames[3][operation] > 0)) ? pjse.Localization.GetString(names[aNames[3][operation]]) : "";
            }

            if (!doid1Enabled && pnDoid1.IsEnabled) refreshDoid1();
        }

        private void doCounted()
        {
            doBoth(aTokenOpsCounted, aNamesCounted, aByGUIDCounted, aCategoryCounted);

            switch (operation)
            {
                case 0x0b: doFromInventory(true); break;
            }
        }

        private void doSingular()
        {
            doBoth(aTokenOpsSingular, aNamesSingular, aByGUIDSingular, aCategorySingular);

            switch (operation)
            {
                case 0x03: ckbReversed.IsEnabled = true; break;
                case 0x07: gbInventoryType.IsEnabled = false; break;
                case 0x08: gbInventoryType.IsEnabled = false; break;
                case 0x09: gbInventoryType.IsEnabled = false; break;
                case 0x0c: ckbReversed.IsEnabled = true; break;
                case 0x12: doFromInventory(true); break;
            }
        }

        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x0033; } }

        public void Execute(Instruction inst)
        {
            this.inst = inst;

            wrappedByteArray ops1 = inst.Operands;
            wrappedByteArray ops2 = inst.Reserved1;

            o5678[0] = ops1[5];
            o5678[1] = ops1[6];
            o5678[2] = ops1[7];
            o5678[3] = ops2[0];

            internalchg = true;

            Boolset option1 = ops1[0];
            if (inst.NodeVersion < 1)
            {
                // In the parser we have something like this...
                //option1 = (inst.NodeVersion >= 1) ? ops1[0] : (byte)(((ops1[0] & 0x3C) << 1) | (ops1[0] & 0x83));
                // 8765 4321
                // 0065 4300 <<1 =
                // 0654 3000 |
                // 8000 0021 =
                // 8654 3021

                List<String> aS = new List<string>(aInventoryType.ToArray());
                aS.RemoveRange(4, aS.Count - 4);
                cbTargetInv.Items.Clear();
                cbTargetInv.Items.AddRange(aS.ToArray());
                cbInventory.Items.Clear();
                cbInventory.Items.AddRange(aS.ToArray());
                cbTargetInv.SelectedIndex = ((option1 & 0x03) < cbTargetInv.Items.Count) ? option1 & 0x03 : -1;

                rb1Counted.IsChecked = option1[2];
                ckbTTInvVisible.IsChecked = !option1[3];
                ckbTTInvMemory.IsChecked = !option1[4];
            }
            else
            {
                cbTargetInv.Items.Clear();
                cbTargetInv.Items.AddRange(aInventoryType.ToArray());
                cbInventory.Items.Clear();
                cbInventory.Items.AddRange(aInventoryType.ToArray());
                cbTargetInv.SelectedIndex = ((option1 & 0x07) < cbTargetInv.Items.Count) ? option1 & 0x07 : -1;

                rb1Counted.IsChecked = option1[3];
                ckbTTInvVisible.IsChecked = !option1[4];
                ckbTTInvMemory.IsChecked = !option1[5];
            }
            ckbReversed.IsChecked = option1[7];

            pnDoid0.IsEnabled = ((cbTargetInv.SelectedIndex) >= 1 && (cbTargetInv.SelectedIndex) <= 3);
            lbDoid0.Content = pnDoid0.IsEnabled ? cbTargetInv.SelectedItem.ToString() : "";
            rb1Singular.IsChecked = rb1Counted.IsChecked == true != true;

            doid0 = new DataOwnerControl(inst, cbDataOwner0, cbPicker0, tbVal0,
                ckbDecimal, ckbAttrPicker, null, ops1[1], BhavWiz.ToShort(ops1[2], ops1[3]));

            operation = ops1[4];

            doid1 = new DataOwnerControl(inst, cbDataOwner1, cbPicker1, tbVal1,
                ckbDecimal, ckbAttrPicker, null, o5678[1], BhavWiz.ToShort(o5678[2], o5678[3]));
            doid1.DataOwnerControlChanged += new EventHandler(doid1_DataOwnerControlChanged);

            ckbTTVisible.IsEnabled = ckbTTMemory.IsEnabled = ckbTTShopping.IsEnabled = (inst.NodeVersion >= 2);
            if (inst.NodeVersion >= 2)
            {
                Boolset option2 = ops2[1];
                ckbTTInvShopping.IsChecked = !option2[0];
                ckbTTVisible.IsChecked = option2[2];
                ckbTTMemory.IsChecked = option2[3];
                ckbTTShopping.IsChecked = option2[5];
            }

            doid2 = new DataOwnerControl(inst, cbDataOwner2, cbPicker2, tbVal2,
                ckbDecimal, ckbAttrPicker, null, ops2[2], BhavWiz.ToShort(ops2[3], ops2[4]));

            doid3 = new DataOwnerControl(inst, cbDataOwner3, cbPicker3, tbVal3,
                ckbDecimal, ckbAttrPicker, null, ops2[5], BhavWiz.ToShort(ops2[6], ops2[7]));


            if (rb1Counted.IsChecked == true)
                doCounted();
            else
                doSingular();

            cbOperation.SelectedIndex = (operation < cbOperation.Items.Count) ? operation : -1;

            internalchg = false;
        }

        void doid1_DataOwnerControlChanged(object sender, EventArgs e)
        {
            if (internalchg) return;
            if (doid1.DataOwner >= 0)
                o5678[1] = doid1.DataOwner;
            BhavWiz.FromShort(ref o5678, 2, doid1.Value);
        }


        public Instruction Write(Instruction inst)
        {
            if (inst != null)
            {
                wrappedByteArray ops1 = inst.Operands;
                wrappedByteArray ops2 = inst.Reserved1;

                Boolset option1 = ops1[0];
                if (inst.NodeVersion < 1)
                {
                    if (cbTargetInv.SelectedIndex != null)
                        option1 = (byte)((option1 & 0xfc) | (cbTargetInv.SelectedIndex & 0x03));

                    option1[2] = rb1Counted.IsChecked == true;
                    option1[3] = ckbTTInvVisible.IsChecked == true != true;
                    option1[4] = ckbTTInvMemory.IsChecked == true != true;
                }
                else
                {
                    if (cbTargetInv.SelectedIndex != null)
                        option1 = (byte)((option1 & 0xf8) | (cbTargetInv.SelectedIndex & 0x07));

                    option1[3] = rb1Counted.IsChecked == true;
                    option1[4] = ckbTTInvVisible.IsChecked == true != true;
                    option1[5] = ckbTTInvMemory.IsChecked == true != true;
                }
                option1[7] = ckbReversed.IsChecked == true;
                ops1[0] = option1;

                ops1[1] = doid0.DataOwner;
                BhavWiz.FromShort(ref ops1, 2, doid0.Value);

                ops1[4] = operation;

                ops1[5] = o5678[0];
                ops1[6] = o5678[1];
                ops1[7] = o5678[2];
                ops2[0] = o5678[3];

                if (inst.NodeVersion >= 2)
                {
                    Boolset option2 = ops2[1];
                    option2[0] = ckbTTInvShopping.IsChecked == true != true;
                    option2[2] = ckbTTVisible.IsChecked == true;
                    option2[3] = ckbTTMemory.IsChecked == true;
                    option2[5] = ckbTTShopping.IsChecked == true;
                    ops2[1] = option2;
                }

                ops2[2] = doid2.DataOwner;
                BhavWiz.FromShort(ref ops2, 3, doid2.Value);

                ops2[5] = doid3.DataOwner;
                BhavWiz.FromShort(ref ops2, 6, doid3.Value);
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
        {            this.pnWiz0x0033 = new StackPanel();
            this.tlpnGetSetValue = new TableLayoutPanel();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.ckbDecimal = new CheckBoxCompat2();
            this.ckbAttrPicker = new CheckBoxCompat2();
            this.lbOperation = new LabelCompat();
            this.flpnOperation = new FlowLayoutPanel();
            this.cbOperation = new ComboBoxCompat();
            this.ckbReversed = new CheckBoxCompat2();
            this.gbTokenTypes = new SimPe.Scenegraph.Compat.GroupBox();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.ckbTTAll = new CheckBoxCompat2();
            this.ckbTTInvShopping = new CheckBoxCompat2();
            this.ckbTTShopping = new CheckBoxCompat2();
            this.ckbTTInvMemory = new CheckBoxCompat2();
            this.ckbTTMemory = new CheckBoxCompat2();
            this.ckbTTInvVisible = new CheckBoxCompat2();
            this.ckbTTVisible = new CheckBoxCompat2();
            this.gbInventoryType = new SimPe.Scenegraph.Compat.GroupBox();
            this.flpnDoid0 = new FlowLayoutPanel();
            this.lbDoid0 = new LabelCompat();
            this.pnDoid0 = new StackPanel();
            this.cbPicker0 = new ComboBoxCompat();
            this.tbVal0 = new TextBoxCompat();
            this.cbDataOwner0 = new ComboBoxCompat();
            this.flpnInventoryType = new FlowLayoutPanel();
            this.rb1Counted = new Avalonia.Controls.RadioButton();
            this.rb1Singular = new Avalonia.Controls.RadioButton();
            this.cbTargetInv = new ComboBoxCompat();
            this.lbDoid1 = new LabelCompat();
            this.pnDoid1 = new StackPanel();
            this.cbPicker1 = new ComboBoxCompat();
            this.tbVal1 = new TextBoxCompat();
            this.cbDataOwner1 = new ComboBoxCompat();
            this.pnDoid3 = new StackPanel();
            this.cbPicker3 = new ComboBoxCompat();
            this.tbVal3 = new TextBoxCompat();
            this.cbDataOwner3 = new ComboBoxCompat();
            this.pnDoid2 = new StackPanel();
            this.cbPicker2 = new ComboBoxCompat();
            this.tbVal2 = new TextBoxCompat();
            this.cbDataOwner2 = new ComboBoxCompat();
            this.lbInventory = new LabelCompat();
            this.lbDoid3 = new LabelCompat();
            this.cbInventory = new ComboBoxCompat();
            this.flpnGUID = new FlowLayoutPanel();
            this.tbGUID = new TextBoxCompat();
            this.tbObjName = new TextBoxCompat();
            this.lbDoid2 = new LabelCompat();
            this.lbGUID = new LabelCompat();            //
            // pnWiz0x0033
            //            this.pnWiz0x0033.Children.Add(this.tlpnGetSetValue);
            this.pnWiz0x0033.Name = "pnWiz0x0033";
            //
            // tlpnGetSetValue
            //            this.tlpnGetSetValue.Children.Add(this.flowLayoutPanel1);
            this.tlpnGetSetValue.Children.Add(this.lbOperation);
            this.tlpnGetSetValue.Children.Add(this.flpnOperation);
            this.tlpnGetSetValue.Children.Add(this.gbTokenTypes);
            this.tlpnGetSetValue.Children.Add(this.gbInventoryType);
            this.tlpnGetSetValue.Children.Add(this.lbDoid1);
            this.tlpnGetSetValue.Children.Add(this.pnDoid1);
            this.tlpnGetSetValue.Children.Add(this.pnDoid3);
            this.tlpnGetSetValue.Children.Add(this.pnDoid2);
            this.tlpnGetSetValue.Children.Add(this.lbInventory);
            this.tlpnGetSetValue.Children.Add(this.lbDoid3);
            this.tlpnGetSetValue.Children.Add(this.cbInventory);
            this.tlpnGetSetValue.Children.Add(this.flpnGUID);
            this.tlpnGetSetValue.Children.Add(this.lbDoid2);
            this.tlpnGetSetValue.Children.Add(this.lbGUID);
            this.tlpnGetSetValue.Name = "tlpnGetSetValue";
            //
            // flowLayoutPanel1
            //            this.flowLayoutPanel1.Children.Add(this.ckbDecimal);
            this.flowLayoutPanel1.Children.Add(this.ckbAttrPicker);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            //
            // ckbDecimal
            //            this.ckbDecimal.Name = "ckbDecimal";
            //
            // ckbAttrPicker
            //            this.ckbAttrPicker.Name = "ckbAttrPicker";
            //
            // lbOperation
            //            this.lbOperation.Name = "lbOperation";
            //
            // flpnOperation
            //            this.flpnOperation.Children.Add(this.cbOperation);
            this.flpnOperation.Children.Add(this.ckbReversed);
            this.flpnOperation.Name = "flpnOperation";
            //
            // cbOperation
            //
            this.cbOperation.SelectionChanged += (s, e) => this.cbOperation_SelectedIndexChanged(s, e);
            //
            // ckbReversed
            //            this.ckbReversed.Name = "ckbReversed";
            // gbTokenTypes
            //            this.gbTokenTypes.Children.Add(this.tableLayoutPanel1);
            this.gbTokenTypes.Name = "gbTokenTypes";
            // tableLayoutPanel1
            //            this.tableLayoutPanel1.Children.Add(this.ckbTTAll);
            this.tableLayoutPanel1.Children.Add(this.ckbTTInvShopping);
            this.tableLayoutPanel1.Children.Add(this.ckbTTShopping);
            this.tableLayoutPanel1.Children.Add(this.ckbTTInvMemory);
            this.tableLayoutPanel1.Children.Add(this.ckbTTMemory);
            this.tableLayoutPanel1.Children.Add(this.ckbTTInvVisible);
            this.tableLayoutPanel1.Children.Add(this.ckbTTVisible);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            //
            // ckbTTAll
            //            this.ckbTTAll.IsChecked = true;
            this.ckbTTAll.Name = "ckbTTAll";
            // ckbTTInvShopping
            //            this.ckbTTInvShopping.Name = "ckbTTInvShopping";
            // ckbTTShopping
            //            this.ckbTTShopping.Name = "ckbTTShopping";
            this.ckbTTShopping.IsCheckedChanged += (s, e) => this.ckbTT_CheckedChanged(s, e);
            //
            // ckbTTInvMemory
            //            this.ckbTTInvMemory.Name = "ckbTTInvMemory";
            // ckbTTMemory
            //            this.ckbTTMemory.Name = "ckbTTMemory";
            this.ckbTTMemory.IsCheckedChanged += (s, e) => this.ckbTT_CheckedChanged(s, e);
            //
            // ckbTTInvVisible
            //            this.ckbTTInvVisible.Name = "ckbTTInvVisible";
            // ckbTTVisible
            //            this.ckbTTVisible.Name = "ckbTTVisible";
            this.ckbTTVisible.IsCheckedChanged += (s, e) => this.ckbTT_CheckedChanged(s, e);
            //
            // gbInventoryType
            //            this.gbInventoryType.Children.Add(this.flpnDoid0);
            this.gbInventoryType.Children.Add(this.flpnInventoryType);
            this.gbInventoryType.Name = "gbInventoryType";
            // flpnDoid0
            //            this.flpnDoid0.Children.Add(this.lbDoid0);
            this.flpnDoid0.Children.Add(this.pnDoid0);
            this.flpnDoid0.Name = "flpnDoid0";
            //
            // lbDoid0
            //            this.lbDoid0.Name = "lbDoid0";
            this.lbDoid0.Tag = "";
            //
            // pnDoid0
            //            this.pnDoid0.Children.Add(this.cbPicker0);
            this.pnDoid0.Children.Add(this.tbVal0);
            this.pnDoid0.Children.Add(this.cbDataOwner0);
            this.pnDoid0.Name = "pnDoid0";
            //
            // cbPicker0
            //
            this.cbPicker0.Name = "cbPicker0";
            // tbVal0
            //            this.tbVal0.Name = "tbVal0";
            // cbDataOwner0
            //
            this.cbDataOwner0.Name = "cbDataOwner0";
            //
            // flpnInventoryType
            //            this.flpnInventoryType.Children.Add(this.rb1Counted);
            this.flpnInventoryType.Children.Add(this.rb1Singular);
            this.flpnInventoryType.Children.Add(this.cbTargetInv);
            this.flpnInventoryType.Name = "flpnInventoryType";
            //
            // rb1Counted
            //            this.rb1Counted.Name = "rb1Counted";
            this.rb1Counted.IsCheckedChanged += (s, e) => this.rb1_CheckedChanged(s, e);
            //
            // rb1Singular
            //            this.rb1Singular.Name = "rb1Singular";
            this.rb1Singular.IsCheckedChanged += (s, e) => this.rb1_CheckedChanged(s, e);
            //
            // cbTargetInv
            //
            this.cbTargetInv.SelectionChanged += (s, e) => this.cbTargetInv_SelectedIndexChanged(s, e);
            //
            // lbDoid1
            //            this.lbDoid1.Name = "lbDoid1";
            //
            // pnDoid1
            //            this.pnDoid1.Children.Add(this.cbPicker1);
            this.pnDoid1.Children.Add(this.tbVal1);
            this.pnDoid1.Children.Add(this.cbDataOwner1);
            this.pnDoid1.Name = "pnDoid1";
            //
            // cbPicker1
            //
            this.cbPicker1.Name = "cbPicker1";
            // tbVal1
            //            this.tbVal1.Name = "tbVal1";
            // cbDataOwner1
            //
            this.cbDataOwner1.Name = "cbDataOwner1";
            //
            // pnDoid3
            //            this.pnDoid3.Children.Add(this.cbPicker3);
            this.pnDoid3.Children.Add(this.tbVal3);
            this.pnDoid3.Children.Add(this.cbDataOwner3);
            this.pnDoid3.Name = "pnDoid3";
            //
            // cbPicker3
            //
            this.cbPicker3.Name = "cbPicker3";
            // tbVal3
            //            this.tbVal3.Name = "tbVal3";
            // cbDataOwner3
            //
            this.cbDataOwner3.Name = "cbDataOwner3";
            //
            // pnDoid2
            //            this.pnDoid2.Children.Add(this.cbPicker2);
            this.pnDoid2.Children.Add(this.tbVal2);
            this.pnDoid2.Children.Add(this.cbDataOwner2);
            this.pnDoid2.Name = "pnDoid2";
            //
            // cbPicker2
            //
            this.cbPicker2.Name = "cbPicker2";
            // tbVal2
            //            this.tbVal2.Name = "tbVal2";
            // cbDataOwner2
            //
            this.cbDataOwner2.Name = "cbDataOwner2";
            //
            // lbInventory
            //            this.lbInventory.Name = "lbInventory";
            this.lbInventory.Tag = "";
            //
            // lbDoid3
            //            this.lbDoid3.Name = "lbDoid3";
            this.lbDoid3.Tag = "";
            //
            // cbInventory
            //
            this.cbInventory.Name = "cbInventory";
            this.cbInventory.SelectionChanged += (s, e) => this.cbInventory_SelectedIndexChanged(s, e);
            //
            // flpnGUID
            //            this.flpnGUID.Children.Add(this.tbGUID);
            this.flpnGUID.Children.Add(this.tbObjName);
            this.flpnGUID.Name = "flpnGUID";
            //
            // tbGUID
            //            this.tbGUID.Name = "tbGUID";
            this.tbGUID.LostFocus += (s, e) => this.hex32_Validated(s, e);
            this.tbGUID.TextChanged += (s, e) => this.tbGUID_TextChanged(s, e);
            //
            // tbObjName
            //            this.tbObjName.Name = "tbObjName";
            this.tbObjName.IsReadOnly = true;
            // lbDoid2
            //            this.lbDoid2.Name = "lbDoid2";
            this.lbDoid2.Tag = "";
            //
            // lbGUID
            //            this.lbGUID.Name = "lbGUID";
            this.lbGUID.Tag = "";
            //
            // UI
            //            this.Controls.Add(this.pnWiz0x0033);

        }
        #endregion

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (internalchg) return;
            internalchg = true;

            if (rb1Counted.IsChecked == true) doCounted(); else doSingular();

            internalchg = false;
        }

        private void cbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            operation = (byte)(cbOperation.SelectedIndex);
            rb1_CheckedChanged(sender, e);
        }

        private void tbGUID_TextChanged(object sender, EventArgs e)
        {
            if (internalchg) return;

            if (!hex32_IsValid(sender)) return;
            setGUID(false, Convert.ToUInt32(((TextBoxCompat)sender).Text, 16));
        }

        private void hex32_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (hex32_IsValid(sender)) return;

            e.Cancel = true;

            bool origstate = internalchg;
            internalchg = true;
            byte[] o = { inst.Operands[0x05], inst.Operands[0x06], inst.Operands[0x07], inst.Reserved1[0] };
            setGUID(o, 0);
            ((TextBoxCompat)sender).SelectAll();
            internalchg = origstate;
        }

        private void hex32_Validated(object sender, System.EventArgs e)
        {
            bool origstate = internalchg;
            internalchg = true;

            ((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(Convert.ToUInt32(((TextBoxCompat)sender).Text, 16));
            ((TextBoxCompat)sender).SelectAll();

            UInt32 i = Convert.ToUInt32(((TextBoxCompat)sender).Text, 16);
            o5678[0] = (byte)(i & 0xff);
            o5678[1] = (byte)((i >> 8) & 0xff);
            o5678[2] = (byte)((i >> 16) & 0xff);
            o5678[3] = (byte)((i >> 24) & 0xff);
            refreshDoid1();
            doFromInventory(false);

            internalchg = origstate;
        }

        private void ckbTT_CheckedChanged(object sender, EventArgs e)
        {
            List<CheckBoxCompat2> tt = new List<CheckBoxCompat2>(new CheckBoxCompat2[] { ckbTTVisible, ckbTTMemory, ckbTTShopping });
            List<CheckBoxCompat2> tti = new List<CheckBoxCompat2>(new CheckBoxCompat2[] { ckbTTInvVisible, ckbTTInvMemory, ckbTTInvShopping });
            int i = tt.IndexOf((CheckBoxCompat2)sender);
            tti[i].IsEnabled = tt[i].IsChecked == true;
            ckbTTAll.IsChecked = ckbTTVisible.IsChecked == true != true && ckbTTMemory.IsChecked == true != true && ckbTTShopping.IsChecked == true != true;
        }

        private void cbTargetInv_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnDoid0.IsEnabled = ((cbTargetInv.SelectedIndex) >= 1 && (cbTargetInv.SelectedIndex) <= 3);
            lbDoid0.Content = pnDoid0.IsEnabled ? cbTargetInv.SelectedItem.ToString() : "";
        }

        private void cbInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool origstate = internalchg;
            internalchg = true;

            if (cbInventory.SelectedIndex != null && (cbInventory.SelectedIndex) <= 7)
                o5678[1] = (byte)((o5678[1] & 0xf8) + cbInventory.SelectedIndex);
            refreshDoid1();

            pnDoid3.IsEnabled = ((cbInventory.SelectedIndex) >= 1 && (cbInventory.SelectedIndex) <= 3);
            lbDoid3.Content = pnDoid3.IsEnabled ? cbInventory.SelectedItem.ToString() : "";

            internalchg = origstate;
        }
    }

}

namespace pjse.BhavOperandWizards
{
    public class BhavOperandWiz0x0033 : pjse.ABhavOperandWiz
	{
        public BhavOperandWiz0x0033(Instruction i) : base(i) { myForm = new Wiz0x0033.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
