using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackgroundManager : MonoBehaviour
{
    // Components
    [SerializeField] private GameObject background;
    private GameObject nextBackground;

    // Variables
    public Sprite[] images;
    [SerializeField] private bool randomize;
    [SerializeField] private float imageDuration;
    [SerializeField] private float fadeDuration;
    private int index;

    // Tweens
    private Tween fadeTween;
    private bool isTweening;



    public void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(imageDuration);
        PlayNext();
    }

    public void Next()
    {
        if (isTweening) { return; }
        StopCoroutine(Timer());
        PlayNext();
    }

    private void PlayNext()
    {
        isTweening = true;

        if (randomize)
        {
            int i = index;
            while (index == i)
            {
                index = Random.Range(0, images.Length);
            }
        }
        else 
        {
            if (index >= images.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }

        nextBackground = background;
        background = Instantiate(nextBackground, nextBackground.transform.parent);
        
        nextBackground.GetComponent<Image>().sprite = images[index];
        nextBackground.GetComponent<AspectRatioFitter>().aspectRatio = images[index].rect.width / images[index].rect.height;

        fadeTween?.Kill();
        fadeTween = background.GetComponent<Image>().DOFade(0f, fadeDuration).SetEase(Ease.InOutQuad).OnComplete(() => OnComplete());
    }

    private void OnComplete()
    {
        Destroy(background);
        background = nextBackground;
        isTweening = false;
        StartCoroutine(Timer());
    }
}