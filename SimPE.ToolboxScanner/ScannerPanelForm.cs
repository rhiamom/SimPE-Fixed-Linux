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

namespace SimPe.Plugin.Scanner
{
	/// <summary>
	/// Singleton data holder for scanner operation control panels.
	/// Never displayed as a window — panels are returned by scanner
	/// OperationControl properties and inserted into ScannerForm.pnop.
	/// </summary>
	internal class ScannerPanelForm
	{
		internal Avalonia.Controls.StackPanel pncloth;
		internal Avalonia.Controls.StackPanel pnep;
		internal Avalonia.Controls.StackPanel pnskin;
		internal Avalonia.Controls.StackPanel pnShelve;

		internal Ambertation.Windows.Forms.EnumComboBox cbshelve;
		internal Avalonia.Controls.Button llShelve;

		internal Avalonia.Controls.CheckBox[] cbages     = new Avalonia.Controls.CheckBox[7];
		internal Avalonia.Controls.CheckBox[] cbsexes    = new Avalonia.Controls.CheckBox[2];
		internal Avalonia.Controls.CheckBox[] cbcategories = new Avalonia.Controls.CheckBox[9];

		private Avalonia.Controls.CheckBox cbbaby;
		private Avalonia.Controls.CheckBox cbtoddler;
		private Avalonia.Controls.CheckBox cbchild;
		private Avalonia.Controls.CheckBox cbteen;
		private Avalonia.Controls.CheckBox cbyoung;
		private Avalonia.Controls.CheckBox cbadult;
		private Avalonia.Controls.CheckBox cbelder;

		private Avalonia.Controls.CheckBox cbact;
		private Avalonia.Controls.CheckBox cbevery;
		private Avalonia.Controls.CheckBox cbformal;
		private Avalonia.Controls.CheckBox cbpj;
		private Avalonia.Controls.CheckBox cbpreg;
		private Avalonia.Controls.CheckBox cbskin;
		private Avalonia.Controls.CheckBox cbswim;
		private Avalonia.Controls.CheckBox cbundies;
		private Avalonia.Controls.CheckBox cbout;

		private Avalonia.Controls.CheckBox cbmale;
		private Avalonia.Controls.CheckBox cbfemale;

		private Avalonia.Controls.TextBox tbname;

		private Avalonia.Controls.CheckBox cbtxtr;
		private Avalonia.Controls.CheckBox cbtxmt;
		private Avalonia.Controls.CheckBox cbref;
		private Avalonia.Controls.ComboBox cbskins;

		private SaveFileDialogStub sfd = new SaveFileDialogStub();

		public ScannerPanelForm()
		{
			BuildControls();

			cbages[0] = cbbaby;    cbbaby.Tag    = Data.Ages.Baby;
			cbages[1] = cbtoddler; cbtoddler.Tag = Data.Ages.Toddler;
			cbages[2] = cbchild;   cbchild.Tag   = Data.Ages.Child;
			cbages[3] = cbteen;    cbteen.Tag     = Data.Ages.Teen;
			cbages[4] = cbyoung;   cbyoung.Tag   = Data.Ages.YoungAdult;
			cbages[5] = cbadult;   cbadult.Tag   = Data.Ages.Adult;
			cbages[6] = cbelder;   cbelder.Tag   = Data.Ages.Elder;

			cbcategories[0] = cbact;    cbact.Tag    = Data.OutfitCats.Gym;
			cbcategories[1] = cbevery;  cbevery.Tag  = Data.OutfitCats.Everyday;
			cbcategories[2] = cbformal; cbformal.Tag = Data.OutfitCats.Formal;
			cbcategories[3] = cbpj;     cbpj.Tag     = Data.OutfitCats.Pyjamas;
			cbcategories[4] = cbpreg;   cbpreg.Tag   = Data.OutfitCats.Maternity;
			cbcategories[5] = cbskin;   cbskin.Tag   = Data.OutfitCats.Skin;
			cbcategories[6] = cbswim;   cbswim.Tag   = Data.OutfitCats.Swimsuit;
			cbcategories[7] = cbundies; cbundies.Tag = Data.OutfitCats.Underwear;
			cbcategories[8] = cbout;    cbout.Tag    = Data.OutfitCats.WinterWear;

			cbsexes[0] = cbmale;   cbmale.Tag   = Data.Sex.Male;
			cbsexes[1] = cbfemale; cbfemale.Tag = Data.Sex.Female;

			if (Helper.XmlRegistry.Username.Trim() != "")
				this.tbname.Text = Helper.XmlRegistry.Username + "-";

			this.cbskins.SelectedIndex = 0;
			sfd.Filter = "Package File (*.package)|*.package|All Files (*.*)|*.*";
			sfd.Title = "Skin Override";
			sfd.InitialDirectory = PathProvider.SimSavegameFolder;

			cbshelve.Enum = typeof(SimPe.PackedFiles.Wrapper.ShelveDimension);
			cbshelve.ResourceManager = SimPe.Localization.Manager;
		}

