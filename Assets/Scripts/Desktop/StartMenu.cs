using UnityEngine;
using DG.Tweening;

public class StartMenu : ResizableRect
{
    //variables
    [SerializeField] new private RectParametersSO rectParameters;
    private bool isOpen;

    //components
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    //tweens
    private Tween alphaTween;
    private Tween sizeTween;
    private float tweenDuration = 0.2f;


    new protected void Awake()
    {
        base.rectParameters = rectParameters;
        base.Awake();
        canvas = transform.parent.GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        canvas.enabled = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        SetSize(rectParameters.cachedSize);
        rectTransform.sizeDelta = new Vector2(size.x, 0f);
    }



    /// <summary>
    /// Open & Close
    /// </summary>
    public void ToggleOpen()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Open()
    {
        if (isOpen) { return; }
        isOpen = true;

        canvas.enabled = true;

        alphaTween?.Kill();
        sizeTween?.Kill();
        alphaTween = canvasGroup.DOFade(1.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvasGroup.blocksRaycasts = true);
        sizeTween = rectTransform.DOSizeDelta(size, tweenDuration).SetEase(Ease.OutExpo);
    }

    public void Close()
    {
        if (!isOpen) { return; }
        isOpen = false;

        canvasGroup.blocksRaycasts = false;

        alphaTween?.Kill();
        sizeTween?.Kill();
        alphaTween = canvasGroup.DOFade(0.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvas.enabled = false);
        sizeTween = rectTransform.DOSizeDelta(new Vector2(size.x, 0f), tweenDuration).SetEase(Ease.OutExpo);
    }
}