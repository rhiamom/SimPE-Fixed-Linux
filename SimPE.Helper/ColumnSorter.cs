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
using System.Collections;

namespace SimPe
{
	// Replaces System.Windows.Forms.SortOrder (same integer values).
	public enum SortOrder
	{
		None       = 0,
		Ascending  = 1,
		Descending = 2,
	}

	/// <summary>
	/// ListView Column Sorter
	/// </summary>
	public class ColumnSorter : IComparer
	{
		public ColumnSorter()
		{
			cc = 0;
			so = SortOrder.Ascending;
		}
		int cc;
		SortOrder so;

		/// <summary>
		/// The Currently active Column
		/// </summary>
		public int CurrentColumn
		{
			get { return cc; }
			set {
				if (cc != value)
				{
					cc = value;
					if (Changed != null) Changed(this, new EventArgs());
				}
			}
		}

		/// <summary>
		/// The Sort Order
		/// </summary>
		public SortOrder Sorting
		{
			get { return so; }
			set {
				if (so != value)
				{
					so = value;
					if (Changed != null) Changed(this, new EventArgs());
				}
			}
		}

		public event EventHandler Changed;

		/// <summary>
		/// The Compare Function to use
		/// </summary>
		/// <param name="x">first ListViewItem</param>
		/// <param name="y">second ListViewItem</param>
		/// <returns>0 if the items match</returns>
		public int Compare(object x, object y)
		{
			// Access SubItems via dynamic so this assembly has no System.Windows.Forms dependency.
			dynamic rowA = x;
			dynamic rowB = y;

			if (Sorting == SortOrder.Ascending)
			{
				return String.Compare(
					(string)rowA.SubItems[CurrentColumn].Text,
					(string)rowB.SubItems[CurrentColumn].Text);
			}
			else
			{
				return String.Compare(
					(string)rowB.SubItems[CurrentColumn].Text,
					(string)rowA.SubItems[CurrentColumn].Text);
			}
		}
	}

	/// <summary>
	/// ListView multi-column sorter
	/// </summary>
	public class ColumnsSorter : IComparer
	{
		int[] co;

		/// <summary>
		/// The Currently active Columns
		/// </summary>
		public int[] ColumnOrder
		{
			get { return co; }
			set { co = value; }
		}

		/// <summary>
		/// The Sort Order
		/// </summary>
		public SortOrder Sorting = SortOrder.Ascending;

		public ColumnsSorter() : this(new int[0]) { }

		public ColumnsSorter(int[] columns)
		{
			co = columns;
		}

		/// <summary>
		/// The Compare Function to use
		/// </summary>
		/// <param name="x">first ListViewItem</param>
		/// <param name="y">second ListViewItem</param>
		/// <returns>0 if the items match</returns>
		public int Compare(object x, object y)
		{
			// Access SubItems via dynamic so this assembly has no System.Windows.Forms dependency.
			dynamic rowA = x;
			dynamic rowB = y;

			if (Sorting != SortOrder.Ascending)
			{
				dynamic tmp = rowB;
				rowB = rowA;
				rowA = tmp;
			}

			for (int cc = 0; cc < co.Length; cc++)
			{
				int cmp = String.Compare(
					(string)rowA.SubItems[co[cc]].Text,
					(string)rowB.SubItems[co[cc]].Text);
				if (cmp != 0) return cmp;
			}

			return 0;
		}
	}
}
