using UnityEngine;

public class WindowOpener : MonoBehaviour
{
    public void OpenWindow(AppSO app)
    {
        WindowManager.current.OpenWindow(app);
    }
}