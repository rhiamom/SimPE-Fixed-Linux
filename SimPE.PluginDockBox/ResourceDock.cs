/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
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
using Avalonia.Input;
using Ambertation.Windows.Forms;
using Compat = SimPe.Scenegraph.Compat;

namespace SimPe.Plugin.Tool.Dockable
{
    /// <summary>
    /// Summary description for ResourceDock.
    /// </summary>
    public class ResourceDock : Avalonia.Controls.Window
    {
        private DockManager manager;
        internal Ambertation.Windows.Forms.DockPanel dcWrapper;
        internal Ambertation.Windows.Forms.DockPanel dcResource;
        private Avalonia.Controls.StackPanel xpGradientPanel1;
        private Avalonia.Controls.StackPanel xpGradientPanel2;
        internal Avalonia.Controls.StackPanel pntypes;
        internal Avalonia.Controls.TextBox tbinstance;
        private Avalonia.Controls.TextBlock label11;
        internal Avalonia.Controls.TextBox tbtype;
        private Avalonia.Controls.TextBlock label8;
        private Avalonia.Controls.TextBlock label9;
        private Avalonia.Controls.TextBlock label10;
        internal Avalonia.Controls.TextBox tbgroup;
        internal Avalonia.Controls.ComboBox cbtypes;
        internal Avalonia.Controls.TextBlock label3;
        internal Avalonia.Controls.ComboBox cbComp;
        internal Avalonia.Controls.TextBox tbinstance2;
        internal Avalonia.Controls.TextBlock lbName;
        internal Avalonia.Controls.TextBlock label1;
        internal Avalonia.Controls.TextBlock label2;
        internal Avalonia.Controls.TextBlock label5;
        internal Avalonia.Controls.TextBlock lbAuthor;
        internal Avalonia.Controls.TextBlock lbVersion;
        internal Avalonia.Controls.TextBlock lbDesc;
        internal Avalonia.Controls.TextBlock lbComp;
        internal Ambertation.Windows.Forms.DockPanel dcPackage;
        private Avalonia.Controls.StackPanel xpGradientPanel3;
        internal Compat.PropertyGridStub pgHead;
        internal Avalonia.Controls.TextBlock label4;
        internal Compat.ListView lv;
        private Compat.ColumnHeader clOffset;
        private Compat.ColumnHeader clSize;
        internal Ambertation.Windows.Forms.DockPanel dcConvert;
        private Avalonia.Controls.StackPanel xpGradientPanel4;
        private Avalonia.Controls.TextBox tbHex;
        private Avalonia.Controls.TextBox tbDec;
        internal Ambertation.Windows.Forms.DockPanel dcHex;
        internal Ambertation.Windows.Forms.HexViewControl hvc;
        private Avalonia.Controls.TextBox tbBin;
        internal Avalonia.Controls.Button button1;
        internal Avalonia.Controls.Button btcopie;
        private Ambertation.Windows.Forms.HexEditControl hexEditControl1;
        private Avalonia.Controls.Button linkLabel1;
        internal Compat.PictureBoxCompat pb;
        private Avalonia.Controls.StackPanel panel1;
        private Avalonia.Controls.TextBox tbFloat;
        private DockContainer dockBottom;
        private Avalonia.Controls.StackPanel gradientpanel1;
        private Avalonia.Controls.TextBlock label13;
        private Avalonia.Controls.TextBlock label12;
        private Avalonia.Controls.TextBlock label7;
        private Avalonia.Controls.TextBlock label6;
        private System.ComponentModel.IContainer components;

