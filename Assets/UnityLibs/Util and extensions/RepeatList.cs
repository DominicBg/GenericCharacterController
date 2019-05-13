using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RepeatList<T>
{
    [SerializeField] List<T> list = new List<T>();
    int index = -1;

    public void Add(T item)
    {
        list.Add(item);
    }
    public bool IsEmpty()
    {
        return list.Count == 0;
    }

    public T GetNext()
    {
        index++;
        return GetItem();
    }

    public T GetPrevious()
    {
        index++;
        return GetItem();
    }

    T GetItem()
    {
        index = (int)Mathf.Repeat(index, list.Count);
        T item = list[index];
        return item;
    }

    public void Shuffle()
    {
        list.Shuffle();
    }
}