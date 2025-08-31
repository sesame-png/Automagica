// https://www.shadertoy.com/view/Xltfzj
// https://discussions.unity.com/t/urp-sprite-gaussian-blur-customer-subshadergraph/892367
void GaussianBlur_float(UnityTexture2D Texture, float2 UV, float directions, float quality, float size, out float4 Output)
{
    float pi = 6.28318530718;
    
    float aspectRatio = _ScreenParams.x / _ScreenParams.y;
    //float aspectRatio = Texture.texelSize.z / Texture.texelSize.w;
    
    float2 radius = float2(size / aspectRatio, size);
    //float2 radius = size;
    //float2 radius = size / Texture.texelSize;
    
    // Normalized pixel coordinates (from 0 to 1)
    //float2 normalizedUV = UV / Texture.texelSize;
    //float2 normalizedUV = UV * aspectRatio;
    float2 normalizedUV = float2(UV.x * aspectRatio, UV.y);
    
    // Pixel colour
    float4 color;
    
    // Blur calculations
    for (float d = 0.0; d < pi; d += pi / directions)
    {
        for (float i = 1.0 / quality; i < 1.001; i += 1.0 / quality)
        {
            color += tex2D(Texture, UV + float2(cos(d), sin(d)) * radius * i);
        }
    }
    
    // Output to screen
    color /= quality * directions + 1.0;
    Output = color;
}