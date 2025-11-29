using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SetPalette : MonoBehaviour
{
    [SerializeField] private IntSO currentPalette;
    [SerializeField] private List<ColorListSO> palettes;

    [SerializeField] private Material primaryFrostedMat;
    [SerializeField] private Material secondaryFrostedMat;
    [SerializeField] private Material primaryMat;
    [SerializeField] private Material secondaryMat;

    private float tweenDuration = 2f;
    private Tween primaryFrostedTween;
    private Tween secondaryFrostedTween;
    private Tween primaryTween;
    private Tween secondaryTween;

    public void Set(int index)
    {
        ColorListSO palette;
        if (0 <= index && index < palettes.Count)
        {
            palette = palettes[index];
            currentPalette.SetValue(index);
        }
        else
        {
            palette = palettes[0];
            currentPalette.SetValue(0);
        }

        primaryFrostedTween?.Kill();
        secondaryFrostedTween?.Kill();
        primaryTween?.Kill();
        secondaryTween?.Kill();

        primaryFrostedTween = primaryFrostedMat.DOColor(palette.GetValue(0), "_Tint", tweenDuration).SetEase(Ease.OutQuint);
        secondaryFrostedTween = secondaryFrostedMat.DOColor(palette.GetValue(1), "_Tint", tweenDuration).SetEase(Ease.OutQuint);
        primaryTween = primaryMat.DOColor(palette.GetValue(2), tweenDuration).SetEase(Ease.OutQuint);
        secondaryTween = secondaryMat.DOColor(palette.GetValue(3), tweenDuration).SetEase(Ease.OutQuint);
    }
}