using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WindowManager : Singleton<WindowManager>
{
    /// Variables
    public IReadOnlyList<Window> windows => _windows;
    private List<Window> _windows = new List<Window>();

    public Window focusedWindow { get { return _focusedWindow; } private set { _focusedWindow = value; } }
    private Window _focusedWindow;

    /// Events
    [HideInInspector] public UnityEvent<Window> OnWindowOpened;
    //[HideInInspector] public UnityEvent<Window> OnWindowClosed;

    /// Instance
    public static WindowManager current => Instance;



    /// Enable & Disable
    private void OnEnable()
    {
        Raycaster.current?.OnAnyClickStarted.AddListener(OnClick);
    }

    private void OnDisable()
    {
        Raycaster.current?.OnAnyClickStarted.RemoveListener(OnClick);
    }



    /// Open & Close
    public Window OpenWindow(AppSO app)
    {
        Window openWindow = FindApp(app);

        if (openWindow && !app.allowMultipleInstances)
        {
            openWindow.Unminimize();
            SetFocusedWindow(openWindow);
            return openWindow;
        }
        else
        {
            Window newWindow = Instantiate(app.windowPrefab, transform).GetComponent<Window>();
            newWindow.Initialize(app);
            newWindow.Open();
            SetFocusedWindow(newWindow);
            _windows.Add(newWindow);

            OnWindowOpened.Invoke(newWindow);
            Debug.Log("Window " + app.appName + " opened.");
            return newWindow;
        }
    }

    public void CloseWindow(Window window)
    {
        Destroy(window.gameObject);
        _windows.Remove(window);
        SetFocusedWindow();

        //OnWindowClosed.Invoke(window);
        Debug.Log("Window " + window.app.appName + " closed.");
    }



    /// Focus window
    private void OnClick(List<GameObject> hitObjects)
    {
        foreach (GameObject obj in hitObjects)
        {
            if (obj.CompareTag("Window"))
            {
                SetFocusedWindow(obj.GetComponentInParent<Window>());
                return;
            }
        }

        RemoveFocusedWindow();
    }

    public void SetFocusedApp(AppSO app)
    {
        Window window = FindApp(app);

        if (window)
        {
            SetFocusedWindow(window);
        }
        else
        {
            OpenWindow(app);
        }
    }

    public void SetFocusedWindow(Window window = null)
    {
        if (focusedWindow == window) { return; }

        focusedWindow?.SetUnfocused();

        if (window)
        {
            focusedWindow = window;
            window.transform.SetAsLastSibling();
            focusedWindow.SetFocused();
        }
        else if (transform.childCount > 0)
        {
            focusedWindow = transform.GetChild(transform.childCount - 1).GetComponent<Window>();
            focusedWindow.SetFocused();
        }
        else
        {
            RemoveFocusedWindow();
        }
    }

    public void RemoveFocusedWindow()
    {
        focusedWindow?.SetUnfocused();
        focusedWindow = null;
    }



    /// Helper Functions
    public Window FindApp(AppSO app)
    {
        return _windows.Find(window => window.app == app);
    }
}