Shader "Unlit/FloorShader"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		_Spread ("Spread", Float) = 0.0
		_FogColour ("Fog Colour", Color) = (0, 0, 0, 1)
		_LandTime ("Land Time", Float) = -10000.0
		_Amplitude ("Amplitude", Float) = 0
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma target 3.0

		uniform sampler2D _MainTex;

		#include "UnityCG.cginc"

		uniform float _Spread;
		uniform float2 _RippleOrigin;

		uniform float2 _LandOrigin;
		uniform float _LandTime;
		uniform float _Amplitude;

		struct Input
		{
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void vert(inout appdata_full v)
		{
			float landRippleSpeed = 5;
			// Displacement according to queen's proximity
			float distance = sqrt(pow(v.vertex.x - _RippleOrigin.x, 2) + pow(v.vertex.z - _RippleOrigin.y, 2));
			float height = _Amplitude * pow(2, -_Spread * distance);
			float period = sin(distance - landRippleSpeed * _Time.y);
			float4 displacement = float4(0.0f, height * period, 0.0f, 0.0f);
			v.vertex += displacement;

			// Displacement according to landed queen
			distance = sqrt(pow(v.vertex.x - _LandOrigin.x, 2) + pow(v.vertex.z - _LandOrigin.y, 2));
			float lifetime = _Time.y - _LandTime;
			if (distance < landRippleSpeed*lifetime) {
				height = 1 / (lifetime + 1) * pow(2, -0.4 * distance);
				period = sin(distance - landRippleSpeed*lifetime);
				displacement = float4(0.0f, height * period, 0.0f, 0.0f);
				v.vertex += displacement;
			}
		}
			
		void surf (Input IN, inout SurfaceOutput o)
		{
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
