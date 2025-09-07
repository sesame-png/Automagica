using UnityEngine;
using System.Collections;

public class NotificationManager : Singleton<NotificationManager>
{
    public float notificationDuration { get { return _notificationDuration; } private set { _notificationDuration = value; } }
    [SerializeField] private float _notificationDuration;

    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private Transform popupCanvas;
    [HideInInspector] public NotificationPopup notificationPopup;

    public static NotificationManager current => Instance;

    public void SendNotification(Sprite sprite, string header, string body)
    {
        if (notificationPopup)
        {
            notificationPopup.Close();
        }

        notificationPopup = Instantiate(notificationPrefab, popupCanvas).GetComponent<NotificationPopup>();
        notificationPopup.Initialize(sprite, header, body);
        notificationPopup.Open();
    }
}