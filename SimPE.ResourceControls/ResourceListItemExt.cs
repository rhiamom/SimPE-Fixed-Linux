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
using System.Collections.Generic;

namespace SimPe.Windows.Forms
{
    // Plain data class used as DataGrid row item in ResourceListViewExt.
    // The DataGrid uses the Col* computed properties via binding.
    public class ResourceListItemExt
    {
        // ── Minimal SubItems collection ──────────────────────────────────────
        public class SubItem
        {
            public string Text { get; set; }
            public SubItem(string text) { Text = text; }
        }

        public class SubItemCollection
        {
            private readonly List<SubItem> _items = new List<SubItem>();
            public SubItem this[int index] => _items[index];
            public void Add(string text) => _items.Add(new SubItem(text));
            public void Clear() => _items.Clear();
            public int Count => _items.Count;
        }

        // ── Item properties ───────────────────────────────────────────────────
        public string Text { get; set; } = "";
        public SubItemCollection SubItems { get; } = new SubItemCollection();
        public int ImageIndex { get; set; } = -1;
        public bool Selected { get; set; }

        // Row style hints — used by ChangeDescription, available for future DataGrid styling
        public enum RowStyle { Normal, Deleted, Compressed, Changed }
        public RowStyle Style { get; private set; } = RowStyle.Normal;

        bool vis;
        NamedPackedFileDescriptor pfd;

        internal ResourceListItemExt(NamedPackedFileDescriptor pfd, ResourceViewManager manager, bool visible)
        {
            this.vis = visible;
            this.pfd = pfd;

            string[] subitems = new string[7];
            subitems[0] = visible ? pfd.GetRealName() : pfd.Descriptor.ToResListString(); // Name
            subitems[1] = GetExtText(); // Type
            subitems[2] = "0x" + Helper.HexString(pfd.Descriptor.Group); // Group
            subitems[3] = "0x" + Helper.HexString(pfd.Descriptor.SubType); // InstHi

            // Inst
            if (Helper.XmlRegistry.ResourceListInstanceFormatHexOnly)
                subitems[4] = "0x" + Helper.HexString(pfd.Descriptor.Instance);
            else if (Helper.XmlRegistry.ResourceListInstanceFormatDecOnly)
                subitems[4] = ((int)pfd.Descriptor.Instance).ToString();
            else
                subitems[4] = "0x" + Helper.HexString(pfd.Descriptor.Instance) + " (" + ((int)pfd.Descriptor.Instance).ToString() + ")";

            subitems[5] = "0x" + Helper.HexString(pfd.Descriptor.Offset);
            subitems[6] = "0x" + Helper.HexString(pfd.Descriptor.Size);

            this.SubItems.Clear();
            this.Text = subitems[0];
            for (int i = 1; i < subitems.Length; i++)
                SubItems.Add(subitems[i]);

            this.ImageIndex = ResourceViewManager.GetIndexForResourceType(pfd.Descriptor.Type);

            ChangeDescription(true);
        }

        // Public accessor for the underlying NamedPackedFileDescriptor
        public NamedPackedFileDescriptor Descriptor { get { return pfd; } }

        // Computed properties for DataGrid column binding
        public string ColName   { get { return vis ? pfd.GetRealName() : pfd.Descriptor.ToResListString(); } }
        public string ColType   { get { return GetExtText(); } }
        public string ColGroup  { get { return "0x" + Helper.HexString(pfd.Descriptor.Group); } }
        public string ColInstHi { get { return "0x" + Helper.HexString(pfd.Descriptor.SubType); } }
        public string ColInst   { get { return GetInstText(); } }
        public string ColOffset { get { return "0x" + Helper.HexString(pfd.Descriptor.Offset); } }
        public string ColSize   { get { return "0x" + Helper.HexString(pfd.Descriptor.Size); } }

        string GetInstText()
        {
            if (Helper.XmlRegistry.ResourceListInstanceFormatHexOnly)
                return "0x" + Helper.HexString(pfd.Descriptor.Instance);
            if (Helper.XmlRegistry.ResourceListInstanceFormatDecOnly)
                return ((int)pfd.Descriptor.Instance).ToString();
            return "0x" + Helper.HexString(pfd.Descriptor.Instance) + " (" + ((int)pfd.Descriptor.Instance).ToString() + ")";
        }

        string GetExtText()
        {
            if (Helper.XmlRegistry.ResourceListExtensionFormat == Registry.ResourceListExtensionFormats.Short)
                return pfd.Descriptor.TypeName.shortname;
            if (Helper.XmlRegistry.ResourceListExtensionFormat == Registry.ResourceListExtensionFormats.Long)
                return pfd.Descriptor.TypeName.Name;
            if (Helper.XmlRegistry.ResourceListExtensionFormat == Registry.ResourceListExtensionFormats.Hex)
                return "0x" + Helper.HexString(pfd.Descriptor.Type);
            return "";
        }

        internal bool Visible
        {
            get { return vis; }
            set
            {
                if (vis != value)
                {
                    vis = value;
                    if (vis) ChangeDescription(false);
                }
            }
        }

        internal void FreeResources() { }

        void ChangeDescription(bool justfont)
        {
            if (!justfont)
            {
                pfd.ResetRealName();
                this.Text = vis ? pfd.GetRealName() : pfd.Descriptor.ToResListString();

                if (Helper.XmlRegistry.ResourceListShowExtensions) this.SubItems[1].Text = GetExtText();
                this.SubItems[2].Text = "0x" + Helper.HexString(pfd.Descriptor.Group);
                this.SubItems[3].Text = "0x" + Helper.HexString(pfd.Descriptor.SubType);
                if (Helper.XmlRegistry.ResourceListInstanceFormatHexOnly)
                    this.SubItems[4].Text = "0x" + Helper.HexString(pfd.Descriptor.Instance);
                else if (Helper.XmlRegistry.ResourceListInstanceFormatDecOnly)
                    this.SubItems[4].Text = ((int)pfd.Descriptor.Instance).ToString();
                else
                    this.SubItems[4].Text = "0x" + Helper.HexString(pfd.Descriptor.Instance) + " (" + ((int)pfd.Descriptor.Instance).ToString() + ")";
                this.SubItems[5].Text = "0x" + Helper.HexString(pfd.Descriptor.Offset);
                this.SubItems[6].Text = "0x" + Helper.HexString(pfd.Descriptor.Size);
            }

            if (pfd.Descriptor.MarkForDelete)
                Style = RowStyle.Deleted;
            else if (pfd.Descriptor.MarkForReCompress)
                Style = RowStyle.Compressed;
            else if (pfd.Descriptor.Changed)
                Style = RowStyle.Changed;
            else
                Style = RowStyle.Normal;
        }
    }
}
