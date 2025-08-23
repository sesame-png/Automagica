using UnityEngine;

public class ResizableRect : MonoBehaviour
{
    //variables
    [HideInInspector] public bool moveable = true;
    [HideInInspector] public bool resizable = true;

    //position & size
    public RectParametersSO rectParameters { get { return _rectParameters; } protected set { _rectParameters = value; } }
    private RectParametersSO _rectParameters;

    public Vector2 position { get { return _position; } protected set { _position = value; } }
    private Vector2 _position;

    public Vector2 size { get { return _size; } protected set { _size = value; } }
    private Vector2 _size;

    //components
    protected RectTransform rectTransform;



    protected void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }



    /// <summary>
    /// Transform Rect
    /// </summary>
    public virtual void MovePosition(Vector2 posDelta)
    {
        if (!moveable) { return; }

        SetPosition(position + posDelta);
    }

    public virtual void Resize(Vector2 inputDelta, int dirX, int dirY)
    {
        if (!resizable) { return; }

        Vector2 sizeDelta = new Vector2(inputDelta.x * dirX, inputDelta.y * dirY);
        Vector2 newSize = size + sizeDelta;

        Vector2 posDelta = Vector2.zero;
        if (newSize.x > rectParameters.minSize.x && newSize.x < rectParameters.maxSize.x)
        {
            posDelta.x = (inputDelta.x / 2) * Mathf.Abs(dirX);
        }
        else
        {
            posDelta.x = 0f;
        }
        if (newSize.y > rectParameters.minSize.y && newSize.y < rectParameters.maxSize.y)
        {
            posDelta.y = (inputDelta.y / 2) * Mathf.Abs(dirY);
        }
        else
        {
            posDelta.y = 0f;
        }
        Vector2 newPos = position + posDelta;

        SetSize(newSize);
        SetPosition(newPos);
    }

    public void SetPosition(Vector2 newPos)
    {
        position = new Vector2(Mathf.Clamp(newPos.x, rectParameters.minPosition.x, rectParameters.maxPosition.x), Mathf.Clamp(newPos.y, rectParameters.minPosition.y, rectParameters.maxPosition.y));
        transform.localPosition = position;
        rectParameters.cachedPosition = position;
    }

    public void SetSize(Vector2 newSize)
    {
        size = new Vector2(Mathf.Clamp(newSize.x, rectParameters.minSize.x, rectParameters.maxSize.x), Mathf.Clamp(newSize.y, rectParameters.minSize.y, rectParameters.maxSize.y));
        rectTransform.sizeDelta = size;
        rectParameters.cachedSize = size;
    }
}