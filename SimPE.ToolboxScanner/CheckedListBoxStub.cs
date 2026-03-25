// Local Avalonia replacement for System.Windows.Forms.CheckedListBox,
// which is not available on the Mac WinForms compatibility surface.
// API surface matches the subset used by ScannerForm.

using System.Collections;
using System.Collections.Generic;

namespace SimPe.Plugin
{
    internal sealed class CheckedListBox
    {
        public CheckedListBoxItemCollection Items { get; } = new CheckedListBoxItemCollection();

        public bool CheckOnClick       { get; set; }
        public bool HorizontalScrollbar { get; set; }
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size  Size     { get; set; }
        public string Name    { get; set; }
        public int    TabIndex { get; set; }
        public System.Windows.Forms.AnchorStyles Anchor { get; set; }

        public bool GetItemChecked(int index) => Items.GetItemChecked(index);
        public void SetItemChecked(int index, bool isChecked) => Items.SetItemChecked(index, isChecked);
    }

    internal sealed class CheckedListBoxItemCollection : IEnumerable
    {
        private readonly List<(object Item, bool Checked)> _list = new();

        public int Count => _list.Count;

        public object this[int i] => _list[i].Item;

        public void Add(object item, bool isChecked) => _list.Add((item, isChecked));
        public void Add(object item) => _list.Add((item, false));

        public void Insert(int index, object item) => _list.Insert(index, (item, false));

        public void Clear() => _list.Clear();

        public bool GetItemChecked(int i) => _list[i].Checked;

        public void SetItemChecked(int i, bool isChecked)
        {
            var entry = _list[i];
            _list[i] = (entry.Item, isChecked);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var (item, _) in _list)
                yield return item;
        }

        public bool Contains(object item)
        {
            foreach (var (it, _) in _list)
                if (Equals(it, item)) return true;
            return false;
        }
    }
}
