// Shims replacing SteepValley XPCommonControls with standard WinForms equivalents.
// XPListView types are preserved in their original namespace so existing code compiles unchanged.
using System.Windows.Forms;

namespace SteepValley.Windows.Forms
{
    /// <summary>
    /// Replaces XPListViewItem — adds integer GroupIndex backed by the item's
    /// ListViewGroup.Name key (set to the index string when groups are created).
    /// </summary>
    public class XPListViewItem : ListViewItem
    {
        private int _pendingGroupIndex = -1;

        public int GroupIndex
        {
            get
            {
                if (Group != null && int.TryParse(Group.Name, out int i))
                    return i;
                return _pendingGroupIndex;
            }
            set
            {
                _pendingGroupIndex = value;
                if (ListView != null)
                    Group = ListView.Groups[value.ToString()];
            }
        }

        public XPListViewItem() : base() { }
        public XPListViewItem(string text) : base(text) { }
    }

    /// <summary>Replaces XPListView — standard ListView with extra shim properties.</summary>
    public class XPListView : ListView
    {
        public XPListView() : base() { }

        public XPListView(System.ComponentModel.IContainer container) : base()
        {
            container.Add(this);
        }

        /// <summary>XPListView tile column widths — no-op in standard ListView.</summary>
        public int[] TileColumns { get; set; } = System.Array.Empty<int>();

        /// <summary>XPListView double-buffering toggle.</summary>
        public bool DoubleBuffering
        {
            get => GetStyle(ControlStyles.OptimizedDoubleBuffer);
            set => SetStyle(ControlStyles.OptimizedDoubleBuffer, value);
        }

        /// <summary>XPListView column style — no-op in standard ListView.</summary>
        public void SetColumnStyle(int column, System.Drawing.Font font, System.Drawing.Color color) { }
    }

    /// <summary>
    /// Replaces ExtendedView enum — maps to standard View values.
    /// </summary>
    public static class ExtendedView
    {
        public static View List => View.List;
        public static View Details => View.Details;
        public static View Tile => View.Tile;
        public static View LargeIcon => View.LargeIcon;
        public static View SmallIcon => View.SmallIcon;
    }
}
