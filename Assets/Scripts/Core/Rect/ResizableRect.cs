using UnityEngine;

public class ResizableRect : MonoBehaviour
{
    /// Variables
    [HideInInspector] public bool isMoveable = true;
    [HideInInspector] public bool isResizable = true;

    /// Position & Size
    public RectParamsSO rectParams { get { return _rectParams; } protected set { _rectParams = value; } }
    private RectParamsSO _rectParams;

    public Vector2 position { get { return _position; } protected set { _position = value; } }
    private Vector2 _position;

    public Vector2 size { get { return _size; } protected set { _size = value; } }
    private Vector2 _size;

    /// Components
    protected RectTransform rectTransform;



    /// Initialization
    protected void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }



    /// Transform Rect
    public virtual void MovePosition(Vector2 posDelta)
    {
        if (!isMoveable) { return; }

        SetPosition(position + posDelta);
    }

    public virtual void Resize(Vector2 inputDelta, int dirX, int dirY)
    {
        if (!isResizable) { return; }

        Vector2 sizeDelta = new Vector2(inputDelta.x * dirX, inputDelta.y * dirY);
        Vector2 newSize = size + sizeDelta;

        Vector2 posDelta = Vector2.zero;
        if (newSize.x > rectParams.minSize.x && newSize.x < rectParams.maxSize.x)
        {
            posDelta.x = (inputDelta.x / 2) * Mathf.Abs(dirX);
        }
        if (newSize.y > rectParams.minSize.y && newSize.y < rectParams.maxSize.y)
        {
            posDelta.y = (inputDelta.y / 2) * Mathf.Abs(dirY);
        }
        Vector2 newPos = position + posDelta;

        SetSize(newSize);
        SetPosition(newPos);
    }

    public void SetPosition(Vector2 newPos)
    {
        position = new Vector2(Mathf.Clamp(newPos.x, rectParams.minPosition.x, rectParams.maxPosition.x), Mathf.Clamp(newPos.y, rectParams.minPosition.y, rectParams.maxPosition.y));
        transform.localPosition = position;
        rectParams.cachedPosition = position;
    }

    public void SetSize(Vector2 newSize)
    {
        size = new Vector2(Mathf.Clamp(newSize.x, rectParams.minSize.x, rectParams.maxSize.x), Mathf.Clamp(newSize.y, rectParams.minSize.y, rectParams.maxSize.y));
        rectTransform.sizeDelta = size;
        rectParams.cachedSize = size;
    }
}