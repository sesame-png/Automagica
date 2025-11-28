using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using TMPro;

public class ContextMenu : MonoBehaviour
{
    // Components
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonHolder;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    // Variables
    private Vector2 size;
    private bool isOpen;

    // Tweens
    private Tween alphaTween;
    private Tween sizeTween;
    private float tweenDuration = 0.2f;



    // Initialization
    public void Initialize(SerializedDictionary<string, UnityEvent> actions)
    {
        foreach (KeyValuePair<string, UnityEvent> action in actions)
        {
            Button button = Instantiate(buttonPrefab, buttonHolder).GetComponent<Button>();
            button.onClick.AddListener(() => action.Value.Invoke()); // Is this legal? Moral? Ethical?
            button.onClick.AddListener(Close);
            button.transform.GetComponentInChildren<TMP_Text>().text = action.Key;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonHolder.GetComponent<RectTransform>());

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        size = new Vector2(rectTransform.sizeDelta.x, buttonHolder.GetComponent<RectTransform>().sizeDelta.y);
        rectTransform.sizeDelta = new Vector2(size.x, 0f);
    }



    // Enable & Disable
    private void OnEnable()
    {
        Raycaster.current?.OnAnyClickCanceled.AddListener(OnClick);
    }

    private void OnDisable()
    {
        Raycaster.current?.OnAnyClickCanceled.RemoveListener(OnClick);
    }



    // Open & Close
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

    public void OnClick(List<GameObject> hitObjects)
    {
        if (!isOpen) { return; }

        foreach (GameObject obj in hitObjects)
        {
            if (obj.CompareTag("ContextMenu"))
            {
                return;
            }
        }

        Close();
    }
}