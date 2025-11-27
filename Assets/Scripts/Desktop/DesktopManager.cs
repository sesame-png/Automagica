using System.Collections.Generic;
using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    [SerializeField] private AppListSO apps;
    [SerializeField] private GameObject desktopIconPrefab;

    private void Awake()
    {
        foreach (AppSO app in apps.GetValue()) 
        {
            Instantiate(desktopIconPrefab, this.transform).GetComponent<DesktopIcon>().Initialize(app);
        }
    }

    /*private void OnEnable()
    {
        apps.Subscribe(UpdateDesktop);
    }

    private void OnDisable()
    {
        apps.Unsubscribe(UpdateDesktop);
    }

    public void UpdateDesktop(AppListSO apps)
    {
        //TODO: add functionality here
    }*/
}