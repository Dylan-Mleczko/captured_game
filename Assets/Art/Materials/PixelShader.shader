Shader "Hidden/PixelShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Proximity ("Proximity", Float) = 0.0
        _Strength ("Strength", Float) = 0.0
    }
    SubShader
    {
        // Tags { "RenderType"="Opaque" }
        // LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Proximity;
            float _Strength;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 pos : TEXCOORD1;
            };

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // o.pos = UnityObjectToViewPos(v.vertex);
                return o;
            }

            // struct v2f
            // {
            //     float2 uv : TEXCOORD0;
            //     float4 vertex : SV_POSITION;
            //     // UNITY_FOG_COORDS(1)
            //     // float distance;
            // };

            // // sampler2D _MainTex;

            // v2f vert (appdata v)
            // {
            //     v2f o;
            //     // o.vertex = v.vertex;
            //     o.vertex = UnityObjectToClipPos(v.vertex);
            //     o.uv = v.uv;
            //     // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            //     // UNITY_TRANSFER_FOG(o,o.vertex);
            //     return o;
            // }

            fixed4 frag (v2f i) : COLOR
            {
                // float dist = length(i.vertex);
                fixed4 tex = tex2D(_MainTex, i.uv);
                // float k = length(i.pos);
                // col.a *= k;
                return tex;
                // float lum = c.r * 0.3 + c.g * 0.59 + c.b * 0.11;
                // float3 bw = float3(lum, lum, lum);
                // float4 result = c;
                // result.rgb = lerp(c.rgb, bw, _Strength);
                // return result;
            }
            ENDCG
        }
    }
}
