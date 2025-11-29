using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class NotificationPopup : MonoBehaviour
{
    // Variables
    private Vector2 size;

    // Components
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    private AppSO source;

    // Tweens
    private Tween alphaTween;
    private Tween sizeTween;
    private float tweenDuration = 0.5f;



    // Initialization
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        size = rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(0f, rectTransform.sizeDelta.y);
    }

    public void Initialize(Sprite sprite, string header, string body, AppSO source)
    {
        image.sprite = sprite;
        text.text = "<font-weight=\"700\">" + header + "</font-weight>\n<font-weight=\"400\">" + body + "</font-weight>";
        this.source = source;
    }



    // Open & Close
    public void Open()
    {
        alphaTween?.Kill();
        sizeTween?.Kill();
        alphaTween = canvasGroup.DOFade(1.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvasGroup.blocksRaycasts = true);
        sizeTween = rectTransform.DOSizeDelta(size, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => StartCoroutine(Timer()));
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(NotificationManager.current.notificationDuration);
        Close();
    }

    public void Close()
    {
        canvasGroup.blocksRaycasts = false;
        NotificationManager.current.notificationPopup = null;

        alphaTween?.Kill();
        sizeTween?.Kill();
        alphaTween = canvasGroup.DOFade(0.0f, tweenDuration).SetEase(Ease.OutExpo);
        sizeTween = rectTransform.DOSizeDelta(new Vector2(0f, size.y), tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => Destroy(gameObject));
    }



    // Redirect
    public void Redirect()
    {
        WindowManager.current.SetFocusedApp(source);
        Close();
    }
}