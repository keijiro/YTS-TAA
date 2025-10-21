Shader "YTS-TAA/Accumulation (Naive)"
{
    Properties
    {
        _MainTex("Source", 2D) = "Black"{}
        _BlendRatio("Blend Ratio", Float) = 0.1
    }

    HLSLINCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Assets/Common/CustomRenderTexture.hlsl"

TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
float _BlendRatio;

float4 FragmentUpdate(CustomRenderTextureVaryings i) : SV_Target
{
    float2 uv = i.globalTexcoord.xy;
    float3 c0 = SAMPLE_TEXTURE2D(_SelfTexture2D, sampler_SelfTexture2D, uv).rgb;
    float3 c1 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).rgb;
    return float4(lerp(c0, c1, _BlendRatio), 1);
}

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            Name "Update"
            HLSLPROGRAM
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment FragmentUpdate
            ENDHLSL
        }
    }
}
