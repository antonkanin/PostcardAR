Shader "Custom/Postcard_alpha"
{
	Properties 
	{
		_VideoTex ("VideoTex", 2D) = "white" {}
		_AlphaTex ("AlphaTex", 2D) = "white" {}
		_AlphaRange("AlphaRange", Range(0, 1)) = 1.0
	}

	SubShader 
	{
		Tags { "Queue" = "Transparent" }

		CGPROGRAM
		#pragma surface surf Lambert alpha:fade

		sampler2D _VideoTex;
		sampler2D _AlphaTex;
		float _AlphaRange;

		struct Input 
		{
			float2 uv_VideoTex;
			float2 uv_AlphaTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = tex2D(_VideoTex, IN.uv_VideoTex).rgb;
			o.Alpha = tex2D(_AlphaTex, IN.uv_AlphaTex).a * _AlphaRange;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
