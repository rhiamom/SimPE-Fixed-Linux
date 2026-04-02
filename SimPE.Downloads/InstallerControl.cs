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

using Ambertation.Windows.Forms;
using SimPe.Plugin.Downloads;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Avalonia.Controls;
using Avalonia.Input;
using SimPe.Scenegraph.Compat;
using DragEventArgs = Avalonia.Input.DragEventArgs;
using Label    = SimPe.Scenegraph.Compat.LabelCompat;
using ComboBox = Avalonia.Controls.ComboBox;
using TextBox  = SimPe.Scenegraph.Compat.TextBoxCompat;
using LinkLabel = SimPe.Scenegraph.Compat.LinkLabel;
using Image    = System.Drawing.Image;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for InstallerControl.
	/// </summary>
	public class InstallerControl : Avalonia.Controls.UserControl
	{
		private PanelCompat pndrop;
		private PictureBox pb;
        private XPTaskBoxSimple tbs;
        private ComboBox cb;
        private TextBox rtb;
        private Label lbCat;
        private Label label1;
        private Label lbFace;
        private Label label7;
        private Label lbVert;
        private Label label5;
        private Label lbPrice;
        private Label label3;
		private Label lbGuid;
		private Label label4;
		private Label label6;
		private Label lbType;
		private LinkLabel llOptions;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public InstallerControl()
		{
			InitializeComponent();
            Clear();
		}

		public void Dispose()
		{
			if (components != null)
			{
				components.Dispose();
			}
		}

		#region InitializeComponent
		private void InitializeComponent()
		{
            this.pndrop   = new PanelCompat();
            this.pb       = new PictureBox();
            this.tbs      = new XPTaskBoxSimple();
            this.lbType   = new Label();
            this.label6   = new Label();
            this.lbGuid   = new Label();
            this.label4   = new Label();
            this.lbVert   = new Label();
            this.label5   = new Label();
            this.lbCat    = new Label();
            this.label1   = new Label();
            this.rtb      = new TextBox { IsReadOnly = true };
            this.cb       = new ComboBox();
            this.lbPrice  = new Label();
            this.label3   = new Label();
            this.lbFace   = new Label();
            this.label7   = new Label();
            this.llOptions = new LinkLabel();

            this.cb.SelectionChanged += (s, e) => this.SelectedInfo(this, EventArgs.Empty);
            this.llOptions.LinkClicked += (s, e) => this.ShowOptions(s, e);

            pndrop.AddHandler(DragDrop.DropEvent,     new EventHandler<DragEventArgs>(this.DragDropFile));
            pndrop.AddHandler(DragDrop.DragOverEvent, new EventHandler<DragEventArgs>(this.DragEnterFile));
            DragDrop.SetAllowDrop(pndrop, true);

            label1.Text  = "Category:";
            label3.Text  = "Price:";
            label4.Text  = "GUID:";
            label5.Text  = "Verts:";
            label6.Text  = "Type:";
            label7.Text  = "Faces:";
            llOptions.Text = "Options";

            var infoGrid = new Grid();
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            int row = 0;
            void AddRow(Avalonia.Controls.Control lbl, Avalonia.Controls.Control val)
            {
                infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                Grid.SetRow(lbl, row); Grid.SetColumn(lbl, 0);
                Grid.SetRow(val, row); Grid.SetColumn(val, 1);
                infoGrid.Children.Add(lbl);
                infoGrid.Children.Add(val);
                row++;
            }
            AddRow(label1,  lbCat);
            AddRow(label3,  lbPrice);
            AddRow(label4,  lbGuid);
            AddRow(label5,  lbVert);
            AddRow(label6,  lbType);
            AddRow(label7,  lbFace);

            pndrop.Children.Add(pb);

            var mainStack = new StackPanel();
            mainStack.Children.Add(pndrop);
            mainStack.Children.Add(cb);
            mainStack.Children.Add(rtb);
            mainStack.Children.Add(infoGrid);
            mainStack.Children.Add(llOptions);

            this.Content = mainStack;
		}
		#endregion

		public static void Cleanup()
		{
			SimPe.Plugin.DownloadsToolFactory.TeleportFileIndex.CloseAssignedPackages();
			SimPe.Packages.StreamFactory.CleanupTeleport();
		}

        public void LoadFiles(string[] files)
        {
			Wait.Start(files.Length);
            foreach (Downloads.IPackageInfo nfo in cb.Items)
                nfo.Dispose();
            this.cb.Items.Clear();
			Cleanup();


			int ct = 0;
            foreach (string file in files)
            {
				Wait.Progress = ct++;
                Downloads.IPackageHandler hnd = Downloads.HandlerRegistry.Global.LoadFileHandler(file);
                if (hnd != null)
                {
                    foreach (Downloads.IPackageInfo nfo in hnd.Objects)
                        cb.Items.Add(nfo);
                }
				hnd.FreeResources();
            }
			if (cb.Items.Count > 0) cb.SelectedIndex = 0;

			Wait.Stop();
        }

		#region Drag&Drop

		/// <summary>
		/// Returns the Names of the Dropped Files
		/// </summary>
		string[] DragDropNames(DragEventArgs e)
		{
			var data = e.Data.Get(DataFormats.Files);
			if (data is System.Collections.Generic.IEnumerable<string> paths)
			{
				var list = new System.Collections.Generic.List<string>(paths);
				return list.ToArray();
			}
			return new string[0];
		}

		/// <summary>
		/// Returns the Effect that should be displayed based on the Files
		/// </summary>
		DragDropEffects DragDropEffect(string[] names)
		{
            foreach (string name in names)
			{
				if (Downloads.HandlerRegistry.Global.HasFileHandler(name))
					return DragDropEffects.Copy;
			}

			return DragDropEffects.None;
		}

		/// <summary>
		/// Someone tries to throw a File
		/// </summary>
		private void DragEnterFile(object sender, DragEventArgs e)
		{
			if (e.Data.Contains(DataFormats.Files))
			{
				try
				{
					e.DragEffects = DragDropEffect(DragDropNames(e));
				}
				catch (Exception)
				{
				}

			}
			else
			{
				e.DragEffects = DragDropEffects.None;
			}
		}

		/// <summary>
		/// A File has been dropped
		/// </summary>
		private void DragDropFile(object sender, DragEventArgs e)
		{
			try
			{
				string[] files = DragDropNames(e);
                LoadFiles(files);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(ex);
			}

		}
		#endregion

        protected void Clear()
        {
            pb.Image = null;
            this.tbs.HeaderText = "";
            this.rtb.Text = "";
            lbCat.Text = "";
            lbPrice.Text = "";
            lbVert.Text = "0";
            lbFace.Text = "0";
			lbGuid.Text = "";
			lbType.Text = "";
        }

        protected Downloads.IPackageInfo SelectedPackageInfo
        {
            get { return cb.SelectedItem as Downloads.IPackageInfo; }
        }
        private void SelectedInfo(object sender, EventArgs e)
        {
            SelectedInfo(SelectedPackageInfo);
        }

		private void ShowOptions(object sender, LinkLabelLinkClickedEventArgs e)
		{
			SimPe.RemoteControl.ShowCustomSettings(SimPe.Plugin.DownloadsToolFactory.Settings);
		}

        protected void SelectedInfo(Downloads.IPackageInfo nfo)
        {
            Clear();
            if (nfo != null)
            {
				nfo.CreatePostponed3DPreview();
                if (nfo.Image!=null) pb.Image = nfo.GetThumbnail();
				else pb.Image = PackageInfo.DefaultImage;
                tbs.HeaderText = nfo.Name;
                rtb.Text = nfo.Description;
                lbCat.Text = nfo.Category;
                lbPrice.Text = "$" + nfo.Price.ToString();
                lbVert.Text = nfo.VertexCount.ToString();
                lbFace.Text = nfo.FaceCount.ToString();
				lbGuid.Text = "";
				lbType.Text = nfo.Type.ToString();
				foreach (uint guid in nfo.Guids)
					lbGuid.Text += "0x"+Helper.HexString(guid)+" ";

				lbVert.ForeColor = Color.Black;
				if (nfo.HighVertexCount) lbVert.ForeColor = Color.Red;

				lbFace.ForeColor = Color.Black;
				if (nfo.HighFaceCount) lbFace.ForeColor = Color.Red;
            }
        }
	}
}
