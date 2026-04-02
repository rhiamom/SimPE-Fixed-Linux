/***************************************************************************
 *   Copyright (C) 2006 by Peter L Jones                                   *
 *   peter@users.sf.net                                                    *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   Rhiamom@mac.com                                                       *
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
using System.Collections.Generic;

namespace pj
{
    public class L
    {
        private static readonly Dictionary<string, string> strings = new Dictionary<string, string>
        {
            { "badMeshPackage", "Could not use this package.  Either zero or multiple CRES or SHPE resources were found." },
            { "didNotOpen", "Chosen package did not open." },
            { "multipleMeshes", "Multiple meshes found (did you pick the right package?).\nImport resources for them all?" },
            { "noCRESSHPE", "3IDR file has incorrect format" },
            { "noGZPSXMOL", "No Property Set or Mesh Overlay XML files in package." },
            { "noMeshName", "No mesh name entered." },
            { "noMeshPkg", "No meshes in package." },
            { "notAllPartsFound", "Not all parts could be found for mesh: " },
            { "pjBMTExtract", @"PJSE\Body Mesh Tool\Extracting stage" },
            { "pjBMTHelp", "PJ Body Mesh Tool" },
            { "pjBMTLink", @"PJSE\Body Mesh Tool\Linking stage" },
            { "pjSME", "Sim Mesh Extractor" },
            { "pjSML", "Sim Mesh Linker" },
            { "pjSMLbegin", "After beginning linking, you will be asked to close your 3IDR resource without committing; so if you have changes to save, please click Cancel and do so first.   Otherwise click OK to browse to your mesh file." },
            { "pjSMLdone", "Now close your 3IDR resource, without committing, by opening a different resource.  After that you can continue as normal.   Remember to save." },
            { "pkgFilter", "Package file|*.package|All files|*.*" },
            { "selectPkgMesh", "Select Custom Mesh Package" },
            { "selectPkgTexture", "Select Exported Texture Package" },
        };

        public static string Get(string name)
        {
            if (strings.TryGetValue(name, out string res)) return res;
#if DEBUG
            return "<<" + name + ">>";
#else
            return name;
#endif
        }
    }
}
