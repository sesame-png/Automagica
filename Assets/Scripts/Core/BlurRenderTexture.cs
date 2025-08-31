using UnityEngine;

#pragma warning disable CS0618

[ExecuteAlways]
public class BlurRenderTexture : MonoBehaviour
{
    [SerializeField] private RenderTexture sourceTexture;
    [SerializeField] private RenderTexture targetTexture;
    [SerializeField] private Material blurMaterial;

    private void Awake()
    {
        if (!targetTexture)
        {
            targetTexture = new RenderTexture(Screen.width, Screen.height, 16);
            targetTexture.Create();
        }
    }

    private void Update()
    {
        targetTexture.Release();
        targetTexture.width = Screen.width;
        targetTexture.height = Screen.height;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, targetTexture, blurMaterial);
        Graphics.Blit(src, dest);
    }

    private void OnDisable()
    {
        targetTexture.Release();
    }
}