using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class StartMenu : ResizableRect
{
    //variables
    [SerializeField] private InputReaderSO inputReader;
    [SerializeField] new private RectParamsSO rectParams;
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
        base.rectParams = rectParams;
        base.Awake();
        canvas = transform.parent.GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        canvas.enabled = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        SetSize(rectParams.cachedSize);
        rectTransform.sizeDelta = new Vector2(size.x, 0f);
    }



    /// <summary>
    /// Enable & Disable
    /// </summary>
    private void OnEnable()
    {
        inputReader.anyClick += OnClick;
    }

    private void OnDisable()
    {
        inputReader.anyClick -= OnClick;
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

    private void OnClick(InputAction.CallbackContext context)
    {
        if (!isOpen) { return; }
        if (context.canceled) 
        { 
            /*foreach (RaycastResult hit in Raycaster.current.raycastHits)
            {
                Debug.Log(hit);
            }*/
        }
    }
}