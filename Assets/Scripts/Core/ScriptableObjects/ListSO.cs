using System.Collections.Generic;
using UnityEngine;

public abstract class ListSO<T> : VariableSO<List<T>> 
{
    public T GetValue(int index)
    {
        return Value[index];
    }

    public void AddValue(T v)
    {
        Value.Add(v);
        InvokeChangedEvent();
    }
}