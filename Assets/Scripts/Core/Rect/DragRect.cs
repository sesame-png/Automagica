using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class DragRect : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public UnityEvent<Vector2> BeginDrag;
    public UnityEvent<Vector2> Dragged;
    public UnityEvent<Vector2> EndDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDrag.Invoke(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Dragged.Invoke(eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag.Invoke(eventData.position);
    }
}