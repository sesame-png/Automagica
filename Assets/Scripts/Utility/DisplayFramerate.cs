using TMPro;
using UnityEngine;

public class DisplayFramerate : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Update()
    {
        text.text = (1 / Time.unscaledDeltaTime) + " fps";
    }
}