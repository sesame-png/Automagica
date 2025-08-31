using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public abstract class Tweener : MonoBehaviour
{
    //variables
    [SerializeField] protected bool playOnStart;
    [SerializeField] protected bool startIn;
    [SerializeField] protected float fromValue = 0f;
    [SerializeField] protected float toValue = 1f;
    [SerializeField] protected float duration = 1f;
    [SerializeField] protected Ease ease;

    //tween
    protected Tween tween;

    //events
    [HideInInspector] public UnityEvent tweenStarted;
    [HideInInspector] public UnityEvent tweenComplete;
    //public Event<bool> tweenStarted = new Event<bool>();
    //public Event<bool> tweenComplete = new Event<bool>();



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

    public virtual void PlayIn()
    {
        tweenStarted.Invoke();
    }

    public virtual void PlayOut()
    {
        tweenStarted.Invoke();
    }

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
        tweenComplete.Invoke();
    }

    private void OnDestroy()
    {
        Kill();
    }
}