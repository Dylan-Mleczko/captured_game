Shader "Unlit/WaveShader"
{
	Properties
	{
        _Texture ("Texture", 2D) = "white" {}
		_Spread ("Spread", Float) = 0.0
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

			uniform sampler2D _Texture;

			// variables defined externally
			uniform float2 _Ripples;
			uniform Float _Spread;

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
				// for (unit i = 0; i < _Ripples.Length; i++) {

				// }
				Float distance = sqrt(pow(v.vertex.x - _Ripples.x, 2) + pow(v.vertex.z - _Ripples.y, 2));
				Float height = 0.05 * pow(2, -_Spread * distance);
				Float period = sin(distance - 10*_Time.y);
				float4 displacement = float4(0.0f, height * period, 0.0f, 0.0f);
				v.vertex += displacement;

				vertOut o;

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
				fixed4 col = tex2D(_Texture, v.uv);
				return col;
			}
			ENDCG
		}
	}
}
