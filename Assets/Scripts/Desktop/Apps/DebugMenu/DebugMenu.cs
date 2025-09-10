using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] private AppSO debugMenuApp;

    public void SendNotification()
    {
        NotificationManager.current.SendNotification(debugMenuApp.icon, "This is a test notification", "This is a body paragraph. This is a body paragraph. This is a body paragraph.", debugMenuApp);
    }
}