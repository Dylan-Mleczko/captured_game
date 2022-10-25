Shader "Unlit/WaveShader"
{
	Properties
	{
        _Texture ("Texture", 2D) = "white" {}
		_Spread ("Spread", Float) = 0.0
		_FogColour ("Fog Colour", Color) = (0, 0, 0, 1)
		_LandTime ("Land Time", Float) = -10000.0
		_Amplitude ("Amplitude", Float) = 0
	}
	SubShader
	{
		Pass {
			Tags { "RenderType" = "Opaque" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _Texture;
			float4 _Main_Tex_ST;
			float4 _LightPoint;
			uniform Float _Spread;
			uniform float2 _RippleOrigin;

			uniform float2 _LandOrigin;
			uniform float _LandTime;
			uniform float _Amplitude;

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float3 worldPosition : TEXCOORD2;
				UNITY_FOG_COORDS(1)
			};
				
				
				
			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				// Displacement according to queen's proximity
				Float distance = sqrt(pow(v.vertex.x - _RippleOrigin.x, 2) + pow(v.vertex.z - _RippleOrigin.y, 2));
				Float height = _Amplitude * pow(2, -_Spread * distance);
				Float period = sin(distance - 5 * _Time.y);
				float4 displacement = float4(0.0f, height * period, 0.0f, 0.0f);
				v.vertex += displacement;

				// Displacement according to landed queen
				distance = sqrt(pow(v.vertex.x - _LandOrigin.x, 2) + pow(v.vertex.z - _LandOrigin.y, 2));
				Float lifetime = _Time.y - _LandTime;
				if (distance < lifetime) {
					height = 0.5 / (lifetime + 1) * pow(2, -0.05 * distance);
					period = sin(distance - lifetime);
					displacement = float4(0.0f, height * period, 0.0f, 0.0f);
					v.vertex += displacement;
				}

				vertOut o;

				// Apply the model and view matrix to the vertex (but not the projection matrix yet)
				v.vertex = mul(UNITY_MATRIX_MV, v.vertex);
				
				// Finally apply the projection matrix to complete the transformation into screen space
				o.vertex = mul(UNITY_MATRIX_P, v.vertex);
				o.uv = v.uv;
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				// fixed3 lightDifference = v.worldPosition - _LightPoint.xyz;
				// fixed3 lightDirection = normalize(lightDifference);
				// fixed intensity = -1 * dot(lightDirection, v.worldNormal);
				// fixed4 col = intensity * tex2D(_Texture, v.uv);

				fixed4 col = tex2D(_Texture, v.uv);
				return col;
			}

			ENDCG
		}
	}
	Fallback "Diffuse"
}
