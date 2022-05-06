using System;
using System.Collections.Generic;

namespace MRCR.datastructures;

public class UniqueList<T> : List<T>
{
    public int this[T index] => IndexOf(index);

    public new int Add(T i)
    {
        if(Contains(i)) throw new Exception("Duplicate item");
        int emptyIndex = IndexOf(default(T)!);
        if (emptyIndex != -1)
        {
            this[emptyIndex] = i;
            return emptyIndex;
        }
        base.Add(i);
        return Count - 1;
    }

    public new void AddRange(IEnumerable<T> collection)
    {
        foreach (var i in collection)
        {
            if (Contains(i)) throw new Exception("Duplicate item");
        }

        base.AddRange(collection);
    }

    public new void Remove(T i)
    {
        if(i == null) throw new Exception("Null item");
        int index = IndexOf(i);
        if (index == -1) throw new Exception("Item not found");
        base[index] = default(T)!;
    }

    public new void RemoveAt(int index)
    {
        base[index] = default(T)!;
    }
    
    public new void RemoveRange(int index, int count)
    {
        for (int i = 0; i < count; i++)
        {
            base[index + i] = default(T)!;
        }
    }
}