        public ResourceDock()
        {
            //
            // Required designer variable.
            //
            InitializeComponent();

            ThemeManager tm = ThemeManager.Global.CreateChild();
            tm.AddControl(this.xpGradientPanel1);
            tm.AddControl(this.xpGradientPanel2);
            tm.AddControl(this.xpGradientPanel3);
            tm.AddControl(this.xpGradientPanel4);

            this.lv.View = Compat.View.Details;
            foreach (SimPe.Data.TypeAlias a in SimPe.Helper.TGILoader.FileTypes)
                cbtypes.Items.Add(a);
            // cbtypes.Sorted = true; // no Sorted in Avalonia ComboBox
            tbFloat.Width = tbBin.Width;
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.manager = new Ambertation.Windows.Forms.DockManager();
            this.dockBottom = new Ambertation.Windows.Forms.DockContainer();
            this.dcConvert = new Ambertation.Windows.Forms.DockPanel();
            this.dcPackage = new Ambertation.Windows.Forms.DockPanel();
            this.dcWrapper = new Ambertation.Windows.Forms.DockPanel();
            this.dcResource = new Ambertation.Windows.Forms.DockPanel();
            this.dcHex = new Ambertation.Windows.Forms.DockPanel();
            this.xpGradientPanel1 = new Avalonia.Controls.StackPanel();
            this.xpGradientPanel2 = new Avalonia.Controls.StackPanel();
            this.xpGradientPanel3 = new Avalonia.Controls.StackPanel();
            this.xpGradientPanel4 = new Avalonia.Controls.StackPanel();
            this.gradientpanel1 = new Avalonia.Controls.StackPanel();
            this.panel1 = new Avalonia.Controls.StackPanel();
            this.pntypes = new Avalonia.Controls.StackPanel();
            this.hvc = new Ambertation.Windows.Forms.HexViewControl();
            this.hexEditControl1 = new Ambertation.Windows.Forms.HexEditControl();
            this.pgHead = new Compat.PropertyGridStub();
            this.lv = new Compat.ListView();
            this.clOffset = new Compat.ColumnHeader();
            this.clSize = new Compat.ColumnHeader();
            this.tbinstance = new Avalonia.Controls.TextBox();
            this.tbtype = new Avalonia.Controls.TextBox();
            this.tbgroup = new Avalonia.Controls.TextBox();
            this.tbinstance2 = new Avalonia.Controls.TextBox();
            this.tbHex = new Avalonia.Controls.TextBox();
            this.tbDec = new Avalonia.Controls.TextBox();
            this.tbBin = new Avalonia.Controls.TextBox();
            this.tbFloat = new Avalonia.Controls.TextBox();
            this.cbtypes = new Avalonia.Controls.ComboBox();
            this.cbComp = new Avalonia.Controls.ComboBox();
            this.button1 = new Avalonia.Controls.Button();
            this.btcopie = new Avalonia.Controls.Button();
            this.linkLabel1 = new Avalonia.Controls.Button();
            this.pb = new Compat.PictureBoxCompat();
            this.label1 = new Avalonia.Controls.TextBlock();
            this.label2 = new Avalonia.Controls.TextBlock();
            this.label3 = new Avalonia.Controls.TextBlock();
            this.label4 = new Avalonia.Controls.TextBlock();
            this.label5 = new Avalonia.Controls.TextBlock();
            this.label6 = new Avalonia.Controls.TextBlock();
            this.label7 = new Avalonia.Controls.TextBlock();
            this.label8 = new Avalonia.Controls.TextBlock();
            this.label9 = new Avalonia.Controls.TextBlock();
            this.label10 = new Avalonia.Controls.TextBlock();
            this.label11 = new Avalonia.Controls.TextBlock();
            this.label12 = new Avalonia.Controls.TextBlock();
            this.label13 = new Avalonia.Controls.TextBlock();
            this.lbName = new Avalonia.Controls.TextBlock();
            this.lbAuthor = new Avalonia.Controls.TextBlock();
            this.lbVersion = new Avalonia.Controls.TextBlock();
            this.lbDesc = new Avalonia.Controls.TextBlock();
            this.lbComp = new Avalonia.Controls.TextBlock();
            this.tbHex.TextChanged += (s, e) => this.HexChanged(s, null);
            this.tbDec.TextChanged += (s, e) => this.DecChanged(s, null);
            this.tbBin.TextChanged += (s, e) => this.BinChanged(s, null);
            this.tbBin.SizeChanged += (s, e) => this.tbBin_SizeChanged(s, null);
            this.tbFloat.TextChanged += (s, e) => this.FloatChanged(s, null);
            this.tbtype.TextChanged += (s, e) => this.tbtype_TextChanged(s, null);
            this.tbgroup.TextChanged += (s, e) => this.TextChanged(s, null);
            this.tbinstance.TextChanged += (s, e) => this.TextChanged(s, null);
            this.tbinstance2.TextChanged += (s, e) => this.TextChanged(s, null);
            this.cbtypes.SelectionChanged += (s, e) => cbtypes_SelectedIndexChanged(s, e);
            this.cbComp.SelectionChanged += (s, e) => cbComp_SelectedIndexChanged(s, e);
            this.dcHex.VisibleChanged += new System.EventHandler(this.dcHex_VisibleChanged);
            // this.Load removed: Avalonia.Controls.Window does not have Load event
        }
        #endregion

        internal SimPe.Events.ResourceEventArgs items;
        internal LoadedPackage guipackage;

        private void ResourceDock_Load(object sender, System.EventArgs e)
        {

        }

