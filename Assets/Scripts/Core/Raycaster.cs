using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Raycaster : Singleton<Raycaster>
{
    //variables
    [SerializeField] private InputReaderSO inputReader;

    //events
    [HideInInspector] public UnityEvent OnRaycastLeftClick;
    [HideInInspector] public UnityEvent OnRaycastMiddleClick;
    [HideInInspector] public UnityEvent OnRaycastRightClick;

    //instance
    public static Raycaster current => Instance;

    private void OnEnable()
    {
        inputReader.leftClick += OnLeftClick;
        inputReader.rightClick += OnMiddleClick;
        inputReader.middleClick += OnRightClick;
    }

    private void OnDisable()
    {
        inputReader.leftClick -= OnLeftClick;
        inputReader.rightClick -= OnMiddleClick;
        inputReader.middleClick -= OnRightClick;
    }

    private void OnLeftClick(InputAction.CallbackContext context)
    {
        List<RaycastResult> raycastHits = RaycastAll();
        OnRaycastLeftClick.Invoke();
    }

    private void OnMiddleClick(InputAction.CallbackContext context)
    {
        List<RaycastResult> raycastHits = RaycastAll();
        OnRaycastMiddleClick.Invoke();
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        List<RaycastResult> raycastHits = RaycastAll();
        OnRaycastRightClick.Invoke();
    }

    private List<RaycastResult> RaycastAll()
    {
        PointerEventData eventData = new PointerEventData(null);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastHits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastHits);
        return raycastHits;
    }
}