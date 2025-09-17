using UnityEngine;

[CreateAssetMenu(menuName = "Apps/App")]
public class AppSO : ScriptableObject
{
    [SerializeField] public string appName;

    public RectParamsSO rectParams { get { return _rectParams; } private set { _rectParams = value; } }
    [SerializeField] private RectParamsSO _rectParams;

    public GameObject windowPrefab { get { return _windowPrefab; } private set { _windowPrefab = value; } }
    [SerializeField] private GameObject _windowPrefab;

    public Sprite icon { get { return _icon; } private set { _icon = value; } }
    [SerializeField] private Sprite _icon;

    public bool allowMultipleInstances { get { return _allowMultipleInstances; } private set { _allowMultipleInstances = value; } }
    [SerializeField] private bool _allowMultipleInstances = false;

    public bool allowMaximize { get { return _allowMaximize; } private set { _allowMaximize = value; } }
    [SerializeField] private bool _allowMaximize = false;

    public bool isMaximized  = false; // If allowMultipleInstances is false, the game uses this value to store the state of the Window after closing. If allowMultipleInstances is true, this value represents the starting state of the Window.
}