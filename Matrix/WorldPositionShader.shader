// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/WorldPositionShader"
{
    Properties
	{
		[HDR]_Color ("Color",Color)=(0,0,0)
		_ColorRange ("ColorRange",float)=0
	}
    SubShader {
		Pass {
			CGPROGRAM
 
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
 
			//add _Color
			//uniform fixed4 _Color;
			fixed4 _Color;
			float _ColorRange;
 
			struct a2v {
				float4 vertex : POSITION;
			};
 
			struct v2f {

				float4 pos : SV_POSITION;
				float3 color : COLOR0;	
			};
 
			v2f vert(a2v v) {
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				 //这里除法是比例缩放，将世界坐标变换到合适的范围
				o.color = mul(unity_ObjectToWorld, v.vertex).xyz /_ColorRange; 
				
				return o;
			}
 
			float4 frag(v2f i) : SV_Target {
 
				//float dis = length(i.color);
				//return float4(dis, dis, dis, 1.0);	//输出灰度时使用
 
				float3 f_color = abs(i.color);
				return float4(f_color, 1.0);
			}
 
			ENDCG
		}
	}
}
