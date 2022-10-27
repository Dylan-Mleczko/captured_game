Shader "Unlit/FloorShader"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		_Spread ("Spread", Float) = 0.0
		_LandTime ("Land Time", Float) = -10000.0
		_Amplitude ("Amplitude", Float) = 0
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

		// use default lighting model, manipulate vertices, use shader model version 3.0
		// requires lights with non-zero bias
		#pragma surface surf Standard vertex:vert fullforwardshadows
		#pragma target 3.0

		uniform sampler2D _MainTex;

		#include "UnityCG.cginc"

		// uniform global parameters
		half _Spread; 	 // the rate of decay of waves
		uniform half _Amplitude; // the multiplier for the height of waves
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// non-uniform parameters
		half2 _RippleOrigin;
		half _LandTime;
		half2 _LandOrigin;

		// input data type for surf function
		struct Input
		{
			float2 uv_MainTex;
			// float3 worldPos;
			// float3 viewDir;
			// float4 name : COLOR;
			// float4 screenPos;
			// float3 worldRefl;
			// float3 worldNormal;
		};

		void vert(inout appdata_full v)
		{
			half landRippleSpeed = 5;
			// Displacement according to queen's proximity
			half distance = sqrt(pow(v.vertex.x - _RippleOrigin.x, 2) + pow(v.vertex.z - _RippleOrigin.y, 2));
			half height = _Amplitude * pow(2, -_Spread * distance);
			half period = sin(distance - landRippleSpeed * _Time.y);
			half4 displacement = float4(0.0f, height * period, 0.0f, 0.0f);
			v.vertex += displacement;

			// Displacement according to landed queen
			distance = sqrt(pow(v.vertex.x - _LandOrigin.x, 2) + pow(v.vertex.z - _LandOrigin.y, 2));
			half lifetime = _Time.y - _LandTime;
			if (distance < landRippleSpeed*lifetime) {
				height = 0 / (lifetime + 1) * pow(2, -0.4 * distance);
				period = sin(distance - landRippleSpeed*lifetime);
				displacement = float4(0.0f, height * period, 0.0f, 0.0f);
				v.vertex += displacement;
			}
		}
			
		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
