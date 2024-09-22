Shader "Unlit/healthbar"
{
	Properties
	{
		_MainTex ("_MainTex", 2D) = "white" {}
		_Percent("_Percent" , range (0,1)) = 1 
		_Percent2("_Percent2" , range (0,1)) = 1 
		_Flash("_Flash" , range (0,1)) = 1 
      	_Color ("Color", Color) = (1,0.2,0.2,1)

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Percent ;
			float _Percent2 ;
			float _Flash ;
			float4 _Color ;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 tex = tex2D(_MainTex, i.uv);
				fixed4 col = tex ;
				fixed4 barProgress = step(i.uv.x ,  _Percent) ;
				fixed4 barProgress2 = step(i.uv.x ,  _Percent2) - barProgress ;

				barProgress = lerp ( barProgress * tex,_Color * tex.r,barProgress2) ;


				barProgress *= tex.a ; 

				col = lerp(barProgress,col,1-tex.a) ;
				col += _Flash * (1-tex.a);

				return col;
			}
			ENDCG
		}
	}
}
