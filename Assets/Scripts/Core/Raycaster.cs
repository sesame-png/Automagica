using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// This custom raycaster allows for multiple stacked GameObjects to be triggered at once
// Otherwise, only the first interactable object (a Button, e.g.) gets triggered by the EventSystem
// It can also be used to simply detect any mouse input; for example, to test if an object wasn't clicked
public class Raycaster : Singleton<Raycaster>
{
    // Variables
    [SerializeField] private InputReaderSO inputReader;

    // Events
    // Is all of this necessary? Probably not. But checking for started/performed/canceled annoys me
    [HideInInspector] public UnityEvent<List<GameObject>> OnAnyClickStarted;
    [HideInInspector] public UnityEvent<List<GameObject>> OnAnyClickPerformed;
    [HideInInspector] public UnityEvent<List<GameObject>> OnAnyClickCanceled;

    //[HideInInspector] public UnityEvent<List<GameObject>> OnLeftClickStarted;
    //[HideInInspector] public UnityEvent<List<GameObject>> OnLeftClickPerformed;
    //[HideInInspector] public UnityEvent<List<GameObject>> OnLeftClickCanceled;

    //[HideInInspector] public UnityEvent<List<GameObject>> OnMiddleClickStarted;
    //[HideInInspector] public UnityEvent<List<GameObject>> OnMiddleClickPerformed;
    //[HideInInspector] public UnityEvent<List<GameObject>> OnMiddleClickCanceled;

    [HideInInspector] public UnityEvent<List<GameObject>> OnRightClickStarted;
    [HideInInspector] public UnityEvent<List<GameObject>> OnRightClickPerformed;
    [HideInInspector] public UnityEvent<List<GameObject>> OnRightClickCanceled;

    // Instance
    public static Raycaster current => Instance;



    // Enable & Disable
    private void OnEnable()
    {
        inputReader.leftClick += RaycastLeftClick;
        inputReader.middleClick += RaycastMiddleClick;
        inputReader.rightClick += RaycastRightClick;
    }

    private void OnDisable()
    {
        inputReader.leftClick -= RaycastLeftClick;
        inputReader.middleClick -= RaycastMiddleClick;
        inputReader.rightClick -= RaycastRightClick;
    }

    // Event Callbacks
    private void RaycastLeftClick(InputAction.CallbackContext context)
    {
        List<GameObject> hitObjects = Raycast();

        if (context.started)
        {
            OnAnyClickStarted.Invoke(hitObjects);
            //OnLeftClickStarted.Invoke(hitObjects);
        }
        else if (context.performed)
        {
            OnAnyClickPerformed.Invoke(hitObjects);
            //OnLeftClickPerformed.Invoke(hitObjects);
        }
        else if (context.canceled)
        {
            OnAnyClickCanceled.Invoke(hitObjects);
            //OnLeftClickCanceled.Invoke(hitObjects);
        }
    }

    private void RaycastMiddleClick(InputAction.CallbackContext context)
    {
        List<GameObject> hitObjects = Raycast();

        if (context.started)
        {
            OnAnyClickStarted.Invoke(hitObjects);
            //OnMiddleClickStarted.Invoke(hitObjects);
        }
        else if (context.performed)
        {
            OnAnyClickPerformed.Invoke(hitObjects);
            //OnMiddleClickPerformed.Invoke(hitObjects);
        }
        else if (context.canceled)
        {
            OnAnyClickCanceled.Invoke(hitObjects);
            //OnMiddleClickCanceled.Invoke(hitObjects);
        }
    }

    private void RaycastRightClick(InputAction.CallbackContext context)
    {
        List<GameObject> hitObjects = Raycast();

        if (context.started)
        {
            OnAnyClickStarted.Invoke(hitObjects);
            OnRightClickStarted.Invoke(hitObjects);
        }
        else if (context.performed)
        {
            OnAnyClickPerformed.Invoke(hitObjects);
            OnRightClickPerformed.Invoke(hitObjects);
        }
        else if (context.canceled)
        {
            OnAnyClickCanceled.Invoke(hitObjects);
            OnRightClickCanceled.Invoke(hitObjects);
        }
    }



    // Raycast
    private List<GameObject> Raycast()
    {
        PointerEventData eventData = new PointerEventData(null);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastHits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastHits);

        // Converting the list of RaycastHits to GameObjects while discarding any objects below a RaycastBlocker for easier comparisons
        // Note: This discards screenPosition and worldPosition pointer data, which could be useful in the future
        List<GameObject> hitObjects = new List<GameObject>();
        foreach (RaycastResult hit in raycastHits)
        {
            if (hit.gameObject.CompareTag("RaycastBlocker"))
            {
                break;
            }
            
            hitObjects.Add(hit.gameObject);
        }

        return hitObjects;
    }
}