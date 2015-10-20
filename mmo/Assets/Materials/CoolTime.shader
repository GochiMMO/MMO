Shader "Custom/CoolTime" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_CoolTimePercentage ("Time Percentage", float) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambart

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float _CoolTimePercentage;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = tex2D(_MainTex, In.uv_MainTex).rgb;	// 色を取得し、設定する
		}


		ENDCG
	} 
	FallBack "Diffuse"
}
