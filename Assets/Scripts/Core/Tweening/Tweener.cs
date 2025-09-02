using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public abstract class UITween : MonoBehaviour
{
    /// Variables
    [SerializeField] protected bool playOnStart;
    [SerializeField] protected bool startIn;
    [SerializeField] protected float fromValue = 0f;
    [SerializeField] protected float toValue = 1f;
    [SerializeField] protected float duration = 1f;
    [SerializeField] protected Ease ease;
    private bool isPlayingIn;

    /// Tween
    protected Tween tween;

    /// Events
    [HideInInspector] public UnityEvent<bool> OnTweenStarted;
    [HideInInspector] public UnityEvent<bool> OnTweenComplete;



    /// Initialization
    protected abstract void Awake();

    protected virtual void Start()
    {
        if (playOnStart)
        {
            if (!startIn)
            {
                PlayIn();
            }
            else
            {
                PlayOut();
            }
        }
    }



    /// Play
    public virtual void PlayIn()
    {
        isPlayingIn = true;
        OnTweenStarted.Invoke(isPlayingIn);
    }

    public virtual void PlayOut()
    {
        isPlayingIn = false;
        OnTweenStarted.Invoke(isPlayingIn);
    }



    /// Complete
    public void Complete()
    {
        tween?.Complete();
    }

    public void Kill()
    {
        tween?.Kill();
    }

    protected virtual void OnComplete()
    {
        OnTweenComplete.Invoke(isPlayingIn);
    }

    private void OnDestroy()
    {
        Kill();
    }
}