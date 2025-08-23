using UnityEngine;

[CreateAssetMenu(menuName = "Apps/App")]
public class AppSO : ScriptableObject
{
    [SerializeField] public string appName;

    public RectParametersSO rectParameters { get { return _rectParameters; } private set { _rectParameters = value; } }
    [SerializeField] private RectParametersSO _rectParameters;

    public GameObject windowPrefab { get { return _windowPrefab; } private set { _windowPrefab = value; } }
    [SerializeField] private GameObject _windowPrefab;

    public Sprite icon { get { return _icon; } private set { _icon = value; } }
    [SerializeField] private Sprite _icon;

    public bool allowMultipleInstances { get { return _allowMultipleInstances; } private set { _allowMultipleInstances = value; } }
    [SerializeField] private bool _allowMultipleInstances = false;
}