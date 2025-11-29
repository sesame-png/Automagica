using UnityEngine;
using UnityEngine.UI;

public class SetTheme : MonoBehaviour
{
    [SerializeField] private IntSO currentTheme;
    [SerializeField] private SpriteListSO sprites;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        currentTheme.Subscribe(Set);
    }

    private void OnDisable()
    {
        currentTheme.Unsubscribe(Set);
    }

    public void Set(int index)
    {
        if (0 <= index && index < sprites.GetValue().Count)
        {
            image.sprite = sprites.GetValue(index);
        }
        else
        {
            image.sprite = sprites.GetValue(0);
        }
    }
}