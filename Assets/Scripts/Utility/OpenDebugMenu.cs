using UnityEngine;
using UnityEngine.InputSystem;

public class OpenDebugMenu : MonoBehaviour
{
    [SerializeField] private InputReaderSO inputReader;
    [SerializeField] private AppSO debugMenuApp;
    private Window debugWindow;

    private void OnEnable()
    {
        inputReader.debug += ToggleOpen;
    }

    private void OnDisable()
    {
        inputReader.debug -= ToggleOpen;
    }

    private void ToggleOpen(InputAction.CallbackContext context)
    {
        if (Debug.isDebugBuild)
        {
            if (!context.performed) { return; }

            if (debugWindow)
            {
                debugWindow.Close();
            }
            else
            {
                debugWindow = WindowManager.current.OpenWindow(debugMenuApp);
            }
        }
    }
}