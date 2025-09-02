using UnityEngine;
using UnityEngine.UI;

public class DesktopIcon : MonoBehaviour
{
    private AppSO app;

    public void Initialize(AppSO app)
    {
        this.app = app;
        GetComponent<Image>().sprite = app.icon;
    }

    public void OpenWindow()
    {
        WindowManager.current.OpenWindow(app);
    }
}