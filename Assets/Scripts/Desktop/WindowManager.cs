using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public static WindowManager WM => Instance;



    /// <summary>
    /// Open & Close
    /// </summary>
    public void OpenWindow(AppSO app)
    {
        Window openWindow = FindApp(app);

        if (openWindow && !app.allowMultipleInstances)
        {
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
            Debug.Log(app.appName + " opened.");
        }
    }

    public void CloseWindow(Window window)
    {
        //SetNextActiveWindow();
        Destroy(window.gameObject);
        _windows.Remove(window);

        OnWindowClosed.Invoke(window);
        Debug.Log(window.app.appName + " closed.");
    }



    /// <summary>
    /// Active window
    /// </summary>
    public void SetActiveWindow(Window window)
    {
        activeWindow = window;
        window.transform.SetAsLastSibling();
    }

    public void SetNextActiveWindow()
    {
        //BUG: this does not work as wanted
        //loop through windows backwards until you find one that isnt minimized?
        if (transform.childCount > 2)
        {
            SetActiveWindow(transform.GetChild(transform.childCount - 2).GetComponent<Window>());
        }
        else
        {
            SetActiveWindow(null);
        }
    }



    /// <summary>
    /// Helper functions
    /// </summary>
    public Window FindApp(AppSO app)
    {
        return _windows.Find(window => window.app == app);
    }
}