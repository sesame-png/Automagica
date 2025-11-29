using UnityEngine;

public class ResizableRect : MonoBehaviour
{
    // Variables
    [HideInInspector] public bool isMoveable = true;
    [HideInInspector] public bool isResizable = true;

    // Position & Size
    public RectParamsSO rectParams { get { return _rectParams; } protected set { _rectParams = value; } }
    private RectParamsSO _rectParams;

    public Vector2 position { get { return _position; } protected set { _position = value; } }
    private Vector2 _position;

    public Vector2 size { get { return _size; } protected set { _size = value; } }
    private Vector2 _size;

    // Components
    protected RectTransform rectTransform;



    // Initialization
    protected void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }



    // Transform Rect
    public virtual void MovePosition(Vector2 posDelta)
    {
        if (!isMoveable) { return; }

        SetPosition(_position + posDelta);
    }

    public virtual void Resize(Vector2 inputDelta, int dirX, int dirY)
    {
        if (!isResizable) { return; }

        Vector2 sizeDelta = new Vector2(inputDelta.x * dirX, inputDelta.y * dirY);
        Vector2 newSize = _size + sizeDelta;

        Vector2 posDelta = Vector2.zero;
        if (newSize.x > _rectParams.minSize.x && newSize.x < _rectParams.maxSize.x)
        {
            posDelta.x = (inputDelta.x / 2) * Mathf.Abs(dirX);
        }
        if (newSize.y > _rectParams.minSize.y && newSize.y < _rectParams.maxSize.y)
        {
            posDelta.y = (inputDelta.y / 2) * Mathf.Abs(dirY);
        }
        Vector2 newPos = _position + posDelta;

        SetSize(newSize);
        SetPosition(newPos);
    }



    // Setters
    public void SetPosition(Vector2 newPos)
    {
        if (_rectParams.clampPosition)
        {
            _position = new Vector2(Mathf.Clamp(newPos.x, _rectParams.minPosition.x, _rectParams.maxPosition.x), Mathf.Clamp(newPos.y, _rectParams.minPosition.y, _rectParams.maxPosition.y));
        }
        else
        {
            _position = newPos;
        }

        transform.localPosition = _position;
        _rectParams.cachedPosition = _position;
    }

    public void SetSize(Vector2 newSize)
    {
        if (_rectParams.clampSize)
        {
            _size = new Vector2(Mathf.Clamp(newSize.x, _rectParams.minSize.x, _rectParams.maxSize.x), Mathf.Clamp(newSize.y, _rectParams.minSize.y, _rectParams.maxSize.y));
        }
        else
        {
            _size = new Vector2(Mathf.Max(newSize.x, _rectParams.minSize.x), Mathf.Max(newSize.y, _rectParams.minSize.y));
        }
        
        rectTransform.sizeDelta = _size;
        _rectParams.cachedSize = _size;
    }
}