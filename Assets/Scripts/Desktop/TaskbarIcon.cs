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

    public void ToggleMinimized()
    {
        if (window.isMaximized && WindowManager.WM.activeWindow != window)
        {
            WindowManager.WM.SetActiveWindow(window);
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