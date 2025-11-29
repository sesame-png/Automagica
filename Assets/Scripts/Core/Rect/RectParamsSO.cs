using UnityEngine;

[CreateAssetMenu(menuName = "Rect/Rect Params")]
public class RectParamsSO : ScriptableObject
{
    // Position
    public Vector2 defaultPosition { get { return _defaultPosition; } private set { _defaultPosition = value; } }
    [SerializeField] private Vector2 _defaultPosition = new Vector2(0, 0);

    public Vector2 cachedPosition = new Vector2(0, 0);

    public bool clampPosition { get { return _clampPosition; } private set { _clampPosition = value; } }
    [SerializeField] private bool _clampPosition = false;

    public Vector2 minPosition { get { return _minPosition; } private set { _minPosition = value; } }
    [SerializeField] private Vector2 _minPosition = new Vector2(-10000, -10000);

    public Vector2 maxPosition { get { return _maxPosition; } private set { _maxPosition = value; } }
    [SerializeField] private Vector2 _maxPosition = new Vector2(10000, 10000);

    // Size
    public Vector2 defaultSize { get { return _defaultSize; } private set { _defaultSize = value; } }
    [SerializeField] private Vector2 _defaultSize = new Vector2(500, 400);

    public Vector2 cachedSize = new Vector2(500, 400);

    public bool clampSize { get { return _clampSize; } private set { _clampSize = value; } } // Minimum size is ALWAYS considered, even when not clamped
    [SerializeField] private bool _clampSize = true;

    public Vector2 minSize { get { return _minSize; } private set { _minSize = value; } }
    [SerializeField] private Vector2 _minSize = new Vector2(300, 300);

    public Vector2 maxSize { get { return _maxSize; } private set { _maxSize = value; } }
    [SerializeField] private Vector2 _maxSize = new Vector2(10000, 10000);
}