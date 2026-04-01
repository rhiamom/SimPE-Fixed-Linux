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
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using SimPe.Scenegraph.Compat;
using SimPe.PackedFiles.Wrapper;

namespace pjse.BhavOperandWizards.Wiz0x0024
{
	/// <summary>
	/// Summary description for BhavInstruction.
	/// </summary>
    internal class UI : Window, iBhavOperandWizForm
	{
		#region Form variables

		internal StackPanel pnWiz0x0024;
		private ComboBoxCompat cbType;
		private LabelCompat label1;
		private LabelCompat label2;
		private LabelCompat lbType;
		private LabelCompat lbMessage;
		private LabelCompat lbTitle;
		private LabelCompat label3;
		private ComboBoxCompat cbScope;
		private LabelCompat lbIconType;
		private CheckBoxCompat2 cbBlockBHAV;
        private CheckBoxCompat2 cbBlockSim;
		private ButtonCompat button1;
		private LabelCompat label4;
		private CheckBoxCompat2 cbUTMessage;
		private CheckBoxCompat2 cbUTButton1;
		private CheckBoxCompat2 cbUTButton2;
		private CheckBoxCompat2 cbUTButton3;
		private CheckBoxCompat2 cbUTTitle;
		private ComboBoxCompat cbIconType;
		private LabelCompat label5;
        private TextBoxCompat tbIconID;
		private ButtonCompat btnStrIcon;
		private StackPanel pnTNS;
		private TextBoxCompat tbPriority;
		private LabelCompat label6;
		private LabelCompat label7;
		private TextBoxCompat tbTimeout;
		private LabelCompat lbTnsStyle;
		private ComboBoxCompat cbTnsStyle;
		private StackPanel pnTempVar;
		private LabelCompat lbTempVar;
		private StackPanel pnLocalVar;
		private LabelCompat label8;
		private ComboBoxCompat cbTempVar;
		private ComboBoxCompat cbTVMessage;
		private ComboBoxCompat cbTVButton1;
		private ComboBoxCompat cbTVButton2;
		private ComboBoxCompat cbTVButton3;
		private ComboBoxCompat cbTVTitle;
		private TextBoxCompat tbLocalVar;
		private TextBoxCompat tbMessage;
		private TextBoxCompat tbButton1;
		private TextBoxCompat tbButton2;
		private TextBoxCompat tbButton3;
		private TextBoxCompat tbTitle;
		private TextBoxCompat tbStrMessage;
		private TextBoxCompat tbStrButton1;
		private TextBoxCompat tbStrButton2;
		private TextBoxCompat tbStrButton3;
		private TextBoxCompat tbStrTitle;
		private LabelCompat lbButton3;
		private LabelCompat lbButton2;
		private LabelCompat lbButton1;
        private ButtonCompat btnDefTitle;
        private ButtonCompat btnDefButton3;
        private ButtonCompat btnDefButton2;
        private ButtonCompat btnDefButton1;
        private ButtonCompat btnDefMessage;
        private ButtonCompat btnStrTitle;
        private ButtonCompat btnStrButton3;
        private ButtonCompat btnStrButton2;
        private ButtonCompat btnStrButton1;
        private ButtonCompat btnStrMessage;
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

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			cbType.Items.Clear();
			cbType.Items.AddRange(BhavWiz.readStr(GS.BhavStr.Dialog).ToArray());

			if (typeDescriptions == null)
				typeDescriptions = BhavWiz.readStr(GS.BhavStr.DialogDesc);

			cbTnsStyle.Items.Clear();
			cbTnsStyle.Items.AddRange(BhavWiz.readStr(GS.BhavStr.TnsStyle).ToArray());

			cbIconType.Items.Clear();
			cbIconType.Items.AddRange(BhavWiz.readStr(GS.BhavStr.DialogIcon).ToArray());

			ButtonCompat[] b = { btnStrMessage ,btnStrButton1 ,btnStrButton2 ,btnStrButton3 ,btnStrTitle ,btnStrIcon ,};
			alStrBtn = new ArrayList(b);

			ButtonCompat[] bd = { btnDefMessage ,btnDefButton1 ,btnDefButton2 ,btnDefButton3 ,btnDefTitle ,};
			alDefBtn = new ArrayList(bd);

			TextBoxCompat[] t = { tbStrMessage ,tbStrButton1 ,tbStrButton2 ,tbStrButton3 ,tbStrTitle ,};
			alTextBox = new ArrayList(t);

			CheckBoxCompat2[] c = { cbUTMessage ,cbUTButton1 ,cbUTButton2 ,cbUTButton3 ,cbUTTitle ,};
			alCBUseTemp = new ArrayList(c);

			ComboBoxCompat[] ct = { cbTVMessage ,cbTVButton1 ,cbTVButton2 ,cbTVButton3 ,cbTVTitle ,};
			alCBTempVar = new ArrayList(ct);

			TextBoxCompat[] tb8 = { tbPriority ,tbTimeout ,tbLocalVar ,tbIconID ,};
			alHex8 = new ArrayList(tb8);

			TextBoxCompat[] tb16 = { tbMessage ,tbButton1 ,tbButton2 ,tbButton3 ,tbTitle ,};
			alHex16 = new ArrayList(tb16);
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


		private static List<String> typeDescriptions = null;

