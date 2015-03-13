Shader "Custom/BoxBlur" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_kernelSize ("Kernel Size", float) = 1
		}
SubShader {
Tags { "RenderType"="Opaque" }

Pass{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

float _kernelSize;
sampler2D _MainTex;
struct v2f {
   float4 pos : SV_POSITION;
   float4 scrPos:TEXCOORD1;
   float2 uv_MainTex: TEXCOORD;
};

//Vertex Shader
v2f vert (appdata_base v){
   v2f o;
   o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
   o.scrPos=ComputeScreenPos(o.pos);
   o.uv_MainTex = v.texcoord;
   //for some reason, the y position of the depth texture comes out inverted
   return o;
}

	float4 convolve(float2 coord)
		{
			float blurAmount = 0.002;
			float4 total;
			float sum = 0;
			for(float i = -_kernelSize; i <= _kernelSize; i++)
			{	
				for(float j = -_kernelSize; j <= _kernelSize; j ++)
				{
					float2 c = float2(coord.x + i * blurAmount, coord.y + j * blurAmount) ; 
					sum ++;
					total +=  tex2D(_MainTex, c); 
				}
			}
			return total / sum;
		}

//Fragment Shader
half4 frag (v2f i) : COLOR{
	float dist = 1 - sqrt(pow(i.uv_MainTex -0.5.x,2) + pow(i.uv_MainTex.y-0.5,2));
	dist = smoothstep(0.2, 0.8, dist);
	return convolve(i.uv_MainTex) * dist;
}
ENDCG
}
GrabPass{}

Pass{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

float _kernelSize;
 sampler2D _GrabTexture;
struct v2f {
   float4 pos : SV_POSITION;
   float4 scrPos:TEXCOORD1;
   float2 uv_MainTex: TEXCOORD;
};

//Vertex Shader
v2f vert (appdata_base v){
   v2f o;
   o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
   o.scrPos = ComputeScreenPos(o.pos);
   o.uv_MainTex = v.texcoord;
   o.uv_MainTex.y = 1 - o.uv_MainTex.y;
   //for some reason, the y position of the depth texture comes out inverted
   return o;
}

	float4 convolve(float2 coord)
		{
			float blurAmount = 0.002;
			float4 total;
			float sum = 0;
			for(float i = -_kernelSize; i <= _kernelSize; i++)
			{	
				for(float j = -_kernelSize; j <= _kernelSize; j ++)
				{
					float2 c = float2(coord.x + i * blurAmount, coord.y + j * blurAmount) ; 
					sum ++;
					total +=  tex2D(_GrabTexture, c); 
				}
			}
			return total / sum;
		}

//Fragment Shader
half4 frag (v2f i) : COLOR{

	return convolve(i.uv_MainTex);
}
ENDCG
}

}
FallBack "Diffuse"
}
