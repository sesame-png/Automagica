using System;
using UnityEngine;

public abstract class VariableSO<T> : ScriptableObject
{
    [SerializeField] protected T Value;
    public event Action<T> OnValueChanged;

    public T GetValue()
    {
        return Value;
    }

    public void SetValue(T v)
    {
        Value = v;
        InvokeChangedEvent(v);
    }

    private void OnValidate()
    {
        InvokeChangedEvent(Value);
    }

    public void InvokeChangedEvent()
    {
        InvokeChangedEvent(Value);
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
        return "" + Value;
    }
}