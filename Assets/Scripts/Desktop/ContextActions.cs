using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using AYellowpaper.SerializedCollections;

public class ContextActions : MonoBehaviour
{
    [SerializeField] private GameObject contextMenuPrefab;
    [SerializeField] private Transform popupHolder; //changeme

    public SerializedDictionary<string, UnityEvent> actions;

    private void OnEnable()
    {
        Raycaster.current?.OnRightClickCanceled.AddListener(OnRightClick);
    }

    private void OnDisable()
    {
        Raycaster.current?.OnRightClickCanceled.RemoveListener(OnRightClick);
    }

    private void OnRightClick(List<GameObject> hitObjects)
    {
        if (hitObjects.Contains(gameObject))
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ContextMenu contextMenu = Instantiate(contextMenuPrefab, position, Quaternion.identity, popupHolder).GetComponent<ContextMenu>();
            contextMenu.Initialize(actions);
            contextMenu.Open();
        }
    }
}