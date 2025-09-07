using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NotificationPopup : MonoBehaviour
{
    /// Variables
    private Vector2 size;

    /// Components
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    /// Tweens
    private Tween alphaTween;
    private Tween sizeTween;
    private float tweenDuration = 0.5f;



    /// Initialization
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        size = rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(0f, rectTransform.sizeDelta.y);
    }

    public void Initialize(Sprite sprite, string header, string body)
    {
        image.sprite = sprite;
        text.text = "<font-weight=\"700\">" + header + "</font-weight>\n<font-weight=\"400\">" + body + "</font-weight>";
    }



    /// Enable & Disable
    /*private void OnEnable()
    {
        Raycaster.current?.OnAnyClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        Raycaster.current?.OnAnyClick.RemoveListener(OnClick);
    }*/



    /// Open & Close
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

    /*public void OnClick(List<RaycastResult> raycastHits, InputAction.CallbackContext context)
    {
        if (!context.canceled) { return; }

        foreach (RaycastResult hit in raycastHits)
        {
            if (hit.gameObject.CompareTag("RaycastBlocker"))
            {
                break;
            }
            else if (hit.gameObject.CompareTag("NotificationPopup"))
            {
                return;
            }
        }

        Close();
    }*/
}