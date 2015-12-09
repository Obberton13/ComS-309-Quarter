using UnityEngine;
using System.Collections.Generic;

public class PQueue<T> where T : IPriority{

	public int Count { get { return items.Count; } }

	//using a list is probably horribly inefficient, but oh well.
	private List<T> items = new List<T>();

	//uses the items "getPriority()" method to figure out where to put this item.
	public void Enqueue(T item)
	{
		int i;
		for(i = 0; i < items.Count; i++)
		{
			if(item.getPriority() < items[i].getPriority())
			{
				items.Insert(i, item);
				Debug.Log(item);
				return;
			}
		}
		items.Add(item);
	}

	public T Dequeue()
	{
		if (items.Count == 0) return default(T);
		T item = items[0];
		items.RemoveAt(0);
		return item;
	}
}
