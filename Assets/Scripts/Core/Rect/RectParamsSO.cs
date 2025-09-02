using UnityEngine;

[CreateAssetMenu(menuName = "Rect/Rect Parameters")]
public class RectParamsSO : ScriptableObject
{
    /// Position
    public Vector2 defaultPosition { get { return _defaultPosition; } private set { _defaultPosition = value; } }
    [SerializeField] private Vector2 _defaultPosition = new Vector2(0, 0);

    public Vector2 cachedPosition = new Vector2(0, 0);

    public bool constrainPosition { get { return _constrainPosition; } private set { _constrainPosition = value; } }
    [SerializeField] private bool _constrainPosition = false;

    public Vector2 minPosition { get { return _minPosition; } private set { _minPosition = value; } }
    [SerializeField] private Vector2 _minPosition = new Vector2(-10000, -10000);

    public Vector2 maxPosition { get { return _maxPosition; } private set { _maxPosition = value; } }
    [SerializeField] private Vector2 _maxPosition = new Vector2(10000, 10000);

    /// Size
    public Vector2 defaultSize { get { return _defaultSize; } private set { _defaultSize = value; } }
    [SerializeField] private Vector2 _defaultSize = new Vector2(500, 400);

    public Vector2 cachedSize = new Vector2(500, 400);

    public bool constrainSize { get { return _constrainSize; } private set { _constrainSize = value; } }
    [SerializeField] private bool _constrainSize = true;

    public Vector2 minSize { get { return _minSize; } private set { _minSize = value; } }
    [SerializeField] private Vector2 _minSize = new Vector2(300, 300);

    public Vector2 maxSize { get { return _maxSize; } private set { _maxSize = value; } }
    [SerializeField] private Vector2 _maxSize = new Vector2(10000, 10000);
}