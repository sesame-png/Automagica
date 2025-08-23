using UnityEngine;
using UnityEngine.EventSystems;

public class RectHandle : MonoBehaviour, IDragHandler
{
    [SerializeField] private ResizableRect rect;
    [SerializeField] private int directionX;
    [SerializeField] private int directionY;

    public void OnDrag(PointerEventData eventData)
    {
        rect.Resize(eventData.delta, directionX, directionY);
    }
}