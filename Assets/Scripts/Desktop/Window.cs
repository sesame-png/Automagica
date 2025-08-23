using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Window : ResizableRect, IPointerDownHandler
{
    //variables
    public AppSO app { get { return _app; } private set { _app = value; } }
    private AppSO _app;

    [HideInInspector] public TaskbarIcon taskbarIcon;

    //bool variables
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
        rectParameters = app.rectParameters;

        header.text = "<font-weight=\"700\">" + app.appName + "</font-weight>";
        icon.sprite = app.icon;

        if (app.allowMultipleInstances)
        {
            SetPosition(rectParameters.defaultPosition);
            SetSize(rectParameters.defaultSize);
        }
        else
        {
            SetPosition(rectParameters.cachedPosition);
            SetSize(rectParameters.cachedSize);
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
        
        SetActiveWindow();
        canvasGroup.blocksRaycasts = false;
        PivotUtility.SetPivotInWorldSpace(rectTransform, taskbarIcon.transform.position);

        alphaTween?.Kill();
        scaleTween?.Kill();
        alphaTween = canvasGroup.DOFade(0.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvas.enabled = false);
        scaleTween = transform.DOScale(tweenScale, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => WindowManager.WM.SetNextActiveWindow());
    }

    public void Unminimize()
    {
        if (!isMinimized) { return; }
        isMinimized = false;
        
        SetActiveWindow();
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

        SetActiveWindow();
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

        SetActiveWindow();
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
            SetActiveWindow();
            canvasGroup.blocksRaycasts = false;

            alphaTween?.Kill();
            scaleTween?.Kill();
            alphaTween = canvasGroup.DOFade(0.0f, tweenDuration).SetEase(Ease.OutExpo);
            scaleTween = transform.DOScale(tweenScale, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => WindowManager.WM.CloseWindow(this));
        }

        OnWindowClosed.Invoke();
    }



    /// <summary>
    /// Helper functions
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        //BUG: any buttons within the window block this raycast, preventing it from becoming active
        SetActiveWindow();
    }

    public void SetActiveWindow()
    {
        WindowManager.WM.SetActiveWindow(this);
    }
}