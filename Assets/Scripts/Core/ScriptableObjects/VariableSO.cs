using UnityEngine;

public abstract class VariableSO<T> : ScriptableObject
{
    [SerializeField] protected T value;

    protected void SetValue(T v)
    {
        value = v;
    }

    public T GetValue()
    {
        return value;
    }
}