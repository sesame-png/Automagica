using UnityEngine;

public class TaskbarManager : MonoBehaviour
{
    [SerializeField] private GameObject taskbarIconPrefab;

    private void OnEnable()
    {
        WindowManager.WM.OnWindowOpened.AddListener(AddWindow);
    }

    private void OnDisable()
    {
        WindowManager.WM.OnWindowOpened.RemoveListener(AddWindow);
    }

    private void AddWindow(Window window)
    {
        TaskbarIcon icon = Instantiate(taskbarIconPrefab, this.transform).GetComponent<TaskbarIcon>();
        icon.Initialize(window);
    }
}