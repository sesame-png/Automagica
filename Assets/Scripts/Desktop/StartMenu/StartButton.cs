using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField] private GameObject startMenuPrefab;
    [SerializeField] private Transform popupCanvas;
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
        startMenu = Instantiate(startMenuPrefab, popupCanvas).GetComponent<StartMenu>();
        startMenu.Open();
    }

    private void Close()
    {
        if (startMenu.isOpen)
        {
            startMenu.Close();
        }
    }
}