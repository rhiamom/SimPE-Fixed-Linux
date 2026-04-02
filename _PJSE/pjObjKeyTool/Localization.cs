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
            { "pjObjKeyHelp",  "PJ ObjKey Tool" },
            { "pjCObjKeyTool", @"PJSE\ObjKey Tool" },
            { "missing3IDR",   "The 3IDR file for this CPF resource has disappeared!" },
            { "missingCPF",    "The CPF resource for this 3IDR has disappeared!" },
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
