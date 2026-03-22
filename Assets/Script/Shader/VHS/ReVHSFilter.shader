Shader "Hidden/ReVHSFilter"
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

            float rand(float2 co)
            {
                return frac(sin(dot(co, float2(12.9898,78.233))) * 43758.5453);
            }

            half4 Frag(Varyings i) : SV_Target
            {
                float3 srcRGB = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, i.texcoord).rgb;
                
                float2 uv = i.texcoord;
                float time = _Time.y  * _TimeScale;

                // 深度（raw）を取得して線形化（0=near,1=far）
                float rawDepth = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uv).r;
                float depth01 = saturate(rawDepth);  //近いほど値が大きい

                //step(a, x)と記述することで、 a =< xのとき1をa > xのときに0を返します
                float mask = 1.0 - step(_ShiftActiveDis, depth01);   // depth01 > 0.5 で 0

                float shiftAmount = 0.000 ;

                float rOffset = shiftAmount;
                float bOffset = -shiftAmount;

                float r = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + rOffset).r;
                float g = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv).g;
                float b = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + bOffset).b;

                float3 outRGB = float3(r, g, b);

                // //画面の縮尺に対応
                // float2 screenSize = float2(_ScreenParams.x, _ScreenParams.y); // x=幅, y=高さ
                // float pixelY = uv.y * screenSize.y; // 実際のピクセル座標

                // half  s = sin(pixelY * 100 + time * 50);
                // half  c = cos(pixelY * 100 + time * 50);

                // outRGB += half3(c,s,c) * _OpacityScanLine;

               
              

               
           
                
                return half4(outRGB,1.0);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
