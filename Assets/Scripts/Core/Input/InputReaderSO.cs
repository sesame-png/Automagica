using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

[CreateAssetMenu(menuName = "Singletons/InputReader")]
public class InputReaderSO : ScriptableObject, InputActions.IUIActions
{
    public InputActions inputActions;

    // Global actions
    public event Action<InputAction.CallbackContext> anyAction = delegate { };

    // UI actions
    public event Action<InputAction.CallbackContext> point = delegate { };
    public event Action<InputAction.CallbackContext> anyClick = delegate { };
    public event Action<InputAction.CallbackContext> leftClick = delegate { };
    public event Action<InputAction.CallbackContext> middleClick = delegate { };
    public event Action<InputAction.CallbackContext> rightClick = delegate { };
    public event Action<InputAction.CallbackContext> scrollWheel = delegate { };
    public event Action<InputAction.CallbackContext> navigate = delegate { };
    public event Action<InputAction.CallbackContext> submit = delegate { };
    public event Action<InputAction.CallbackContext> cancel = delegate { };

    private void OnEnable()
    {
        if (inputActions == null)
        {
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif

            inputActions = new InputActions();
            inputActions.UI.SetCallbacks(this);

            foreach (var map in inputActions.asset.actionMaps)
            {
                map.actionTriggered += OnAnyAction;
            }
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        foreach (var map in inputActions.asset.actionMaps)
        {
            map.actionTriggered -= OnAnyAction;
        }

        inputActions.Disable();
    }

    #region Enable/Disable
    public void EnableUIControls()
    {
        inputActions.UI.Enable();
    }

    public void DisableUIControls()
    {
        inputActions.UI.Disable();
    }
    #endregion

    #region Event Callbacks
    private void OnAnyAction(InputAction.CallbackContext context)
    {
        anyAction.Invoke(context);
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        point?.Invoke(context);
    }

    public void OnAnyClick(InputAction.CallbackContext context)
    {
        anyClick?.Invoke(context);
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        leftClick?.Invoke(context);
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        middleClick?.Invoke(context);
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        rightClick?.Invoke(context);
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        scrollWheel?.Invoke(context);
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        navigate?.Invoke(context);
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        submit?.Invoke(context);
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        cancel?.Invoke(context);
    }
    #endregion
}