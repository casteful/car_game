Shader "Custom/Simple" 
{
	Properties
	{
		_MainTex("Texture (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		CGPROGRAM
		#pragma surface surf Standard 

		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _MainTex;

		void surf(Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
