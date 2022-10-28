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
            #pragma vertex vert_img
            #pragma fragment frag
            // make fog work
            // #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Proximity;
            float _Strength;

            // struct appdata
            // {
            //     float4 vertex : POSITION;
            //     float2 uv : TEXCOORD0;
            // };

            // struct v2f
            // {
            //     float2 uv : TEXCOORD0;
            //     UNITY_FOG_COORDS(1)
            //     float4 vertex : SV_POSITION;
            // };

            // sampler2D _MainTex;

            // v2f vert (appdata v)
            // {
            //     v2f o;
            //     o.vertex = UnityObjectToClipPos(v.vertex);
            //     o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            //     UNITY_TRANSFER_FOG(o,o.vertex);
            //     return o;
            // }

            fixed4 frag (v2f_img i) : COLOR
            {
                fixed4 c = tex2D(_MainTex, i.uv);
                float lum = c.r * 0.3 + c.g * 0.59 + c.b * 0.11;
                float3 bw = float3(lum, lum, lum);
                float4 result = c;
                result.rgb = lerp(c.rgb, bw, _Strength);
                return result;
            }
            ENDCG
        }
    }
}
