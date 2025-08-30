using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Window : ResizableRect, IPointerDownHandler
{
    //variables
    public AppSO app { get { return _app; } private set { _app = value; } }
    private AppSO _app;

    [HideInInspector] public TaskbarIcon taskbarIcon;

    //bool variables
    public bool isActive { get { return _isActive; } private set { _isActive = value; } }
    private bool _isActive = false;

    public bool isMaximized { get { return _isMaximized; } private set { _isMaximized = value; } }
    private bool _isMaximized = false;

    public bool isMinimized { get { return _isMinimized; } private set { _isMinimized = value; } }
    private bool _isMinimized = false;

    //events
    [HideInInspector] public UnityEvent OnWindowOpened;
    [HideInInspector] public UnityEvent OnWindowClosed;

    //components
    [SerializeField] private TMP_Text header;
    [SerializeField] private Image icon;
    [SerializeField] private FadeTween dropShadow;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform parentRectTransform;

    //tweens
    private Tween sizeTween;
    private Tween positionTween;
    private Tween alphaTween;
    private Tween scaleTween;
    private float tweenDuration = 0.2f;
    private float tweenScale = 0.9f;



    new protected void Awake()
    {
        base.Awake();
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.0f;
        transform.localScale = new Vector3(tweenScale, tweenScale, tweenScale);
    }

    public void Initialize(AppSO app)
    {
        this.app = app;
        rectParams = app.rectParams;

        header.text = "<font-weight=\"700\">" + app.appName + "</font-weight>";
        icon.sprite = app.icon;

        if (app.allowMultipleInstances)
        {
            SetPosition(rectParams.defaultPosition);
            SetSize(rectParams.defaultSize);
        }
        else
        {
            SetPosition(rectParams.cachedPosition);
            SetSize(rectParams.cachedSize);
        }
    }



    /// <summary>
    /// Open
    /// </summary>
    public void Open()
    {
        alphaTween?.Kill();
        scaleTween?.Kill();
        alphaTween = canvasGroup.DOFade(1.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvasGroup.blocksRaycasts = true);
        scaleTween = transform.DOScale(1.0f, tweenDuration).SetEase(Ease.OutExpo);

        OnWindowOpened.Invoke();
    }



    /// <summary>
    /// Minimize & Unminimize
    /// </summary>
    public void ToggleMinimized()
    {
        if (isMinimized)
        {
            Unminimize();
        }
        else
        {
            Minimize();
        }
    }

    public void Minimize()
    {
        if (isMinimized) { return; }
        isMinimized = true;

        WindowManager.WM.SetActiveWindow(this);
        canvasGroup.blocksRaycasts = false;
        PivotUtility.SetPivotInWorldSpace(rectTransform, taskbarIcon.transform.position);

        alphaTween?.Kill();
        scaleTween?.Kill();
        alphaTween = canvasGroup.DOFade(0.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvas.enabled = false);
        scaleTween = transform.DOScale(tweenScale, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => Disable());
    }

    public void Unminimize()
    {
        if (!isMinimized) { return; }
        isMinimized = false;

        WindowManager.WM.SetActiveWindow(this);
        canvas.enabled = true;

        alphaTween?.Kill();
        scaleTween?.Kill();
        alphaTween = canvasGroup.DOFade(1.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvasGroup.blocksRaycasts = true);
        scaleTween = transform.DOScale(1.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => PivotUtility.SetPivot(rectTransform, new Vector2(0.5f, 0.5f)));
    }



    /// <summary>
    /// Maximize & Unmaximize
    /// </summary>
    public void ToggleMaximized()
    {
        if (isMaximized)
        {
            Unmaximize();
        }
        else
        {
            Maximize();
        }
    }

    public void Maximize()
    {
        if (isMinimized) { Unminimize(); }
        if (isMaximized) { return; }
        isMaximized = true;
        moveable = false;
        resizable = false;

        WindowManager.WM.SetActiveWindow(this);
        canvasGroup.blocksRaycasts = false;
        //rectTransform.anchorMin = new Vector2(0f, 0f);
        //rectTransform.anchorMax = new Vector2(1f, 1f);

        positionTween?.Kill();
        sizeTween?.Kill();
        positionTween = rectTransform.DOAnchorPos(parentRectTransform.anchoredPosition, tweenDuration).SetEase(Ease.OutExpo);
        sizeTween = rectTransform.DOSizeDelta(new Vector2(parentRectTransform.rect.width, parentRectTransform.rect.height), tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvasGroup.blocksRaycasts = true);
    }

    public void Unmaximize()
    {
        if (isMinimized) { return; }
        if (!isMaximized) { return; }
        isMaximized = false;
        moveable = true;
        resizable = true;

        WindowManager.WM.SetActiveWindow(this);
        canvasGroup.blocksRaycasts = false;
        //rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        //rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

        positionTween?.Kill();
        sizeTween?.Kill();
        positionTween = rectTransform.DOAnchorPos(position, tweenDuration).SetEase(Ease.OutExpo);
        sizeTween = rectTransform.DOSizeDelta(size, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvasGroup.blocksRaycasts = true);
    }



    /// <summary>
    /// Close
    /// </summary>
    public void Close()
    {
        if (isMinimized)
        {
            WindowManager.WM.CloseWindow(this);
        }
        else
        {
            WindowManager.WM.SetActiveWindow(this);
            canvasGroup.blocksRaycasts = false;

            alphaTween?.Kill();
            scaleTween?.Kill();
            alphaTween = canvasGroup.DOFade(0.0f, tweenDuration).SetEase(Ease.OutExpo);
            scaleTween = transform.DOScale(tweenScale, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => WindowManager.WM.CloseWindow(this));
        }

        OnWindowClosed.Invoke();
    }



    /// <summary>
    /// Active Window
    /// </summary>
    public void SetActive()
    {
        isActive = true;
        dropShadow.PlayIn();
    }

    public void SetInactive()
    {
        isActive = false;
        dropShadow.PlayOut();
    }

    private void Disable()
    {
        WindowManager.WM.SetActiveWindow();
        transform.SetAsFirstSibling();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        WindowManager.WM.SetActiveWindow(this);
    }
}