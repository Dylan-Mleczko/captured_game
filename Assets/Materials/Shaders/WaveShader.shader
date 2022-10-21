Shader "Unlit/WaveShader"
{
	Properties
	{
        _Texture ("Texture", 2D) = "white" {}
		// _Ripples ("Ripples", float4[]) = []
		_Spread ("Spread", Float) = 0.0
		_FogColour ("Fog Colour", Color) = (0, 0, 0, 1)
		// _Glossiness ("Glossiness", Range(0, 1)) = 0.5
		// _Metallic ("Metallic", Range(0, 1)) = 0
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
			uniform float2 _Ripples;
			// half _Glossiness;
			// half _Metallic;

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
				// Displace the original vertex in model space
				// for (unit i = 0; i < _Ripples.Length; i++) {
				// }
				Float distance = sqrt(pow(v.vertex.x - _Ripples.x, 2) + pow(v.vertex.z - _Ripples.y, 2));
				Float height = 0.0 * pow(2, -_Spread * distance);
				Float period = sin(distance - 10*_Time.y);
				float4 displacement = float4(0.0f, height * period, 0.0f, 0.0f);
				v.vertex += displacement;

				vertOut o;

				// Apply the model and view matrix to the vertex (but not the projection matrix yet)
				v.vertex = mul(UNITY_MATRIX_MV, v.vertex);
				
				// Finally apply the projection matrix to complete the transformation into screen space
				o.vertex = mul(UNITY_MATRIX_P, v.vertex);
				o.uv = v.uv;
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				fixed3 lightDifference = v.worldPosition - _LightPoint.xyz;
				fixed3 lightDirection = normalize(lightDifference);
				fixed intensity = -1 * dot(lightDirection, v.worldNormal);
				fixed4 col = intensity * tex2D(_Texture, v.uv);

				// fixed4 col = tex2D(_Texture, v.uv);
				// UNITY_APPLY_FOG(v.fogCoord, col);
				return col;
			}

			ENDCG
		}
	}
	Fallback "Diffuse"
}
