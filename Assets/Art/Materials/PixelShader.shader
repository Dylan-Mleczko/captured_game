Shader "Hidden/PixelShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Proximity ("Proximity", Float) = 0.0
        _Noise ("Noise", 2D) = "white" {}
    }
    SubShader
    {
        // Tags { "RenderType"="Opaque" }
        // LOD 100

        Pass
        {
            CGPROGRAM
            #pragma target 1.0
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Proximity;
            sampler2D _Noise;
            int _ScreenWidth;
            int _ScreenHeight;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                // float3 pos : TEXCOORD1;
            };

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // float noiseValue(float2 uv) {
            //     float scale = 10;

            //     int x = uv.x;
            //     int y = uv.y;

            //     float xCoord = x / (float)_ScreenWidth  * scale;
            //     float yCoord = y / (float)_ScreenHeight * scale;

            //     float3 coord = float3(xCoord, yCoord, 0);
            //     return 0;
            // }

            fixed4 frag (v2f i) : COLOR
            {
                fixed4 c = tex2D(_MainTex, i.uv);
                // float shade = noiseValue(i.uv);
                // c = fixed4(shade, shade, shade, 0);
                // c = tex2D(_Noise, i.uv);
                // return tex;

                // float dist = length(i.vertex);
                // float k = length(i.pos);
                // col.a *= k;

                float effectDistance = 12;
                // scale multiplier from 0 to 1 based on distance to closest piece
                float multiplier = saturate(1.0 - min(effectDistance, _Proximity)/effectDistance);

                // grayscale on approaching pieces
                float brightness = c.r + c.g + c.b;
                float3 shade = float3(brightness, brightness, brightness);
                float4 result = c;
                result.rgb = lerp(c.rgb, shade, multiplier);

                // darkness on approaching pieces
                float2 centre = float2(0.5, 0.5);
                float maxRadius = length(centre);
                float radius = (1.0 - multiplier) * maxRadius;
                // float maxRadius = (1.0 - multiplier) * length(centre);
                // float radius = (1.0 - multiplier*2) * maxRadius;
                float pixDistFromCentre = length(i.uv - centre);
                if (multiplier > 0) {
                    result.rgb = lerp(result.rgb, float3(0, 0, 0), max(0, pixDistFromCentre - radius) / (maxRadius - radius));
                }

                return result;
            }
            ENDCG
        }
    }
}