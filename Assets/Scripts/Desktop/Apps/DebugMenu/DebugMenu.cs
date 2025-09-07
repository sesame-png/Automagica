using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] private AppSO debugApp;

    public void SendNotification()
    {
        NotificationManager.current.SendNotification(debugApp.icon, "This is a test notification", "This is a body paragraph. This is a body paragraph. This is a body paragraph.");
    }
}