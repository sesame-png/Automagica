// https://www.shadertoy.com/view/Xltfzj
// https://discussions.unity.com/t/urp-sprite-gaussian-blur-customer-subshadergraph/892367
void GaussianBlur_float(sampler2D _Texture, float2 _UV, float _Directions, float _Quality, float _Size, out float4 _Output)
{
    float pi = 6.28318530718;
    
    float aspectRatio = _ScreenParams.x / _ScreenParams.y;
    //float aspectRatio = Texture.texelSize.z / Texture.texelSize.w;
    
    float2 radius = float2(_Size / aspectRatio, _Size);
    //float2 radius = size;
    //float2 radius = size / Texture.texelSize;
    
    // Normalized pixel coordinates (from 0 to 1)
    //float2 normalizedUV = UV / Texture.texelSize;
    //float2 normalizedUV = UV * aspectRatio;
    float2 normalizedUV = float2(_UV.x * aspectRatio, _UV.y);
    
    float4 color;
    for (float d = 0.0; d < pi; d += pi / _Directions)
    {
        for (float i = 1.0 / _Quality; i < 1.001; i += 1.0 / _Quality)
        {
            color += tex2D(_Texture, _UV + float2(cos(d), sin(d)) * radius * i);
        }
    }
    
    color /= _Quality * _Directions + 1.0;
    _Output = color;
}