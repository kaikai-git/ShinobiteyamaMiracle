Shader "Hidden/Vinette"
{
     Properties
    {
        _OpacityScanLine ("Opacity ScanLine", Range(0, 1)) = 0.03
        _TimeScale("Time Scale", Float) = 1.0               // 時間の速さ。ノイズのアニメーション速度に影響。
        _ShiftActiveDis("ShiftActiveDis Scale", Range(0, 1)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        Cull Off ZWrite Off ZTest Always



        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            
            
            half _OpacityScanLine;
            float _TimeScale;
            float _ShiftActiveDis;

            
            TEXTURE2D(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            float ShirtProcess(float2 uv, float t)
            {
               
            }

            half4 Frag(Varyings i) : SV_Target
            {
                float3 srcRGB = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, i.texcoord).rgb;
                
                float2 uv = i.texcoord;

               
              
                uv = 2. * uv - 1.;  //0 1から-1 1に

                srcRGB *= 1.0 - dot(uv,uv);
               
              
               return half4(srcRGB,1.0);
            }
            ENDHLSL
        }
    }
}