		private Instruction inst = null;
		private ArrayList alStrBtn = null;
		private ArrayList alDefBtn = null;
		private ArrayList alTextBox = null;
		private ArrayList alCBUseTemp = null;
		private ArrayList alCBTempVar = null;
		private ArrayList alHex8 = null;
		private ArrayList alHex16 = null;

		byte dialog   = 0;
		bool nowait   = false;
		byte iconType = 0;
		byte iconID   = 0;
		byte tempVar  = 0;
		bool noblock  = false;
		byte tnsStyle = 0;
		byte priority = 0;
		byte timeout  = 0;
		byte localVar = 0;
		Scope scope   = Scope.Private;
		ushort[] messages = { 0, 0, 0, 0, 0 }; // Message, Yes, No, Cancel, Title
		bool[] useTemp = { false, false, false, false, false }; // Message, Yes, No, Cancel, Title
		bool[] states = { false, false, false, false, false }; // message, yes, no, cancel, title

		bool internalchg = false;

		private bool hex8_IsValid(object sender)
		{
			if (alHex8.IndexOf(sender) < 0)
				throw new Exception("hex8_IsValid not applicable to control " + sender.ToString());
			try { Convert.ToByte(((TextBoxCompat)sender).Text, 16); }
			catch (Exception) { return false; }
			return true;
		}

		private bool hex16_IsValid(object sender)
		{
			if (alHex16.IndexOf(sender) < 0)
				throw new Exception("hex16_IsValid not applicable to control " + sender.ToString());
			try { Convert.ToUInt16(((TextBoxCompat)sender).Text, 16); }
			catch (Exception) { return false; }
			return true;
		}


		private void setType(int newType)
		{
			internalchg = true;

			dialog = (byte)newType;

			if (dialog != cbType.SelectedIndex)
				cbType.SelectedIndex = (cbType.Items.Count > dialog) ? dialog : -1;

            this.lbType.Content = typeDescriptions.Count > dialog ? typeDescriptions[dialog] : "";

			bool tvState = false;
			bool tnsState = false;
            bool lvState = false;
            bool gtState = false;

			states[0] = states[1] = states[2] = states[3] = states[4] = false; // forget everything...
			switch(dialog)
			{
				case 0x00: case 0x03: case 0x04:
					states[0] = states[1] = states[4] = true; // message, button 1, title
					break;
				case 0x02:
					states[0] = states[1] = states[2] = states[4] = true; // message, button 1, button 2, title
					tvState = states[3] = true; // button 3
					break;
				case 0x08: case 0x0a: // TNS, TNS modify
					tnsState = tvState = states[0] = true; // message
					break;
				case 0x09: // TNS stop
					tvState = true;
					break;
				case 0x0e:
					states[0] = states[1] = states[2] = states[4] = true; // message, button 1, button 2, title
					lvState = true;
					break;
				case 0x0f:
					states[1] = states[2] = true; // button 1, button 2
					states[0] = states[3] = states[4] = false; // msg, btn3, title
					break;
				case 0x13:
					states[1] = states[2] = states[4] = true; // button 1, button 2, title
					break;
				case 0x0b: case 0x0c: case 0x0d:
				case 0x10: case 0x11: case 0x12:
				case 0x14: case 0x15:
                    break;
                    case 0x39: // Game Tip - CJH
                    gtState = states[0] = true;
                    break;
				case 0x16: case 0x19:
					states[0] = states[4] = true; // message, title
					break;
                case 0x1c: // TNS Append
                    tvState = states[0] = true; // message
                    break;
				default:
					states[0] = states[1] = states[2] = states[4] = true; // message, button 1, button 2, title
					break;
			}

			this.pnTempVar.IsVisible  = tvState;
			this.pnTNS.IsVisible      = tnsState;
            this.pnLocalVar.IsVisible = lvState;
            this.tbStrMessage.IsVisible = !gtState;
            this.label3.IsVisible = !gtState;
            this.label4.IsVisible = !gtState;
            this.lbIconType.IsVisible = !gtState;
            this.cbUTMessage.IsVisible = !gtState;
            this.cbScope.IsVisible = !gtState;
            this.cbBlockSim.IsVisible = !gtState;
            this.cbBlockBHAV.IsVisible = !gtState;
            this.cbIconType.IsVisible = !gtState;
            this.btnStrMessage.IsVisible = !gtState;
            
			internalchg = false;

			// Make the display match the help text
            for (int i = 0; i < states.Length; i++)
                setString(i, messages[i]);

            if (!gtState) 
                this.lbMessage.Content = "Message";
            else
                this.lbMessage.Content = "GameTip";
		}

		private void setTnsStyle(int newStyle)
		{
			internalchg = true;

			tnsStyle = (byte)newStyle;

			if (cbTnsStyle.Items.Count != tnsStyle)
				cbTnsStyle.SelectedIndex = (tnsStyle >= 0 && tnsStyle < cbTnsStyle.Items.Count) ? tnsStyle : -1;

			internalchg = false;
		}

		private void setScope(int newScope)
		{
			internalchg = true;

			scope = (Scope)newScope;

			if (cbScope.SelectedIndex != newScope)
				cbScope.SelectedIndex = (newScope >= 0 && newScope < cbScope.Items.Count) ? newScope : -1;

			for(int i = 0; i < messages.Length; i++)
				setString(i, messages[i]);

			internalchg = false;
		}

