//風呂場の水用のシェーダー
Shader "Hidden/BathWater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _WaveSpeed("Wave Speed", Vector) = (0.1, 0.1, 0, 0)
        _Distortion("Distortion Strength", Float) = 0.05
        _FresnelPower("Fresnel Power", Float) = 3.0
        _Color("Water Color", Color) = (0.5,0.7,1,0.5)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _NormalMap;
            float4 _WaveSpeed;
            float _Distortion;
            float _FresnelPower;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv + _WaveSpeed.xy * _Time.y;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float3 normalTex = tex2D(_NormalMap, i.uv).rgb * 2 - 1;
                float2 distortion = normalTex.xy * _Distortion;
                float4 baseCol = tex2D(_MainTex, i.uv + distortion);

                // Fresnelで縁光
                float fresnel = pow(1.0 - dot(i.viewDir, float3(0,1,0)), _FresnelPower);

                float4 col = baseCol * _Color;
                col.rgb += fresnel; // 光沢の加算
                col.a = _Color.a;
                return col;
            }
            ENDCG
        }
    }
}
