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

namespace SimPe.Plugin.Tool.Dockable
{
	/// <summary>
	/// Summary description for PreviewForm.
	/// </summary>
	public class PreviewForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		//Ambertation.Graphics.DirectXPanel dx = null;

		public PreviewForm()
		{
			//
			// Required designer variable.
			//
			InitializeComponent();

			//dx.Settings.AddAxis = false;
			//dx.LoadSettings(Helper.SimPeViewportFile);
			//dx.ResetDevice += new EventHandler(dx_ResetDevice);
            
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() { }
		#endregion

		static void Exception()
		{
			throw new SimPe.Warning("This Item can't be previewed!", "SimPe was unable to build the Scenegraph.");
		}

		public static Ambertation.Scenes.Scene BuildScene(MmatWrapper mmat, SimPe.Interfaces.Files.IPackageFile package)
		{
			SimPe.Interfaces.Scenegraph.IScenegraphFileIndex fii;
			Ambertation.Scenes.Scene scn = BuildScene(out fii, mmat, package);
			fii.Clear();
			return scn;
		}		

		public static Ambertation.Scenes.Scene BuildScene(out SimPe.Interfaces.Scenegraph.IScenegraphFileIndex fii, MmatWrapper mmat, SimPe.Interfaces.Files.IPackageFile package)
		{
			SimPe.Interfaces.Files.IPackageFile npkg;			
			Ambertation.Scenes.Scene scn = BuildScene(out fii, mmat, package, out npkg);

			if (npkg!=null) 
			{
				npkg.Close();
				if (npkg is SimPe.Packages.GeneratableFile)
					((SimPe.Packages.GeneratableFile)npkg).Dispose();
			}
			npkg = null;

			return scn;
		}

		public static Ambertation.Scenes.Scene BuildScene(SimPe.Interfaces.Scenegraph.IScenegraphFileIndex fii, MmatWrapper mmat, SimPe.Interfaces.Files.IPackageFile package)
		{
			SimPe.Interfaces.Files.IPackageFile npkg;
			Ambertation.Scenes.Scene scn = BuildScene(fii, mmat, package, out npkg);

			if (npkg!=null) 
			{
				npkg.Close();
				if (npkg is SimPe.Packages.GeneratableFile)
					((SimPe.Packages.GeneratableFile)npkg).Dispose();
			}
			npkg = null;

			return scn;
		}
		
		public static Ambertation.Scenes.Scene BuildScene(out SimPe.Interfaces.Scenegraph.IScenegraphFileIndex fii, MmatWrapper mmat, SimPe.Interfaces.Files.IPackageFile package, out SimPe.Interfaces.Files.IPackageFile npkg)
		{
			npkg = null;
			Wait.Start();
			fii = FileTable.FileIndex.AddNewChild();			
			try 
			{				
				return BuildScene(fii, mmat, package, out npkg);											
			}
			catch (System.IO.FileNotFoundException)
			{
				Wait.Stop();
                /* DirectX not supported on Mac — message suppressed */
                return null;
			}
			
			finally 
			{				
				FileTable.FileIndex.RemoveChild(fii);				
				Wait.Stop();
            }
		}

		public static Ambertation.Scenes.Scene BuildScene(SimPe.Interfaces.Scenegraph.IScenegraphFileIndex fii, MmatWrapper mmat, SimPe.Interfaces.Files.IPackageFile package, out SimPe.Interfaces.Files.IPackageFile npkg)
		{
			npkg = null;
            try
            {
                FileTable.FileIndex.Load();
                if (System.IO.File.Exists(package.SaveFileName))
                    fii.AddIndexFromFolder(System.IO.Path.GetDirectoryName(package.SaveFileName));

                npkg = SimPe.Plugin.Tool.Dockable.ObjectWorkshopHelper.CreatCloneByCres(mmat.ModelName);
                try
                {
                    foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in package.Index)
                    {
                        SimPe.Interfaces.Files.IPackedFileDescriptor npfd = pfd.Clone();
                        npfd.UserData = package.Read(pfd).UncompressedData;
                        if (pfd == mmat.FileDescriptor)
                            mmat.ProcessData(npfd, npkg);

                        npkg.Add(npfd, true);
                    }

                    fii.AddIndexFromPackage(npkg, true);
                    //fii.WriteContentToConsole();

                    return RenderScene(mmat);
                }
                finally
                {

                }
            }
            catch (System.IO.FileNotFoundException)
            {
                Wait.Stop();
                /* DirectX not supported on Mac — message suppressed */
                return null;
            }
		}

		public static Ambertation.Scenes.Scene RenderScene(MmatWrapper mmat)
		{			
			try 
			{
                try 
				{			
					GenericRcol rcol = mmat.GMDC;
					if (rcol!=null)
					{
						GeometryDataContainerExt gmdcext = new GeometryDataContainerExt(rcol.Blocks[0] as GeometryDataContainer);	
						gmdcext.UserTxmtMap[mmat.SubsetName] = mmat.TXMT;
						gmdcext.UserTxtrMap[mmat.SubsetName] = mmat.TXTR;
						Ambertation.Scenes.Scene scene = gmdcext.GetScene(new SimPe.Plugin.Gmdc.ElementOrder(Gmdc.ElementSorting.Preview));

						return scene;
					}
					else Exception();
				} 
				finally 
				{
					
				}
			}
			catch (System.IO.FileNotFoundException)
			{
				Wait.Stop();
                /* DirectX not supported on Mac — message suppressed */
                return null;
			}
			return null;
		}

		Ambertation.Scenes.Scene scene;
        //static Ambertation.Panel3D p3d;
        public static void Execute(SimPe.PackedFiles.Wrapper.Cpf cmmat, SimPe.Interfaces.Files.IPackageFile package)
        {
            if (!(cmmat is MmatWrapper)) return;

            // Managed DirectX preview is not supported in MacSimPE / cross-platform builds.
            // The original implementation opened a DirectX-based PreviewForm and rendered a scene.
            // That stack depended on old Microsoft Managed DirectX, which we are not using.
            return;
        }

        private void dx_ResetDevice(object sender, EventArgs e)
		{
			//nothing to clear and reset
		}
	}
}