		private void setIconType(int newType)
		{
			internalchg = true;

			iconType = (byte)newType;

			if (cbIconType.SelectedIndex != iconType)
                cbIconType.SelectedIndex = (iconType >= 0  && iconType < cbIconType.Items.Count) ? iconType : -1;
			tbIconID.IsEnabled = (iconType == 3);
			btnStrIcon.IsEnabled = (iconType == 4);

			internalchg = false;
		}

		private void setTempVar(int newTempVar)
		{
			internalchg = true;

			tempVar = (byte)newTempVar;
            if (cbTempVar.SelectedIndex != tempVar)
    			cbTempVar.SelectedIndex = (tempVar >= 0 && tempVar < cbTempVar.Items.Count) ? tempVar : -1;

			internalchg = false;
		}

		private void setBlockBHAV(bool newFlag)
		{
			internalchg = true;

			nowait = !newFlag;
			this.cbBlockBHAV.Checked = newFlag;

			internalchg = false;
		}

		private void setBlockSim(bool newFlag)
		{
			internalchg = true;

			noblock = !newFlag;
			this.cbBlockSim.Checked = newFlag;

			internalchg = false;
		}

		private void setIconID(int newIconID)
		{
			iconID = (byte)newIconID;

			if (internalchg) return;
			internalchg = true;

			this.tbIconID.Text = "0x" + SimPe.Helper.HexString((byte)newIconID);

			internalchg = false;
		}

		private void setString(int which, int strnum)
		{
			messages[which] = (ushort)strnum;

			if (!states[which])
			{
				internalchg = true;
				((ComboBoxCompat)alCBTempVar[which]).SelectedIndex = -1;
				((TextBoxCompat)alHex16[which]).Text = "";
				internalchg = false;

				((TextBoxCompat)alTextBox[which]).Text = "";

				((ComboBoxCompat)this.alCBTempVar[which]).IsEnabled =
					((CheckBoxCompat2)this.alCBUseTemp[which]).IsEnabled =
					((TextBoxCompat)alHex16[which]).IsEnabled =
					((ButtonCompat)alStrBtn[which]).IsEnabled =
					((ButtonCompat)alDefBtn[which]).IsEnabled =
					((TextBoxCompat)alTextBox[which]).IsEnabled =
					false;

				return;
			}

			((CheckBoxCompat2)this.alCBUseTemp[which]).IsEnabled = true;

			if (useTemp[which])
			{
				ComboBoxCompat c = (ComboBoxCompat)alCBTempVar[which];
				internalchg = true;
				c.SelectedIndex = c.Items.Count > strnum ? strnum : -1;
				((TextBoxCompat)alHex16[which]).Text = "";
				internalchg = false;

				((TextBoxCompat)alTextBox[which]).Text = "";

				((CheckBoxCompat2)this.alCBUseTemp[which]).Checked =
					((ComboBoxCompat)this.alCBTempVar[which]).IsEnabled = true;
				((TextBoxCompat)alHex16[which]).IsEnabled =
					((ButtonCompat)alStrBtn[which]).IsEnabled =
					((ButtonCompat)alDefBtn[which]).IsEnabled =
					((TextBoxCompat)alTextBox[which]).IsEnabled =
					false;
			}
			else
			{
				if (!internalchg)
				{
					internalchg = true;
					((ComboBoxCompat)this.alCBTempVar[which]).SelectedIndex = -1;
					((TextBoxCompat)alHex16[which]).Text = "0x" + SimPe.Helper.HexString((ushort)strnum);
					internalchg = false;
				}

				((TextBoxCompat)alTextBox[which]).Text = (strnum <= 0)
                    ? "[" + pjse.Localization.GetString("none") + "]"
					: ((BhavWiz)inst).readStr(scope, GS.GlobalStr.DialogString, (ushort)(strnum - 1), -1, pjse.Detail.ErrorNames)
					;

				((CheckBoxCompat2)this.alCBUseTemp[which]).Checked =
					((ComboBoxCompat)this.alCBTempVar[which]).IsEnabled = false;
				((TextBoxCompat)alHex16[which]).IsEnabled =
					((ButtonCompat)alStrBtn[which]).IsEnabled =
					((TextBoxCompat)alTextBox[which]).IsEnabled =
					true;
				((ButtonCompat)alDefBtn[which]).IsEnabled = (strnum != 0);
			}
		}

		private void setUseTemp(int which, bool newFlag)
		{
			useTemp[which] = newFlag;
			setString(which, messages[which]);
		}

		private void setPriority(int newPriority)
		{
			priority = (byte)newPriority;

			if (internalchg) return;
			internalchg = true;

			this.tbPriority.Text = "0x" + SimPe.Helper.HexString((byte)newPriority);

			internalchg = false;
		}

		private void setTimeout(int newTimeout)
		{
			timeout = (byte)newTimeout;

			if (internalchg) return;
			internalchg = true;

			this.tbTimeout.Text = "0x" + SimPe.Helper.HexString((byte)newTimeout);

			internalchg = false;
		}

		private void setLocalVar(int newLocalVar)
		{
			localVar = (byte)newLocalVar;

			if (internalchg) return;
			internalchg = true;

			this.tbLocalVar.Text = "0x" + SimPe.Helper.HexString((byte)newLocalVar);

			internalchg = false;
		}


