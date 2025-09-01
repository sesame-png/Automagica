using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField] private GameObject startMenuPrefab;
    [SerializeField] private Transform startMenuHolder;
    private StartMenu startMenu;

    public void ToggleOpen()
    {
        if (startMenu)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Open()
    {
        if (startMenu) { return; }

        startMenu = Instantiate(startMenuPrefab, startMenuHolder).GetComponent<StartMenu>();
        startMenu.Open();
    }

    public void Close()
    {
        if (!startMenu) { return; }

        startMenu.Close();
    }
}