        private void cbtypes_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cbtypes.Tag != null) return;
            tbtype.Text = "0x" + Helper.HexString(((SimPe.Data.TypeAlias)cbtypes.Items[cbtypes.SelectedIndex]).Id);
            this.tbtype.Tag = true;
            tbtype_TextChanged2(this.tbtype, e);
        }

        private void tbtype_TextChanged(object sender, System.EventArgs e)
        {
            cbtypes.Tag = true;
            Data.TypeAlias a = Data.MetaData.FindTypeAlias(Helper.HexStringToUInt(tbtype.Text));

            int ct = 0;
            foreach (Data.TypeAlias i in cbtypes.Items)
            {
                if (i == a)
                {
                    cbtypes.SelectedIndex = ct;
                    cbtypes.Tag = null;
                    return;
                }
                ct++;
            }

            cbtypes.SelectedIndex = -1;
            cbtypes.Tag = null;
            TextChanged(sender, null);
        }

        private void tbtype_TextChanged2(object sender, System.EventArgs ea)
        {
            if (items == null || ((TextBox)sender).Tag == null) return;
            ((TextBox)sender).Tag = null;
            guipackage.PauseIndexChangedEvents();
            foreach (SimPe.Events.ResourceContainer e in items)
            {
                if (!e.HasFileDescriptor) continue;
                try
                {
                    e.Resource.FileDescriptor.Type = Convert.ToUInt32(tbtype.Text, 16);

                    e.Resource.FileDescriptor.Changed = true;
                }
                catch { }
            }
            guipackage.PauseIndexChangedEvents();
            guipackage.RestartIndexChangedEvents();
        }

        private void tbgroup_TextChanged(object sender, System.EventArgs ea)
        {
            if (items == null || ((TextBox)sender).Tag == null) return;
            ((TextBox)sender).Tag = null;

            guipackage.PauseIndexChangedEvents();
            foreach (SimPe.Events.ResourceContainer e in items)
            {
                if (!e.HasFileDescriptor) continue;
                try
                {
                    e.Resource.FileDescriptor.Group = Convert.ToUInt32(tbgroup.Text, 16);

                    e.Resource.FileDescriptor.Changed = true;
                }
                catch { }
            }
            guipackage.PauseIndexChangedEvents();
            guipackage.RestartIndexChangedEvents();
        }

        private void tbinstance_TextChanged(object sender, System.EventArgs ea)
        {
            if (items == null || ((TextBox)sender).Tag == null) return;
            ((TextBox)sender).Tag = null;

            guipackage.PauseIndexChangedEvents();
            foreach (SimPe.Events.ResourceContainer e in items)
            {
                if (!e.HasFileDescriptor) continue;


                try
                {
                    e.Resource.FileDescriptor.Instance = Convert.ToUInt32(tbinstance.Text, 16);

                    e.Resource.FileDescriptor.Changed = true;
                }
                catch { }
            }

            guipackage.PauseIndexChangedEvents();
            guipackage.RestartIndexChangedEvents();

        }

        private void tbinstance2_TextChanged(object sender, System.EventArgs ea)
        {
            if (items == null || ((TextBox)sender).Tag == null) return;
            ((TextBox)sender).Tag = null;

            guipackage.PauseIndexChangedEvents();
            foreach (SimPe.Events.ResourceContainer e in items)
            {
                if (!e.HasFileDescriptor) continue;


                try
                {
                    e.Resource.FileDescriptor.SubType = Convert.ToUInt32(tbinstance2.Text, 16);
                    e.Resource.FileDescriptor.Changed = true;
                }
                catch { }
            }
            guipackage.PauseIndexChangedEvents();
            guipackage.RestartIndexChangedEvents();
        }


        private void cbComp_SelectedIndexChanged(object sender, System.EventArgs ea)
        {
            if (this.cbComp.SelectedIndex < 0) return;
            if (this.cbComp.SelectedIndex > 1) return;
            if (items == null) return;

            guipackage.PauseIndexChangedEvents();
            foreach (SimPe.Events.ResourceContainer e in items)
            {
                if (!e.HasFileDescriptor) continue;

                try
                {
                    e.Resource.FileDescriptor.MarkForReCompress = (cbComp.SelectedIndex == 1);
                    if (!e.Resource.FileDescriptor.MarkForReCompress && e.Resource.FileDescriptor.WasCompressed)
                    {
                        e.Resource.FileDescriptor.UserData = e.Resource.Package.Read(e.Resource.FileDescriptor).UncompressedData;
                    }
                    e.Resource.FileDescriptor.Changed = true;
                }
                catch { }
            }
            guipackage.PauseIndexChangedEvents();
            guipackage.RestartIndexChangedEvents();
        }

        private void tbtype_KeyUp(object sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Return)
            {
                TextChanged(sender, null);
                this.tbtype_TextChanged2(sender, null);
            }
        }

        private void tbgroup_KeyUp(object sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Return)
            {
                TextChanged(sender, null);
                this.tbgroup_TextChanged(sender, null);
            }
        }

        private void tbinstance_KeyUp(object sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Return)
            {
                TextChanged(sender, null);
                this.tbinstance_TextChanged(sender, null);
            }
        }

        private void tbinstance2_KeyUp(object sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Return)
            {
                TextChanged(sender, null);
                this.tbinstance2_TextChanged(sender, null);
            }
        }

        #region Hex <-> Dec Converter
        bool sysupdate = false;
        void SetConverted(object exclude, long val)
        {
            if (exclude != this.tbDec) this.tbDec.Text = val.ToString();
            if (exclude != this.tbHex) this.tbHex.Text = Helper.HexString(val);
            if (exclude != this.tbBin) this.tbBin.Text = Convert.ToString(val, 2); ;
            if (exclude != this.tbFloat) this.tbFloat.Text = BitConverter.ToSingle(BitConverter.GetBytes((int)val), 0).ToString();
        }
        void ClearConverted(object exclude)
        {
            if (exclude != this.tbDec) this.tbDec.Text = "";
            if (exclude != this.tbHex) this.tbHex.Text = "";
            if (exclude != this.tbBin) this.tbBin.Text = "";
            if (exclude != this.tbFloat) this.tbFloat.Text = "";
        }
        private void FloatChanged(object sender, System.EventArgs e)
        {
            if (sysupdate) return;
            sysupdate = true;
            try
            {
                float f = Convert.ToSingle(tbFloat.Text);
                long val = BitConverter.ToInt32(BitConverter.GetBytes(f), 0);
                SetConverted(this.tbFloat, val);
            }
            catch
            {
                ClearConverted(this.tbFloat);
            }
            sysupdate = false;
        }
        private void BinChanged(object sender, System.EventArgs e)
        {
            if (sysupdate) return;
            sysupdate = true;
            try
            {
                long val = Convert.ToInt64(tbBin.Text.Replace(" ", ""), 2);
                SetConverted(this.tbBin, val);
            }
            catch
            {
                ClearConverted(this.tbBin);
            }
            sysupdate = false;
        }
        private void HexChanged(object sender, System.EventArgs e)
        {
            if (sysupdate) return;
            sysupdate = true;
            try
            {
                long val = Convert.ToInt64(tbHex.Text.Replace(" ", ""), 16);
                SetConverted(this.tbHex, val);
            }
            catch
            {
                ClearConverted(this.tbHex);
            }
            sysupdate = false;
        }

        private void DecChanged(object sender, System.EventArgs e)
        {
            if (sysupdate) return;
            sysupdate = true;
            try
            {
                long val = Convert.ToInt64(tbDec.Text);
                SetConverted(this.tbDec, val);
            }
            catch (Exception)
            {
                ClearConverted(this.tbDec);
            }
            sysupdate = false;
        }
        #endregion

        internal SimPe.Interfaces.Files.IPackedFileDescriptor hexpfd;
        private new void TextChanged(object sender, System.EventArgs e)
        {
            if (items == null) return;
            ((TextBox)sender).Tag = true;
        }


        private void btcopie_Click(object sender, System.EventArgs e)
        {
            int i = 1;
            string s = "";
            string d;
            foreach (byte b in hvc.Data)
            {

                d = b.ToString("X");
                if (d.Length == 1) d = "0" + d;
                s += d;
                if (i == 24) { s += " \r\n"; i = 0; }
                else s += " ";
                i++;
            }
            // TODO: await TopLevel.GetTopLevel(this)?.Clipboard?.SetTextAsync(s);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            hexpfd.UserData = hvc.Data;
        }

        private void dcHex_VisibleChanged(object sender, System.EventArgs e)
        {
            this.hvc.IsVisible = dcHex.Visible;
            hvc.Refresh(true);
        }

        private void linkLabel1_LinkClicked(object sender, System.EventArgs ev)
        {
            if (items == null) return;
            guipackage.PauseIndexChangedEvents();
            foreach (SimPe.Events.ResourceContainer e in items)
            {
                if (!e.HasFileDescriptor) continue;
                try
                {
                    e.Resource.FileDescriptor.Type = Convert.ToUInt32(tbtype.Text, 16);
                    e.Resource.FileDescriptor.Group = Convert.ToUInt32(tbgroup.Text, 16);
                    e.Resource.FileDescriptor.Instance = Convert.ToUInt32(tbinstance.Text, 16);
                    e.Resource.FileDescriptor.SubType = Convert.ToUInt32(tbinstance2.Text, 16);
                    e.Resource.FileDescriptor.MarkForReCompress = (cbComp.SelectedIndex == 1 && !e.Resource.FileDescriptor.WasCompressed);

                    e.Resource.FileDescriptor.Changed = true;
                }
                catch { }
            }
            guipackage.RestartIndexChangedEvents();
        }

        private void tbBin_SizeChanged(object sender, System.EventArgs e)
        {
            tbFloat.Width = tbBin.Width;
        }
    }
}