		private void BuildControls()
		{
			// Age checkboxes
			cbbaby    = new Avalonia.Controls.CheckBox { Content = "Baby" };
			cbtoddler = new Avalonia.Controls.CheckBox { Content = "Toddler" };
			cbchild   = new Avalonia.Controls.CheckBox { Content = "Child" };
			cbteen    = new Avalonia.Controls.CheckBox { Content = "Teenager" };
			cbyoung   = new Avalonia.Controls.CheckBox { Content = "young Adult" };
			cbadult   = new Avalonia.Controls.CheckBox { Content = "Adult" };
			cbelder   = new Avalonia.Controls.CheckBox { Content = "Elder" };

			// Category checkboxes
			cbact    = new Avalonia.Controls.CheckBox { Content = "Gym" };
			cbevery  = new Avalonia.Controls.CheckBox { Content = "Everyday" };
			cbformal = new Avalonia.Controls.CheckBox { Content = "Formal" };
			cbpj     = new Avalonia.Controls.CheckBox { Content = "Pyjamas" };
			cbpreg   = new Avalonia.Controls.CheckBox { Content = "Maternity" };
			cbskin   = new Avalonia.Controls.CheckBox { Content = "Skin" };
			cbswim   = new Avalonia.Controls.CheckBox { Content = "Swim Suit" };
			cbundies = new Avalonia.Controls.CheckBox { Content = "Underwear" };
			cbout    = new Avalonia.Controls.CheckBox { Content = "Winter Wear" };

			// Sex checkboxes
			cbmale   = new Avalonia.Controls.CheckBox { Content = "Male" };
			cbfemale = new Avalonia.Controls.CheckBox { Content = "Female" };

			// Clothes panel
			var setAgeBtn = new Avalonia.Controls.Button { Content = "set age" };
			setAgeBtn.Click += (s, e) => setAge(s, EventArgs.Empty);
			var setCatBtn = new Avalonia.Controls.Button { Content = "set category" };
			setCatBtn.Click += (s, e) => SetCat(s, EventArgs.Empty);
			var setSexBtn = new Avalonia.Controls.Button { Content = "set gender" };
			setSexBtn.Click += (s, e) => setSex(s, EventArgs.Empty);

			pncloth = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Vertical };
			pncloth.Children.Add(new Avalonia.Controls.TextBlock { Text = "Ages:" });
			pncloth.Children.Add(cbbaby);
			pncloth.Children.Add(cbtoddler);
			pncloth.Children.Add(cbchild);
			pncloth.Children.Add(cbteen);
			pncloth.Children.Add(cbyoung);
			pncloth.Children.Add(cbadult);
			pncloth.Children.Add(cbelder);
			pncloth.Children.Add(setAgeBtn);
			pncloth.Children.Add(new Avalonia.Controls.TextBlock { Text = "Categories:" });
			pncloth.Children.Add(cbact);
			pncloth.Children.Add(cbevery);
			pncloth.Children.Add(cbformal);
			pncloth.Children.Add(cbpj);
			pncloth.Children.Add(cbpreg);
			pncloth.Children.Add(cbskin);
			pncloth.Children.Add(cbswim);
			pncloth.Children.Add(cbundies);
			pncloth.Children.Add(cbout);
			pncloth.Children.Add(setCatBtn);
			pncloth.Children.Add(new Avalonia.Controls.TextBlock { Text = "Gender:" });
			pncloth.Children.Add(cbmale);
			pncloth.Children.Add(cbfemale);
			pncloth.Children.Add(setSexBtn);

