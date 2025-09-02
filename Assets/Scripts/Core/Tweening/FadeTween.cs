using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeTween : UITween
{
    /// Variables
    [SerializeField] private bool checkInteractable;

    /// Components
    private CanvasGroup canvasGroup;



    /// Initialization
    protected override void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = startIn ? toValue : fromValue;

        if (checkInteractable)
        {
            SetInteractable();
        }
    }



    /// Play
    public override void PlayIn()
    {
        base.PlayIn();
        tween?.Kill();
        tween = canvasGroup.DOFade(toValue, duration).SetEase(ease).OnComplete(() => OnComplete());
    }

    public override void PlayOut()
    {
        base.PlayOut();
        tween?.Kill();
        tween = canvasGroup.DOFade(fromValue, duration).SetEase(ease).OnComplete(() => OnComplete());
    }



    /// Complete
    protected override void OnComplete()
    {
        if (checkInteractable)
        {
            SetInteractable();
        }
        
        base.OnComplete();
    }

    private void SetInteractable()
    {
        if (canvasGroup.alpha == 0)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
}