		private void doStrChooser(int which)
		{
			pjse.FileTable.Entry[] items = pjse.FileTable.GFT[(uint)SimPe.Data.MetaData.STRING_FILE, inst.Parent.GroupForScope(scope), (uint)GS.GlobalStr.DialogString];

            if (items == null || items.Length == 0)
            {
                MessageBox.Show(pjse.Localization.GetString("bow_noStrings")
                    + " (" + pjse.Localization.GetString(scope.ToString())  + ")");
                return; // eek!
            }

			SimPe.PackedFiles.Wrapper.StrWrapper str = new StrWrapper();
			str.ProcessData(items[0].PFD, items[0].Package);

			int i = (new StrChooser()).Strnum(str);
			if (i >= 0)
			{
				if (messages.Length > which)
				{
					setString(which, i + 1);
				}
				else
				{
					switch(which)
					{
						case 5: setIconID(i + 1); break;
					}
				}
			}
		}


        #region iBhavOperandWizForm
        public StackPanel WizPanel { get { return this.pnWiz0x0024; } }

        public void Execute(Instruction inst)
		{
			this.inst = inst;

			wrappedByteArray ops1 = inst.Operands;
			wrappedByteArray ops2 = inst.Reserved1;

			setType(ops1[5]);

			setTnsStyle(ops2[4]);

			if      ((ops2[0] & 0x01) != 0) setScope((int)Scope.SemiGlobal);
			else if ((ops2[0] & 0x40) != 0) setScope((int)Scope.Global);
			else                            setScope((int)Scope.Private);

			setIconID(ops1[0x01]);

			if (inst.NodeVersion == 0)
			{
				setString(0, ops1[2]);	// message
				setString(3, ops1[0]);	// cancel
			}
			else
			{
				setString(0, BhavWiz.ToShort(ops2[5], ops2[6]));	// message
				setString(3, BhavWiz.ToShort(ops1[0], ops1[2]));	// cancel
			}
			setString(1, ops1[3]); // Yes
			setString(2, ops1[4]); // No
			setString(4, ops1[6]); // Title

			setBlockBHAV((ops1[7] & 0x01) == 0);
			setIconType((ops1[7] >> 1) & 0x07);
			setTempVar((ops1[7] >> 4) & 0x07);
			setBlockSim((ops1[7] & 0x80) == 0);

			setUseTemp(0, (ops2[0] & 0x02) != 0); // Message
			setUseTemp(1, (ops2[0] & 0x04) != 0); // Yes
			setUseTemp(2, (ops2[0] & 0x08) != 0); // No
			setUseTemp(3, (ops2[0] & 0x20) != 0); // Cancel
			setUseTemp(4, (ops2[0] & 0x10) != 0); // Title

			setPriority(ops2[1] + 1);
			setTimeout(ops2[2]);
			setLocalVar(ops2[3]);
		}

