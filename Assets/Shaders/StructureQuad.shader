Shader "Custom/StructureQuad" {
	Properties {
		_TopTex("Top Texture", 2D) = "white" {}
		_SideTex("Side Texture", 2D) = "white" {}
		_BotTex ("Bottom Texture", 2D) = "white" {}
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

		struct Input {
			float3 worldNormal;
			float2 uv_TopTex;
			float2 uv_SideTex;
			float2 uv_BotTex;
		};

		sampler2D _BotTex;
		sampler2D _SideTex;
		sampler2D _TopTex;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			if (IN.worldNormal.y > 0.9) {
				// Top
				o.Albedo = tex2D(_TopTex, IN.uv_TopTex).rgb;
			} else if (IN.worldNormal.y < -0.9) {
				// Bot
				o.Albedo = tex2D(_BotTex, IN.uv_BotTex).rgb;
			} else {
				// Side
				o.Albedo = tex2D(_SideTex, IN.uv_SideTex).rgb;
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
