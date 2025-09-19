using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OpenContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject contextMenuPrefab;

    private void OnEnable()
    {
        Raycaster.current?.OnRightClick.AddListener(OnRightClick);
    }

    private void OnDisable()
    {
        Raycaster.current?.OnRightClick.RemoveListener(OnRightClick);
    }

    private void OnRightClick(List<RaycastResult> raycastHits, InputAction.CallbackContext context)
    {
        if (!context.started) { return; }

        foreach (RaycastResult hit in raycastHits)
        {
            if (hit.gameObject.CompareTag("RaycastBlocker"))
            {
                return;
            }

            ContextActions contextActions = hit.gameObject.GetComponent<ContextActions>();

            if (contextActions)
            {
                GameObject menuObject = Instantiate(contextMenuPrefab, transform);
                //do transforms

                ContextMenu contextMenu = menuObject.GetComponent<ContextMenu>();
                //contextMenu.Initialize(contextActions.actions);
                contextMenu.Open();

                return;
            }
        }
    }
}