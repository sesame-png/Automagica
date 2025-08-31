using UnityEngine;

[ExecuteAlways]
public class BlurRenderTexture : MonoBehaviour
{
    [SerializeField] private RenderTexture sourceTexture;
    [SerializeField] private RenderTexture targetTexture;
    [SerializeField] private Material blurMaterial;

    private void Awake()
    {
        /*if (!targetTexture)
        {
            targetTexture = new RenderTexture(Screen.width, Screen.height, 16);
            targetTexture.Create();
        }*/
    }

    private void OnDisable()
    {
        targetTexture.Release();
    }

    private void Update()
    {
        targetTexture.Release();
        targetTexture.width = sourceTexture.width;
        targetTexture.height = sourceTexture.height;
        Graphics.Blit(sourceTexture, targetTexture, blurMaterial, -1);
        //targetTexture.Create();
        //Graphics.CopyTexture(sourceTexture, targetTexture);
        //Graphics.Blit(sourceTexture, targetTexture);
    }
}