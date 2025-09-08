using UnityEngine;
using TMPro;

public class DisplayFramerate : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] float refreshTime;
    [SerializeField] int decimalPlaces = 2;

    private int frames = 0;
    private float time = 0f;

    private void Update()
    {
        time += Time.unscaledDeltaTime;
        frames++;

        if (time >= refreshTime)
        {
            text.text = ((float)frames / time).ToString("F" + decimalPlaces) + " fps";
            frames = 0;
            time = 0f;
        }
    }
}