Shader "Custom/bloodDrips" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BloodTex ("Blood texture", 2D) = "white" {}
	}
	SubShader {
		Pass
         {
             Blend SrcAlpha OneMinusSrcAlpha 
             Name "HorizontalBlur"
 
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag            
             #include "UnityCG.cginc"
             
             sampler2D _MainTex;
             sampler2D _BloodTex;
             int _StartIndex;
             float _StepSize;
             float4x4 _GaussianKernel;
             
             struct vertexOutput
             {
                 float4  pos : SV_POSITION;
                 float2  uv : TEXCOORD0;
             };
             
             float4 _MainTex_ST;
             
             vertexOutput vert (appdata_base vertexInput)
             {
                 vertexOutput output;
                 output.pos = mul(UNITY_MATRIX_MVP, vertexInput.vertex);
                 output.uv = vertexInput.texcoord;
                 return output;
             }
             
             half4 frag (vertexOutput vertexOut) : COLOR
             {
              	float4 c = tex2D(_BloodTex, float2(vertexOut.uv.x, vertexOut.uv.y));
             	if(c.x < 0.7)
             	{
             		return (c * 3 + tex2D(_MainTex, float2(vertexOut.uv.x, vertexOut.uv.y))) / 4.0;
             	}
             	else
             	{
             		return tex2D(_MainTex, float2(vertexOut.uv.x, vertexOut.uv.y));
             	}
            
             }
             ENDCG
         }
	} 
	FallBack "Diffuse"
}
