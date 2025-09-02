using System.Collections.Generic;
using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    public List<AppSO> apps = new List<AppSO>(); //TODO: convert to AppListSO

    [SerializeField] private GameObject desktopIconPrefab;

    private void Awake()
    {
        foreach (AppSO app in apps) 
        {
            Instantiate(desktopIconPrefab, this.transform).GetComponent<DesktopIcon>().Initialize(app);
        }
    }

    public void AddApp()
    {
        //TODO: add functionality here
    }

    public void RemoveApp()
    {
        //TODO: add functionality here
    }
}