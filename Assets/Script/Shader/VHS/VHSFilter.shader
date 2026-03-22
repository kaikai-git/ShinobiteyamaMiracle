Shader "Hidden/VHSFilter"
{
    Properties
    {
        //グレインノイズ用変数
        _GrainNoiseIntensity("GrainNoise Intensity", Range(0,1)) = 0.25// グレインノイズの強さ（0..1）。0で元映像のみ、1でノイズのみ
        _BlockNoiseIntensity("BlockNoise Intensity", Range(0,1)) = 0.25// ブロックノイズの強さ（0..1）。0で元映像のみ、1でノイズのみ
        _NoiseIntensity("Noise Intensity", Range(0,1)) = 0.25// ブロックノイズの強さ（0..1）。0で元映像のみ、1でノイズのみ
         _OpacityScanLine ("Opacity ScanLine", Range(0, 2)) = 0.8
         _TimeScale("Time Scale", Float) = 1.0               // 時間の速さ。ノイズのアニメーション速度に影響。
        //

        _NoiseY("NoiseY (vertical position)", Float) = 0.0  // 縦方向の特定位置に“ブロックノイズ”を出すための位置（元コード互換オプション）。
        _StripeCount("Stripe Count", Float) = 500.0         // 縦ストライプの数（密度）。大きいほど細いストライプ。
        
        _Saturation("Saturation", Range(0,1)) = 1.0         //色の彩度を調整するための変数 0完全にモノクロ、1 元の色のまま
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
            
            //変数にはプロパティの値が自動で代入される、、らしい
            float _BlockNoiseIntensity;
            float _GrainNoiseIntensity;
            float _NoiseIntensity;
            float _TimeScale;
            float _NoiseY;
            float _StripeCount;
            float _OpacityScanLine;
            float _Saturation;

            // Hash / ノイズ（軽量）
            float hashf(float n) 
            { 
                //43758.5453123 という大きな定数を掛けて乱雑化し、frac()（小数部分を取る）ことで、[0,1) の範囲の値にする。
                return frac(sin(n) * 43758.5453123);
            }
            
            //float2 p（座標など2次元の値）を入力。
            float hash2(float2 p)
            { 
                //127.1 と 311.7 は「ノイズパターンが均等にばらけるようにする」ための係数。
                //hashf() に渡して、最終的に [0,1) の乱数に変換。
                return hashf(p.x * 127.1 + p.y * 311.7); 
            }

            // // 簡易テープノイズ（元の nn を簡略）
            // float tapeNoise(float2 uv, float t)
            // {
            //     float y = uv.y * _NoiseScale;
            //     float s = t * 2.0;
            //     float v = hash2(float2(floor(y * 10.0 + s), floor(uv.x * 10.0)));
            //     v *= hash2(float2(floor(y * 7.0 + 100.0 + s), floor(uv.x * 5.0)));
            //     v *= hash2(float2(floor(y * 3.0 + 421.0 + s), floor(uv.x * 3.0)));
            //     v = pow(v + 0.3, 1.0);
            //     if (v < 0.65) v = 0.0;
            //     return v;
            // }

            float tapeNoise(float2 uv, float t)
            {
                // ブロックの高さを決める
                float blockHeight = 0.03; // 0.03の高さごとに同じノイズになる

                // uv.y をブロック単位で丸める
                float yBlock = floor(uv.y / blockHeight);

                // 丸めた座標 + 時間でハッシュを生成
                return hashf(yBlock + t * _TimeScale);
            }
            
            half4 Frag(Varyings i) : SV_Target
            {
                // _BlitTexture 画面全体（あるいはカメラからの出力）のテクスチャ 元の映像を取得
                //sampler_LinearClamp テクスチャをサンプリングするときの補間方法と境界処理の指定
                //i.texcoord  現在処理しているピクセルの UV座標。
                //float3 src結果として得られる色
                float3 srcRGB = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, i.texcoord).rgb;

                float2 uv = i.texcoord;

                float t = _Time.y * _TimeScale;

                //彩度低下させる
                float gray = dot(srcRGB, float3(0.299, 0.587, 0.114)); // 輝度だけの色
                float3 desaturated = lerp(float3(gray,gray,gray), srcRGB, saturate(_Saturation)); // _Saturation = 0で完全モノクロ、1で元色

                 // 色ずれ
                desaturated.r += sin(t*5.0) * 0.05;
                desaturated.b += cos(t*3.0) * 0.05;

                float3 outRGB = float3(0, 0, 0);
               float s = sin(uv.y * 1000);
               float c = cos(uv.y * 1000);
                outRGB += float3(c, s, c) * _OpacityScanLine;

                //時間経過をもとに、ノイズアニメーション用の時間パラメータを作る
               
                //float n = tapeNoise(uv2, t);
                
                //uv.x * 1000.0 → 横方向の変化を強める
                //uv.y * 10000.0 → 縦方向の変化をさらに強める（横より縦の方が目立ちやすいので強めにしている）
                //UV座標だけではノイズは静止したままになるため、時間を足してノイズが動くようにする

                float grainNoise = hashf(uv.x * 1000.0 + uv.y * 10000.0 + t);    
                // 縦ブロックノイズ
                float blockNoise = tapeNoise(uv, t);
              
                // 強さを別々に調整して足す
                float col = grainNoise * _GrainNoiseIntensity + blockNoise * _BlockNoiseIntensity;
                col = saturate(col); // 0..1にクランプ

                // R, G, B 全てのチャンネルに同じ値を入れるてグレースケールにする
                float3 noiseRGB = float3(col, col, col);

                // ノイズとブレンド
                 outRGB = lerp(srcRGB, noiseRGB, saturate(_NoiseIntensity));

                return half4(outRGB,1.0);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