		public Instruction Write(Instruction inst)
		{
			if (inst != null)
			{
				wrappedByteArray ops1 = inst.Operands;
				wrappedByteArray ops2 = inst.Reserved1;

				ops1[0x01] = iconID;

				if (inst.NodeVersion == 0)
				{
					ops1[2] = (byte)messages[0];	// message
					ops1[0] = (byte)messages[3];	// cancel
				}
				else
				{
                    BhavWiz.FromShort(ref ops2, 5, messages[0]);	// message
                    byte[] lohi = { 0, 0 };
                    BhavWiz.FromShort(ref lohi, 0, messages[3]);	// cancel
                    ops1[0] = lohi[0];
                    ops1[2] = lohi[1];
				}
				ops1[3] = (byte)messages[1]; // Yes
				ops1[4] = (byte)messages[2]; // No
				ops1[6] = (byte)messages[4]; // Title

				ops1[5] = dialog;

				ops1[7] &= 0xfe; ops1[7] |= (byte)(nowait  ? 0x01 : 0);
				ops1[7] &= 0xf1; ops1[7] |= (byte)((iconType & 0x07) << 1);
				ops1[7] &= 0x8f; ops1[7] |= (byte)((tempVar  & 0x07) << 4);
				ops1[7] &= 0x7f; ops1[7] |= (byte)(noblock ? 0x80 : 0);

				ops2[0] &= 0xfd; ops2[0] |= (byte)(useTemp[0] ? 0x02 : 0); // Message
				ops2[0] &= 0xfb; ops2[0] |= (byte)(useTemp[1] ? 0x04 : 0); // Yes
				ops2[0] &= 0xf7; ops2[0] |= (byte)(useTemp[2] ? 0x08 : 0); // No
				ops2[0] &= 0xdf; ops2[0] |= (byte)(useTemp[3] ? 0x20 : 0); // Cancel
				ops2[0] &= 0xef; ops2[0] |= (byte)(useTemp[4] ? 0x10 : 0); // Title

				ops2[0] &= 0xbe;
				if      (scope == Scope.SemiGlobal) ops2[0] |= 0x01;
				else if (scope == Scope.Global)     ops2[0] |= 0x40;

				ops2[1] = (byte)(priority - 1);
				ops2[2] = timeout;
				ops2[3] = localVar;
				ops2[4] = tnsStyle;

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
		{            this.pnWiz0x0024 = new StackPanel();
            this.btnDefTitle = new ButtonCompat();
            this.btnDefButton3 = new ButtonCompat();
            this.btnDefButton2 = new ButtonCompat();
            this.btnDefButton1 = new ButtonCompat();
            this.btnDefMessage = new ButtonCompat();
            this.btnStrTitle = new ButtonCompat();
            this.btnStrButton3 = new ButtonCompat();
            this.btnStrButton2 = new ButtonCompat();
            this.btnStrButton1 = new ButtonCompat();
            this.btnStrMessage = new ButtonCompat();
            this.tbStrTitle = new TextBoxCompat();
            this.tbStrButton3 = new TextBoxCompat();
            this.tbStrButton2 = new TextBoxCompat();
            this.tbTitle = new TextBoxCompat();
            this.tbMessage = new TextBoxCompat();
            this.tbButton3 = new TextBoxCompat();
            this.cbTVMessage = new ComboBoxCompat();
            this.tbButton2 = new TextBoxCompat();
            this.lbMessage = new LabelCompat();
            this.tbButton1 = new TextBoxCompat();
            this.cbBlockBHAV = new CheckBoxCompat2();
            this.cbBlockSim = new CheckBoxCompat2();
            this.cbUTTitle = new CheckBoxCompat2();
            this.cbUTButton3 = new CheckBoxCompat2();
            this.lbIconType = new LabelCompat();
            this.cbUTButton2 = new CheckBoxCompat2();
            this.cbIconType = new ComboBoxCompat();
            this.cbUTButton1 = new CheckBoxCompat2();
            this.tbStrButton1 = new TextBoxCompat();
            this.label5 = new LabelCompat();
            this.tbStrMessage = new TextBoxCompat();
            this.tbIconID = new TextBoxCompat();
            this.btnStrIcon = new ButtonCompat();
            this.cbTVTitle = new ComboBoxCompat();
            this.cbTVButton3 = new ComboBoxCompat();
            this.cbTVButton2 = new ComboBoxCompat();
            this.cbTVButton1 = new ComboBoxCompat();
            this.label4 = new LabelCompat();
            this.cbUTMessage = new CheckBoxCompat2();
            this.cbScope = new ComboBoxCompat();
            this.label3 = new LabelCompat();
            this.lbTitle = new LabelCompat();
            this.lbButton3 = new LabelCompat();
            this.lbButton2 = new LabelCompat();
            this.lbButton1 = new LabelCompat();
            this.lbType = new LabelCompat();
            this.label1 = new LabelCompat();
            this.cbType = new ComboBoxCompat();
            this.pnLocalVar = new StackPanel();
            this.tbLocalVar = new TextBoxCompat();
            this.label8 = new LabelCompat();
            this.pnTempVar = new StackPanel();
            this.cbTempVar = new ComboBoxCompat();
            this.lbTempVar = new LabelCompat();
            this.pnTNS = new StackPanel();
            this.tbPriority = new TextBoxCompat();
            this.label6 = new LabelCompat();
            this.label7 = new LabelCompat();
            this.tbTimeout = new TextBoxCompat();
            this.lbTnsStyle = new LabelCompat();
            this.cbTnsStyle = new ComboBoxCompat();
            this.label2 = new LabelCompat();
            this.button1 = new ButtonCompat();            // pnWiz0x0024
            // 
            this.pnWiz0x0024.Children.Add(this.btnDefTitle);
            this.pnWiz0x0024.Children.Add(this.btnDefButton3);
            this.pnWiz0x0024.Children.Add(this.btnDefButton2);
            this.pnWiz0x0024.Children.Add(this.btnDefButton1);
            this.pnWiz0x0024.Children.Add(this.btnDefMessage);
            this.pnWiz0x0024.Children.Add(this.btnStrTitle);
            this.pnWiz0x0024.Children.Add(this.btnStrButton3);
            this.pnWiz0x0024.Children.Add(this.btnStrButton2);
            this.pnWiz0x0024.Children.Add(this.btnStrButton1);
            this.pnWiz0x0024.Children.Add(this.btnStrMessage);
            this.pnWiz0x0024.Children.Add(this.tbStrTitle);
            this.pnWiz0x0024.Children.Add(this.tbStrButton3);
            this.pnWiz0x0024.Children.Add(this.tbStrButton2);
            this.pnWiz0x0024.Children.Add(this.tbTitle);
            this.pnWiz0x0024.Children.Add(this.tbMessage);
            this.pnWiz0x0024.Children.Add(this.tbButton3);
            this.pnWiz0x0024.Children.Add(this.cbTVMessage);
            this.pnWiz0x0024.Children.Add(this.tbButton2);
            this.pnWiz0x0024.Children.Add(this.lbMessage);
            this.pnWiz0x0024.Children.Add(this.tbButton1);
            this.pnWiz0x0024.Children.Add(this.cbBlockBHAV);
            this.pnWiz0x0024.Children.Add(this.cbBlockSim);
            this.pnWiz0x0024.Children.Add(this.cbUTTitle);
            this.pnWiz0x0024.Children.Add(this.cbUTButton3);
            this.pnWiz0x0024.Children.Add(this.lbIconType);
            this.pnWiz0x0024.Children.Add(this.cbUTButton2);
            this.pnWiz0x0024.Children.Add(this.cbIconType);
            this.pnWiz0x0024.Children.Add(this.cbUTButton1);
            this.pnWiz0x0024.Children.Add(this.tbStrButton1);
            this.pnWiz0x0024.Children.Add(this.label5);
            this.pnWiz0x0024.Children.Add(this.tbStrMessage);
            this.pnWiz0x0024.Children.Add(this.tbIconID);
            this.pnWiz0x0024.Children.Add(this.btnStrIcon);
            this.pnWiz0x0024.Children.Add(this.cbTVTitle);
            this.pnWiz0x0024.Children.Add(this.cbTVButton3);
            this.pnWiz0x0024.Children.Add(this.cbTVButton2);
            this.pnWiz0x0024.Children.Add(this.cbTVButton1);
            this.pnWiz0x0024.Children.Add(this.label4);
            this.pnWiz0x0024.Children.Add(this.cbUTMessage);
            this.pnWiz0x0024.Children.Add(this.cbScope);
            this.pnWiz0x0024.Children.Add(this.label3);
            this.pnWiz0x0024.Children.Add(this.lbTitle);
            this.pnWiz0x0024.Children.Add(this.lbButton3);
            this.pnWiz0x0024.Children.Add(this.lbButton2);
            this.pnWiz0x0024.Children.Add(this.lbButton1);
            this.pnWiz0x0024.Children.Add(this.lbType);
            this.pnWiz0x0024.Children.Add(this.label1);
            this.pnWiz0x0024.Children.Add(this.cbType);
            this.pnWiz0x0024.Children.Add(this.pnLocalVar);
            this.pnWiz0x0024.Children.Add(this.pnTempVar);
            this.pnWiz0x0024.Children.Add(this.pnTNS);            this.pnWiz0x0024.Name = "pnWiz0x0024";
            // btnDefTitle
            this.btnDefTitle.Name = "btnDefTitle";
            this.btnDefTitle.Click += (s, e) => this.btnDef_Click(s, e);
            // btnDefButton3
            this.btnDefButton3.Name = "btnDefButton3";
            this.btnDefButton3.Click += (s, e) => this.btnDef_Click(s, e);
            // btnDefButton2
            this.btnDefButton2.Name = "btnDefButton2";
            this.btnDefButton2.Click += (s, e) => this.btnDef_Click(s, e);
            // btnDefButton1
            this.btnDefButton1.Name = "btnDefButton1";
            this.btnDefButton1.Click += (s, e) => this.btnDef_Click(s, e);
            // btnDefMessage
            this.btnDefMessage.Name = "btnDefMessage";
            this.btnDefMessage.Click += (s, e) => this.btnDef_Click(s, e);
            // btnStrTitle
            //            this.btnStrTitle.Name = "btnStrTitle";
            this.btnStrTitle.Click += (s, e) => this.btnStr_Click(s, e);
            // btnStrButton3
            //            this.btnStrButton3.Name = "btnStrButton3";
            this.btnStrButton3.Click += (s, e) => this.btnStr_Click(s, e);
            // btnStrButton2
            //            this.btnStrButton2.Name = "btnStrButton2";
            this.btnStrButton2.Click += (s, e) => this.btnStr_Click(s, e);
            // btnStrButton1
            //            this.btnStrButton1.Name = "btnStrButton1";
            this.btnStrButton1.Click += (s, e) => this.btnStr_Click(s, e);
            // btnStrMessage
            //            this.btnStrMessage.Name = "btnStrMessage";
            this.btnStrMessage.Click += (s, e) => this.btnStr_Click(s, e);
            // tbStrTitle
            //this.tbStrTitle.Name = "tbStrTitle";
            this.tbStrTitle.IsReadOnly = true;
            //this.tbStrButton3.Name = "tbStrButton3";
            this.tbStrButton3.IsReadOnly = true;
            //this.tbStrButton2.Name = "tbStrButton2";
            this.tbStrButton2.IsReadOnly = true;
            //            this.tbTitle.Name = "tbTitle";
            this.tbTitle.TextChanged += (s, e) => this.hex16_TextChanged(s, e);
            this.tbTitle.LostFocus += (s, e) => this.hex16_Validated(s, e);
            //            this.tbMessage.Name = "tbMessage";
            this.tbMessage.TextChanged += (s, e) => this.hex16_TextChanged(s, e);
            this.tbMessage.LostFocus += (s, e) => this.hex16_Validated(s, e);
            //            this.tbButton3.Name = "tbButton3";
            this.tbButton3.TextChanged += (s, e) => this.hex16_TextChanged(s, e);
            this.tbButton3.LostFocus += (s, e) => this.hex16_Validated(s, e);
            // 
            this.cbTVMessage.SelectionChanged += (s, e) => this.cbTempVar_SelectedIndexChanged(s, e);
            // tbButton2
            //            this.tbButton2.Name = "tbButton2";
            this.tbButton2.TextChanged += (s, e) => this.hex16_TextChanged(s, e);
            this.tbButton2.LostFocus += (s, e) => this.hex16_Validated(s, e);
            //            this.lbMessage.Name = "lbMessage";
            // tbButton1
            //            this.tbButton1.Name = "tbButton1";
            this.tbButton1.TextChanged += (s, e) => this.hex16_TextChanged(s, e);
            this.tbButton1.LostFocus += (s, e) => this.hex16_Validated(s, e);
            //            this.cbBlockBHAV.Name = "cbBlockBHAV";
            this.cbBlockBHAV.IsCheckedChanged += (s, e) => this.cbBlockBHAV_CheckedChanged(s, e);
            // cbBlockSim
            //            this.cbBlockSim.Name = "cbBlockSim";
            this.cbBlockSim.IsCheckedChanged += (s, e) => this.cbBlockSim_CheckedChanged(s, e);
            // cbUTTitle
            //            this.cbUTTitle.Name = "cbUTTitle";
            this.cbUTTitle.IsCheckedChanged += (s, e) => this.cbUT_CheckedChanged(s, e);
            // cbUTButton3
            //            this.cbUTButton3.Name = "cbUTButton3";
            this.cbUTButton3.IsCheckedChanged += (s, e) => this.cbUT_CheckedChanged(s, e);
            // lbIconType
            //            this.lbIconType.Name = "lbIconType";
            // cbUTButton2
            //            this.cbUTButton2.Name = "cbUTButton2";
            this.cbUTButton2.IsCheckedChanged += (s, e) => this.cbUT_CheckedChanged(s, e);
            // cbIconType
            // 
            this.cbIconType.Name = "cbIconType";
            this.cbIconType.SelectionChanged += (s, e) => this.cbIconType_SelectedIndexChanged(s, e);
            // cbUTButton1
            //            this.cbUTButton1.Name = "cbUTButton1";
            this.cbUTButton1.IsCheckedChanged += (s, e) => this.cbUT_CheckedChanged(s, e);
            // tbStrButton1
            //this.tbStrButton1.Name = "tbStrButton1";
            this.tbStrButton1.IsReadOnly = true;
            //            this.label5.Name = "label5";
            // tbStrMessage
            //this.tbStrMessage.Name = "tbStrMessage";
            this.tbStrMessage.IsReadOnly = true;
            //            this.tbIconID.Name = "tbIconID";
            this.tbIconID.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbIconID.LostFocus += (s, e) => this.hex8_TextChanged(s, e);
            //            this.btnStrIcon.Name = "btnStrIcon";
            this.btnStrIcon.Click += (s, e) => this.btnStr_Click(s, e);
            // cbTVTitle
            // 
            this.cbTVTitle.SelectionChanged += (s, e) => this.cbTempVar_SelectedIndexChanged(s, e);
            // cbTVButton3
            // 
            this.cbTVButton3.SelectionChanged += (s, e) => this.cbTempVar_SelectedIndexChanged(s, e);
            // cbTVButton2
            // 
            this.cbTVButton2.SelectionChanged += (s, e) => this.cbTempVar_SelectedIndexChanged(s, e);
            // cbTVButton1
            // 
            this.cbTVButton1.SelectionChanged += (s, e) => this.cbTempVar_SelectedIndexChanged(s, e);
            // label4
            //            this.label4.Name = "label4";
            // cbUTMessage
            //            this.cbUTMessage.Name = "cbUTMessage";
            this.cbUTMessage.IsCheckedChanged += (s, e) => this.cbUT_CheckedChanged(s, e);
            // cbScope
            // 
            this.cbScope.SelectionChanged += (s, e) => this.cbScope_SelectedIndexChanged(s, e);
            // label3
            //            this.label3.Name = "label3";
            // lbTitle
            //            this.lbTitle.Name = "lbTitle";
            // lbButton3
            //            this.lbButton3.Name = "lbButton3";
            // lbButton2
            //            this.lbButton2.Name = "lbButton2";
            // lbButton1
            //            this.lbButton1.Name = "lbButton1";
            // lbType
            //            this.lbType.Name = "lbType";
            // label1
            //            this.label1.Name = "label1";
            // cbType
            // 
            this.cbType.Name = "cbType";
            this.cbType.SelectionChanged += (s, e) => this.cbType_SelectedIndexChanged(s, e);
            // pnLocalVar
            //            this.pnLocalVar.Children.Add(this.tbLocalVar);
            this.pnLocalVar.Children.Add(this.label8);
            this.pnLocalVar.Name = "pnLocalVar";
            // tbLocalVar
            //            this.tbLocalVar.Name = "tbLocalVar";
            this.tbLocalVar.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbLocalVar.LostFocus += (s, e) => this.hex8_Validated(s, e);
            //            this.label8.Name = "label8";
            // pnTempVar
            //            this.pnTempVar.Children.Add(this.cbTempVar);
            this.pnTempVar.Children.Add(this.lbTempVar);
            this.pnTempVar.Name = "pnTempVar";
            // cbTempVar
            // 
            this.cbTempVar.SelectionChanged += (s, e) => this.cbTempVar_SelectedIndexChanged(s, e);
            // lbTempVar
            //            this.lbTempVar.Name = "lbTempVar";
            // pnTNS
            //            this.pnTNS.Children.Add(this.tbPriority);
            this.pnTNS.Children.Add(this.label6);
            this.pnTNS.Children.Add(this.label7);
            this.pnTNS.Children.Add(this.tbTimeout);
            this.pnTNS.Children.Add(this.lbTnsStyle);
            this.pnTNS.Children.Add(this.cbTnsStyle);
            this.pnTNS.Name = "pnTNS";
            // tbPriority
            //            this.tbPriority.Name = "tbPriority";
            this.tbPriority.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbPriority.LostFocus += (s, e) => this.hex8_Validated(s, e);
            //            this.label6.Name = "label6";
            // label7
            //            this.label7.Name = "label7";
            // tbTimeout
            //            this.tbTimeout.Name = "tbTimeout";
            this.tbTimeout.TextChanged += (s, e) => this.hex8_TextChanged(s, e);
            this.tbTimeout.LostFocus += (s, e) => this.hex8_Validated(s, e);
            //            this.lbTnsStyle.Name = "lbTnsStyle";
            // cbTnsStyle
            //            this.cbTnsStyle.Name = "cbTnsStyle";
            this.cbTnsStyle.SelectionChanged += (s, e) => this.cbTnsStyle_SelectedIndexChanged(s, e);
            // label2
            //            this.label2.Name = "label2";
            // button1
            //            this.button1.Name = "button1";
            // UI
            //            this.Controls.Add(this.button1);
            
            this.Content = this.pnWiz0x0024;
		}
		#endregion

		private void cbType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (internalchg) return;

			setType(((ComboBoxCompat)sender).SelectedIndex);
		}

		private void cbTnsStyle_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (internalchg) return;

			setTnsStyle(((ComboBoxCompat)sender).SelectedIndex);
		}

