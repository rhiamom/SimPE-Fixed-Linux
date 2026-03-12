using System;
using System.Collections;

namespace Ambertation.Collections;

public class StringCollection : IDisposable, IEnumerable
{
	private ArrayList list;

	public int Count => list.Count;

	public string this[int index]
	{
		get
		{
			return (string)list[index];
		}
		set
		{
			list[index] = value;
		}
	}

	public StringCollection()
	{
		list = new ArrayList();
	}

	public void Add(string pd)
	{
		list.Add(pd);
	}

	public bool Contains(string pd)
	{
		return list.Contains(pd);
	}

	public void Remove(string pd)
	{
		list.Remove(pd);
	}

	public void Clear()
	{
		list.Clear();
	}

	public virtual void Dispose()
	{
		if (list != null)
		{
			list.Clear();
		}
		list = null;
	}

	public IEnumerator GetEnumerator()
	{
		return list.GetEnumerator();
	}

	public override string ToString()
	{
		return GetType().Name + " (" + Count + ")";
	}
}
