using UnityEngine;

public class TaskbarManager : MonoBehaviour
{
    [SerializeField] private GameObject taskbarIconPrefab;

    private void OnEnable()
    {
        WindowManager.current?.OnWindowOpened.AddListener(AddWindow);
    }

    private void OnDisable()
    {
        WindowManager.current?.OnWindowOpened.RemoveListener(AddWindow);
    }

    private void AddWindow(Window window)
    {
        TaskbarIcon icon = Instantiate(taskbarIconPrefab, transform).GetComponent<TaskbarIcon>();
        icon.Initialize(window);
    }
}