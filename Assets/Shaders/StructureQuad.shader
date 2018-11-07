Shader "Custom/StructureQuad" {
	Properties {
		_TopTex0("Top Texture Cold", 2D) = "white" {}
		_SideTex0("Side Texture Cold", 2D) = "white" {}
		_BotTex0("Bottom Texture Cold", 2D) = "white" {}
		_TopTex1("Top Texture Warm", 2D) = "white" {}
		_SideTex1("Side Texture Warm", 2D) = "white" {}
		_BotTex1("Bottom Texture Warm", 2D) = "white" {}
		_TopTex2("Top Texture Hot", 2D) = "white" {}
		_SideTex2("Side Texture Hot", 2D) = "white" {}
		_BotTex2("Bottom Texture Hot", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float face_type;

		struct Input {
			float3 worldNormal;
			float2 uv_TopTex0;
			float2 uv_SideTex0;
			float2 uv_BotTex0;
			float2 uv_TopTex1;
			float2 uv_SideTex1;
			float2 uv_BotTex1;
			float2 uv_TopTex2;
			float2 uv_SideTex2;
			float2 uv_BotTex2;
		};

		sampler2D _BotTex0;
		sampler2D _SideTex0;
		sampler2D _TopTex0;
		sampler2D _BotTex1;
		sampler2D _SideTex1;
		sampler2D _TopTex1;
		sampler2D _BotTex2;
		sampler2D _SideTex2;
		sampler2D _TopTex2;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			switch (face_type) {
			case 0: // Cold
				if (IN.worldNormal.y > 0.9) {
					// Top
					o.Albedo = tex2D(_TopTex0, IN.uv_TopTex0).rgb;
				}
				else if (IN.worldNormal.y < -0.9) {
					// Bot
					o.Albedo = tex2D(_BotTex0, IN.uv_BotTex0).rgb;
				}
				else {
					// Side
					o.Albedo = tex2D(_SideTex0, IN.uv_SideTex0).rgb;
				}
				break;
			case 1: // Warm
				if (IN.worldNormal.y > 0.9) {
					// Top
					o.Albedo = tex2D(_TopTex0, IN.uv_TopTex0).rgb;
				}
				else if (IN.worldNormal.y < -0.9) {
					// Bot
					o.Albedo = tex2D(_BotTex0, IN.uv_BotTex0).rgb;
				}
				else {
					// Side
					o.Albedo = tex2D(_SideTex0, IN.uv_SideTex0).rgb;
				}
				break;
			case 2: // Hot
				if (IN.worldNormal.y > 0.9) {
					// Top
					o.Albedo = tex2D(_TopTex0, IN.uv_TopTex0).rgb;
				}
				else if (IN.worldNormal.y < -0.9) {
					// Bot
					o.Albedo = tex2D(_BotTex0, IN.uv_BotTex0).rgb;
				}
				else {
					// Side
					o.Albedo = tex2D(_SideTex0, IN.uv_SideTex0).rgb;
				}
				break;
			}

			o.Metallic = 0.0;
			o.Smoothness = 0.0;
		}
			

		/*half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}*/
		ENDCG
	}
	FallBack "Diffuse"
}
