using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class StartMenu : ResizableRect
{
    /// Variables
    [SerializeField] new private RectParamsSO rectParams;
    public bool isOpen { get { return _isOpen; } private set { _isOpen = value; } }
    private bool _isOpen;

    /// Components
    private CanvasGroup canvasGroup;

    /// Tweens
    private Tween alphaTween;
    private Tween sizeTween;
    private float tweenDuration = 0.3f;



    /// Initialization
    new protected void Awake()
    {
        base.rectParams = rectParams;
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        SetSize(rectParams.cachedSize);
        rectTransform.sizeDelta = new Vector2(size.x, 0f);
    }



    /// Enable & Disable
    private void OnEnable()
    {
        Raycaster.current?.OnAnyClickCanceled.AddListener(OnClick);
    }

    private void OnDisable()
    {
        Raycaster.current?.OnAnyClickCanceled.RemoveListener(OnClick);
    }



    /// Open & Close
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

        alphaTween?.Kill();
        sizeTween?.Kill();
        alphaTween = canvasGroup.DOFade(1.0f, tweenDuration).SetEase(Ease.OutExpo);
        sizeTween = rectTransform.DOSizeDelta(size, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvasGroup.blocksRaycasts = true);
    }

    public void Close()
    {
        if (!isOpen) { return; }
        isOpen = false;

        canvasGroup.blocksRaycasts = false;

        alphaTween?.Kill();
        sizeTween?.Kill();
        alphaTween = canvasGroup.DOFade(0.0f, tweenDuration).SetEase(Ease.OutExpo);
        sizeTween = rectTransform.DOSizeDelta(new Vector2(size.x, 0f), tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => Destroy(gameObject));
    }

    private void OnClick(List<GameObject> hitObjects)
    {
        if (!isOpen) { return; }

        foreach (GameObject obj in hitObjects)
        {
            if (obj.CompareTag("StartMenu"))
            {
                return;
            }
        }

        Close();
    }
}