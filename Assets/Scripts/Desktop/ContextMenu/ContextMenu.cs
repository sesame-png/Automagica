using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using AYellowpaper.SerializedCollections;
using DG.Tweening;

public class ContextMenu : MonoBehaviour
{
    /// Components
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonHolder;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    /// Variables
    private Vector2 size;
    private bool isOpen;

    /// Tweens
    private Tween alphaTween;
    private Tween sizeTween;
    private float tweenDuration = 0.3f;



    /// Initialization
    public void Initialize(SerializedDictionary<string, UnityEvent> actions)
    {
        foreach (var action in actions)
        {
            Button button = Instantiate(buttonPrefab, buttonHolder).GetComponent<Button>();
        }

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        size = rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0f);
    }



    /// Enable & Disable
    private void OnEnable()
    {
        Raycaster.current?.OnAnyClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        Raycaster.current?.OnAnyClick.RemoveListener(OnClick);
    }



    /// Open & Close
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

    public void OnClick(List<RaycastResult> raycastHits, InputAction.CallbackContext context)
    {
        if (!isOpen) { return; }
        if (!context.canceled) { return; }

        foreach (RaycastResult hit in raycastHits)
        {
            if (hit.gameObject.CompareTag("RaycastBlocker"))
            {
                break;
            }
            else if (hit.gameObject.CompareTag("ContextMenu"))
            {
                return;
            }
        }

        Close();
    }
}