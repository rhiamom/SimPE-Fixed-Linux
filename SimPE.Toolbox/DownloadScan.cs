/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
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
using Avalonia.Layout;
using SimPe.Scenegraph.Compat;
using ListView    = SimPe.Scenegraph.Compat.ListView;
using ColumnHeader = SimPe.Scenegraph.Compat.ColumnHeader;
using ImageList   = SimPe.Scenegraph.Compat.ImageList;
using GroupBox    = SimPe.Scenegraph.Compat.GroupBox;
using PictureBox  = SimPe.Scenegraph.Compat.PictureBox;
using LinkLabel   = SimPe.Scenegraph.Compat.LinkLabel;
using Label       = SimPe.Scenegraph.Compat.LabelCompat;
using TextBox     = SimPe.Scenegraph.Compat.TextBoxCompat;
using CheckBox    = Avalonia.Controls.CheckBox;
using ProgressBar = Avalonia.Controls.ProgressBar;
using ListViewItem = SimPe.Scenegraph.Compat.ListViewItem;
using LinkLabelLinkClickedEventArgs = SimPe.Scenegraph.Compat.LinkLabelLinkClickedEventArgs;
using ColumnClickEventArgs = SimPe.Scenegraph.Compat.ColumnClickEventArgs;
using Image = System.Drawing.Image;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for DownloadScan.
	/// </summary>
	public class DownloadScan : Avalonia.Controls.Window
	{
		const string STR_NOT_EP = "either original Maxis or not EP Ready";
		const string STR_COMP_DIR = "Compressed Dir incomplete";
		const string STR_COMP_SIZE = "Corrupted compressed Size";
		private ComboBox comboBox1;
		private GroupBox lbdir;
		private LinkLabel linkLabel1;
		private ListView lv;
		private ColumnHeader columnHeader1;
		private ColumnHeader columnHeader2;
		private ColumnHeader columnHeader3;
		private TextBox tbfilename;
		private LinkLabel llopen;
		private LinkLabel lldis;
		private ColumnHeader columnHeader4;
		private ProgressBar pb;
		private GroupBox groupBox1;
		private CheckBox cbcompress;
		private CheckBox cbguid;
		private ColumnHeader columnHeader5;
		private System.ComponentModel.IContainer components;
		private LinkLabel llfix;
		private Label label1;
		private TextBox tbname;
		private CheckBox cbready;
		private GroupBox groupBox2;
		private LinkLabel llskin;
		private CheckBox cbbaby;
		private CheckBox cbtoddler;
		private CheckBox cbchild;
		private CheckBox cbteen;
		private CheckBox cbyoung;
		private CheckBox cbadult;
		private CheckBox cbelder;
		private GroupBox gbskin;
		private ImageList iList;
		private CheckBox cbprev;
		private PictureBox pbprev;
		private CheckBox cbact;
		private CheckBox cbskin;
		private CheckBox cbformal;
		private CheckBox cbpreg;
		private CheckBox cbundies;
		private CheckBox cbpj;
		private CheckBox cbevery;
		private CheckBox cbswim;
		private LinkLabel llcomp;

		internal Interfaces.IProviderRegistry prov;
		public DownloadScan()
		{
			InitializeComponent();

			this.comboBox1.SelectedIndex = 0;
			Select(null, null);

			sorter = new ColumnSorter();

			if (Helper.XmlRegistry.Username.Trim()!="")
				this.tbname.Text = Helper.XmlRegistry.Username+"-";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected virtual void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
		}

		#region Avalonia layout
		private void InitializeComponent()
		{
			this.Title  = "Scan Folders";
			this.Width  = 810;
			this.Height = 600;

			// ── Folder combo + scan link ─────────────────────────────────────────
			this.comboBox1 = new ComboBox();
			this.comboBox1.Items.Add("Download Folder");
			this.comboBox1.Items.Add("Teleport Folder");
			this.comboBox1.Items.Add("...");
			this.comboBox1.SelectionChanged += (s, e) => Select(s, EventArgs.Empty);

			this.linkLabel1 = new LinkLabel { Text = "scan" };
			this.linkLabel1.LinkClicked += (s, e) => Scan(s, e);

			var topRow = new Avalonia.Controls.StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
			topRow.Children.Add(this.comboBox1);
			topRow.Children.Add(this.linkLabel1);

			// ── Options group ────────────────────────────────────────────────────
			this.cbcompress = new CheckBox { Content = "Compression check" };
			this.cbguid     = new CheckBox { Content = "Guid check" };
			this.cbready    = new CheckBox { Content = "EP Ready?", IsChecked = true };
			this.cbprev     = new CheckBox { Content = "Preview" };

			var optionsRow = new Avalonia.Controls.StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
			optionsRow.Children.Add(this.cbcompress);
			optionsRow.Children.Add(this.cbguid);
			optionsRow.Children.Add(this.cbready);
			optionsRow.Children.Add(this.cbprev);

			this.groupBox1 = new GroupBox { Text = "Options" };
			this.groupBox1.Content = optionsRow;

			// ── ListView columns ─────────────────────────────────────────────────
			this.columnHeader1 = new ColumnHeader { Text = "Package File", Width = 192 };
			this.columnHeader2 = new ColumnHeader { Text = "Content",      Width = 146 };
			this.columnHeader5 = new ColumnHeader { Text = "GUID",         Width = 75 };
			this.columnHeader4 = new ColumnHeader { Text = "Enabled" };
			this.columnHeader3 = new ColumnHeader { Text = "State",        Width = 89 };

			this.lv = new ListView();
			this.lv.FullRowSelect = true;
			this.lv.HideSelection = false;
			this.lv.View = SimPe.Scenegraph.Compat.View.Details;
			this.lv.Columns.AddRange(new ColumnHeader[] {
				this.columnHeader1,
				this.columnHeader2,
				this.columnHeader5,
				this.columnHeader4,
				this.columnHeader3 });
			this.lv.ColumnClick          += (s, e) => Sort(s, e);
			this.lv.SelectedIndexChanged += (s, e) => SelectPackage(s, e);

			// ── Right-side controls ──────────────────────────────────────────────
			this.tbfilename = new TextBox { IsReadOnly = true };

			this.llopen = new LinkLabel { Text = "open",    IsEnabled = false };
			this.lldis  = new LinkLabel { Text = "disable", IsEnabled = false };
			this.llcomp = new LinkLabel { Text = "fix Compression", IsEnabled = false };
			this.llopen.LinkClicked += (s, e) => Openpackage(s, e);
			this.lldis.LinkClicked  += (s, e) => Disable(s, e);
			this.llcomp.LinkClicked += (s, e) => FixCompression(s, e);

			var openRow = new Avalonia.Controls.StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
			openRow.Children.Add(this.llopen);
			openRow.Children.Add(this.lldis);

			// ── groupBox2: "make EP Ready" ───────────────────────────────────────
			this.llfix  = new LinkLabel { Text = "make EP Ready", IsEnabled = false };
			this.label1 = new Label { Text = "using Modelname" };
			this.tbname = new TextBox { Text = "SimPe-" };
			this.llfix.LinkClicked += (s, e) => Fix(s, e);

			var gb2Inner = new Avalonia.Controls.StackPanel { Orientation = Orientation.Vertical, Spacing = 2 };
			gb2Inner.Children.Add(this.llfix);
			gb2Inner.Children.Add(this.label1);
			gb2Inner.Children.Add(this.tbname);

			this.groupBox2 = new GroupBox { Text = "EP Ready" };
			this.groupBox2.Content = gb2Inner;

			// ── gbskin: age/category checkboxes ──────────────────────────────────
			this.llskin    = new LinkLabel { Text = "set Skin", IsEnabled = false };
			this.cbbaby    = new CheckBox { Content = "baby",       IsThreeState = true };
			this.cbtoddler = new CheckBox { Content = "toddler",    IsThreeState = true };
			this.cbchild   = new CheckBox { Content = "child",      IsThreeState = true };
			this.cbteen    = new CheckBox { Content = "teen",       IsThreeState = true };
			this.cbyoung   = new CheckBox { Content = "young Adult",IsThreeState = true };
			this.cbadult   = new CheckBox { Content = "Adult",      IsThreeState = true };
			this.cbelder   = new CheckBox { Content = "Elder",      IsThreeState = true };
			this.cbevery   = new CheckBox { Content = "Everyday",   IsThreeState = true };
			this.cbpj      = new CheckBox { Content = "PJ",         IsThreeState = true };
			this.cbundies  = new CheckBox { Content = "Undies",     IsThreeState = true };
			this.cbpreg    = new CheckBox { Content = "Preg.",      IsThreeState = true };
			this.cbswim    = new CheckBox { Content = "Swim.",      IsThreeState = true };
			this.cbformal  = new CheckBox { Content = "Formal",     IsThreeState = true };
			this.cbskin    = new CheckBox { Content = "Skin",       IsThreeState = true };
			this.cbact     = new CheckBox { Content = "Active",     IsThreeState = true };
			this.llskin.LinkClicked += (s, e) => SetSkinAge(s, e);

			var skinLeft = new Avalonia.Controls.StackPanel { Orientation = Orientation.Vertical, Spacing = 2 };
			skinLeft.Children.Add(this.cbbaby);
			skinLeft.Children.Add(this.cbtoddler);
			skinLeft.Children.Add(this.cbchild);
			skinLeft.Children.Add(this.cbteen);
			skinLeft.Children.Add(this.cbevery);
			skinLeft.Children.Add(this.cbpj);
			skinLeft.Children.Add(this.cbundies);
			skinLeft.Children.Add(this.cbpreg);

			var skinRight = new Avalonia.Controls.StackPanel { Orientation = Orientation.Vertical, Spacing = 2 };
			skinRight.Children.Add(this.cbyoung);
			skinRight.Children.Add(this.cbadult);
			skinRight.Children.Add(this.cbelder);
			skinRight.Children.Add(this.cbswim);
			skinRight.Children.Add(this.cbformal);
			skinRight.Children.Add(this.cbskin);
			skinRight.Children.Add(this.cbact);

			var skinCols = new Avalonia.Controls.StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
			skinCols.Children.Add(skinLeft);
			skinCols.Children.Add(skinRight);

			var skinInner = new Avalonia.Controls.StackPanel { Orientation = Orientation.Vertical, Spacing = 2 };
			skinInner.Children.Add(this.llskin);
			skinInner.Children.Add(skinCols);

			this.gbskin = new GroupBox { Text = "Skin", IsEnabled = false };
			this.gbskin.Content = skinInner;

			// ── PictureBox preview ───────────────────────────────────────────────
			this.pbprev = new PictureBox { Size = new System.Drawing.Size(184, 40) };

			// ── ImageList ─────────────────────────────────────────────────────────
			this.iList = new ImageList
			{
				ColorDepth = SimPe.Scenegraph.Compat.ColorDepth.Depth32Bit,
				ImageSize  = new System.Drawing.Size(48, 48)
			};

			// ── Right panel (stacked) ─────────────────────────────────────────────
			var rightPanel = new Avalonia.Controls.StackPanel { Orientation = Orientation.Vertical, Spacing = 4, Width = 200 };
			rightPanel.Children.Add(this.tbfilename);
			rightPanel.Children.Add(openRow);
			rightPanel.Children.Add(this.llcomp);
			rightPanel.Children.Add(this.groupBox2);
			rightPanel.Children.Add(this.gbskin);

			// ── Main content grid (lv left, rightPanel right) ─────────────────────
			var mainGrid = new Avalonia.Controls.Grid();
			mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(206) });
			Avalonia.Controls.Grid.SetColumn(this.lv, 0);
			Avalonia.Controls.Grid.SetColumn(rightPanel, 1);
			mainGrid.Children.Add(this.lv);
			mainGrid.Children.Add(rightPanel);

			// ── lbdir GroupBox wraps the main grid ────────────────────────────────
			this.lbdir = new GroupBox { Text = "---" };
			this.lbdir.Content = mainGrid;

			// ── Progress bar ─────────────────────────────────────────────────────
			this.pb = new ProgressBar { Minimum = 0, Maximum = 100, Value = 0 };

			// ── Root layout ───────────────────────────────────────────────────────
			var root = new Avalonia.Controls.Grid();
			root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			Avalonia.Controls.Grid.SetRow(topRow,       0);
			Avalonia.Controls.Grid.SetRow(this.groupBox1, 1);
			Avalonia.Controls.Grid.SetRow(this.lbdir,   2);
			Avalonia.Controls.Grid.SetRow(this.pb,      3);

			root.Children.Add(topRow);
			root.Children.Add(this.groupBox1);
			root.Children.Add(this.lbdir);
			root.Children.Add(this.pb);

			this.Content = root;
		}
		#endregion

		string filename = null;
		public string FileName
		{
			get { return filename; }
		}

		private async void Select(object sender, System.EventArgs e)
		{
			if (comboBox1.SelectedIndex==2)
			{
				var folders = await StorageProvider.OpenFolderPickerAsync(
					new Avalonia.Platform.Storage.FolderPickerOpenOptions { AllowMultiple = false });
				if (folders.Count > 0) lbdir.Text = folders[0].Path.LocalPath;
			}
			else if (comboBox1.SelectedIndex==1)
			{
                lbdir.Text = System.IO.Path.Combine(SimPe.PathProvider.SimSavegameFolder, "Teleport");
			}
			else
			{
                lbdir.Text = System.IO.Path.Combine(SimPe.PathProvider.SimSavegameFolder, "Downloads");
			}
		}

		/// <summary>
		/// returns true if the packages Compressed Directory is ok
		/// </summary>
		/// <param name="package"></param>
		/// <returns>0 if everything is ok, -1 if the List is incorrect, -2 if the uncompressed Sizes are wrong</returns>
		private int CheckCompressed(SimPe.Packages.File package)
		{
			bool incomplete = false;
			foreach (Interfaces.Files.IPackedFileDescriptor pfd in package.Index)
			{
				Interfaces.Files.IPackedFile file = package.Read(pfd);

				int nr = -1;
				if (package.FileListFile!=null) nr = package.FileListFile.FindFile(pfd);
				bool inlist = (nr!=-1);

				if (inlist)
				{
					byte[] uncdata = file.UncompressedData;
					if (package.FileListFile.Items[nr].UncompressedSize != uncdata.Length) return -2;
				}

				if (inlist != file.IsCompressed) incomplete = true;

			}

			if (incomplete) return -1;
			return 0;
		}

		/// <summary>
		/// returns the Guid of this Object (if found)
		/// </summary>
		/// <param name="package"></param>
		/// <returns>0 or a Guid</returns>
		private uint[] CheckGuid(SimPe.Packages.File package)
		{
			Interfaces.Files.IPackedFileDescriptor[] pfds = package.FindFiles(Data.MetaData.OBJD_FILE);
			uint[] res = new uint[pfds.Length];

			for (int i=0; i<res.Length; i++)
			{
				SimPe.PackedFiles.Wrapper.Objd objd = new SimPe.PackedFiles.Wrapper.Objd(null);
				objd.ProcessData(pfds[i], package);
				res[i] = objd.SimId;
			}
			return res;
		}

		protected void AddFiles(string[] files, string note, int count, int offset)
		{
			ArrayList guids = new ArrayList();
			ArrayList aguids = new ArrayList();
			if ((this.cbguid.IsChecked == true) && (this.prov!=null))
			{
				foreach(uint g in prov.OpcodeProvider.StoredMemories.Keys)
				{
					if (aguids.Contains(g)) guids.Add(g);
					else aguids.Add(g);
				}
			}

			foreach (string file in files)
			{
				double val = (offset++ * pb.Maximum) / count;
				if (val > pb.Value) pb.Value = val;
				string name = System.IO.Path.GetFileNameWithoutExtension(file);
				string desc = SimPe.Localization.Manager.GetString("Unknown");
				string state = "OK";
				uint[] guid = new uint[0];
				Image img = null;

				try
				{
					SimPe.Packages.File package = SimPe.Packages.File.LoadFromFile(file);

					//find Texture
					Interfaces.Files.IPackedFileDescriptor[] pfds = null;
					try
					{
						if (cbprev.IsChecked == true)
						{
							pfds = package.FindFiles(Data.MetaData.TXTR);
							if (pfds.Length>0)
							{
								SimPe.Plugin.Txtr txtr = new Txtr(null, false);
								txtr.ProcessData(pfds[0], package);

								SimPe.Plugin.ImageData id = (SimPe.Plugin.ImageData)txtr.Blocks[0];
								MipMap mm = id.GetLargestTexture(iList.ImageSize);
								if (mm!=null)
								{
									// mm.Texture is SKBitmap; img is System.Drawing.Image — skip preview
									ImageLoader.Preview(mm.Texture, iList.ImageSize);
								}
							}
						}//if
					}
					catch {}

					//find Name
					pfds = package.FindFiles(Data.MetaData.CTSS_FILE);
					if (pfds.Length==0) pfds = package.FindFiles(Data.MetaData.STRING_FILE);
					if (pfds.Length>0)
					{
						SimPe.PackedFiles.Wrapper.Str str = new SimPe.PackedFiles.Wrapper.Str();
						str.ProcessData(pfds[0], package);
						SimPe.PackedFiles.Wrapper.StrItemList items = str.FallbackedLanguageItems(Helper.XmlRegistry.LanguageCode);
						if (items!=null) if (items.Length>0) desc = items[0].Title;
					}
					else
					{
						//check if Recolor
						pfds = package.FindFiles(0x1C4A276C);	//TXTR
						if (pfds.Length>0)
						{
							pfds = package.FindFiles(0x49596978);	//TXMT
							if (pfds.Length>0)
							{
								pfds = package.FindFiles(0x4C697E5A);	//MMAT
								if (pfds.Length>0)
								{
									SimPe.PackedFiles.Wrapper.Cpf mmat = new SimPe.PackedFiles.Wrapper.Cpf();
									mmat.ProcessData(pfds[0], package);
									desc = "[recolor] "+mmat.GetSaveItem("modelName").StringValue;
								}
							}
						}
					}

					bool isskin = false;
					pfds = package.FindFiles(Data.MetaData.GZPS);
					foreach (Interfaces.Files.IPackedFileDescriptor pfd in pfds)
					{
						SimPe.PackedFiles.Wrapper.Cpf cpf = new SimPe.PackedFiles.Wrapper.Cpf();
						cpf.ProcessData(pfd, package);

						desc = "["+cpf.GetSaveItem("type").StringValue+"] " + desc;
						isskin = true;
					}

					if (this.cbcompress.IsChecked == true)
					{
						int ret = this.CheckCompressed(package);
						if (ret==-1) state = STR_COMP_DIR;
						if (ret==-2) state = STR_COMP_SIZE;
					}

					if ((this.cbready.IsChecked == true) && (!isskin))
					{
						if ((System.IO.Path.GetFileName(package.FileName).Trim().ToLower()!= System.IO.Path.GetFileName(ScenegraphHelper.MMAT_PACKAGE).Trim().ToLower()) && (System.IO.Path.GetFileName(package.FileName).Trim().ToLower()!= System.IO.Path.GetFileName(ScenegraphHelper.GMND_PACKAGE).Trim().ToLower()) && (package.FindFilesByGroup(Data.MetaData.CUSTOM_GROUP).Length==0))
						{
							pfds = package.FindFiles(0x1C4A276C);	//TXTR
							if (pfds.Length>0)
							{
								state = STR_NOT_EP;
							}
						}
					}

					if (this.cbguid.IsChecked == true)
					{
						guid = this.CheckGuid(package);
						foreach (uint g in guid)
						{
							if (g!=0)
							{
								if (guids.Contains(g))
								{
									if (state=="OK") state = "Duplicate GUID";
									else state = "Duplicate GUID, "+state;
								}
								else
								{
									guids.Add(g);
								}
							}
						}//foreach
					}
				}
				catch (Exception ex)
				{
					state = "Not loaded, "+ex.Message;
				}

				ListViewItem lvi = new ListViewItem();
				lvi.Text = name;
				lvi.SubItems.Add(desc);
				lvi.Tag = file;
				if (guid.Length>0)
				{
					string s = "";
					for (int i=0; i<guid.Length; i++)
					{
						if (i!=0) s+= ", ";
						s += "0x"+Helper.HexString(guid[i]);
					}
					lvi.SubItems.Add(s);
				} else lvi.SubItems.Add("");
				lvi.SubItems.Add(note);
				lvi.SubItems.Add(state);

				if (img!=null)
				{
					iList.Images.Add(img);
					lvi.ImageIndex = iList.Images.Count-1;
				}

				lv.Items.Add(lvi);
			}
		}

		private void Scan(object sender, LinkLabelLinkClickedEventArgs e)
		{
			lv.Items.Clear();
			iList.Images.Clear();
			lv.SmallImageList = null;
			if (cbprev.IsChecked == true) iList.ImageSize = new Size(48, 48);
			else iList.ImageSize = new Size(16, 16);
			lv.SmallImageList = iList;

			lv.ListViewItemSorter = null;
			tbfilename.Text = "";
			pb.Value = 0;
			if (!System.IO.Directory.Exists(lbdir.Text)) return;

			this.Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Wait);
			string[] files = System.IO.Directory.GetFiles(lbdir.Text, "*.package");
			string[] dis_files = System.IO.Directory.GetFiles(lbdir.Text, "*.simpedis");
            this.AddFiles(files, "yes", files.Length + dis_files.Length, 0);
			this.AddFiles(dis_files, "no", files.Length + dis_files.Length, files.Length);
			this.Cursor = Avalonia.Input.Cursor.Default;
            lv.ListViewItemSorter = sorter;
			pb.Value = 0;
		}

		private ColumnSorter sorter;
		private void Sort(object sender, System.EventArgs e)
		{
			if (((ListView)sender).ListViewItemSorter == null) return;
			sorter.CurrentColumn = (e as ColumnClickEventArgs)?.Column ?? 0;
			((ListView)sender).Sort();
		}

		void SetSkinBoxes(CheckBox cb, uint age, uint cmp)
		{
			if ((age & cmp) == cmp)
			{
				if (cb.IsChecked == false) cb.IsChecked = true;
			}
			else
			{
				if (cb.IsChecked == true) cb.IsChecked = null;
			}
		}

		void SetSkinBoxes(SimPe.PackedFiles.Wrapper.Cpf cpf)
		{
			uint age = cpf.GetSaveItem("age").UIntegerValue;
			uint cat = cpf.GetSaveItem("category").UIntegerValue;

			SetSkinBoxes(cbbaby, age, (uint)Data.Ages.Baby);
			SetSkinBoxes(cbtoddler, age, (uint)Data.Ages.Toddler);
			SetSkinBoxes(cbchild, age, (uint)Data.Ages.Child);
			SetSkinBoxes(cbteen, age, (uint)Data.Ages.Teen);
			SetSkinBoxes(cbyoung, age, (uint)Data.Ages.YoungAdult);
			SetSkinBoxes(cbadult, age, (uint)Data.Ages.Adult);
			SetSkinBoxes(cbelder, age, (uint)Data.Ages.Elder);

			SetSkinBoxes(cbact, cat, (uint)Data.SkinCategories.Activewear);
			SetSkinBoxes(cbevery, cat, (uint)Data.SkinCategories.Everyday);
			SetSkinBoxes(cbformal, cat, (uint)Data.SkinCategories.Formal);
			SetSkinBoxes(cbpj, cat, (uint)Data.SkinCategories.PJ);
			SetSkinBoxes(cbpreg, cat, (uint)Data.SkinCategories.Pregnant);
			SetSkinBoxes(cbskin, cat, (uint)Data.SkinCategories.Skin);
			SetSkinBoxes(cbswim, cat, (uint)Data.SkinCategories.Swimmwear);
			SetSkinBoxes(cbundies, cat, (uint)Data.SkinCategories.Undies);
		}

		void SetSkinBoxes(ListViewItem lvi)
		{
			if (lvi==null)
			{
				this.cbbaby.IsChecked    = false;
				this.cbtoddler.IsChecked = false;
				this.cbchild.IsChecked   = false;
				this.cbteen.IsChecked    = false;
				this.cbyoung.IsChecked   = false;
				this.cbadult.IsChecked   = false;
				this.cbelder.IsChecked   = false;

				cbact.IsChecked    = false;
			    cbevery.IsChecked  = false;
			    cbformal.IsChecked = false;
			    cbpj.IsChecked     = false;
			    cbpreg.IsChecked   = false;
			    cbskin.IsChecked   = false;
			    cbswim.IsChecked   = false;
			    cbundies.IsChecked = false;
			}
			else
			{
				SimPe.Packages.File skin = SimPe.Packages.File.LoadFromFile((string)lvi.Tag);
				SimPe.Interfaces.Files.IPackedFileDescriptor[] pfds = skin.FindFiles(Data.MetaData.GZPS);
				foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in pfds)
				{
					SimPe.PackedFiles.Wrapper.Cpf cpf = new SimPe.PackedFiles.Wrapper.Cpf();
					cpf.ProcessData(pfd, skin);

					if (cpf.GetSaveItem("type").StringValue == "skin") SetSkinBoxes(cpf);
				}
			}
		}

		delegate void ShowTextureDelegate(ListViewItem lvi);
		void ShowTexture(ListViewItem lvi)
		{
			if ((lvi==null) || (cbprev.IsChecked != true))
			{
				pbprev.Image = null;
			}
			else
			{
				SimPe.Packages.File skin = SimPe.Packages.File.LoadFromFile((string)lvi.Tag);
				SimPe.Interfaces.Files.IPackedFileDescriptor[] pfds = skin.FindFiles(Data.MetaData.TXTR);
				foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in pfds)
				{
					SimPe.Plugin.Txtr txtr = new Txtr(null, false);
					txtr.ProcessData(pfd, skin);
					SimPe.Plugin.ImageData id = (SimPe.Plugin.ImageData)txtr.Blocks[0];

					MipMap mm = id.GetLargestTexture(pbprev.Size);
					if (mm!=null)
					{
						// mm.Texture is SKBitmap; pbprev.Image is System.Drawing.Image — skip preview
						ImageLoader.Preview(mm.Texture, pbprev.Size);
					}
					break;
				}
			}
		}

		private void SelectPackage(object sender, System.EventArgs e)
		{
			tbfilename.Text = "";
			llopen.IsEnabled = false;
			lldis.IsEnabled  = false;
			llfix.IsEnabled  = false;
			llskin.IsEnabled = false;
			gbskin.IsEnabled = false;
			llcomp.IsEnabled = false;
			SetSkinBoxes((ListViewItem)null);

			if (lv.SelectedItems.Count==0) return;
			llopen.IsEnabled = lv.SelectedItems.Count==1;
			lldis.IsEnabled  = true;

			foreach (ListViewItem lvi in lv.SelectedItems)
			{
				if (lvi.SubItems[4].Text == STR_NOT_EP)
				{
					llfix.IsEnabled = true;
				}

				if ((lvi.SubItems[4].Text.IndexOf(STR_COMP_DIR)!=-1) || (lvi.SubItems[4].Text.IndexOf(STR_COMP_SIZE)!=-1))
				{
					llcomp.IsEnabled = true;
				}
			}

			if (Helper.XmlRegistry.HiddenMode) llfix.IsEnabled = true;

            bool oner = (lv.SelectedItems.Count==1);
			foreach (ListViewItem lvi in lv.SelectedItems)
			{
				if (lvi.SubItems[1].Text.StartsWith("[skin]"))
				{
					gbskin.IsEnabled = true;
					llskin.IsEnabled = true;
					SetSkinBoxes(lvi);
				}

				if ((lvi.SubItems[1].Text.StartsWith("[skin]")) || (lvi.SubItems[1].Text.StartsWith("[recolor]")))
				{
					if (oner)
					{
						object[] os = new object[1];
						os[0] = lvi;
						Avalonia.Threading.Dispatcher.UIThread.Post(() => ShowTexture((ListViewItem)os[0]));
					}
				}
			}

			tbfilename.Text = lv.SelectedItems[0].Text;

			if (System.IO.File.Exists(System.IO.Path.Combine(lbdir.Text, lv.SelectedItems[0].Text+".package"))) lldis.Text = "disable";
			else lldis.Text = "enable";
		}

		private void Openpackage(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (lv.SelectedItems.Count!=1) return;
			this.filename = System.IO.Path.Combine(lbdir.Text, lv.SelectedItems[0].Text+".package");
			if (!System.IO.File.Exists(filename)) filename = System.IO.Path.Combine(lbdir.Text, lv.SelectedItems[0].Text+".simpedis");

			if (!System.IO.File.Exists(filename)) filename = null;
			else Close();
		}

		private void Disable(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (lv.SelectedItems.Count==0) return;

			foreach (ListViewItem lvi in lv.SelectedItems)
			{
				string filename = System.IO.Path.Combine(lbdir.Text, lvi.Text+".package");
				string target = System.IO.Path.Combine(lbdir.Text, lvi.Text+".simpedis");

				SimPe.Packages.StreamItem si = SimPe.Packages.StreamFactory.GetStreamItem(filename, false);
				if (si!=null) si.Close();
				si = SimPe.Packages.StreamFactory.GetStreamItem(target, false);
				if (si!=null) si.Close();
				try
				{
					if (System.IO.File.Exists(filename))
					{
						System.IO.File.Move(filename, target);
						lvi.SubItems[3].Text = "no";
					}
					else
					{
						System.IO.File.Move(target, filename);
						lvi.SubItems[3].Text = "yes";
					}
				}
				catch (Exception ex){
					Helper.ExceptionMessage("", ex);
				}

			}

			SelectPackage(null, null);
		}

		private void Fix(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (lv.SelectedItems.Count==0) return;

			string mname = tbname.Text;
			DateTime now = DateTime.Now;
			mname += now.Date.ToShortDateString();
			mname += "-"+now.Hour.ToString("x");
			mname += now.Minute.ToString("x");
			mname += now.Second.ToString("x");
			mname += now.Millisecond.ToString("x");
			WaitingScreen.Wait();
			try
			{
				int count = lv.Items.Count;
				int pos = 0;
				foreach (ListViewItem lvi in lv.SelectedItems)
				{
					//only non EP Ready packages
					if ((!Helper.XmlRegistry.HiddenMode) && (lvi.SubItems[4].Text != STR_NOT_EP)) continue;

					string filename = System.IO.Path.Combine(lbdir.Text, lvi.Text+".package");
					pb.Value = (pos++ * pb.Maximum) / count;

					try
					{
                        SimPe.Plugin.FixPackage.Fix(filename, mname, FixVersion.UniversityReady2);
					}
					catch (Exception ex)
					{
						Helper.ExceptionMessage("", ex);
					}

				}
				pb.Value = 0;
			}
			finally
			{
				WaitingScreen.Stop();
			}
			SelectPackage(null, null);
		}

		void SetSkinAge(CheckBox cb, ref uint age, uint cmp)
		{
			if (cb.IsChecked == null) return;

			age |= cmp;
			if (cb.IsChecked == false) age ^= cmp;
		}

		private void FixCompression(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (lv.SelectedItems.Count<1) return;

			for (int i=0; i<lv.SelectedItems.Count; i++)
			{
				pb.Value = i * pb.Maximum / lv.SelectedItems.Count;
				ListViewItem lvi = lv.SelectedItems[i];
				if ((lvi.SubItems[4].Text.IndexOf(STR_COMP_DIR)!=-1) || (lvi.SubItems[4].Text.IndexOf(STR_COMP_SIZE)!=-1))
				{
					this.filename = System.IO.Path.Combine(lbdir.Text, lvi.Text+".package");
					if (!System.IO.File.Exists(filename)) filename = System.IO.Path.Combine(lbdir.Text, lvi.Text+".simpedis");

					if (System.IO.File.Exists(filename))
					{
						SimPe.Packages.GeneratableFile pkg = SimPe.Packages.GeneratableFile.LoadFromFile(filename);
						foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in pkg.Index)
						{
							SimPe.Interfaces.Files.IPackedFile file = pkg.Read(pfd);
							pfd.UserData = file.UncompressedData;

							pfd.MarkForReCompress = (file.IsCompressed || Data.MetaData.CompressionCandidates.Contains(pfd.Type));
						}

						pkg.Save();
					}
				}

			}
			pb.Value = 0;
		}

		void AddUniversityFields(SimPe.PackedFiles.Wrapper.Cpf cpf)
		{
			if (cpf.GetItem("product") == null)
			{
				SimPe.PackedFiles.Wrapper.CpfItem i = new SimPe.PackedFiles.Wrapper.CpfItem();
				i.Name = "product";
				i.UIntegerValue = 1;
				cpf.AddItem(i);
			}

			if (cpf.GetItem("version") == null)
			{
				SimPe.PackedFiles.Wrapper.CpfItem i = new SimPe.PackedFiles.Wrapper.CpfItem();
				i.Name = "version";
				i.UIntegerValue = 2;
				cpf.AddItem(i);
			}
		}

		bool hinted = false;
		private void SetSkinAge(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (lv.SelectedItems.Count==0) return;

			uint age = 0;
			uint cat = 0;

			try
			{
				int count = lv.SelectedItems.Count;
				int pos = 0;
				foreach (ListViewItem lvi in lv.SelectedItems)
				{
					this.pb.Value = (pos++ * pb.Maximum) / count;

					if (lvi.SubItems[1].Text.StartsWith("[skin]"))
					{
						string name = (string)lvi.Tag;
						SimPe.Packages.GeneratableFile skin = SimPe.Packages.GeneratableFile.LoadFromFile(name);

						SimPe.Interfaces.Files.IPackedFileDescriptor[] pfds = skin.FindFiles(Data.MetaData.GZPS);
						foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in pfds)
						{
							SimPe.PackedFiles.Wrapper.Cpf cpf = new SimPe.PackedFiles.Wrapper.Cpf();
							cpf.ProcessData(pfd, skin);

							if (cpf.GetSaveItem("type").StringValue == "skin")
							{
								age = cpf.GetSaveItem("age").UIntegerValue;
								cat = cpf.GetSaveItem("category").UIntegerValue;

								SetSkinAge(cbbaby, ref age, (uint)Data.Ages.Baby);
								SetSkinAge(cbtoddler, ref age, (uint)Data.Ages.Toddler);
								SetSkinAge(cbchild, ref age, (uint)Data.Ages.Child);
								SetSkinAge(cbteen, ref age, (uint)Data.Ages.Teen);
								SetSkinAge(cbyoung, ref age, (uint)((uint)Data.Ages.YoungAdult));
								if (cbyoung.IsChecked == true) AddUniversityFields(cpf);
								SetSkinAge(cbadult, ref age, (uint)Data.Ages.Adult);
								SetSkinAge(cbelder, ref age, (uint)Data.Ages.Elder);

								SetSkinAge(cbact, ref cat, (uint)Data.SkinCategories.Activewear);
								SetSkinAge(cbevery, ref cat, (uint)Data.SkinCategories.Everyday);
								SetSkinAge(cbformal, ref cat, (uint)Data.SkinCategories.Formal);
								SetSkinAge(cbpj, ref cat, (uint)Data.SkinCategories.PJ);
								SetSkinAge(cbpreg, ref cat, (uint)Data.SkinCategories.Pregnant);
								SetSkinAge(cbskin, ref cat, (uint)Data.SkinCategories.Skin);
								SetSkinAge(cbswim, ref cat, (uint)Data.SkinCategories.Swimmwear);
								SetSkinAge(cbundies, ref cat, (uint)Data.SkinCategories.Undies);

								cpf.GetSaveItem("age").UIntegerValue = age;
								cpf.GetSaveItem("category").UIntegerValue = cat;

								cpf.SynchronizeUserData();
							}
						}

						skin.Save();
					}

				}
				pb.Value = 0;
			}
			finally
			{
				WaitingScreen.Stop();
			}

			if (!hinted)
			{
				hinted = true;
				SimPe.Scenegraph.Compat.MessageBox.Show("If your Game should crash after converting a Skin, pleas scan the Folder again with the 'check Compression' Option checked.\nIf it reports any Errors, please fix them using 'fix Compression'");
			}
			SelectPackage(null, null);
		}
	}
}
