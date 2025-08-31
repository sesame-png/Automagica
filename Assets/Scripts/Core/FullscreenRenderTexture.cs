using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class FullscreenRenderTexture : MonoBehaviour
{
    private Camera renderCamera;
    private RenderTexture renderTexture;

    private void Awake()
    {
        renderCamera = GetComponent<Camera>();

        if (renderCamera.targetTexture)
        {
            renderTexture = renderCamera.targetTexture;
        }
        else
        {
            renderTexture = new RenderTexture(Screen.width, Screen.height, 16);
            renderTexture.Create();
        }
    }

    private void OnDisable()
    {
        renderTexture.Release();
    }

    private void Update()
    {
        renderTexture.Release();
        renderTexture.width = Screen.width;
        renderTexture.height = Screen.height;
        renderCamera.ResetAspect();
    }
}