			// EP-Ready panel
			tbname = new Avalonia.Controls.TextBox { Text = "SimPe-" };
			var makeEPReadyBtn = new Avalonia.Controls.Button { Content = "make University-Ready" };
			makeEPReadyBtn.Click += (s, e) => MakeEPReady(s, EventArgs.Empty);
			pnep = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Vertical };
			pnep.Children.Add(new Avalonia.Controls.TextBlock { Text = "Name Prefix:" });
			pnep.Children.Add(tbname);
			pnep.Children.Add(makeEPReadyBtn);

			// Skin panel
			cbtxtr  = new Avalonia.Controls.CheckBox { Content = "override TXTR", IsChecked = true };
			cbtxmt  = new Avalonia.Controls.CheckBox { Content = "override TXMT", IsChecked = true };
			cbref   = new Avalonia.Controls.CheckBox { Content = "override Reference" };
			cbskins = new Avalonia.Controls.ComboBox();
			cbskins.Items.Add("Light");
			cbskins.Items.Add("Normal");
			cbskins.Items.Add("Medium");
			cbskins.Items.Add("Dark");
			cbskins.Items.Add("Alien");
			cbskins.Items.Add("Zombie");
			cbskins.Items.Add("Mannequin");
			cbskins.Items.Add("CAS Mannequin");
			cbskins.Items.Add("Vampire");
			var createOverrideBtn = new Avalonia.Controls.Button { Content = "create default Skin override" };
			createOverrideBtn.Click += (s, e) => CreateSkinOverride(s, EventArgs.Empty);
			pnskin = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Vertical };
			pnskin.Children.Add(new Avalonia.Controls.TextBlock { Text = "Base Skin:" });
			pnskin.Children.Add(cbskins);
			pnskin.Children.Add(cbtxmt);
			pnskin.Children.Add(cbtxtr);
			pnskin.Children.Add(cbref);
			pnskin.Children.Add(createOverrideBtn);

			// Shelve panel
			cbshelve = new Ambertation.Windows.Forms.EnumComboBox();
			cbshelve.SelectionChanged += (s, e) => cbshelve_SelectedIndexChanged(s, EventArgs.Empty);
			llShelve = new Avalonia.Controls.Button { Content = "set Shelve Dimension" };
			llShelve.Click += (s, e) => visualStyleLinkLabel3_LinkClicked(s, EventArgs.Empty);
			pnShelve = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Vertical };
			pnShelve.Children.Add(new Avalonia.Controls.TextBlock { Text = "Dimension:" });
			pnShelve.Children.Add(cbshelve);
			pnShelve.Children.Add(llShelve);
		}

		static ScannerPanelForm form;
		public static ScannerPanelForm Form
		{
			get
			{
				if (form == null) form = new ScannerPanelForm();
				return form;
			}
		}

		private void SetCat(object sender, EventArgs e)
		{
			ClothingScanner cs = (ClothingScanner)pncloth.Tag;
			cs.SetCategory();
		}

		private void setAge(object sender, EventArgs e)
		{
			ClothingScanner cs = (ClothingScanner)pncloth.Tag;
			cs.SetAge();
		}

		private void setSex(object sender, EventArgs e)
		{
			ClothingScanner cs = (ClothingScanner)pncloth.Tag;
			cs.SetSex();
		}

		private void MakeEPReady(object sender, EventArgs e)
		{
			EPReadyScanner cs = (EPReadyScanner)pnep.Tag;
			cs.Fix(this.tbname.Text);
		}

		private void CreateSkinOverride(object sender, EventArgs e)
		{
			if (!cbtxtr.IsChecked.GetValueOrDefault() && !cbtxmt.IsChecked.GetValueOrDefault() && !cbref.IsChecked.GetValueOrDefault())
			{
				MessageBoxStub.Show("Please select at least one Checkbox!");
				return;
			}

			if (sfd.ShowDialog() == DialogResult.OK)
			{
				string skintone = "";
				string family = "";
				if (cbskins.SelectedIndex < 4) skintone = "0000000" + (cbskins.SelectedIndex + 1).ToString() + "-0000-0000-0000-000000000000";
				else if (cbskins.SelectedIndex == 4) skintone = "6baf064a-85ad-4e37-8d81-a987e9f8da46";
				else if (cbskins.SelectedIndex == 5) skintone = "b6ee1dbc-5bb3-4146-8315-02bd64eda707";
				else if (cbskins.SelectedIndex == 6) skintone = "b9a94827-7544-450c-a8f4-6f643ae89a71";
				else if (cbskins.SelectedIndex == 7) skintone = "6eea47c7-8a35-4be7-9242-dcd082f53b55";
				else if (cbskins.SelectedIndex == 8) skintone = "00000000-0000-0000-0000-000000000000";

				if (cbskins.SelectedIndex < 4) family = "21afb87c-e872-4f4c-af3c-c3685ed4e220";
				else if (cbskins.SelectedIndex == 4) family = "ad5da337-bdd1-4593-acdd-19001595cbbb";
				else if (cbskins.SelectedIndex == 5) family = "b6ee1dbc-5bb3-4146-8315-02bd64eda707";
				else if (cbskins.SelectedIndex == 6) family = "59621330-1005-4b88-b4f2-77deb751fcf3";
				else if (cbskins.SelectedIndex == 7) family = "59621330-1005-4b88-b4f2-77deb751fcf3";
				else if (cbskins.SelectedIndex == 8) family = "13ae91e7-b825-4559-82a3-0ead8e8dd7fd";

				SkinScanner cs = (SkinScanner)pnskin.Tag;
				cs.CreateOverride(skintone, family, sfd.FileName, cbtxmt.IsChecked.GetValueOrDefault(), cbtxtr.IsChecked.GetValueOrDefault(), cbref.IsChecked.GetValueOrDefault());
			}
		}

		private void visualStyleLinkLabel3_LinkClicked(object sender, EventArgs e)
		{
			SimPe.PackedFiles.Wrapper.ShelveDimension sd = (SimPe.PackedFiles.Wrapper.ShelveDimension)cbshelve.SelectedValue;
			ShelveScanner cs = (ShelveScanner)this.pnShelve.Tag;
			cs.Set(sd);
		}

		private void cbshelve_SelectedIndexChanged(object sender, EventArgs e)
		{
			SimPe.PackedFiles.Wrapper.ShelveDimension sd = (SimPe.PackedFiles.Wrapper.ShelveDimension)cbshelve.SelectedValue;
			llShelve.IsEnabled = (sd != SimPe.PackedFiles.Wrapper.ShelveDimension.Indetermined
				&& sd != SimPe.PackedFiles.Wrapper.ShelveDimension.Multitile
				&& sd != SimPe.PackedFiles.Wrapper.ShelveDimension.Unknown1
				&& sd != SimPe.PackedFiles.Wrapper.ShelveDimension.Unknown2);
		}
	}
}
