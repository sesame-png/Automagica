using UnityEngine;
using UnityEngine.UI;

public class DesktopIcon : MonoBehaviour
{
    private AppSO app;

    public void Initialize(AppSO app)
    {
        this.app = app;
        GetComponent<Image>().sprite = app.icon;
        //TODO: add text
    }

    public void OpenWindow()
    {
        //TODO: add double click
        WindowManager.current.OpenWindow(app);
    }
}