using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WindowManager : Singleton<WindowManager>
{
    //variables
    public IReadOnlyList<Window> windows => _windows;
    private List<Window> _windows = new List<Window>();

    public Window activeWindow { get { return _activeWindow; } private set { _activeWindow = value; } }
    private Window _activeWindow;

    //events
    [HideInInspector] public UnityEvent<Window> OnWindowOpened;
    [HideInInspector] public UnityEvent<Window> OnWindowClosed;

    //instance
    public static WindowManager current => Instance;



    /// <summary>
    /// Open & Close
    /// </summary>
    public void OpenWindow(AppSO app)
    {
        Window openWindow = FindApp(app);

        if (openWindow && !app.allowMultipleInstances)
        {
            openWindow.Unminimize();
            SetActiveWindow(openWindow);
        }
        else
        {
            Window newWindow = Instantiate(app.windowPrefab, transform).GetComponent<Window>();
            newWindow.Initialize(app);
            newWindow.Open();
            SetActiveWindow(newWindow);
            _windows.Add(newWindow);

            OnWindowOpened.Invoke(newWindow);
            Debug.Log("Window " + app.appName + " opened.");
        }
    }

    public void CloseWindow(Window window)
    {
        Destroy(window.gameObject);
        _windows.Remove(window);
        SetActiveWindow();

        OnWindowClosed.Invoke(window);
        Debug.Log("Window " + window.app.appName + " closed.");
    }



    /// <summary>
    /// Active window
    /// </summary>
    public void SetActiveWindow(Window window = null)
    {
        activeWindow?.SetInactive();

        if (window)
        {
            activeWindow = window;
            window.transform.SetAsLastSibling();
        }
        else if (transform.childCount > 0)
        {
            activeWindow = transform.GetChild(transform.childCount - 1).GetComponent<Window>();
        }
        else
        {
            activeWindow = null;
        }

        activeWindow?.SetActive();
    }



    /// <summary>
    /// Helper functions
    /// </summary>
    public Window FindApp(AppSO app)
    {
        return _windows.Find(window => window.app == app);
    }
}