using UnityEngine;
using System.Collections;

public class NotificationManager : Singleton<NotificationManager>
{
    public float notificationDuration { get { return _notificationDuration; } private set { _notificationDuration = value; } }
    [SerializeField] private float _notificationDuration;

    [SerializeField] private GameObject notificationPrefab;
    [HideInInspector] public NotificationPopup notificationPopup;

    public static NotificationManager current => Instance;

    public void SendNotification(Sprite sprite, string header, string body, AppSO source)
    {
        if (notificationPopup)
        {
            notificationPopup.Close();
        }

        notificationPopup = Instantiate(notificationPrefab, transform).GetComponent<NotificationPopup>();
        notificationPopup.Initialize(sprite, header, body, source);
        notificationPopup.Open();
    }
}