using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

public class ContextActions : MonoBehaviour
{
    public delegate void Delegate();
    public Delegate attack;

    //public SerializedDictionary<string, UnityEvent> actions;
}