using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ContextActions : MonoBehaviour
{
    [SerializeField] private GameObject contextMenuPrefab;
    [SerializeField] private Transform popupHolder;

    private void OnEnable()
    {
        Raycaster.current?.OnRightClickStarted.AddListener(OnRightClick);
    }

    private void OnDisable()
    {
        Raycaster.current?.OnRightClickStarted.RemoveListener(OnRightClick);
    }

    private void OnRightClick(List<GameObject> hitObjects)
    {
        if (hitObjects.Contains(gameObject))
        {
            GameObject menuObject = Instantiate(contextMenuPrefab, transform.position, Quaternion.identity, popupHolder);
            //do transforms

            ContextMenu contextMenu = menuObject.GetComponent<ContextMenu>();
            contextMenu.Initialize(); //initialize with actions
            contextMenu.Open();
        }
    }
}