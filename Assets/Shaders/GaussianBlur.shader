/// <summary>
/// Author Inge Becht
/// A Gaaussian blur shader 
/// </summary> 
 Shader "Custom/GaussianBlur"
 {
     Properties
     {
         _MainTex ("Texture", 2D) = "white" { }     

     }
 
     SubShader
     {
         Pass
         {
             Blend SrcAlpha OneMinusSrcAlpha 
             Name "HorizontalBlur"
 
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag            
             #include "UnityCG.cginc"
             
             sampler2D _MainTex;
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
             	float4 total = float4(0.0,0,0,1.0);
             	for(int i = - _StartIndex; i <=_StartIndex; i ++)
             	{
             		total += _GaussianKernel[abs(i) / 4][abs(i) % 4] * tex2D(_MainTex, float2(vertexOut.uv.x + i * _StepSize, vertexOut.uv.y));
             	}
             	return total;
             }
             ENDCG
         }
         
         GrabPass{}
  
         Pass
         {
             Blend SrcAlpha OneMinusSrcAlpha 
             Name "HorizontalBlur"
 
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag            
             #include "UnityCG.cginc"
             
             sampler2D _GrabTexture;
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
             	float4 total = float4(0.0,0,0,1.0);
              	for(int i = - _StartIndex; i <=_StartIndex; i ++)
             	{
             		total += _GaussianKernel[abs(i) / 4][abs(i) % 4] * tex2D(_GrabTexture, float2(vertexOut.uv.x, vertexOut.uv.y + i * _StepSize));
             	}
             	return total;
             }
             ENDCG
         }
         
         
     }
 
     Fallback "VertexLit"
 }