		private void cbScope_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (internalchg) return;

			setScope(((ComboBoxCompat)sender).SelectedIndex);
		}

		private void cbIconType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (internalchg) return;

			setIconType(((ComboBoxCompat)sender).SelectedIndex);
		}


		private void cbBlockBHAV_CheckedChanged(object sender, System.EventArgs e)
		{
			if (internalchg) return;

			setBlockBHAV(((CheckBoxCompat2)sender).Checked);
		}

		private void cbBlockSim_CheckedChanged(object sender, System.EventArgs e)
		{
			if (internalchg) return;

			setBlockSim(((CheckBoxCompat2)sender).Checked);
		}

		private void cbUT_CheckedChanged(object sender, System.EventArgs e)
		{
			if (internalchg) return;

			setUseTemp(alCBUseTemp.IndexOf(sender), ((CheckBoxCompat2)sender).Checked);
		}

		private void cbTempVar_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (internalchg) return;

			int i = this.alCBTempVar.IndexOf(sender);
			if (i >= 0)
				setString(i, ((ComboBoxCompat)sender).SelectedIndex);
			else
				setTempVar(((ComboBoxCompat)sender).SelectedIndex);
		}


		private void btnStr_Click(object sender, System.EventArgs e)
		{
			doStrChooser(alStrBtn.IndexOf(sender));
		}

		private void btnDef_Click(object sender, System.EventArgs e)
		{
			this.setString(alDefBtn.IndexOf(sender), 0);
		}


		private void hex8_TextChanged(object sender, System.EventArgs ev)
		{
			if (internalchg) return;
			if (!hex8_IsValid(sender)) return;

			byte val = Convert.ToByte(((TextBoxCompat)sender).Text, 16);
			int i = alHex8.IndexOf(sender);

			internalchg = true;

			switch(i)
			{
				case 0: setPriority(val); break;
				case 1: setTimeout(val); break;
				case 2: setLocalVar(val); break;
				case 3: setIconID(val); break;
			}

			internalchg = false;
		}

		private void hex8_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (hex8_IsValid(sender)) return;

			e.Cancel = true;

			byte val = 0;
			int i = alHex8.IndexOf(sender);

			switch(i)
			{
				case 0: val = priority; break;
				case 1: val = timeout; break;
				case 2: val = localVar; break;
				case 3: val = iconID; break;
			}

			bool origstate = internalchg;
			internalchg = true;
			((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(val);
			((TextBoxCompat)sender).SelectAll();
			internalchg = origstate;
		}

		private void hex8_Validated(object sender, System.EventArgs e)
		{
			bool origstate = internalchg;
			internalchg = true;
			((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(Convert.ToByte(((TextBoxCompat)sender).Text, 16));
			((TextBoxCompat)sender).SelectAll();
			internalchg = origstate;
		}


		private void hex16_TextChanged(object sender, System.EventArgs ev)
		{
			if (internalchg) return;
			if (!hex16_IsValid(sender)) return;

			internalchg = true;
			setString(alHex16.IndexOf(sender), Convert.ToUInt16(((TextBoxCompat)sender).Text, 16));
			internalchg = false;
		}

		private void hex16_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (hex16_IsValid(sender)) return;

			e.Cancel = true;

			bool origstate = internalchg;
			internalchg = true;
			((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(messages[alHex16.IndexOf(sender)]);
			((TextBoxCompat)sender).SelectAll();
			internalchg = origstate;
		}

		private void hex16_Validated(object sender, System.EventArgs e)
		{
			bool origstate = internalchg;
			internalchg = true;
			((TextBoxCompat)sender).Text = "0x" + SimPe.Helper.HexString(Convert.ToUInt16(((TextBoxCompat)sender).Text, 16));
			((TextBoxCompat)sender).SelectAll();
			internalchg = origstate;
		}


	}


}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x0024 : pjse.ABhavOperandWiz
	{
		public BhavOperandWiz0x0024(Instruction i) : base(i) { myForm = new Wiz0x0024.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
