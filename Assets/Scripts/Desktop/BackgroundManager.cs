using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    /// Components
    [SerializeField] private GameObject background;
    private GameObject nextBackground;

    /// Variables
    public Sprite[] images;
    [SerializeField] private bool randomize;
    [SerializeField] private float imageDuration;
    [SerializeField] private float fadeDuration;
    private int currentIndex;

    /// Tweens
    private Tween fadeTween;



    public void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(imageDuration);
        PlayNext();
    }

    private void PlayNext()
    {
        if (randomize)
        {
            int i = currentIndex;
            while (currentIndex == i)
            {
                currentIndex = Random.Range(0, images.Length);
            }
        }
        else 
        {
            if (currentIndex >= images.Length - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
        }

        nextBackground = background;
        background = Instantiate(nextBackground, nextBackground.transform.parent);
        
        nextBackground.GetComponent<Image>().sprite = images[currentIndex];
        nextBackground.GetComponent<AspectRatioFitter>().aspectRatio = images[currentIndex].rect.width / images[currentIndex].rect.height;

        fadeTween?.Kill();
        fadeTween = background.GetComponent<Image>().DOFade(0f, fadeDuration).SetEase(Ease.InOutQuad).OnComplete(() => OnComplete());
    }

    private void OnComplete()
    {
        Destroy(background);
        background = nextBackground;
        StartCoroutine(Timer());
    }
}