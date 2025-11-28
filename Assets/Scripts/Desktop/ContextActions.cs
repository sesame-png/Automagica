using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
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
            //position = new Vector3(position.x, position.y, 0f);
            ContextMenu contextMenu = Instantiate(contextMenuPrefab, position, Quaternion.identity, popupHolder).GetComponent<ContextMenu>();
            contextMenu.Initialize(actions);
            contextMenu.Open();
        }
    }
}