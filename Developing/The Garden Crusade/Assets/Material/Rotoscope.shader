Shader "Custom/Rotoscope" 
{
	Properties 
	{
		_MainTex ("Diffuse", 2D) = "white" {}
		_RampTex ("Gradient Texture (Ramp)", 2D) = "white" {}
		_WrapMultiplier ("Wrap Multiplier", Range(0,1)) = 0.5
		_WrapAddition ("Wrap Addition", Range(0,1)) = 0.5
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Rotoscope
		
		sampler2D _MainTex;
		sampler2D _RampTex;
		float _WrapMultiplier;
		float _WrapAddition;
		
		half4 LightingRotoscope (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{
			half NdotV = dot (s.Normal, viewDir);
			half diff = NdotV * _WrapMultiplier + _WrapAddition;
			half3 ramp = tex2D(_RampTex, float2(diff, NdotV)).rgb;

			
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp;
			c.a = s.Alpha;
			return c;
		}

		struct Input 
		{
			float2 uv_MainTex;
		};
		
		

		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}