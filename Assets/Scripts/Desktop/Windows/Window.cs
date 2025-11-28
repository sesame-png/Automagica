using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public class Window : ResizableRect
{
    // Variables
    public AppSO app { get { return _app; } private set { _app = value; } }
    private AppSO _app;

    [HideInInspector] public TaskbarIcon taskbarIcon;

    // Bools
    public bool isMaximized { get { return _isMaximized; } private set { _isMaximized = value; } }
    private bool _isMaximized = false;
    public bool isMinimized { get { return _isMinimized; } private set { _isMinimized = value; } }
    private bool _isMinimized = false;
    public bool isBeingDragged { get { return _isBeingDragged; } private set { _isBeingDragged = value; } }
    private bool _isBeingDragged = false;

    // Events
    //[HideInInspector] public UnityEvent OnWindowOpening;
    //[HideInInspector] public UnityEvent OnWindowOpened;
    //[HideInInspector] public UnityEvent OnWindowClosing;
    [HideInInspector] public UnityEvent OnWindowClosed;
    //[HideInInspector] public UnityEvent OnWindowFocused;
    //[HideInInspector] public UnityEvent OnWindowUnfocused;

    // Components
    [SerializeField] private TMP_Text header;
    [SerializeField] private Image icon;
    [SerializeField] private FadeTween dropShadow;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform parentRectTransform;

    // Tweens
    private Tween sizeTween;
    private Tween positionTween;
    private Tween alphaTween;
    private Tween scaleTween;
    private float tweenDuration = 0.3f;
    private float tweenScale = 0.9f;



    // Initialization
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

        if (app.isMaximized)
        {
            if (!app.allowMaximize) { return; }
            isMaximized = true;
            isMoveable = false;
            isResizable = false;

            RectUtility.SetAnchorsInPlace(rectTransform, Vector2.zero, Vector2.one);
            rectTransform.anchoredPosition = parentRectTransform.anchoredPosition;
            rectTransform.sizeDelta = Vector2.zero;
        }
    }



    // Open
    public void Open()
    {
        //OnWindowOpening.Invoke();

        alphaTween?.Kill();
        scaleTween?.Kill();
        alphaTween = canvasGroup.DOFade(1.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvasGroup.blocksRaycasts = true);
        scaleTween = transform.DOScale(1.0f, tweenDuration).SetEase(Ease.OutExpo);//.OnComplete(() => OnWindowOpened.Invoke());
    }



    // Minimize & Unminimize
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
        isMoveable = false;
        isResizable = false;

        WindowManager.current.SetFocusedWindow(this);
        canvasGroup.blocksRaycasts = false;
        RectUtility.SetPivotInWorldSpace(rectTransform, taskbarIcon.transform.position);

        alphaTween?.Kill();
        scaleTween?.Kill();
        alphaTween = canvasGroup.DOFade(0.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvas.enabled = false);
        scaleTween = transform.DOScale(tweenScale, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => MoveToBack());
    }

    public void Unminimize()
    {
        if (!isMinimized) { return; }
        isMinimized = false;
        isMoveable = true;
        isResizable = true;

        WindowManager.current.SetFocusedWindow(this);
        canvas.enabled = true;
        RectUtility.SetPivotInWorldSpace(rectTransform, taskbarIcon.transform.position);

        alphaTween?.Kill();
        scaleTween?.Kill();
        alphaTween = canvasGroup.DOFade(1.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvasGroup.blocksRaycasts = true);
        if (isMaximized)
        {
            scaleTween = transform.DOScale(1.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => rectTransform.pivot = new Vector2(0.5f, 0.5f)); //BUG HERE
        }
        else
        {
            scaleTween = transform.DOScale(1.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => RectUtility.SetPivotInPlace(rectTransform, new Vector2(0.5f, 0.5f)));
        }
    }



    // Maximize & Unmaximize
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
        if (!app.allowMaximize) { return; }
        if (isMinimized) { Unminimize(); }
        if (isMaximized) { return; }
        if (!app.allowMultipleInstances) { app.isMaximized = true; }
        isMaximized = true;
        isMoveable = false;
        isResizable = false;

        WindowManager.current.SetFocusedWindow(this);
        canvasGroup.blocksRaycasts = false;
        RectUtility.SetAnchorsInPlace(rectTransform, Vector2.zero, Vector2.one);

        positionTween?.Kill();
        sizeTween?.Kill();
        positionTween = rectTransform.DOAnchorPos(parentRectTransform.anchoredPosition, tweenDuration).SetEase(Ease.OutExpo);
        sizeTween = rectTransform.DOSizeDelta(Vector2.zero, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvasGroup.blocksRaycasts = true);
    }

    public void Unmaximize()
    {
        if (isMinimized) { return; }
        if (!isMaximized) { return; }
        if (!app.allowMultipleInstances) { app.isMaximized = false; }
        isMaximized = false;
        isMoveable = true;
        isResizable = true;

        WindowManager.current.SetFocusedWindow(this);
        canvasGroup.blocksRaycasts = false;
        RectUtility.SetAnchorsInPlace(rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));

        positionTween?.Kill();
        sizeTween?.Kill();
        if (!isBeingDragged) { positionTween = rectTransform.DOAnchorPos(position, tweenDuration).SetEase(Ease.OutExpo); }
        sizeTween = rectTransform.DOSizeDelta(size, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => canvasGroup.blocksRaycasts = true);
    }



    // Close
    public void Close()
    {
        //OnWindowClosing.Invoke();

        if (isMinimized)
        {
            OnWindowClosed.Invoke();
            WindowManager.current.CloseWindow(this);
        }
        else
        {
            WindowManager.current.SetFocusedWindow(this);
            canvasGroup.blocksRaycasts = false;

            alphaTween?.Kill();
            scaleTween?.Kill();
            alphaTween = canvasGroup.DOFade(0.0f, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => OnWindowClosed.Invoke());
            scaleTween = transform.DOScale(tweenScale, tweenDuration).SetEase(Ease.OutExpo).OnComplete(() => WindowManager.current.CloseWindow(this));
        }
    }



    // Focus & Unfocus
    public void SetFocused()
    {
        dropShadow.PlayIn();
        //OnWindowFocused.Invoke();
    }

    public void SetUnfocused()
    {
        dropShadow.PlayOut();
        //OnWindowUnfocused.Invoke();
    }

    private void MoveToBack()
    {
        transform.SetAsFirstSibling();
        WindowManager.current.SetFocusedWindow();
    }



    // Drag
    public void OnBeginDrag(Vector2 pos)
    {
        isBeingDragged = true;

        Vector2 localPos = new Vector2();
        if (isMaximized) 
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), pos, Camera.main, out localPos);
            SetPosition(localPos);
            
            Vector2 normalizedPos = RectUtility.LocalToNormalizedPoint(rectTransform, localPos);
            Vector2 posDelta = RectUtility.SetPivotInPlace(rectTransform, normalizedPos);
            MovePosition(-posDelta);

            Unmaximize(); 
        }
        else 
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pos, Camera.main, out localPos);
            
            Vector2 normalizedPos = RectUtility.LocalToNormalizedPoint(rectTransform, localPos);
            Vector2 posDelta = RectUtility.SetPivotInPlace(rectTransform, normalizedPos);
            MovePosition(-posDelta);
        }
    }

    public void OnDrag(Vector2 posDelta)
    {
        MovePosition(posDelta);
    }

    public void OnEndDrag(Vector2 pos)
    {
        isBeingDragged = false;

        Vector2 posDelta = RectUtility.SetPivotInPlace(rectTransform, new Vector2(0.5f, 0.5f));
        MovePosition(-posDelta);
    }
}