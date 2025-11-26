using UnityEngine;
using System;

public abstract class VariableSO<T> : ScriptableObject
{
    [SerializeField] protected T value;
    public event Action<T> OnValueChanged;

    public T GetValue()
    {
        return value;
    }

    public void SetValue(T v)
    {
        value = v;
        InvokeChangedEvent(v);
    }

    private void OnValidate()
    {
        InvokeChangedEvent(value);
    }

    public void InvokeChangedEvent()
    {
        InvokeChangedEvent(value);
    }

    private void InvokeChangedEvent(T v)
    {
        OnValueChanged?.Invoke(v);
    }

    public void Subscribe(Action<T> function)
    {
        OnValueChanged += function;
    }

    public void Unsubscribe(Action<T> function)
    {
        OnValueChanged -= function;
    }

    public override string ToString()
    {
        return "" + value;
    }
}