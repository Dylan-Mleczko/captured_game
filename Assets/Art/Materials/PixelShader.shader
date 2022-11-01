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

            // float  mod289(float x) { return x - floor(x * (1.0 / 289.0)) * 289.0;}
            // float4 mod289(float4 x){ return x - floor(x * (1.0 / 289.0)) * 289.0;}
            // float4 perm(float4 x)  { return mod289(((x * 34.0) + 1.0) * x);}

            // float gnoise(float3 p){
            //     float3 a = floor(p);
            //     float3 d = p - a;
            //     d = d * d * (3.0 - 2.0 * d);

            //     float4 b = a.xxyy + float4(0.0, 1.0, 0.0, 1.0);
            //     float4 k1 = perm(b.xyxy);
            //     float4 k2 = perm(k1.xyxy + b.zzww);

            //     float4 c = k2 + a.zzzz;
            //     float4 k3 = perm(c);
            //     float4 k4 = perm(c + 1.0);

            //     float4 o1 = frac(k3 * (1.0 / 41.0));
            //     float4 o2 = frac(k4 * (1.0 / 41.0));

            //     float4 o3 = o2 * d.z + o1 * (1.0 - d.z);
            //     float2 o4 = o3.yw * d.x + o3.xz * (1.0 - d.x);

            //     return o4.y * d.y + o4.x * (1.0 - d.y);
            // }

            float noiseValue(float2 uv) {
                float scale = 10;

                int x = uv.x;
                int y = uv.y;

                float xCoord = x / (float)_ScreenWidth  * scale;
                float yCoord = y / (float)_ScreenHeight * scale;

                float3 coord = float3(xCoord, yCoord, 0);
                // float noisiness = noise(coord);
                // float noisiness = gnoise(coord);
                return 0;
            }

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
                // float radius = (1.0 - multiplier) * maxRadius;
                float2 centre = float2(_ScreenWidth/2.0, _ScreenHeight/2.0);
                float distFromCentre = length(centre);
                float pixDistFromCentre = length(i.uv - float2(0.5, 0.5));
                // if (multiplier > pixDistFromCentre / maxDistFromCentre) {
                if (multiplier > 0) {
                    result.rgb = lerp(result.rgb, float3(0, 0, 0), pixDistFromCentre*2);
                }
                // float darknessAlpha = max(0, length(i.uv) - radius) / maxRadius;
                // result.rgb = lerp(result.rgb, darkness)

                return result;
            }
            ENDCG
        }
    }
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
