using UnityEngine;
using System.Collections.Generic;

public abstract class ListSO<T> : VariableSO<List<T>> 
{
    public T GetValue(int index)
    {
        return value[index];
    }
}