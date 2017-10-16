Shader "Custom/Ring"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Radius("Radius", Range(0, 0.5)) = 0
		_Thickness("Thickness", Range(0.0, 0.5)) = 0
		_Min("Min", Range(0.0, 0.5)) = 0
		_Max("Max", Range(0.0, 0.5)) = 0
		_Speed("Speed", Range(0.0, 10)) = 0
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag alpha

			#include "UnityCG.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float4 _Color;

			float _Radius;
			float _Thickness;

			float _Min;
			float _Max;

			float _Speed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float4 col = _Color;
				float2 vec = i.uv - float2(0.5, 0.5);
				
				float t = (sin(_Time.w * _Speed) + 1) * 0.5;
				float t2 = _Thickness * 0.5;

				_Radius = lerp(_Min, _Max, t);

				float lower = _Radius - t2;
				float upper = _Radius + t2;

				float l = length(vec);

				col.a = 1 - step(step(l, upper), step(l, lower));
				
				return col;
			}
			ENDCG
		}
	}
}
