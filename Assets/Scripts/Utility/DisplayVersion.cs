using UnityEngine;
using TMPro;

public class DisplayVersion : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        text.text = "v" + Application.version;
    }
}