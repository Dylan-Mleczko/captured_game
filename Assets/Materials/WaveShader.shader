Shader "Unlit/WaveShader"
{
	Properties
	{
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Position ("Position", Float) = 0.0
	}
	SubShader
	{
		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _Position;

			struct vertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				// Displace the original vertex in model space
				float4 displacement = float4(0.0f, _Position + sin(sqrt(pow(v.vertex.x, 2) + pow(v.vertex.z, 2)) + _Time.y), 0.0f, 0.0f);
				v.vertex += displacement;

				vertOut o;

				// Task 8 - Need to apply wave transformation between MV and P!
				// Apply the model and view matrix to the vertex (but not the projection matrix yet)
				v.vertex = mul(UNITY_MATRIX_MV, v.vertex);
				
				// Finally apply the projection matrix to complete the transformation into screen space
				o.vertex = mul(UNITY_MATRIX_P, v.vertex);

				o.uv = v.uv;
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv);
				return col;
			}
			ENDCG
		}
	}
}
