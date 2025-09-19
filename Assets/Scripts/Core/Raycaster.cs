using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Raycaster : Singleton<Raycaster>
{
    /// Variables
    [SerializeField] private InputReaderSO inputReader;

    /// Events
    [HideInInspector] public UnityEvent<List<RaycastResult>, InputAction.CallbackContext> OnAnyClick;
    //[HideInInspector] public UnityEvent<List<RaycastResult>, InputAction.CallbackContext> OnLeftClick;
    //[HideInInspector] public UnityEvent<List<RaycastResult>, InputAction.CallbackContext> OnMiddleClick;
    [HideInInspector] public UnityEvent<List<RaycastResult>, InputAction.CallbackContext> OnRightClick;

    /// Instance
    public static Raycaster current => Instance;



    /// Enable & Disable
    private void OnEnable()
    {
        inputReader.anyClick += RaycastAnyClick;
        //inputReader.leftClick += RaycastLeftClick;
        //inputReader.middleClick += RaycastMiddleClick;
        inputReader.rightClick += RaycastRightClick;
    }

    private void OnDisable()
    {
        inputReader.anyClick -= RaycastAnyClick;
        //inputReader.leftClick -= RaycastLeftClick;
        //inputReader.middleClick -= RaycastMiddleClick;
        inputReader.rightClick -= RaycastRightClick;
    }



    /// Event Callbacks
    private void RaycastAnyClick(InputAction.CallbackContext context)
    {
        OnAnyClick.Invoke(RaycastAll(), context);
    }

    /*private void RaycastLeftClick(InputAction.CallbackContext context)
    {
        OnLeftClick.Invoke(RaycastAll(), context);
    }*/

    /*private void RaycastMiddleClick(InputAction.CallbackContext context)
    {
        OnMiddleClick.Invoke(RaycastAll(), context);
    }*/

    private void RaycastRightClick(InputAction.CallbackContext context)
    {
        OnRightClick.Invoke(RaycastAll(), context);
    }



    /// Raycast
    private List<RaycastResult> RaycastAll()
    {
        PointerEventData eventData = new PointerEventData(null);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastHits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastHits);
        return raycastHits;
    }
}