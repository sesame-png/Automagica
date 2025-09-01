using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class BlurRenderTexture : MonoBehaviour
{
    private Camera renderCamera;
    [SerializeField] private RenderTexture sourceTexture;
    [SerializeField] private RenderTexture targetTexture;
    [SerializeField] private Material blurMaterial;

    private void Awake()
    {
        renderCamera = GetComponent<Camera>();

        if (!targetTexture)
        {
            targetTexture = new RenderTexture(Screen.width, Screen.height, 16);
            targetTexture.Create();
        }

        CommandBuffer commandBuffer = new CommandBuffer();
        commandBuffer.name = "GaussianBlur";
        commandBuffer.Blit(sourceTexture, targetTexture, blurMaterial);
        renderCamera.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
    }

    private void Update()
    {
        targetTexture.Release();
        targetTexture.width = Screen.width;
        targetTexture.height = Screen.height;
    }

    private void OnDisable()
    {
        targetTexture.Release();
    }
}