Shader "Custom/BlinkL" {
    Properties
     {
         _MainTex ("Texture", 2D) = "white" { }
         _VignettePercentage("Vignette Percentage", Float) = 0.3
     }
	SubShader {
	Pass
         {
             Name "Blink"
 
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag            
             #include "UnityCG.cginc"
             
             float _CurrentClosePercentage;
             float _FilterPercentage;
             sampler2D _MainTex;
             struct vertexOutput
             {
                 float4  pos : SV_POSITION;
                 float2  uv : TEXCOORD0;
             };
             
             vertexOutput vert (appdata_base vertexInput)
             {
                 vertexOutput output;
                 output.pos = mul(UNITY_MATRIX_MVP, vertexInput.vertex);
                 output.uv = vertexInput.texcoord;
                 return output;
             }
             
             half4 frag (vertexOutput vertexOut) : COLOR
             {
             	if(vertexOut.uv.y < (_CurrentClosePercentage / 2.0f)
             	   ||vertexOut.uv.y > 1 - (_CurrentClosePercentage / 2.0f))
             	{
             		return float4(0.0f, 0.0f, 0.0f, 1.0f);
             	}
             
   			  	float dist = 1 - sqrt(pow(vertexOut.uv.x - 1,2) + pow(vertexOut.uv.y - 0.5,2));
             				
				float4 filtered = tex2D(_MainTex, vertexOut.uv) * (1 - _FilterPercentage) 
             		   			  + float4(0.0f, 0.0f, 0.0f, 1.0f) * _FilterPercentage;
				return filtered * dist;
             }
             ENDCG
         }
	} 
	FallBack "Diffuse"
}
