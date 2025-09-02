using UnityEngine;
using UnityEngine.UI;

public class TaskbarIcon : MonoBehaviour
{
    private Window window;

    public void Initialize(Window window)
    {
        this.window = window;
        window.OnWindowClosed.AddListener(OnWindowClosed);
        window.taskbarIcon = this;
        GetComponent<Image>().sprite = window.app.icon;
    }

    private void OnEnable()
    {
        window?.OnWindowClosed.AddListener(OnWindowClosed);
    }

    private void OnDisable()
    {
        window?.OnWindowClosed.RemoveListener(OnWindowClosed);
    }

    public void ToggleMinimized()
    {
        if (!window.isMinimized && WindowManager.current.focusedWindow != window)
        {
            WindowManager.current.SetFocusedWindow(window);
        }
        else
        {
            window.ToggleMinimized();
        }
    }

    private void OnWindowClosed()
    {
        Destroy(gameObject);
    }
}